using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class buttonScript : MonoBehaviour {
    
	public Image imagemPapel;
    public Image imagemFoto;

    [SerializeField]
    public ButtonData   tipoDeBotao;

    public Color targetColor;
	Color corBase;
	Color corPiscado;
	public bool aceso;

    public Sprite foto;
    public string nomeDeExibicao;
    public AudioSource som;

    float clock=0;

	void Start(){
        
        som = this.gameObject.GetComponent<AudioSource>();
		imagemPapel = this.gameObject.GetComponent<Image>();
		corBase = this.gameObject.GetComponent<Image>().color;
        corPiscado = targetColor;

        foto = tipoDeBotao.foto;
        nomeDeExibicao = tipoDeBotao.nomeDeExibicao;
        som.clip = tipoDeBotao.som;

        imagemFoto.sprite = foto;
	}

	void Update(){
		if(aceso){
			imagemPapel.color = corPiscado;
			clock+=Time.deltaTime;

			if(clock>=0.4f){
				clock=0;
				aceso=false;
				imagemPapel.color = corBase;
			}
		}
	}

	public void Acender(){		
		aceso=true;
        som.Play();
	}
}