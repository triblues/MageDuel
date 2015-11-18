using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(launchScene))]
public class editorBtn : Editor {

    // Use this for initialization
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        launchScene myScript = (launchScene)target;
        if (GUILayout.Button("reset playerprefs"))
        {
            myScript.resetPlayerPrefs();
        }

        if (GUILayout.Button("add 100 coins"))
        {
            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 100);
        }
    }
}
