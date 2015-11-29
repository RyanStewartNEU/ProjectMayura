using UnityEngine;
using System.Collections;

public class Sound_Player : MonoBehaviour {

	public AudioClip clip;
	public AudioSource source;
	// Use this for initialization
	void Start () 
	{
		source.clip = clip;
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
