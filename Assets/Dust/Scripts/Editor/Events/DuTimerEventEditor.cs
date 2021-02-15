using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTimerEvent))]
    [CanEditMultipleObjects]
    public class DuTimerEventEditor : DuEventEditor
    {
        protected DuProperty m_Delay;
        protected DuProperty m_Repeat;
        protected DuProperty m_FireOnStart;

        protected DuProperty m_OnFire;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Events/On Timer")]
        public static DuTimerEvent AddComponent()
        {
            if (Dust.IsNull(Selection.activeGameObject))
                return null;

            return Selection.activeGameObject.AddComponent<DuTimerEvent>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_Delay = FindProperty("m_Delay", "Delay");
            m_Repeat = FindProperty("m_Repeat", "Repeat");
            m_FireOnStart = FindProperty("m_FireOnStart", "Fire On Start");

            m_OnFire = FindProperty("m_OnFire", "On Fire");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Delay, 0.0f, 5.0f, 0.01f, 0.0f);
            PropertyExtendedIntSlider(m_Repeat, 0, 100, 1, 0);
            PropertyField(m_FireOnStart);

            Space();

            PropertyField(m_OnFire);
            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Delay.isChanged)
                m_Delay.valFloat = DuTimerEvent.Normalizer.Delay(m_Delay.valFloat);

            if (m_Repeat.isChanged)
                m_Repeat.valInt = DuTimerEvent.Normalizer.Repeat(m_Repeat.valInt);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
