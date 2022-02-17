using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ADBRuntime.UntiyEditor
{
    using Mono.Tool;
    public enum ChainGeneratorModeCN
    {
        //�Զ�ģʽ,
        DynamicBoneģʽ = 0,
        �ؼ���ģʽ = 1,
        ���ģʽ=2,
    }
    [CustomEditor(typeof(ADBChainGenerateTool))]

    public class ADBChainGeneratorEditor : Editor //OYM:�����������Editor
    {
        ADBChainGenerateTool controller;
        private bool isFoldout;
        private bool[] isFoldouts;
        private Dictionary<Object, Editor> containerEditors;

        public void OnEnable()
        {
            controller = target as ADBChainGenerateTool;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Titlebar("ADB����������", new Color(0.8F, 1, 1),0);
            controller.generatorMode = (ChainGeneratorMode)EditorGUILayout.EnumPopup("������ģʽ", (ChainGeneratorModeCN)controller.generatorMode);
            switch ((ChainGeneratorModeCN)controller.generatorMode)
            {
                case ChainGeneratorModeCN.DynamicBoneģʽ:
                    {
                        if (controller.setting == null)
                        {
                            Titlebar("����:�������ò���Ϊ��!", new Color(0.7f, 0.3f, 0.3f));
                        }
                        for (int i = 0; i < controller.generateTransformList?.Count; i++)
                        {
                            if (controller.generateTransformList[i]==null)
                            {
                                continue;
                            }
                            if (!controller.generateTransformList[i].gameObject.GetComponentsInParent<Transform>(true).Contains(controller.transform))//OYM:�����ڵ����Ϊ���ؽڵ���ӽڵ����
                            {
                                Titlebar("����:�ڵ� "+ controller.generateTransformList[i] .name+ "���ǹ��ؽڵ���ӽڵ����!", new Color(0.7f, 0.3f, 0.3f));
                            }
                        }
                    }
                    break;
                case ChainGeneratorModeCN.�ؼ���ģʽ:
                    {
                        if (controller.linker == null)
                        {
                            Titlebar("����:ȫ�ֹ������ò���Ϊ��!", new Color(0.7f, 0.3f, 0.3f));
                        }
                        if (controller.generateKeyWordWhiteList == null || controller.generateKeyWordWhiteList.Count == 0 && controller.linker.AllKeyWord.Count == 0)
                        {
                            Titlebar("����:ʶ��ؼ���ȱʧ", Color.yellow);
                        }
                        else if (controller.linker != null)
                        {
                            for (int i = 0; i < controller.generateKeyWordWhiteList.Count; i++)
                            {
                                if (!controller.linker.isContain(controller.generateKeyWordWhiteList[i]))
                                {
                                    Titlebar("����:�ؼ���: " + controller.generateKeyWordWhiteList[i] + "����ȫ�ֹ���������!", Color.yellow);
                                }
                            }
                        }
                    }
                    break;
                case ChainGeneratorModeCN.���ģʽ:
                    if (GUILayout.Button("������нڵ�����", GUILayout.Height(22.0f)))
                    {
                        ADBChainGenerateTool.ClearBoneChain(controller.transform);
                        containerEditors?.Clear();

                    }
                    return;
                default:
                    break;
            }

            Titlebar("=============== �ڵ�����", new Color(0.8f
                , 1, 1));
            switch ((ChainGeneratorModeCN)controller.generatorMode)
            {
                case ChainGeneratorModeCN.DynamicBoneģʽ:
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("setting"), new GUIContent("����Ч������"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("generateTransformList"), new GUIContent("������ʼ��"), true);
                    }
                    break;
                case ChainGeneratorModeCN.�ؼ���ģʽ:
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("linker"), new GUIContent("ȫ�ֹ�������"), true);
                        GUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKeyWordWhiteList"), new GUIContent("ʶ��ؼ���"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("blackListOfGenerateTransform"), new GUIContent("�ڵ������"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKeyWordBlackList"), new GUIContent("�ؼ��ʺ�����"), true);
                    }
                    break;
                default:
                    break;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("���ɽڵ�����", GUILayout.Height(22.0f)))
            {
                controller.InitializeChain();
                containerEditors?.Clear();
            }
            if (GUILayout.Button("������ɵĽڵ�����", GUILayout.Height(22.0f)))
            {
                controller.ClearBoneChain();
                containerEditors?.Clear();

            }
            EditorGUILayout.EndHorizontal();
            if (controller.allChain != null)
            {

                if (isFoldouts == null || isFoldouts.Length != controller.allChain.Count)
                {
                    isFoldouts = new bool[controller.allChain == null ? 0 : controller.allChain.Count];
                }
                isFoldout = EditorGUILayout.Foldout(isFoldout, "  ���нڵ����� :" + controller.GetPointCount());
                if (isFoldout)
                {
                    for (int i = 0; i < controller.allChain?.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();
                        isFoldouts[i] = EditorGUILayout.Foldout(isFoldouts[i], "Element " + i + ": Count:" + (controller.allChain[i].allPointTransforms == null ? 0 : controller.allChain[i].allPointTransforms.Length));
                        if (isFoldouts[i])
                        {
                            ShowContainerGUI(controller.allChain[i]);
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.Space(20);
            serializedObject.ApplyModifiedProperties();
        }
        void Titlebar(string text, Color color,int space =12)
        {
            GUILayout.Space(space);

            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(text);
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = backgroundColor;

            GUILayout.Space(3);
        }

        void ShowContainerGUI<T>(T container) where T : MonoBehaviour
        {
            if (container == null) return;
            if (containerEditors == null) containerEditors = new Dictionary<Object, Editor>();
            if (containerEditors.TryGetValue(container, out Editor editor))
            {
                editor.OnInspectorGUI();
            }
            else
            {
                containerEditors.Add(container, Editor.CreateEditor(container));
            }
        }
    }

}