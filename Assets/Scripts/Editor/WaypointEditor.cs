using Navigation;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Waypoint)), CanEditMultipleObjects]
    public class WaypointEditor : UnityEditor.Editor
    {
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