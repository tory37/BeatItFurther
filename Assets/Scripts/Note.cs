using UnityEngine;
using System.Collections;

/// <summary>
/// This is the button that needs to be pressed for this note
/// </summary>
public enum NoteButton
{
	A,
	B,
	X,
	Y,
    WAIT
}

/// <summary>
/// This is the direction that needs to be held on the 
/// analog stick for the note
/// </summary>
public enum NoteDirection
{
	Up,
	Left,
	Down,
	Right,
	NoVertical,
	NoHorizontal,
    NONE
}

/// <summary>
/// This script represents a note
/// </summary>
public class Note : MonoBehaviour
{
	#region Public Interface

	public NoteButton Button
	{
		get;
		private set;
	}

	public NoteDirection Direction
	{
		get;
		private set;
	}

    public int? SpawnTime
    {
        get;
        private set;
    }

	private Vector3 noteSpeed;

	public void Initialize(NoteButton button, NoteDirection direction, Vector3 noteSpeed)
	{
		this.Button = button;
		this.Direction = direction;
		this.noteSpeed = noteSpeed;
	}

	#endregion

	#region Mono Methods

	private void Update()
	{
		transform.Translate( noteSpeed * Time.deltaTime);
	}

	#endregion

    #region staticfuncts

    public static NoteDirection CharToDirection(char c)
    {
        switch (c)
        {
            case 'U':
                return NoteDirection.Up;
            case 'D':
                return NoteDirection.Down;
            case 'L':
                return NoteDirection.Left;
            case 'R':
                return NoteDirection.Right;
            default:
                return NoteDirection.Up;
        }
    }

    public static NoteButton CharToButton(char c)
    {
        switch(c)
        {
            case 'A':
                return NoteButton.A;
            case 'B':
                return NoteButton.B;
            case 'X':
                return NoteButton.X;
            case 'Y':
                return NoteButton.Y;
            default:
                return NoteButton.A;
        }
    }


    #endregion

    //private void OnDestroy()
	//{
	//	LevelManager.Instance.NoteQueue.Remove( this );
	//}

}
