using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {

	public bool focused;
	public Camera cam;
	Material mat;
	float storedAlpha;
	public float force;
	public Transform forcePoint;
	public DetectCloseByObject detector;
	public Vector3 storedPosition;
	public Quaternion storedRotation;
	Rigidbody rb;
	Vector2 axis;
	// Use this for initialization
	void Start () 
	{
		mat = transform.GetComponent<Renderer>().material;
		storedAlpha = mat.color.a;	
		rb = transform.GetComponent<Rigidbody>();
		storedPosition = transform.position;
		storedRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(focused)
		{
			if(Input.GetButtonDown("Run"))
			{
				detector.enabled = true;
				detector.RestoreDefault();
			}

			if(Input.GetButtonDown("Jump"))
			{
				transform.position = storedPosition;
				transform.rotation = storedRotation;
				rb.velocity = Vector3.zero;
			}
			axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector3 camT = translatedToCam(new Vector3(axis.x,0,axis.y));
			rb.AddForceAtPosition(camT, forcePoint.position);
		
		}
	}

	Vector3 translatedToCam(Vector3 move)
    {
            move = transform.TransformDirection(move) * force;
            Vector3 zAx = (move.x * cam.transform.right);
            zAx.y = 0;
            move = (move.z  * cam.transform.forward) + zAx;  
            move.y = 0;
            return move;
    }

	public void Highlight()
	{
		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b,0.8f);
	}

	public void Unhighlight()
	{
		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b,storedAlpha);
	}

}
