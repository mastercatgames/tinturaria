using InfinityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InfinityEditor
{
    /// <summary>
    /// Serialize inpector data.
    /// </summary>
    internal class InspectorData : ScriptableObject
    {
        private const string ScriptableObjectPath = "Assets/InfinityEngine/Gen/Resources/InspectorData.asset";
        private const string ScriptableObjectName = "InspectorData";
        private static InspectorData mInstance;

        [HideInInspector]
        [SerializeField]
        private List<BoolKV> bools;

        [HideInInspector]
        [SerializeField]
        private List<IntKV> ints;

        [HideInInspector]
        [SerializeField]
        private List<FloatKV> floats;

        [HideInInspector]
        [SerializeField]
        private List<StringKV> strings;

        [HideInInspector]
        [SerializeField]
        private bool mIsInitialized;

        public static InspectorData Instance
        {
            get
            {
                if (!Application.isEditor)
                    return null;

                if (mInstance == null)
                {
                    mInstance = Resources.Load<InspectorData>(ScriptableObjectName);

                    if (mInstance == null)
                    {
                        mInstance = CreateInstance<InspectorData>();
                        SaveDataBase();
                    }
                }
                return mInstance;
            }
        }

        private void OnEnable()
        {
            if (!mIsInitialized)
            {

                bools = new List<BoolKV>();
                ints = new List<IntKV>();
                floats = new List<FloatKV>();
                strings = new List<StringKV>();

                hideFlags = HideFlags.DontUnloadUnusedAsset;
                mIsInitialized = true;
            }
        }

        public static bool GetBool(string name)
        {
            var result = Instance.bools.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.bools.Add(new BoolKV(name, false));
                return false;
            }
            return result.Value;
        }

        public static void SetBool(string name, bool value)
        {
            var result = Instance.bools.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.bools.Add(new BoolKV(name, value));
            }
            else
            {
                result.Value = value;
            }
        }

        public static int GetInt(string name)
        {
            var result = Instance.ints.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.ints.Add(new IntKV(name, 0));
                return 0;
            }
            return result.Value;
        }

        public static void SetInt(string name, int value)
        {
            var result = Instance.ints.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.ints.Add(new IntKV(name, value));
            }
            else
            {
                result.Value = value;
            }
        }

        public static float GetFloat(string name)
        {
            var result = Instance.floats.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.floats.Add(new FloatKV(name, 0f));
                return 0f;
            }
            return result.Value;
        }

        public static void SetFloat(string name, float value)
        {
            var result = Instance.floats.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.floats.Add(new FloatKV(name, value));
            }
            else
            {
                result.Value = value;
            }
        }

        public static string GetString(string name)
        {
            var result = Instance.strings.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.strings.Add(new StringKV(name, ""));
                return "";
            }
            return result.Value;
        }

        public static void SetString(string name, string value)
        {
            var result = Instance.strings.FirstOrDefault(elem => elem.Key == name);
            if (result == null)
            {
                Instance.strings.Add(new StringKV(name, value));
            }
            else
            {
                result.Value = value;
            }
        }

        private static void SaveDataBase()
        {
            AssetDatabase.SaveAssets();
            var directoryPath = ScriptableObjectPath.Replace($"/{ScriptableObjectName}.asset", string.Empty);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var asset = AssetDatabase.LoadAssetAtPath<InspectorData>(ScriptableObjectPath);
            if (asset == null)
            {
                AssetDatabase.CreateAsset(Instance, ScriptableObjectPath);
            }
            else
            {
                EditorUtility.CopySerialized(Instance, asset);
            }

        }

        public static void Clear()
        {
            Instance.bools.Clear();
            Instance.ints.Clear();
            Instance.floats.Clear();
            Instance.strings.Clear();
        }
    }
}
