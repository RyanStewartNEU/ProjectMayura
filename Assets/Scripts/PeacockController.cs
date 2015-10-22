using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {
    public Camera camera;
    public Transform model;
    public float acceleration, jumpSpeed;
    public float maxSpeed;
    public float holdSpeed;
    CharacterController controller;
    Vector3 moveDirection;
    Vector2 lastPosition;
    // Use this for initialization    
    void Start () 
    {
       controller = transform.GetComponent<CharacterController>();
       moveDirection = Vector3.zero;
       lastPosition = transform.position;
    }


    // Update is called once per frame
    void Update () 
    {
                
        float xAxis = 0;
        float yAxis = 0;
        if(Mathf.Abs(Input.GetAxis("Vertical Movement")) > 0.3f)
        {
            xAxis = Input.GetAxis("Vertical Movement");
        }

        if(Mathf.Abs(Input.GetAxis("Horizontal Movement")) > 0.3f)
        {
            yAxis = Input.GetAxis("Horizontal Movement");
        }
        // get current XZ movement
        Vector3 yAx = (yAxis * camera.transform.right);
        yAx.y = 0;
        Vector3 addedVel = (xAxis  * camera.transform.forward) + yAx;
        addedVel *= acceleration; // apply accelration rate
        addedVel.y = 0;
        moveDirection += addedVel; // add to move
        

        Vector2 XZ  = new Vector2(moveDirection.x,moveDirection.z);
        if(XZ.magnitude > maxSpeed) //if we are moving faster than the top speed
        {
            XZ = XZ.normalized * maxSpeed;
            moveDirection = new Vector3(XZ.x,moveDirection.y,XZ.y); // scale to move at top speed
        }
        //Grounded Movement
        if(controller.isGrounded) 
        {   

            if(addedVel.magnitude == 0) // if we arent moving
            {
                if(moveDirection.magnitude < 1) // if we are close to stopped
                    moveDirection = Vector3.zero; // stop
                else // apply friction
                    moveDirection*=.7f;
            }
           
            if(Input.GetButtonDown("Jump")) // if the jump button is pressed
            {
                moveDirection.y = jumpSpeed; // apply jump to move
            }             
        }
        else
        {
            if(Input.GetButton("Jump") && moveDirection.y > 0) // if the jump button is being held
            {
                moveDirection.y += holdSpeed / 50f; // move upwards slightly, for hold jumps
            }   
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime;
      
        controller.Move(moveDirection * Time.deltaTime);

        if(lastPosition != new Vector2(transform.position.x,transform.position.z))
        {
            Vector2 dir = new Vector2(transform.position.x - lastPosition.x, transform.position.z - lastPosition.y);
            float ang = Mathf.Atan2(dir.y, dir.x);
            model.eulerAngles = new Vector3(model.eulerAngles.x,Mathf.Rad2Deg * -ang + 90, model.eulerAngles.z);
        }
    }

    public void LateUpdate()
    {
        lastPosition = new Vector2(transform.position.x,transform.position.z);
    }
}