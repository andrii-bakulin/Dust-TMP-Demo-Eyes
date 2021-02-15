using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Fields Space")]
    public class DuFieldsSpace : DuMonoBehaviour
    {
        [SerializeField]
        private DuFieldsMap m_FieldsMap = DuFieldsMap.FieldsSpace();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuField.Point m_CalcFieldPoint = new DuField.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPower(Vector3 worldPosition)
            => GetPower(worldPosition, 0f);

        public float GetPower(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.endPower;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Color GetColor(Vector3 worldPosition)
            => GetColor(worldPosition, 0f);

        public Color GetColor(Vector3 worldPosition, float offset)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.endColor;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public float GetPowerAndColor(Vector3 worldPosition, out Color color)
            => GetPowerAndColor(worldPosition, 0f, out color);

        public float GetPowerAndColor(Vector3 worldPosition, float offset, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = offset;

            m_CalcFieldPoint.inFactoryInstanceState = null;

            fieldsMap.Calculate(m_CalcFieldPoint);

            color = m_CalcFieldPoint.endColor;
            return m_CalcFieldPoint.endPower;
        }
    }
}
