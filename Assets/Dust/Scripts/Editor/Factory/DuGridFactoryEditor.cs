﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuGridFactory))]
    [CanEditMultipleObjects]
    public class DuGridFactoryEditor : DuFactoryEditor
    {
        private DuProperty m_Count;
        private DuProperty m_Step;

        private DuProperty m_OffsetDirection;
        private DuProperty m_OffsetWidth;
        private DuProperty m_OffsetHeight;

        //--------------------------------------------------------------------------------------------------------------

        private DuGridFactory.OffsetDirection offsetDirection
            => (DuGridFactory.OffsetDirection) m_OffsetDirection.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Factory/Grid Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(DuGridFactory));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactory();

            m_Count = FindProperty("m_Count", "Count");
            m_Step = FindProperty("m_Step", "Step");

            m_OffsetDirection = FindProperty("m_OffsetDirection", "Offset Direction");
            m_OffsetWidth = FindProperty("m_OffsetWidth", "Offset Width");
            m_OffsetHeight = FindProperty("m_OffsetHeight", "Offset Height");
        }

        public override void OnInspectorGUI()
        {
            BeginData();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Grid", "DuFactory.Grid"))
            {
                PropertyField(m_Count);
                PropertyField(m_Step);
                Space();

                PropertyField(m_OffsetDirection);

                if (offsetDirection != DuGridFactory.OffsetDirection.Disabled)
                {
                    PropertyExtendedSlider(m_OffsetWidth, -1f, +1f, 0.01f);
                    PropertyExtendedSlider(m_OffsetHeight, -1f, +1f, 0.01f);
                }
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildInstances |= m_Count.isChanged;
            m_IsRequireResetupInstances |= m_Step.isChanged;

            m_IsRequireResetupInstances |= m_OffsetDirection.isChanged;
            m_IsRequireResetupInstances |= m_OffsetWidth.isChanged;
            m_IsRequireResetupInstances |= m_OffsetHeight.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_SourceObjects();
            OnInspectorGUI_Instances();
            OnInspectorGUI_FactoryMachines();
            OnInspectorGUI_Transform();
            OnInspectorGUI_Gizmos();
            OnInspectorGUI_Tools();

            //----------------------------------------------------------------------------------------------------------
            // Validate & Normalize Data

            if (m_Count.isChanged)
                m_Count.valVector3Int = DuGridFactory.Normalizer.Count(m_Count.valVector3Int);

            //----------------------------------------------------------------------------------------------------------

            CommitDataAndUpdateStates();
        }
    }
}
