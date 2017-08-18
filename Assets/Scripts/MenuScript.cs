using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Image fadeWall;
    bool fadingIn;
    bool closing;
    bool startingClassic;
    bool startingGame;
    float clock;
        
    void Update()
    {
        if (fadingIn)
        {
            clock += Time.deltaTime;
            fadeWall.color = new Color(1, 1, 1, (1-clock));
            if(fadeWall.color.a <= 0)
            {
                fadeWall.color = new Color(1, 1, 1, 0);
                fadingIn = false;
                clock = 0;
            }
        }

        if (closing)
        {
            clock += Time.deltaTime;
            fadeWall.color = new Color(1, 1, 1, (clock / 1.5f));
            if (fadeWall.color.a >= 1)
            {
                fadeWall.color = new Color(1, 1, 1, 1);
                closing = false;
                clock = 0;
                Application.Quit();
            }
        }

        if (startingGame)
        {
            clock += Time.deltaTime;
            fadeWall.color = new Color(1, 1, 1, (clock / 1.5f));
            if (fadeWall.color.a >= 1)
            {
                fadeWall.color = new Color(1, 1, 1, 1);
                startingGame = false;
                clock = 0;
                SceneManager.LoadScene("cena1");
            }
        }

        if (startingClassic)
        {
            clock += Time.deltaTime;
            fadeWall.color = new Color(1, 1, 1, (clock / 1.5f));
            if (fadeWall.color.a >= 1)
            {
                fadeWall.color = new Color(1, 1, 1, 1);
                startingClassic = false;
                clock = 0;
                SceneManager.LoadScene("cenaClassicMode");
            }
        }

    }

	void Start () {
        fadingIn = true;
	}

    public void CloseGame()
    {
        closing = true;
    }
	
    public void GoToGame()
    {
        startingGame = true;
    }

    public void GoToGameClassic()
    {
        startingClassic = true;
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("menu");
    }

}
