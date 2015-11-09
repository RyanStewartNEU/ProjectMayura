//SmoothLookAt.cs
//Written by Jake Bayer
//Written and uploaded November 18, 2012
//This is a modified C# version of the SmoothLookAt JS script.  Use it the same way as the Javascript version.
 
using UnityEngine;
using System.Collections;
 
///<summary>
///Looks at a target
///</summary>
[AddComponentMenu("Camera-Control/Smooth Look At CS")]
public class SmoothLookAt : MonoBehaviour {
	public Transform target;		//an Object to lock on to
	public float damping = 6.0f;	//to control the rotation 
	public bool smooth = true;
	public float minDistance = 5.0f;	//How far the target is from the camera
	public float maxDistance = 10.0f;
	public string property = "";
 	public float rotateSpeed;
	private Color color;
	private float alpha = 1.0f;
	private Transform _myTransform;
 	private Vector2 axis, movement;
	void Awake() {
		_myTransform = transform;
	}
 
	// Use this for initialization
	void Start () {
//		if(renderer.material.HasProperty(property)) {
//			color = renderer.material.GetColor(property);
//		}
//		else {
//			property = "";
//		}
//		if(rigidbody) {
//			rigidbody.freezeRotation = true;
//		}
 
	}
 	public void getAxises()
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
 	}
	// Update is called once per frame
	void Update () 
	{
 		movement = Vector2.zero;
 		getAxises();
		float yAng = (axis.y * rotateSpeed) + _myTransform.eulerAngles.x;
	    movement.y = axis.y * rotateSpeed;
            
        movement.x = axis.x * rotateSpeed;
	}
	
	public Vector2 Vector2FromAngle(float a)
	{
	    //a *= Mathf.Deg2Rad;
	    return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
	}

	void LateUpdate() 	
	{
		if(target) 
		{
			if(smooth) 
			{
				float distance = Vector3.Distance(target.position, _myTransform.position);
				/*
				Vector2 dirYZ = new Vector2(target.position.y - _myTransform.position.y, target.position.z - _myTransform.position.z).normalized;
				float angY = Mathf.Atan2(dirYZ.x,dirYZ.y);

				angY +=movement.y * Time.deltaTime;
				Vector2 temp = Vector2FromAngle(angY);
				temp = new Vector2(temp.y,temp.x);
				Debug.Log(dirYZ + " " + temp); 
				*/
				//dirYZ = temp;

				Vector3 dir = ((_myTransform.position - target.position)).normalized;
				
				dir += transform.right * (movement.x * Time.deltaTime);
				
				dir += transform.up * (movement.y * Time.deltaTime * .3f);
				//Quaternion fromTo = new Quaternion(0,0,0,0);
				//fromTo.SetLookRotation(-dir);
				//Vector3 eulers = fromTo.eulerAngles;
				//Quaternion   newRot =  Quaternion.Euler(eulers.x,eulers.y, eulers.z);
				//float ang;
				
				//Vector3 newDir = newRot * dir ;
				
				
				
				float dist = 0;
				if(distance > maxDistance)
				{
					dist = maxDistance;
					
				}
				else
				{
					if(distance < minDistance)
					{ dist = minDistance; }
					else
					dist = distance;
				}
	
				_myTransform.position = new Vector3(target.position.x + dir.x * dist, 
														target.position.y + dir.y * dist, 
														target.position.z + dir.z * dist);
				//Look at and dampen the rotation
				Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position);
				_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
				

			}
			
		}
	}
}