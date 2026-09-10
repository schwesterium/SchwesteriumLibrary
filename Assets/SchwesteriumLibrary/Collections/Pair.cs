/*
Author : schwesterium
Date   : 2026/09/10
*/

using System;

namespace SchwesteriumLibrary.Collections
{
    //keyとvalueを保存しておくクラス
    [Serializable]
    public class Pair<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}