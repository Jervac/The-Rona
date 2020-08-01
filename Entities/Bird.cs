using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Bird : Enemy {
        Animation fly_right, fly_left;
        bool dropped = false;
        public Bird(string side) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -7, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -7, Globals.TILE_SIZE, Globals.TILE_SIZE, "left", "null", false);
            hp = 1;
            vel_x = 2f;
            vel_y = .3f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 6;
            accelleration_y = .2f;
            decceleration_y = .24f;
            setCustomCollisionSize(Globals.TILE_SIZE/1.5f, Globals.TILE_SIZE/1.1f);

            fly_right = new Animation();
			fly_right.AddFrame (new Rectangle (16*4, 16*4, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_right.AddFrame (new Rectangle (16*5, 16*4, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_right.AddFrame (new Rectangle (16*6, 16*4, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_right.AddFrame (new Rectangle (16*7, 16*4, 16, 16), TimeSpan.FromSeconds (anim_speed));

            fly_left = new Animation();
			fly_left.AddFrame (new Rectangle (16*4, 16*5, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_left.AddFrame (new Rectangle (16*5, 16*5, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_left.AddFrame (new Rectangle (16*6, 16*5, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fly_left.AddFrame (new Rectangle (16*7, 16*5, 16, 16), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = fly_right;
            else current_animation = fly_left;
        }

        public override void update(GameTime gt) {
            walk();
            if (dropped && (getCenterPos().X < -Globals.LEVEL_WIDTH || getCenterPos().X > Globals.LEVEL_WIDTH)) {
                alive = false;
                return;
            }
            if (!dropped && (direction.X == 1 && getCenterPos().X > Globals.tree.getCenterPos().X ||
                        direction.X == -1 && getCenterPos().X < Globals.tree.getCenterPos().X)) {
                dropped = true;
                //Globals.entities.Add(new Bullet("egg", new Vector2(getCenterPos().X, position.Y + (size.Y*1.5f)), "down"));
            }
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}