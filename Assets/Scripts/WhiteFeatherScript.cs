using UnityEngine;
using System.Collections;

public class WhiteFeatherScript : MonoBehaviour {

	public AudioClip featherGetClip;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + 3 + Time.deltaTime, 0);
		//transform.RotateAround(transform.position - transform.GetComponent<CapsuleCollider>().center, Vector3.up, Time.deltaTime * 200);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player")
        {
			AudioSource.PlayClipAtPoint(featherGetClip, this.transform.position);
            Destroy(this.gameObject);
        	PlayerPrefs.SetInt("WhiteFeathers" , PlayerPrefs.GetInt("WhiteFeathers")+ 1);
        }
    }
}
