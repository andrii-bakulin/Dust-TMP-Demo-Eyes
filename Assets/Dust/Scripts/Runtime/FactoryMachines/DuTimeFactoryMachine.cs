using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Time Machine")]
    public class DuTimeFactoryMachine : DuPRSFactoryMachine
    {
        private float m_TimeSinceStart;
        public float timeSinceStart => m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Time";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_TimeSinceStart += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = (min + timeSinceStart * (max - min)) * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, m_TimeSinceStart);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            // Define default states
        }
    }
}
