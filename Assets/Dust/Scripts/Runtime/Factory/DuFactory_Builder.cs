using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public partial class DuFactory
    {
        private DuFactoryBuilder m_Builder;

        internal DuFactoryBuilder builder
        {
            get
            {
                if (Dust.IsNull(m_Builder))
                {
                    m_Builder = GetFactoryBuilder();
                    m_Builder.Initialize(this);
                }

                return m_Builder;
            }
        }

        internal abstract DuFactoryBuilder GetFactoryBuilder();

        private void DestroyBuilder()
        {
            m_Builder = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        private int m_SourceObjectsLastStampId = 0;

        private int GetSourceObjectsStampId()
        {
            int stampId = ((int) sourceObjectsMode + 25) * 1001;

            if (sourceObjectsMode == SourceObjectsMode.Holder ||
                sourceObjectsMode == SourceObjectsMode.HolderAndList)
            {
                if (Dust.IsNotNull(sourceObjectsHolder))
                {
                    for (int i = 0; i < sourceObjectsHolder.transform.childCount; i++)
                    {
                        stampId ^= i * 835141 + sourceObjectsHolder.transform.GetChild(i).GetInstanceID();
                    }
                }
            }

            if (sourceObjectsMode == SourceObjectsMode.List ||
                sourceObjectsMode == SourceObjectsMode.HolderAndList)
            {
                for (int i = 0; i < sourceObjects.Count; i++)
                {
                    stampId ^= i * 835141 + sourceObjects[i].GetInstanceID();
                }
            }

            if (stampId == 0)
                stampId = 1;

            return stampId;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void RebuildInstancesIfRequired()
        {
            if (m_SourceObjectsLastStampId == GetSourceObjectsStampId())
                return;

            RebuildInstances();
        }

        public void RebuildInstances()
        {
            DestroyAllInstances();
            DestroyBuilder();

            // @notice: on get builder -> it'll auto create it!
            if (Dust.IsNull(builder))
                return;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Instantiate All Instances + Ignore NULL elements + finally repack to array

            var instancesPacked = new List<DuFactoryInstance>();
            var instancesRandom = new DuRandom(seed);

            builder.ObjectsQueue_Initialize();

            int instancesCount = builder.GetTotalInstancesCount();

            List<bool> fillStates = null;

            if (instancesFillRate < 1f)
            {
                DuRandom duRandom = new DuRandom(DuRandom.NormalizeSeedToNonRandom(instancesFillSeed));

                fillStates = new List<bool>(instancesCount);
                int enabledItemsCount = Mathf.RoundToInt(instancesCount * instancesFillRate);

                for (int i = 0; i < instancesCount; i++)
                {
                    fillStates.Insert(duRandom.Range(0, fillStates.Count), i < enabledItemsCount);
                }
            }

            for (int instanceIndex = 0; instanceIndex < instancesCount; instanceIndex++)
            {
                float randomScalar = instancesRandom.Next();
                Vector3 randomVector = instancesRandom.NextVector3();

                if (Dust.IsNotNull(fillStates) && fillStates[instanceIndex] == false)
                {
                    builder.CreateFactoryFakeInstance(instanceIndex, instancesCount, randomScalar, randomVector);
                    continue;
                }

                var instance = builder.CreateFactoryInstance(instanceIndex, instancesCount, randomScalar, randomVector);

                if (Dust.IsNull(instance))
                    continue;

                instancesPacked.Add(instance);

                switch (instanceAccessMode)
                {
                    case InstanceAccessMode.Normal:
                    default:
                        instance.gameObject.hideFlags = HideFlags.DontSave;
                        break;

                    case InstanceAccessMode.NotEditable:
                        instance.gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
                        break;

                    case InstanceAccessMode.HideInHierarchy:
                        instance.gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideInHierarchy;
                        break;
                }
            }

            builder.ObjectsQueue_Release();

            m_Instances = instancesPacked.ToArray();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Create prev/next instance references

            for (int i = 0; i < m_Instances.Length; i++)
            {
                DuFactoryInstance prevInstance = i > 0 ? m_Instances[i - 1] : null;
                DuFactoryInstance nextInstance = i < m_Instances.Length - 1 ? m_Instances[i + 1] : null;

                m_Instances[i].SetPrevNextInstances(prevInstance, nextInstance);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstancesZeroStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_SourceObjectsLastStampId = GetSourceObjectsStampId();
        }

        protected void DestroyAllInstances()
        {
            // Step 1: drop by list
            for (int i = 0; i < m_Instances.Length; i++)
            {
                if (Dust.IsNull(m_Instances[i]))
                    continue;

                Dust.DestroyObjectWhenReady(m_Instances[i].gameObject);
            }

            // Step 2: drop all object which left same how from previous state
            GameObject[] holders =
            {
                this.instancesHolder,
                this.gameObject
            };

            foreach (var holder in holders)
            {
                if (Dust.IsNull(holder))
                    continue;

                for (int i = holder.transform.childCount - 1; i >= 0; i--)
                {
                    DuFactoryInstance instance = holder.transform.GetChild(i).GetComponent<DuFactoryInstance>();

                    if (Dust.IsNotNull(instance) && instance.parentFactory == this)
                    {
                        Dust.DestroyObjectWhenReady(instance.gameObject);
                    }
                }
            }

            // Reset array (but it cannot be null)
            m_Instances = new DuFactoryInstance[0];
        }

        //--------------------------------------------------------------------------------------------------------------

        public void UpdateInstancesZeroStates()
        {
            if (Dust.IsNull(builder))
                return;

            builder.Reinitialize(this);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int instancesCount = m_Instances.Length;
            for (int i = 0; i < instancesCount; i++)
            {
                DuFactoryInstance factoryInstance = m_Instances[i];

                if (Dust.IsNull(factoryInstance))
                    continue;

                var instanceState = builder.GetDefaultInstanceState(factoryInstance);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Apply instances transform params

                switch (transformSequence)
                {
                    case TransformSequence.PositionRotationScale:
                        SetInstanceZeroStates_Position(instanceState);
                        SetInstanceZeroStates_Rotation(instanceState);
                        SetInstanceZeroStates_Scale(instanceState);
                        break;

                    case TransformSequence.RotationPositionScale:
                        SetInstanceZeroStates_Rotation(instanceState);
                        SetInstanceZeroStates_Position(instanceState);
                        SetInstanceZeroStates_Scale(instanceState);
                        break;

                    default:
                        break;
                }

                SetInstanceZeroStates_Value(instanceState);
                SetInstanceZeroStates_Color(instanceState);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                factoryInstance.SetDefaultState(instanceState);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstancesDynamicStates(true);

#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        private void SetInstanceZeroStates_Position(DuFactoryInstance.State instanceState)
        {
            switch (transformSpace)
            {
                case TransformSpace.Factory:
                    instanceState.position += DuMath.RotatePoint(transformPosition, instanceState.rotation);
                    break;

                case TransformSpace.Instance:
                    instanceState.position += transformPosition;
                    break;

                default:
                    break;
            }
        }

        private void SetInstanceZeroStates_Rotation(DuFactoryInstance.State instanceState)
        {
            instanceState.rotation += transformRotation;
        }

        private void SetInstanceZeroStates_Scale(DuFactoryInstance.State instanceState)
        {
            instanceState.scale.Scale(transformScale);
        }

        private void SetInstanceZeroStates_Value(DuFactoryInstance.State instanceState)
        {
            instanceState.value = defaultValue;
        }

        private void SetInstanceZeroStates_Color(DuFactoryInstance.State instanceState)
        {
            instanceState.color = defaultColor;
        }
    }
}
