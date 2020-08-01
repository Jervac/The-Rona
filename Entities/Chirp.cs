using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace the_rona.Entities {
    public class Chirp : Enemy {
        Animation walk_right, walk_left;
        public Chirp(string side) {
            if (side == "left") init(-Globals.CHUNK_WIDTH/2, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            else init(Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "left", "null", false);
            facePlayer();
            hp = 1;
            vel_x = .4f;
            vel_y = 8f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 6;
            accelleration_y = .2f;
            decceleration_y = .24f;
            setCustomCollisionSize(Globals.TILE_SIZE/2.2f, Globals.TILE_SIZE/3);

            walk_right = new Animation();
			walk_right.AddFrame (new Rectangle (16*4, 16*6, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*5, 16*6, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*6, 16*6, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*7, 16*6, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_right.AddFrame (new Rectangle (16*8, 16*6, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left = new Animation();
			walk_left.AddFrame (new Rectangle (16*4, 16*7, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left.AddFrame (new Rectangle (16*5, 16*7, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left.AddFrame (new Rectangle (16*6, 16*7, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left.AddFrame (new Rectangle (16*7, 16*7, 16, 16), TimeSpan.FromSeconds (anim_speed));
            walk_left.AddFrame (new Rectangle (16*8, 16*7, 16, 16), TimeSpan.FromSeconds (anim_speed));

            if (direction.X == 1) current_animation = walk_right;
            else current_animation = walk_left;
        }

        public override void update(GameTime gt) {
            setCustomCollisionPosition(getCenterPos().X - custom_collision_width/2, position.Y + size.Y - custom_collision_height);
            applyGravity();
            walkTowardsPlayer();
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            //sb.Draw(texture, new Vector2(getBounds().X, getBounds().Y), new Rectangle((int)getBounds().X, (int)getBounds().Y, (int)getBounds().Width, (int)getBounds().Height), Color.Fuchsia);
            sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, Color.White);
        }
    }
}
