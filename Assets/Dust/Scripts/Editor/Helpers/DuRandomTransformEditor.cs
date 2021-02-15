using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRandomTransform))]
    [CanEditMultipleObjects]
    public class DuRandomTransformEditor : DuEditor
    {
        private DuProperty m_ActivateMode;
        private DuProperty m_TransformMode;
        private DuProperty m_Space;

        private DuProperty m_PositionEnabled;
        private DuProperty m_PositionRangeMin;
        private DuProperty m_PositionRangeMax;

        private DuProperty m_RotationEnabled;
        private DuProperty m_RotationRangeMin;
        private DuProperty m_RotationRangeMax;

        private DuProperty m_ScaleEnabled;
        private DuProperty m_ScaleRangeMin;
        private DuProperty m_ScaleRangeMax;

        private DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Helpers/Random Transform")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Random Transform", typeof(DuRandomTransform));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_ActivateMode = FindProperty("m_ActivateMode", "Activate On");
            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode",
                "Relative - add random values to current transform" + "\n" +
                "Absolute - set random values as new transform");
            m_Space = FindProperty("m_Space", "Space");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Enabled");
            m_PositionRangeMin = FindProperty("m_PositionRangeMin", "Range Min");
            m_PositionRangeMax = FindProperty("m_PositionRangeMax", "Range Max");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Enabled");
            m_RotationRangeMin = FindProperty("m_RotationRangeMin", "Range Min");
            m_RotationRangeMax = FindProperty("m_RotationRangeMax", "Range Max");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Enabled");
            m_ScaleRangeMin = FindProperty("m_ScaleRangeMin", "Range Min");
            m_ScaleRangeMax = FindProperty("m_ScaleRangeMax", "Range Max");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_ActivateMode);
            PropertyField(m_TransformMode);
            PropertyField(m_Space);

            Space();

            if (DustGUI.FoldoutBegin("Position", "DuRandomTransform.Position"))
            {
                PropertyField(m_PositionEnabled);

                if (m_PositionEnabled.IsTrue)
                {
                    PropertyField(m_PositionRangeMin);
                    PropertyField(m_PositionRangeMax);
                }
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Rotation", "DuRandomTransform.Rotation"))
            {
                PropertyField(m_RotationEnabled);

                if (m_RotationEnabled.IsTrue)
                {
                    PropertyField(m_RotationRangeMin);
                    PropertyField(m_RotationRangeMax);
                }
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Scale", "DuRandomTransform.Scale"))
            {
                PropertyField(m_ScaleEnabled);

                if (m_ScaleEnabled.IsTrue)
                {
                    PropertyField(m_ScaleRangeMin);
                    PropertyField(m_ScaleRangeMax);
                }
            }
            DustGUI.FoldoutEnd();

            Space();

            PropertySeedRandomOrFixed(m_Seed);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
