using CarAI;
using Traffic;
using UnityEngine;
using UnityEditor;

namespace CarEditor
{
    [CustomEditor(typeof(CarAI.Path))]
    public class CarEngineEditor : Editor
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
            if (t.UseWaypoint)
            {
                for (int i = t.nodes.Count - 1; i >= 0; i--)
                {
                    t.nodes[i].GetComponent<Waypoint>().RotatePolarity();
                }
            }

            
        }


        public void ReverseNodes()
        {
            var t = target as Path;
            var index = 0;
            for (int i = t.nodes.Count - 1; i >= 0; i--)
            {
                t.nodes[i].transform.SetSiblingIndex(index++);
            }
        }
    }


}
  