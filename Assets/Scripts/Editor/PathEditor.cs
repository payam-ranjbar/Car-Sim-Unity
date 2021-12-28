using System;
using CarAI;
using Navigation;
using Traffic;
using UnityEngine;
using UnityEditor;

namespace CarEditor
{
    [CustomEditor(typeof(Path))]
    public class CarEngineEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //            DrawDefaultInspector();

            if (GUILayout.Button("reverse"))
            {
                    
                ReverseNodes();
            }
                
            if (GUILayout.Button("Turn 90 deg"))
            {
                    
                RotateNodes();
            }
                
                
        }

        private void RotateNodes()
        {
            var t = target as Path;
            for (int i = t.NodeCount - 1; i >= 0; i--)
            {
                t.Waypoints[i].GetComponent<Waypoint>()?.RotatePolarity();
            }


        }


        public void ReverseNodes()
        {
            var t = target as Path;
            var index = 0;
            for (int i = t.NodeCount - 1; i >= 0; i--)
            {
                t.Waypoints[i].transform.SetSiblingIndex(index++);
            }
        }

       
    }


}
  