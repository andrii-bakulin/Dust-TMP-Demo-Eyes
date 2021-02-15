using UnityEngine;

namespace DustEngine
{
    public static class DuTransform
    {
        public static void Reset(Transform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
    }
}
