using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMaterialFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuMaterialFactoryMachineEditor : DuFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static DuMaterialFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuMaterialFactoryMachine), "Material");
        }

        [MenuItem("Dust/Factory/Machines/Material")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuMaterialFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactoryMachine();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyField(m_CustomHint);
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
