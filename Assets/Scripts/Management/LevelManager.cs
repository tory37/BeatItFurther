using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This script controls the level itself.  It listens for input from the controller, 
/// and handles that, etc.
/// </summary>
public class LevelManager : MonoBehaviour {

	#region Singleton

	public static LevelManager Instance
	{
		get {
			return instance;
		}
		set {
			if (instance == null)
				instance = value;
			else
				Destroy(value.gameObject);
		}
	}

	private static LevelManager instance;

	#endregion

	#region Editor Interface

	[SerializeField] private Transform centerPointTransform;

    [SerializeField]
    private float introSeconds;

	[Header("UI Elements")]
	[SerializeField] private Text textScore;
	[SerializeField] private Text textMultiplier;
	[SerializeField] private Text textDistance1Count;
	[SerializeField] private Text textDistance2Count;
	[SerializeField] private Text textDistance3Count;
	[SerializeField] private Text textDistance4Count;
	[SerializeField] private Text textDistance5Count;
	[SerializeField] private Image failMeterImage;
	[SerializeField] private Image failtMeterCenterImage;
	[SerializeField] private Animator centerPointAnimator;
    [SerializeField] private GameObject[] indicators;

    [Header("Note Spawner")]
    [SerializeField]
    NoteSpawner spawner;

	#endregion

	#region Private Fields

	private Vector3 centerPoint;

	private Dictionary<AccuracyType, int> accuracyCount;

	private int multiplierCount;
	private int multiplier;

	private int score;

	private float failMeter;
	private float failMeterLength;
    private AudioSource source;
    private Visualizer visual;
	#endregion

	public Queue<Note> NoteQueue;

	#region Mono Methods

	private void Awake()
	{
		Instance = this;

		NoteQueue = new Queue<Note>();

		accuracyCount = new Dictionary<AccuracyType, int> {
			{ AccuracyType.Distance1, 0 },
			{ AccuracyType.Distance2, 0 },
			{ AccuracyType.Distance3, 0 },
			{ AccuracyType.Distance4, 0 },
			{ AccuracyType.Distance5, 0 }
		};

        source = gameObject.GetComponent<AudioSource>();
        visual = Visualizer.Instance;
	}

	private void Start()
	{
        initializeIndicators();

		centerPoint = centerPointTransform.position;

		multiplierCount = 0;
		multiplier = 1;
		score = 0;

		failMeter = 50.0f;
		failMeterLength = failMeterImage.rectTransform.rect.width;

        StartCoroutine(WaitForIntro());
	}

	private void Update()
	{
        updateIndicators();

		if ( NoteQueue.Count > 0 && CheckIfNotePassed() )
		{
			//Debug.Log(NoteQueue.Peek().GetInstanceID());
			Destroy( NoteQueue.Dequeue().gameObject );
			AdjustAndRedrawGUI( AccuracyType.Distance5 );
		}
		else
		{
			ListenForButton();
		}
	}

	#endregion

	#region Private Methods

    private void initializeIndicators()
    {
        for (int i = 0; i < 4; i++)
            indicators[i].SetActive(false);

    }

    private void updateIndicators()
    {
        if (Input.GetAxis("Horizontal") > 0f)
        {
            indicators[0].SetActive(true);
        }
        else
        {
            indicators[0].SetActive(false);
        }
        if (Input.GetAxis("Vertical") > 0f)
        {
            indicators[1].SetActive(true);
            
        }
        else
        {
            indicators[1].SetActive(false);
            
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            indicators[2].SetActive(true);
           
        }
        else
        {
            indicators[2].SetActive(false);
            
        }
        if (Input.GetAxis("Vertical") < 0f)
        {
            indicators[3].SetActive(true);
            
        }
        else
        {
            indicators[3].SetActive(false);
            
        }
    }


	private bool CheckIfNotePassed()
	{
		Note note = NoteQueue.Peek();

		if ( (note.Direction == NoteDirection.Up && (note.transform.position.z - centerPoint.z) > GameManager.Instance.Distance5) ||
		    (note.Direction == NoteDirection.Down && (note.transform.position.z - centerPoint.z) < -GameManager.Instance.Distance5) ||
			(note.Direction == NoteDirection.Left && (note.transform.position.x - centerPoint.x) < -GameManager.Instance.Distance5) ||
		    (note.Direction == NoteDirection.Right && (note.transform.position.x - centerPoint.x) > GameManager.Instance.Distance5) )
		{
			//Debug.Log( "MISS!" );
			return true;
		}
		return false;
	}

	private void ListenForButton()
	{
		if (Input.GetButtonDown("A"))
		{
			HandleInput(NoteButton.A);
			Debug.Log( "A" );
		} 
		else if (Input.GetButtonDown("B"))
		{
			HandleInput( NoteButton.B );
			Debug.Log( "B" );
		}
		else if (Input.GetButtonDown("X"))
		{
			HandleInput( NoteButton.X );
			Debug.Log( "X" );
		}
		else if (Input.GetButtonDown("Y"))
		{
			HandleInput( NoteButton.Y );
			Debug.Log( "Y" );
		}
	}

	private AccuracyType DetermineAccuracy()
	{
		Note note = NoteQueue.Peek();

		if ( NoteQueue.Count > 0 && note != null )
		{
			if (( new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) - 
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude 
                                                        >= GameManager.Instance.Distance5)
			{
				return AccuracyType.Distance5;
			}
			else
			{
				AccuracyType accuracy;
                if ((new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) -
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude
                                                        <= GameManager.Instance.Distance1)
				{
					accuracy = AccuracyType.Distance1;
				}
                else if ((new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) -
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude
                                                        <= GameManager.Instance.Distance2)
				{
					accuracy = AccuracyType.Distance2;
				}
                else if ((new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) -
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude
                                                        <= GameManager.Instance.Distance3)
				{
					accuracy = AccuracyType.Distance3;
				}
                else if ((new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) -
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude
                                                        <= GameManager.Instance.Distance4)
				{
					accuracy = AccuracyType.Distance4;
				}
				else
				{
					accuracy = AccuracyType.Distance5;
				}
				return accuracy;
			}
		}
		else
		{
			return AccuracyType.Distance5;
		}
	}

	private NoteDirection DetermineVerticalDirection()
	{
		float vertical = Input.GetAxis( "Vertical" );
		if ( vertical > 0f )
			return NoteDirection.Up;
		else if ( vertical < 0f )
			return NoteDirection.Down;
		else
			return NoteDirection.NoVertical;
	}


	private NoteDirection DetermineHorizontalDirecction()
	{
		float horizontal = Input.GetAxis( "Horizontal" );
		if ( horizontal > 0f )
			return NoteDirection.Right;
		else if ( horizontal < 0f )
			return NoteDirection.Left;
		else
			return NoteDirection.NoHorizontal;
	}

	private void HandleInput(NoteButton buttonPressed)
    {
        AccuracyType type = AccuracyType.Distance5;

        if (NoteQueue.Count > 0)
        {
            Note note = NoteQueue.Peek();

            if (NoteQueue.Count > 0 && note != null && (new Vector3(note.transform.position.x, 0.0f, note.transform.position.z) - 
                                                        new Vector3(centerPoint.x, 0.0f, centerPoint.z)).magnitude 
                                                        <= GameManager.Instance.Distance5)
            {
                //Debug.Log("Current Note: " + note.Button + ", " + note.Direction);
                //Debug.Log("Button Pressed: " + buttonPressed.ToString() + ", Horizontal: " + Input.GetAxis("Horizontal") + ", Vertical: " + Input.GetAxis("Vertical"));

                if (note.Button == buttonPressed)
                {
                    if (note.Direction == NoteDirection.Left || note.Direction == NoteDirection.Right)
                    {
                        if (note.Direction == DetermineHorizontalDirecction())
                            //Debug.Log( DetermineAccuracy().ToString() + "!" );
                            type = DetermineAccuracy();
                        else
                            //Debug.Log("Miss");
                            type = AccuracyType.Distance5;
                    }
                    else if (note.Direction == NoteDirection.Up || note.Direction == NoteDirection.Down)
                    {
                        if (note.Direction == DetermineVerticalDirection())
                            //Debug.Log( DetermineAccuracy().ToString() );
                            type = DetermineAccuracy();
                        else
                            //Debug.Log( "MISS!" );
                            type = AccuracyType.Distance5;
                    }
                }
                else
                {
                    type = AccuracyType.Distance5;
                }

                centerPointAnimator.SetTrigger("Bounce");
                Destroy(NoteQueue.Dequeue().gameObject);
            }
            else
            {
                //Debug.Log( "Miss!: No top note." );
                type = AccuracyType.Distance5;
            }
        }
        else
        {
            //Debug.Log( "Miss!: No top note." );
            type = AccuracyType.Distance5;
        }

		AdjustAndRedrawGUI(type);
        //FeedbackManager.Instance.Animate(type);
	}

	private void AdjustAndRedrawGUI( AccuracyType type )
	{
		// Adjust Accuracy Count
		accuracyCount[type]++;

		// Adjust Multiplier
		if ( type != AccuracyType.Distance5 )
		{
			if ( multiplier < 4 )
			{
				if ( multiplierCount >= 9 )
				{
					multiplier++;
					multiplierCount = 0;
				}
				else
				{
					multiplierCount++;
				}
			}
		}
		else
		{
			multiplier = 1;
			multiplierCount = 0;
		}

		// Adjust Score and Fail Meter
		if ( type == AccuracyType.Distance1 )
		{
			score += GameManager.Instance.Distance1Points * multiplier;
			failMeter += GameManager.Instance.FailMeter1Amount * multiplier;
		}
		else if ( type == AccuracyType.Distance2 )
		{
			score += GameManager.Instance.Distance2Points * multiplier;
			failMeter += GameManager.Instance.FailMeter2Amount * multiplier;
		}
		else if ( type == AccuracyType.Distance3 )
		{
			score += GameManager.Instance.Distance3Points * multiplier;
			failMeter += GameManager.Instance.FailMeter3Amount * multiplier;
		}
		else if ( type == AccuracyType.Distance4 )
		{
			score += GameManager.Instance.Distance4Points * multiplier;
			failMeter += GameManager.Instance.FailMeter4Amount * multiplier;
		}
		else if ( type == AccuracyType.Distance5 )
		{
			score += GameManager.Instance.Distance5Points * multiplier;
			failMeter += GameManager.Instance.FailMeter5Amount * multiplier;
		}

		failMeter = Mathf.Clamp( failMeter, 0, 100 );

        if (failMeter <= 0)
        {
            GameManager.Instance.EndGame(false);
        }

		textScore.text = score.ToString();
		textMultiplier.text = "x" + multiplier;
		textDistance1Count.text = accuracyCount[AccuracyType.Distance1].ToString();
		textDistance2Count.text = accuracyCount[AccuracyType.Distance2].ToString();
		textDistance3Count.text = accuracyCount[AccuracyType.Distance3].ToString();
		textDistance4Count.text = accuracyCount[AccuracyType.Distance4].ToString();
		textDistance5Count.text = accuracyCount[AccuracyType.Distance5].ToString();
		failtMeterCenterImage.rectTransform.position = failMeterImage.rectTransform.position - (Vector3.right * (failMeterLength / 2)) + new Vector3( ((failMeterLength / 100) * failMeter), 0f, 0f );
	}

    private IEnumerator WaitForIntro()
    {
        int tempo = spawner.Load();
        visual.SetTempo(tempo);

        AudioClip clip = Resources.Load(spawner.MP3Name.Replace(".mp3", "")) as AudioClip;
        source.clip = clip;
        yield return new WaitForSeconds(introSeconds);

        spawner.WakeUp();

        yield return new WaitForSeconds(2f * (60f / (float)tempo));

        source.Play();
    }

#endregion
}
