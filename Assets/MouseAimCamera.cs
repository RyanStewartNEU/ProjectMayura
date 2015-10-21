using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour {
    public GameObject target;
    public float rotateSpeed = 5;
    float vertical;
    Vector3 offset;
     
    void Start() 
    {
        offset = target.transform.position - transform.position;
        vertical = transform.eulerAngles.x;
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

    void LateUpdate() 
    {
        float horizontal = Input.GetAxis("Horizontal Camera") * rotateSpeed;
        vertical += Input.GetAxis("Vertical Camera") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);
       
        Quaternion rotation = Quaternion.Euler(vertical, target.transform.eulerAngles.y, 0);
        transform.position = target.transform.position - (rotation * offset);
         
        transform.LookAt(target.transform);
    }
}