using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Foot : Enemy {
        Animation current_animation;
        Animation jump_left, jump_right, fall_left, fall_right;
        Timer jump_timer;
        float jump_delay = 3.6f;

        public Foot(string side /*float x, float y*/) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "left", "null", false);
            facePlayer();
            //init(x, y, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            vel_x = .9f;
            vel_y = 8f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 2;
            accelleration_y = .2f;
            decceleration_y = .04f;
            setCustomCollisionSize(Globals.TILE_SIZE/1.3f, Globals.TILE_SIZE/1.1f);

            jump_timer = new Timer();

            jump_right = new Animation();
			jump_right.AddFrame (new Rectangle (16*3, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            jump_left = new Animation();
			jump_left.AddFrame (new Rectangle (16*3, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));

            fall_right = new Animation();
			fall_right.AddFrame (new Rectangle (16*3, 16*2, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fall_left = new Animation();
			fall_left.AddFrame (new Rectangle (16*3, 16*3, 16, 16), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = fall_right;
            else current_animation = fall_left;
        }

        public override void update(GameTime gt) {
            applyGravity();
            if (on_ground) facePlayer();
            if (!on_ground) walkTowardsPlayer();

            if (!jump_timer.didStart()) jump_timer.start();
            if (on_ground && jump_timer.getTimeElapsed() >= jump_delay) {
                jump_timer.pause();
                jump();
            }

            if (direction.X == 1) {
                if (jumping) current_animation = jump_right;
                else current_animation = fall_right;
            } else {
                if (jumping) current_animation = jump_left;
                else current_animation = fall_left;
            }
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            //sb.Draw(texture, new Vector2(getBounds().X, getBounds().Y), new Rectangle((int)getBounds().X, (int)getBounds().Y, (int)getBounds().Width, (int)getBounds().Height), Color.Fuchsia);
            sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}