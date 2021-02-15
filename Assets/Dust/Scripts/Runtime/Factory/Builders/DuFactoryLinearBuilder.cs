using UnityEngine;

namespace DustEngine
{
    public class DuFactoryLinearBuilder : DuFactoryBuilder
    {
        public override void Initialize(DuFactory duFactory)
        {
            var linearFactory = duFactory as DuLinearFactory;

            base.Initialize(duFactory);

            int instancesCount = linearFactory.offset + linearFactory.count;

            Vector3 genPosition = Vector3.zero;
            Vector3 genRotation = Vector3.zero;
            Vector3 genScale = Vector3.one;
            Vector3 genUvw = Vector3.zero;

            for (int instanceIndex = 0; instanceIndex < instancesCount; instanceIndex++)
            {
                if (instanceIndex >= linearFactory.offset)
                {
                    var instanceState = new DuFactoryInstance.State();

                    instanceState.position = genPosition;
                    instanceState.rotation = genRotation;
                    instanceState.scale = genScale;
                    instanceState.uvw = genUvw;

                    m_InstancesStates.Add(instanceState);
                }

                genPosition += linearFactory.position * linearFactory.amount;
                genRotation += linearFactory.rotation * linearFactory.amount;

                if (!linearFactory.stepRotation.Equals(Vector3.zero))
                {
                    var stepRotation = linearFactory.stepRotation * linearFactory.amount;

                    genPosition = DuMath.RotatePoint(genPosition, stepRotation);
                    genRotation += stepRotation;
                }

                genScale = Vector3.one + (linearFactory.scale - Vector3.one) * instanceIndex * linearFactory.amount;

                if (instancesCount > 1)
                    genUvw = Vector3.Lerp(Vector3.zero, Vector3.right, 1f / (instancesCount - 1) * instanceIndex);
                else
                    genUvw = Vector3.zero;
            }
        }
    }
}
