using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SongEditorButtons : MonoBehaviour {

	public void AddSongLine()
	{
		GameObject newLine = Instantiate( SongEditorManager.Instance.SongLine, SongEditorManager.Instance.AddLineButton.transform.position, Quaternion.identity ) as GameObject;
		SongEditorManager.Instance.AddLineButton.transform.position += new Vector3( 0f, 0f, -1f );
		newLine.transform.parent = SongEditorManager.Instance.SongLineParent;
	}

	public void DeleteSongLine(GameObject line)
	{
		if (UnityEditor.EditorUtility.DisplayDialog( "Delete Line", "Are you sure you want to delete this line?", "Yes", "No" ))
		{
			SongEditorManager.Instance.AddLineButton.transform.position += new Vector3( 0f, 0f, 1f );
			Destroy( line.gameObject );
		}

	}
}
