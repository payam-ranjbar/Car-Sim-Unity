using UnityEngine;

namespace Traffic
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] [Range(0f, 2f)] private float rightOffset;
        [SerializeField] [Range(0f, 2f)] private float leftOffset;

        
        [SerializeField]  [HideInInspector]private bool xAxis = true;
        public float RightOffset => rightOffset;

        public float LeftOffset => leftOffset;

        private Vector3 recentPos;

        public bool XAxis => xAxis;

        public Vector3 Position => GetPosition();

        public void RotatePolarity()
        {
            xAxis = !xAxis;
        }

        private Vector3 GetPosition()
        {
            if (xAxis)
            {
                var x = transform.position.x + Random.Range(-leftOffset, rightOffset);
                recentPos = new Vector3(x, transform.localPosition.y, transform.position.z);
                return new Vector3(x, transform.localPosition.y, transform.position.z);
            }
            else
            {
                var z = transform.position.z + Random.Range(-leftOffset, rightOffset);
                recentPos =new Vector3(transform.position.x, transform.localPosition.y, z);

                return new Vector3(transform.position.x, transform.localPosition.y, z);
            }
        }
    }
}