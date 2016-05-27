using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A representation of the level of accuracy the player has hit the note
/// with, this will be based on distance in the editor for the Game Manager
/// </summary>
public enum AccuracyType
{
	Distance1 = 0,
	Distance2 = 1,
	Distance3 = 2,
	Distance4 = 3,
	Distance5 = 4,
	TooFar
}

/// <summary>
/// This sccript controls the overall gameplay.  Variables that persist through
/// levels will be here, etc.
/// This can also control the flow of the gameplay.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Singleton

	public static GameManager Instance
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

	private static GameManager instance;

	#endregion

	#region Editor Interface

    [SerializeField] private string gameplaySceneName;
    [Header("For Testing")]
    [SerializeField] private string gameplayFileName;

	[Header("Accuracy Distances")]
	[SerializeField] private float distance1;
	[SerializeField] private float distance2;
	[SerializeField] private float distance3;
	[SerializeField] private float distance4;
	[SerializeField] private float distance5;

	[Header("Accuracy Points")]
	[SerializeField] private int distance1Points;
	[SerializeField] private int distance2Points;
	[SerializeField] private int distance3Points;
	[SerializeField] private int distance4Points;
	[SerializeField] private int distance5Points;

	[Header("Fail Meter Amounts")]
	[SerializeField] private float failMeter1Amount;
	[SerializeField] private float failMeter2Amount;
	[SerializeField] private float failMeter3Amount;
	[SerializeField] private float failMeter4Amount;
	[SerializeField] private float failMeter5Amount;

	#endregion

    #region Private Interface
    IEnumerator EndGameActivity(bool won)
    {
        if(won)
        {
            float SecondsToEnd = 5.0f;
            float TimePassed = 0.0f;
            while(TimePassed < SecondsToEnd)
            {
                yield return new WaitForSeconds(SecondsToEnd/5f);
                TimePassed += SecondsToEnd / 5f;
            }
            
            Application.LoadLevel("WinScene");
        }
        else
        {
            Application.LoadLevel("LoseScene");
        }
    }
   

    #endregion

    #region Public Interface

    public string GameplayFileName { get { return gameplayFileName; } }

    public float Distance1 { get { return distance1; } }
	public float Distance2 { get { return distance2; } }
	public float Distance3 { get { return distance3; } }
	public float Distance4 { get { return distance4; } }
	public float Distance5 { get { return distance5; } }

	public int Distance1Points { get { return distance1Points; } }
	public int Distance2Points { get { return distance2Points; } }
	public int Distance3Points { get { return distance3Points; } }
	public int Distance4Points { get { return distance4Points; } }
	public int Distance5Points { get { return distance5Points; } }

	public float FailMeter1Amount { get { return failMeter1Amount; } }
	public float FailMeter2Amount { get { return failMeter2Amount; } }
	public float FailMeter3Amount { get { return failMeter3Amount; } }
	public float FailMeter4Amount { get { return failMeter4Amount; } }
	public float FailMeter5Amount { get { return failMeter5Amount; } }

    public void LoadGameplayLevel(Text btnTxt)
    {
        gameplayFileName = btnTxt.text;
        Application.LoadLevel(gameplaySceneName); 
    }

    public void EndGame(bool won)
    {
        StartCoroutine(EndGameActivity(won));
    }

    public void LoadGameplayLevel(string name)
    {
        Application.LoadLevel(name);
    }

	#endregion

	#region Mono Methods

	private void Awake()
	{
		Instance = this;
        DontDestroyOnLoad(this);
	}

	#endregion
}
