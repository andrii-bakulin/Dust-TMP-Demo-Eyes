using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuKeyEvent))]
    [CanEditMultipleObjects]
    public class DuKeyEventEditor : DuEventEditor
    {
        private DuProperty m_KeyCode;

        private DuProperty m_OnKeyDown;
        private DuProperty m_OnKeyHold;
        private DuProperty m_OnKeyUp;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Events/On Key")]
        public static DuKeyEvent AddComponent()
        {
            return AddComponentByEventType(typeof(DuKeyEvent)) as DuKeyEvent;
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_KeyCode = FindProperty("m_KeyCode", "Key Code");

            m_OnKeyDown = FindProperty("m_OnKeyDown", "On Key Down Callbacks");
            m_OnKeyHold = FindProperty("m_OnKeyHold", "On Key Hold Callbacks");
            m_OnKeyUp = FindProperty("m_OnKeyUp", "On Key Up Callbacks");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_KeyCode);

            Space();

            var titleOnKeyDown = "Down" + (m_OnKeyDown.valUnityEvent.arraySize > 0 ? " (" + m_OnKeyDown.valUnityEvent.arraySize + ")" : "");
            var titleOnKeyHold = "Hold" + (m_OnKeyHold.valUnityEvent.arraySize > 0 ? " (" + m_OnKeyHold.valUnityEvent.arraySize + ")" : "");
            var titleOnKeyUp   = "Up"   + (m_OnKeyUp.valUnityEvent.arraySize   > 0 ? " (" + m_OnKeyUp.valUnityEvent.arraySize   + ")" : "");

            var tabIndex = DustGUI.Toolbar("DuKeyEvent.Events", new[] {titleOnKeyDown, titleOnKeyHold, titleOnKeyUp});

            switch (tabIndex)
            {
                case 0: PropertyField(m_OnKeyDown); break;
                case 1: PropertyField(m_OnKeyHold); break;
                case 2: PropertyField(m_OnKeyUp); break;
            }

            Space();
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
