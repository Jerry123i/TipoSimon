using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PontosHolderScript : MonoBehaviour {

    public int pontos;
    public int botoesMarcados;
    public int moedasMarcadas;
    public int gemasMarcadas;
    public List<GameObject> localBotoes;
    public List<GameObject> localMoedas;
    public List<GameObject> localPedras;
    public List<Sprite>     imagensDosBotoes;
    public List<Sprite> imagensDasMoedas;
    public List<Sprite> imagensDasGemas;

    public void AddAPoint()
    {
        pontos++;
        int r;
        

        if (botoesMarcados != localBotoes.Count)
        {
            r = Random.Range(0, imagensDosBotoes.Count);
            botoesMarcados++;
            localBotoes[botoesMarcados - 1].GetComponent<Image>().sprite = imagensDosBotoes[r];
            localBotoes[botoesMarcados - 1].SetActive(true);
        }

        else if (moedasMarcadas != localMoedas.Count)
        {
            r = Random.Range(0, imagensDasMoedas.Count);
            botoesMarcados = 0;
            localBotoes.ForEach(DeactivateThing);

            moedasMarcadas++;
            localMoedas[moedasMarcadas - 1].GetComponent<Image>().sprite = imagensDasMoedas[r];
            localMoedas[moedasMarcadas - 1].SetActive(true);
        }
        else if (gemasMarcadas != localPedras.Count)
        {
            r = Random.Range(0, imagensDasGemas.Count);
            botoesMarcados = 0;
            localBotoes.ForEach(DeactivateThing);
            moedasMarcadas = 0;
            localMoedas.ForEach(DeactivateThing);

            gemasMarcadas++;
            localPedras[gemasMarcadas - 1].GetComponent<Image>().sprite = imagensDasGemas[r];
            localPedras[gemasMarcadas-1].SetActive(true);
        }
    }

    public void DeactivateThing(GameObject thing)
    {
        thing.SetActive(false);
    }

}
