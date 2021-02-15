using UnityEngine;

namespace DustDemo
{
    public class DemoEvents : MonoBehaviour
    {
        public void LogMessage(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void AddUpVelocity(GameObject obj)
        {
            obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
        }
    }
}
