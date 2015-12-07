// by @torahhorse

using UnityEngine;
using System.Collections;

public class CameraFadeOutEnd : MonoBehaviour
{
	public bool fadeInWhenSceneStarts = true;
	public Color fadeColor = Color.black;
	public float fadeTime = 5f;
	float time;
	bool going;
	
	public void Fade()
	{
		CameraFade.StartAlphaFade(fadeColor, false, fadeTime);
		going = true;
		time = Time.time;
	}

	public void Update()
	{
		if(going)
		{
			if(Time.time - time >= fadeTime - 2.25f)
			going = false;
		}
	}

	public bool done()
	{
		return !going;
	}


}
