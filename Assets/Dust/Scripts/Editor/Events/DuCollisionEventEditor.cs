using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollisionEvent))]
    [CanEditMultipleObjects]
    public class DuCollisionEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/On Collision")]
        public static DuCollisionEvent AddComponent()
        {
            return AddComponentByEventType(typeof(DuCollisionEvent)) as DuCollisionEvent;
        }
    }
}
