using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {

	public static float AXES_THRESHOLD = 0.3F;
	public static string JUMP_BUTTON_STRING = "Jump";
	public static string VERTICAL_MOVEMENT_STRING = "Vertical Movement";
	public static string HORIZONTAL_MOVEMENT_STRING = "Horizontal Movement";
	public static int MOVEMENT_COEFFICIENT = 4;

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
    void Start () {
       controller = transform.GetComponent<CharacterController>();
       moveDirection = Vector3.zero;
       lastPosition = transform.position;
       holdingJump = false;
    }


    // Update is called once per frame
    void Update () {   
		float xAxis = getVerticalInput();
		float yAxis = getHorizontalInput();
		Vector3 addedVel = getXZMovement();
		Vector2 axisVector = new Vector2(xAxis, yAxis);
		float axisVectorMagnitude = axisVector.magnitude;
        float speed = axisVectorMagnitude * rate;

        if (controller.isGrounded) {
            Vector2 axisChange = axisVector - prevAxis;

            if (went(axisChange)) {
                moveDirection += MOVEMENT_COEFFICIENT * addedVel * speed;     
            } else {
            	moveDirection += addedVel * speed;
			}
        } else {
            addedVel *= acceleration; // apply acceleration rate
            moveDirection += addedVel; // add to move
        } 
        
        Vector2 XZ  = new Vector2(moveDirection.x, moveDirection.z);
		float scale = Mathf.Max(Mathf.Pow(axisVectorMagnitude / Mathf.Sqrt(2), 2), AXES_THRESHOLD);
        
		// if we are moving faster than the top speed
		if (XZ.magnitude > maxSpeed * scale) {
            XZ = XZ.normalized * maxSpeed * scale;
            Vector3 wantedMovement = new Vector3(XZ.x, moveDirection.y, XZ.y);
            moveDirection = wantedMovement; // scale to move at top speed
        }

        // Grounded Movement
        if (controller.isGrounded) {   
			doGroundedMovement(addedVel);            
        } else {
			doAirMovement(); 
        }

		Vector2 moveDirectionVector = new Vector2 (moveDirection.x, moveDirection.z);
      
        if (moveDirectionVector.magnitude > 0 && controller.isGrounded) {
            Vector2 dir = moveDirectionVector.normalized;
            lookDir = Vector3.RotateTowards(model.forward, new Vector3(dir.x, 0, dir.y), deltaRotation * Time.deltaTime, 1F);
            lookTransform.position = model.position + lookDir;
           	model.LookAt(lookTransform);
        }
        
        prevAxis = axisVector;      
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void LateUpdate()
    {
        lastPosition = new Vector2(transform.position.x,transform.position.z);
  
    }

	private float getVerticalInput() {
		float xInput = Input.GetAxis(VERTICAL_MOVEMENT_STRING);
		
		if (Mathf.Abs(xInput) > AXES_THRESHOLD) {
			return xInput;
		}

		return 0;
	}

	private float getHorizontalInput() {
		float yInput = Input.GetAxis(HORIZONTAL_MOVEMENT_STRING);
		
		if (Mathf.Abs(yInput) > AXES_THRESHOLD) {
			return yInput;
		}
		
		return 0;
	}

	private Vector3 getXZMovement(float xAxis, float yAxis) {
		Vector3 yAx = (yAxis * camera.transform.right);
		yAx.y = 0;
		Vector3 addedVel = (xAxis * camera.transform.forward) + yAx;  
		addedVel.y = 0;

		return addedVel;
	}

	// TODO: rename this plz
	private void went(Vector2 axisChange) {
		return Mathf.Abs(axisChange.x) > 1  
			|| Mathf.Abs(axisChange.y) > 1 
				&& prevAxis.magnitude > 0.25;
	}

	private float getMagnitude(float x, float y) {
		return new Vector2(x, y).magnitude;
	}

	private void doGroundedMovement(Vector3 addedVel) {
		// if we arent moving
		if (addedVel.magnitude == 0) {
			
			// if we are close to stopped
			if (moveDirection.magnitude < 1) {
				moveDirection = Vector3.zero; // stop
			} else {
				moveDirection *= friction; // apply friction
			}
		}
		
		// if the jump button is pressed
		if (Input.GetButtonDown (JUMP_BUTTON_STRING)) {
			moveDirection.y = jumpSpeed; // apply jump to move
			jumpTime = Time.time;
			holdingJump = true;
		} 
	}

	private void doAirMovement() {
		// if the jump button is being held
		if (!Input.GetButton(JUMP_BUTTON_STRING) ||  jumpTime < Time.time - holdTime) {
			holdingJump = false;
		}
		
		if (holdingJump) {
			moveDirection.y = jumpSpeed; // move upwards slightly, for hold jumps 
		}
		
		moveDirection.y -= gravity; 
	}
}