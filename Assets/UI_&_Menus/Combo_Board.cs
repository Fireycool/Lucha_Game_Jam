using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Combo_Board : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI comboText;
    public int combo;

    public void SetCombo (float playercombo){
        combo = (int)Math.Round(playercombo);
        comboText.text = combo.ToString();
    }
}
