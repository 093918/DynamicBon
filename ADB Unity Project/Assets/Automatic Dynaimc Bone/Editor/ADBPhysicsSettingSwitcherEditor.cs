using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ADBRuntime.UntiyEditor
{
    using Mono;
    [CustomEditor(typeof(ADBPhysicsSettingSwitcher))]
    public class ADBPhysicsSettingSwitcherEditor : Editor
    {
        ADBPhysicsSettingSwitcher controller;
        public void OnEnable()
        {
            controller = (target as ADBPhysicsSettingSwitcher);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("runtimeController"), new GUIContent("�л�Ŀ��"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentLinker"), new GUIContent("��ǰ�����趨"), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetLinkers"), new GUIContent("�����趨�б�"), true);
            if (GUILayout.Button("�л�Ч��"))
            {
                controller.Switch();
            }
            serializedObject.ApplyModifiedProperties();
        }

    }

}
