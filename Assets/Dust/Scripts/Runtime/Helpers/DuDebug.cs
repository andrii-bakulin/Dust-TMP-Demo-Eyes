using UnityEngine;

namespace DustEngine
{
    public class DuDebug : DuMonoBehaviour
    {
        //--------------------------------------------------------------------------------------------------------------
        // Log: string

        public static void LogNotice(string message)
        {
            Debug.Log("DuDebug: " + message);
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning("DuDebug: " + message);
        }

        public static void LogError(string message)
        {
            Debug.LogError("DuDebug: " + message);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Log: GameObject

        public static void LogNotice(GameObject gameObj)
        {
            Debug.Log("DuDebug.GameObject: " + gameObj);
        }

        public static void LogWarning(GameObject gameObj)
        {
            Debug.LogWarning("DuDebug.GameObject: " + gameObj);
        }

        public static void LogError(GameObject gameObj)
        {
            Debug.LogError("DuDebug.GameObject: " + gameObj);
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}
