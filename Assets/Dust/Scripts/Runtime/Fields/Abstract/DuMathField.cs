using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuMathField : DuField
    {
        public override DuFieldsMap.FieldRecord.BlendPowerMode GetBlendPowerMode()
        {
            return DuFieldsMap.FieldRecord.BlendPowerMode.Set;
        }

        public override DuFieldsMap.FieldRecord.BlendColorMode GetBlendColorMode()
        {
            return DuFieldsMap.FieldRecord.BlendColorMode.Set;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return false;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = 0f;
            return null;
        }
#endif
    }
}
