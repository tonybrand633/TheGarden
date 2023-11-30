using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float moveInput;

    Vector2 movementInput;
    Rigidbody2D rb;    

    public PlayerData Data;

    #region StateParameters

    
    public bool isFacingRight { get; private set; }

    public bool isDashing { get; private set; }
    public bool isJumping;

    //Timer
    public float lastOnGroundTime;


    public bool _isJumpCut;
    public bool _isJumpFalling;


    #endregion


    #region InputParameters
    public float lastPressJumpTime;

    #endregion

    #region CheckParameter
    //��Inspector���������
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Vector2 groundCheckSize;
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
        #endregion


        #region InputHandle

        GetInput();

        #endregion

        #region CollisionCheck
        if (Physics2D.OverlapBox(checkPoint.position,groundCheckSize,0,_groundLayer)) 
        {
            lastOnGroundTime = Data.coyoteTime;
        }

        #endregion

        #region JumpCheck

        if (isJumping&&rb.velocity.y<0) 
        {
            isJumping = false;
            _isJumpFalling = true;
        }

        if (lastOnGroundTime>0&&!isJumping) 
        {
            _isJumpFalling = false;
            _isJumpCut = false;
        }

        if (CanJump()&&lastPressJumpTime>0) 
        {
            isJumping = true;                   
            Jump();
        }

        #endregion

        #region Gravity

        if (_isJumpCut)
        {
            SetGravity(Data.gravityScale * Data.jumpCutGravityMult);
            Debug.Log("JumpCut Apply");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
        }
        else if (rb.velocity.y < 0)
        {
            SetGravity(Data.gravityScale * Data.fallGravityMult);
            Debug.Log("JumpFall Apply");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
        } else if (isJumping||_isJumpFalling&&Mathf.Abs(rb.velocity.y)<Data.jumpHangTimeThreshold) 
        {
            SetGravity(Data.gravityScale * Data.jumpHangGravityMult);
            Debug.Log("JumpHang Apply");
        }
        else
        {
            SetGravity(Data.gravityScale);
            Debug.Log("On The Ground");
        }       
        #endregion
    }

    private void FixedUpdate()
    {
        Run(1);

        #region Friction



        #endregion

       
    }

    #region InputCallBack
    public void OnJumpInput() 
    {
        lastPressJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput() 
    {
        if (CanJumpCut()) 
        {
            _isJumpCut = true;
        }
    }

    public void ActiveHook(bool isHook) 
    {
        
    }


    #endregion

    #region GetInput
    void GetInput()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

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
        //������ٶ�-������Ǽ����������ֵ*������ƶ��ٶ�
        //���ӳٵļ��ٻ��ߵִﵱǰ�ٶ�
        float targetSpeed = movementInput.x * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        //rb.velocity.x�ǻ����ź���AddForce�ı�ģ���û��Ħ����������£���ֵһֱ����ֱ������targetSpeed
        //��ôspeedDif����ֵ��һֱ��Сֱ��Ϊ0
        //����ٷſ���targetSpeed�е�movementInput�����0����ʱrb.velocity.x���Ϊһ���෴�������;
        float speedDif = targetSpeed - rb.velocity.x;

        //Logʵʱ�鿴speedDif;
        //Debug.Log($"<color=green>  targetSpeed  :</color> {targetSpeed:F1} <color=green>  rb.velocity.x  :</color> {rb.velocity.x:F1} <color=green>  speedDif  :</color> {speedDif:F1}");

        //ֻҪ�ƶ��ˣ��Ͷ���accelRate,�����Ӧ���Ǽ��ٶ�-Acceleration�������ļ��ٶ�-Decceleration�����ٵļ��ٶ�
        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAcceleration : Data.runDecceleration;
        float accelRate;
        //��������һ�������ڵ���ʹ�������ļ��ټ���-�ڿ���ʹ����һ�����ټ���
        if (lastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        }
        else 
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount*Data.runAccelInAir : Data.runDeccelAmount*Data.runDeccelInAir;
        }

        //���ڴ��ڿ��е������ƶ�����������
        if (isJumping||_isJumpFalling&&Mathf.Abs(rb.velocity.y)<Data.jumpHangTimeThreshold) 
        {
            accelRate *= Data.jumpHangAccelerationMult;
        }

        //velPower���Կ����Ǹı�speedDif�����������������Ϊ���ٶ�������һ������
        //��1Ϊ��׼,0<velPower<1,velPower������һ����������
        //���velPower>1����һ���̼���ǿ������
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, Data.velPower) * Mathf.Sign(speedDif);

        //Logʵʱ�鿴
        //Debug.Log($"<color=green>  speedDif  :</color> {speedDif:F1} <color=green>  movement  :</color> {movement:F1}");

        rb.AddForce(movement * Vector2.right,ForceMode2D.Force);
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
        rb.AddForce(Vector2.up*force, ForceMode2D.Impulse);
    }

    private void SetGravity(float value) 
    {
        rb.gravityScale = value;
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
        return isJumping && rb.velocity.y > 0;
    }

    #endregion

}
