using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Transform Machine")]
    public class DuTransformFactoryMachine : DuPRSFactoryMachine
    {
        public override string FactoryMachineName()
        {
            return "Transform";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = min + (max - min) * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            // Define default states
        }
    }
}
