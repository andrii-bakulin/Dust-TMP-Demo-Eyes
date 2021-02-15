using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuSpaceField : DuField
    {
        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GizmoVisibility m_GizmoVisibility = GizmoVisibility.DrawOnSelect;
        public GizmoVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        [SerializeField]
        private bool m_GizmoFieldColor = true;
        public bool gizmoFieldColor
        {
            get => m_GizmoFieldColor;
            set => m_GizmoFieldColor = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return remapping.colorMode != DuRemapping.ColorMode.Ignore;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            return GetFieldColorPreview(remapping, out colorPower);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, transform);
            DuDynamicState.Append(ref dynamicState, ++seq, remapping);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmoVisibility.AlwaysDraw)
                return;

            DrawFieldGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (gizmoVisibility == GizmoVisibility.AlwaysHide)
                return;

            DrawFieldGizmos();
        }

        protected abstract void DrawFieldGizmos();

        protected Color GetGizmoColorRange0()
        {
            return gizmoFieldColor ? remapping.color * 0.66f : k_GizmosColorRangeZero;
        }

        protected Color GetGizmoColorRange1()
        {
            return gizmoFieldColor ? remapping.color : k_GizmosColorRangeOne;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetDefaults();
        }

        protected void ResetDefaults()
        {
            // Use this method to reset values for default to remapping object
        }
    }
}
