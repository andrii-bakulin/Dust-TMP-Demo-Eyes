using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCurveField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuCurveFieldEditor : DuFieldEditor
    {
        protected DuProperty m_Shape;
        protected DuProperty m_Offset;
        protected DuProperty m_AnimationSpeed;

        protected DuProperty m_BeforeCurve;
        protected DuProperty m_AfterCurve;

        //--------------------------------------------------------------------------------------------------------------

        static DuCurveFieldEditor()
        {
            DuFieldsPopupButtons.AddMathField(typeof(DuCurveField), "Curve");
        }

        [MenuItem("Dust/Fields/Math Fields/Curve")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCurveField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Shape = FindProperty("m_Shape", "Shape");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_AnimationSpeed = FindProperty("m_AnimationSpeed", "Animation Speed");

            m_BeforeCurve = FindProperty("m_BeforeCurve", "Before Curve");
            m_AfterCurve = FindProperty("m_AfterCurve", "After Curve");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyFieldCurve(m_Shape);
                PropertyExtendedSlider(m_AnimationSpeed, 0f, 10f, 0.01f, 0f);
                PropertyExtendedSlider(m_Offset, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_BeforeCurve);
                PropertyField(m_AfterCurve);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Shape.isChanged)
                m_Shape.valAnimationCurve = DuCurveField.Normalizer.Shape(m_Shape.valAnimationCurve);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
