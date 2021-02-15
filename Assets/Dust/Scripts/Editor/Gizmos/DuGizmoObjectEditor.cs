using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuGizmoObjectEditor : DuEditor
    {
        protected DuProperty m_Color;
        protected DuProperty m_GizmoVisibility;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddGizmoToSelectedOrNewObject(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddGizmoToNewObject(duComponentType);
            }
        }

        public static Component AddGizmoToNewObject(System.Type duComponentType)
            => AddGizmoToNewObject(duComponentType, true);

        public static Component AddGizmoToNewObject(System.Type duComponentType, bool fixUndoState)
        {
            var gameObject = new GameObject();

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            var component = gameObject.AddComponent(duComponentType) as DuGizmoObject;

            gameObject.name = component.GizmoName() + " Gizmo";
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableGizmo()
        {
            m_Color = FindProperty("m_Color", "Color");
            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
        }

        public override void OnInspectorGUI()
        {
            // Hide base implementation
        }
    }
}
