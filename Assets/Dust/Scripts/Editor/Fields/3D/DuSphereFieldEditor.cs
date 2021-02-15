using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuSphereField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuSphereFieldEditor : DuSpaceFieldEditor
    {
        private DuProperty m_Radius;

        //--------------------------------------------------------------------------------------------------------------

        static DuSphereFieldEditor()
        {
            DuFieldsPopupButtons.Add3DField(typeof(DuSphereField), "Sphere");
        }

        [MenuItem("Dust/Fields/3D Fields/Sphere")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuSphereField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Radius = FindProperty("m_Radius", "Radius");
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
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = DuSphereField.ShapeNormalizer.Radius(m_Radius.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
