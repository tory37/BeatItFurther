using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SongLoadAndPlay : MonoBehaviour {

	// Use this for initialization
    private Queue<string> songLines;
    private int millisecondsToWait;
    private NoteFactory nf;
    
    void Start () {
        songLines = new Queue<string>();
        nf = gameObject.GetComponent<NoteFactory>();
        loadSong(@".\Assets\Test Songs\TestSong.txt");
        playSong();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void loadSong(string filename)
    {
        FileStream fs = File.OpenRead(filename);
        StreamReader sr = new StreamReader(fs);
        millisecondsToWait = int.Parse(sr.ReadLine());
        string nextLine = sr.ReadLine();
        while(!sr.EndOfStream)
        {
            songLines.Enqueue(nextLine);
            nextLine = sr.ReadLine();
        }
        sr.Dispose();
        fs.Dispose();
    }

    void playSong()
    {
        StartCoroutine(play());
    }

    IEnumerator play()
    {
        float timeToWait = millisecondsToWait / 1000f;
        string nextLine;
        while(songLines.Count != 0)
        {
            yield return new WaitForSeconds(timeToWait);
            nextLine = songLines.Dequeue();
            if (nextLine[0].ToString() == "1") nf.spawnBlock(laneNumber.lane1, blockType.note);
            if (nextLine[1].ToString() == "1") nf.spawnBlock(laneNumber.lane2, blockType.note);
            if (nextLine[2].ToString() == "1") nf.spawnBlock(laneNumber.lane3, blockType.note);
            if (nextLine[3].ToString() == "1") nf.spawnBlock(laneNumber.lane4, blockType.note);
            if (nextLine[4].ToString() == "1") nf.spawnBlock(laneNumber.lane5, blockType.note);
        }

        Application.Quit();
    }


}
