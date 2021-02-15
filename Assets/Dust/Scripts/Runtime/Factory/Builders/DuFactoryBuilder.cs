using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuFactoryBuilder
    {
        protected DuFactory m_DuFactory;

        protected List<DuFactoryInstance.State> m_InstancesStates;

        public virtual void Initialize(DuFactory duFactory)
        {
            m_DuFactory = duFactory;

            m_InstancesStates = new List<DuFactoryInstance.State>();
        }

        // Alias for Initialize, but in some cases logic maybe much simpler
        public virtual void Reinitialize(DuFactory duFactory)
        {
            Initialize(duFactory);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Instance Manager

        // WARNING!
        //     If some logic change here, then require to do same changes in CreateFactoryFakeInstance() method !!!
        //
        internal DuFactoryInstance CreateFactoryInstance(int instanceIndex, int instancesCount, float randomScalar, Vector3 randomVector)
        {
            GameObject prefab = ObjectsQueue_GetNextPrefab();

            if (Dust.IsNull(prefab))
                return null;

            Transform parent = m_DuFactory.GetInstancesHolderTransform();

            GameObject instanceGameObject = null;

#if UNITY_EDITOR
            if (m_DuFactory.instanceTypeMode == DuFactory.InstanceTypeMode.Inherit)
            {
                instanceGameObject = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            }
#endif

            // Notice: When I create prefab by PrefabUtility.InstantiatePrefab() call sometimes it cannot be created
            // and instanceGameObject is NULL. In that cases I forced create instance as object!
            if (Dust.IsNull(instanceGameObject))
            {
                instanceGameObject = Object.Instantiate(prefab, parent);
                instanceGameObject.name = instanceGameObject.name.Replace("(Clone)", "");
            }

            if (Dust.IsNull(instanceGameObject))
                return null;

            if (m_DuFactory.forcedSetActive)
                instanceGameObject.SetActive(true);

            DuFactoryInstance duFactoryInstance = instanceGameObject.GetComponent<DuFactoryInstance>();

            if (Dust.IsNull(duFactoryInstance))
                duFactoryInstance = instanceGameObject.AddComponent<DuFactoryInstance>();

            float instanceOffset = instancesCount > 1 ? (float) instanceIndex / (instancesCount - 1) : 0f;

            duFactoryInstance.Initialize(m_DuFactory, instanceIndex, instanceOffset, randomScalar, randomVector);

            return duFactoryInstance;
        }

        // WARNING! Why need this method?
        //     This method will be call when fill-rate is less then 1.0f.
        //     So when need to skip instance creating Builder will call this method.
        //     It should not create anything, but it should "call sub-method" to make offset in random generators
        //     or others data
        internal DuFactoryInstance CreateFactoryFakeInstance(int instanceIndex, int instancesCount, float randomScalar, Vector3 randomVector)
        {
            ObjectsQueue_GetNextPrefab();
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Objects Queue

        private int m_ObjectsQueue_index;
        private DuRandom m_ObjectsQueue_duRandom;
        private List<GameObject> m_FinalSourceObjects;

        internal void ObjectsQueue_Initialize()
        {
            m_FinalSourceObjects = new List<GameObject>();

            if (m_DuFactory.sourceObjectsMode == DuFactory.SourceObjectsMode.Holder ||
                m_DuFactory.sourceObjectsMode == DuFactory.SourceObjectsMode.HolderAndList)
            {
                // Make SourceObject list. Step 1:
                if (Dust.IsNotNull(m_DuFactory.sourceObjectsHolder))
                {
                    var holder = m_DuFactory.sourceObjectsHolder.transform;
                    for (int i = 0; i < holder.childCount; i++)
                    {
                        m_FinalSourceObjects.Add(holder.GetChild(i).gameObject);
                    }
                }
            }

            if (m_DuFactory.sourceObjectsMode == DuFactory.SourceObjectsMode.List ||
                m_DuFactory.sourceObjectsMode == DuFactory.SourceObjectsMode.HolderAndList)
            {
                // Make SourceObject list. Step 2:
                foreach (var sourceObject in m_DuFactory.sourceObjects)
                {
                    m_FinalSourceObjects.Add(sourceObject);
                }
            }

            switch (m_DuFactory.iterateMode)
            {
                case DuFactory.IterateMode.Iterate:
                    m_ObjectsQueue_index = 0;
                    break;

                case DuFactory.IterateMode.Random:
                    m_ObjectsQueue_duRandom = new DuRandom( DuRandom.NormalizeSeedToNonRandom(m_DuFactory.seed) );
                    break;

                default:
                    break;
            }
        }

        private GameObject ObjectsQueue_GetNextPrefab()
        {
            if (m_FinalSourceObjects.Count == 0)
                return null;

            switch (m_DuFactory.iterateMode)
            {
                case DuFactory.IterateMode.Iterate:
                default:
                    return m_FinalSourceObjects[m_ObjectsQueue_index++ % m_FinalSourceObjects.Count];

                case DuFactory.IterateMode.Random:
                    return m_FinalSourceObjects[m_ObjectsQueue_duRandom.Range(0, m_FinalSourceObjects.Count)];
            }
        }

        internal void ObjectsQueue_Release()
        {
            m_FinalSourceObjects = null;
            m_ObjectsQueue_duRandom = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual int GetTotalInstancesCount()
        {
            return m_InstancesStates.Count;
        }

        // This method should calculate:
        // - position, rotation, scale
        // - UVW
        // Values for (next) params will be defined by instance:
        // - value
        // - color
        public virtual DuFactoryInstance.State GetDefaultInstanceState(DuFactoryInstance duFactoryInstance)
        {
            return m_InstancesStates[duFactoryInstance.index].Clone();
        }
    }
}
