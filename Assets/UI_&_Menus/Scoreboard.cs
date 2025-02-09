using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    public int score;

    public void SetScore (float playerscore){
        score = (int)Math.Round(playerscore);
        scoreText.text = score.ToString();
    }
}
