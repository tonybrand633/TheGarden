using UnityEngine;


[CreateAssetMenu(menuName = ("PlayerData"),fileName = ("PlayerData_"))]

public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength;
    [HideInInspector] public float gravityScale;

    [Space(5)]
    public float fallGravityMult;
    public float maxFallSpeed;

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed;
    public float runAcceleration;
    [HideInInspector] public float runAccelAmount;
    public float runDecceleration;
    [HideInInspector] public float runDeccelAmount;
    [Range(0, 1f)] public float runAccelInAir;
    [Range(0, 1f)] public float runDeccelInAir;
    public float velPower;

    [Space(20)]
    [Header("Jump")]
    public float jumpHeight;
    public float jumpTimeToApex;
    [Range(0f, 1.5f)] public float wallJumpTime;
    public float wallJumpLerp;
    public Vector2 wallJumpForce;
    [HideInInspector]public float jumpForce;
    public float canJumpAlongFallTime;

    [Space(10)]
    [Header("Both Jumps")]
    public float jumpCutGravityMult;
    [Range(0,1f)]public float jumpHangGravityMult;
    public float jumpHangTimeThreshold;

    [Space(2f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Space(10f)]
    [Header("Slider")]
    public bool isASlider;
    public float slideAccerate;
    public float slideSpeed;


    [Space(20)]
    [Header("Assistant")]
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;
    [Range(0.01f, 0.5f)] public float coyoteTime;

    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }
}