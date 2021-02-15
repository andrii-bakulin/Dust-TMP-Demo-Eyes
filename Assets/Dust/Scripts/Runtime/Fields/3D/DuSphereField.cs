using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/3D Fields/Sphere Field")]
    public class DuSphereField : DuSpaceField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = ShapeNormalizer.Radius(value);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, radius);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Sphere";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            float offset = 0f;

            if (DuMath.IsNotZero(radius))
            {
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

                float distanceToPoint = localPosition.magnitude;
                float distanceToEdge = radius;

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
                Gizmos.DrawWireSphere(Vector3.zero, radius * remapping.offset);

                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            }
            else
            {
                Gizmos.color = colorRange0;
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ShapeNormalizer
        {
            public static float Radius(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
