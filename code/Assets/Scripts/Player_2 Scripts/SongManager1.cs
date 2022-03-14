using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager1 : MonoBehaviour
{
    public static SongManager1 Instance;
    public AudioSource audioSource; // Music
    public Lane1[] lanes;
    public float songDelayInSeconds; // 
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds; //For keyboard
    

    public string fileLocation; //Name of file
    public float noteTime; // How much time the note is on screen
    public float noteSpawnY; //Where note spawns
    public float noteTapY; //Where note can be tap by user
    public float noteDespawnY // Where note disappears if missed by user

    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile; //Where MiDI file going to load on RAM

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    //Specify file location
    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi(); //Function to manipulate MiDI
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes(); //Gets note data from MiDi file
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; 
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        Application.targetFrameRate = 60;  //Caps Framerate 
        audioSource.Play(); //Plays audio 
    }

    public static double GetAudioSourceTime() //Utility functions
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency; //Return Midi frequency for notes
    }

    void Update()
    {
        
    }
}
