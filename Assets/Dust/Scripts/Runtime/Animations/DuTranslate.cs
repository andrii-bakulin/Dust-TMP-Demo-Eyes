using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/Translate")]
    public class DuTranslate : DuMonoBehaviour
    {
        public enum DirectionSpace
        {
            Self = 0,
            Parent = 1,
            World = 2,
        }

        public enum TranslateType
        {
            Linear = 0,
            Wave = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private TranslateType m_TranslateType = TranslateType.Linear;
        public TranslateType translateType
        {
            get => m_TranslateType;
            set => m_TranslateType = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_LinearSpeed = Vector3.forward;
        public Vector3 linearSpeed
        {
            get => m_LinearSpeed;
            set => m_LinearSpeed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_WaveAmplitude = Vector3.forward;
        public Vector3 waveAmplitude
        {
            get => m_WaveAmplitude;
            set => m_WaveAmplitude = value;
        }

        [SerializeField]
        private Vector3 m_WaveSpeed = Vector3.forward * 360;
        public Vector3 waveSpeed
        {
            get => m_WaveSpeed;
            set => m_WaveSpeed = value;
        }

        [SerializeField]
        private Vector3 m_WaveOffset = Vector3.zero;
        public Vector3 waveOffset
        {
            get => m_WaveOffset;
            set => m_WaveOffset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private DirectionSpace m_DirectionSpace = DirectionSpace.Parent;
        public DirectionSpace directionSpace
        {
            get => m_DirectionSpace;
            set => m_DirectionSpace = value;
        }

        [SerializeField]
        private bool m_Freeze = false;
        public bool freeze
        {
            get => m_Freeze;
            set => m_Freeze = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.Update;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_LastDeltaPosition;
        public Vector3 lastDeltaPosition => m_LastDeltaPosition;

        private float m_TimeSinceStart;

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
            if (freeze)
                return;

            m_TimeSinceStart += deltaTime;

            Vector3 deltaPosition;
            bool requireRollbackLastTranslate = false;

            switch (translateType)
            {
                case TranslateType.Linear:
                    deltaPosition = linearSpeed * deltaTime;
                    break;

                case TranslateType.Wave:
                    deltaPosition = new Vector3(
                        CalculateWaveOffset(waveAmplitude.x, waveSpeed.x, waveOffset.x),
                        CalculateWaveOffset(waveAmplitude.y, waveSpeed.y, waveOffset.y),
                        CalculateWaveOffset(waveAmplitude.z, waveSpeed.z, waveOffset.z));
                    requireRollbackLastTranslate = true;
                    break;

                default:
                    return;
            }

            if (directionSpace == DirectionSpace.Self)
                deltaPosition = DuMath.RotatePoint(deltaPosition, this.transform.localEulerAngles);

            Vector3 extDeltaPosition = deltaPosition;

            if (requireRollbackLastTranslate)
                extDeltaPosition -= m_LastDeltaPosition;

            switch (directionSpace)
            {
                default:
                case DirectionSpace.Self:
                case DirectionSpace.Parent:
                    this.transform.localPosition += extDeltaPosition;
                    break;

                case DirectionSpace.World:
                    this.transform.position += extDeltaPosition;
                    break;
            }

            m_LastDeltaPosition = deltaPosition;
        }

        float CalculateWaveOffset(float amplitude, float speed, float offset)
        {
            if (DuMath.IsZero(amplitude))
                return 0f;

            return Mathf.Sin(DuConstants.PI2 * (speed / 360f) * m_TimeSinceStart + DuConstants.PI2 * offset) * amplitude;
        }
    }
}
