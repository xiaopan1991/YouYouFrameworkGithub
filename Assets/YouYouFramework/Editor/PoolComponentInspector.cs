﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YouYouFramework
{
    [CustomEditor(typeof(PoolComponent), true)]
    public class PoolComponentInspector : Editor
    {
        //关联PoolComponent组件上的ClearInterval（释放池中对象的时间间隔）字段的值
        private SerializedProperty m_ClearInterval;

        //关联PoolComponent组件上的游戏对象池集合
        private SerializedProperty m_GameObjectPoolGroups;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            PoolComponent component = base.target as PoolComponent;

            //绘制滑动条
            int clearIntervalSlider = (int)EditorGUILayout.Slider("释放池中对象的间隔", m_ClearInterval.intValue, 10, 1800);
            if (clearIntervalSlider != m_ClearInterval.intValue)
            {
                component.ClearInterval = clearIntervalSlider;
            }
            //else
            //{
            //    m_ClearInterval.intValue = clearIntervalSlider;
            //}

            GUILayout.BeginHorizontal("box");
            GUILayout.Label("类名");
            GUILayout.Label("池中数量", GUILayout.Width(60));
            GUILayout.Label("常驻数量", GUILayout.Width(60));
            GUILayout.EndHorizontal();

            if (component != null && component.PoolManager != null)
            {
                foreach (var item in component.PoolManager.ClassObjectPool.InspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(item.Key.Name);
                    GUILayout.Label(item.Value.ToString(), GUILayout.Width(60));

                    int key = item.Key.GetHashCode();
                    byte resideCount = 0;
                    component.PoolManager.ClassObjectPool.ClassObjectCountDic.TryGetValue(key, out resideCount);
                    GUILayout.Label(resideCount.ToString(), GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.PropertyField(m_GameObjectPoolGroups, true);
            serializedObject.ApplyModifiedProperties();
            //重绘
            Repaint();
        }

        private void OnEnable()
        {
            //建立字段关联
            m_ClearInterval = serializedObject.FindProperty("ClearInterval");
            m_GameObjectPoolGroups = serializedObject.FindProperty("m_GameObjectPoolGroups");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
