/*
Author : schwesterium
Date   : 2026/11/19
*/

using UnityEditor;
using UnityEngine;

namespace SchwesteriumLibrary.Editor
{
    public static class DropField
    {
        //
        //  概要
        //      ドロップできるフィールドを描画する
        //  引数
        //      rect: 描画位置とサイズ, label: ラベル, (out) paths: ドラッグされているファイルパス, (out) objects: ドラッグされているobjectsの参照
        //  戻り値
        //      ドロップできたか
        //
        public static bool Draw(Rect rect, string label, out string[] paths, out UnityEngine.Object[] objects)
        {
            paths = null; 
            objects = null;
            GUI.Box(rect, label, EditorStyles.helpBox);

            var e = Event.current;
            if (!rect.Contains(e.mousePosition)) { return false; }

            switch (e.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy; //カーソルをコピーのアイコンにする
                    e.Use();
                    break;

                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    paths = DragAndDrop.paths; //絶対パス
                    objects = DragAndDrop.objectReferences;
                    e.Use();
                    return true;
            }

            return false;
        }

        //
        //  概要
        //      ドロップできるフィールドを描画する
        //  引数
        //      label: ラベル, (out) paths: ドラッグされているファイルパス, (out) objects: ドラッグされているobjectsの参照
        //  戻り値
        //      ドロップできたか
        //
        public static bool Draw(string label, out string[] paths, out UnityEngine.Object[] objects)
        {
            paths = null;
            objects = null;

            var rect = GUILayoutUtility.GetRect(0, GUISize.Height.XL * 2, GUILayout.ExpandWidth(true));
            GUI.Box(rect, label, EditorStyles.helpBox);

            var e = Event.current;
            if (!rect.Contains(e.mousePosition)) { return false; }

            switch (e.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy; //カーソルをコピーのアイコンにする
                    e.Use();
                    break;

                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    paths = DragAndDrop.paths; //絶対パス
                    objects = DragAndDrop.objectReferences;
                    e.Use();
                    return true;
            }

            return false;
        }
    }
}