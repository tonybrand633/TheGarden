using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerHook : MonoBehaviour
{
    float a;
    public float distanceInput = 0;
    
    Vector2 mousePosition;
    Vector2 endPosition;
    Vector3 newPosition;
    DistanceJoint2D distanceJoint2D;
    LineRenderer lineRenderer;
    RaycastHit2D raycastHit2D;


    [Header("抓钩状态")]
    public bool isLaunching;
    public bool isAttached;
    public bool canIndicatorMove = true;
    public bool hasExtendMaxLength;
    public bool outMaxLength;
    public bool hasAdjustLength;


    [Header("抓钩参数设置")]
    public float ropeLength = 5f;
    public float ropeWidth = 3f;
    public float lauchSpeed = 5f;
    public float reciveSpeed = 10f;
    public float grapplSpeed = 5f;
    public float arrowRadius = 1f;
    public LayerMask ropeLayerMask;
    public GameObject arrowIndicator; // 鼠标朝向的指示器
   



    // Start is called before the first frame update
    void Start()
    {
        canIndicatorMove = true;
        distanceInput = ropeLength;
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();        
        distanceJoint2D.enabled = false;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        arrowIndicator.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        //发射抓钩
        if (Input.GetMouseButtonDown(0)&&!isAttached&&!isLaunching) 
        {
            canIndicatorMove = false;
            mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            endPosition = (Vector2)transform.position + direction * ropeLength;            
            lineRenderer.enabled = true;
            isLaunching = true;           
        }

        //如果没有抓住物体则回缩
        if (hasExtendMaxLength&&!isLaunching) 
        {
            lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), arrowIndicator.transform.position, Time.deltaTime * reciveSpeed));
            if (Vector3.Distance(lineRenderer.GetPosition(1),arrowIndicator.transform.position)<0.5f) 
            {
                ReleaseTheRope();
            }
        }

        //触碰目标后手动回缩节点
        if (Input.GetKeyDown(KeyCode.R)&&!isLaunching) 
        {
            hasExtendMaxLength = true;
        }

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //这里可以来控制最大长度                
        lineRenderer.SetPosition(0, arrowIndicator.transform.position);

        //判断线段长度的方法
        if (isAttached && !hasAdjustLength)
        {
            distanceInput = AdjustJointLength();
        }

        if (isAttached) 
        {
            Vector2 currentVelocity = this.GetComponent<Rigidbody2D>().velocity;
            Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position +currentVelocity, Color.red, 0.1f);
            GetDirectionWhenAttached();
        }

        //保持绳索的最大长度
        DetectLength();

        //线段延长的方法
        ExtendLine();

        //瞄准的图标实时位置更新
        if (canIndicatorMove)
        {
            UpdateArrowIndicator();
        }        
    }

    void ExtendLine() 
    {
        if (isLaunching && Vector3.Distance(lineRenderer.GetPosition(1), endPosition) > 0.1f)
        {
            a += Time.deltaTime * lauchSpeed;
            newPosition = Vector3.Lerp(arrowIndicator.transform.position, endPosition, a);
            float currentDistance = Vector3.Distance(lineRenderer.GetPosition(0), newPosition);
            
            // 检测路径上的碰撞物体
            raycastHit2D = Physics2D.Raycast(lineRenderer.GetPosition(0), newPosition - lineRenderer.GetPosition(0), currentDistance, ropeLayerMask);

            if (raycastHit2D.collider != null)
            {
                a = 1f;                
                endPosition = raycastHit2D.collider.gameObject.transform.position;
                GetTarget(raycastHit2D.collider.gameObject.transform.position);
            }
            lineRenderer.SetPosition(1, Vector3.Lerp(arrowIndicator.transform.position, endPosition, a));                               
        } 
        //在到达最大长度之后，设置distanceJoint2D.enable的属性
        else if (isLaunching && Vector3.Distance(lineRenderer.GetPosition(1), endPosition) < 0.1f) 
        {
            a = 0f;
            isLaunching = false;
            hasExtendMaxLength = true;
        }
    }

    void GetDirectionWhenAttached() 
    {        
        Vector3 referenceVec = transform.right;

        Vector3 direction = arrowIndicator.transform.position - raycastHit2D.collider.gameObject.transform.position;

        float dotProduct = Vector3.Dot(referenceVec, direction);
        if (dotProduct > 0)
        {
            //Debug.Log("箭头在物体的右边");
        }
        else if(dotProduct<0) 
        {
            //Debug.Log("箭头在物体的左边");
        }
    }

    void DetectLength() 
    {
        // 根据垂直输入调整距离
        distanceInput -= Input.GetAxis("Vertical") * grapplSpeed * Time.deltaTime;

        // 使用Mathf.Clamp确保distanceInput在[0, ropeLength]范围内
        distanceInput = Mathf.Clamp(distanceInput, 0f, ropeLength);

        // 设置距离关节的距离
        distanceJoint2D.distance = distanceInput;
        outMaxLength = false;

        if (distanceInput >= ropeLength)
        {
            outMaxLength = true;
            return;
        }

        
    }

    float AdjustJointLength()
    {
        float dis = Vector3.Distance(arrowIndicator.transform.position, raycastHit2D.collider.gameObject.transform.position);
        Debug.Log(dis);
        hasAdjustLength = true;
        return dis;
    }

    void GetTarget(Vector3 pos) 
    {
        //Vector3 localPos = transform.InverseTransformPoint(pos);
        distanceJoint2D.enabled = true;
        distanceJoint2D.connectedAnchor = pos;
        isAttached = true;
        isLaunching = false;
    }

    void ReleaseTheRope() 
    {
        a = 0f;
        distanceInput = ropeLength;
        distanceJoint2D.distance = ropeLength;
        distanceJoint2D.enabled = false;
        lineRenderer.enabled = false;        
        hasExtendMaxLength = false;
        canIndicatorMove = true;
        isLaunching = false;
        isAttached = false;
        hasAdjustLength = false;
    }    

    private void UpdateArrowIndicator()
    {
        // 获取鼠标位置并计算相对于主角的方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0f; // 确保朝向在2D平面上

        // 计算旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 获取固定圆心的位置
        Vector3 centerPosition = transform.position;

        // 计算旋转后的位置
        Vector3 rotatedPosition = centerPosition + Quaternion.Euler(0, 0, angle) * new Vector3(arrowRadius, 0, 0);

        //旋转自身
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // 设置arrowIndicator的位置
        arrowIndicator.transform.position = rotatedPosition;
        arrowIndicator.transform.rotation = rotation;   
    }
}
