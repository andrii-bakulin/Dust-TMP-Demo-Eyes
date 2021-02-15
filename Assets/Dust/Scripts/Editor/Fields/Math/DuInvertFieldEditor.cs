using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuInvertField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuInvertFieldEditor : DuFieldEditor
    {
        protected DuProperty m_ColorInvertAlpha;

        //--------------------------------------------------------------------------------------------------------------

        static DuInvertFieldEditor()
        {
            DuFieldsPopupButtons.AddMathField(typeof(DuInvertField), "Invert");
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_ColorInvertAlpha = FindProperty("m_ColorInvertAlpha", "Invert Alpha");
        }

        [MenuItem("Dust/Fields/Math Fields/Invert")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuInvertField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                DustGUI.Header("Color");
                PropertyField(m_ColorInvertAlpha);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
