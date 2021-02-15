using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuClampFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuClampFactoryMachineEditor : DuBasicFactoryMachineEditor
    {
        private DuProperty m_PositionMode;
        private DuProperty m_PositionMin;
        private DuProperty m_PositionMax;

        private DuProperty m_RotationMode;
        private DuProperty m_RotationMin;
        private DuProperty m_RotationMax;

        private DuProperty m_ScaleMode;
        private DuProperty m_ScaleMin;
        private DuProperty m_ScaleMax;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private ClampMode positionMode => (ClampMode) m_PositionMode.enumValueIndex;
        private ClampMode rotationMode => (ClampMode) m_RotationMode.enumValueIndex;
        private ClampMode scaleMode => (ClampMode) m_ScaleMode.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        static DuClampFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuClampFactoryMachine), "Clamp");
        }

        [MenuItem("Dust/Factory/Machines/Clamp")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuClampFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactoryMachine();

            m_PositionMode = FindProperty("m_PositionMode", "Position");
            m_PositionMin = FindProperty("m_PositionMin", "Min");
            m_PositionMax = FindProperty("m_PositionMax", "Max");

            m_RotationMode = FindProperty("m_RotationMode", "Rotation");
            m_RotationMin = FindProperty("m_RotationMin", "Min");
            m_RotationMax = FindProperty("m_RotationMax", "Max");

            m_ScaleMode = FindProperty("m_ScaleMode", "Scale");
            m_ScaleMin = FindProperty("m_ScaleMin", "Min");
            m_ScaleMax = FindProperty("m_ScaleMax", "Max");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_BaseParameters();

            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyField(m_CustomHint);
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_PositionMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= positionMode == ClampMode.MinOnly;
                    showMin |= positionMode == ClampMode.MinAndMax;

                    showMax |= positionMode == ClampMode.MaxOnly;
                    showMax |= positionMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_PositionMin, !showMin);
                    PropertyFieldOrHide(m_PositionMax, !showMax);
                }
                Space();

                PropertyField(m_RotationMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= rotationMode == ClampMode.MinOnly;
                    showMin |= rotationMode == ClampMode.MinAndMax;

                    showMax |= rotationMode == ClampMode.MaxOnly;
                    showMax |= rotationMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_RotationMin, !showMin);
                    PropertyFieldOrHide(m_RotationMax, !showMax);
                }
                Space();

                PropertyField(m_ScaleMode);
                {
                    bool showMin = false;
                    bool showMax = false;

                    showMin |= scaleMode == ClampMode.MinOnly;
                    showMin |= scaleMode == ClampMode.MinAndMax;

                    showMax |= scaleMode == ClampMode.MaxOnly;
                    showMax |= scaleMode == ClampMode.MinAndMax;

                    PropertyFieldOrHide(m_ScaleMin, !showMin);
                    PropertyFieldOrHide(m_ScaleMax, !showMax);
                }
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
