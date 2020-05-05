using ToolboxEngine;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MoveAround2D))]
public class MoveAround2DInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Move Around Settings", EditorStyles.boldLabel);
        SerializedProperty radiusProperty = serializedObject.FindProperty("_radius");
        EditorGUILayout.PropertyField(radiusProperty);
        SerializedProperty speedProperty = serializedObject.FindProperty("speed");
        EditorGUILayout.PropertyField(speedProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Debug GUI Settings", EditorStyles.boldLabel);
        SerializedProperty guiDebugProperty = serializedObject.FindProperty("guiDebug");
        EditorGUILayout.PropertyField(guiDebugProperty);
        if (guiDebugProperty.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("guiFontSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("guiDebugTextColor"));
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Gizmos GUI Settings", EditorStyles.boldLabel);
        SerializedProperty gizmosDebugProperty = serializedObject.FindProperty("gizmosDebug");
        EditorGUILayout.PropertyField(gizmosDebugProperty);
        if(gizmosDebugProperty.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosCenterColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosPositionColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosLineColor"));
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
    private static void DrawGizmos(MoveAround2D moveScript, GizmoType gizmoType)
    {
        if (!moveScript.gizmosDebug) return;

        //Draw center 
        Gizmos.color = moveScript.gizmosCenterColor;
        Gizmos.DrawWireSphere(moveScript.GetCenter(), moveScript.gizmosSize);

        //Draw destination
        Gizmos.color = moveScript.gizmosPositionColor;
        Gizmos.DrawWireSphere(moveScript.transform.position, moveScript.gizmosSize);

        //Draw line between center and destination
        Gizmos.color = moveScript.gizmosLineColor;
        Gizmos.DrawLine(moveScript.GetCenter(), moveScript.transform.position);
    }
}
