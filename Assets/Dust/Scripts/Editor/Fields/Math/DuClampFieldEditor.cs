using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuClampField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuClampFieldEditor : DuFieldEditor
    {
        protected DuProperty m_PowerClampMode;
        protected DuProperty m_PowerClampMin;
        protected DuProperty m_PowerClampMax;

        protected DuProperty m_ColorClampMode;
        protected DuProperty m_ColorClampMin;
        protected DuProperty m_ColorClampMax;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode powerClampMode => (ClampMode) m_PowerClampMode.enumValueIndex;
        private ClampMode colorClampMode => (ClampMode) m_ColorClampMode.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        static DuClampFieldEditor()
        {
            DuFieldsPopupButtons.AddMathField(typeof(DuClampField), "Clamp");
        }

        [MenuItem("Dust/Fields/Math Fields/Clamp")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuClampField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_PowerClampMode = FindProperty("m_PowerClampMode", "Clamp Mode");
            m_PowerClampMin = FindProperty("m_PowerClampMin", "Min");
            m_PowerClampMax = FindProperty("m_PowerClampMax", "Max");

            m_ColorClampMode = FindProperty("m_ColorClampMode", "Clamp Mode");
            m_ColorClampMin = FindProperty("m_ColorClampMin", "Min");
            m_ColorClampMax = FindProperty("m_ColorClampMax", "Max");
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
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Power", "DuAnyField.Power"))
            {
                PropertyField(m_PowerClampMode);

                bool isMinEnabled = powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MinOnly;
                bool isMaxEnabled = powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMin, 0f, 1f, 0.01f);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyExtendedSlider(m_PowerClampMax, 0f, 1f, 0.01f);
                if (!isMaxEnabled) DustGUI.Unlock();

                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "DuAnyField.Color"))
            {
                PropertyField(m_ColorClampMode);

                bool isMinEnabled = colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MinOnly;
                bool isMaxEnabled = colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MaxOnly;

                if (!isMinEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMin);
                if (!isMinEnabled) DustGUI.Unlock();

                if (!isMaxEnabled) DustGUI.Lock();
                PropertyField(m_ColorClampMax);
                if (!isMaxEnabled) DustGUI.Unlock();

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
