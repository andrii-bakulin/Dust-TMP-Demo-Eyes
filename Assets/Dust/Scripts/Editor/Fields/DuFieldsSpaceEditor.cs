using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpace))]
    public class DuFieldsSpaceEditor : DuEditor
    {
        private DuProperty m_CalculatePower;
        private DuProperty m_CalculateColor;

        private DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Fields/Fields Space")]
        protected static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Fields Space", typeof(DuFieldsSpace));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnEnable()
        {
            SerializedProperty propertyFieldsMap = serializedObject.FindProperty("m_FieldsMap");

            m_CalculatePower = FindProperty(propertyFieldsMap, "m_CalculatePower", "Calculate Power");
            m_CalculateColor = FindProperty(propertyFieldsMap, "m_CalculateColor", "Calculate Color");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, propertyFieldsMap, (target as DuFieldsSpace).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuFieldsSpace.Parameters"))
            {
                PropertyField(m_CalculatePower);
                PropertyField(m_CalculateColor);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_FieldsMapEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
