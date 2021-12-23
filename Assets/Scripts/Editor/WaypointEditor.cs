using Traffic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Waypoint)), CanEditMultipleObjects]
    public class WaypointEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
        }
        protected virtual void OnSceneGUI()
        {
            var t = target as Waypoint;
            EditorGUI.BeginChangeCheck();
            var pos = Handles.DoPositionHandle(t.TangentOffset, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                t.TangentOffset = pos - t.transform.position;

            }
            
        }
    }
}