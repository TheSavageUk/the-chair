// Television.cs - Television control
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using UnityStandardAssets.ImageEffects;

using System.Collections;

// Television class

public class Television:MonoBehaviour
{
	// Public attributes

	public Material OffMaterial=null;
	public Material StaticMaterial=null;
	public Material VideoMaterial=null;

	// States

	private const int S_OFF=0;
	private const int S_STATIC=1;
	private const int S_WAIT=2;
	private const int S_VIDEO=3;

	// Local data

	private int state=S_OFF;

	private MeshRenderer screen=null;
	private AudioSource sound=null;
	private Light glow=null;
	private Bloom bloom=null;

	private AudioSource soundClick=null;
	private AudioSource soundStatic=null;

	private WWW request=null;
	private MovieTexture movie=null;

/*	private float movieTime=0.0f;
	private float movieLast=0.0f;
	
	private int movieFreeze=0;*/

	// Methods

	// Start
	// Called when object initialized

	void Start()
	{
		GameObject o;

		// get screen
		o=GameObject.Find("Television/Screen");

		screen=o.GetComponent<MeshRenderer>();
		sound=o.GetComponent<AudioSource>();

		// get glow light
		o=GameObject.Find("Television/Glow");
		glow=o.GetComponent<Light>();

		// get sound
		soundClick=gameObject.GetComponent<AudioSource>();
		soundStatic=o.GetComponent<AudioSource>();

		// get bloom effect
		o=GameObject.Find("Player/FirstPersonCharacter");
		bloom=o.GetComponent<Bloom>();
	}

	// Update
	// Called when object updated

	void Update()
	{
		// what state?
		switch(state)
		{
			case S_OFF:
				// tv off
				break;

			case S_STATIC:
				// on, but untuned
				UpdateStatic();
				break;

			case S_WAIT:
				// waiting for load
				UpdateStatic();

				// ready yet?
				if(request!=null &&
					request.movie.isReadyToPlay)
					PlayVideoUrl();
				break;

			case S_VIDEO:
				// playing video
				UpdateVideo();
				break;
		}
	}

	// TurnOff
	// Call to switch tv off

	public void TurnOff()
	{
		// stop playback
		StopVideo();

		// play click
		soundStatic.Stop();
		soundClick.Play();

		// turn screen and light off
		screen.material=OffMaterial;

		glow.enabled=false;
		bloom.enabled=false;

		state=S_OFF;
	}

	// TurnOn
	// Call to switch tv on

	public void TurnOn()
	{
		// play click
		soundClick.Play();

		// set to static
		DisplayStatic();
	}

	// SetVideoFile
	// Call to play video from file

	public void SetVideoFile(string file)
	{
	}

	// SetVideoUrl
	// Call to play video from url (ogg only)

	public void SetVideoUrl(string url)
	{
		// play click
		soundClick.Play();

		// back to static?
		if(state!=S_STATIC)
			DisplayStatic();

		// load up url
		request=new WWW(url);
		state=S_WAIT;
	}

	// Private functions

	// DisplayStatic
	// Call to display television static

	private void DisplayStatic()
	{
		// stop playback
		StopVideo();

		// start static noise
		soundStatic.PlayDelayed(0.15f);

		// turn screen to static and light on
		screen.material=StaticMaterial;

		glow.enabled=true;
		bloom.enabled=true;

		state=S_STATIC;
	}

	// UpdateStatic
	// Call to update static screen

	private void UpdateStatic()
	{
		// set uv to random
		Vector2 v=new Vector2(
			Random.value,
			Random.value);

		StaticMaterial.SetTextureOffset("_MainTex",v);
		glow.intensity=4.0f+Random.value;
	}

	// PlayVideo
	// Call to play current video

	private void PlayVideoUrl()
	{
		// turn off static
		soundStatic.Stop();

		// start video
		movie=request.movie;

		VideoMaterial.mainTexture=movie;
		sound.clip=movie.audioClip;

		movie.Play();
		sound.Play();

/*		movieTime=0.0f;
		movieLast=0.0f;
		
		movieFreeze=0;*/

		request=null;

		// turn screen to video and light on
		screen.material=VideoMaterial;

		glow.enabled=true;
		glow.intensity=1.0f;

		state=S_VIDEO;
	}

	// StopVideo
	// Call to stop video playback

	private void StopVideo()
	{
		// stop audio
		sound.Stop();
		if(movie!=null) movie.Stop();

		// reset usage
		VideoMaterial.mainTexture=null;
		sound.clip=null;

		screen.material=null;
	}

	// UpdateVideo
	// Call to update video screen

	private void UpdateVideo()
	{
		// inc movie play time
/*		if(request.movie.isPlaying)
			movieTime+=Time.deltaTime;

		if(movieTime==movieLast)
			movieFreeze++;

		movieLast=movieTime;

		// back to static if frozen
		if(movieFreeze>100)
			DisplayStatic();*/
	}
}
