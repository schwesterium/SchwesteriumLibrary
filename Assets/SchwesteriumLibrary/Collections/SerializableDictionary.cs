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
                _dictionary.Add(pair.Key, pair.Value);
            }

            return true;
        }

        public void AddElement(Tkey key, Tvalue value)
        {
            if(_dictionary == null)
            {
                Debug.LogWarning("_dictionaryがnullです！");
                return;
            }

            _dictionary.Add(key, value);
            //Inspector上の表示も更新する
            _keyValueList.Add(new Pair<Tkey, Tvalue>(key, value));
        }

        public void UpdateElementValue(Tkey key, Tvalue value)
        {
            _dictionary[key] = value;
        }

        public Tvalue GetElement(Tkey key)
        {
            return _dictionary[key];
        }

        public bool ContainsKey(Tkey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void UpdateList()
        {
            //前提として、_dictionaryが最新の状態、_keyValueListは更新前の状態

            for (int i = 0; i < _keyValueList.Count; i++)
            {
                //keyの取り出し
                var key = _keyValueList[i].Key;

                //keyが存在する場合は、Listの要素を更新する
                if (_dictionary.TryGetValue(key, out Tvalue value))
                {
                    _keyValueList[i] = new Pair<Tkey, Tvalue>(key, value);
                }
            }
        }
    }

    //keyとvalueを保存しておくクラス
    [Serializable]
    public class Pair<Tkey, Tvalue>
    {
        [SerializeField]
        private Tkey _key;
        [SerializeField]
        private Tvalue _value;

        public Pair(Tkey key, Tvalue value)
        {
            _key = key;
            _value = value;
        }

        public Tkey Key { get => _key; }
        public Tvalue Value { get => _value; }
    }
}