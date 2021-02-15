using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuBasicFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuBasicFactoryMachineEditor : DuFactoryMachineEditor
    {
        protected DuProperty m_ValueImpactEnabled;
        protected DuProperty m_ValueImpactIntensity;
        protected DuProperty m_ValueBlendMode;
        protected DuProperty m_ValueClampEnabled;
        protected DuProperty m_ValueClampMin;
        protected DuProperty m_ValueClampMax;

        protected DuProperty m_ColorImpactEnabled;
        protected DuProperty m_ColorImpactIntensity;
        protected DuProperty m_ColorBlendMode;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        static DuBasicFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuBasicFactoryMachine), "Basic");
        }

        [MenuItem("Dust/Factory/Machines/Basic")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuBasicFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactoryMachine();
        }

        protected override void OnEnableFactoryMachine()
        {
            base.OnEnableFactoryMachine();

            m_ValueImpactEnabled = FindProperty("m_ValueImpactEnabled", "Enabled");
            m_ValueImpactIntensity = FindProperty("m_ValueImpactIntensity", "Intensity");
            m_ValueBlendMode = FindProperty("m_ValueBlendMode", "Blend Mode");
            m_ValueClampEnabled = FindProperty("m_ValueClampEnabled", "Clamp");
            m_ValueClampMin = FindProperty("m_ValueClampMin", "Min");
            m_ValueClampMax = FindProperty("m_ValueClampMax", "Max");

            m_ColorImpactEnabled = FindProperty("m_ColorImpactEnabled", "Enabled");
            m_ColorImpactIntensity = FindProperty("m_ColorImpactIntensity", "Intensity");
            m_ColorBlendMode = FindProperty("m_ColorBlendMode", "Blend Mode");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuBasicFactoryMachine).fieldsMap);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }

        //--------------------------------------------------------------------------------------------------------------

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_ImpactOnValueBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Value", "DuFactoryMachine.ImpactOnValue"))
            {
                PropertyField(m_ValueImpactEnabled);

                if (m_ValueImpactEnabled.IsTrue)
                {
                    PropertyField(m_ValueBlendMode);
                    PropertyExtendedSlider(m_ValueImpactIntensity, 0f, +1f, 0.01f);

                    Space();

                    PropertyField(m_ValueClampEnabled);

                    if (m_ValueClampEnabled.IsTrue)
                    {
                        DustGUI.IndentLevelInc();
                        PropertyExtendedSlider(m_ValueClampMin, -1f, +1f, 0.01f);
                        PropertyExtendedSlider(m_ValueClampMax, -1f, +1f, 0.01f);
                        DustGUI.IndentLevelDec();
                    }
                }

                Space();
            }
            DustGUI.FoldoutEnd();
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_ImpactOnColorBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Color", "DuFactoryMachine.ImpactOnColor"))
            {
                PropertyField(m_ColorImpactEnabled);

                if (m_ColorImpactEnabled.IsTrue)
                {
                    PropertyField(m_ColorBlendMode);
                    PropertyExtendedSlider(m_ColorImpactIntensity, 0f, +1f, 0.01f);
                }
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        // WARNING!
        // On change logic/structure here, also required to check changes in parent/children methods
        // with same method name/links
        protected void OnInspectorGUI_FieldsMap()
        {
            var showColumnPower = DuFieldsMapEditor.ColumnVisibility.Auto;
            var showColumnColor = DuFieldsMapEditor.ColumnVisibility.Auto;

            if (!m_ColorImpactEnabled.IsTrue)
                showColumnColor = DuFieldsMapEditor.ColumnVisibility.ForcedHide;

            m_FieldsMapEditor.OnInspectorGUI(showColumnPower, showColumnColor);
        }
    }
}
