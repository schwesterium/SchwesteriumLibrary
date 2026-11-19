using UnityEditor;
using UnityEngine;
using SchwesteriumLibrary.Utility;

namespace SchwesteriumLibrary.Editor.Tools
{
    public class PrefabBrush : EditorWindow
    {
        private GameObject _prefab = null;
        private Transform _parent = null;
        private float _radius = 3f;
        private float _density = 0.5f; // 1平方メートルあたりの個数
        private float _maxScale = 1f;
        private float _minScale = 0.5f;
        private bool _isActive = false;

        private Vector3 _hitPoint = Vector3.zero;
        private bool _hasHit = false;
        private Vector3 _lastPaintPoint = Vector3.zero;
        private bool _painting = false;

        [MenuItem("SchwesteriumLibrary/Editor/PrefabBrush")]
        static void Open() => GetWindow<PrefabBrush>("Prefab Brush");

        void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
        void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

        void OnGUI()
        {
            _prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false);
            _parent = (Transform)EditorGUILayout.ObjectField("Parent", _parent, typeof(Transform), true);
            _radius = EditorGUILayout.Slider("Radius", _radius, 0.01f, 100f);
            _maxScale = EditorGUILayout.Slider("maxScale", _maxScale, 1f, 50f);
            _minScale = EditorGUILayout.Slider("mixScale", _minScale, 0.01f, 50f);
            if (_minScale > _maxScale) { _minScale = _maxScale; }
            _density = EditorGUILayout.Slider("Density (個/m²)", _density, 0.001f, 5f);
            _isActive = GUILayout.Toggle(_isActive, "Brush ON", "Button", GUILayout.Height(28));
            SceneView.RepaintAll();
        }

        void OnSceneGUI(SceneView view)
        {
            if (!_isActive || _prefab == null) { return; }

            Event e = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            _hasHit = Physics.Raycast(ray, out RaycastHit hit);
            if (!_hasHit) { return; }

            _hitPoint = hit.point;

            //ブラシ円の表示
            Handles.color = new Color(0.2f, 1f, 0.4f, 1f);
            Handles.DrawWireDisc(hit.point, hit.normal, _radius);

            if (e.alt) { return; }

            if (e.type == EventType.Layout) { HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); }

            if (e.button != 0) { return; }

            if (e.type == EventType.MouseDown)
            {
                _painting = true;
                Undo.IncrementCurrentGroup();
                Paint();
                _lastPaintPoint = _hitPoint;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && _painting)
            {
                if (Vector3.Distance(_hitPoint, _lastPaintPoint) > _radius * 0.5f)
                {
                    Paint();
                    _lastPaintPoint = _hitPoint;
                }
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                _painting = false;
            }

            view.Repaint();
        }

        void Paint()
        {
            int count = Mathf.Max(1, Mathf.RoundToInt(_density * Mathf.PI * _radius * _radius));

            Vector2 up = Vector2.up;
            Vector3 scale = Vector3.one;
            for (int i = 0; i < count; i++)
            {
                Vector2 r = Random.insideUnitCircle * _radius;
                Vector3 origin = _hitPoint + new Vector3(r.x, _radius + 50f, r.y);
                MathUtility.RoundVector3(ref origin, 2);

                //真上から落として地面に合わせる
                if (!Physics.Raycast(origin, Vector3.down, out RaycastHit h, 200f)) { continue; }

                var obj = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, _parent);
                var pos = new Vector3(h.point.x, System.MathF.Round(h.point.y, 2), h.point.z);

                var s = System.MathF.Round(Random.Range(_minScale, _maxScale), 1);
                scale.x = s;
                scale.y = s;
                scale.z = s;

                obj.transform.position = pos;
                obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 180f), up);
                obj.transform.localScale = scale;
                Undo.RegisterCreatedObjectUndo(obj, "Prefab Brush");
            }
        }
    }

}
