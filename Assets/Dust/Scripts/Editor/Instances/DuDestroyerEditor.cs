using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDestroyer))]
    [CanEditMultipleObjects]
    public class DuDestroyerEditor : DuEditor
    {
        private DuProperty m_DestroyMode;

        private DuProperty m_Timeout;
        private DuProperty m_TimeoutRange;

        private DuProperty m_VolumeCenterMode;
        private DuProperty m_VolumeCenter;
        private DuProperty m_VolumeOffset;
        private DuProperty m_VolumeSize;
        private DuProperty m_VolumeSourceCenter;

        private DuProperty m_DisableColliders;

        private DuProperty m_OnDestroy;

        private DuDestroyer.DestroyMode destroyMode => (DuDestroyer.DestroyMode) m_DestroyMode.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Instance/Destroyer")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroyer", typeof(DuDestroyer));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_DestroyMode = FindProperty("m_DestroyMode", "Destroy Mode");

            m_Timeout = FindProperty("m_Timeout", "Timeout");
            m_TimeoutRange = FindProperty("m_TimeoutRange", "Timeout Range");

            m_VolumeCenterMode = FindProperty("m_VolumeCenterMode", "Volume Center At");
            m_VolumeCenter = FindProperty("m_VolumeCenter", "Center");
            m_VolumeOffset = FindProperty("m_VolumeOffset", "Offset");
            m_VolumeSize = FindProperty("m_VolumeSize", "Size");
            m_VolumeSourceCenter = FindProperty("m_VolumeSourceCenter", "Center Source Object");

            m_DisableColliders = FindProperty("m_DisableColliders", "Disable Colliders");

            m_OnDestroy = FindProperty("m_OnDestroy", "On Destroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_DestroyMode);

            switch (destroyMode)
            {
                case DuDestroyer.DestroyMode.Manual:
                    // Nothing need to show here
                    break;

                case DuDestroyer.DestroyMode.Time:
                    PropertyField(m_Timeout);
                    break;

                case DuDestroyer.DestroyMode.TimeRange:
                    PropertyFieldRange(m_TimeoutRange, "Timeout Range Min", "Timeout Range Max");
                    break;

                case DuDestroyer.DestroyMode.AliveZone:
                case DuDestroyer.DestroyMode.DeadZone:
                    PropertyField(m_VolumeCenterMode);

                    switch ((DuDestroyer.VolumeCenterMode) m_VolumeCenterMode.enumValueIndex)
                    {
                        case DuDestroyer.VolumeCenterMode.StartPosition:
                            if (Application.isPlaying)
                                PropertyFieldOrLock(m_VolumeCenter, true);
                            else
                                DustGUI.StaticTextField("Center", "Self position when object appears in scene");
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize, "Size");
                            break;

                        case DuDestroyer.VolumeCenterMode.FixedWorldPosition:
                            PropertyField(m_VolumeCenter);
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize);
                            break;

                        case DuDestroyer.VolumeCenterMode.SourceObject:
                            PropertyField(m_VolumeSourceCenter);
                            PropertyField(m_VolumeOffset);
                            PropertyField(m_VolumeSize);
                            break;
                    }
                    break;

                default:
                    return;
            }

            Space();

            PropertyField(m_DisableColliders);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Space();

            if (DustGUI.FoldoutBegin("Events", "DuDestroyer.Events", false))
            {
                PropertyField(m_OnDestroy);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (destroyMode)
            {
                case DuDestroyer.DestroyMode.Manual:
                    DustGUI.HelpBoxInfo("To destroy this GameObject call Destroy() method");
                    break;

                case DuDestroyer.DestroyMode.Time:
                case DuDestroyer.DestroyMode.TimeRange:
                    // Nothing need to show here
                    break;

                case DuDestroyer.DestroyMode.AliveZone:
                    DustGUI.HelpBoxInfo("GameObject will be destroyed\nwhen it will leave Alive Zone");
                    break;

                case DuDestroyer.DestroyMode.DeadZone:
                    DustGUI.HelpBoxInfo("GameObject will be destroyed\nwhen it will get inside Dead Zone");
                    break;

                default:
                    return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (Application.isPlaying && targets.Length == 1)
            {
                if (destroyMode == DuDestroyer.DestroyMode.Time || destroyMode == DuDestroyer.DestroyMode.TimeRange)
                {
                    var main = target as DuDestroyer;

                    if (main.timeLimit > 0)
                    {
                        var progressBarState = Mathf.Max(1f - main.timeAlive / main.timeLimit, 0f);
                        var progressBarTitle = Mathf.Max(main.timeLimit - main.timeAlive, 0f).ToString("F1") + " sec";

                        var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                        EditorGUI.ProgressBar(rect, progressBarState, progressBarTitle);

                        DustGUI.ForcedRedrawInspector(this);
                    }
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Post Update

            if (m_VolumeSize.isChanged)
                m_VolumeSize.valVector3 = DuDestroyer.Normalizer.VolumeSize(m_VolumeSize.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
