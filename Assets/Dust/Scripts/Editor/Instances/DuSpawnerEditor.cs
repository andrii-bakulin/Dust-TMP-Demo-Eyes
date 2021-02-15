using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuSpawner))]
    [CanEditMultipleObjects]
    public class DuSpawnerEditor : DuEditor
    {
        private DuProperty m_SpawnEvent;
        private DuProperty m_Interval;
        private DuProperty m_IntervalRange;

        private DuProperty m_SpawnObjects;
        private DuProperty m_SpawnObjectsIterate;
        private DuProperty m_SpawnObjectsSeed;

        private DuProperty m_SpawnPointMode;
        private DuProperty m_SpawnPoints;
        private DuProperty m_SpawnPointsIterate;
        private DuProperty m_SpawnPointsSeed;

        private DuProperty m_MultipleSpawnEnabled;
        private DuProperty m_MultipleSpawnCountMin;
        private DuProperty m_MultipleSpawnCountMax;
        private DuProperty m_MultipleSpawnSeed;

        private DuProperty m_ParentMode;
        private DuProperty m_Limit;
        private DuProperty m_SpawnOnAwake;

        private DuProperty m_OnSpawn;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Instance/Spawner")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Spawner", typeof(DuSpawner));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_SpawnEvent = FindProperty("m_SpawnEvent", "Spawn Event");
            m_Interval = FindProperty("m_Interval", "Interval");
            m_IntervalRange = FindProperty("m_IntervalRange", "Interval Range");

            m_SpawnObjects = FindProperty("m_SpawnObjects", "Objects");
            m_SpawnObjectsIterate = FindProperty("m_SpawnObjectsIterate", "Objects Iterate");
            m_SpawnObjectsSeed = FindProperty("m_SpawnObjectsSeed", "Seed");

            m_SpawnPointMode = FindProperty("m_SpawnPointMode", "Spawn At");
            m_SpawnPoints = FindProperty("m_SpawnPoints", "Spawn Points");
            m_SpawnPointsIterate = FindProperty("m_SpawnPointsIterate", "Spawn Points Iterate");
            m_SpawnPointsSeed = FindProperty("m_SpawnPointsSeed", "Seed");

            m_MultipleSpawnEnabled = FindProperty("m_MultipleSpawnEnabled", "Enabled");
            m_MultipleSpawnCountMin = FindProperty(serializedObject.FindProperty("m_MultipleSpawnCount"), "m_Min", "Min Count");
            m_MultipleSpawnCountMax = FindProperty(serializedObject.FindProperty("m_MultipleSpawnCount"), "m_Max", "Max Count");
            m_MultipleSpawnSeed = FindProperty("m_SpawnPointsSeed", "Seed");

            m_ParentMode = FindProperty("m_ParentMode", "Set Parent As");
            m_Limit = FindProperty("m_Limit", "Total Limit");
            m_SpawnOnAwake = FindProperty("m_SpawnOnAwake", "Spawn On Awake");

            m_OnSpawn = FindProperty("m_OnSpawn", "On Spawn");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_SpawnEvent);

            switch ((DuSpawner.SpawnEvent) m_SpawnEvent.enumValueIndex)
            {
                case DuSpawner.SpawnEvent.Manual:
                    DustGUI.HelpBoxInfo("Call method Spawn() or SpawnSingleObject() to spawn object(s)");
                    break;

                case DuSpawner.SpawnEvent.FixedInterval:
                    PropertyField(m_Interval);
                    break;

                case DuSpawner.SpawnEvent.IntervalInRange:
                    PropertyFieldRange(m_IntervalRange);
                    break;

                default:
                    // Nothing to show
                    break;
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Spawn Objects");

            PropertyField(m_SpawnObjects);

            if (m_SpawnObjects.property.arraySize > 1)
            {
                PropertyField(m_SpawnObjectsIterate);

                if ((DuSpawner.IterateMode) m_SpawnObjectsIterate.enumValueIndex == DuSpawner.IterateMode.Random)
                    PropertySeedRandomOrFixed(m_SpawnObjectsSeed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Spawn Points");

            PropertyField(m_SpawnPointMode);

            switch ((DuSpawner.SpawnPointMode) m_SpawnPointMode.enumValueIndex)
            {
                case DuSpawner.SpawnPointMode.Self:
                    break;

                case DuSpawner.SpawnPointMode.Points:
                    PropertyField(m_SpawnPoints);

                    if (m_SpawnPoints.property.arraySize > 1)
                    {
                        PropertyField(m_SpawnPointsIterate);

                        if ((DuSpawner.IterateMode) m_SpawnPointsIterate.enumValueIndex == DuSpawner.IterateMode.Random)
                            PropertySeedRandomOrFixed(m_SpawnPointsSeed);
                    }
                    break;

                default:
                    // Nothing to show
                    break;
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.Header("Multiple Objects Spawn");

            PropertyField(m_MultipleSpawnEnabled);

            if (m_MultipleSpawnEnabled.IsTrue)
            {
                PropertyExtendedIntSlider(m_MultipleSpawnCountMin, 0, 10, 1, 0);
                PropertyExtendedIntSlider(m_MultipleSpawnCountMax, 0, 10, 1, 0);
                PropertySeedRandomOrFixed(m_MultipleSpawnSeed);
            }

            Space();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Limit);
            PropertyField(m_SpawnOnAwake);
            PropertyField(m_ParentMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Space();

            if (DustGUI.FoldoutBegin("Events", "DuSpawner.Events", false))
            {
                PropertyField(m_OnSpawn);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Application.isPlaying)
            {
                if (DustGUI.FoldoutBegin("Debug", "DuSpawner.Debug"))
                {
                    DustGUI.BeginHorizontal();
                    {
                        if (DustGUI.Button("Spawn"))
                        {
                            foreach (var subTarget in targets)
                                (subTarget as DuSpawner).Spawn();
                        }

                        if (DustGUI.Button("Spawn Single Object"))
                        {
                            foreach (var subTarget in targets)
                                (subTarget as DuSpawner).SpawnSingleObject();
                        }
                    }
                    DustGUI.EndHorizontal();

                    if (targets.Length == 1)
                    {
                        var main = target as DuSpawner;

                        Space();

                        DustGUI.Header("Stats");
                        DustGUI.StaticTextField("Spawned", main.count.ToString());
                        DustGUI.StaticTextField("Total Limit", main.limit.ToString());
                    }
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Interval.isChanged)
                NormalizeIntervalValue(m_Interval.property);

            if (m_IntervalRange.isChanged)
            {
                NormalizeIntervalValue(m_IntervalRange.FindInnerProperty("m_Min"));
                NormalizeIntervalValue(m_IntervalRange.FindInnerProperty("m_Max"));
            }

            if (m_MultipleSpawnCountMin.isChanged || m_MultipleSpawnCountMax.isChanged)
            {
                DuIntRange range = new DuIntRange(m_MultipleSpawnCountMin.valInt, m_MultipleSpawnCountMax.valInt);

                range = DuSpawner.Normalizer.MultipleSpawnCount(range);

                m_MultipleSpawnCountMin.valInt = range.min;
                m_MultipleSpawnCountMax.valInt = range.max;
            }

            if (m_Limit.isChanged)
                m_Limit.valInt = DuSpawner.Normalizer.Limit(m_Limit.valInt);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }

        private void NormalizeIntervalValue(SerializedProperty property)
        {
            property.floatValue = DuSpawner.Normalizer.IntervalValue(property.floatValue);
        }
    }
}
