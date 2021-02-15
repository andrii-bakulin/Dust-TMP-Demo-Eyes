using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Helpers/Lock Transform")]
    [ExecuteAlways]
    public class DuLockTransform : MonoBehaviour
    {
        public enum LockInSpace
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        public bool m_LockPosition = true;
        public bool lockPosition
        {
            get => m_LockPosition;
            set
            {
                if (m_LockPosition == value)
                    return;

                m_LockPosition = value;
                FixLockStates();
            }
        }

        [SerializeField]
        public bool m_LockRotation = true;
        public bool lockRotation
        {
            get => m_LockRotation;
            set
            {
                if (m_LockRotation == value)
                    return;

                m_LockRotation = value;
                FixLockStates();
            }
        }

        [SerializeField]
        public bool m_LockScale = true;
        public bool lockScale
        {
            get => m_LockScale;
            set
            {
                if (m_LockScale == value)
                    return;

                m_LockScale = value;
                FixLockStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        public Vector3 m_Position;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        public Quaternion m_Rotation;
        public Quaternion rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        public Vector3 m_Scale;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        public LockInSpace m_LockInSpace;
        public LockInSpace lockInSpace
        {
            get => m_LockInSpace;
            set
            {
                if (m_LockInSpace == value)
                    return;

                m_LockInSpace = value;
                FixLockStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            FixLockStates();
        }

        private void OnDisable()
        {
            ReleaseLockStates();
        }

        private void Update()
        {
            UpdateTransformState();
        }

        private void UpdateTransformState()
        {
            Transform t = transform;

            if (lockPosition)
            {
                if (lockInSpace == LockInSpace.Local)
                    t.localPosition = m_Position;
                else
                    t.position = m_Position;
            }

            if (lockRotation)
            {
                if (lockInSpace == LockInSpace.Local)
                    t.localRotation = m_Rotation;
                else
                    t.rotation = m_Rotation;
            }

            if (lockScale)
            {
                t.localScale = m_Scale;
            }
        }

        public void FixLockStates()
        {
            Transform t = transform;

            if (lockPosition)
                m_Position = lockInSpace == LockInSpace.Local ? t.localPosition : t.position;
            else
                m_Position = Vector3.zero;

            if (lockRotation)
                m_Rotation = lockInSpace == LockInSpace.Local ? t.localRotation : t.rotation;
            else
                m_Rotation = Quaternion.identity;

            if (lockScale)
                m_Scale = t.localScale;
            else
                m_Scale = Vector3.one;
        }

        private void ReleaseLockStates()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            m_Scale = Vector3.one;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            FixLockStates();
        }
    }
}
