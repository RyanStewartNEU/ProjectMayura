using UnityEngine;
using System.Collections;

public class WarpTo : MonoBehaviour {

    public int levelNum;
    CameraFadeOutEnd fade;
    public float fadeTime = 4;
	// Use this for initialization
	void Start () 
	{
		fade = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(fade != null && fade.done())
		  Application.LoadLevel(levelNum);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
           fade = Camera.main.transform.gameObject.AddComponent<CameraFadeOutEnd>();
          	fade.fadeTime = fadeTime;
          	fade.Fade();
        }
    }
}
