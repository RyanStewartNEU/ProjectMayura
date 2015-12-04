using UnityEngine;
using System.Collections;

public class CameraFollowBehind : MonoBehaviour {

	public Transform target;
	public bool lockY;
	Vector2 dir;
	Vector2 targetDir;
	Vector3 prevPos;
	public float dist;
	public float speed;
	float currentY;
	public Vector2 yMinMax;
	public Transform lookTransform;
	Vector2 startDir,endDir;
	public float rotateRate;
	float autoRotating;
	Vector2 axis;
	Camera cam;
	// Use this for initialization
	void Start () 
	{
		prevPos = target.position;
		autoRotating = 1;		
		cam = transform.GetComponent<Camera>();
		transform.parent = null;
		currentY =  transform.position.y - target.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		getAxises();
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

        if(autoRotating >= 1 && Input.GetButtonDown("Auto Rotate"))
        {
        	autoRotating = 0;
        	Vector3 temp = (lookTransform.position - target.position).normalized;
        	endDir = new Vector2(temp.x,temp.z); 
        	startDir = new Vector2(target.position.x - transform.position.x,target.position.z - transform.position.z).normalized;
        }
 	}

	void FixedUpdate()
	{
		float yHeight =   transform.position.y - target.position.y;
		if(lockY)
		yHeight = currentY;
		else
		{
			if(yHeight < yMinMax.x)
			yHeight = yMinMax.x;
			else
			{
				if(yHeight > yMinMax.y)
				{
					yHeight = yMinMax.y;
				}
			}
		}
		

		if(autoRotating >= 1)
		{
			float change = Vector3.Distance(prevPos, target.position);
			targetDir = new Vector2(target.position.x  - prevPos.x,target.position.z  - prevPos.z).normalized;		
			dir = new Vector2(target.position.x - transform.position.x,target.position.z - transform.position.z).normalized;
			dir = Vector2.Lerp(dir,targetDir, 1f * change);
			dir = new Vector2(target.position.x - transform.position.x,target.position.z - transform.position.z).normalized;
			
			currentY += axis.y * 0.005f * speed;
			currentY = Mathf.Min(currentY, yMinMax.y);
			currentY = Mathf.Max(currentY, yMinMax.x);
			
		}
		else
		{
			dir = Vector2.Lerp(startDir, endDir, autoRotating);
			autoRotating+= rotateRate;
		}
		Vector3 newPos = new Vector3(target.position.x - (dir.x * dist), target.position.y + yHeight, target.position.z - (dir.y * dist));
		float distS = Vector3.Distance(target.position, newPos);
		newPos += (axis.x * transform.right * speed * 0.01f) + (axis.y * transform.up * speed * 0.005f); 
		transform.position =  target.position + ((newPos - target.position).normalized * distS);
		transform.position = new Vector3(transform.position.x, target.position.y + yHeight, transform.position.z);
		transform.LookAt(target);
	}

	void LateUpdate()
	{
		prevPos = target.position;
	}


	
}
