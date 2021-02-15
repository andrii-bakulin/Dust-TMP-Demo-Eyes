using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCubeField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuCubeFieldEditor : DuSpaceFieldEditor
    {
        private DuProperty m_Size;

        //--------------------------------------------------------------------------------------------------------------

        static DuCubeFieldEditor()
        {
            DuFieldsPopupButtons.Add3DField(typeof(DuCubeField), "Cube");
        }

        [MenuItem("Dust/Fields/3D Fields/Cube")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCubeField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Size = FindProperty("m_Size", "Size");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyField(m_Size);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.isChanged)
                m_Size.valVector3 = DuCubeField.ShapeNormalizer.Size(m_Size.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            foreach (var subTarget in targets)
            {
                var origin = subTarget as DuCubeField;

                if (m_Size.isChanged || DustGUI.IsUndoRedoPerformed())
                    origin.ResetCalcData();
            }
        }
    }
}
