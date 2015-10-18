using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {
    public Camera camera;
    public float acceleration, jumpSpeed;
    public float maxSpeed;
    CharacterController controller;
	Vector3 moveDirection;
    // Use this for initialization
	void Start () 
    {
	   controller = transform.GetComponent<CharacterController>();
	   moveDirection = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
                
        //Grounded Movement
        if(controller.isGrounded) 
        {   
            Vector3 addedVel = (Input.GetAxisRaw("Vertical Movement")  * transform.forward) + (Input.GetAxisRaw("Horizontal Movement") * transform.right);
            addedVel *= acceleration;
            moveDirection += addedVel;

            if(addedVel.magnitude == 0)
            {
                if(moveDirection.magnitude < 1)
                moveDirection = Vector3.zero;
                else
                moveDirection*=.9f;
            }

            if(moveDirection.magnitude > maxSpeed)
                moveDirection = moveDirection.normalized * maxSpeed;
            
            
            if(Input.GetAxisRaw("Jump") > 0)
            {
                moveDirection.y = jumpSpeed;
            }             
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        //if(controller.velocity.magnitude )
        
    }
}
