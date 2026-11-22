/*
Author : schwesterium
Date   : 2026/11/19
*/

using UnityEngine;

namespace SchwesteriumLibrary.Editor
{
    public static class GUISize
    {
        public static class Width { public const float S = 60, M = 100, L = 160, Label = 120, Icon = 24; }
        public static class Height { public const float S = 18, M = 24, L = 32, XL = 48; }
        public static class Space { public const float XS = 2, S = 4, M = 8, L = 16, XL = 24; }

        public static class  Layout
        {
            public static GUILayoutOption WidthS => GUILayout.Width(Width.S);
            public static GUILayoutOption WidthM => GUILayout.Width(Width.M);
            public static GUILayoutOption WidthL => GUILayout.Width(Width.L);
            public static GUILayoutOption HeightS => GUILayout.Height(Height.S);
            public static GUILayoutOption HeightM => GUILayout.Height(Height.M);
            public static GUILayoutOption HeightL => GUILayout.Height(Height.L);
            public static GUILayoutOption HeightXL => GUILayout.Height(Height.XL);
        }
    }
}