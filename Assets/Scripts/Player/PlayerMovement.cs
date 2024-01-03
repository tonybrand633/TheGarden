using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rb;

    public PlayerData Data;

    #region StateParameters
    public bool isFacingRight { get; private set; }
    public bool isDashing { get; private set; }
    public bool isJumping;

    public bool isWallJumping;
    float wallJumpStartTime;

    public bool isSliding;

    public bool _isJumpCut;
    public bool _isJumpFalling;
    

    //Timer
    public float lastOnGroundTime;
    public float lastOnWallTime;
    public float lastOnLeftWallTime;
    public float lastOnRightWallTime;

    public float _lastOnWallDir;



    #endregion


    #region InputParameters
    public float lastPressJumpTime;

    #endregion

    #region CheckParameter
    //在Inspector面板中设置
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Transform _frontPoint;
    [SerializeField] private Transform _backPoint;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Vector2 wallCheckSize;

    #endregion

    #region Layer&Tags
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        SetGravity(Data.gravityScale);
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region Timer
        lastOnGroundTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;
        lastOnLeftWallTime -= Time.deltaTime;
        lastOnRightWallTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;
        #endregion


        #region InputHandle

        GetInput();
        #endregion

        #region CollisionCheck
        //Ground Check
        if (Physics2D.OverlapBox(checkPoint.position, groundCheckSize, 0, _groundLayer))
        {
            lastOnGroundTime = Data.coyoteTime;
        }

        //RightWall Check
        if ((Physics2D.OverlapBox(_frontPoint.position, wallCheckSize, 0, _groundLayer) && isFacingRight) || (Physics2D.OverlapBox(_backPoint.position, wallCheckSize, 0, _groundLayer) && !isFacingRight))
        {
            lastOnRightWallTime = Data.coyoteTime;
        }

        //LeftWallCheck
        if ((Physics2D.OverlapBox(_frontPoint.position, wallCheckSize, 0, _groundLayer) && !isFacingRight) || (Physics2D.OverlapBox(_backPoint.position, wallCheckSize, 0, _groundLayer) && isFacingRight))
        {
            lastOnLeftWallTime = Data.coyoteTime;
        }

        lastOnWallTime = Mathf.Max(lastOnLeftWallTime, lastOnRightWallTime);

        #endregion

        #region JumpCheck

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            _isJumpFalling = true;
        }

        if (isWallJumping && Time.time - wallJumpStartTime > Data.wallJumpTime)
        {
            isWallJumping = false;
        }

        if (lastOnGroundTime > 0 && !isJumping)
        {
            _isJumpFalling = false;
            _isJumpCut = false;
        }

        //用时间来限定只触发一次！
        if (CanJump() && lastPressJumpTime > 0)
        {
            isJumping = true;
            isWallJumping = false;
            _isJumpFalling = false;
            Jump();
        } else if (CanWallJump() && lastPressJumpTime > 0)
        {
            isWallJumping = true;
            isJumping = false;
            _isJumpFalling = false;
            _isJumpCut = false;
            wallJumpStartTime = Time.time;
            if (lastOnRightWallTime > 0)
            {
                _lastOnWallDir = -1;
            }
            else
            {
                _lastOnWallDir = 1;

            }
            WallJump(_lastOnWallDir);
        }

        #endregion

        #region SlideCheck
        if (Data.isASlider && CanSlide() && ((movementInput.x > 0 && lastOnRightWallTime > 0) || (movementInput.x < 0 && lastOnLeftWallTime > 0)))
        {
            isSliding = true;
            isJumping = false;
            isWallJumping = false;
        }
        else
        {
            isSliding = false;
        }
        #endregion


        #region Gravity
        if (isSliding)
        {
            SetGravity(0);
            //Debug.Log("Slider Apply");
        }
        else if (_isJumpCut)
        {
            SetGravity(Data.gravityScale * Data.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
            //Debug.Log("JumpCut Apply");
        }
        else if (rb.velocity.y < 0 && lastOnGroundTime < 0 && !isSliding)
        {
            SetGravity(Data.gravityScale * Data.fallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
            //Debug.Log("JumpFall Apply");
        }
        else if (isJumping || _isJumpFalling  || isWallJumping && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravity(Data.gravityScale * Data.jumpHangGravityMult);
            //Debug.Log("JumpHang Apply");
        }
        else
        {
            SetGravity(Data.gravityScale);
            //Debug.Log("On The Ground");
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (isWallJumping)
        {
            Run(Data.wallJumpLerp);
        }
        else 
        {
            Run(1);
        }
        #region Friction



        #endregion


        if (isSliding) 
        {
            Slide();
        }
    }

    #region InputCallBack
    public void OnJumpInput()
    {
        lastPressJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut()||CanWallJumpCut())
        {
            _isJumpCut = true;
        }
    }

    #endregion

    #region GetInput
    void GetInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        if (movementInput.x != 0)
        {
            CheckDirectionToFace(movementInput.x > 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUpInput();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //ActiveHook(hook.enabled);
        }
    }
    #endregion


    #region RunMethod
    private void Run(float lerpAmount)
    {
        //输入的速度-检测我们键盘输入的数值*定义的移动速度
        //有延迟的加速或者抵达当前速度
        float targetSpeed = movementInput.x * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        //rb.velocity.x是会随着后面AddForce改变的，在没有摩擦力的情况下，数值一直增大，直到等于targetSpeed
        //那么speedDif的数值会一直减小直到为0
        //如果再放开后，targetSpeed中的movementInput会等于0，这时rb.velocity.x会成为一个相反方向的力;
        float speedDif = targetSpeed - rb.velocity.x;

        //Log实时查看speedDif;
        //Debug.Log($"<color=green>  targetSpeed  :</color> {targetSpeed:F1} <color=green>  rb.velocity.x  :</color> {rb.velocity.x:F1} <color=green>  speedDif  :</color> {speedDif:F1}");

        //只要移动了，就定义accelRate,这里对应的是加速度-Acceleration：启动的加速度-Decceleration：减速的加速度
        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAcceleration : Data.runDecceleration;
        float accelRate;
        //继续增加一个功能在地上使用正常的加速减速-在空中使用另一个加速减速
        if (lastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.runAccelInAir : Data.runDeccelAmount * Data.runDeccelInAir;
        }

        //让在处于空中的主角移动更加灵敏？
        if (isJumping || _isJumpFalling && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }

        //velPower可以看作是改变speedDif基数的量，可以理解为对速度再做了一个控制
        //以1为标准,0<velPower<1,velPower是起到了一个缓解作用
        //如果velPower>1就是一个刺激增强的作用
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, Data.velPower) * Mathf.Sign(speedDif);

        //Log实时查看
        //Debug.Log($"<color=green>  speedDif  :</color> {speedDif:F1} <color=green>  movement  :</color> {movement:F1}");

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    #endregion

    #region JumpMethod
    private void Jump()
    {
        lastPressJumpTime = 0;
        lastOnGroundTime = 0;

        float force = Data.jumpForce;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void WallJump(float dir) 
    {
        lastPressJumpTime = 0;
        lastOnGroundTime = 0;
        lastOnRightWallTime = 0;
        lastOnLeftWallTime = 0;

        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir;
        //Debug.Log($"<color=green>  speedDif  :</color> {force.x:F1} <color=green>  movement  :</color> {force.y:F1}");
        if (Mathf.Sign(force.x) != Mathf.Sign(rb.velocity.x))
        {
            force.x -= rb.velocity.x;
        }

        if (rb.velocity.y < 0)
        {
            //这一段是让force.y抵消重力
            force.y -= rb.velocity.y;
        }

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void SetGravity(float value)
    {
        rb.gravityScale = value;
    }

    #endregion

    #region SlideMethod
    private void Slide() 
    {
        //slide方法作用于Y轴
        //限制住高度，在靠近墙的时候直接限制高度,完成一种吸附在墙面的效果
        rb.AddForce(-rb.velocity.y * Vector2.up, ForceMode2D.Impulse);

        float speedDif = Data.slideSpeed - rb.velocity.y;
        float movement = speedDif *Data.slideAccerate;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        rb.AddForce(movement * Vector2.up);
    }
    #endregion


    #region CheckMethod

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            Turn();
        }
    }

    public bool CanJump()
    {
        return lastOnGroundTime > 0 && !isJumping;
    }

    public bool CanJumpCut()
    {
        return isJumping||isWallJumping && rb.velocity.y > 0;
    }

    public bool CanWallJump() 
    {
        return lastOnWallTime > 0 && lastOnGroundTime < 0 && !isWallJumping && ((lastOnRightWallTime >0||_lastOnWallDir == 1)||(lastOnLeftWallTime>0||_lastOnWallDir == -1));
    }

    public bool CanWallJumpCut() 
    {
        return isWallJumping&&rb.velocity.y > 0;
    }

    public bool CanSlide() 
    {
        if (lastOnGroundTime <= 0 && lastOnWallTime > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(checkPoint.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontPoint.position, wallCheckSize);
        Gizmos.DrawWireCube(_backPoint.position, wallCheckSize);
    }
    #endregion
}
