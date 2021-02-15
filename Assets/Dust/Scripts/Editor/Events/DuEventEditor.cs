using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuEventEditor : DuEditor
    {
        protected static DuEvent AddComponentByEventType(System.Type eventType)
        {
            if (Dust.IsNull(Selection.activeGameObject))
                return null;

            return Selection.activeGameObject.AddComponent(eventType) as DuEvent;
        }

        public override void OnInspectorGUI()
        {
            // Hide base implementation
        }
    }
}
