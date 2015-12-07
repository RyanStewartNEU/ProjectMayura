using UnityEngine;
using System.Collections;

public class FootStepper : MonoBehaviour {

	public float volume;
	public AudioClip[] grass;
	public AudioClip[] water;
	public AudioClip[] wood;
	public AudioClip[] stone;
	public AudioClip[] noisyGrass;
	FirstPersonDrifter movement;
	int playingMovementSound;
	
	enum CurrentTerrain {
		Water,
		Grass,
		Wood,
		Stone,
		NoisyGrass
	}

	private CurrentTerrain currentTerrain;

	// Use this for initialization
	void Start () 
	{
		movement = transform.GetComponent<FirstPersonDrifter>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentTerrain = terrainFromString(movement.terrain);
		if ((movement.moveDirection.x != 0 || movement.moveDirection.z != 0) && playingMovementSound == 0 && movement.grounded) 
		{
			playMovementSound();
		}
	}

	void FixedUpdate()
	{
		if (playingMovementSound > 0) {
			playingMovementSound--;
		}
	}

	void playMovementSound() 
	{
		if (playingMovementSound == 0) 
		{
			playingMovementSound = 15;

			playWalkingSound(currentTerrain);
		}
	}
	CurrentTerrain terrainFromString(string nam)
	{
		switch (nam)
		{
			case "grass" : 
			return CurrentTerrain.Grass;
			
			case "water" : 
			return CurrentTerrain.Water;
		
			case "wood" : 
			return CurrentTerrain.Wood;

			case "stone" : 
			return CurrentTerrain.Stone;

			case "noisy grass" :
			return CurrentTerrain.NoisyGrass;
		}

		return CurrentTerrain.Grass;
	}
	void playWalkingSound(CurrentTerrain terrain)
	{
		AudioClip[] clips = new AudioClip[0];

		switch(terrain)
		{
			case CurrentTerrain.Grass :
			clips = grass;
			break;

			case CurrentTerrain.Water :
			clips = water;
			break;

			case CurrentTerrain.Wood :
			clips = wood;
			break;

			case CurrentTerrain.Stone :
			clips = stone;
			break;

			case CurrentTerrain.NoisyGrass :
			clips = noisyGrass;
			break;

		}
		int randInt = Random.Range(0, clips.Length - 1);
		AudioClip sound;
		
		sound = clips[randInt];

		AudioSource.PlayClipAtPoint (sound, Camera.main.transform.position, volume);
	}

	

	

	
}
