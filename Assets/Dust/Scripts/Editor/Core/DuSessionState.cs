using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public static class DuSessionState
    {
        public static void SetBool(string key, bool value)
        {
            SessionState.SetBool(GetKey(key), value);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return SessionState.GetBool(GetKey(key), defaultValue);
        }

        //------------------------------------------------------------------------------------------------------------------

        public static void SetFloat(string key, float value)
        {
            SessionState.SetFloat(GetKey(key), value);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            return SessionState.GetFloat(GetKey(key), defaultValue);
        }

        //------------------------------------------------------------------------------------------------------------------

        public static void SetVector3(string key, Vector3 value)
        {
            SessionState.SetVector3(GetKey(key), value);
        }

        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return SessionState.GetVector3(GetKey(key), defaultValue);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void SetVector3(string key, Object target, Vector3 value)
        {
            SessionState.SetVector3(GetKey(key) + "." + target.GetInstanceID(), value);
        }

        public static Vector3 GetVector3(string key, Object target, Vector3 defaultValue)
        {
            return SessionState.GetVector3(GetKey(key) + "." + target.GetInstanceID(), defaultValue);
        }

        //------------------------------------------------------------------------------------------------------------------

        private static string GetKey(string key)
        {
            return "DustEngine." + key;
        }
    }
}
