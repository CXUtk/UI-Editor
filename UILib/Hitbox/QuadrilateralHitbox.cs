﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UIEditor.UILib.Hitbox {
    public class QuadrilateralHitbox : IHitBox {
        private Vector2[] _points;

        /// <summary>
        /// 逆时针多边形顶点序列
        /// </summary>
        /// <param name="points"></param>
        public QuadrilateralHitbox() {
            _points = new Vector2[4];
        }

        public void Reset(int w, int h) {
            _points[0] = Vector2.Zero;
            _points[1] = new Vector2(0, h);
            _points[2] = new Vector2(w, h);
            _points[3] = new Vector2(w, 0);
        }

        public void Transform(Matrix matrix) {
            for (int i = 0; i < _points.Length; i++) {
                _points[i] = Vector2.Transform(_points[i], matrix);
            }
        }

        public static bool ToLeft(Vector2 p1, Vector2 p2, Vector2 point) {
            Vector2 v1 = p2 - p1, v2 = point - p1;
            double cross = v1.X * v2.Y - v1.Y * v2.X;
            return cross < 0;
        }

        public bool Contains(Vector2 point) {
            // Main.NewText(ToLeft(new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1)));
            for (int i = 0; i < _points.Length; i++) {
                if (!ToLeft(_points[i], _points[(i + 1) % _points.Length], point)) return false;
            }
            return true;
        }

        public void Draw(SpriteBatch sb) {
            Drawing.StrokePolygon(sb, _points.ToList(), 1, Color.Lime);
        }


        private Vector2 getProjections(Vector2 projVec) {
            float maxx = float.NegativeInfinity;
            float minn = float.PositiveInfinity;
            foreach (var point in _points) {
                float a = Vector2.Dot(point, projVec);
                maxx = Math.Max(maxx, a);
                minn = Math.Min(minn, a);
            }
            return new Vector2(minn, maxx);
        }
        public bool Intersects(IHitBox hitBox) {
            if (hitBox is QuadrilateralHitbox) {
                var quad = (QuadrilateralHitbox)hitBox;
                List<Vector2> projVec = new List<Vector2>();
                int n = _points.Length;
                for (int i = 0; i < n; i++) {
                    var vec = _points[(i + 1) % n] - _points[i];
                    vec.Normalize();
                    projVec.Add(new Vector2(-vec.Y, vec.X));
                }
                for (int i = 0; i < n; i++) {
                    var vec = quad._points[(i + 1) % n] - quad._points[i];
                    vec.Normalize();
                    projVec.Add(new Vector2(-vec.Y, vec.X));
                }
                foreach (var proj in projVec) {
                    var A = getProjections(proj);
                    var B = quad.getProjections(proj);
                    if (Math.Max(A.X, B.X) > Math.Min(A.Y, B.Y)) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public Rectangle GetOuterRectangle() {
            float minX = int.MaxValue, maxX = 0;
            float minY = int.MaxValue, maxY = 0;
            foreach (var pt in _points) {
                minX = Math.Min(minX, pt.X);
                minY = Math.Min(minY, pt.Y);
                maxX = Math.Max(maxX, pt.X);
                maxY = Math.Max(maxY, pt.Y);
            }
            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX + 1), (int)(maxY - minY + 1));
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var p in _points) {
                sb.Append(p.ToString());
                sb.Append(",");
            }
            return sb.ToString();
        }
    }
}
