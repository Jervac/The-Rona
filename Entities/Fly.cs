using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace the_rona.Entities {
    public class Fly : Enemy {
        Animation fly_right, fly_left;

        public Fly(string side) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -8, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -8, Globals.TILE_SIZE, Globals.TILE_SIZE, "left", "null", false);
            hp = 1;
            vel_x = .3f;
            vel_y = .3f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 6;
            accelleration_y = .2f;
            decceleration_y = .24f;
            setCustomCollisionSize(Globals.TILE_SIZE/1.5f, Globals.TILE_SIZE/1.1f);

            fly_right = new Animation();
			fly_right.AddFrame (new Rectangle (16*4, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_right.AddFrame (new Rectangle (16*5, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_right.AddFrame (new Rectangle (16*6, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_left = new Animation();
            fly_left.AddFrame (new Rectangle (16*4, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));
			fly_left.AddFrame (new Rectangle (16*5, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_left.AddFrame (new Rectangle (16*6, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = fly_right;
            else current_animation = fly_left;
        }

        public override void update(GameTime gt) {
            float dx = Globals.tree.getCenterPos().X - getCenterPos().X;
            float dy = Globals.tree.getCenterPos().Y - getCenterPos().Y;
            float len = (float)System.Math.Sqrt((dx*dx)+(dy*dy));
            if (len != 0) {
                dx /= len;
                dy /= len;
            }
            position.X += vel_x * dx;
            position.Y += vel_y * dy;
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}
