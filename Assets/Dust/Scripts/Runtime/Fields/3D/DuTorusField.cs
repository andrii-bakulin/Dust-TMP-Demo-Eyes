using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/3D Fields/Torus Field")]
    public class DuTorusField : DuSpaceField
    {
        [SerializeField]
        private float m_Radius = 2f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = Normalizer.Radius(value);
        }

        [SerializeField]
        private float m_Thickness = 0.5f;
        public float thickness
        {
            get => m_Thickness;
            set => m_Thickness = Normalizer.Thickness(value);
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, radius);
            DuDynamicState.Append(ref dynamicState, ++seq, thickness);
            DuDynamicState.Append(ref dynamicState, ++seq, direction);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Torus";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            float offset = 0f;

            if (DuMath.IsNotZero(radius) && DuMath.IsNotZero(thickness))
            {
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

                // Convert to [X+]-axis-space by direction
                localPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

                // Convert 3D point to 2D (x; y-&-z) -> (x; y)
                Vector2 localPoint2D = new Vector2(localPosition.x, DuMath.Length(localPosition.y, localPosition.z));
                localPoint2D.duAbs();

                // Move center to torus radius (center of thickness-radius)
                localPoint2D.y -= radius;

                float distanceToPoint = localPoint2D.magnitude;
                float distanceToEdge = thickness;

                offset = 1f - distanceToPoint / distanceToEdge;
            }

            result.fieldPower = remapping.MapValue(offset);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapForceEnabled)
            {
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                DuGizmos.DrawWireTorus(radius, thickness * remapping.offset, Vector3.zero, direction, 64, 32);

                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                DuGizmos.DrawWireTorus(radius, thickness, Vector3.zero, direction, 64, 32);
            }
            else
            {
                Gizmos.color = colorRange0;
                DuGizmos.DrawWireTorus(radius, thickness, Vector3.zero, direction, 64, 32);
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static float Radius(float value)
            {
                return Mathf.Abs(value);
            }

            public static float Thickness(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
