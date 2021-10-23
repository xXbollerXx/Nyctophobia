using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mutebut : MonoBehaviour
{
    bool isMuted;
    public Sprite Muted, Unmute;
    public Image ButtonImage;
    public void Mute()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;

        if (isMuted)
        {
            ButtonImage.sprite = Muted;
        }
        else
        {
            ButtonImage.sprite = Unmute;

        }
    }
}
