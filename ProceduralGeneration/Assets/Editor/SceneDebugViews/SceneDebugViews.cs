using System.Collections.Generic;
using ClockworkEditor.SceneViewDebugModes;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace

    public class SceneDebugViews
    {
        private static readonly Dictionary<SceneView, SceneView.CameraMode> PreviousCameraMode =
            new Dictionary<SceneView, SceneView.CameraMode>();

        [InitializeOnLoadMethod]
        public static void HookIntoSceneView()
        {
            // Delay so Params are accessible.
            EditorApplication.delayCall += () =>
            {
                // Add draw modes to dropdown.
                SceneView.ClearUserDefinedCameraModes();
                for (int i = 0; i < SceneDebugViewsAsset.Instance.DebugDrawModes.Count; i++)
                {
                    if (SceneDebugViewsAsset.Instance.DebugDrawModes[i].name != "" &&
                        SceneDebugViewsAsset.Instance.DebugDrawModes[i].category != "")
                    {
                        SceneView.AddCameraMode(SceneDebugViewsAsset.Instance.DebugDrawModes[i].name,
                            SceneDebugViewsAsset.Instance.DebugDrawModes[i].category);
                    }
                }

                // Hook into editor to swap the shader when selecting the debug mode.
                EditorApplication.update += () =>
                {
                    foreach (SceneView view in SceneView.sceneViews)
                    {
                        if (!PreviousCameraMode.ContainsKey(view) || PreviousCameraMode[view] != view.cameraMode)
                        {
                            view.SetSceneViewShaderReplace(GetDrawModeShader(view.cameraMode), "");
                        }

                        PreviousCameraMode[view] = view.cameraMode;
                    }
                };
            };
        }

        private static Shader GetDrawModeShader(SceneView.CameraMode mode)
        {
            for (int i = 0; i < SceneDebugViewsAsset.Instance.DebugDrawModes.Count; i++)
            {
                if (SceneDebugViewsAsset.Instance.DebugDrawModes[i].name == "" ||
                    mode.name != SceneDebugViewsAsset.Instance.DebugDrawModes[i].name)
                {
                    continue;
                }

                Shader shader = SceneDebugViewsAsset.Instance.DebugDrawModes[i].shader;
                if (shader != null)
                {
                    return shader;
                }
            }

            return null;
        }
    }
