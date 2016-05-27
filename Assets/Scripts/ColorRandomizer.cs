using UnityEngine;
using System.Collections;

/// <summary>
/// This script simply randomly generates a color for its parent object to be
/// upon its creation
/// </summary>
public class ColorRandomizer : MonoBehaviour {

    private float red;
    private float green;
    private float blue;
    private Material theMaterial;
    public Material generic;
    

	// Use this for initialization
	void Start () {
        red = Random.Range(.3f, 1);
        green = Random.Range(.3f, 1);
        blue = Random.Range(.3f, 1);
        theMaterial = new Material(generic);
        theMaterial.color =  new Color(red, green, blue);
        gameObject.GetComponent<Renderer>().material = theMaterial;
    }
}
