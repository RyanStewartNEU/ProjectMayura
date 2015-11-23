using UnityEngine;
using System.Collections;

public class CameraFollowBehind : MonoBehaviour {

	public Transform target;
	Vector2 dir;
	Vector2 targetDir;
	Vector3 prevPos;
	public float dist;
	public float speed;
	public Vector2 yMinMax;
	Vector2 axis;
	Camera cam;
	// Use this for initialization
	void Start () 
	{
		prevPos = target.position;		
		cam = transform.GetComponent<Camera>();
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
 	}

	void FixedUpdate()
	{
		float change = Vector3.Distance(prevPos, target.position);
		targetDir = new Vector2(target.position.x  - prevPos.x,target.position.z  - prevPos.z).normalized;
		dir = new Vector2(target.position.x - transform.position.x,target.position.z - transform.position.z).normalized;
		dir = Vector2.Lerp(dir,targetDir, 0.1f * change);
		float yHeight =   transform.position.y - target.position.y;

		if(yHeight < yMinMax.x)
		yHeight = yMinMax.x;
		else
		{
			if(yHeight > yMinMax.y)
			{
				yHeight = yMinMax.y;
			}
		}
		Vector3 newPos = new Vector3(target.position.x - (dir.x * dist), target.position.y + yHeight, target.position.z - (dir.y * dist));
		float distS = Vector3.Distance(target.position, newPos);
		newPos += axis.x * transform.right * speed + (axis.y * transform.up * speed * 0.5f); 
		transform.position =  target.position + ((newPos - target.position).normalized * distS);
		transform.LookAt(target);
	}

	void LateUpdate()
	{
		prevPos = target.position;
	}


	
}
