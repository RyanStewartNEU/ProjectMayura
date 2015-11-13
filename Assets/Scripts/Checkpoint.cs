using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player")
        {
            Debug.Log("CHECKPOINT SET");
            col.GetComponent<FirstPersonDrifter>().lastCheckpoint = this.transform;
        }
    }
}
