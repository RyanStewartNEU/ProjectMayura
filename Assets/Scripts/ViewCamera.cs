using UnityEngine;
using System.Collections;

public class ViewCamera : MonoBehaviour {

	public Transform target;
	public float distance;
	public float rotationSpeed;
	Camera cam;
	Vector2 axis;
	// Use this for initialization
	void Start () 
	{
		cam = transform.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(cam.enabled && target != null)
		{
			axis = Vector2.zero;
	        if(Mathf.Abs(Input.GetAxis("Horizontal Camera")) > 0.3f)
	        {
	            axis.x = Input.GetAxis("Horizontal Camera");
	        }

	        if(Mathf.Abs(Input.GetAxis("Vertical Camera")) > 0.3f)
	        {
	            axis.y = Input.GetAxis("Vertical Camera");
	        }
	        
	        Vector3 newPos = transform.position + (transform.right * -axis.x + transform.up * axis.y) * Time.deltaTime * rotationSpeed;
	        Vector3 dir = (newPos - target.position).normalized;
	        transform.position = target.position + dir * distance;
			transform.LookAt(target);
		}
	}

	public void SetToUse(Transform newTarget, float dist = 20)
	{
		target = newTarget;
		cam.enabled = true;
		distance = dist;
	}

	public void RestoreDefault()
	{
		cam.enabled = false;	
	}
}
