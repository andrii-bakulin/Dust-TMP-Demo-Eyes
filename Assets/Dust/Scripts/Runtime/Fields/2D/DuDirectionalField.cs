using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/2D Fields/Directional Field")]
    public class DuDirectionalField : DuSpaceField
    {
        [SerializeField]
        private float m_Length = 1.0f;
        public float length
        {
            get => m_Length;
            set => m_Length = ShapeNormalizer.Length(value);
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.XPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_GizmoWidth = 4.0f;
        public float gizmoWidth
        {
            get => m_GizmoWidth;
            set => m_GizmoWidth = ShapeNormalizer.GizmoWidth(value);
        }

        [SerializeField]
        private float m_GizmoHeight = 2.0f;
        public float gizmoHeight
        {
            get => m_GizmoHeight;
            set => m_GizmoHeight = ShapeNormalizer.GizmoHeight(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, length);
            DuDynamicState.Append(ref dynamicState, ++seq, direction);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Directional";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            float offset = 0f;

            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            float distanceToPoint;

            switch (direction)
            {
                default:
                case Axis6xDirection.XPlus:  distanceToPoint = -localPosition.x; break;
                case Axis6xDirection.XMinus: distanceToPoint = +localPosition.x; break;
                case Axis6xDirection.YPlus:  distanceToPoint = -localPosition.y; break;
                case Axis6xDirection.YMinus: distanceToPoint = +localPosition.y; break;
                case Axis6xDirection.ZPlus:  distanceToPoint = -localPosition.z; break;
                case Axis6xDirection.ZMinus: distanceToPoint = +localPosition.z; break;
            }

            if (DuMath.IsNotZero(length))
            {
                float halfLength = length / 2f;
                offset = DuMath.Fit(-halfLength, +halfLength, 0f, 1f, distanceToPoint);
            }
            else
            {
                offset = distanceToPoint >= 0f ? +1f : -1f;
            }

            result.fieldPower = remapping.MapValue(1f - offset);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            float halfLength = length / 2f;

            Vector3 plainSize = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(0.001f, gizmoHeight, gizmoWidth));
            Vector3 offsetPlane0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-halfLength, 0f, 0f));
            Vector3 offsetPlane1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(+halfLength, 0f, 0f));

            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapForceEnabled)
            {
                // End plane
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                Gizmos.DrawWireCube(offsetPlane1, plainSize);

                // Middle plane
                Gizmos.DrawWireCube(DuVector3.Fit01To(offsetPlane0, offsetPlane1, 1f - remapping.offset), plainSize);

                // Begin plane
                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                Gizmos.DrawWireCube(offsetPlane0, plainSize);
            }
            else
            {
                // End plane
                Gizmos.color = colorRange0;
                Gizmos.DrawWireCube(offsetPlane0, plainSize);

                // Begin plane
                Gizmos.color = colorRange1;
                Gizmos.DrawWireCube(offsetPlane1, plainSize);
            }

            // 3: Draw arrow
            float arrowSign = remapping.remapForceEnabled && remapping.invert ? -1f : +1f;
            Gizmos.color = colorRange1;
            Gizmos.DrawRay(offsetPlane0 * arrowSign, (offsetPlane1 - offsetPlane0) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, +0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, -0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, +0.06f, 0f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, -0.06f, 0f) * halfLength) * arrowSign);
        }

        private void Reset()
        {
            ResetDefaults();
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ShapeNormalizer
        {
            public static float Length(float value)
            {
                return Mathf.Max(0f, value);
            }

            public static float GizmoWidth(float value)
            {
                return Mathf.Abs(value);
            }

            public static float GizmoHeight(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
