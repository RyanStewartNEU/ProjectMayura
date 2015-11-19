using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public Transform moveTo;
    public Transform moveFrom;
    public float moveSpeed = .1f;
    public bool movingAway = true;
	// Use this for initialization
	void Start () {

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
            if (Vector3.Distance(this.transform.position, moveFrom.transform.position) < 0.1f)
            {
                movingAway = true;
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, moveFrom.transform.position, moveSpeed);
            }
        }
	}
}
