using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Shroom : Enemy {
        Animation walk_left, walk_right;
        public Shroom(string side/*float x, float y*/) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -4, Globals.TILE_SIZE * 2, Globals.TILE_SIZE * 2, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -4, Globals.TILE_SIZE * 2, Globals.TILE_SIZE *2, "left", "null", false);
            facePlayer();
            //init(x, y, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            hp = 5;
            vel_x = .18f;
            vel_y = 8f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 6;
            accelleration_y = .2f;
            decceleration_y = .24f;

            walk_right = new Animation();
			walk_right.AddFrame (new Rectangle (16*0, 16*4, 32, 32), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*2, 16*4, 32, 32), TimeSpan.FromSeconds (anim_speed));
            walk_left = new Animation();
			walk_left.AddFrame (new Rectangle (16*0, 16*6, 32, 32), TimeSpan.FromSeconds (anim_speed));
            walk_left.AddFrame (new Rectangle (16*2, 16*6, 32, 32), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = walk_right;
            else current_animation = walk_left;
        }

        public override void update(GameTime gt) {
            applyGravity();
            walkTowardsPlayer();
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}
