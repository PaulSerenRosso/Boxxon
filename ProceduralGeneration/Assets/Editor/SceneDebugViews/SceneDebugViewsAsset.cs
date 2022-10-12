using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace ClockworkEditor.SceneViewDebugModes
{
    [CreateAssetMenu(fileName = "SceneDebugViews.asset", menuName = "ASG/SceneDebugViewsAsset")]
    public class SceneDebugViewsAsset : ScriptableObject
    {
        [Serializable]
        public struct CustomDrawMode
        {
            public string name;
            public string category;
            public Shader shader;
        }
        
        public List<CustomDrawMode> DebugDrawModes;

        private static SceneDebugViewsAsset cachedInstance;

        /// <summary>
        ///     Retrieves the singleton asset for this ScriptParams type. If none or more than one asset exists for the type,
        ///     an error is thrown.
        /// </summary>
        public static SceneDebugViewsAsset Instance
        {
            get
            {
                if (cachedInstance == null)
                {
                    cachedInstance = AssetDatabase.LoadAssetAtPath<SceneDebugViewsAsset>(
                            "Assets/Editor/SceneDebugViews/SceneDebugViews.asset");
                }

                return cachedInstance;
            }
        }
    }
}
