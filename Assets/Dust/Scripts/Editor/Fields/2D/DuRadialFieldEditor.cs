﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRadialField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuRadialFieldEditor : DuSpaceFieldEditor
    {
        private DuProperty m_StartAngle;
        private DuProperty m_EndAngle;

        private DuProperty m_FadeInOffset;
        private DuProperty m_FadeOutOffset;

        private DuProperty m_Iterations;
        private DuProperty m_Offset;

        private DuProperty m_Direction;

        private DuProperty m_GizmoRadius;

        //--------------------------------------------------------------------------------------------------------------

        static DuRadialFieldEditor()
        {
            DuFieldsPopupButtons.Add2DField(typeof(DuRadialField), "Radial");
        }

        [MenuItem("Dust/Fields/2D Fields/Radial")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuRadialField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_StartAngle = FindProperty("m_StartAngle", "Start Angle");
            m_EndAngle = FindProperty("m_EndAngle", "End Angle");

            m_FadeInOffset = FindProperty("m_FadeInOffset", "Fade In Offset");
            m_FadeOutOffset = FindProperty("m_FadeOutOffset", "Fade Out Offset");

            m_Iterations = FindProperty("m_Iterations", "Iterations");
            m_Offset = FindProperty("m_Offset", "Offset");

            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoRadius = FindProperty("m_GizmoRadius", "Radius");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_StartAngle, 0f, 360f, 1f);
                PropertyExtendedSlider(m_EndAngle, 0f, 360f, 1f);
                Space();

                PropertyExtendedSlider(m_FadeInOffset, 0f, 360f, 1f);
                PropertyExtendedSlider(m_FadeOutOffset, 0f, 360f, 1f);
                Space();

                PropertyExtendedSlider(m_Iterations, 1f, 10f, 0.01f, 1f);
                PropertyExtendedSlider(m_Offset, 0f, 360f, 1f);
                Space();

                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_GizmoBlock();

            if (DustGUI.FoldoutBegin("Gizmo", "DuAnyField.Gizmo"))
            {
                PropertyField(m_GizmoVisibility);
                PropertyField(m_GizmoFieldColor);
                PropertyExtendedSlider(m_GizmoRadius, 0f, 5f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_FadeInOffset.isChanged)
                m_FadeInOffset.valFloat = DuRadialField.ShapeNormalizer.FadeOffset(m_FadeInOffset.valFloat);

            if (m_FadeOutOffset.isChanged)
                m_FadeOutOffset.valFloat = DuRadialField.ShapeNormalizer.FadeOffset(m_FadeOutOffset.valFloat);

            if (m_Iterations.isChanged)
                m_Iterations.valFloat = DuRadialField.ShapeNormalizer.Iterations(m_Iterations.valFloat);

            if (m_GizmoRadius.isChanged)
                m_GizmoRadius.valFloat = DuRadialField.ShapeNormalizer.GizmoRadius(m_GizmoRadius.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
