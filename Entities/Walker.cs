using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Walker : Enemy {
        Animation walk_left, walk_right;
        bool darker = false;

        public Walker(string side /*float x, float y*/) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "left", "null", false);
            facePlayer();
            //init(x, y, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            hp = 2;
            if (Utils.Utils.GetRandomNumber(1, 4) == 1) darker = true; // Chance of being darker and a little faster if 2 hp
            vel_x = .3f;
            vel_y = 8f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 6;
            accelleration_y = .2f;
            decceleration_y = .24f;
            setCustomCollisionSize(Globals.TILE_SIZE/1.5f, Globals.TILE_SIZE/1.1f);

            if (darker) {
                hp = 3;
                vel_x *= 1.2f;
            }

            anim_speed*=3;
            walk_right = new Animation();
			walk_right.AddFrame (new Rectangle (16*0, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*1, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left = new Animation();
            walk_left.AddFrame (new Rectangle (16*0, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));
			walk_left.AddFrame (new Rectangle (16*1, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = walk_right;
            else current_animation = walk_left;
        }

        public override void update(GameTime gt) {
            applyGravity();
            walkTowardsPlayer();
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            //sb.Draw(texture, new Vector2(getBounds().X, getBounds().Y), new Rectangle((int)getBounds().X, (int)getBounds().Y, (int)getBounds().Width, (int)getBounds().Height), Color.Fuchsia);
            if (darker) sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.MediumPurple);
            else sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}