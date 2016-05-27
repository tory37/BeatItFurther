using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiddleMan : MonoBehaviour {

	public void LoadLevel(Text myText)
    {
        GameManager.Instance.LoadGameplayLevel(myText);
    }

    public void LoadLevel(string level)
    {
        GameManager.Instance.LoadGameplayLevel(level);
    }
}
