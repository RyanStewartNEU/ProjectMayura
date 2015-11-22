using UnityEngine;
using System.Collections;

public class WhiteFeatherScript : MonoBehaviour {

	public AudioClip featherGetClip;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + 3 + Time.deltaTime, 0);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player")
        {
			AudioSource.PlayClipAtPoint(featherGetClip, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
