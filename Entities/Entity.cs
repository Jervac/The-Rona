using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using the_rona.Math;
using the_rona.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace the_rona.Entities {
    public abstract class Entity {
        public bool is_boss = false;
        public bool moving = false;
        public bool can_move = true;
        public bool can_attack = true;
        public bool player_dependant_tick = true;
        public bool initial_player_dependant_tick = true;
        public bool null_texture = true;
        public string texture_name = "null";
        public string direction_facing = "left";
        public string initial_direction_facing = "left";
        public SoundEffect player_hurt_sound, enemy_hurt_sound;
        public Color color = Color.White;
        public SpriteEffects sprite_effect;
        public Animation current_animation;
        public Texture2D texture;
        public Texture2D shadow_texture = null;
        public Texture2D solid_texture;
        public Rectangle dummyRectangle;
        public Vector2 position = new Vector2();
        public Vector2 initial_position = new Vector2();
        public Vector2 direction = new Vector2(0, 0);
        public Vector2 initial_direction = new Vector2(0, 0);
        public Vector2 previous_tile_pos = new Vector2();
        public Vector2 size;
        public float anim_speed = .25f;
        public int hp = 1;
        public int initial_hp = 1;
        public float accelleration_y = .4f / 2;
        public float decceleration_y = .5f / 2;
        public float vel_y = 2.8f;
        public float vel_x = 8f / 2;
        public float max_vel = 2.6f / 2;
        public float min_vel = .8f / 2;
        public float jump_vel = 12 / 2;
        public float hurt_vel = 9 / 2;
        public float angle = 0;
        public float custom_collision_width = 0;
        public float custom_collision_height = 0;
        public float custom_collision_x = 0;
        public float custom_collision_y = 0;
        public bool custom_collision_size = false;
        public bool custom_collision_position = false;
        public bool attacking = false;
        public bool initial_solid = true;
        public bool solid = true;
        public bool walkable = true; // For cracked floor
        public bool alive = true;
        public int layer = 1;
        public bool do_save = true;
        public bool kill_on_touch = false; // Kill player on touch
        public bool is_ledge = false;
        public bool jumping = false;
        public bool on_ground = false;
        public bool update_outside_camera = false;
        public bool render_outside_camera = false;
        public bool visible = true;
        public bool initial_visible = true;
        public bool upside_down = false;
        public bool initial_upside_down = false;
        public bool initial_kill_on_touch = false;
        public bool clockwise = true;
        public bool hide_when_editor = false;
        public bool is_npc = false;

        // Animation
        public Texture2D texture2 = null, texture3 = null, texture4 = null;
        public bool active = false; // For feathertiles
        public int frame = 1, frame_count = 2; // For 2-frame animations
        public bool animated = false;

        // I divide by 2 here a lot because this is when i shrunk the tile size from 32 to 16
        //public float max_vel_x = 6;
        public float max_vel_x = 7 / 2;
        public float acceleration_x = .4f / 2;
        public float stomp_vel = 16 / 2;
        //public static float head_bounce_vel = 14 / 2;
        public static float head_bounce_vel = 8f / 2;
        //public float bouncer_vel = head_bounce_vel * 1.6f / 2;
        public float bouncer_vel = head_bounce_vel * 1.4f;
        public float booster_vel = head_bounce_vel * 1.1f;
        public float booster_bounds_size = Globals.TILE_SIZE * 4.6f / 2;

        public enum Power {
            NONE,
            JETPACK
        };

        public Power power = Power.NONE;

        public abstract void update(GameTime gt);
        public abstract void render(GraphicsDevice g, SpriteBatch sb, GameTime gt);

        public bool adjacentTo(Entity e) {
            // Left / Right
            if (e.getTilePosAbsolute().Y == this.getTilePosAbsolute().Y && (e.getTilePosAbsolute().X == this.getTilePosAbsolute().X + 1 || e.getTilePosAbsolute().X == this.getTilePosAbsolute().X - 1)) { 
                return true;
            }
            // Up / Down
            if (e.getTilePosAbsolute().X == this.getTilePosAbsolute().X && (e.getTilePosAbsolute().Y == this.getTilePosAbsolute().Y + 1 || e.getTilePosAbsolute().Y == this.getTilePosAbsolute().Y - 1)) { 
                return true;
            }
            return false;
        }

        

        public void init(float x, float y, float width, float height, string direction_facing, string texture_name, bool solid) {
            position.X = x;
            position.Y = y;
            this.direction_facing = direction_facing;
            this.texture_name = texture_name;
            size.X = width;
            size.Y = height;
            vel_y = 6;
            vel_x = 4;
            layer = Globals.layer;
            this.solid = solid;
            this.initial_solid = solid;
            this.initial_position = position;
            this.initial_direction_facing = direction_facing;
            this.initial_upside_down = upside_down;
            this.initial_kill_on_touch = this.kill_on_touch;
            this.do_save = true;

            if (direction_facing == "right") direction.X = 1;
            else if (direction_facing == "left") direction.X = -1;

            initTexture(texture_name);
            player_hurt_sound = Globals.game.Content.Load<SoundEffect>("audio/player_hurt");
            enemy_hurt_sound = Globals.game.Content.Load<SoundEffect>("audio/mike_hurt");
        }

        public void init(float x, float y, float width, float height, string direction_facing, string texture_name, bool solid, bool upside_down) {
            position.X = x;
            position.Y = y;
            this.direction_facing = direction_facing;
            this.texture_name = texture_name;
            size.X = width;
            size.Y = height;
            vel_y = 6;
            vel_x = 4;
            layer = Globals.layer;
            this.upside_down = upside_down;
            this.solid = solid;
            this.initial_solid = solid;
            this.initial_position = position;
            this.initial_direction_facing = direction_facing;
            this.initial_upside_down = upside_down;
            this.initial_kill_on_touch = this.kill_on_touch;
            this.do_save = true;
            do_save = true;

            if (direction_facing == "right") direction.X = 1;
            else if (direction_facing == "left") direction.X = -1;

            initTexture(texture_name);
            player_hurt_sound = Globals.game.Content.Load<SoundEffect>("audio/player_hurt");
            enemy_hurt_sound = Globals.game.Content.Load<SoundEffect>("audio/mike_hurt");
        }

        public void initTick(bool player_dependant_tick) {
            this.player_dependant_tick = player_dependant_tick;
            initial_player_dependant_tick = player_dependant_tick;
        }

        // Called in Game.cs on level load
        public void preInit() { 
            solid_texture = new Texture2D(Globals.g, 1, 1);
            solid_texture.SetData(new Color[] {Color.White});
        }

        public Texture2D initSolidTexture(Color color) { 
            Texture2D temp_text = new Texture2D(Globals.g, 1, 1);
            temp_text.SetData(new Color[] { color });
            return temp_text;
            
        }

        // NOTE: This is better used at the beginning of an update() function to prevent the visible delay of shadow positioning if used in render()
        public void updateDummyRect() {
            dummyRectangle.X = (int)position.X;
            dummyRectangle.Y = (int)position.Y;
            dummyRectangle.Width = (int)size.X;
            dummyRectangle.Height = (int)size.Y;
        }

        public void initTexture() {
            texture = new Texture2D(Globals.g, 1, 1);
            texture.SetData(new Color[] { color });
            dummyRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public void initTexture(Texture2D _tex) {
            _tex = new Texture2D(Globals.g, 1, 1);
            _tex.SetData(new Color[] { color });
        }

        public void initTexture(string target_texture_name) {
            this.texture_name = target_texture_name;
            null_texture = (target_texture_name == "null" || target_texture_name == "white");
            dummyRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            if (null_texture) {
                if (target_texture_name == "null") {
                    texture = new Texture2D(Globals.game.GraphicsDevice, 1, 1);
                    texture.SetData(new Color[] { Color.Fuchsia });
                } else if (target_texture_name == "white") {
                    texture = new Texture2D(Globals.game.GraphicsDevice, 1, 1);
                    texture.SetData(new Color[] { Color.White });
                }
                return;
            }
            try {
                texture = Globals.game.Content.Load<Texture2D>(target_texture_name);
            } catch (Exception e) {
                null_texture = true;
                Console.WriteLine("Failed to load ["+target_texture_name +"]");
            }
        }

        public Texture2D loadTexture(string target_texture_name) {
            try {
                return Globals.game.Content.Load<Texture2D>(target_texture_name);
            } catch (Exception e) {
                Console.WriteLine("Failed to load ["+target_texture_name +"]");
            }
            return null;
        }

        public void setTextureColor(Color _color) {
            texture.SetData(new Color[] { _color });
        }

        /* Doesn't Work
        public void initTexture(string target_texture_name, Texture2D target_texture) {
            if (target_texture == texture) null_texture = false;
            target_texture = new Texture2D(Globals.game.GraphicsDevice, 1, 1);
            target_texture.SetData(new Color[] { Color.White });
            try {
                target_texture = Globals.game.Content.Load<Texture2D>(target_texture_name);
            } catch (Exception e) {
                if (target_texture == texture) null_texture = true;
                Console.WriteLine("Failed to load ["+target_texture_name +"]");
            }
        }
        */

        public void drawShadow() {
            Globals.spriteBatch.Draw(texture, new Rectangle(dummyRectangle.X - Globals.shadow_offset_x, dummyRectangle.Y + Globals.shadow_offset_y, dummyRectangle.Width, dummyRectangle.Height), null, new Color(0, 0, 0, .5f), 0, new Vector2(0, 0), sprite_effect, 0f);
        }

        public void initShadow() {
            shadow_texture = new Texture2D(Globals.game.GraphicsDevice, 1, 1);
            shadow_texture.SetData(new Color[] { new Color(0, 0, 0, .5f) });
            //shadow_texture = Globals.game.Content.Load<Texture2D>(texture_name);
        }

        public void updateTextureDirection() {
            if (direction_facing == "right") sprite_effect = SpriteEffects.None;
            if (direction_facing == "left") sprite_effect = SpriteEffects.FlipHorizontally;
        }

        public void setCustomCollisionSize(float width, float height) {
            custom_collision_size = true;
            custom_collision_width = width;
            custom_collision_height = height;
        }

        public void setCustomCollisionPosition(float x, float y) {
            custom_collision_position = true;
            custom_collision_x = x;
            custom_collision_y = y;
        }

        public Rect getBounds() {
            if (this is Player) {
                if (((Player)(this)).squating) custom_collision_height = Globals.TILE_SIZE/2;
                else setCustomCollisionSize(Globals.TILE_SIZE/1.5f, Globals.TILE_SIZE/1.1f);
            }
            if (custom_collision_size && custom_collision_position) return new Rect(custom_collision_x, custom_collision_y, custom_collision_width, custom_collision_height);
            else if (custom_collision_size) return new Rect(getCenterPos().X - custom_collision_width/2, getCenterPos().Y - custom_collision_height/2, custom_collision_width, custom_collision_height);
            else if (custom_collision_position) return new Rect(custom_collision_x, custom_collision_y, size.X, size.Y);
            return new Rect(position.X, position.Y, size.X, size.Y);
        }

        public Vector2 getCenterPos() {
            return new Vector2(position.X + (size.X / 2), position.Y + (size.Y / 2));
        }

        public Vector2 getInitialCenterPos() {
            return new Vector2(initial_position.X + (size.X / 2), initial_position.Y + (size.Y / 2));
        }

        public Vector2 getTilePosCenter() {
            return new Vector2((float)System.Math.Floor(getCenterPos().X / Globals.TILE_SIZE), (float)System.Math.Floor(getCenterPos().Y / Globals.TILE_SIZE));
        }

        public Vector2 getTilePosAbsolute() {
            return new Vector2((float)System.Math.Floor(position.X / Globals.TILE_SIZE), (float)System.Math.Floor(position.Y / Globals.TILE_SIZE));
        }

        public Vector2 getTilePosAbsolute_CEILING() {
            return new Vector2((float)System.Math.Ceiling(position.X / Globals.TILE_SIZE), (float)System.Math.Ceiling(position.Y / Globals.TILE_SIZE));
        }

        public Vector2 getInitialTilePosCenter() {
            return new Vector2((float)System.Math.Floor(getInitialCenterPos().X / Globals.TILE_SIZE), (float)System.Math.Floor(getInitialCenterPos().Y / Globals.TILE_SIZE));
        }

        public Vector2 getInitialTilePosAbsolute() {
            return new Vector2((float)System.Math.Floor(position.X / Globals.TILE_SIZE), (float)System.Math.Floor(position.Y / Globals.TILE_SIZE));
        }

        public Vector2 getTileSize() {
            return new Vector2((float)System.Math.Floor(size.X / Globals.TILE_SIZE), (float)System.Math.Floor(size.Y / Globals.TILE_SIZE));
        }

        public bool touchingPlayer() {
            return Globals.player.alive && Globals.player.getBounds().collides(this.getBounds());
        }

        // Spikes are considered solid to non-player entities
        // Water treated as solid
        public bool touchingSolid() {
            /*
            foreach (var e in Globals.current_player.getRoom().entities) {
                if (!(this is Player) && e.touchingNoninitialRoom()) this.alive = false;
                if (e != this && e.alive && e.getBounds().collides(getBounds()) && (e.solid || (e.texture_name.StartsWith("spike") && !(this is Player)) && (e.layer == this.layer || e.texture_name.StartsWith("water")))) {
                    // Pushblock goes in water or slides over other pusblocks in water
                    if (this is PushBlock && e.texture_name.StartsWith("water")) {
                        if (!hasNonsolidPushblock(e)) {
                            this.solid = false;
                            this.position = e.position;
                            e.solid = false;
                            if (!((PushBlock)this).in_water) {
                                ((PushBlock)this).sink_sound.Play();
                                ((PushBlock)this).in_water = true;
                            }
                        }
                        return false;
                    }
                    Console.WriteLine("touched byu: " + e.texture_name);
                    return true;
                }
            }
            */
            return false;
        }

        public bool touchingEnemy() {
            return false;
        }

        public bool touchingHostileEntity() {
            return false;
        }

        public bool touchingWall() {
            return false;
        }

        public Rect ledgeBounds() {
            return new Rect(position.X, position.Y, Globals.TILE_SIZE, Globals.TILE_SIZE);
        }

        // @side: 0 = left, 1 = right
        // @UPSIDEDOWN
        public bool touchingLedge(int side) {
            return false;
        }

        public bool touchingFloor() {
            if (position.Y + size.Y >= 0/*Globals.CHUNK_HEIGHT*/) return true;
            return false;
        }

        // Returns the floor being touched
        public Entity floorTouching() {
            return null;
        }

        //@UPSIDEDOWN
        public bool touchingHead(Entity e) {
            if (e != this && e.alive && e.layer == this.layer && e.getBounds().collides(getBounds())) {
                if ((position.Y + size.Y >= e.position.Y && position.Y < e.position.Y) || (e.getTilePosAbsolute().X == this.getTilePosAbsolute().X && e.getTilePosAbsolute().Y == this.getTilePosAbsolute().Y + 1)) {
                    return true;
                }
            }
            return false;
        }

        // Modifies direction if about to bounce into an object next frame
        //@UPSIDEDOWN
        public void handleBounce() {
            /*
            position.X += direction.X * vel_y;
            if (touchingSolid()) position.X -= direction.X * vel_y;
            foreach (var e in Globals.entities) {
                if (e != this && e.alive && e.layer == this.layer && e.solid) {
                    if (!this.getBounds().collides(e.getBounds())) { // if not X collision
                        Rect temp = getBounds();
                        temp.X += direction.X * vel_y;
                        if (temp.collides(e.getBounds())) { // if it will make X collision
                            direction.X *= -1;
                            break;
                        }
                    }
                }
            }
            position.Y += direction.Y * vel_y;
            if (touchingSolid()) position.Y -= direction.Y * vel_y;
            foreach (var e in Globals.entities) {
                if (e != this && e.alive && e.layer == this.layer && e.solid) {
                    if (!this.getBounds().collides(e.getBounds())) { // if not Y collision
                        Rect temp = getBounds();
                        temp.Y += direction.Y * vel_y;
                        if (temp.collides(e.getBounds())) { // if it will make Y collision
                            direction.Y *= -1;
                            break;
                        }
                    }
                }
            }
            */
        }

        public void hurt(int damage) {
            hp -= damage;
            if (hp <= 0) die();
            else if (this is Enemy) { // Enemy knockback
                position.X += vel_x * (Globals.TILE_SIZE/4) * -direction.X;
            }
            //if (!alive && this is Player) Globals.game.Respawn(false);
            
        }

        public void hurt(int damage, Vector2 hurt_direction) {
            hp -= damage;
            if (hp <= 0) alive = false;
            //if (!alive && this is Player) Globals.game.generateRooms();
        }

        public void die() {
            /*
            if (this is Jester) {
                Globals.beat_jester = true;
                Globals.game.Respawn(Globals.current_player.getRoom(), false, false);
                Globals.current_player.returnToOverworld(false, false, 62, 73);
                Globals.chat_box.win.Play();
            }
            else if (this is Pincher) {
                this.alive = false;
                bool dont_kill = false;
                foreach (var e in Globals.current_player.getRoom().entities) {
                    if (e is Pincher && e.alive) dont_kill = true;
                }
                if (!dont_kill) {
                    Globals.beat_pincher = true;
                    MediaPlayer.Stop();
                    Globals.chat_box.playTheme(Globals.chat_box.overworld_theme);
                    Globals.game.emptyVillage();
                }
                Globals.chat_box.win.Play();
            }
            else if (this is Player && !(this is ShadowPlayer)) {
                this.alive = false;
                Globals.current_player.death_sound.Play();
                //Globals.chat_box.stopAllThemes();
                Globals.game.Respawn(Globals.current_player.getRoom(), false, false);
                //((Player)(this)).sfx = Utils.Utils.GetRandomNumber(1, 3);
                //if (((Player)(this)).sfx == 1) ((Player)(this)).death_sound.Play();
                //else if (((Player)(this)).sfx == 2) ((Player)(this)).death_sound2.Play();
                //else ((Player)(this)).death_sound3.Play();
                Globals.game.vibrateController(1.0f, 1.0f, 1.0f);
                moving = false;
                if (Globals.fishing) {
                    ((Player)(this)).returnToOverworld(false, true, 31, 113);
                    ((Player)(this)).on_stopper = true;
                }
                else if (Globals.level == "boss") {
                    ((Player)(this)).returnToOverworld(false, false, 62, 73);
                }
                else {
                    
                }
                return; 
            }
            else if (this is Enemy) {
                //((Enemy)(this)).death_sound.Play();
            }
            else {
                this.alive = false;
            }
            */
            alive = false;

            //if (Utils.Utils.GetRandomNumber(1, 4) == 1) Globals.entities.Add(new Item(this, "soul"));
            Globals.game.cam.screenShake(.25f, 20);
            Globals.KILLS++;
        }

        public void applyGravity() {
            on_ground = touchingFloor();
            
            if (!on_ground) {
                vel_y += decceleration_y;
                if (vel_y > max_vel) vel_y = max_vel;
                position.Y += vel_y;

                /*
                foreach(Entity e in Globals.entities) {
                    // Spike
                    if (e is Tile && e.texture_name == "spike" && e.getBounds().collides(this.getBounds())) {
                        alive = false;
                        return;
                    }
                    // Bounce off bouncers
                    else if (e is Tile && e.texture_name == "bouncer" && e.getBounds().collides(this.getBounds())) {
                        if (touchingHead(e)) {
                            jumping = true;
                            if (upside_down) { 
                                position.Y += vel_y;
                                vel_y = -bouncer_vel;
                            } else { 
                                position.Y -= vel_y;
                                vel_y = -bouncer_vel;
                            }
                            return; // Skip touchingFloor() check
                        }
                    }
                }
                */
            }

            if (touchingFloor()) {
                position.Y -= vel_y;
                vel_y = 0;
                if (jumping) {
                    if (this is Player) ((Player)(this)).landing_sfx.Play();
                }
                if (jumping && this is Tree) ((Tree)(this)).landing_sfx.Play();
                jumping = false;
                on_ground = true;
                if (this is Player) ((Player)(this)).doublejumped = false;
                if (this is Player && ((Player)(this)).prejumped) {
                    ((Player)(this)).prejumped = false;
                    ((Player)(this)).jump();
                }
            }
        }

        //@UPSIDEDOWN
        public void walk() {
            if (direction_facing == "right") {
                direction.X = 1;
                position.X += vel_x;
                //if (touchingPlayer() && kill_on_touch) Globals.current_player.die();
                //if (touchingSolid()) {
                    //direction_facing = "left";
                    //position.X -= vel_x;
                //}
            } else if (direction_facing == "left") {
                direction.X = -1;
                position.X -= vel_x;
                //if (touchingPlayer() && kill_on_touch) Globals.current_player.die();
                //if (touchingSolid()) {
                  //  direction_facing = "right";
                    //position.X += vel_x;
                //}
            }
        }

        public void facePlayer() {
          //  if (distanceFromPlayer() <= Globals.UPDATE_DISTANCE) {
            //    if (Globals.current_player.getCenterPos().X < getCenterPos().X) direction.X = -1;
            //    else direction.X = 1;
          //  }
            if (getCenterPos().X > 0) direction.X = -1;
            else direction.X = 1;
            if (direction.X == 1) direction_facing = "right";
            else direction_facing = "left";
        }
        public void walkTowardsPlayer() {
            if (direction_facing == "right") {
                direction.X = 1;
                position.X += vel_x;
            } else if (direction_facing == "left") {
                direction.X = -1;
                position.X -= vel_x;
            }
        }

        public float distanceFromPlayer() {
            float dx = Globals.player.getCenterPos().X - getCenterPos().X;
            float dy = Globals.player.getCenterPos().Y - getCenterPos().Y;
            return (float)System.Math.Sqrt((dx*dx)+(dy*dy));
        }

        public float distanceFromInitialPosition() { 
            float dx = this.initial_position.X - position.X;
            float dy = this.initial_position.Y - position.Y;
            return (float)System.Math.Sqrt((dx*dx)+(dy*dy));
        }

        public int distanceFromInitialTile() { 
            float dx = this.initial_position.X - position.X;
            float dy = this.initial_position.Y - position.Y;
            return (int)System.Math.Sqrt((dx*dx)+(dy*dy)) / Globals.TILE_SIZE;
        }

        public void flip() { 
            if (upside_down) { 
              position.Y -= (Globals.TILE_SIZE + size.Y);
            } else { 
                position.Y += (Globals.TILE_SIZE + size.Y);
            }
            upside_down = !upside_down;
            if (this is Player) {
                //((Player)(this)).stomping = false;
                //((Player)(this)).clearStates();
            }
        }

        public void tickFrame() {
            if (!animated) return;
            frame++;
            if (frame > frame_count) frame = 1;
        }

        public void jump() {
            on_ground = false;
            jumping = true;
            //can_jump = false;
            vel_y = -jump_vel;
            if (this is Player) ((Player)(this)).jump_sfx.Play();
        }
    }
}