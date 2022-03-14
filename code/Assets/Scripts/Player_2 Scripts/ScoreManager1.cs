using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager1 : MonoBehaviour
{
    public static ScoreManager1 Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int comboScore1;
    static int missNotes1;

    void Start()
    {
        Instance = this;
        comboScore1 = 0;
        missNotes1 = 0;
    }
    public static void Hit()
    {
        comboScore1 += 1;
        Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        missNotes1++;
        Instance.missSFX.Play();    
    }
    private void Update()
    {
        scoreText.text = "Player 2 \nScore:" + comboScore1.ToString() + "\nMiss: " + missNotes1.ToString();;
    }
}
