using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using the_rona.Utils;

namespace the_rona.Entities {
    public class Chunk : Entity {
        public string type = "null";
        public Chunk(int tilex, int tiley, string type) {
            this.type = type;
            init(Globals.CHUNK_WIDTH * tilex, Globals.CHUNK_HEIGHT * tiley, Globals.CHUNK_WIDTH, Globals.CHUNK_HEIGHT, "right", "white", false);

            if (type == "spawn") {
                Globals.entities.Add(new Player());
                Globals.player = (Player)Globals.entities[Globals.entities.Count-1];
            }
            else if (type == "hostile") {
                for (int i = 0; i < Globals.CHUNK_WIDTH/Globals.TILE_SIZE; i++) {
                    bool do_spawn = Utils.Utils.GetRandomNumber(1, 1) == 1;
                    if (do_spawn) {
                        int enemy = Utils.Utils.GetRandomNumber(1, 10);
                      //  if (enemy <= 5) Globals.entities.Add(new Walker(position.X + (Globals.TILE_SIZE * i), Globals.CHUNK_HEIGHT + position.Y - Globals.TILE_SIZE));
                      //  else if (enemy >= 6 && enemy <= 9) Globals.entities.Add(new Foot(position.X + (Globals.TILE_SIZE * i), Globals.CHUNK_HEIGHT + position.Y - Globals.TILE_SIZE));
                     //   else if (Globals.DIFFICULTY > 4) Globals.entities.Add(new Shroom(position.X + (Globals.TILE_SIZE * i), Globals.CHUNK_HEIGHT + position.Y - Globals.TILE_SIZE));
                     //   else Globals.entities.Add(new Walker(position.X + (Globals.TILE_SIZE * i), Globals.CHUNK_HEIGHT + position.Y - Globals.TILE_SIZE));
                    }
                }
            }
            else if (type == "plant") {
                //Globals.entities.Add(new Tree((int)getTilePosCenter().X, (int)getTilePosCenter().Y));
            }
        }

        public override void update(GameTime gt) {
            
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            for (int i = 0; i < Globals.CHUNK_WIDTH/Globals.TILE_SIZE; i++) {
                for (int j = 0; j < Globals.CHUNK_HEIGHT/Globals.TILE_SIZE; j++) {
                    //Utils.Utils.DrawBorder(sb, texture, new Rectangle((int)position.X + (Globals.TILE_SIZE * i), (int)position.Y + (Globals.TILE_SIZE * j), Globals.TILE_SIZE, Globals.TILE_SIZE), 1, Color.DarkGray);
                }
            }
            Utils.Utils.DrawBorder(sb, texture, dummyRectangle, 1, new Color(40, 40, 40));
            
            Globals.spriteBatch.Draw(texture, new Rectangle((int)position.X, Globals.CHUNK_HEIGHT, (int)size.X, 1), Color.DarkSlateGray);
        }
    }
}