using System.Collections.Generic;
using System.Linq;
using UnityEditor.Purchasing;
using UnityEngine;

namespace Algorithms
{
    public class GrahamScan
    {
        private Vector3[] _points;

        private int _bottomPointIndex;

        private Vector3 _origin;
        private Stack<int> _stack;
        public GrahamScan(Vector3[] points)
        {
            _points = points;
            _stack = new Stack<int>();
            Scan();
            PrintStack();
        }

        private void Scan()
        {
            SortPoints();
            _stack.Push(0);
            _stack.Push(1);
            // _stack.Push(2);
            for (int i = 2; i < _points.Length; i++)
            {
                var p = _points[i];
                var stackedIndex = _stack.Pop();
                var stackedPoint = _points[stackedIndex];
                while (_stack.Count > 0 && !IsClockWise(_points[_stack.Peek()], stackedPoint, p))
                {
                    stackedIndex = _stack.Pop();
                }

                _stack.Push(stackedIndex);
                _stack.Push(i);
            }

            var lastPointIndex = _stack.Pop();
            if (!IsClockWise(_points[_stack.Peek()], _points[lastPointIndex], _origin))
            {
                _stack.Push(lastPointIndex);
            }

            // for (int i = 2; i < _points.Length; i++)
            // {
            //     var p = _points[i];
            //     var stackedIndex = _stack.Pop();
            //     var stackedPoint = _points[stackedIndex];
            //     while (_stack.Count > 0 && !IsClockWise(_points[_stack.Peek()], stackedPoint, p))
            //     {
            //         stackedIndex = _stack.Pop();
            //     }
            //
            //     _stack.Push(stackedIndex);
            //     _stack.Push(i);
            //     
            //     for (int j = i - 1; j < _points.Length; j++)
            //     {
            //         var p2 = _points[j];
            //         
            //         if (IsClockWise(p, p2))
            //         {
            //             if(_stack.Count > 0)
            //                 _stack.Pop();
            //         }
            //     }
            //
            //     _stack.Push(i);
            // }
        }

        private void PrintStack()
        {
            foreach (var p in _stack)
            {
                Debug.Log(_points[p]);
            }
        }
        private void SortPoints()
        {
            MoveOriginToFirst();

            for (int i = 1; i < _points.Length; i++)
            {
                var p = _points[i];

                for (int j = 1; j < _points.Length; j++)
                {
                    var p2 = _points[j];

                    if (IsPointHasWiderAngle(p, p2))
                    {
                        Swap(i, j);
                    }

                }
            }
        }

        private bool IsPointHasWiderAngle(Vector3 p, Vector3 p2)
        {
            var ground = _origin + Vector3.right;
            return Vector3.Angle(ground, p) < Vector3.Angle(ground, p2);
        }


        private void MoveOriginToFirst() => Swap(0, _bottomPointIndex);
        private void Swap(int i, int j)
        {
            var toSwapWith = _points[i];
            _points[i] = _points[j];
            _points[j] = toSwapWith;
        }

        private void FindBottomIndex()
        {
            var vec = Vector3.zero;

            for (var index = 0; index < _points.Length; index++)
            {
                var point = _points[index];
                if (!(vec.z >= point.z)) continue;
                vec = point;
                _bottomPointIndex = index;
            }

            _origin = _points[_bottomPointIndex];
        }

        private Vector3 GetVectorFromOrigin(Vector3 point)
        {
            return point - _origin;
        }

        public bool IsClockWise(Vector3 point1, Vector3 point2)
        {
            var p0p1 = GetVectorFromOrigin(point1);
            var p0p2 = GetVectorFromOrigin(point2);

            var cross = CrossBetweenPoints(p0p1, p0p2);

            var clockWise = cross > 0;

            return clockWise;
        }
        public bool IsClockWise(Vector3 p1, Vector3 p2, Vector3 p3)
        {

            var cross = (p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x);

            var clockWise = cross > 0;

            return clockWise;
        }

        private float CrossBetweenPoints(Vector3 point1, Vector3 point2)
        {
            return (point2.x * point1.z) - (point1.x * point2.z);

        }
    }
}