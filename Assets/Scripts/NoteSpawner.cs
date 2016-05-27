using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{


	#region Editor Interface

	[SerializeField] private string songFileName;

	[SerializeField] private List<NoteButton> testNoteButtons;
	[SerializeField] private List<NoteDirection> testNoteDirections;
	[SerializeField] private float testSpeed;
	[SerializeField] private float testSpawnInterval;

	[Header("Spawn Points")]
	[SerializeField] private Transform bottomSpawner;
	[SerializeField] private Transform rightSideSpawner;
	[SerializeField] private Transform topSpawner;
	[SerializeField] private Transform leftSideSpawner;

	[Header("NotePrefabs")]
	[SerializeField] private Note aNote;
	[SerializeField] private Note bNote;
	[SerializeField] private Note xNote;
	[SerializeField] private Note yNote;

	[Header("Animators")]
	[SerializeField] private Animator topAnim;
	[SerializeField] private Animator leftAnim;
	[SerializeField] private Animator bottomAnim;
	[SerializeField] private Animator rightAnim;

	#endregion

    /// <summary>
    /// Used to store notes read from file
    /// </summary>

	public static NoteSpawner Instance
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
	private static NoteSpawner instance;

    public SongDataLoader Loader {get; private set;}

    public string MP3Name { get; private set; }
    public float noteSpeed;
    private float timeToWait;
    private float timeWaited;

    private bool running;
    
    NoteDataHolder next;

    public int Load()
    {
        Loader = new SongDataLoader();
        //ReadNotes = new Queue<NoteDataHolder>();
        if (Application.isEditor)
            Loader.loadSong(@".\Assets\Resources\" + GameManager.Instance.GameplayFileName + ".dat");
        else
            Loader.loadSong(@".\beatit_Data\Resources\" + GameManager.Instance.GameplayFileName + ".dat");
        
        MP3Name = GameManager.Instance.GameplayFileName + ".mp3";
        noteSpeed = 5 / ( Loader.NoteSpeed * ( 60f / Loader.Tempo));
        return Loader.Tempo;
    }

	void Awake()
	{
		Instance = this;
	}

    void Start()
    {
        running = false;

    }

	public void WakeUp()
	{
		running = true;
		timeWaited = 0.0f;
		NoteDataHolder initialWait = Loader.getNextNote();
		timeToWait = initialWait.noteLength;
	}

	//public void WakeUp()
	//{
	//	running = true;
	//	timeWaited = 0.0f;
	//	next = loader.getNextNote();
	//	timeToWait = next.noteLength;
	//}

	//void Update()
	//{
	//	if (running)
	//	{
	//		if ( timeWaited >= timeToWait )
	//		{
	//			next = loader.getNextNote();
	//			if ( next.button == NoteButton.WAIT && next.direction == NoteDirection.NONE ) //we encountered a wait
	//			{

	//			}
	//			else
	//			{
	//				Note spawnedNote = Instantiate( DetermineNote( next.button ), DetermineSpawnPoint( next.direction ), Quaternion.identity ) as Note;
	//				spawnedNote.Initialize( next.button, next.direction, DetermineNoteSpeed( next.direction ) + (Vector3.up * 1.5f) );
	//				LevelManager.Instance.NoteQueue.Enqueue( spawnedNote );
	//			}
	//			if (!loader.IsEmpty())
	//			{
	//				timeToWait = next.noteLength;
	//				next = loader.getNextNote();
	//			}
	//		}
	//		timeWaited += Time.deltaTime;
	//		if (timeWaited >= timeToWait)
	//		{

	//		}
	//	}
	//}
    
    void Update()
    {
        if(running)
        {
            timeWaited += Time.deltaTime;
            Debug.Log("Started Parsing notes");
            //if it spawned, spawn it
            if(timeWaited >= timeToWait)
            {
				if ( !Loader.IsEmpty() )
				{
					next = Loader.getNextNote();
					if ( next.button == NoteButton.WAIT && next.direction == NoteDirection.NONE ) //we encountered a wait
					{

					}
					else
					{
						Note spawnedNote = Instantiate( DetermineNote( next.button ), DetermineSpawnPoint( next.direction ), Quaternion.identity ) as Note;
						spawnedNote.Initialize( next.button, next.direction, DetermineNoteSpeed( next.direction ) + (Vector3.up * 1.5f) );
						LevelManager.Instance.NoteQueue.Enqueue( spawnedNote );
					}

						float extraWaited = 0;
						float newWait = next.noteLength;
						if ( timeWaited - timeToWait > 0 )
						{
							extraWaited = timeWaited - timeToWait;
						}
						timeWaited = extraWaited;
						timeToWait = newWait;
						timeToWait = next.noteLength;
				}
				else
				{
					running = false;
					//Notify the game manager
					GameManager.Instance.EndGame( true );
				}
            }

        }
    }

	public void Signal()
    {
		//StartCoroutine( SpawnNotes() );
        running = true;
	}

    //private IEnumerator SpawnNotes()
    //{
    //    NoteDataHolder temp;
    //    Debug.Log("Started Parsing notes");
    //    while(!loader.IsEmpty())
    //    {
    //        temp = loader.getNextNote();
    //        //we only want to actually do this if it is not a no-note
    //        if(temp.button == NoteButton.WAIT && temp.direction == NoteDirection.NONE) //we encountered a wait
    //        {
    //            yield return new WaitForSeconds(temp.timeToWait);
    //        }
    //        else
    //        {
    //            Note spawnedNote = Instantiate(DetermineNote(temp.button), DetermineSpawnPoint(temp.direction), Quaternion.identity) as Note;
    //            spawnedNote.Initialize(temp.button, temp.direction, DetermineNoteSpeed(temp.direction) + (Vector3.up * 1.5f));
    //            LevelManager.Instance.NoteQueue.Enqueue(spawnedNote);
    //        }
    //        if(!loader.IsEmpty())
    //        {
    //            yield return new WaitForSeconds(loader.GetTimeToWait());
    //        }
    //        else
    //        {
    //            break; //end of the song
    //        }			
    //    }
    //}

	private Note DetermineNote(NoteButton button)
	{
		if ( button == NoteButton.A )
			return aNote;
		else if ( button == NoteButton.B )
			return bNote;
		else if ( button == NoteButton.X )
			return xNote;
		else 
			return yNote;
	}

	private Vector3 DetermineSpawnPoint(NoteDirection direction)
	{
		if ( direction == NoteDirection.Down )
		{
			topAnim.SetTrigger( "Go" );
			return topSpawner.position;
		}
		else if ( direction == NoteDirection.Left )
		{
			rightAnim.SetTrigger( "Go" );
			return rightSideSpawner.position;
		}
		else if ( direction == NoteDirection.Up )
		{
			bottomAnim.SetTrigger( "Go" );
			return bottomSpawner.position;
		}
		else
		{
			leftAnim.SetTrigger( "Go" );
			return leftSideSpawner.position;
		}
	}

	private Vector3 DetermineNoteSpeed(NoteDirection direction)
	{

        if (direction == NoteDirection.Down)
            return new Vector3(0f, 0f, -noteSpeed);
        else if (direction == NoteDirection.Left)
            return new Vector3(-noteSpeed, 0f, 0f);
        else if (direction == NoteDirection.Up)
            return new Vector3(0f, 0f, noteSpeed);
        else
            return new Vector3(noteSpeed, 0f, 0f);
	}

}
