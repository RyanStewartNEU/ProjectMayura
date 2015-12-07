using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIValues : MonoBehaviour {

	public Text feathers;
	int numFeathers;
	// Use this for initialization
	void Start () 
	{
		numFeathers = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		feathers.text = "" + numFeathers;
	}

	public void AddFeather()
	{
		numFeathers++;
	}
}
