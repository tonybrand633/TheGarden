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


    [Header("ץ��״̬")]
    public bool isLaunching;
    public bool isAttached;
    public bool canIndicatorMove = true;
    public bool hasExtendMaxLength;
    public bool outMaxLength;
    public bool hasAdjustLength;


    [Header("ץ����������")]
    public float ropeLength = 5f;
    public float ropeWidth = 3f;
    public float lauchSpeed = 5f;
    public float reciveSpeed = 10f;
    public float grapplSpeed = 5f;
    public float arrowRadius = 1f;
    public LayerMask ropeLayerMask;
    public GameObject arrowIndicator; // ��곯���ָʾ��
   



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

        //����ץ��
        if (Input.GetMouseButtonDown(0)&&!isAttached&&!isLaunching) 
        {
            canIndicatorMove = false;
            mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            endPosition = (Vector2)transform.position + direction * ropeLength;            
            lineRenderer.enabled = true;
            isLaunching = true;           
        }

        //���û��ץס���������
        if (hasExtendMaxLength&&!isLaunching) 
        {
            lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), arrowIndicator.transform.position, Time.deltaTime * reciveSpeed));
            if (Vector3.Distance(lineRenderer.GetPosition(1),arrowIndicator.transform.position)<0.5f) 
            {
                ReleaseTheRope();
            }
        }

        //����Ŀ����ֶ������ڵ�
        if (Input.GetKeyDown(KeyCode.R)&&!isLaunching) 
        {
            hasExtendMaxLength = true;
        }

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //���������������󳤶�                
        lineRenderer.SetPosition(0, arrowIndicator.transform.position);

        //�ж��߶γ��ȵķ���
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

        //������������󳤶�
        DetectLength();

        //�߶��ӳ��ķ���
        ExtendLine();

        //��׼��ͼ��ʵʱλ�ø���
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
            
            // ���·���ϵ���ײ����
            raycastHit2D = Physics2D.Raycast(lineRenderer.GetPosition(0), newPosition - lineRenderer.GetPosition(0), currentDistance, ropeLayerMask);

            if (raycastHit2D.collider != null)
            {
                a = 1f;                
                endPosition = raycastHit2D.collider.gameObject.transform.position;
                GetTarget(raycastHit2D.collider.gameObject.transform.position);
            }
            lineRenderer.SetPosition(1, Vector3.Lerp(arrowIndicator.transform.position, endPosition, a));                               
        } 
        //�ڵ�����󳤶�֮������distanceJoint2D.enable������
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
            //Debug.Log("��ͷ��������ұ�");
        }
        else if(dotProduct<0) 
        {
            //Debug.Log("��ͷ����������");
        }
    }

    void DetectLength() 
    {
        // ���ݴ�ֱ�����������
        distanceInput -= Input.GetAxis("Vertical") * grapplSpeed * Time.deltaTime;

        // ʹ��Mathf.Clampȷ��distanceInput��[0, ropeLength]��Χ��
        distanceInput = Mathf.Clamp(distanceInput, 0f, ropeLength);

        // ���þ���ؽڵľ���
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
        // ��ȡ���λ�ò�������������ǵķ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0f; // ȷ��������2Dƽ����

        // ������ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��ȡ�̶�Բ�ĵ�λ��
        Vector3 centerPosition = transform.position;

        // ������ת���λ��
        Vector3 rotatedPosition = centerPosition + Quaternion.Euler(0, 0, angle) * new Vector3(arrowRadius, 0, 0);

        //��ת����
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // ����arrowIndicator��λ��
        arrowIndicator.transform.position = rotatedPosition;
        arrowIndicator.transform.rotation = rotation;   
    }
}
