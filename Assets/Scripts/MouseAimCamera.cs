using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour {
    public GameObject target;
    public float rotateSpeed = 5;
    float vertical;
    float horizontal;

    Vector3 offset;
    Vector2 axis;
     
    void Start() 
    {
        offset = target.transform.position - transform.position;
        vertical = transform.eulerAngles.x;
        horizontal = transform.eulerAngles.y;
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
    }

    void LateUpdate() 
    {
        if(vertical + axis.y * rotateSpeed < 20 && vertical + axis.y * rotateSpeed > -30)
        vertical += axis.y * rotateSpeed;
        
        horizontal += axis.x * rotateSpeed;
        //target.transform.Rotate(0, , 0);
       
        Quaternion rotation = Quaternion.Euler(vertical,horizontal, 0);
        transform.position = target.transform.position - (rotation * offset); 
        transform.LookAt(target.transform);
           
    }
}