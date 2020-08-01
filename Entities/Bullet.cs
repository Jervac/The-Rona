using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace the_rona.Entities {
    public class Bullet : Entity {
        public Bullet(string texture_name, Vector2 startpos, string direction_facing) {
            this.texture_name = texture_name;
            init(startpos.X, startpos.Y, Globals.TILE_SIZE/2, Globals.TILE_SIZE/2, direction_facing, "null", false);
            layer = 1;
            vel_x = 2.4f;
            vel_y = 2.4f;
            if (direction_facing == "right") direction.X = 1;
            else if (direction_facing == "left") direction.X = -1;
            else if (direction_facing == "down") {
                direction.Y = 1;
                vel_y = .6f;
            }
            else if (direction_facing == "up") direction.Y = -1;

            if (texture_name == "egg") loadTexture("entities/egg");
        }

        public override void update(GameTime gt) {
            if (direction.X != 0) position.X += direction.X * vel_x;
            else if (direction.Y != 0) position.Y += direction.Y * vel_y;

            if (position.X + size.X >= 145) alive = false;
            else if (position.X <= -145) alive = false;
            for (int i = 0; i <= Globals.entities.Count-1; i++) {
                if (texture_name == "egg") {
                    if (Globals.tree.getBounds().collides(this.getBounds())) {
                        Globals.tree.hurt(1);
                        this.alive = false;
                    }
                    return;
                }
                if (Globals.entities[i].alive && Globals.entities[i] is Enemy && Globals.entities[i].getBounds().collides(this.getBounds())) {
                    this.alive = false;
                    Globals.entities[i].hurt(2);
                }
            }
            updateDummyRect();
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            sb.Draw(texture, dummyRectangle, Color.Red);
        }
    }
}