using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFactoryTextureField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuFactoryTextureFieldEditor : DuFieldEditor
    {
        private DuProperty m_Texture;
        private DuProperty m_SpaceUVW;

        private DuProperty m_FlipU;
        private DuProperty m_FlipV;
        private DuProperty m_FlipW;

        private DuProperty m_PowerSource;
        private DuProperty m_ApplyPowerToAlpha;

        protected DuRemappingEditor m_RemappingEditor;

        //--------------------------------------------------------------------------------------------------------------

        static DuFactoryTextureFieldEditor()
        {
            DuFieldsPopupButtons.AddFactoryField(typeof(DuFactoryTextureField), "Factory Texture");
        }

        [MenuItem("Dust/Fields/Factory Fields/Factory Texture")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuFactoryTextureField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Texture = FindProperty("m_Texture", "Texture");
            m_SpaceUVW = FindProperty("m_SpaceUVW", "Space");

            m_FlipU = FindProperty("m_FlipU", "Flip U");
            m_FlipV = FindProperty("m_FlipV", "Flip V");
            m_FlipW = FindProperty("m_FlipW", "Flip W");

            m_PowerSource = FindProperty("m_PowerSource", "Power Source");

            m_ApplyPowerToAlpha = FindProperty("m_ApplyPowerToAlpha", "Apply Power To Alpha");

            m_RemappingEditor = new DuRemappingEditor((target as DuFactoryTextureField).remapping, serializedObject.FindProperty("m_Remapping"));
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

                PropertyField(m_Texture);
                PropertyField(m_SpaceUVW);

                switch ((DuFactoryTextureField.SpaceUVW) m_SpaceUVW.enumValueIndex)
                {
                    default:
                    case DuFactoryTextureField.SpaceUVW.UV:
                        PropertyField(m_FlipU);
                        PropertyField(m_FlipV);
                        break;

                    case DuFactoryTextureField.SpaceUVW.UW:
                        PropertyField(m_FlipU);
                        PropertyField(m_FlipW);
                        break;

                    case DuFactoryTextureField.SpaceUVW.VW:
                        PropertyField(m_FlipV);
                        PropertyField(m_FlipW);
                        break;
                }
                Space();

                DustGUI.Header("Power Impact");
                PropertyField(m_PowerSource);
                Space();

                DustGUI.Header("Color Impact");
                PropertyField(m_ApplyPowerToAlpha);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_RemappingBlock();

            if ((DuFactoryTextureField.ColorComponent) m_PowerSource.enumValueIndex != DuFactoryTextureField.ColorComponent.Ignore)
            {
                m_RemappingEditor.OnInspectorGUI(false);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
