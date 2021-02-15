using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/Rotate")]
    public class DuRotate : DuMonoBehaviour
    {
        [SerializeField]
        private Vector3 m_Axis = Vector3.up;
        public Vector3 axis
        {
            get => m_Axis;
            set => m_Axis = value;
        }

        [SerializeField]
        private float m_Speed = 45f;
        public float speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }

        [SerializeField]
        private WorkSpace m_Space = WorkSpace.Local;
        public WorkSpace space
        {
            get => m_Space;
            set => m_Space = value;
        }

        [SerializeField]
        private GameObject m_RotateAroundObject = null;
        public GameObject rotateAroundObject
        {
            get => m_RotateAroundObject;
            set => m_RotateAroundObject = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        void UpdateState(float deltaTime)
        {
            if (Dust.IsNotNull(rotateAroundObject) && rotateAroundObject != this.gameObject)
                this.transform.RotateAround(rotateAroundObject.transform.position, axis, speed * deltaTime);
            else
                this.transform.Rotate(axis, speed * deltaTime, WorkSpaceToSpace(space));
        }
    }
}
