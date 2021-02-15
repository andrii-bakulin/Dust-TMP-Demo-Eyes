using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTimeFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTimeFactoryMachineEditor : DuPRSFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static DuTimeFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuTimeFactoryMachine), "Time");
        }

        [MenuItem("Dust/Factory/Machines/Time")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuTimeFactoryMachine));
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

            OnInspectorGUI_BaseParameters();
            OnInspectorGUI_TransformBlock();
            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
