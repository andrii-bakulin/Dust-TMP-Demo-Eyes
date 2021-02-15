﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpaceGizmo))]
    [CanEditMultipleObjects]
    public class DuFieldsSpaceGizmoEditor : DuGizmoObjectEditor
    {
        private DuProperty m_FieldsSpace;

        private DuProperty m_GridCount;
        private DuProperty m_GridStep;

        private DuProperty m_PowerVisible;
        private DuProperty m_PowerSize;
        private DuProperty m_PowerDotsVisible;
        private DuProperty m_PowerDotsSize;
        private DuProperty m_PowerDotsColor;
        private DuProperty m_PowerImpactOnDotsSize;

        private DuProperty m_ColorVisible;
        private DuProperty m_ColorSize;
        private DuProperty m_PowerImpactOnColorSize;
        private DuProperty m_ColorAllowTransparent;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Fields Space")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(DuFieldsSpaceGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableGizmo();

            m_FieldsSpace = FindProperty("m_FieldsSpace", "Fields Space");

            m_GridCount = FindProperty("m_GridCount", "Grid Count");
            m_GridStep = FindProperty("m_GridStep", "Grid Step");

            m_PowerVisible = FindProperty("m_PowerVisible", "Visible");
            m_PowerSize = FindProperty("m_PowerSize", "Size");
            m_PowerImpactOnDotsSize = FindProperty("m_PowerImpactOnDotsSize", "Change Size by Power");
            m_PowerDotsVisible = FindProperty("m_PowerDotsVisible", "Dots Visible");
            m_PowerDotsSize = FindProperty("m_PowerDotsSize", "Dots Size");
            m_PowerDotsColor = FindProperty("m_PowerDotsColor", "Dots Color");

            m_ColorVisible = FindProperty("m_ColorVisible", "Visible");
            m_ColorSize = FindProperty("m_ColorSize", "Size");
            m_PowerImpactOnColorSize = FindProperty("m_PowerImpactOnColorSize", "Change Size by Power");
            m_ColorAllowTransparent = FindProperty("m_ColorAllowTransparent", "Allow Transparent");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_FieldsSpace);

            Space();

            if (DustGUI.FoldoutBegin("Grid", "DuFieldsSpaceGizmo.Grid"))
            {
                PropertyField(m_GridCount);
                PropertyField(m_GridStep);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Power", "DuFieldsSpaceGizmo.Power"))
            {
                PropertyField(m_PowerVisible);
                PropertyExtendedSlider(m_PowerSize, 0.1f, 2.0f, +0.1f, 0.1f);
                PropertyField(m_PowerImpactOnDotsSize);
                Space();
                PropertyField(m_PowerDotsVisible);
                PropertyExtendedSlider(m_PowerDotsSize, 0.1f, 2.0f, +0.1f, 0.1f);
                PropertyField(m_PowerDotsColor);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "DuFieldsSpaceGizmo.Color"))
            {
                PropertyField(m_ColorVisible);
                PropertyExtendedSlider(m_ColorSize, 0.1f, 5.0f, +0.1f, 0.1f);
                PropertyField(m_PowerImpactOnColorSize);
                PropertyField(m_ColorAllowTransparent);
                Space();
            }
            DustGUI.FoldoutEnd();

            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_GridCount.isChanged)
                m_GridCount.valVector3Int = DuFieldsSpaceGizmo.Normalizer.GridCount(m_GridCount.valVector3Int);

            if (m_PowerSize.isChanged)
                m_PowerSize.valFloat = DuFieldsSpaceGizmo.Normalizer.Size(m_PowerSize.valFloat);

            if (m_PowerDotsSize.isChanged)
                m_PowerDotsSize.valFloat = DuFieldsSpaceGizmo.Normalizer.Size(m_PowerDotsSize.valFloat);

            if (m_ColorSize.isChanged)
                m_ColorSize.valFloat = DuFieldsSpaceGizmo.Normalizer.Size(m_ColorSize.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
