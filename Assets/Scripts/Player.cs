// Player.cs - Player script
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// Player class

public class Player:MonoBehaviour
{
	// Methods
	
	// Start
	// Called when object initialized

	void Start()
	{
		// remove and lock cursor
		Cursor.visible=false;
		Cursor.lockState=CursorLockMode.Locked;
	}
	
	// Update
	// Called when object updated

	void Update()
	{
	}
}
