using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class DynamicSelectList : MonoBehaviour, UILayer
{

	#region Editor Interface

    [SerializeField]
    private bool useFolder;
    [SerializeField]
    private string folderLocation;

	[SerializeField]
	private List<Button> selectableButtons;

	[SerializeField]
	private List<string> selectItems;

    [SerializeField]
    private string navigationAxisName;
    [SerializeField]
    private bool invertAxis;
    [SerializeField]
    private float firstTickMultiplier;

    [SerializeField, Tooltip("The length of time between each autoscroll jump")]
    private float autoScrollWaitTime;

	#endregion

	#region Private Fields

	private int currentSelectItemIndex;
	private int currentButtonIndex;

    private bool movingDown;
    private bool movingUp;

	#endregion

	#region Mono Methods
	
	private void Start()
	{
        if (useFolder)
        {
            selectItems = new List<string>();
            string path;
            if (Application.isEditor)
                path = @".\Assets\Resources";
            else
                path = @".\beatit_Data\Resources";

            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains(".dat") && !files[i].Contains(".meta"))
                {
                    if (Application.isEditor)
                        selectItems.Add(files[i].Replace(".\\Assets\\Resources\\", "").Replace(".dat", ""));
                    else
                        selectItems.Add(files[i].Replace(".\\beatit_Data\\Resources\\", "").Replace(".dat", ""));
                    
                    
                }
            }

        }

		ResetSelection();

        movingUp = movingDown = false;
	}

	private void Update()
	{
        float input = Input.GetAxis(navigationAxisName);

        if (!movingUp)
        {
            if (input > 0)
            {
                movingUp = true;
                StartCoroutine("MoveUpRoutine");
            }
        }
        else
        {
            if (input <= 0)
            {
                movingUp = false;
                StopCoroutine("MoveUpRoutine");
            }
        }

        if (!movingDown)
        {
            if (input < 0)
            {
                movingDown = true;
                StartCoroutine("MoveDownRoutine");
            }
        }
        else
        {
            if (input >= 0)
            {
                movingDown = false;
                StopCoroutine("MoveDownRoutine");
            }
        }
	}

	#endregion

	#region Private Methods

	private void ResetSelection()
	{
		if ( selectableButtons.Count > 0 )
		{
			currentSelectItemIndex = 0;
			currentButtonIndex = 0;

			selectableButtons[0].Select();

			FillInList( 0 );
		}
	}

	private void FillInList( int startingIndex )
	{
		int selectItemsIndex = startingIndex;

		for ( int i = 0; i < selectableButtons.Count; i++ )
		{
			if ( selectItems.Count > selectItemsIndex )
			{
				selectableButtons[i].GetComponentInChildren<Text>().text = selectItems[selectItemsIndex];
				selectItemsIndex++;
			}
			else
			{
				selectableButtons[i].GetComponentInChildren<Text>().text = "";
				selectItemsIndex++;
			}
		}
	}

	private void MoveDown()
	{
		//If we're at the bottom of the list
		if ( currentSelectItemIndex == selectItems.Count - 1 )
			return;

		// At bottom of list, but there are more items
		if (currentButtonIndex == selectableButtons.Count - 1)
		{
    		currentSelectItemIndex++;
            FillInList( currentSelectItemIndex - (selectableButtons.Count - 1) );
		}
		else
		{
			currentButtonIndex++;
            currentSelectItemIndex++;
			//EventSystem.current.SetSelectedGameObject( selectableButtons[currentButtonIndex].gameObject );
			selectableButtons[currentButtonIndex].Select();
		}
	}

	private void MoveUp()
	{
		//If we're at the top of the list
        if (currentSelectItemIndex == 0)
        {
            return;
        }

		// At top of list, but there are more items
		if ( currentButtonIndex == 0 && currentSelectItemIndex > 0)
		{
			currentSelectItemIndex--;
			FillInList( currentSelectItemIndex );
		}
        else
		{
			currentButtonIndex--;
            currentSelectItemIndex--;
            selectableButtons[currentButtonIndex].Select();
		}
	}

    private IEnumerator MoveUpRoutine()
    {
        if (!invertAxis)
            MoveUp();
        else
            MoveDown();
        yield return new WaitForSeconds(autoScrollWaitTime * firstTickMultiplier);

        while (true)
        {
            if (!invertAxis)
                MoveUp();
            else
                MoveDown();    
            yield return new WaitForSeconds(autoScrollWaitTime);
        }
    }

    private IEnumerator MoveDownRoutine()
    {
        if (!invertAxis)
            MoveDown();
        else
            MoveUp();
        yield return new WaitForSeconds(autoScrollWaitTime * firstTickMultiplier);

        while (true)
        {
            if (!invertAxis)
                MoveDown();
            else
                MoveUp();
            yield return new WaitForSeconds(autoScrollWaitTime);
        }
    }

	#endregion


	public void OnFocus()
	{
		throw new System.NotImplementedException();
	}
}
