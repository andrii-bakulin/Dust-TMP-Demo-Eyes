using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    /// <summary>
    ///
    /// Main idea for color usage:
    /// - R+G+B define color
    /// - A     define intensity of color in this point
    ///
    /// If need change intensity of some color only require multiple A for required value
    ///
    /// </summary>
    [Serializable]
    public class DuFieldsMap : DuDynamicStateInterface
    {
        [Serializable]
        public class FieldRecord
        {
            public enum BlendPowerMode
            {
                Ignore = 0,
                Set = 1,
                Add = 2,
                Subtract = 3,
                Multiply = 4,
                Divide = 5,
                Avg = 6,
                Min = 7,
                Max = 8,
            }

            public enum BlendColorMode
            {
                Ignore = 0,
                Blend = 1,
                Set = 2,
                Add = 3,
                Subtract = 4,
                Multiply = 5,
                Min = 6,
                Max = 7,
            }

            //----------------------------------------------------------------------------------------------------------

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }

            [SerializeField]
            private DuField m_Field = null;
            public DuField field
            {
                get => m_Field;
                set => m_Field = value;
            }

            [SerializeField]
            private BlendPowerMode m_BlendPowerMode = BlendPowerMode.Set;
            public BlendPowerMode blendPowerMode
            {
                get => m_BlendPowerMode;
                set => m_BlendPowerMode = value;
            }

            [SerializeField]
            private BlendColorMode m_BlendColorMode = BlendColorMode.Set;
            public BlendColorMode blendColorMode
            {
                get => m_BlendColorMode;
                set => m_BlendColorMode = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public enum FieldsMapMode
        {
            Custom = 0,
            FieldsSpace = 1,
            FactoryMachine = 2,
#if DUST_ALPHA_DEFORMERS
            Deformer = 3,
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private FieldsMapMode m_FieldsMapMode = FieldsMapMode.Custom;
        public FieldsMapMode fieldsMapMode => m_FieldsMapMode;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_CalculatePower;
        public bool calculatePower
        {
            get => m_CalculatePower;
            set => m_CalculatePower = value;
        }

        [SerializeField]
        private float m_DefaultPower = 0f;
        public float defaultPower
        {
            get => m_DefaultPower;
            set => m_DefaultPower = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_CalculateColor;
        public bool calculateColor
        {
            get => m_CalculateColor;
            set => m_CalculateColor = value;
        }

        [SerializeField]
        private Color m_DefaultColor = Color.clear;
        public Color defaultColor
        {
            get => m_DefaultColor;
            set => m_DefaultColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected List<FieldRecord> m_Fields = new List<FieldRecord>();
        public List<FieldRecord> fields => m_Fields;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuField.Point m_CalcFieldPoint = new DuField.Point();

        //--------------------------------------------------------------------------------------------------------------

#if DUST_ALPHA_DEFORMERS
        public static DuFieldsMap Deformer()
        {
            return new DuFieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.Deformer,

                calculatePower = true,
                calculateColor = false,

                defaultPower = 1f,
            };
        }
#endif

        public static DuFieldsMap FactoryMachine()
        {
            return new DuFieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.FactoryMachine,

                calculatePower = true,
                calculateColor = true,

                defaultPower = 1f,
                defaultColor = Color.clear,
            };
        }

        public static DuFieldsMap FieldsSpace()
        {
            return new DuFieldsMap
            {
                m_FieldsMapMode = FieldsMapMode.FieldsSpace,

                calculatePower = true,
                calculateColor = true,

                defaultPower = 0f,
                defaultColor = Color.black,
            };
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, calculatePower);
            DuDynamicState.Append(ref dynamicState, ++seq, defaultPower);
            DuDynamicState.Append(ref dynamicState, ++seq, calculateColor);
            DuDynamicState.Append(ref dynamicState, ++seq, defaultColor);

            DuDynamicState.Append(ref dynamicState, ++seq, fields.Count);

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.enabled);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.field);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendPowerMode);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendColorMode);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.intensity);
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float power)
        {
            if (!HasFields())
            {
                power = defaultPower;
                return false;
            }

            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            m_CalcFieldPoint.inFactoryInstanceState = null;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.endPower;

            return result;
        }

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float power, out Color color)
        {
            if (!HasFields())
            {
                power = defaultPower;
                color = defaultColor;
                return false;
            }

            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            m_CalcFieldPoint.inFactoryInstanceState = null;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.endPower;
            color = m_CalcFieldPoint.endColor;

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(DuFactoryMachine.FactoryInstanceState factoryInstanceState, out float power, out Color color)
        {
            if (!HasFields())
            {
                power = defaultPower;
                color = defaultColor;
                return false;
            }

            m_CalcFieldPoint.inPosition = factoryInstanceState.factory.GetPositionInWorldSpace(factoryInstanceState.instance);
            m_CalcFieldPoint.inOffset = factoryInstanceState.instance.offset;

            m_CalcFieldPoint.inFactoryInstanceState = factoryInstanceState;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.endPower;
            color = m_CalcFieldPoint.endColor;

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(DuField.Point fieldPoint)
        {
            fieldPoint.endPower = defaultPower;
            fieldPoint.endColor = defaultColor;

            if (fields.Count == 0)
                return false;

            DuField.Result fieldResultData;

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                bool calculatePowerNow = calculatePower
                                         && fieldRecord.blendPowerMode != FieldRecord.BlendPowerMode.Ignore;

                bool calculateColorNow = calculateColor
                                         && fieldRecord.blendColorMode != FieldRecord.BlendColorMode.Ignore
                                         && fieldRecord.field.IsAllowCalculateFieldColor();

                fieldRecord.field.Calculate(fieldPoint, out fieldResultData, calculateColorNow);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate Power

                if (calculatePowerNow)
                {
                    float afterBlendPower;

                    switch (fieldRecord.blendPowerMode)
                    {
                        default:
                        case FieldRecord.BlendPowerMode.Set:
                            afterBlendPower = fieldResultData.fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Add:
                            afterBlendPower = fieldPoint.endPower + fieldResultData.fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Subtract:
                            afterBlendPower = fieldPoint.endPower - fieldResultData.fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Multiply:
                            afterBlendPower = fieldPoint.endPower * fieldResultData.fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Divide:
                            afterBlendPower = DuMath.IsNotZero(fieldResultData.fieldPower) ? fieldPoint.endPower / fieldResultData.fieldPower : 0f;
                            break;

                        case FieldRecord.BlendPowerMode.Avg:
                            afterBlendPower = (fieldPoint.endPower + fieldResultData.fieldPower) / 2f;
                            break;

                        case FieldRecord.BlendPowerMode.Min:
                            afterBlendPower = Mathf.Min(fieldPoint.endPower, fieldResultData.fieldPower);
                            break;

                        case FieldRecord.BlendPowerMode.Max:
                            afterBlendPower = Mathf.Max(fieldPoint.endPower, fieldResultData.fieldPower);
                            break;
                    }

                    fieldPoint.endPower = Mathf.LerpUnclamped(fieldPoint.endPower, afterBlendPower, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate new color

                if (calculateColorNow)
                {
                    Color blendedColor;

                    switch (fieldRecord.blendColorMode)
                    {
                        default:
                        case FieldRecord.BlendColorMode.Set:
                            blendedColor = fieldResultData.fieldColor;
                            break;

                        case FieldRecord.BlendColorMode.Blend:
                            blendedColor = DuColorBlend.AlphaBlend(fieldPoint.endColor, fieldResultData.fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Add:
                            blendedColor = DuColorBlend.Add(fieldPoint.endColor, fieldResultData.fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Subtract:
                            blendedColor = DuColorBlend.Subtract(fieldPoint.endColor, fieldResultData.fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Multiply:
                            blendedColor = DuColorBlend.Multiply(fieldPoint.endColor, fieldResultData.fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Min:
                            blendedColor = DuColorBlend.MinAfterBlend(fieldPoint.endColor, fieldResultData.fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Max:
                            blendedColor = DuColorBlend.MaxAfterBlend(fieldPoint.endColor,fieldResultData.fieldColor);
                            break;
                    }

                    fieldPoint.endColor = Color.Lerp(fieldPoint.endColor, blendedColor, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            }

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool HasFields()
        {
            return fields.Count > 0;
        }

        public FieldRecord.BlendPowerMode GetDefaultBlendPower(DuField field)
        {
            var blendMode = field.GetBlendPowerMode();

            if (blendMode != FieldRecord.BlendPowerMode.Ignore)
                return blendMode;

            return fields.Count == 0 ? FieldRecord.BlendPowerMode.Set : FieldRecord.BlendPowerMode.Max;
        }

        public FieldRecord.BlendColorMode GetDefaultBlendColor(DuField field)
        {
            var blendMode = field.GetBlendColorMode();

            if (blendMode != FieldRecord.BlendColorMode.Ignore)
                return blendMode;

            return FieldRecord.BlendColorMode.Blend;
        }

        //--------------------------------------------------------------------------------------------------------------

        public FieldRecord AddField(DuField field)
        {
            var fieldRecord = new FieldRecord
            {
                field = field,
                enabled = true,
                intensity = 1f,
                blendPowerMode = GetDefaultBlendPower(field),
                blendColorMode = GetDefaultBlendColor(field),
            };

            fields.Add(fieldRecord);

            return fieldRecord;
        }
    }
}
