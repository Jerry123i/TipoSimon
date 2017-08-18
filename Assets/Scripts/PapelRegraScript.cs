using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PapelRegraScript : MonoBehaviour {

    public TextMeshProUGUI texto;
    public bool tagedParaMudar;
    public Lei lei;
    
    public RectTransform rTransform;

    void Start()
    {
       // rTransform = this.gameObject.GetComponent<RectTransform>();
    }

    public void AtualizaTexto()
    {
        texto.text = lei.descricaoRegra;
    }

}
