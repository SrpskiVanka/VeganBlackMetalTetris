using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public int diff;
    public TextMeshProUGUI difficultyLabel;

    public void OnClick()
    {
        diff++;
        diff = diff % 3;
        PlayerPrefs.SetInt("difficulty", diff);
        difficultyLabel.text = ((Difficulty) diff).ToString();
    }

    public void Start()
    {
        diff = PlayerPrefs.GetInt("difficulty");
        difficultyLabel.text = ((Difficulty) diff).ToString();
    }
}
