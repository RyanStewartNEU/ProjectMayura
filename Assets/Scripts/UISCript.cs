using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISCript : MonoBehaviour {

	private static string SHOW_UI_BUTTON = "z";
	private static string FEATHER_COUNT_KEY = "WhiteFeathers";

	public Text featherCountUIText;
	private int lastFeatherCount;

	// Use this for initialization
	protected void Start () {
		setFeatherCountInUI (getFeatherCountFromSaveData());
	}
	
	// Update is called once per frame
	protected void Update () {
		setFeatherCountInUI (getFeatherCountFromSaveData());
	}

	// Call this public method when the data is updated
	public void shouldReadNewDataAndUpdate() {
		int featherCount = getFeatherCountFromSaveData ();

		if (featherCount > lastFeatherCount) {
			Debug.Log ("Received update with new feather count");
			setFeatherCountInUI(featherCount);
		}
	}

	// Is the UI button held (temporarily set to z on keyboard)
	// ** currently unused **
	private bool showUIButtonHeld() {
		return Input.GetButton(SHOW_UI_BUTTON);
	}

	// Given a feather count, set the UI text
	private void setFeatherCountInUI(int featherCount) {
		this.featherCountUIText.text = "" + featherCount.ToString();
		this.lastFeatherCount = featherCount;
		Debug.Log ("set feather count as " + featherCount.ToString ());
	}

	// Read feather count from data. Return 0 if negative or missing
	private int getFeatherCountFromSaveData() {
		int featherCount = 0;

		if (PlayerPrefs.HasKey (FEATHER_COUNT_KEY)) {
			featherCount = PlayerPrefs.GetInt(FEATHER_COUNT_KEY);
			
			if (featherCount < 0) {
				featherCount = 0;
			}
		}

		return featherCount;
	}
}
