using UnityEngine;
using System.Collections;

public class QuickEasyFactoryLaneTester : MonoBehaviour {

    NoteFactory factory;
    // Start
    void Start ()
    {
        factory = gameObject.GetComponent<NoteFactory>();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) factory.spawnBlock(laneNumber.lane1, blockType.note);
        if (Input.GetKeyDown(KeyCode.W)) factory.spawnBlock(laneNumber.lane2, blockType.note);
        if (Input.GetKeyDown(KeyCode.E)) factory.spawnBlock(laneNumber.lane3, blockType.note);
        if (Input.GetKeyDown(KeyCode.R)) factory.spawnBlock(laneNumber.lane4, blockType.note);
        if (Input.GetKeyDown(KeyCode.T)) factory.spawnBlock(laneNumber.lane5, blockType.note);


	}
}
