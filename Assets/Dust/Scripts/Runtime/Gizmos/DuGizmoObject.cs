using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuGizmoObject : DuMonoBehaviour
    {
        protected static readonly Color k_GizmosDefaultColor = new Color(1.00f, 0.66f, 0.33f);

        public enum GizmosVisibility
        {
            DrawOnSelect = 1,
            AlwaysDraw = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_Color = k_GizmosDefaultColor;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        private GizmosVisibility m_GizmoVisibility = GizmosVisibility.AlwaysDraw;

        public GizmosVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string GizmoName();

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmosVisibility.AlwaysDraw)
                return;

            DrawGizmos();
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos();
        }

        protected abstract void DrawGizmos();
#endif
    }
}
