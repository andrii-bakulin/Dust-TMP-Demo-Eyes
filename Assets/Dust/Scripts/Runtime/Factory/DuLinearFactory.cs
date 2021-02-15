using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Linear Factory")]
    [ExecuteInEditMode]
    public class DuLinearFactory : DuFactory
    {
        [SerializeField]
        private int m_Count = 5;
        public int count
        {
            get => m_Count;
            set
            {
                if (!UpdatePropertyValue(ref m_Count, Normalizer.Count(value)))
                    return;

                RebuildInstances();
            }
        }

        [SerializeField]
        private int m_Offset = 0;
        public int offset
        {
            get => m_Offset;
            set
            {
                if (!UpdatePropertyValue(ref m_Offset, Normalizer.Offset(value)))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_Amount = 1f;
        public float amount
        {
            get => m_Amount;
            set
            {
                if (!UpdatePropertyValue(ref m_Amount, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.right * 5f;
        public Vector3 position
        {
            get => m_Position;
            set
            {
                if (!UpdatePropertyValue(ref m_Position, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set
            {
                if (!UpdatePropertyValue(ref m_Rotation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set
            {
                if (!UpdatePropertyValue(ref m_Scale, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Vector3 m_StepRotation = Vector3.zero;
        public Vector3 stepRotation
        {
            get => m_StepRotation;
            set
            {
                if (!UpdatePropertyValue(ref m_StepRotation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Linear";
        }

        internal override DuFactoryBuilder GetFactoryBuilder()
        {
            return new DuFactoryLinearBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static int Count(int value)
            {
                return Mathf.Max(0, value);
            }

            public static int Offset(int value)
            {
                return Mathf.Max(0, value);
            }
        }
    }
}
