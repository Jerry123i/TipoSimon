using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CiclosData))]
public class CiclosCustomEditor : Editor {

    public override void OnInspectorGUI()
    {
        CiclosData data = (CiclosData)target;

        if(data.unidades == null)
        {
            Debug.Log(data.name.ToString() + " Restarted");
            data.unidades = new List<UnidadeDeCiclo>();
            data.unidades.Add(new UnidadeDeCiclo());
            data.condInit = new CondicaoInicial(1);
            data.started = true;
        }

        EditorGUILayout.LabelField("Condição Inicial",EditorStyles.boldLabel);
        GUILayout.BeginHorizontal("Box");
        EditorGUILayout.PrefixLabel("Numero Inicial de Botoes:");
        data.condInit.nDeBotoes = int.Parse(GUILayout.TextField(data.condInit.nDeBotoes.ToString()));
        GUILayout.EndHorizontal();
                
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Regra:");
        data.condInit.regrasIniciais[0] =(TipoDeRegra)EditorGUILayout.EnumPopup(data.condInit.regrasIniciais[0]);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Regra:");
        data.condInit.regrasIniciais[1] = (TipoDeRegra)EditorGUILayout.EnumPopup(data.condInit.regrasIniciais[1]);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Regra:");
        data.condInit.regrasIniciais[2] = (TipoDeRegra)EditorGUILayout.EnumPopup(data.condInit.regrasIniciais[2]);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Unidades", EditorStyles.boldLabel);
        for(int i=0; i < data.unidades.Count; i++)
        {
            UnidadeDeCiclo uni = data.unidades[i];

            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal(EditorStyles.miniButton);
            uni.novaCor = GUILayout.Toggle(uni.novaCor, "Nova Cor");
            uni.novaRegra = GUILayout.Toggle(uni.novaRegra, "Nova Regra");

            if (uni.novaRegra)
            {
                uni.quantasRegras = int.Parse(GUILayout.TextField(uni.quantasRegras.ToString()));
            }

            uni.trocarRegra = GUILayout.Toggle(uni.trocarRegra, "Trocar Regra");

            if (GUILayout.Button("X", EditorStyles.miniButtonRight))
            {
                data.unidades.RemoveAt(i);
                Undo.RecordObject(data, "Remove Effect");
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }

        if (GUILayout.Button("Add", EditorStyles.miniButtonRight))
        {
            data.unidades.Add(new UnidadeDeCiclo());
            Undo.RecordObject(data, "Add Effect");
        }



    }

}
