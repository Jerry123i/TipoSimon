using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCycle", menuName = "Ciclo", order = 2)]
public class CiclosData : ScriptableObject {

    [SerializeField]
    public List<UnidadeDeCiclo> unidades;
    [SerializeField]
    public CondicaoInicial condInit;
    [SerializeField]
    public bool started;

    void OnEnable()
    {
       
    }
	
}
