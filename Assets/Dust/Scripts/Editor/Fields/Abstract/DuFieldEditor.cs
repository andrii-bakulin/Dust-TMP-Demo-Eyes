using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFieldEditor : DuEditor
    {
        protected DuProperty m_CustomHint;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddFieldComponentByType(System.Type duFieldType)
        {
            Selection.activeGameObject = AddFieldComponentByType(Selection.activeGameObject, duFieldType);
        }

        public static GameObject AddFieldComponentByType(GameObject activeGameObject, System.Type duFieldType)
        {
#if DUST_ALPHA_DEFORMERS
            DuDeformer selectedDeformer = null;
#endif
            DuFieldsSpace selectedFieldsSpace = null;
            DuBasicFactoryMachine selectedFactoryMachine = null;

            if (Dust.IsNotNull(activeGameObject))
            {
#if DUST_ALPHA_DEFORMERS
                selectedDeformer = activeGameObject.GetComponent<DuDeformer>();

                if (Dust.IsNull(selectedDeformer) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedDeformer = activeGameObject.transform.parent.GetComponent<DuDeformer>();
#endif

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFieldsSpace = activeGameObject.GetComponent<DuFieldsSpace>();

                if (Dust.IsNull(selectedFieldsSpace) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFieldsSpace = activeGameObject.transform.parent.GetComponent<DuFieldsSpace>();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                selectedFactoryMachine = activeGameObject.GetComponent<DuBasicFactoryMachine>();

                if (Dust.IsNull(selectedFactoryMachine) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFactoryMachine = activeGameObject.transform.parent.GetComponent<DuBasicFactoryMachine>();
            }

            var gameObject = new GameObject();
            {
                DuField field = gameObject.AddComponent(duFieldType) as DuField;

#if DUST_ALPHA_DEFORMERS
                if (Dust.IsNotNull(selectedDeformer))
                {
                    field.transform.parent = selectedDeformer.transform;
                    selectedDeformer.fieldsMap.AddField(field);
                }
                else
#endif
                if (Dust.IsNotNull(selectedFieldsSpace))
                {
                    field.transform.parent = selectedFieldsSpace.transform;
                    selectedFieldsSpace.fieldsMap.AddField(field);
                }
                else if (Dust.IsNotNull(selectedFactoryMachine))
                {
                    field.transform.parent = selectedFactoryMachine.transform;
                    selectedFactoryMachine.fieldsMap.AddField(field);
                }

                gameObject.name = field.FieldName() + " Field";
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableField()
        {
            m_CustomHint = FindProperty("m_CustomHint", "Hint for Field");
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-fields-view if need in future...
        }
    }
}
