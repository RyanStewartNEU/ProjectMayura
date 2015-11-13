// original by Eric Haines (Eric5h5)
// adapted by @torahhorse
// http://wiki.unity3d.com/index.php/FPSWalkerEnhanced

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FirstPersonDrifter: MonoBehaviour
{
    public Transform lastCheckpoint;
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public bool canDoubleJump;
    public Camera cam;
    public float maxMovementPerFrame;
    public float maxMovement;
    public float maxDownwardSpeed;
    public float doubleJumpSpeed;
    // If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
    private bool limitDiagonalSpeed = true;
 
    public bool enableRunning = false;
 
    public float jumpSpeed = 4.0f;
    public float gravity = 10.0f;
 
    // Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
    private float fallingDamageThreshold = 10.0f;
 
    // If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
    public bool slideWhenOverSlopeLimit = false;
 
    // If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
    public bool slideOnTaggedObjects = false;
 
    public float slideSpeed = 5.0f;
 
    // If checked, then the player can change direction while in the air
    public bool airControl = true;
 
    // Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
    public float antiBumpFactor = .75f;
 
    // Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
    public int antiBunnyHopFactor = 1;
    public float holdTime;
    public float jumpHold;
    public Transform model;
    public Transform lookTransform;
    public float deltaRotation;
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;
    private CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
    private float startJumpTime;
    private bool holdingJump;
    private Vector3 actualMovement;
    private bool doubleJumped;
    private bool jumpingUp,jumpingDown;
    private float jumpTimeUp,jumpTimeDown;

    int jumpCheckWait;
    Vector3 lookDir;
    Animator anim;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;
        jumpTimer = antiBunnyHopFactor;
        anim = transform.GetComponentInChildren<Animator>();
        actualMovement = new Vector3(0,0,0);
        Debug.Log(anim.hasRootMotion);
        if(cam == null)
        cam = Camera.main;
    }
 
    void FixedUpdate() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed)? .7071f : 1.0f;
        
        /*if(anim.GetBool("jump"))
        {
             anim.SetBool("jump", false);
        }*/
       
        if (grounded) // if you are on the ground 
        {
            bool sliding = false;
            // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
            // because that interferes with step climbing amongst other annoyances
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance)) {
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }
            // However, just raycasting straight down from the center can fail when on steep slopes
            // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
            else {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }
 
            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (falling) {
                falling = false;
                if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                    FallingDamageAlert (fallStartLevel - myTransform.position.y);
            }
 
            if( enableRunning )
            {
            	speed = Input.GetButton("Run")? runSpeed : walkSpeed;
            }
 
            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
            else 
            {               
                moveDirection = new Vector3(inputX * inputModifyFactor * Mathf.Abs(inputX) , -antiBumpFactor, inputY * inputModifyFactor * Mathf.Abs(inputY) );
                moveDirection = translatedToCam(moveDirection);
                playerControl = true;
            }
 
            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
            if (!Input.GetButton("Jump"))
            {
                jumpTimer++;
                                   
            }
            else if (jumpTimer >= antiBunnyHopFactor) 
            {
                
                jumpTimeUp = 0;
                jumpTimeDown = 0;
                jumpingUp = true;
                grounded = false;
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
                startJumpTime = Time.time;
                doubleJumped = false;
                holdingJump = true;
                jumpCheckWait = 3;
                anim.SetBool("jump", true);

            }
        }
        else {

            // if you are holding the jump button, move upwards for a factor of a second
            if(holdingJump && Input.GetButton("Jump") && Time.time - startJumpTime < holdTime)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = 0;
                holdingJump = false;
            }
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!falling) {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }

            if(jumpingDown)
            {
                jumpTimeDown++;
            }
            if(jumpingUp)
            {
                jumpTimeUp++;
                if(actualMovement.y < 0)
                {
                    jumpingDown = true;
                    jumpingUp = false;
                }
            }
            // If air control is allowed, check movement but don't touch the y component
            if (airControl && playerControl) 
            {
                moveDirection.x = inputX  * inputModifyFactor;
                moveDirection.z = inputY  * inputModifyFactor;
                float storeY = moveDirection.y;
                moveDirection = translatedToCam(moveDirection) / 1.5f;
                moveDirection.y = storeY;
            }
        }
         
        Vector2 moveDirectionXY = new Vector2(moveDirection.x,moveDirection.z);
        Vector2 actualMovementXY = new Vector2(actualMovement.x,actualMovement.z);

        Vector2 mdir = moveDirectionXY - actualMovementXY;

        if(mdir.magnitude > maxMovementPerFrame)
        {
            mdir = mdir.normalized * maxMovementPerFrame;
        }

        actualMovement +=new Vector3(mdir.x,moveDirection.y, mdir.y);
                
        actualMovementXY = new Vector2(actualMovement.x,actualMovement.z);
        if(actualMovementXY.magnitude > maxMovement)
        {
            actualMovementXY = actualMovementXY.normalized * maxMovement; 
            actualMovement = new Vector3(actualMovementXY.x,actualMovement.y,actualMovementXY.y);
        }
        
        if(grounded)
        {
            actualMovement.y = 0;
            if(jumpingDown)
            {
                jumpingDown = false;
            }
            doubleJumped = true;
        }
        else
        {
            if(!holdingJump)
            {
                if(canDoubleJump && !doubleJumped && Input.GetButtonDown("Jump") && actualMovement.y <=0)
                {
                    actualMovement = moveDirection;
                    actualMovement.y = doubleJumpSpeed;
                    doubleJumped = true;
                }
                else
                actualMovement.y -= gravity;
            }
            else
            actualMovement.y = moveDirection.y;
        }
        
        
        if(actualMovement.y < -maxDownwardSpeed)
        {
            actualMovement.y = -maxDownwardSpeed;
        }
        float tempWalking = actualMovement.magnitude * Time.deltaTime;
        anim.SetFloat("walking" , tempWalking);
        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(actualMovement * Time.deltaTime) & CollisionFlags.Below) != 0;
        float turnSpeed = new Vector2(moveDirection.x,moveDirection.z).magnitude;
        if(turnSpeed != 0)
        {          
            Vector2 dir = new Vector2(moveDirection.x,moveDirection.z).normalized;
            lookDir = Vector3.RotateTowards(model.forward, new Vector3(dir.x,0,dir.y), deltaRotation * Time.deltaTime * (turnSpeed / speed) , 1F);
            lookTransform.position = model.position + lookDir;
            model.LookAt(lookTransform);
        }
    }
    

    void LateUpdate()
    {

    }
    // Store point that we're in contact with for use in FixedUpdate if needed
    void OnControllerColliderHit (ControllerColliderHit hit) {
        if(hit.transform.tag == "FalloutCatcher")
        {
            if(lastCheckpoint != null)
            {
                Vector3 cameraOffset = this.transform.position - cam.transform.position;
                this.transform.position = lastCheckpoint.position;
                this.transform.rotation = lastCheckpoint.rotation;
                cam.transform.position = lastCheckpoint.position + cameraOffset;
                cam.transform.rotation = lastCheckpoint.rotation;
            }
        }
        contactPoint = hit.point;
    }
 
    // If falling damage occured, this is the place to do something about it. You can make the player
    // have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
    void FallingDamageAlert (float fallDistance)
    {
        //print ("Ouch! Fell " + fallDistance + " units!");   
    }



    Vector3 translatedToCam(Vector3 move)
    {
            move = myTransform.TransformDirection(move) * speed;
            Vector3 zAx = (move.x * cam.transform.right);
            zAx.y = 0;
            move = (move.z  * cam.transform.forward) + zAx;  
            move.y = 0;
            return move;
    }
}