using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/3D Fields/Cylinder Field")]
    public class DuCylinderField : DuSpaceField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = ShapeNormalizer.Radius(value);
        }

        [SerializeField]
        private float m_Height = 2.0f;
        public float height
        {
            get => m_Height;
            set => m_Height = ShapeNormalizer.Height(value);
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
            DuDynamicState.Append(ref dynamicState, ++seq, height);
            DuDynamicState.Append(ref dynamicState, ++seq, direction);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Cylinder";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to [X+]-axis-space by direction
            localPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            float distanceToPoint = localPosition.magnitude;
            float distanceToEdge = DuMath.Cylinder.DistanceToEdge(radius, height, localPosition);

            float offset = distanceToEdge > 0f ? 1f - distanceToPoint / distanceToEdge : 0f;

            result.fieldPower = remapping.MapValue(offset);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            float innerScale = remapping.offset;

            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapForceEnabled)
            {
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                DuGizmos.DrawWireCylinder(radius * innerScale, height * innerScale, Vector3.zero, direction, 32, 4);

                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                DuGizmos.DrawWireCylinder(radius, height, Vector3.zero, direction, 32, 4);
            }
            else
            {
                Gizmos.color = colorRange0;
                DuGizmos.DrawWireCylinder(radius, height, Vector3.zero, direction, 32, 4);
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ShapeNormalizer
        {
            public static float Height(float value)
            {
                return Mathf.Abs(value);
            }

            public static float Radius(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
