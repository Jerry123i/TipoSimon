using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum TipoDeRegra{Nenhuma,Ignore,Dobre,Substitua};

public class DirectorScript : MonoBehaviour
{

    [SerializeField]
    public List<ButtonData> botaoTypeList;
    [SerializeField]
    public List<ButtonData> secretButtons;
    [SerializeField]
    public List<CiclosData> ciclos;
    
    public List<GameObject> quatroBotoes, fitaBase, fitaEsperada;
    public GameObject endButton;
    public PapelRegraScript[] objetosRegras;
    public PontosHolderScript pontosHolder;

    int vidas;
    public List<GameObject> vidasLista;

    public AudioClip audioOhNo;
    public AudioClip audioOK;
    private AudioSource audioSource;

    int fase;
    int pontos;
    int unidadeDoCiclo;
    int agulha;

    public bool classicMode;

    public TextMeshProUGUI endGameText;
    public Image fadeWall;
    
    float clock;
    bool lendo;
    bool endingGame;
    bool trocandoRegras;
    int inversorDeTroca=1;

    Lei[] regras;

    void Update()
    {       
        if (trocandoRegras)
        {
            ToggleQuatroBotoes(false);
            TrocarRegras();
        }

        if (lendo)
        {
            ToggleQuatroBotoes(false);
            LeAFita();
        }
        if (endingGame)
        {
            clock += Time.deltaTime;
            fadeWall.color = new Color(1, 1, 1, (clock / 2f));
            if (fadeWall.color.a >= 1)
            {
                fadeWall.color = new Color(1, 1, 1, 1);
                endingGame = false;
                clock = 0;
            }
        }
    }


    void Start()
    {                
        //Sorteia e inicia os botoes
        EscolheEIniciaBotoes();

        vidas = 3;
        clock = 0;
        agulha = 0;
        fase = 0;
        unidadeDoCiclo = -1;
        audioSource = this.GetComponent<AudioSource>();

        regras = new Lei[3];

        regras[0] = new Lei(TipoDeRegra.Nenhuma);
        regras[1] = new Lei(TipoDeRegra.Nenhuma);
        regras[2] = new Lei(TipoDeRegra.Nenhuma);

        objetosRegras[0].lei = regras[0];
        objetosRegras[1].lei = regras[1];
        objetosRegras[2].lei = regras[2];

        Inicia(ciclos[fase].condInit);
        
    }

    void TrocarRegras()
    {
        for (int i = 0; i < objetosRegras.Length; i++)
        {
            if (objetosRegras[i].tagedParaMudar)
            {
                objetosRegras[i].rTransform.pivot = new Vector2(objetosRegras[i].rTransform.pivot.x, objetosRegras[i].rTransform.pivot.y - 0.07f * inversorDeTroca);

                if (objetosRegras[i].rTransform.pivot.y <= -3.0f && inversorDeTroca == 1)
                {
                    for (int k = 0; k<objetosRegras.Length;k++)
                    {
                        objetosRegras[k].AtualizaTexto();
                    }                    
                    inversorDeTroca = -1;                   
                }
                else if(inversorDeTroca == -1 && objetosRegras[i].rTransform.pivot.y >= 0.5f)
                {

                    for (int k = 0; k<objetosRegras.Length;k++)
                    {
                        objetosRegras[k].tagedParaMudar = false;
                        objetosRegras[k].rTransform.pivot = new Vector2(objetosRegras[k].rTransform.pivot.x, 0.5f);
                    }     
                    inversorDeTroca = 1;                    
                    trocandoRegras = false;
                    lendo = true;
                }               
            }
        }
    }

    void LeAFita()
    {
        clock += Time.deltaTime;

        if (clock >= 0.65f)
        {
            clock = 0;
            fitaBase[agulha].GetComponent<buttonScript>().Acender();
            agulha++;

            if (agulha == fitaBase.Count)
            {
                lendo = false;
                ToggleQuatroBotoes(true);
                quatroBotoes[0].GetComponent<Button>().interactable = true;
                quatroBotoes[1].GetComponent<Button>().interactable = true;
                quatroBotoes[2].GetComponent<Button>().interactable = true;
                quatroBotoes[3].GetComponent<Button>().interactable = true;
                agulha = 0;

            }
        }
    }

    void Inicia(CondicaoInicial c)
    {
        fitaBase.Clear();

        for (int i = 0; i < regras.Length; i++)
        {
            regras[i] = new Lei(TipoDeRegra.Nenhuma);
        }

        for (int i = 0; i < c.nDeBotoes; i++)
        {
            AdicionaNovaCor();
        }

        for (int i = 0; i < c.nDeRegrasIniciaisASeremSorteadas; i++)
        {
            regras[i] = SorteiaRegra();
        }

        for (int i = 0; i < c.regrasIniciais.Length; i++)
        {
            if (c.regrasIniciais[i] != TipoDeRegra.Nenhuma)
            {
                regras[i] = SorteiaObjetosParaUmaRegra(c.regrasIniciais[i]);
            }
        }
        AtualizaAFitaEsperada();
    }

    void AdicionaNovaCor()
    {
        fitaBase.Add(quatroBotoes[Random.Range(0, 4)]);
    }

    void AdicionaNovaRegraERemoveAMaisAntiga()
    {
        regras[2] = regras[1];
        regras[1] = regras[0];
        regras[0] = new Lei(TipoDeRegra.Nenhuma);
        regras[0] = SorteiaRegra();
    }

    void AtualizaAFitaEsperada()
    {
        fitaEsperada.Clear();
        for (int i = 0; i < fitaBase.Count; i++)
        {
            fitaEsperada.Add(fitaBase[i]);
        }

        //Verifica regras em ordem
        for (int i = 0; i < regras.Length; i++)
        {
            if (regras[i].qualRegra == TipoDeRegra.Ignore)
            {
                IgnoraCor(fitaEsperada, regras[i].objetoUm);
            }
        }

        for (int i = 0; i < regras.Length; i++)
        {
            if (regras[i].qualRegra == TipoDeRegra.Dobre)
            {
                DobraCor(fitaEsperada, regras[i].objetoUm);
            }
        }

        for (int i = 0; i < regras.Length; i++)
        {
            if (regras[i].qualRegra == TipoDeRegra.Substitua)
            {
                SubstituiCor(fitaEsperada, regras[i].objetoUm, regras[i].listaObjetos);
            }
        }

        fitaEsperada.Add(endButton);
        AtualizaTextoDeRegras();
    }

    void AtualizaTextoDeRegras()
    {
       bool[] respostasBool;
       bool[] objetosBool;

       List<PapelRegraScript> papeisParaMudar;
       papeisParaMudar = new List<PapelRegraScript>();

       List<PapelRegraScript> papeisTempList;
       papeisTempList = new List<PapelRegraScript>();

       respostasBool = new bool[3]{false, false, false};
       objetosBool = new bool[3]{false, false, false};
        
    

       for (int i = 0; i<objetosBool.Length;i++)
       {
            papeisTempList.Add(objetosRegras[i]);
       }

       for (int i = 0; i<regras.Length; i++)
       {
            if(papeisTempList.Count>0)
            {
                for (int j = papeisTempList.Count-1;j>=0;j--)
                {
                    if(respostasBool[i]==false)
                    {
                        if(papeisTempList[j].lei.descricaoRegra==regras[i].descricaoRegra && !objetosBool[j])
                        {    
                            Debug.Log("ping " + "i:" + i.ToString() + " j:" + papeisTempList[j].lei.descricaoRegra);                        
                            respostasBool[i] = true;
                            objetosBool[j] = true;
                            //papeisTempList.RemoveAt(j);
                            Debug.Log(objetosBool[j].ToString() + j.ToString() + " (Deve ser true)" ); //J dando sempre igual a 0
                        }
                    }
                }
            }
       }

        for (int i =0; i<objetosRegras.Length; i++)
        {
            Debug.Log(objetosBool[i].ToString() + " " + i.ToString());
            if(!objetosBool[i])
            {
                papeisParaMudar.Add(objetosRegras[i]);
            }
        }

        int contador=0;

        for (int i=0; i<regras.Length;i++)
        {
            if(!respostasBool[i])
            {
                if(contador>papeisParaMudar.Count)
                {
                    Debug.Log("Erro");
                    Debug.Break();
                }
                else
                {
                    Debug.Log(regras[i].descricaoRegra);
                    papeisParaMudar[contador].lei = regras[i];
                    papeisParaMudar[contador].tagedParaMudar = true;
                    contador++;
                }
            }    
        }

        if (papeisParaMudar.Count > 0)
        {
            Debug.Log("entrou no trocando regras");
            trocandoRegras = true;
        }

        else
        {
            Debug.Log("ping lendo is true");
            lendo = true;
        }
        
        
    }

    void AdicionaAFita(UnidadeDeCiclo unidade)
    {
        if (unidade.novaCor)
        {
            AdicionaNovaCor();
        }
        if (unidade.novaRegra)
        {
            for (int i =0; i< unidade.quantasRegras; i++)
            {
                Debug.Log("Nova regra");
                AdicionaNovaRegraERemoveAMaisAntiga();
            }
        }
        if (unidade.trocarRegra)
        {
            Debug.Log("troca regra");
            List<Lei> trocasPossiveis;
            trocasPossiveis = new List<Lei>();

            Lei pointer;

            for(int i =0; i < regras.Length; i++)
            {
                if(regras[i].qualRegra != TipoDeRegra.Nenhuma)
                {
                    trocasPossiveis.Add(regras[i]);
                }
                if (trocasPossiveis.Count != 0)
                {
                    pointer = trocasPossiveis[(Random.Range(0, trocasPossiveis.Count))];
                    for (int j = 0; j < regras.Length; j++)
                    {
                        if (regras[i] == pointer)
                        {
                            regras[i] = SorteiaRegra();
                        }
                    }
                }
            }
        }

        AtualizaAFitaEsperada();

    }

    public void ReceberInput(GameObject botaoApertado)
    {
        ToggleQuatroBotoes(true);

        if (botaoApertado == fitaEsperada[agulha])
        {
            if (fitaEsperada[agulha].GetComponent<buttonScript>() != null)
            {
                fitaEsperada[agulha].GetComponent<buttonScript>().Acender();
            }
            
            agulha++;
            Debug.Log("OK");
            if (agulha == fitaEsperada.Count)
            {
                pontos++;
                audioSource.clip = audioOK;
                audioSource.Play();
                agulha = 0;
                unidadeDoCiclo++;

                if (classicMode)
                {
                    Debug.Log("classic");
                    ciclos[fase].unidades.Add(new UnidadeDeCiclo());
                    pontosHolder.AddAPoint();
                }   

                if (unidadeDoCiclo == ciclos[fase].unidades.Count) //caso tenha terminado esse ciclo
                {
                    fase++;
                    pontosHolder.AddAPoint();
                    unidadeDoCiclo = -1;

                        if(fase == ciclos.Count) //Adiciona um ciclo extra caso os pre escolhidos tenham acabado
                            {
                                ciclos.Add(ciclos[fase-1]);
                            }

                    Inicia(ciclos[fase].condInit);
                }
                else
                {
                    AdicionaAFita(ciclos[fase].unidades[unidadeDoCiclo]);
                }
            }
        }
        else {
            vidas--;
            if (vidas < 0)
            {
                fadeWall.gameObject.SetActive(true);
                endGameText.text = pontos.ToString() + " respostas corretas \n" + fase.ToString() + " ciclos terminados";
                audioSource.clip = audioOhNo;
                audioSource.Play();
                endingGame = true;
            }
            else
            {
                Debug.Log(vidasLista[vidas]);
                vidasLista[vidas].SetActive(false);
                audioSource.clip = audioOhNo;
                audioSource.Play();
                agulha = 0;
                lendo = true;
            }            
        }
    }

    void EscolheEIniciaBotoes()
    {
        for (int i = 0; i < quatroBotoes.Count; i++)
        {
            int r = Random.Range(0, 200);
            if (r == 0)
            {
                r = Random.Range(0, secretButtons.Count);
                quatroBotoes[i].GetComponent<buttonScript>().tipoDeBotao = secretButtons[r];
                secretButtons.RemoveAt(r);
            }
            else
            {
                r = Random.Range(0, botaoTypeList.Count);
                quatroBotoes[i].GetComponent<buttonScript>().tipoDeBotao = botaoTypeList[r];
                botaoTypeList.RemoveAt(r);
            }
            quatroBotoes[i].SetActive(true);
        }
    }

    void DobraCor(List<GameObject> lista, GameObject coisaASerDobrada)
    {
        List<GameObject> placeHolder;
        placeHolder = new List<GameObject>();

        for (int i = 0; i < lista.Count; i++)
        {
            placeHolder.Add(lista[i]);
            if (lista[i].name == coisaASerDobrada.name)
            {
                placeHolder.Add(lista[i]);
            }
        }

        lista.Clear();
        for (int i = 0; i < placeHolder.Count; i++)
        {
            lista.Add(placeHolder[i]);
        }
    }

    void IgnoraCor(List<GameObject> lista, GameObject coisaASerIgnorada)
    {
        for (int i = lista.Count - 1; i >= 0; i--)
        {
            if (lista[i].name == coisaASerIgnorada.name)
            {
                lista.RemoveAt(i);
            }
        }
    }

    void SubstituiCor(List<GameObject> lista, GameObject coisaASerSubstituida, List<GameObject> coisaPraSubstituir)
    {
        List<GameObject> placeHolder;
        placeHolder = new List<GameObject>();

        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].name == coisaASerSubstituida.name)
            {
                for (int j = 0; j < coisaPraSubstituir.Count; j++)
                {
                    placeHolder.Add(coisaPraSubstituir[j]);
                }
            }
            else {
                placeHolder.Add(lista[i]);
            }
        }
        lista.Clear();
        for (int i = 0; i < placeHolder.Count; i++)
        {
            lista.Add(placeHolder[i]);
        }
    }

    public Lei SorteiaRegra()
    {
        List<GameObject> opcoesValidas;
        opcoesValidas = new List<GameObject>();

        int controleDeExcessoDeSubstituicao = 4; //Alerta Gambiarra

        for (int i = 0; i < quatroBotoes.Count; i++)
        {
            opcoesValidas.Add(quatroBotoes[i]);
        }

        for (int i = 0; i < regras.Length; i++)
        {
            if (regras[i].qualRegra == TipoDeRegra.Substitua)
            {
                controleDeExcessoDeSubstituicao = 3;
            }
        }

        switch (Random.Range(1, controleDeExcessoDeSubstituicao))
        {
            case 1: //Ignora                
                for (int i = 0; i < regras.Length; i++)
                {
                    if (regras[i].qualRegra == TipoDeRegra.Dobre || regras[i].qualRegra == TipoDeRegra.Ignore)
                    {
                        opcoesValidas.Remove(regras[i].objetoUm);
                    }
                }
                if (opcoesValidas.Count == 0)
                {
                    Debug.Log("Zero opções válidas encontradas para regra");
                }
                return new Lei(TipoDeRegra.Ignore, opcoesValidas[Random.Range(0, (opcoesValidas.Count))]);

            case 2: //Dobra
                for (int i = 0; i < regras.Length; i++)
                {
                    if (regras[i].qualRegra == TipoDeRegra.Dobre || regras[i].qualRegra == TipoDeRegra.Ignore)
                    {
                        opcoesValidas.Remove(regras[i].objetoUm);
                    }
                }
                if (opcoesValidas.Count == 0)
                {
                    Debug.Log("Zero opções válidas encontradas para regra");
                }
                return new Lei(TipoDeRegra.Dobre, opcoesValidas[Random.Range(0, (opcoesValidas.Count))]);

            case 3: //Substitui
                int a;
                int b;

                a = Random.Range(0, opcoesValidas.Count);
                do
                {
                    b = Random.Range(0, opcoesValidas.Count);
                } while (b == a);

                return new Lei(TipoDeRegra.Substitua, opcoesValidas[a], new List<GameObject> { opcoesValidas[b] });

            default:
                Debug.Log("Sorteio de regra defaultou");
                return null;

        }
    }

    public Lei SorteiaObjetosParaUmaRegra(TipoDeRegra tipo)
    {
        List<GameObject> opcoesValidas;
        opcoesValidas = new List<GameObject>();

        for (int i = 0; i < quatroBotoes.Count; i++)
        {
            opcoesValidas.Add(quatroBotoes[i]);
        }

        if (tipo == TipoDeRegra.Substitua)
        {
            for (int i = 0; i < regras.Length; i++)
            {
                if (regras[i].qualRegra == TipoDeRegra.Substitua)
                {
                    Debug.Log("Duas substituicoes encontradas, alterando tipo de regra");
                    tipo = TipoDeRegra.Dobre;
                }
            }
        }

        switch (tipo)
        {
            case TipoDeRegra.Ignore: //Ignora                
                for (int i = 0; i < regras.Length; i++)
                {
                    if (regras[i].qualRegra == TipoDeRegra.Dobre || regras[i].qualRegra == TipoDeRegra.Ignore)
                    {
                        opcoesValidas.Remove(regras[i].objetoUm);
                    }
                }
                if (opcoesValidas.Count == 0)
                {
                    Debug.Log("Zero opções válidas encontradas para regra");
                }
                return new Lei(TipoDeRegra.Ignore, opcoesValidas[Random.Range(0, (opcoesValidas.Count))]);

            case TipoDeRegra.Dobre: //Dobra
                for (int i = 0; i < regras.Length; i++)
                {
                    if (regras[i].qualRegra == TipoDeRegra.Dobre || regras[i].qualRegra == TipoDeRegra.Ignore)
                    {
                        opcoesValidas.Remove(regras[i].objetoUm);
                    }
                }
                if (opcoesValidas.Count == 0)
                {
                    Debug.Log("Zero opções válidas encontradas para regra");
                }
                return new Lei(TipoDeRegra.Dobre, opcoesValidas[Random.Range(0, (opcoesValidas.Count))]);

            case TipoDeRegra.Substitua: //Substitui
                int a;
                int b;

                a = Random.Range(0, opcoesValidas.Count);
                do
                {
                    b = Random.Range(0, opcoesValidas.Count);
                } while (b == a);

                return new Lei(TipoDeRegra.Substitua, opcoesValidas[a], new List<GameObject> { opcoesValidas[b] });

            default:
                Debug.Log("Sorteio de regra defaultou");
                return null;
        }
    }

    public void ToggleQuatroBotoes(bool ligaDesliga)
    {
        for (int i = 0; i<quatroBotoes.Count; i++)
        {
            quatroBotoes[i].GetComponent<Button>().interactable = ligaDesliga;
        }
    }

}

    public class Lei
    {
        public GameObject objetoUm;
        public List<GameObject> listaObjetos;
        public TipoDeRegra qualRegra;

        public string descricaoRegra;

        public Lei(TipoDeRegra regra, GameObject obj)
        {
            qualRegra = regra;
            objetoUm = obj;
            listaObjetos = null;
            descricaoRegra = (qualRegra.ToString() + " " + objetoUm.GetComponent<buttonScript>().tipoDeBotao.nomeDeExibicao);
        }

        //Para substituicao
        public Lei(TipoDeRegra regra, GameObject obj, List<GameObject> lista)
        {
            qualRegra = regra;
            objetoUm = obj;
            listaObjetos = lista;
            descricaoRegra = (qualRegra.ToString() + " " + objetoUm.GetComponent<buttonScript>().tipoDeBotao.nomeDeExibicao + " " + listaObjetos[0].GetComponent<buttonScript>().tipoDeBotao.nomeComPronome);
        }

        //Para nenhuma
        public Lei(TipoDeRegra regra)
        {
            qualRegra = regra;
            objetoUm = null;
            listaObjetos = null;
        }

    }

    [System.Serializable]
    public class UnidadeDeCiclo
    {
        public bool novaCor;
        public bool novaRegra;
        public int quantasRegras;
        public bool trocarRegra;
        public bool isOrientada;

        public TipoDeRegra regraEspecifica;

        public UnidadeDeCiclo()
        {
            novaCor = true;
            novaRegra = false;
            quantasRegras = 0;
            trocarRegra = false;
            isOrientada = false;
        }

        public UnidadeDeCiclo(bool cor, bool regra, bool trocar)
        {
            novaCor = cor;
            novaRegra = regra;
            quantasRegras = 1;
            trocarRegra = trocar;
            isOrientada = true;
        }

       public UnidadeDeCiclo(bool cor, bool regra, bool trocar, int n)
       {
            novaCor = cor;
            novaRegra = regra;
            quantasRegras =n;
            trocarRegra = trocar;
            isOrientada = true;
        }

    public UnidadeDeCiclo(bool cor, bool regra, bool trocar, bool orientacao)
        {
            novaCor = cor;
            novaRegra = regra;
            trocarRegra = trocar;
            isOrientada = orientacao;
        }
    }

    [System.Serializable]
    public class CondicaoInicial
    {
        public int nDeBotoes;
        public TipoDeRegra[] regrasIniciais;
        public int nDeRegrasIniciaisASeremSorteadas;

        void iniciarRegras()
        {
            regrasIniciais = new TipoDeRegra[3];
            regrasIniciais[0] = TipoDeRegra.Nenhuma;
            regrasIniciais[1] = TipoDeRegra.Nenhuma;
            regrasIniciais[2] = TipoDeRegra.Nenhuma;
        }

        public CondicaoInicial(int botoes)
        {
            iniciarRegras();
            nDeBotoes = botoes;
            nDeRegrasIniciaisASeremSorteadas = 0;
        }

        public CondicaoInicial(int botoes, int regras)
        {
            iniciarRegras();
            nDeBotoes = botoes;
            nDeRegrasIniciaisASeremSorteadas = regras;
        }

        public CondicaoInicial(int botoes, TipoDeRegra regra0)
        {
            iniciarRegras();
            nDeBotoes = botoes;
            nDeRegrasIniciaisASeremSorteadas = 0;
            regrasIniciais[0] = regra0;

        }

        public CondicaoInicial(int botoes, TipoDeRegra regra0, TipoDeRegra regra1)
        {
            iniciarRegras();
            nDeBotoes = botoes;
            nDeRegrasIniciaisASeremSorteadas = 0;
            regrasIniciais[0] = regra0;
            regrasIniciais[1] = regra1;
        }
        public CondicaoInicial(int botoes, TipoDeRegra regra0, TipoDeRegra regra1, TipoDeRegra regra2)
        {
            iniciarRegras();
            nDeBotoes = botoes;
            nDeRegrasIniciaisASeremSorteadas = 0;
            regrasIniciais[0] = regra0;
            regrasIniciais[1] = regra1;
            regrasIniciais[2] = regra2;

        }

    }
