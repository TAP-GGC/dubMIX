using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane1 : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; //Restrict note to certain key
    public KeyCode input; // Input for lane
    public GameObject notePrefab; // Note to spawn in
    List<Note> notes = new List<Note>(); // Manages list of notes to track all notes spawned
    public List<double> timeStamps = new List<double>(); // Every single time where user need to tap on the note

    int spawnIndex = 0; //Keep track of what timestamp need to be spawn
    int inputIndex = 0;// What timestamp need to be detected if player hitss that timestamp
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array) //Filters notes
        {
            if (note.NoteName == noteRestriction) //Checks note in array is equal to noteRestriction; if it is, we get note time
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager1.midiFile.GetTempoMap()); //Converts Midi file tempo time
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); // Converts Metric time to seconds
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager1.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager1.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager1.Instance.marginOfError;
            double audioTime = SongManager1.GetAudioSourceTime() - (SongManager1.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }       
    
    }
    private void Hit()
    {
        ScoreManager1.Hit();
    }
    private void Miss()
    {
        ScoreManager1.Miss();
    }
}
