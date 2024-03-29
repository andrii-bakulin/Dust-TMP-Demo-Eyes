﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuLookAt))]
    [CanEditMultipleObjects]
    public class DuLookAtEditor : DuEditor
    {
        private DuProperty m_TargetObject;
        private DuProperty m_UpVectorObject;

        private DuProperty m_UpdateMode;
        private DuProperty m_UpdateInEditor;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animation/LookAt")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("LookAt", typeof(DuLookAt));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");
            m_UpVectorObject = FindProperty("m_UpVectorObject", "Up Vector Object");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
            m_UpdateInEditor = FindProperty("m_UpdateInEditor", "Update In Editor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_TargetObject);
            PropertyField(m_UpVectorObject);

            Space();

            PropertyField(m_UpdateMode);
            PropertyFieldOrLock(m_UpdateInEditor, Application.isPlaying);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
