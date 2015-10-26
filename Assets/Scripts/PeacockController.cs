using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {
    public Camera camera;
    public Transform model;
    public Transform lookTransform;
    public float acceleration, jumpSpeed;
    public float maxSpeed;
    public float holdTime;
    public float friction;
    public float deltaRotation;
    public float gravity;
    public float rate;
    Vector2 prevAxis;
    CharacterController controller;
    Vector3 lookDir;
    Vector3 moveDirection;
    Vector2 lastPosition;
    float jumpTime;
    bool holdingJump;
    // Use this for initialization    
    void Start () 
    {
       controller = transform.GetComponent<CharacterController>();
       moveDirection = Vector3.zero;
       lastPosition = transform.position;
       holdingJump = false;
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
        addedVel.y = 0;

        float speed = new Vector2(xAxis,yAxis).magnitude * rate;
        

        if(controller.isGrounded)
        {
            Vector2 axisChange = new Vector2(xAxis,yAxis) - prevAxis;
            if(Mathf.Abs(axisChange.x) > 1  || Mathf.Abs(axisChange.y) > 1 && prevAxis.magnitude > 0.25)
            {
                moveDirection += addedVel * speed * 4;     
            }
            else
            moveDirection += addedVel * speed;
        }
        else
        {
            addedVel *= acceleration; // apply accelration rate
            moveDirection += addedVel; // add to move
        } 

        
        Vector2 XZ  = new Vector2(moveDirection.x,moveDirection.z);
        float scale = new Vector2(xAxis,yAxis).magnitude / Mathf.Sqrt(2);
        scale *= scale;
        if(scale < 0.3f) scale = 0.3f;
        if(XZ.magnitude > maxSpeed * scale) //if we are moving faster than the top speed
        {
            XZ = XZ.normalized * maxSpeed * scale;
            Vector3 wantedMovement = new Vector3(XZ.x,moveDirection.y,XZ.y);
            moveDirection =  wantedMovement;//Vector3.Slerp(moveDirection,wantedMovement,0.5f);// scale to move at top speed
        }

        //Grounded Movement
        if(controller.isGrounded) 
        {   
            if(addedVel.magnitude == 0) // if we arent moving
            {
                if(moveDirection.magnitude < 1) // if we are close to stopped
                    moveDirection = Vector3.zero; // stop
                else // apply friction
                    moveDirection*= friction;
            }
           
            if(Input.GetButtonDown("Jump")) // if the jump button is pressed
            {
                moveDirection.y = jumpSpeed; // apply jump to move
                jumpTime = Time.time;
                holdingJump = true;
            }             
        }
        else
        {
            if(!Input.GetButton("Jump") ||  jumpTime < Time.time - holdTime) // if the jump button is being held
            {
                holdingJump = false;
            }

            if(holdingJump)
            {
                moveDirection.y = jumpSpeed; // move upwards slightly, for hold jumps 
            }
             moveDirection.y -= gravity;  
        }
        
       
      
        if(new Vector2(moveDirection.x,moveDirection.z).magnitude != 0 && controller.isGrounded)
        {
            
            
            Vector2 dir = new Vector2(moveDirection.x,moveDirection.z).normalized;
            lookDir = Vector3.RotateTowards(model.forward, new Vector3(dir.x,0,dir.y), deltaRotation * Time.deltaTime, 1F);
            lookTransform.position = model.position + lookDir;
            //float ang = Mathf.Atan2(dir.y, dir.x);
          // Debug.Log(dir + " " +  new Vector2(newDir.x, newDir.z));
         // Debug.Log(lookDir + " " + dir);
           model.LookAt(lookTransform);
        }
        
        prevAxis  = new Vector2(xAxis,yAxis);      
        controller.Move(moveDirection * Time.deltaTime);

        

    }

    public void LateUpdate()
    {
        lastPosition = new Vector2(transform.position.x,transform.position.z);
  
    }
}