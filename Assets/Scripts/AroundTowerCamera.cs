using UnityEngine;
using System.Collections;

public class AroundTowerCamera : MonoBehaviour {

	public Camera cam;
	public CameraFollowBehind otherCamMovement; 
	public float distance;
	public float ySpeed;
	float yHeight;
	Transform player;
	bool tracking;
	float yMove;
	// Use this for initialization
	void Start () 
	{
		tracking = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Mathf.Abs(Input.GetAxis("Vertical Camera")) > 0.3f)
        {
            yMove = Input.GetAxis("Vertical Camera");
        	yHeight += yMove * ySpeed / 100f;
        }

		if(tracking)
		{
			Vector2 XZdir = new Vector2(transform.position.x - player.transform.position.x,transform.position.z - player.transform.position.z).normalized * -1;
			cam.transform.position = new Vector3(transform.position.x + XZdir.x * distance, player.position.y + yHeight, transform.position.z + XZdir.y * distance);
			cam.transform.LookAt(player);
		}

		
	}

	 void OnTriggerEnter(Collider other) 
	 {
     	cam = other.GetComponentInChildren<FirstPersonDrifter>().cam;   
     	otherCamMovement = cam.GetComponent<CameraFollowBehind>();
     	otherCamMovement.enabled = false;
     	player = otherCamMovement.target;
     	yHeight = cam.transform.position.y - player.position.y;
     	tracking = true;
     }

     void OnTriggerExit(Collider other)
     {
     	tracking = false;
     	otherCamMovement.enabled = true;
     }
}
