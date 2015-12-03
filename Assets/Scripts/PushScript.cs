using UnityEngine;
using System.Collections;

public class PushScript : MonoBehaviour {

    public Vector3 pushdirection = new Vector3(5, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //if in this zone, move the player as if in high winds
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.gameObject.transform.position += pushdirection*Time.deltaTime;
        }
    }
}
