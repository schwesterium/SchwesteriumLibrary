/*
Author : schwesterium
Date   : 2026/08/01
*/

using UnityEngine;

namespace SchwesteriumLibrary.Utility
{
    public static class PlayerPrefsBool 
    {
        /// <summary>
        /// PlayerPrefsからbool値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="defaultValue">キーが存在しない場合に返す値</param>
        public static bool GetBool(string key, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) != 0;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// PlayerPrefsからbool値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        public static bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) != 0;
        }

        /// <summary>
        /// PlayerPrefsにbool値を保存する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">保存する値</param>
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }


        /// <summary>
        /// Keyが存在する場合のみPlayerPrefsからbool値を取得する。存在しない場合はfalseを返す。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolHasKey(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return GetBool(key);
            }
            else
            {
                Debug.LogWarning($"key : {key} は存在していません！");
                return false;
            }
        }

        /// <summary>
        /// Keyが存在する場合のみPlayerPrefsからint値を取得する。存在しない場合は0を返す。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetIntHasKey(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                Debug.LogWarning($"key : {key} は存在していません！");

                return 0;
            }
        }
    }
}