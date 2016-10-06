// Chair.cs - Chair behaviour
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

using System.Collections;

// Chair class

public class Chair:MonoBehaviour
{
	// Public attributes

	public AudioClip sitSound=null;
	public AudioClip standSound=null;

	public string[] stations;

	// Transition times

	private const float T_SIT=1.2f;
	private const float T_LEAN=0.6f;
	private const float T_STAND=1.0f;

	// Fov extents

	private const float F_FORWARD=31.0f;
	private const float F_BACKWARD=60.0f;

	// States

	private const int S_SIT=0;
	private const int S_SITTING=1;
	private const int S_LEANFORWARD=2;
	private const int S_LEANBACKWARD=3;
	private const int S_STAND=4;
	private const int S_STANDING=5;
	
	// Private data

	private int state=S_STANDING;

	private GameObject player=null;
	private GameObject sit=null;
	private GameObject stand=null;

	private FirstPersonController controller=null;
	private Camera view=null;
	private Television television=null;

	private AudioSource sound=null;
	
	private float time=0.0f;
	private float fov=0.0f;

	// Methods

	// Start
	// Called when object initialized

	void Start()
	{
		GameObject o;

		// get objects
		player=GameObject.Find("Player");
		sit=GameObject.Find("Chair/Sit");
		stand=GameObject.Find("Chair/Stand");

		// get controllers
		controller=player.GetComponent<FirstPersonController>();
		
		// get camera
		o=GameObject.Find("Player/FirstPersonCharacter");
		view=o.GetComponent<Camera>();

		// get television script
		o=GameObject.Find("Television");
		television=o.GetComponent<Television>();

		// get sound source
		sound=gameObject.GetComponent<AudioSource>();
	}
	
	// Update
	// Called when object updated

	void Update()
	{
		float lerp;

		// what state?
		switch(state)
		{
			case S_SIT:
				// sit down
				lerp=(Time.time-time)/T_SIT;

				if(lerp>1.0f)
				{
					// now sitting
					Sitting();
					lerp=1.0f;
				}

				// lerp to sitting pos
				LerpTransform(
					view.transform,
					sit.transform,
					lerp);
				break;

			case S_SITTING:
				// sitting down
				HandleSitting();
				break;

			case S_LEANFORWARD:
				// leaning forward
				lerp=(Time.time-time)/T_LEAN;

				if(lerp>1.0f)
				{
					// comfey?
					state=S_SITTING;
					lerp=1.0f;
				}

				// lerp fov in
				view.fieldOfView=Mathf.Lerp(
					F_BACKWARD,
					F_FORWARD,
					lerp);
				break;

			case S_LEANBACKWARD:
				// leaning backward
				lerp=(Time.time-time)/T_LEAN;

				if(lerp>1.0f)
				{
					// comfey?
					state=S_SITTING;
					lerp=1.0f;
				}

				// lerp fov out
				view.fieldOfView=Mathf.Lerp(
					F_FORWARD,
					F_BACKWARD,
					lerp);
				break;

			case S_STAND:
				// stand up
				lerp=(Time.time-time)/T_STAND;

				if(lerp>1.0f)
				{
					// now standing
					Standing();
					lerp=1.0f;
				}

				// lerp to standing pos
				LerpTransform(
					view.transform,
					stand.transform,
					lerp);

				// lerp fov out
				view.fieldOfView=Mathf.Lerp(
					fov,
					F_BACKWARD,
					lerp*2.0f);
				break;

			case S_STANDING:
				// now standing
				break;
		}
	}

	// OnTriggerEnter
	// Called when colliders enter each other

	void OnTriggerEnter(Collider other)
	{
		// not the player?
		if(other.gameObject.name!="Player")
			return;

		// sit down
		SitDown();
	}

	// Local functions

	// SitDown
	// Call to sit player in chair

	private void SitDown()
	{
		// ignore if not standing
		if(state!=S_STANDING)
			return;

		// disable controller
		controller.enabled=false;

		// keep camera details
		stand.transform.position=view.transform.position;
		stand.transform.rotation=view.transform.rotation;
		
		// play sitting
		sound.clip=sitSound;
		sound.Play();
		
		// start sitting
		time=Time.time;
		state=S_SIT;
	}

	// Sitting
	// Called when player is sitting

	private void Sitting()
	{
		// turn tv on
		television.TurnOn();

		// pick random channel
		string url=stations[Random.Range(0,10)];
		television.SetVideoUrl(url);

		// now sitting
		state=S_SITTING;
	}

	// StandUp
	// Call to stand player back up

	private void StandUp()
	{
		// ignore if not sitting
		if(state!=S_SITTING)
			return;

		// play standing
		sound.clip=standSound;
		sound.Play();

		// start standing
		fov=view.fieldOfView;

		time=Time.time;
		state=S_STAND;
	}

	// Standing
	// Called when player is standing

	private void Standing()
	{
		// turn tv off
		television.TurnOff();

		// re-enable controller
		controller.enabled=true;

		// restore camera
		view.transform.position=stand.transform.position;
		view.transform.rotation=stand.transform.rotation;

		// now standing
		state=S_STANDING;
	}

	// HandleSitting
	// Called when player is sitting

	private void HandleSitting()
	{
		// lean in
		if(Input.GetKeyDown(KeyCode.UpArrow) &&
			LeanForward())
			return;

		// lean out
		if(Input.GetKeyDown(KeyCode.DownArrow) &&
			LeanBackward())
			return;

		// play station?
		for(int n=0;n<=9;n++)
		{
			if(Input.GetKeyDown(KeyCode.Alpha0+n))
			{
				television.SetVideoUrl(stations[n]);
				return;
			}
		}

		// any other key?
		if(Input.anyKeyDown)
			StandUp();
	}

	// LeanForward
	// Call to lean forward in chair

	private bool LeanForward()
	{
		// already forward?
		if(view.fieldOfView<=F_FORWARD)
			return false;

		// play chair noise
		sound.Play();

		// lean forward
		time=Time.time;
		state=S_LEANFORWARD;

		return true;
	}

	// LeanBackward
	// Call to lean backward in chair

	private bool LeanBackward()
	{
		// already backward?
		if(view.fieldOfView>=F_BACKWARD)
			return false;

		// play chair noise
		sound.Play();

		// lean backward
		time=Time.time;
		state=S_LEANBACKWARD;

		return true;
	}

	// LerpTransform
	// Call to lerp between two transforms

	private void LerpTransform(Transform t1,Transform t2,float t)
	{
		// lerp position and rotation
		t1.position=Vector3.Lerp(t1.position,t2.position,t);
		t1.rotation=Quaternion.Slerp(t1.rotation,t2.rotation,t);
	}
}
