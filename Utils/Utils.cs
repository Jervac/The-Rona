using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace the_rona.Utils {
    public class Utils {

        // https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object(); 

        public static double GetRandomNumber(double minimum, double maximum) {
            lock(syncLock) { // synchronize
                return random.NextDouble() * (maximum - minimum) + minimum;
            }
        }

        public static int GetRandomNumber(int minimum, int maximum) {
            lock(syncLock) { // synchronize
                return random.Next(minimum, maximum+1);
            }
        }

        public static void DrawBorder(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor) {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
    }
}