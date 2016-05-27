using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public struct NoteDataHolder
{
    public NoteButton button;
    public NoteDirection direction;
    public float noteLength;
}

public class SongDataLoader {

    public int Tempo { get { return tempo; } }

	public float NoteSpeed { get { return noteSpeed; } }

    public string MP3Name { get; private set; }

    private Queue<NoteDataHolder> ReadNotes;
    private int tempo, beatsPerMeasure, fullBeatNoteType;
    private float noteSpeed,secsPerBeat;

    public SongDataLoader()
    {
        this.ReadNotes = new Queue<NoteDataHolder>();
    }

    public NoteDataHolder getNextNote()
    {
        if(ReadNotes.Count > 0)
        {
            return ReadNotes.Dequeue();
        }
        else
        {
            NoteDataHolder note = new NoteDataHolder();
            note.button = NoteButton.WAIT;
            note.direction = NoteDirection.NONE;
            note.noteLength = 0;
            return note;
        }
    }

    public float GetTimeToWait()
    {
        if(this.IsEmpty())
        {
            return 0;
        }
        else
        {
            return ReadNotes.Peek().noteLength;
        }
    }

    public bool IsEmpty()
    {
        if (ReadNotes.Count == 0)
            return true;
        return false;
    }


    private NoteDataHolder SongLineToNoteData(string fileLine, float secsPerBeat)
    {
        float numberOfBeatsToWait;
        string[] parts = fileLine.Split('.');
        if (parts[0].Contains("/"))
        {
            string[] timeToWaitParts = parts[0].Split('/');
            float numerator = float.Parse(timeToWaitParts[0]);
            float denominator = float.Parse(timeToWaitParts[1]);
            numberOfBeatsToWait = numerator / denominator;
        }
        else
        {
            numberOfBeatsToWait = float.Parse(parts[0]);
        }
        NoteDirection dir;
        NoteButton but;

        if (parts[1] != "")
        {
            dir = Note.CharToDirection(parts[1][0]);
            but = Note.CharToButton(parts[1][1]);
        }
        else
        {
            Debug.Log("Encountered a wait for " + numberOfBeatsToWait);
            dir = NoteDirection.NONE;
            but = NoteButton.WAIT;
        }

        NoteDataHolder note = new NoteDataHolder();
        note.button = but;
        note.direction = dir;
        note.noteLength = secsPerBeat * numberOfBeatsToWait;
        return note;
    }

    public void loadSong(string filename)
    {
        FileStream fs = File.OpenRead(filename);
        StreamReader sr = new StreamReader(fs);
        NoteDataHolder nextNote;

        //This is useful for debugging purposes
        int lineNumber = 0;

        string readString;
        //consume all blanks and comments before hte song name
        do
        {
            readString = sr.ReadLine();
            lineNumber++;
        } while (readString.StartsWith("//") || stringIsWhiteSpace(readString));
        MP3Name = readString;

        //this consumes all comments present before the tempo-line
        do
        {
            readString = sr.ReadLine();
            lineNumber++;
        } while (readString.StartsWith("//") || stringIsWhiteSpace(readString));
        tempo = int.Parse(readString);

        secsPerBeat = 60.0f / (float)tempo;

		//this consumes all comments present before the note speed line
		do
		{
			readString = sr.ReadLine();
			lineNumber++;
		} while ( readString.StartsWith( "//" ) || stringIsWhiteSpace( readString ) );
		noteSpeed = float.Parse( readString );

        //beatsPerMeasure = int.Parse(sr.ReadLine());
        //fullBeatNoteType = int.Parse(sr.ReadLine());

        while (!sr.EndOfStream)
        {
            //Trim leading and trailing whitespace from the input
            string nextLine = sr.ReadLine().Trim();
            lineNumber++;
            if (!nextLine.StartsWith("//") && !stringIsWhiteSpace(nextLine))
            {
                foreach (string noteData in nextLine.Split(' '))
                {
                    //try
                    //{
                        nextNote = SongLineToNoteData(noteData, secsPerBeat);
                        ReadNotes.Enqueue(nextNote);
                    //}catch(Exception e)
                    //{
                    //    Debug.Log(nextLine + " was formatted wrong");
                    //}
                    
                }
            }
        }

        sr.Dispose();
        fs.Dispose();
    }

    public static bool stringIsWhiteSpace(string str)
    {
        if (str == null) return true;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != ' ' && str[i] != '\n' && str[i] != '\t' && str[i] != '\r')
                return false;
        }
        return true;
    }
}
