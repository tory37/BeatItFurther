using UnityEngine;
using System.Collections;

public class SongEditorNote : MonoBehaviour
{

	#region Editor Interface

	[SerializeField] private NoteDirection direction;
	[SerializeField] private NoteButton button;

	#endregion

	#region Public Interface

	public NoteDirection Direction;
	public NoteButton Button;
	public string Duration;

	#endregion

}
