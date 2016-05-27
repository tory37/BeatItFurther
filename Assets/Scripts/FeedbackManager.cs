using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FeedbackManager: MonoBehaviour
{

    #region Editor Interface

    [Header("HUE HUE")]
    [SerializeField]
    private Texture ballaImage;
    [SerializeField]
    private Texture prettyGoodImage;
    [SerializeField]
    private Texture alrightImage;
    [SerializeField]
    private Texture crappyImage;
    [SerializeField]
    private Texture failImage;
    
    [SerializeField]
    private List<Animator> feedbackAnimators;
    #endregion

    #region Private Fields
    private int index = 0;

    #endregion

    #region Singleton
    public static FeedbackManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
                instance = value;
            else
                Destroy(value.gameObject);
        }
    }

    private static FeedbackManager instance;
    #endregion



    #region Mono Methods


    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Public Methods

    public void Animate(AccuracyType type)
    {
        
        if (index == 4)
            index = 0;
        
        // **********************************
        // NEED TO ATTACH IMAGE TO EACH OBJECT
        feedbackAnimators[index].SetTrigger("Bounce");
        feedbackAnimators[index].gameObject.GetComponent<Material>();
        
        
        /*int num = Random.Range(0, 4);
        if (type == AccuracyType.Distance1)
        {
            Instantiate(ballaPrefab, this.transform.position, Quaternion.identity);
        }
        else if (type == AccuracyType.Distance2)
        {
            Instantiate(prettyGoodPrefab, this.transform.position, Quaternion.identity);
        }
        else if (type == AccuracyType.Distance3)
        {
            Instantiate(alrightPrefab, this.transform.position, Quaternion.identity);
        }
        else if (type == AccuracyType.Distance4)
        {
            Instantiate(crappyPrefab, this.transform.position, Quaternion.identity);
        }
        else if (type == AccuracyType.Distance5)
        {
            Instantiate(failPrefab, this.transform.position, Quaternion.identity);
        }*/

        index++;
    }

    #endregion

}
