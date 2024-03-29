﻿using UnityEngine;

namespace DustEngine
{
    public partial class DuFactory
    {
#if UNITY_EDITOR
        public class Stats
        {
            public int updatesCount { get; internal set; }
            public float lastUpdateTime { get; internal set; }
        }

        public readonly Stats stats = new Stats();
#endif
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private int m_LastDynamicStateHashCode = 0;
        public int lastDynamicStateHashCode => m_LastDynamicStateHashCode;

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public virtual int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, GetInstancesHolderTransform());
            DuDynamicState.Append(ref dynamicState, ++seq, factoryMachines.Count);

            foreach (DuFactoryMachine.Record record in factoryMachines)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + UpdateInstancesDynamicStates()

                if (Dust.IsNull(record) || !record.enabled || Dust.IsNull(record.factoryMachine))
                    continue;

                if (!record.factoryMachine.enabled || !record.factoryMachine.gameObject.activeInHierarchy)
                    continue;

                // @END

                DuDynamicState.Append(ref dynamicState, ++seq, record.enabled);
                DuDynamicState.Append(ref dynamicState, ++seq, record.intensity);
                DuDynamicState.Append(ref dynamicState, ++seq, record.factoryMachine);
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void UpdateInstancesDynamicStates()
            => UpdateInstancesDynamicStates(false);

        public void UpdateInstancesDynamicStates(bool forced)
        {
            if (forcedUpdateEachFrame)
            {
                // Nothing need to do
                // Just execute down logic.
                // Even if factoryMachines.Count is ZERO
            }
            else if (!forced)
            {
                if (factoryMachines.Count == 0)
                    return;

                int newDynamicStateHash = GetDynamicStateHashCode();

                if (newDynamicStateHash != 0 && m_LastDynamicStateHashCode == newDynamicStateHash)
                    return;

                m_LastDynamicStateHashCode = newDynamicStateHash;
            }

#if UNITY_EDITOR
            var timer = Dust.Debug.StartTimer();
#endif

            //----------------------------------------------------------------------------------------------------------
            // Step 1: reset instanceState to ZERO-State

            foreach (var factoryInstance in m_Instances)
            {
                if (Dust.IsNull(factoryInstance))
                    continue;

                factoryInstance.ResetDynamicStateToZeroState();
            }

            //----------------------------------------------------------------------------------------------------------
            // Step 2: Calculate new transforms by Machines

            foreach (DuFactoryMachine.Record record in factoryMachines)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + UpdateInstancesDynamicStates()

                if (Dust.IsNull(record) || !record.enabled || Dust.IsNull(record.factoryMachine))
                    continue;

                if (!record.factoryMachine.enabled || !record.factoryMachine.gameObject.activeInHierarchy)
                    continue;

                // @END

                // Notice: cannot do this
                // For example in MaterialFactoryMachine if with intensity=0.0f logic will skip UpdateInstanceState()
                // then it'll ignore material changes and it'll return to default state.
                // But should apply material is zero-state.
                // So, factoryMachine.***UpdateInstanceState() logic should decide what to do with intensity=0.0f
                //
                // if (DuMath.IsZero(record.intensity))
                //     continue;

                var factoryInstanceState = new DuFactoryMachine.FactoryInstanceState
                {
                    factory = this,
                    intensityByFactory = record.intensity,
                };

                if (record.factoryMachine.PrepareForUpdateInstancesStates(factoryInstanceState) == false)
                    continue;

                foreach (var instance in m_Instances)
                {
                    if (Dust.IsNull(instance))
                        continue;

                    factoryInstanceState.instance = instance;

                    record.factoryMachine.UpdateInstanceState(factoryInstanceState);
                }

                record.factoryMachine.FinalizeUpdateInstancesStates(factoryInstanceState);
            }

            //----------------------------------------------------------------------------------------------------------
            // Step 3: Apply Dynamic state to transform objects in scenes

            foreach (var factoryInstance in m_Instances)
            {
                if (Dust.IsNull(factoryInstance))
                    continue;

                factoryInstance.ApplyDynamicStateToObject();
            }

            //----------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
            stats.updatesCount++;
            stats.lastUpdateTime = timer.Stop();
#endif
        }
    }
}
