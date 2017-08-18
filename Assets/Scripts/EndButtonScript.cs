using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButtonScript : MonoBehaviour {

    public Image image;
    public Sprite botao1, botao2;
    public float delayTime;

    float clock;
    public bool pressed;

    public DirectorScript directorScript;

    void Update()
    {
        if (pressed)
        {
            clock += Time.deltaTime;
            image.sprite = botao2;

            if (clock >= delayTime)
            {
                pressed = false;
                clock = 0;
                directorScript.ReceberInput(this.gameObject);
            }
        }
        else
        {
            image.sprite = botao1;
        }
    }

    public void PressButton()
    {
        pressed = true;
    }

}

