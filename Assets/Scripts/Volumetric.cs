// Volumetric.cs - Main volumetric light
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// Volumetric class

public class Volumetric:MonoBehaviour
{
	// States

	private const int S_OFF=0;
	private const int S_WAIT=1;
	private const int S_ON=2;
	
	// Local data

	private int state=S_OFF;

	private MeshRenderer volumetricShaft;
	private Light volumetricLight;
	private AudioSource volumetricSound;

	private float time=0.0f;

	// Methods

	// Start
	// Called when object initialized

	void Start()
	{
		GameObject o;

		// find light shaft
		o=GameObject.Find("Volumetric/Shaft");
		volumetricShaft=o.GetComponent<MeshRenderer>();

		// find actual light
		o=GameObject.Find("Volumetric/Light");
		volumetricLight=o.GetComponent<Light>();

		// find switch noise
		volumetricSound=o.GetComponent<AudioSource>();

		// initial light on
		DelayTurnOn(1.0f);
	}

	// Update
	// Called when object updated

	void Update()
	{
		// what state?
		switch(state)
		{
			case S_OFF:
				// light is off
				break;

			case S_WAIT:
				// wait to come on
				if(Time.time>=time)
					TurnOn();
				break;

			case S_ON:
				// light is on
				break;
		}
	}

	// TurnOff
	// Call to switch light off
	
	public void TurnOff()
	{
		// switch light off
		volumetricLight.enabled=false;
		volumetricShaft.enabled=false;
		
		state=S_OFF;
	}

	// TurnOn
	// Call to switch light on

	public void TurnOn()
	{
		// play switch noise
		volumetricSound.Play();
		
		// enable the shaft and light
		volumetricShaft.enabled=true;
		volumetricLight.enabled=true;
		
		state=S_ON;
	}

	// DelayTurnOn
	// Call to switch light on with delay

	public void DelayTurnOn(float fDelay)
	{
		// set trigger time and wait
		time=Time.time+fDelay;
		state=S_WAIT;
	}
}
