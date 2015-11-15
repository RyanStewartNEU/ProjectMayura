using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour {
    public GameObject target;
    public Transform stayCloseTransform;
    public Transform lookAtTarget;
    public float rotateSpeed = 5;
    public float movementDistance;
    public float autoRotateSpeed;
    public float verticleReset;
    float vertical;
    float horizontal;
    bool autoRotating;
    bool connected;
    Vector3 startRot;
    public Vector3 offset, minOffset, maxOffset;
    Vector2 axis;
    float tDist;
    void Start() 
    {
        offset = target.transform.position - transform.position;
        vertical = transform.eulerAngles.x;
        horizontal = transform.eulerAngles.y;
        connected = true;
    }
    
   /* void CheckCameraCollision()
    {
        Ray ray=new Ray(target.position, transform.position - target.position);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, (transform.position - target.position).magnitude))
        {
        
        if(hit.distance > offset)
            transform.position=ray.GetPoint(hit.distance - offset);
        else
            transform.position=target.position;
        }
    }*/
    public float XZDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector2.Distance(new Vector2(pos1.x,pos1.z),new Vector2(pos2.x,pos2.z));
    }
    void Update()
    {
        axis = Vector2.zero;
        if(Mathf.Abs(Input.GetAxis("Horizontal Camera")) > 0.3f)
        {
            axis.x = Input.GetAxis("Horizontal Camera");
        }

        if(Mathf.Abs(Input.GetAxis("Vertical Camera")) > 0.3f)
        {
            axis.y = Input.GetAxis("Vertical Camera");
        }
        
        if(Input.GetKeyDown(KeyCode.JoystickButton8)  && !autoRotating)
        {            
            autoRotating = true;
            startRot = transform.localEulerAngles;
            tDist = 0;
        }
    }
    public Vector3 getFilteredRotation()
    {
            Vector3 rotateTo = lookAtTarget.localEulerAngles;
            Vector3 diff = startRot - rotateTo;
            rotateTo.x = verticleReset; 
            if(diff.x > 180)
            {
                rotateTo.x+=360;
            }
            if(diff.y > 180)
            {
                rotateTo.y+=360;
            }
            if(diff.y < -180)
            {
                rotateTo.y-=360;
            }

            if(diff.x < -180)
            {
                rotateTo.x-=360;
            }
            return rotateTo;

    }
    void FixedUpdate() 
    {
        
        if(!autoRotating)
        {
            // limit y movement so you cant click through ground / flip camera
            if(vertical + axis.y * rotateSpeed < 30 && vertical + axis.y * rotateSpeed > -50)
            vertical += axis.y * rotateSpeed;
            
            horizontal += axis.x * rotateSpeed;
        }
        else
        {
            if(tDist < 1)
            {
                 Vector3 newRot = Vector3.Lerp(startRot, getFilteredRotation(), tDist);
                 vertical = newRot.x;
                 horizontal = newRot.y;
                 //float diff = (startRot - getFilteredRotation()).magnitude; 
                 tDist += (autoRotateSpeed * Time.deltaTime * 10);  
                                
            }
            else
            {
                target.transform.position = lookAtTarget.position;
                autoRotating = false;
            }
        }
        //target.transform.Rotate(0, , 0);
        if(vertical > 180)
        {
            vertical -=360;
        }
        

        Quaternion rotation = Quaternion.Euler(vertical,horizontal, 0);
        //Debug.Log(rotation * offset - (target.transform.position - transform.position));
        transform.position = target.transform.position - (rotation * offset); 
        
        transform.LookAt(lookAtTarget);
           
    }

    public void GetOffset(Quaternion rotation)
    {
        
        //offset = (target.transform.position - transform.position);
        offset = Quaternion.Inverse(rotation)  * ((transform.position  - target.transform.position) * -1) ;
        if(offset.x < minOffset.x)
            offset.x = minOffset.x;
        if(offset.x > maxOffset.x)
            offset.x = maxOffset.x;

        if(offset.y < minOffset.y)
            offset.y = minOffset.y;
        if(offset.y > maxOffset.y)
            offset.y = maxOffset.y;

        if(offset.z < minOffset.z)
            offset.z = minOffset.z;
        if(offset.z > maxOffset.z)
            offset.z = maxOffset.z;
    }
}