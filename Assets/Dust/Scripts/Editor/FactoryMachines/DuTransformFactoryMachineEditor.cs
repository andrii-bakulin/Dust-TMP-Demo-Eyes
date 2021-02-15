using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTransformFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTransformFactoryMachineEditor : DuPRSFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static DuTransformFactoryMachineEditor()
        {
            DuFactoryMachinesPopupButtons.AddMachine(typeof(DuTransformFactoryMachine), "Transform");
        }

        [MenuItem("Dust/Factory/Machines/Transform")]
        public new static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuTransformFactoryMachine));
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
