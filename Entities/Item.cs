using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Item : Entity {
        Timer death_timer;
        float death_delay = 3;
        public Item(Entity source, string texture_name) {
            float isize = Globals.TILE_SIZE/2;
            init(source.getCenterPos().X , source.getCenterPos().Y, isize, isize, "right", "entities/"+texture_name, false);
            this.texture_name = texture_name;
            layer = 2;
            vel_x = .4f;
        }

        public override void update(GameTime gt) {
            applyGravity();

            if (distanceFromPlayer() <= Globals.TILE_SIZE * 4) {
                if (Globals.player.getCenterPos().X < this.getCenterPos().X) direction.X = -1;
                else direction.X = 1;
                position.X += vel_x * direction.X;
                if (getBounds().collides(Globals.player.getBounds())) {
                    alive = false;
                    //if (texture_name == "") Globals.player.weapon = texture_name;
                    //if (!weapon) 
                    if (texture_name == "soul") Globals.SOULS++;
                }
            }
            updateDummyRect();
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            sb.Draw(texture, dummyRectangle, Color.White);
        }
    }
}