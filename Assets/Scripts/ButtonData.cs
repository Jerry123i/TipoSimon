using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewButtonType", menuName = "Button Type", order = 1)]
public class ButtonData : ScriptableObject
{
    public Sprite foto;
    public string nomeDeExibicao;
    public string nomeComPronome;
    public AudioClip som;
}
