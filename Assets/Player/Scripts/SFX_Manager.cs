using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;

    public AudioClip jump;
    public AudioClip parry;
    
    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }


}
