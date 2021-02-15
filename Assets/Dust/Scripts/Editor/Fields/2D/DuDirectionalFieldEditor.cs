using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDirectionalField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuDirectionalFieldEditor : DuSpaceFieldEditor
    {
        private DuProperty m_Length;
        private DuProperty m_Direction;

        private DuProperty m_GizmoWidth;
        private DuProperty m_GizmoHeight;

        //--------------------------------------------------------------------------------------------------------------

        static DuDirectionalFieldEditor()
        {
            DuFieldsPopupButtons.Add2DField(typeof(DuDirectionalField), "Directional");
        }

        [MenuItem("Dust/Fields/2D Fields/Directional")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuDirectionalField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Length = FindProperty("m_Length", "Length");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoWidth = FindProperty("m_GizmoWidth", "Width");
            m_GizmoHeight = FindProperty("m_GizmoHeight", "Height");
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

                PropertyExtendedSlider(m_Length, 0f, 20f, 0.01f);
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
                PropertyExtendedSlider(m_GizmoWidth, 0f, 10f, 0.1f, 0f);
                PropertyExtendedSlider(m_GizmoHeight, 0f, 10f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Length.isChanged)
                m_Length.valFloat = DuDirectionalField.ShapeNormalizer.Length(m_Length.valFloat);

            if (m_GizmoWidth.isChanged)
                m_GizmoWidth.valFloat = DuDirectionalField.ShapeNormalizer.GizmoWidth(m_GizmoWidth.valFloat);

            if (m_GizmoHeight.isChanged)
                m_GizmoHeight.valFloat = DuDirectionalField.ShapeNormalizer.GizmoHeight(m_GizmoHeight.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
