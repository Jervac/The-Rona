using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using the_rona.Utils;

namespace the_rona.Utils {
    public class Camera {

        private bool shaking = false;
        private Timer shake_timer = new Timer();
        private float shake_delay = 1f;
        private float shake_intensity = 20;

        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix() {
            var scaleX = (float)Globals.WINDOW_WIDTH / Globals.VIRTUAL_WIDTH;
            var scaleY = (float)Globals.WINDOW_HEIGHT / Globals.VIRTUAL_HEIGHT;
            return
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(scaleX, scaleY, 1) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            
            
        }

        public Vector2 mouseToWorld(Point mouse_pos)
        {
            return Vector2.Transform(new Vector2(mouse_pos.X, mouse_pos.Y), Matrix.Invert(TransformMatrix()));
        }

        public Camera(int x, int y, int width, int height) {
            Position = new Vector2(x, y);
            Bounds = new Rectangle(x, y, width, height);
            Zoom = 1;
        }

        public void updateScreenShake() {
            if (shaking) {
                Position = new Vector2(Position.X + ((float)System.Math.Sin((double)(Utils.GetRandomNumber(0, 60) * shake_intensity))), Position.Y + ((float)System.Math.Cos((double)(Utils.GetRandomNumber(0, 60) * shake_intensity))));
                shake_intensity -= 0.25f;
                if (shake_timer.getTimeElapsed() >= shake_delay || shake_intensity <= 0) {
                    shake_timer.pause();
                    shaking = false;
                }
            }
        }

        public void screenShake(float duration, float intensity) {
            shaking = true;
            shake_delay = duration;
            shake_intensity = intensity;
            shake_timer.pause();
            shake_timer.start();
        }

        /*
        public Rectangle VisibleArea {
            get {
                var inverseViewMatrix = Matrix.Invert(View);
                var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
                var tr = Vector2.Transform(new Vector2(_screenSize.X, 0), inverseViewMatrix);
                var bl = Vector2.Transform(new Vector2(0, _screenSize.Y), inverseViewMatrix);
                var br = Vector2.Transform(_screenSize, inverseViewMatrix);
                var min = new Vector2(
                    MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                    MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
                var max = new Vector2(
                    MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                    MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
                return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }
        */

        
    }
}

/*
    To go from screen to world space simply. This is commonly used to get the location of the mouse in the world for object picking.
    
    Vector2.Transform(mouseLocation, Matrix.Invert(Camera.TransformMatrix));



    To go from world to screen space simply do the opposite.

    Vector2.Transform(mouseLocation, Camera.TransformMatrix);
 */
