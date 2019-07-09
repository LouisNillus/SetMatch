using UnityEngine;
using UnityEditor;
using NS_Menu.Customize;

[CustomEditor(typeof(CustomizeMenu))]
public class FirstTry : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string pass = string.Empty;
        pass = GUILayout.PasswordField(pass, "-"[0], 25);

        if (GUILayout.Button("Generate"))
        {
            Debug.Log(pass);
        }

        GUILayout.Label("Hohoho");




        
    }
}
