using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCylinderField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuCylinderFieldEditor : DuSpaceFieldEditor
    {
        private DuProperty m_Height;
        private DuProperty m_Radius;
        private DuProperty m_Direction;

        //--------------------------------------------------------------------------------------------------------------

        static DuCylinderFieldEditor()
        {
            DuFieldsPopupButtons.Add3DField(typeof(DuCylinderField), "Cylinder");
        }

        [MenuItem("Dust/Fields/3D Fields/Cylinder")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCylinderField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Height = FindProperty("m_Height", "Height");
            m_Radius = FindProperty("m_Radius", "Radius");
            m_Direction = FindProperty("m_Direction", "Direction");
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

                PropertyExtendedSlider(m_Radius, 0f, 20f, 0.01f);
                PropertyExtendedSlider(m_Height, 0f, 20f, 0.01f);
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Height.isChanged)
                m_Height.valFloat = DuCylinderField.ShapeNormalizer.Height(m_Height.valFloat);

            if (m_Radius.isChanged)
                m_Radius.valFloat = DuCylinderField.ShapeNormalizer.Radius(m_Radius.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
