﻿using UnityEngine;
using System.Collections;

public class WhiteFeatherScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + 3 + Time.deltaTime, 0);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}