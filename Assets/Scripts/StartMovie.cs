﻿using UnityEngine;
using System.Collections;

public class StartMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// this line of code will make the Movie Texture begin playing
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
	}
}
