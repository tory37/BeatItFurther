using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SongEditorManager : MonoBehaviour {

	#region Editor Interface

	[Header( "Inputs" )]
	[SerializeField] private InputField inptTitle;
	[SerializeField] private InputField inptArtist;
	[SerializeField] private InputField inptBPM;

	[Header( "Prefabs" )]
	[SerializeField] private GameObject songLine;

	[Header ( "Components" )]
	[SerializeField] private Transform songLineParent;
	[SerializeField] private GameObject addLineButton;

	[Header( "Variables" )]
	[SerializeField] private float cameraSpeed;

	#endregion

	#region Public Interface

	public static SongEditorManager Instance
	{
		get
		{
			return instance;
		}
		set
		{
			if ( instance == null )
				instance = value;
			else
				Destroy( value.gameObject );
		}
	}

	public GameObject SongLine { get { return songLine; } }
	public GameObject AddLineButton { get { return addLineButton; } }
	public Transform SongLineParent { get { return songLineParent; } }

	#endregion

	#region Private Fields

	private static SongEditorManager instance;

	private int numLines;

	#endregion

	#region Mono Methods

	private void Awake()
	{
		Instance = this;

		numLines = 1;
	}

	private void Update()
	{
		float vertical = Input.GetAxis( "Vertical" ) * cameraSpeed * Time.deltaTime;

		Camera.main.transform.position += Vector3.forward * vertical;
	}

	#endregion

	public void ButtonTest()
	{
		Debug.Log( "Button Pressed" );
	}

	
}
