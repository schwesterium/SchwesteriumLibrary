/*
Author : schwesterium
Date   : 2026/08/01
*/


using System;
using System.Collections.Generic;
using UnityEngine;

//inspectorから_dictionaryの中身が見れるようになっているSerializableDictionaryクラス
//inspectorから編集すると壊れます

namespace SchwesteriumLibrary.Collections
{
    [Serializable]
    public class SerializableDictionary<Tkey, Tvalue>
    {
        [SerializeField]
        private List<Pair<Tkey, Tvalue>> _keyValueList = new();

        private Dictionary<Tkey, Tvalue> _dictionary = null;

        /// <summary>
        /// Dictionaryがnullでない場合はDictionaryを作成します
        /// </summary>
        /// <returns>Dictionaryが作成できたかどうか</returns>
        public bool TryCreateDictionary()
        {
            //Dictionaryが生成済みなら処理は行わない
            if (_dictionary != null) { return false; }

            _dictionary = new();

            //生成したDictionaryにkeyとvalueを追加する
            foreach (var pair in _keyValueList)
            {
                _dictionary.Add(pair.First, pair.Second);
            }

            return true;
        }

        /// <summary>
        /// 要素を追加する。#defineディレクティブ UNITY_EDITORが有効な場合はInspectorの更新も行う
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">要素</param>
        public void AddElement(Tkey key, Tvalue value)
        {
            if(_dictionary == null)
            {
                Debug.LogWarning("_dictionaryがnullです！");
                return;
            }

            _dictionary.Add(key, value);

#if UNITY_EDITOR
            //Inspector上の表示も更新する
            _keyValueList.Add(new Pair<Tkey, Tvalue>(key, value));
#endif
        }

        /// <summary>
        /// keyの要素をvalueで更新する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">要素</param>
        public void UpdateElementValue(Tkey key, Tvalue value)
        {
            _dictionary[key] = value;
        }

        /// <summary>
        /// keyの値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>keyの値 Tvalue</returns>
        public Tvalue GetElement(Tkey key)
        {
            return _dictionary[key];
        }

        /// <summary>
        /// Dictionaryに指定したキーが含まれているかどうか
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>指定したキーが含まれているかどうか</returns>
        public bool ContainsKey(Tkey key)
        {
            return _dictionary.ContainsKey(key);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor専用: Inspectorの更新を行う
        /// </summary>
        public void UpdateList()
        {
            //前提として、_dictionaryが最新の状態、_keyValueListは更新前の状態

            for (int i = 0; i < _keyValueList.Count; i++)
            {
                //keyの取り出し
                var key = _keyValueList[i].First;

                //keyが存在する場合は、Listの要素を更新する
                if (_dictionary.TryGetValue(key, out Tvalue value))
                {
                    _keyValueList[i] = new Pair<Tkey, Tvalue>(key, value);
                }
            }
        }
#endif
    }
}