using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using the_rona.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace the_rona.Utils {
    public class ChatBox : Entity {
        public SpriteFont font;
        public List<String> words = new List<string>();
        public Texture2D dummyTexture, dummyTexture2;
        public Rectangle dummyRectangle, dummyRectangle2;
        int spacing = Globals.TILE_SIZE / 4;
        float profile_size = 0;
        SoundEffect open_sound, next_sound;

        List<SoundEffect> sfxs = new List<SoundEffect>();

        public SoundEffect win, collected, dad_talk, fisherman_talk, woman_talk, cat_meow, gordo_talk, fatass_snooze, door_toggle, beheading, cracked_tile_break_sound, bark, bark2, bark3;
        public Song opening_theme, overworld_theme, eg_theme, jester_theme, sadsailor_theme, pincher_theme;
        public Texture2D profile, profile_null, profile_eg, profile_fatass, profile_fatass2;

        public bool is_open = false;
        public bool paused = false; // For when animations, movements, or delays are happening
        public string profile_name = "";
        public string text_to_render = "";

        Timer blu_company_timer = new Timer();
        float blu_company_delay = 8;

        public bool bottom = true;

        public Vector2 initial_position_top = new Vector2();

        public ChatBox() {
            font = Globals.game.Content.Load<SpriteFont>("Font");
            
            size.X = (Globals.WINDOW_WIDTH/1.5f) - spacing;
            size.Y = Globals.WINDOW_HEIGHT/6;
            position.X = Globals.WINDOW_WIDTH/2 - size.X/2 + spacing;
            position.Y = Globals.WINDOW_HEIGHT - (size.Y*1.5f) - spacing;
            initial_position = position;

            initial_position_top.X = position.X;
            initial_position_top.Y = ((size.Y*1.5f) + spacing);

            profile_size = size.Y - (spacing*2);

            dummyTexture = new Texture2D(Globals.g, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            dummyRectangle = new Rectangle((int)position.X - spacing, (int)position.Y - spacing, (int)size.X + spacing, (int)size.Y + spacing*2);

            dummyTexture2 = new Texture2D(Globals.g, 1, 1);
            dummyTexture2.SetData(new Color[] { Color.Black });
            dummyRectangle2 = new Rectangle((int)position.X, (int)position.Y, (int)size.X - spacing, (int)size.Y);

            profile = loadTexture("profiles/null");
            profile_null = loadTexture("profiles/null");
            profile_fatass = loadTexture("profiles/fatass");
            profile_fatass2 = loadTexture("profiles/fatass2");
            profile_eg = loadTexture("profiles/eg");

            win = Globals.game.Content.Load<SoundEffect>("audio/level_complete");
            open_sound = Globals.game.Content.Load<SoundEffect>("audio/chatbox_open");
            next_sound = Globals.game.Content.Load<SoundEffect>("audio/chatbox_next");

            dad_talk = Globals.game.Content.Load<SoundEffect>("audio/dad_talk");
            sfxs.Add(dad_talk);
            fisherman_talk = Globals.game.Content.Load<SoundEffect>("audio/fisherman_ho");
            sfxs.Add(fisherman_talk);
            woman_talk = Globals.game.Content.Load<SoundEffect>("audio/woman_hi");
            sfxs.Add(woman_talk);

            cat_meow = Globals.game.Content.Load<SoundEffect>("audio/meow");
            gordo_talk = Globals.game.Content.Load<SoundEffect>("audio/gordo_talk");
            collected = Globals.game.Content.Load<SoundEffect>("audio/item_collected");
            fatass_snooze = Globals.game.Content.Load<SoundEffect>("audio/sleeping");
            door_toggle = Globals.game.Content.Load<SoundEffect>("audio/sfx/door_toggle");
            beheading = Globals.game.Content.Load<SoundEffect>("audio/sfx/beheading");
            cracked_tile_break_sound = Globals.game.Content.Load<SoundEffect>("audio/cracked_tile_break");
            bark = Globals.game.Content.Load<SoundEffect>("audio/sfx/bark");
            bark2 = Globals.game.Content.Load<SoundEffect>("audio/sfx/bark2");
            bark3 = Globals.game.Content.Load<SoundEffect>("audio/sfx/bark3");

            opening_theme = Globals.game.Content.Load<Song>("audio/the_day_i_was_crowned_king");
            eg_theme = Globals.game.Content.Load<Song>("audio/polo_kanye");
            jester_theme = Globals.game.Content.Load<Song>("audio/team_plasma_music");
            sadsailor_theme = Globals.game.Content.Load<Song>("audio/botany_bay");
            pincher_theme = Globals.game.Content.Load<Song>("audio/runway_v2");
            overworld_theme = Globals.game.Content.Load<Song>("audio/themes/overworld");
        }

        public override void update(GameTime gt) {
            if (words.Count > 0 && !paused && !is_open) {
                open();
            }
            else if (words.Count == 0) is_open = false;

            if (paused) {
            }
        }

        public override void render(GraphicsDevice g, SpriteBatch b, GameTime gt) {
            if (words.Count > 0 && !paused) {
                
                if (bottom) position.Y += ((Globals.WINDOW_HEIGHT - (size.Y*1.5f) - spacing) - position.Y) * 0.2f;
                else position.Y += (((size.Y*1.5f) + spacing) - position.Y) * 0.2f;

                dummyRectangle.Y = (int)position.Y - spacing;
                dummyRectangle2.Y = (int)position.Y;

                b.Draw(dummyTexture, dummyRectangle, Color.White);
                b.Draw(dummyTexture2, dummyRectangle2, new Color(9, 9, 9));
                
                if (profile == profile_null) setProfile();

                // Render nameless entity
                if (!nameIsToBeRendered() && profile == profile_null) {
                    Globals.spriteBatch.DrawString(font, text_to_render, new Vector2(position.X + spacing, position.Y + spacing * 2), Color.White);
                } else { // Render name/pfp/text
                    if (nameIsToBeRendered()) Globals.spriteBatch.DrawString(font, profile_name, new Vector2(position.X + profile_size + spacing * 5, position.Y + spacing * 2), Color.White);
                    Globals.spriteBatch.DrawString(font, text_to_render, new Vector2(position.X + profile_size + (spacing * 5), position.Y + font.LineSpacing + spacing * 2), Color.White);
                }
                
                Globals.spriteBatch.Draw(profile, new Rectangle((int)position.X + spacing, (int)position.Y + spacing, (int)profile_size, (int)profile_size), Color.White);
            } else {
                if (bottom) setBottom();
                else setTop();
            }
        }

        public void setTop() {
            bottom = false;
            position.Y = -(size.Y - spacing);
        }

        public void setBottom() {
            bottom = true;
            position.Y = Globals.WINDOW_HEIGHT;
        }

        public void setProfile() {
            if (words.Count > 0) {
                if (!words[0].Contains(":")) {
                    profile = profile_null;
                    profile_name = "";
                    text_to_render = words[0];
                    return;
                }
                string[] data = words[0].Split(':');
                profile_name = data[0].ToLower().Replace(".", string.Empty);
                text_to_render = data[1];

                if (profile_name == "fatass") profile = profile_fatass;
                else if (profile_name == "fatass2") profile = profile_fatass2;
                else if (profile_name == "eg") profile = profile_eg;
                else profile = profile_null;
            }
        }

        public bool nameIsToBeRendered() {
            if (profile_name.StartsWith("fatass")) return false;
            return true;
        }

        public void open() {
            is_open = true;
            open_sound.Play();
        }

        public void close() {
            
        }

        public void open(int sound_effect) {
            open_sound.Play();
            sfxs[sound_effect].Play();
        }

        public void next() {
            if (words.Count > 0) {
                words.Remove(words[0]);
                next_sound.Play();
            }
        }

        // For specific sounds to play
        public void next(int sound_effect) {
            if (words.Count > 0) {
                words.Remove(words[0]);
                sfxs[sound_effect].Play();
            }
        }

        public bool containsWords() {
            return words.Count > 0;
        }

        public void playTheme(Song theme) {
            MediaPlayer.Volume = 1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(theme);
        }

        public void stopAllThemes() {
            MediaPlayer.Stop();
        }

        public void playSfx(SoundEffect sfx) {
            sfx.Play();
        }
    }
}