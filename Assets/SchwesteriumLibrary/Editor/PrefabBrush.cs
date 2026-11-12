using UnityEditor;
using UnityEngine;

namespace SchwesteriumLibrary.Editor
{
    public class PrefabBrush : EditorWindow
    {
        GameObject prefab;
        Transform parent = null;
        float radius = 3f;
        float density = 0.5f; // 1平方メートルあたりの個数
        float maxScale = 1f;
        float minScale = 0.5f;
        bool active;

        Vector3 hitPoint;
        bool hasHit;
        Vector3 lastPaintPoint;
        bool painting;

        [MenuItem("Tools/Prefab Brush")]
        static void Open() => GetWindow<PrefabBrush>("Prefab Brush");

        void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
        void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

        void OnGUI()
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
            parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);
            radius = EditorGUILayout.Slider("Radius", radius, 0.1f, 100f);
            maxScale = EditorGUILayout.Slider("maxScale", maxScale, 1f, 50f);
            minScale = EditorGUILayout.Slider("mixScale", minScale, 0.01f, 50f);
            if (minScale > maxScale) { minScale = maxScale; }
            density = EditorGUILayout.Slider("Density (個/m²)", density, 0.001f, 5f);
            active = GUILayout.Toggle(active, "Brush ON", "Button", GUILayout.Height(28));
            SceneView.RepaintAll();
        }

        void OnSceneGUI(SceneView view)
        {
            if (!active || prefab == null) return;

            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            hasHit = Physics.Raycast(ray, out RaycastHit hit);
            if (!hasHit) return;
            hitPoint = hit.point;

            // ブラシ円の表示
            Handles.color = new Color(0.2f, 1f, 0.4f, 1f);
            Handles.DrawWireDisc(hit.point, hit.normal, radius);

            if (e.alt) return; // カメラ操作を優先

            if (e.type == EventType.Layout)
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (e.button != 0) return;

            if (e.type == EventType.MouseDown)
            {
                painting = true;
                Undo.IncrementCurrentGroup();
                Paint();
                lastPaintPoint = hitPoint;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && painting)
            {
                if (Vector3.Distance(hitPoint, lastPaintPoint) > radius * 0.5f)
                {
                    Paint();
                    lastPaintPoint = hitPoint;
                }
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                painting = false;
            }

            view.Repaint();
        }

        void Paint()
        {
            int count = Mathf.Max(1, Mathf.RoundToInt(density * Mathf.PI * radius * radius));

            Vector2 up = Vector2.up;
            Vector3 scale = Vector3.one;
            for (int i = 0; i < count; i++)
            {
                Vector2 r = Random.insideUnitCircle * radius;
                Vector3 origin = hitPoint + new Vector3(r.x, radius + 50f, r.y);
                RoundVector3(ref origin, 2);

                // 真上から落として地面に合わせる
                if (!Physics.Raycast(origin, Vector3.down, out RaycastHit h, 200f)) continue;

                var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                var pos = new Vector3(h.point.x, System.MathF.Round(h.point.y, 2), h.point.z);

                var s = System.MathF.Round(Random.Range(minScale, maxScale), 1);
                scale.x = s;
                scale.y = s;
                scale.z = s;

                go.transform.position = pos;
                go.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 180f), up);
                go.transform.localScale = scale;
                Undo.RegisterCreatedObjectUndo(go, "Prefab Brush");
            }
        }

        private void RoundVector3(ref Vector3 vector, int digit)
        {
            vector.x = System.MathF.Round(vector.x, 2);
            vector.y = System.MathF.Round(vector.y, 2);
            vector.z = System.MathF.Round(vector.z, 2);
        }
    }

}
