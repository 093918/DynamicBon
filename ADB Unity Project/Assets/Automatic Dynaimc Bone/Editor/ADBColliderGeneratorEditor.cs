using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace ADBRuntime.UntiyEditor
{
    using Mono;
    using Mono.Tool;


    [CustomEditor(typeof(ADBColliderGenerateTool))]
    public class ADBColliderGeneratorEditor : Editor
    {
        private bool isDeleteCollider;
        private bool isGenerateColliderOpenTrigger;
        private ADBColliderGenerateTool controller;
        public void OnEnable()
        {
            controller = target as ADBColliderGenerateTool;

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Titlebar("���ν�ɫ��ײ��������", new Color(0.5F, 1, 1));
            if (controller.gameObject.GetComponent<Animator>()==null)
            {
                Titlebar("����: ��ǰ�ڵ���û�м�⵽Animator!", new Color(0.7f, 0.3f, 0.3f));
            }
            if (controller.gameObject.GetComponentsInChildren<ADBChainProcessor>() == null)
            {
                Titlebar("��ʾ:�����ɽڵ�����֮��������ײ�����Ӿ�ȷ.", Color.grey);
            }
            if (controller.gameObject.GetComponentsInChildren<ADBColliderReader>() .Length!=0 && (controller.generateColliderList == null || controller.generateColliderList.Count == 0))
            {
                Titlebar("��ʾ:��⵽һЩǱ�ڵ�collider,��ˢ������ʾ", Color.grey);
            }
            if (controller.generateColliderList!=null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("generateColliderList"), new GUIContent("��ײ���б� :" + controller.generateColliderList.Count), true);
            }



            string key = controller.isGenerateColliderAutomaitc ? "����" : "ˢ��";

            if (GUILayout.Button(key + "��ײ��", GUILayout.Height(22.0f)))
            {
                controller.initializeCollider();
            }
            if (controller.generateColliderList == null || controller.generateColliderList.Count == 0)
            {
                controller.isGenerateColliderAutomaitc = EditorGUILayout.Toggle("�Զ�����ȫ����ײ�� ", controller.isGenerateColliderAutomaitc);

                if (controller.isGenerateColliderAutomaitc)
                {
                    controller.colliderSize = EditorGUILayout.Slider("  �������ű��� ", controller.colliderSize, 0.001f, 2f);
                    controller.isGenerateColliderOpenTrigger = EditorGUILayout.Toggle("  �������ɵ���ײ��Ϊtrigger ", controller.isGenerateColliderOpenTrigger);
                    controller.isGenerateByAllPoint = EditorGUILayout.Toggle("  ���������нڵ���Ϊ���� ", controller.isGenerateByAllPoint);
                    controller.isGenerateFinger = EditorGUILayout.Toggle("  ����������ָ ", controller.isGenerateFinger);
                }
                if (controller.isGenerateColliderAutomaitc)
                {

                }
                if (controller.isGenerateColliderAutomaitc)
                {

                }
            }
            else
            {
/*                controller.colliderSize = EditorGUILayout.Slider("��ײ���С", controller. colliderSize, 0, 2);*/
            }
            if (GUILayout.Button("ɾ���������ɵ���ײ��", GUILayout.Height(22.0f)))
            {
                if (EditorUtility.DisplayDialog("��ȷ����Ҫɾ����?", "�ò������ɳ���", "ok", "cancel"))
                {
                    for (int i = 0; i < controller.generateColliderList?.Count; i++)
                    {
                        if (controller.generateColliderList[i] != null)
                        {
                            if (controller.generateColliderList[i].gameObject.GetComponents<Component>().Length <= 3)
                            {
                                DestroyImmediate(controller.generateColliderList[i].gameObject);
                            }
                            else
                            {
                                DestroyImmediate(controller.generateColliderList[i]);
                            }

                        }
                    }
                    controller.generateColliderList = null;

                    var overlapsColliderList = controller.transform.GetComponentsInChildren<Collider>();
                    if (isDeleteCollider)
                    {
                        for (int i = 0; i < overlapsColliderList.Length; i++)
                        {
                            if (overlapsColliderList[i] != null)
                            {
                                if (overlapsColliderList[i].gameObject.GetComponents<Component>().Length <= 3)
                                {
                                    DestroyImmediate(overlapsColliderList[i].gameObject);
                                }
                                else
                                {
                                    DestroyImmediate(overlapsColliderList[i]);
                                }
                            }
                        }
                        controller.generateColliderList.Clear();
                    }
                }
            }
            isDeleteCollider = EditorGUILayout.Toggle("  �������������Զ����ɵ���ײ�� ", isDeleteCollider);
            serializedObject.ApplyModifiedProperties();
        }



        void Titlebar(string text, Color color)
        {
            GUILayout.Space(12);

            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(text);
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = backgroundColor;

            GUILayout.Space(3);
        }
    }
}
