using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace the_rona.Math
{
    public class Rect
    {
        public float X, Y;
        public float Width, Height;

        public Rect(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool collides(Rect b)
        {
            Rect a = this;
            return (a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y + a.Height > b.Y && a.Y < b.Y + b.Height);
        }

        public bool contains(Vector2 point)
        {
            return (point.X >= this.X && point.X <= this.X + this.Width && point.Y >= this.Y && point.Y <= this.Y + this.Height);
        }

        public Vector2 getCenterPos() {
            return new Vector2(X + (Width/2), Y + (Height/2));
        }

        public Vector2 getTilePosCenter() {
            return new Vector2((float)System.Math.Floor(getCenterPos().X / Globals.TILE_SIZE), (float)System.Math.Floor(getCenterPos().Y / Globals.TILE_SIZE));
        }

        public Vector2 getTilePosAbsolute() {
            return new Vector2((float)System.Math.Floor(X / Globals.TILE_SIZE), (float)System.Math.Floor(Y / Globals.TILE_SIZE));
        }
    }
}