using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuLockTransform))]
    [CanEditMultipleObjects]
    public class DuLockTransformEditor : DuEditor
    {
        private DuProperty m_LockPosition;
        private DuProperty m_LockRotation;
        private DuProperty m_LockScale;

        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        private DuProperty m_LockInSpace;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Helpers/Lock Transform")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(DuLockTransform));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_LockPosition = FindProperty("m_LockPosition", "Lock Position");
            m_LockRotation = FindProperty("m_LockRotation", "Lock Rotation");
            m_LockScale = FindProperty("m_LockScale", "Lock Scale");

            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");

            m_LockInSpace = FindProperty("m_LockInSpace", "Lock In Space");
        }

        public override void OnInspectorGUI()
        {
            var main = target as DuLockTransform;

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_LockPosition);
            PropertyField(m_LockRotation);
            PropertyField(m_LockScale);

            Space();

            PropertyField(m_LockInSpace);

            if (targets.Length == 1)
            {
                Space();

                if (DustGUI.FoldoutBegin("Lock States At", "DuLockTransform.LockStatesAt"))
                {
                    DustGUI.Lock();

                    if (main.enabled && (m_LockPosition.IsTrue || m_LockRotation.IsTrue || m_LockScale.IsTrue))
                    {
                        var lockInSpace = (DuLockTransform.LockInSpace) m_LockInSpace.enumValueIndex;

                        if (m_LockPosition.IsTrue)
                        {
                            string title = lockInSpace == DuLockTransform.LockInSpace.Local
                                ? "Local Position"
                                : "World Position";
                            DustGUI.Field(title, m_Position.valVector3);
                        }

                        if (m_LockRotation.IsTrue)
                        {
                            string title = lockInSpace == DuLockTransform.LockInSpace.Local
                                ? "Local Rotation"
                                : "World Rotation";
                            DustGUI.Field(title, m_Rotation.valQuaternion.eulerAngles);
                        }

                        if (m_LockScale.IsTrue)
                        {
                            string title = lockInSpace == DuLockTransform.LockInSpace.Local
                                ? "Local Scale"
                                : "Local* Scale";
                            DustGUI.Field(title, m_Scale.valVector3);
                        }
                    }
                    else
                    {
                        DustGUI.Label("Transform is not locked");
                    }

                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            bool reLockState = false;
            reLockState |= m_LockPosition.isChanged;
            reLockState |= m_LockRotation.isChanged;
            reLockState |= m_LockScale.isChanged;
            reLockState |= m_LockInSpace.isChanged;

            if (reLockState)
            {
                foreach (var subTarget in targets)
                {
                    var origin = subTarget as DuLockTransform;

                    origin.FixLockStates();
                }
            }
        }
    }
}
