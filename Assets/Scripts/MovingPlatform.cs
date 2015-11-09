using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public Transform moveTo;
    public float moveSpeed = .1f;
    private Vector3 startFrom;
    public bool movingAway = true;
	// Use this for initialization
	void Start () {
        startFrom = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (movingAway)
        {
            if (Vector3.Distance(this.transform.position, moveTo.position) < 0.1f)
            {
                movingAway = false;
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, moveTo.position, moveSpeed);
            }
        }
        else
        {
            if (Vector3.Distance(this.transform.position, startFrom) < 0.1f)
            {
                movingAway = true;
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, startFrom, moveSpeed);
            }
        }
	}
}
