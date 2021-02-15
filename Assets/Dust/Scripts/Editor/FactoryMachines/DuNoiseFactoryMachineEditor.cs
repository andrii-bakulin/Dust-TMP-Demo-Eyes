using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuNoiseFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuNoiseFactoryMachineEditor : DuPRSFactoryMachineEditor
    {
        private DuProperty m_NoiseMode;
        private DuProperty m_NoiseDimension;
        private DuProperty m_Synchronized;
        private DuProperty m_AnimationSpeed;
        private DuProperty m_AnimationOffset;
        private DuProperty m_NoiseSpace;
        private DuProperty m_NoiseForce;
        private DuProperty m_NoiseScale;
        private DuProperty m_Seed;

        private DuProperty m_PositionAxisRemapping;
        private DuProperty m_RotationAxisRemapping;
        private DuProperty m_ScaleAxisRemapping;

        //--------------------------------------------------------------------------------------------------------------

        static DuNoiseFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuNoiseFactoryMachine), "Noise");
        }

        [MenuItem("Dust/Factory/Machines/Noise")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuNoiseFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactoryMachine();

            m_NoiseMode = FindProperty("m_NoiseMode", "Noise Mode");
            m_NoiseDimension = FindProperty("m_NoiseDimension", "Noise Dimension");
            m_AnimationSpeed = FindProperty("m_AnimationSpeed", "Animation Speed");
            m_AnimationOffset = FindProperty("m_AnimationOffset", "Animation Offset");
            m_NoiseSpace = FindProperty("m_NoiseSpace", "Noise Space");
            m_NoiseForce = FindProperty("m_NoiseForce", "Noise Force");
            m_NoiseScale = FindProperty("m_NoiseScale", "Noise Scale");
            m_Synchronized = FindProperty("m_Synchronized", "Synchronize P.R.S.", "If TRUE, noises will be equal for Position, Rotation and Scale");
            m_Seed = FindProperty("m_Seed", "Seed");

            m_PositionAxisRemapping = FindProperty("m_PositionAxisRemapping", "Remap Position");
            m_RotationAxisRemapping = FindProperty("m_RotationAxisRemapping", "Remap Rotation");
            m_ScaleAxisRemapping = FindProperty("m_ScaleAxisRemapping", "Remap Scale");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();

            if (DustGUI.FoldoutBegin("Noise", "DuFactoryMachine.Noise"))
            {
                PropertyField(m_NoiseMode);
                PropertyField(m_NoiseDimension);
                PropertySeedFixed(m_Seed);
                PropertyField(m_Synchronized);
                Space();

                switch ((DuNoiseFactoryMachine.NoiseMode) m_NoiseMode.enumValueIndex)
                {
                    case DuNoiseFactoryMachine.NoiseMode.Random:
                    default:
                        // Ignore
                        break;

                    case DuNoiseFactoryMachine.NoiseMode.Perlin:
                        PropertyField(m_NoiseSpace);
                        PropertyExtendedSlider(m_NoiseForce, 0.0f, 4.0f, 0.01f, 0.00f, 10f);
                        PropertyExtendedSlider(m_NoiseScale, 0.01f, 16f, 0.01f, 0.01f);
                        Space();

                        PropertyExtendedSlider(m_AnimationSpeed, 0f, 10f, 0.01f);
                        PropertyExtendedSlider(m_AnimationOffset, -5f, 5f, 0.01f);
                        Space();
                        break;
                }
            }
            DustGUI.FoldoutEnd();


            if (m_Synchronized.IsTrue
                && (DuNoiseFactoryMachine.NoiseDimension) m_NoiseDimension.enumValueIndex == DuNoiseFactoryMachine.NoiseDimension.Noise3D)
            {
                if (DustGUI.FoldoutBegin("Remap Axises for Noise Forces", "DuFactoryMachine.RemapNoiseForces"))
                {
                    PropertyField(m_PositionAxisRemapping);
                    PropertyField(m_RotationAxisRemapping);
                    PropertyField(m_ScaleAxisRemapping);
                    Space();
                }
                DustGUI.FoldoutEnd();
            }

            OnInspectorGUI_TransformBlock();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_NoiseForce.isChanged)
                m_NoiseForce.valFloat = DuNoiseFactoryMachine.Normalizer.NoiseForce(m_NoiseForce.valFloat);

            if (m_NoiseScale.isChanged)
                m_NoiseScale.valFloat = DuNoiseFactoryMachine.Normalizer.NoiseScale(m_NoiseScale.valFloat);

            if (m_Seed.isChanged)
                m_Seed.valInt = DuNoiseFactoryMachine.Normalizer.Seed(m_Seed.valInt);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as DuNoiseFactoryMachine;

                if (m_Seed.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetStates();
            }
        }
    }
}
