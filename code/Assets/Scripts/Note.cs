using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; //Time the Note is supposed to be tapped by player

    public GameObject missEffect;

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2)); //Note time accounts time between spawn y and tap y

        
        if (t > 0.59) //Original: t>1
        {
            Instantiate(missEffect,transform.position,missEffect.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t); 
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
