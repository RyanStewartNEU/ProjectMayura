using UnityEngine;
using System.Collections;

public class PeacockController : MonoBehaviour {
    public Camera camera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDirection = Vector3.zero;
        
        //Grounded Movement
        if(GetComponent<CharacterController>().isGrounded) {
            //GetComponent<CharacterController>().Move(new Vector3(0, 0, Input.GetAxis("Vertical Movement") * Time.deltaTime));
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical Movement")*6f);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= 8f;
            //Jump
            if (Input.GetButton("Jump")) {
                moveDirection.y = 8f;
            }
        }
        moveDirection.y -= 20f * Time.deltaTime;
        GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);

    }
}
