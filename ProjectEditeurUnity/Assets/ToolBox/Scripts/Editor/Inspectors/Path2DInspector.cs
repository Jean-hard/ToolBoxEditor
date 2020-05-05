using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using UnityEngine.UIElements;

[CustomEditor(typeof(Path2D))]
public class Path2DInspector : Editor
{
    private GUIStyle _pointLabelStyle = null;
    private ReorderableList _pointsReorderableList = null;

    private Tool _lastUsedTool = Tool.None;

    private void OnEnable()
    {
        _pointsReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"));
        _pointsReorderableList.drawElementCallback = _OnDrawPointsListElement;

        _pointLabelStyle = new GUIStyle();
        _pointLabelStyle.fontSize = 16;
        _pointLabelStyle.normal.textColor = Color.yellow;

        //Tools correspond aux outils en haut à gauche, ici on désactive le curseur de l'objet car inutile
        _lastUsedTool = Tools.current;
        Tools.current = Tool.None;
    }

    private void _OnDrawPointsListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, _pointsReorderableList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
    }

    private void OnDisable()
    {
        _pointsReorderableList = null;

        //On réactive le curseur pour les autres objects
        if (Tools.current == Tool.None)
            Tools.current = _lastUsedTool;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "points");

        _pointsReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    //Ne s'affiche que si je clique sur le gameObject
    private void OnSceneGUI()
    {
        Path2D path2D = target as Path2D;
        if (null == path2D) return;

        //Edit points
        for (int i = 0; i < path2D.points.Length; ++i)
        {
            Vector2 point = path2D.points[i];
            EditorGUI.BeginChangeCheck();
            //point = Handles.PositionHandle(point, Quaternion.identity);
            point = Handles.FreeMoveHandle(point, Quaternion.identity, 0.5f, Vector3.one, Handles.CircleHandleCap); //Change le curseur de déplacement
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path2D, "Edit Path Point " + (i + 1));
                path2D.points[i] = point;
            }
        }
        
        //Draw points
        Handles.color = Color.yellow;
        for(int i = 0; i < path2D.points.Length; ++i)
        {
            Vector2 point = path2D.points[i];
            Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);

            Vector2 labelPos = point;
            labelPos.y -= 0.2f;
            Handles.Label(labelPos, (i + 1).ToString(), _pointLabelStyle);

            if(i > 0)
            {
                Vector2 previousPoint = path2D.points[i - 1];
                Handles.DrawLine(previousPoint, point);
            }
        }

        //Relie le premier et le dernier point (loop)
        if(path2D.isLooping && path2D.points.Length > 1)
        {
            Handles.DrawLine(path2D.points[path2D.points.Length -1], path2D.points[0]);
        }

        //Disable Scene Interactions
        if(Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }
    }
}
