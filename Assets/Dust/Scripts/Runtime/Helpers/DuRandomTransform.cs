using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Helpers/Random Transform")]
    public class DuRandomTransform : DuMonoBehaviour
    {
        public enum ActivateMode
        {
            Awake = 0,
            Start = 1,
        }

        public enum TransformMode
        {
            Relative = 0,
            Absolute = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ActivateMode m_ActivateMode = ActivateMode.Start;
        public ActivateMode activateMode
        {
            get => m_ActivateMode;
            set => m_ActivateMode = value;
        }

        [SerializeField]
        private TransformMode m_TransformMode = TransformMode.Relative;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private WorkSpace m_Space = WorkSpace.Local;
        public WorkSpace space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PositionEnabled = false;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMin = DuVector3.New(-1f);
        public Vector3 positionRangeMin
        {
            get => m_PositionRangeMin;
            set => m_PositionRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionRangeMax = DuVector3.New(+1f);
        public Vector3 positionRangeMax
        {
            get => m_PositionRangeMax;
            set => m_PositionRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled = false;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMin = Vector3.up * -180f;
        public Vector3 rotationRangeMin
        {
            get => m_RotationRangeMin;
            set => m_RotationRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationRangeMax = Vector3.up * +180f;
        public Vector3 rotationRangeMax
        {
            get => m_RotationRangeMax;
            set => m_RotationRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled = false;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMin = DuVector3.New(-0.5f);
        public Vector3 scaleRangeMin
        {
            get => m_ScaleRangeMin;
            set => m_ScaleRangeMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleRangeMax = DuVector3.New(+1.0f);
        public Vector3 scaleRangeMax
        {
            get => m_ScaleRangeMax;
            set => m_ScaleRangeMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ?? (m_DuRandom = new DuRandom(seed));

        //--------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            if (activateMode == ActivateMode.Awake)
                ApplyRandomState();
        }

        void Start()
        {
            if (activateMode == ActivateMode.Start)
                ApplyRandomState();
        }

        void ApplyRandomState()
        {
            if (positionEnabled)
            {
                Vector3 value = duRandom.Range(positionRangeMin, positionRangeMax);
                Vector3 position = Vector3.zero;

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        position = transform.localPosition;
                        break;

                    case WorkSpace.World:
                        position = transform.position;
                        break;
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                        position += value;
                        break;

                    case TransformMode.Absolute:
                        position = value;
                        break;
                }

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        transform.localPosition = position;
                        break;

                    case WorkSpace.World:
                        transform.position = position;
                        break;
                }
            }

            if (rotationEnabled)
            {
                Vector3 value = duRandom.Range(rotationRangeMin, rotationRangeMax);
                Vector3 rotation = Vector3.zero;

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        rotation = transform.localEulerAngles;
                        break;

                    case WorkSpace.World:
                        rotation = transform.eulerAngles;
                        break;
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                        rotation += value;
                        break;

                    case TransformMode.Absolute:
                        rotation = value;
                        break;
                }

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        transform.localEulerAngles = rotation;
                        break;

                    case WorkSpace.World:
                        transform.eulerAngles = rotation;
                        break;
                }
            }

            if (scaleEnabled)
            {
                Vector3 value = duRandom.Range(scaleRangeMin, scaleRangeMax);
                Vector3 scale = Vector3.one;

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        scale = transform.localScale;
                        break;

                    case WorkSpace.World:
                        // Ignore for now
                        break;
                }

                switch (transformMode)
                {
                    default:
                    case TransformMode.Relative:
                        scale += value;
                        break;

                    case TransformMode.Absolute:
                        scale = value;
                        break;
                }

                switch (space)
                {
                    default:
                    case WorkSpace.Local:
                        transform.localScale = scale;
                        break;

                    case WorkSpace.World:
                        // Ignore for now
                        break;
                }
            }
        }
    }
}
