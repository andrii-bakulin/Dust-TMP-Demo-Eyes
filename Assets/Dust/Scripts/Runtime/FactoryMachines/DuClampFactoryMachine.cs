using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Clamp Machine")]
    public class DuClampFactoryMachine : DuBasicFactoryMachine
    {
        [SerializeField]
        private ClampMode m_PositionMode = ClampMode.NoClamp;
        public ClampMode positionMode
        {
            get => m_PositionMode;
            set => m_PositionMode = value;
        }

        [SerializeField]
        private Vector3 m_PositionMin = Vector3.zero;
        public Vector3 positionMin
        {
            get => m_PositionMin;
            set => m_PositionMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionMax = Vector3.one;
        public Vector3 positionMax
        {
            get => m_PositionMax;
            set => m_PositionMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_RotationMode = ClampMode.NoClamp;
        public ClampMode rotationMode
        {
            get => m_RotationMode;
            set => m_RotationMode = value;
        }

        [SerializeField]
        private Vector3 m_RotationMin = Vector3.zero;
        public Vector3 rotationMin
        {
            get => m_RotationMin;
            set => m_RotationMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationMax = Vector3.one;
        public Vector3 rotationMax
        {
            get => m_RotationMax;
            set => m_RotationMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_ScaleMode = ClampMode.NoClamp;
        public ClampMode scaleMode
        {
            get => m_ScaleMode;
            set => m_ScaleMode = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMin = Vector3.zero;
        public Vector3 scaleMin
        {
            get => m_ScaleMin;
            set => m_ScaleMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMax = Vector3.one;
        public Vector3 scaleMax
        {
            get => m_ScaleMax;
            set => m_ScaleMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Clamp";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var instanceState = factoryInstanceState.instance.stateDynamic;

            // @notice: here fieldPower also involve to transferPower (not like in PRS-Factory)
            float transferPower = factoryInstanceState.intensityByFactory
                                  * factoryInstanceState.intensityByMachine
                                  * factoryInstanceState.fieldPower;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Position

            if (positionMode != ClampMode.NoClamp)
            {
                Vector3 endPosition = instanceState.position;

                if (positionMode == ClampMode.MinOnly || positionMode == ClampMode.MinAndMax)
                    endPosition = Vector3.Max(endPosition, positionMin);

                if (positionMode == ClampMode.MaxOnly || positionMode == ClampMode.MinAndMax)
                    endPosition = Vector3.Min(endPosition, positionMax);

                instanceState.position = Vector3.LerpUnclamped(instanceState.position, endPosition, transferPower);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Rotation

            if (rotationMode != ClampMode.NoClamp)
            {
                Vector3 endRotation = instanceState.rotation;

                if (rotationMode == ClampMode.MinOnly || rotationMode == ClampMode.MinAndMax)
                    endRotation = Vector3.Max(endRotation, rotationMin);

                if (rotationMode == ClampMode.MaxOnly || rotationMode == ClampMode.MinAndMax)
                    endRotation = Vector3.Min(endRotation, rotationMax);

                instanceState.rotation = Vector3.LerpUnclamped(instanceState.rotation, endRotation, transferPower);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Scale

            if (scaleMode != ClampMode.NoClamp)
            {
                Vector3 endScale = instanceState.scale;

                if (scaleMode == ClampMode.MinOnly || scaleMode == ClampMode.MinAndMax)
                    endScale = Vector3.Max(endScale, scaleMin);

                if (scaleMode == ClampMode.MaxOnly || scaleMode == ClampMode.MinAndMax)
                    endScale = Vector3.Min(endScale, scaleMax);

                instanceState.scale = Vector3.LerpUnclamped(instanceState.scale, endScale, transferPower);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, positionMode);
            if (positionMode != ClampMode.NoClamp)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, positionMin);
                DuDynamicState.Append(ref dynamicState, ++seq, positionMax);
            }

            DuDynamicState.Append(ref dynamicState, ++seq, rotationMode);
            if (rotationMode != ClampMode.NoClamp)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, rotationMin);
                DuDynamicState.Append(ref dynamicState, ++seq, rotationMax);
            }

            DuDynamicState.Append(ref dynamicState, ++seq, scaleMode);
            if (scaleMode != ClampMode.NoClamp)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, scaleMin);
                DuDynamicState.Append(ref dynamicState, ++seq, scaleMax);
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}
