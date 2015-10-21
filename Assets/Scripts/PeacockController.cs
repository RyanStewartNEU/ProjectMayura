using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {
    public Camera camera;
    public float acceleration, jumpSpeed;
    public float maxSpeed;
    public float holdSpeed;
    CharacterController controller;
	Vector3 moveDirection;
    // Use this for initialization
	void Start () 
    {
	   controller = transform.GetComponent<CharacterController>();
	   moveDirection = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () 
    {
                
        // get current XZ movement
        Vector3 addedVel = (Input.GetAxisRaw("Vertical Movement")  * transform.forward) + (Input.GetAxisRaw("Horizontal Movement") * transform.right);
        addedVel *= acceleration; // apply accelration rate
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
                moveDirection*=.8f;
            }

                  
            if(Input.GetAxisRaw("Jump") > 0) // if the jump button is pressed
            {
                moveDirection.y = jumpSpeed; // apply jump to move
            }             
        }
        else
        {
            if(Input.GetAxisRaw("Jump") > 0 && moveDirection.y > 0) // if the jump button is being held
            {
                moveDirection.y += holdSpeed; // move upwards slightly, for hold jumps
            }   
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
