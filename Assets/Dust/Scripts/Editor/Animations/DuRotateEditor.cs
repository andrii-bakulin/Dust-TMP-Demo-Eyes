using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRotate))]
    [CanEditMultipleObjects]
    public class DuRotateEditor : DuEditor
    {
        private DuProperty m_Axis;
        private DuProperty m_Speed;
        private DuProperty m_Space;

        private DuProperty m_RotateAroundObject;

        private DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animation/Rotate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Rotate", typeof(DuRotate));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_Axis = FindProperty("m_Axis", "Axis");
            m_Speed = FindProperty("m_Speed", "Speed");
            m_Space = FindProperty("m_Space", "Space");

            m_RotateAroundObject = FindProperty("m_RotateAroundObject", "Rotate Around Object");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Axis);
            PropertyExtendedSlider(m_Speed, -100f, +100f, 1f);
            PropertyFieldOrLock(m_Space, m_RotateAroundObject.ObjectReferenceExists);

            Space();

            PropertyField(m_RotateAroundObject);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
