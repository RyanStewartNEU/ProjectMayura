using UnityEngine;
using System.Collections;

public class ClearSave:MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            Debug.Log("PLAYER DATA WIPED");
            PlayerPrefs.DeleteAll();
        }
    }

    
}
