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
            var pos = Handles.DoPositionHandle(t.UpperTangentOffset, Quaternion.identity);
            var pos2 = Handles.DoPositionHandle(t.LowerTangentOffset, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                t.UpperTangentOffset = pos - t.transform.position;
                t.LowerTangentOffset = pos2 - t.transform.position;

            }
            
        }
    }
}