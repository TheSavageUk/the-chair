// Warp.cs - Collider warp
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// Warp class

public class Warp:MonoBehaviour
{
	// Public attributes

	public float WarpX=0.0f;
	public float WarpY=0.0f;
	public float WarpZ=0.0f;

	// Local data

	private Volumetric volumetric=null;

	// Methods

	// Start
	// Called when object initialized

	void Start()
	{
		GameObject o;

		// get main light
		o=GameObject.Find("Volumetric");
		volumetric=o.GetComponent<Volumetric>();
	}

	// Update
	// Called when object updated

	void Update()
	{
	}

	// OnTriggerEnter
	// Called when colliders enter each other
	
	void OnTriggerEnter(Collider other)
	{
		// not the player?
		if(other.gameObject.name!="Player")
			return;

		// turn light off
		volumetric.TurnOff();

		// i like to move it, move it
		Vector3 v=other.gameObject.transform.position;

		v.x+=WarpX;
		v.y+=WarpY;
		v.z+=WarpZ;

		other.gameObject.transform.position=v;

		// then back on
		volumetric.TurnOn();
	}
}
