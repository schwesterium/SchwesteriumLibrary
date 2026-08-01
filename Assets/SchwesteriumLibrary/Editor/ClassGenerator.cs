using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SchwesteriumLibrary.Editor
{
    public class ClassGenerator : EditorWindow
    {
        private string _folderPath = "Assets/Scripts";

        private string _userName = "YourName";

        private string _className = "MyClass";
        private string _namespase = "MySpace";

        [SerializeField]
        private string[] _inheritanceNames = new string[] { "MonoBehaviour" };

        private bool _multipleInheritance = false;
        private bool _useNamespace = true;
        private bool _includeMITLicense = false;

        [SerializeField]
        private SerializedObject _so = null;

        private Vector2 _scrollPosition = Vector2.zero;

        [MenuItem("SchwesteriumLibrary/Editor/ClassGenerator")]
        public static void ShowWindow()
        {
            var window = GetWindow<ClassGenerator>("ClassGenerator");
            window.Init();
            window.minSize = new Vector2(400f, 300f);
        }

        public void Init()
        {
            _so = new(this);
        }

        private void OnEnable()
        {
            _so = new(this);
        }

        private void OnGUI()
        {
            _so.Update();

            _userName = EditorGUILayout.TextField("User Name", _userName);

            EditorGUILayout.Space();

            _useNamespace = EditorGUILayout.Toggle("Use Namespace", _useNamespace);
            if (_useNamespace)
            {
                _namespase = EditorGUILayout.TextField("Namespace", _namespase);
            }

            EditorGUILayout.Space();

            _className = EditorGUILayout.TextField("Class Name", _className);

            EditorGUILayout.Space();

            _multipleInheritance = EditorGUILayout.Toggle("Use Multiple Inheritance", _multipleInheritance);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.PropertyField(_so.FindProperty("_inheritanceNames"));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            _includeMITLicense = EditorGUILayout.Toggle("Include MITLicense", _includeMITLicense);

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                _folderPath = EditorGUILayout.TextField("Output Folder", _folderPath);

                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    string selectedPath = EditorUtility.OpenFolderPanel("Select Output Folder", "Assets", "");
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        if (selectedPath.StartsWith(Application.dataPath))
                        {
                            _folderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate", GUILayout.Height(30)))
            {
                GenerateScript();
            }

            _so.ApplyModifiedProperties();
        }

        private void GenerateScript()
        {
            if (string.IsNullOrEmpty(_className))
            {
                EditorUtility.DisplayDialog("Error", "ÉNÉâÉXñºÇì¸óÕÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB", "OK");
                return;
            }

            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                EditorUtility.DisplayDialog("Error", "éwíËÇ≥ÇÍÇΩÉtÉHÉãÉ_Ç™ë∂ç›ÇµÇ‹ÇπÇÒÅB", "OK");
                return;
            }

            var result = true;

            result = CreateScript(_folderPath, _className, GetClassTemplate());

            AssetDatabase.Refresh();

            if (result)
            {
                EditorUtility.DisplayDialog("Success", $"{_className}.csÇê∂ê¨ÇµÇ‹ÇµÇΩ", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"{_className}.csÇÃê∂ê¨Ç…é∏îsÇµÇ‹ÇµÇΩ", "OK");
            }

        }

        private bool CreateScript(string folderPath, string fileName, string content)
        {
            string filePath = Path.Combine(_folderPath, fileName + ".cs");

            if (File.Exists(filePath))
            {
                Debug.LogWarning($"[Class Generator] : {fileName}.cs ÇÕä˘Ç…ë∂ç›ÇµÇƒÇ¢Ç‹Ç∑");
                return false;
            }

            File.WriteAllText(filePath, content, System.Text.Encoding.UTF8);
            Debug.Log($"[Class Generator] : Generated -> {filePath}");

            return true;
        }

        #region GetCodeFunc
        private string GetInheritanceNames()
        {
            var str = string.Empty;
            int count = 0;

            if (_multipleInheritance)
            {
                foreach (var name in _inheritanceNames)
                {
                    if (count >= _inheritanceNames.Length - 1) { str += name; }
                    else { str += name + ", "; }

                    ++count;
                }
            }
            else
            {
                str = _inheritanceNames[0];
            }

            return str;
        }

        private string GetMITLicense()
        {
            return $@"/*
Copyright (c) 2026 {_userName}

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the ÅgSoftwareÅh), to deal in the Software without 
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED ÅgAS ISÅh, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.
*/";
        }

        private string GetDetail()
        {
            return $@"{(_includeMITLicense ? GetMITLicense() : null)}/*
Author : {_userName}
Date   : {DateTime.Today:yyyy/MM/dd}
*/";
        }

        private string GetUsingNamespaces()
        {
            return $@"using System;
using UnityEngine;";
        }

        private string GetClassTemplate()
        {
            if (_useNamespace)
            {
                return $@"{GetDetail()}

{GetUsingNamespaces()}

namespace {_namespase}
{{
    public class {_className} : {GetInheritanceNames()}
    {{

    }}
}}";
            }
            else
            {
                return $@"{GetDetail()}

{GetUsingNamespaces()}

public class {_className} : {GetInheritanceNames()}
{{

}}";
            }
        }

        #endregion
    }
}