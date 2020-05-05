using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeSpriteColor))]
public class ChangeSpriteColorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);    //Créé une box 
        EditorGUILayout.LabelField("Change Color Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();    //Ajoute un espace entre 2 property

        SerializedProperty changeModeProperty = serializedObject.FindProperty("changeMode");
        EditorGUILayout.PropertyField(changeModeProperty);   

        ChangeSpriteColor.CHANGE_MODE changeMode = (ChangeSpriteColor.CHANGE_MODE)changeModeProperty.intValue;
        if (changeMode == ChangeSpriteColor.CHANGE_MODE.CUSTOM)
        { 
            SerializedProperty customColorProperty = serializedObject.FindProperty("customColor");
            EditorGUILayout.PropertyField(customColorProperty);
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();    //Ajoute un espace entre 2 property

        if(changeMode == ChangeSpriteColor.CHANGE_MODE.RANDOM)
        {
            EditorGUILayout.HelpBox("Color will be randomize. Be careful !", MessageType.Info);
        }

        if (GUILayout.Button("Change Color"))
        {
            ChangeSpriteColor changeColorScript = target as ChangeSpriteColor;

            switch (changeMode)
            {
                case ChangeSpriteColor.CHANGE_MODE.RANDOM:
                    Color randomColor = new Color();
                    randomColor.r = Random.Range(0f, 1f);
                    randomColor.g = Random.Range(0f, 1f);
                    randomColor.b = Random.Range(0f, 1f);
                    randomColor.a = 1f;
                    changeColorScript.GetComponent<SpriteRenderer>().color = randomColor;
                    break;

                case ChangeSpriteColor.CHANGE_MODE.CUSTOM:
                    changeColorScript.GetComponent<SpriteRenderer>().color = changeColorScript.customColor;
                    break;
            }
            
        }
    }
}
