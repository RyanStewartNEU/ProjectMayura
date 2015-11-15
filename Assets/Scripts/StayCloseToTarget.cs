using UnityEngine;
using System.Collections;

public class StayCloseToTarget : MonoBehaviour {

	public Transform target;
	public Vector2 minDist; // the fis=rst value is the min XZ dist, the second is min y dist
	public Vector2 maxDist;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
			Vector2 XZdir = new Vector2(transform.position.x, transform.position.z) -  new Vector2(target.position.x, target.position.z);
			if(XZdir.magnitude < minDist.x)
				XZdir = XZdir.normalized * minDist.x;
			else 
			{
				if(XZdir.magnitude > maxDist.x)
					XZdir = XZdir.normalized * maxDist.x;		
			}
			
			float yDiff = transform.position.y - target.position.y;
			if(yDiff < minDist.y)
				yDiff = minDist.y;
			else
				{
					if(yDiff > maxDist.y)
					yDiff = maxDist.y;
				}

			transform.position = target.position + new Vector3(XZdir.x, yDiff, XZdir.y);
		}
	}
}
