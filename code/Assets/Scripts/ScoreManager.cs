using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int comboScore;
    static int missNotes;

    void Start()
    {
        Instance = this;
        comboScore = 0;
        missNotes = 0;
    }
    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        missNotes++;
        Instance.missSFX.Play();    
    }
    private void Update()
    {
        scoreText.text = "Player 1 \nScore:" + comboScore.ToString() + "\nMiss: " + missNotes.ToString();
    }
}
