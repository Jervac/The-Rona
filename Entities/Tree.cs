using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_rona.Entities {
    public class Tree : Entity {
        public SoundEffect landing_sfx;
        public Tree(/*int tilex, int tiley*/) {
            init(-Globals.TILE_SIZE/2, Globals.TILE_SIZE * -6, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "entities/tree", false);
            //init(Globals.TILE_SIZE * tilex, Globals.TILE_SIZE * tiley, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "entities/tree", false);
            hp = 3;
            layer = 0;
            landing_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/landing");
            jumping = true; // So it registers a land sfx
        }

        public override void update(GameTime gt) {
            applyGravity();
            foreach (var e in Globals.entities) {
                if (e is Enemy && getBounds().collides(e.getBounds())) {
                    e.die();
                    this.hurt(1);
                    if (!alive) Globals.GAME_OVER = true;
                    return;
                }
            }
        }

        public override void render(GraphicsDevice g, SpriteBatch b, GameTime gt) {
            b.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
        }
    }
}