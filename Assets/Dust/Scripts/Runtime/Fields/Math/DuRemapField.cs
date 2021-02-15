using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Remap Field")]
    public class DuRemapField : DuMathField
    {
        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, remapping);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Remap";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            result.fieldPower = remapping.MapValue(fieldPoint.endPower);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
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
    }
}
