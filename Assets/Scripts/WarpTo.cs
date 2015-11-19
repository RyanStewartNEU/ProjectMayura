using UnityEngine;
using System.Collections;

public class WarpTo : MonoBehaviour {

    public int levelNum;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            Application.LoadLevel(levelNum);
        }
    }
}
