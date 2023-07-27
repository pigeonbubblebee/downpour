#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Downpour
{
    public struct GizmosDrawer
    {
        private readonly struct HandlesDrawingScope : IDisposable {
            private readonly Color _oldColor;
            private readonly bool _back;

             public HandlesDrawingScope(bool back) {
                _back = back;
                _oldColor = Handles.color;
                Handles.color = Gizmos.color;
            }

            public void Dispose() {
                if (_back)
                    Handles.color = _oldColor;
            }
        }

        public GizmosDrawer SetColor(Color color) {
            Gizmos.color = color;
            return this;
        }

        public GizmosDrawer DrawWireDisc(Vector2 center, float radius, float thickness = 0) {
            using (new HandlesDrawingScope(true))
                Handles.DrawWireDisc(center, Vector3.forward, radius, thickness);

            return this;
        }

        public GizmosDrawer DrawWireSquare(Vector2 center, Vector2 size) {
            Vector2 vector2 = size * 0.5f;

            Vector2 a = center + new Vector2(-vector2.x, -vector2.y);
            Vector2 b = center + new Vector2(vector2.x, -vector2.y);
            Vector2 c = center + new Vector2(vector2.x, vector2.y);
            Vector2 d = center + new Vector2(-vector2.x, vector2.y);

            Gizmos.DrawLine(d, c);
            Gizmos.DrawLine(c, b);
            Gizmos.DrawLine(b, a);
            Gizmos.DrawLine(d, a);

            return this;
        }

        public GizmosDrawer DrawWireSquare(Rect rect) {
            DrawWireSquare(rect.center, rect.size);
            return this;
        }

        public GizmosDrawer DrawRay(Vector2 from, Vector2 direction) {
            Gizmos.DrawRay(from, direction);
            return this;
        }

        public GizmosDrawer DrawLine(Vector2 from, Vector2 to) {
            Gizmos.DrawLine(from, to);
            return this;
        }

        // public GizmosDrawer DrawPath(Pathfinding.Path path) {
        //     SetColor(GizmosColor.instance.pathfinding.pathColor);
        //     for (int i = 0; i < path.vectorPath.Count - 1; i++)
        //         DrawLine(path.vectorPath[i], path.vectorPath[i + 1]);
        //     return this;
        // }
    }
}
#endif