/*
Author : schwesterium
Date   : 2026/08/01
*/

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SchwesteriumLibrary.Editor
{
    public class ReplaceObjects : EditorWindow
    {
        private enum SeachingObjectMode
        {
            Name,
            Tag
        }

        private SeachingObjectMode currentSearchingObjectMode = SeachingObjectMode.Name;

        private string objectName = "ObjectName";

        private string selectedTag = "TagName";


        [SerializeField]
        private GameObject[] targetObjects = null;

        [SerializeField]
        private GameObject replaceObject = null;

        [SerializeField]
        private GameObject replacePrefab = null;

        //置換するオブジェクトがPrefabかどうかを判定するフラグ
        private bool isObject = false;

        private bool replacePosition = true;
        private bool replaceRotation = false;
        private bool replaceScale = false;

        [MenuItem("SchwesteriumLibrary/Editor/Replace Objects")]
        public static void ShowWindow()
        {
            GetWindow<ReplaceObjects>();
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 30;

            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 20;
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            var smolLabelStyle = new GUIStyle();
            smolLabelStyle.fontSize = 15;
            smolLabelStyle.normal.textColor = Color.white;
            smolLabelStyle.alignment = TextAnchor.MiddleCenter;

            var warningLabelStyle = new GUIStyle();
            warningLabelStyle.fontSize = 15;
            warningLabelStyle.normal.textColor = Color.red;
            warningLabelStyle.alignment = TextAnchor.MiddleCenter;

            //==================================置換するオブジェクトの指定==================================
            GUILayout.Space(5f);

            isObject = GUILayout.Toggle(isObject, "Prefabでないオブジェクトを使用する", "Button");

            GUILayout.Space(5f);

            //置換するオブジェクトを指定するGUI
            if (isObject)
            {
                GUILayout.Label("置換するオブジェクトを指定してください", labelStyle);
                PropGUI("replaceObject");

                if (replaceObject == null)
                {
                    GUILayout.Label("置換するオブジェクトが指定されていません", warningLabelStyle);
                    return;
                }
            }
            else
            {
                GUILayout.Label("置換するPrefabを指定してください", labelStyle);
                PropGUI("replacePrefab");

                if (replacePrefab == null)
                {
                    GUILayout.Label("置換するPrefabが指定されていません", warningLabelStyle);
                    return;
                }
            }



            //===========================置換対象オブジェクトの検索=========================================
            GUILayout.Space(15f);

            //検索GUI
            SearchGUI(smolLabelStyle);


            //=========================置換対象オブジェクトの指定=========================================
            GUILayout.Space(20f);

            GUILayout.Label("置換対象オブジェクトを指定してください", labelStyle);
            GUILayout.Label("検索した場合は自動で指定されます", smolLabelStyle);
            PropGUI("targetObjects");

            if (targetObjects == null || targetObjects.Length <= 0)
            {
                GUILayout.Label("置換対象オブジェクトが指定されていません", warningLabelStyle);
                return;
            }


            //================================置換設定の指定=========================================
            GUILayout.Space(10f);

            //置換設定GUI
            SettingGUI(labelStyle);

            GUILayout.Space(10f);

            //====================================置換の実行========================================
            GUILayout.Space(20f);

            //オブジェクト置換実行ボタン
            if (GUILayout.Button("オブジェクトを置換する!", GUILayout.Height(40)))
            {
                if (isObject)
                {
                    if (replaceObject == null)
                    {
                        Debug.LogError("置換するオブジェクトが指定されていません");
                        return;
                    }
                }
                else
                {
                    if (replacePrefab == null)
                    {
                        Debug.LogError("置換するPrefabが指定されていません");
                        return;
                    }
                }

                Replase();
            }

            GUILayout.Label("警告 : 親オブジェクトを置換する場合、子オブジェクトは破棄されます！", warningLabelStyle);
        }

        /// <summary>
        /// オブジェクトを検索する
        /// </summary>
        private void SearchGUI(GUIStyle style)
        {
            //置換対象オブジェクトのモードを選択GUI
            GUILayout.Label("置換対象オブジェクトの検索", style);
            currentSearchingObjectMode = (SeachingObjectMode)EditorGUILayout.EnumPopup("検索モード", currentSearchingObjectMode);

            GUILayout.BeginHorizontal();

            //オブジェクトの名前またはタグを入力して、検索を実行する
            switch (currentSearchingObjectMode)
            {
                case SeachingObjectMode.Name://名前で検索

                    GUILayout.Label("オブジェクト名前を入力してください");
                    objectName = GUILayout.TextField(objectName, GUILayout.Width(200f));
                    GUILayout.EndHorizontal();

                    //オブジェクトの名前を検索するボタン
                    if (GUILayout.Button("オブジェクトを検索する!", GUILayout.Height(40)))
                    {
                        var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>()
                            .Where(c => c.hideFlags == HideFlags.None && c.scene.IsValid() && c.scene.name != null && c.name == objectName)
                            .ToArray();

                        //オブジェクトの数が0の場合、警告を表示
                        if (objs.Length <= 0)
                        {
                            Debug.LogWarning($"指定した名前を : {objectName} のオブジェクトは見つかりませんでした");
                            return;
                        }

                        targetObjects = new GameObject[objs.Length];
                        targetObjects = objs;
                    }

                    break;

                case SeachingObjectMode.Tag://タグで検索

                    GUILayout.Label("オブジェクトのタグを選択してください");
                    selectedTag = EditorGUILayout.TagField(selectedTag);
                    GUILayout.EndHorizontal();

                    //タグのオブジェクトを検索するボタン
                    if (GUILayout.Button("オブジェクトを検索する!", GUILayout.Height(40)))
                    {
                        //tagで検索したオブジェクトを取得 omo function
                        var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>()
                            .Where(c => c.hideFlags == HideFlags.None && c.scene.IsValid() && c.scene.name != null && c.CompareTag(selectedTag))
                            .ToArray();

                        //オブジェクトの数が0の場合、警告を表示
                        if (objs.Length <= 0)
                        {
                            Debug.LogWarning($"指定したタグ : {selectedTag} のオブジェクトは見つかりませんでした");
                            return;
                        }

                        targetObjects = new GameObject[objs.Length];
                        targetObjects = objs;
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// プロパティを表示する
        /// </summary>
        /// <param name="propName"></param>
        private void PropGUI(string propName)
        {
            ScriptableObject target = this;

            SerializedObject so = new SerializedObject(target);

            SerializedProperty stringsProperty = so.FindProperty(propName);

            EditorGUILayout.PropertyField(stringsProperty, true);

            so.ApplyModifiedProperties();
        }

        private void SettingGUI(GUIStyle style)
        {
            GUILayout.Label("置換設定", style);
            replacePosition = GUILayout.Toggle(replacePosition, "位置を置換する", "Button");
            replaceRotation = GUILayout.Toggle(replaceRotation, "回転を置換する", "Button");
            replaceScale = GUILayout.Toggle(replaceScale, "スケールを置換する", "Button");
        }

        /// <summary>
        /// 置換処理を実行する
        /// </summary>
        private void Replase()
        {
            var replaceObj = isObject ? replaceObject : replacePrefab;

            //置換対象のオブジェクトを取得
            for (int i = 0; i < targetObjects.Length; i++)
            {
                if (targetObjects[i] == null)
                {
                    Debug.LogWarning($"TargetObjects Index = {i} は見つかりませんでした");
                    continue;
                }

                //置換対象のオブジェクトを取得
                GameObject targetObject = targetObjects[i];

                //置換対象のオブジェクトの親を取得
                var replaceParent = targetObject.transform.parent;

                //置換対象のオブジェクトの位置、回転、スケールを取得
                var settings = ReplasePropSetter(targetObject);

                //置換対象のオブジェクトの位置、回転、スケールを、置換するオブジェクトに設定
                replaceObj.transform.SetPositionAndRotation(settings.Item1, settings.Item2);
                replaceObj.transform.localScale = settings.Item3;

                //置換対象のオブジェクトを削除
                Undo.DestroyObjectImmediate(targetObject);

                //undo用に置換後のオブジェクトを保存
                GameObject newObj = null;

                //Prefabでないオブジェクトを使用する場合
                if (isObject)//Prefabでないオブジェクトで置換する処理
                {
                    newObj = replaceParent == null ? Instantiate(replaceObj) : Instantiate(replaceObj, replaceParent);
                }
                else//Prefabで置換する処理
                {
                    newObj = replaceParent == null 
                        ? PrefabUtility.InstantiatePrefab(replaceObj) as GameObject 
                        : PrefabUtility.InstantiatePrefab(replaceObj, replaceParent) as GameObject;
                }

                Undo.RegisterCreatedObjectUndo(newObj, "オブジェクトの置換");
            }

            //置換後、置換対象の配列をnullにする
            targetObjects = null;
        }

        /// <summary>
        /// 置定に基づいて、置換するオブジェクトの位置、回転、スケール取得する
        /// </summary>
        /// <param name="targetObject"></param>
        /// <return>位置、回転、スケール | 設定が無効な場合は0が返される</return>
        private (Vector3, Quaternion, Vector3) ReplasePropSetter(GameObject targetObject)
        {
            Vector3 pos = new Vector3(0, 0, 0), scale = new Vector3(0, 0, 0);
            Quaternion rot = Quaternion.identity;

            if (replacePosition)
            {
                //置換対象のオブジェクト位置に置換するオブジェクトを配置
                pos = targetObject.transform.position;
            }
            if (replaceRotation)
            {
                //置換対象のオブジェクトの回転に置換するオブジェクトを配置
                rot = targetObject.transform.rotation;
            }
            if (replaceScale)
            {
                //置換対象のオブジェクトのスケールに置換するオブジェクトを配置
                scale = targetObject.transform.localScale;
            }

            return (pos, rot, scale);
        }


    }
}