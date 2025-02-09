using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo_Bar_script : MonoBehaviour
{
    public Slider slider;

    public void SetMaxTimer(int timeleft){
        slider.maxValue = timeleft;
        slider.value = timeleft;
    }
    public void SetTimeLeft (int timeleft){
        slider.value = timeleft;
    }
}
