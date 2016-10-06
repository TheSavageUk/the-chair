// FacePlayer.cs - Face object towards player
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// FacePlayer class

public class FacePlayer:MonoBehaviour
{
	// Local data

	private GameObject player=null;

	// Methods
	
	// Start
	// Called when object initialized

	void Start()
	{
		// find player
		player=GameObject.Find("Player");
	}

	// Update
	// Called when object updated

	void Update()
	{
		// look at player from same height
		Vector3 v=player.transform.position;
		v.y=transform.position.y;

		transform.LookAt(v);
	}
}
