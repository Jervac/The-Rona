using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_rona.Entities;
using the_rona.Utils;
using the_rona.Math;

namespace the_rona.Entities {
    public class Player : Entity {

        Rectangle attack_bounds;

        Animation idle_left, idle_right;
        Animation run_left, run_right;
        Animation jump_left, jump_right;
        Animation fall_left, fall_right;
        Animation squat_left, squat_right;

        public bool doublejumped = false;
        public bool prejumped = false;
        public bool squating = false;

        Timer attack_timer;
        float attack_delay = .25f;
        float default_attack_delay = .25f;
        float deagle_attack_delay = 1f;

        public SoundEffect jump_sfx, landing_sfx, swordswipe_sfx, swordhit_sfx, deagle_sfx;
        public string weapon = "null";

        public Player(/*Chunk spawn_room*/) {
            init(Globals.TILE_SIZE/-2, Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            //init(spawn_room.position.X + (Globals.CHUNK_WIDTH/2 - Globals.TILE_SIZE/2), Globals.CHUNK_HEIGHT + Globals.TILE_SIZE * -2, Globals.TILE_SIZE, Globals.TILE_SIZE, "right", "null", false);
            layer = 1;
            vel_x = 1.6f;
            vel_y = 8f;
            min_vel = vel_y;
            max_vel = vel_y;
            jump_vel = 4;
            accelleration_y = .2f;
            decceleration_y = .26f;
            direction.X = 1;
            attack_bounds = new Rectangle(0, 0, (int)(Globals.TILE_SIZE * 1.8f), (int)(Globals.TILE_SIZE/2.5f));
            updateAttackBounds();
            setCustomCollisionSize(Globals.TILE_SIZE/1.5f, Globals.TILE_SIZE/1.1f);
            setCustomCollisionPosition(getCenterPos().X - custom_collision_width/2, position.Y + size.Y - custom_collision_height);

            attack_timer = new Timer();

            float anim_speed = .25f;
            float idle_speed = .75f;

            idle_right = new Animation();
			idle_right.AddFrame (new Rectangle (16*0, 0, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_right.AddFrame (new Rectangle (16, 0, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_right.AddFrame (new Rectangle (16*0, 0, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_right.AddFrame (new Rectangle (16, 0, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_right.AddFrame (new Rectangle (16*2, 0, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_left = new Animation();
			idle_left.AddFrame (new Rectangle (16*0, 16, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_left.AddFrame (new Rectangle (16, 16, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_left.AddFrame (new Rectangle (16*0, 16, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_left.AddFrame (new Rectangle (16, 16, 16, 16), TimeSpan.FromSeconds (idle_speed));
            idle_left.AddFrame (new Rectangle (16*2, 16, 16, 16), TimeSpan.FromSeconds (idle_speed));

            run_right = new Animation();
			run_right.AddFrame (new Rectangle (16*3, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
			run_right.AddFrame (new Rectangle (16*4, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
			run_right.AddFrame (new Rectangle (16*5, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
            run_right.AddFrame (new Rectangle (16*6, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
            run_left = new Animation();
			run_left.AddFrame (new Rectangle (16*3, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));
			run_left.AddFrame (new Rectangle (16*4, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));
			run_left.AddFrame (new Rectangle (16*5, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));
            run_left.AddFrame (new Rectangle (16*6, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));

            jump_right = new Animation();
            jump_right.AddFrame (new Rectangle (16*7, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
            jump_left = new Animation();
            jump_left.AddFrame (new Rectangle (16*7, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));

            fall_right = new Animation();
            fall_right.AddFrame (new Rectangle (16*8, 0, 16, 16), TimeSpan.FromSeconds (anim_speed));
            fall_left = new Animation();
            fall_left.AddFrame (new Rectangle (16*8, 16, 16, 16), TimeSpan.FromSeconds (anim_speed));

            squat_right = new Animation();
            squat_right.AddFrame (new Rectangle (16*9, 16*0, 16, 16), TimeSpan.FromSeconds (anim_speed));
            squat_left = new Animation();
            squat_left.AddFrame (new Rectangle (16*9, 16*1, 16, 16), TimeSpan.FromSeconds (anim_speed));


            current_animation = idle_right;

            jump_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/jump");
            landing_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/landing");
            swordswipe_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/sword_swipe");
            swordhit_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/sword_hit_flesh");
            deagle_sfx = Globals.game.Content.Load<SoundEffect>("audio/sfx/deagle_shot");
        }

        public override void update(GameTime gt) {
            foreach (var e in Globals.entities) {
                if (e is Enemy && e.getBounds().collides(this.getBounds())) {
                    Globals.GAME_OVER = true;
                    return;
                }
            }
            applyGravity();

            squating = Keyboard.GetState().IsKeyDown(Keys.Down);

			moving = false;
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                if (!squating) {
				    moving = true;
                    position.X -= vel_x;
                }
                direction.X = -1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                if (!squating) {
				    moving = true;
                    position.X += vel_x;
                }
                direction.X = 1;
            }
            if (position.X <= -145) position.X += vel_x;
            if (position.X + size.X >= 145) position.X -= vel_x;

            if (direction.X == 1) direction_facing = "right";
            else direction_facing = "left";
            if (!squating && Keyboard.GetState().IsKeyDown(Keys.Z) && Globals.last_keyboard_state.IsKeyUp(Keys.Z)) {
                if (!prejumped && on_ground) jump();
                else if (!prejumped && jumping && !doublejumped) {
                    jump();
                    doublejumped = true;
                } else if (!on_ground && preJumpBounds().Y + preJumpBounds().Height >= 0) {
                    prejumped = true;
                }
            }
            if (!attacking && can_attack && Keyboard.GetState().IsKeyDown(Keys.X) && Globals.last_keyboard_state.IsKeyUp(Keys.X)) {
                position.X += vel_x * 2 * -direction.X;
                attacking = true;
                can_attack = false;
                if (weapon == "null") { // Default Weapon
                    bool hit_something = false;
                    for (int i = 0; i <= Globals.entities.Count-1; i++) {
                        if (Globals.entities[i] is Enemy && Globals.entities[i].getBounds().collides(new Math.Rect(attack_bounds.X, attack_bounds.Y, attack_bounds.Width, attack_bounds.Height))) {
                            Globals.entities[i].hurt(1);
                            hit_something = true;
                            swordhit_sfx.Play();
                        }
                    }
                    if (!hit_something) swordswipe_sfx.Play();
                }
                else if (weapon == "deagle") {
                    deagle_sfx.Play();
                    Globals.game.cam.screenShake(.25f, 40);
                    Globals.entities.Add(new Bullet("egg", new Vector2(getCenterPos().X + (direction.X * size.X), position.Y), direction_facing));
                }
            }
            if (!can_attack) {
                if (!attack_timer.didStart()) attack_timer.start();
                if (weapon == "null") attack_delay = default_attack_delay;
                else if (weapon == "deagle") attack_delay = deagle_attack_delay;
                if (attack_timer.getTimeElapsed() >= attack_delay) {
                    attack_timer.pause();
                    can_attack = true;
                }
            }
            
            setCustomCollisionPosition(getCenterPos().X - custom_collision_width/2, position.Y + size.Y - custom_collision_height);
            updateAttackBounds();
            
            if (moving && !jumping) {
                if (direction.X == 1) current_animation = run_right;
                else if (direction.X == -1) current_animation = run_left;
            }
            else if (!moving && !jumping) {
                if (direction.X == 1) current_animation = idle_right;
                else if (direction.X == -1) current_animation = idle_left;
            }

            if (jumping && vel_y <= 0) { // Jump
                if (direction.X == 1) current_animation = jump_right;
                else if (direction.X == -1) current_animation = jump_left;
            }
            else if (jumping && vel_y >= 0) {
                if (direction.X == 1) current_animation = fall_right;
                else if (direction.X == -1) current_animation = fall_left;
            }

            if (squating) {
                if (direction.X == 1) current_animation = squat_right;
                else if (direction.X == -1) current_animation = squat_left;
            }
            current_animation.Update(gt);
        }

        public override void render(GraphicsDevice g, SpriteBatch sb, GameTime gt) {
            //sb.Draw(texture, new Vector2(preJumpBounds().X, preJumpBounds().Y), new Rectangle((int)preJumpBounds().X, (int)preJumpBounds().Y, (int)preJumpBounds().Width, (int)preJumpBounds().Height), Color.Fuchsia);
            //sb.Draw(texture, new Vector2(getBounds().X, getBounds().Y), new Rectangle((int)getBounds().X, (int)getBounds().Y, (int)getBounds().Width, (int)getBounds().Height), Color.Fuchsia);
            if (attacking) {
                if (weapon == "null") sb.Draw(texture, attack_bounds, Color.DarkRed);
                attacking = false;
            }
            if (can_attack) color = Color.White;
            else color = new Color(110, 110, 110);
			sb.Draw(Globals.spriteSheet, position, current_animation.CurrentRectangle, color);
        }

        void updateAttackBounds() {
            attack_bounds.X = (int)(getCenterPos().X - (attack_bounds.Width/2) + (direction.X * attack_bounds.Width/4) + (direction.X * size.X/2));
            if (squating) attack_bounds.Y = (int)(position.Y + size.Y - attack_bounds.Height);
            else attack_bounds.Y = (int)position.Y;
        }

        public Rect preJumpBounds() {
            return new Rect((int)position.X, (int)position.Y + (int)size.Y/2, (int)Globals.TILE_SIZE, (int)Globals.TILE_SIZE*1.5f);
        }
    }
}