using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum BC_States
{
    GoUp,
    GoDown
}

public class BackgroundColorizer : MonoFSM {

    public enum Value
    {
        Red,
        Green,
        Blue
    }

    public Value CurrentColor { get; set; }

    public Image MyImage { get; private set; }

    protected override void Initialize()
    {
        GetComponent<Image>().color = new Color(1, 0, 0);
        CurrentColor = Value.Blue;
        MyImage = GetComponent<Image>();
    }
}
