using UnityEngine;
using System.Collections;

public class DetectCloseByObject : MonoBehaviour {

	public ViewCamera viewCam;
	public Camera myCamera;
	float curDist;
	public Transform detected;
	MovableObject mo;
	FirstPersonDrifter movementScript;
	// Use this for initialization
	void Start () 
	{
		curDist = 99;
		movementScript = transform.GetComponent<FirstPersonDrifter>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(detected != null)
		{
			if(Input.GetButtonDown("Run"))
			{
				myCamera.enabled = false;
				mo.Unhighlight();
				mo.focused = true;
				mo.detector = this;
				viewCam.SetToUse(detected);
				movementScript.enabled = false;
				this.enabled = false;				
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		MovableObject closeByMovable = other.GetComponent<MovableObject>();
		if(closeByMovable != null)
		{
			if(detected != null && detected != other.transform)
			{
				float dist = Vector3.Distance(other.transform.position, transform.position);
				if(dist < curDist)
				{
					detected = other.transform;
					curDist = dist;
				}
			} 
			else
			{
				if(detected != other.transform)
				{
					curDist = Vector3.Distance(other.transform.position, transform.position);
					detected = other.transform;
				}
			}
			mo = detected.GetComponent<MovableObject>();		
			mo.Highlight();
		}	
	}

	void OnTriggerExit(Collider other)
	{
		if(other.transform == detected)
		{		
			mo.Unhighlight();
			detected = null;
		}
	}

	public void RestoreDefault()
	{
		movementScript.enabled = true;
		viewCam.RestoreDefault();
		myCamera.enabled = true;
		if(mo!=null) mo.Highlight();
	}
}
