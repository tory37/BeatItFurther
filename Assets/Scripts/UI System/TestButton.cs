using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestButton : MonoBehaviour {

	public void PrintText()
	{
		Debug.Log( GetComponentInChildren<Text>().text );
	}
}
