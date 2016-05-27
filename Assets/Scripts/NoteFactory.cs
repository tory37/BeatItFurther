using UnityEngine;
using System.Collections;

public enum laneNumber
{
    lane1=0,
    lane2,
    lane3,
    lane4,
    lane5
}

public enum blockType
{
    note = 0,
    sustain
}

public class NoteFactory : MonoBehaviour {

    public float laneLength, laneWidth;

    [SerializeField]
    private GameObject blockPrototype, sustainPrototype;

    [SerializeField]
    private Material lane1, lane2, lane3, lane4, lane5;

    private Vector3 lane1Start, lane2Start, lane3Start, lane4Start, lane5Start;

	// Use this for initialization
	void Start () {
        Vector3 upperLeftPos = transform.position;
        lane1Start = new Vector3(upperLeftPos.x, 0.0f, upperLeftPos.z + laneLength);
        lane2Start = new Vector3(upperLeftPos.x + laneWidth, 0.0f, upperLeftPos.z + laneLength);
        lane3Start = new Vector3(upperLeftPos.x + 2 * laneWidth, 0.0f, upperLeftPos.z + laneLength);
        lane4Start = new Vector3(upperLeftPos.x + 3 * laneWidth, 0.0f, upperLeftPos.z + laneLength);
        lane5Start = new Vector3(upperLeftPos.x + 4 * laneWidth, 0.0f, upperLeftPos.z + laneLength);
	}
	
	public void spawnBlock(laneNumber blockLane, blockType type )
    {
        Vector3 toPlaceAt;
        GameObject newBlock;
        Material blockColor;

        if (type == blockType.note)
        {
            newBlock = Instantiate(blockPrototype);
        }
        else
        {
            newBlock = Instantiate(sustainPrototype);
        }

        switch(blockLane)
        {
            case laneNumber.lane1:
            {
                toPlaceAt = lane1Start;
                blockColor = lane1;
                break;
            }
            case laneNumber.lane2:
            {
                toPlaceAt = lane2Start;
                blockColor = lane2;
                break;
            }
            case laneNumber.lane3:
            {
                toPlaceAt = lane3Start;
                blockColor = lane3;
                break;
            }
            case laneNumber.lane4:
            {
                toPlaceAt = lane4Start;
                blockColor = lane4;
                break;
            }
            case laneNumber.lane5:
            {
                toPlaceAt = lane5Start;
                blockColor = lane5;
                break;
            }
            default:
            {
                toPlaceAt = Vector3.zero;
                blockColor = lane1;
                break;
            }
        }

        newBlock.transform.position = toPlaceAt;
        newBlock.GetComponent<Renderer>().material = blockColor;
    }
}
