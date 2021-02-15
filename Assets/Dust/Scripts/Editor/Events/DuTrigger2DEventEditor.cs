using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTrigger2DEvent))]
    [CanEditMultipleObjects]
    public class DuTrigger2DEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/On Trigger 2D")]
        public static DuTrigger2DEvent AddComponent()
        {
            return AddComponentByEventType(typeof(DuTrigger2DEvent)) as DuTrigger2DEvent;
        }
    }
}
