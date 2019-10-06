#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UFORenderer))]
public class UFOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var target = (UFORenderer)serializedObject.targetObject;

        EditorGUILayout.Space();
        Vector2 result = EditorGUILayout.Vector2Field("Shot Speed Range", new Vector2(target.speedMultiplierMin, target.speedMultiplierMax));
        target.speedMultiplierMin = result.x;
        target.speedMultiplierMax = result.y;

        EditorGUILayout.Space();
        target.initialPosition = EditorGUILayout.Vector3Field("Initial Position", target.initialPosition);

        EditorGUILayout.Space();
        target.initialDirection = EditorGUILayout.Vector3Field("Shot Direction", target.initialDirection);
    }
}

#endif