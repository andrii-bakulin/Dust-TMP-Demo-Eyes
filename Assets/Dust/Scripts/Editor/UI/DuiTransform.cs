using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class DuiTransform : DuEditor
    {
        // @Thanks for:
        // https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/

        //Unity's built-in editor
        private Editor defaultEditor;

        private bool meshInfoRead = false;
        private string meshInfoMessage = "";

        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector

            defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable

            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor,null);

            DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Extend UI

            bool isRequireShowMeshInfo = DuSessionState.GetBool("DuiTransform.ShowMeshInfo", false);

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(UI.Icons.GAME_OBJECT_STATS, isRequireShowMeshInfo ? DustGUI.ButtonState.Pressed : DustGUI.ButtonState.Normal))
                {
                    isRequireShowMeshInfo = !isRequireShowMeshInfo;
                    DuSessionState.SetBool("DuiTransform.ShowMeshInfo", isRequireShowMeshInfo);
                }

                if (DustGUI.IconButton(UI.Icons.TRANSFORM_RESET))
                {
                    foreach (var subTarget in targets)
                    {
                        Undo.RecordObject(subTarget, "Reset Transform");
                        DuTransform.Reset(subTarget as Transform);
                    }
                }
            }
            DustGUI.EndHorizontal();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (isRequireShowMeshInfo)
            {
                if (!meshInfoRead)
                {
                    var total = new DuGameObject.Data();

                    foreach (var subTarget in targets)
                    {
                        total += DuGameObject.GetStats((subTarget as Transform).gameObject, true);
                    }

                    if (total.meshesCount > 0)
                    {
                        meshInfoMessage += "Meshes Count: " + total.meshesCount.ToString() + "\n";
                        meshInfoMessage += "Vertex Count: " + total.vertexCount.ToString() + "\n";

                        if (total.unreadableCount > 0)
                        {
                            meshInfoMessage += "Triangles Count: " + total.triangleCount.ToString() + "*\n";

                            if (total.unreadableCount == 1)
                                meshInfoMessage += "* 1 mesh is unreadable, so value is not exact";
                            else
                                meshInfoMessage += "* " + total.unreadableCount + " meshes are unreadable, so value is not exact";
                        }
                        else
                        {
                            meshInfoMessage += "Triangles Count: " + total.triangleCount.ToString();
                        }
                    }
                    else
                    {
                        meshInfoMessage += "Meshes not found";
                    }

                    meshInfoRead = true;
                }

                DustGUI.HelpBoxInfo(meshInfoMessage);
            }
        }
    }
}
