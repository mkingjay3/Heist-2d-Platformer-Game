using System;
using System.Collections.Generic;

namespace MGFGame
{
    /// <summary>
    /// The main start screen with menu options using custom image
    /// </summary>
    public class StartScreen
    {
        private Texture backgroundImage;

        // Define clickable regions for each button (x, y, width, height)
        private Bounds2 startButton = new Bounds2(374, 324, 280, 140);      // START button
        private Bounds2 rulesButton = new Bounds2(705, 324, 280, 140);      // RULES button
        private Bounds2 scoreboardButton = new Bounds2(374, 478, 280, 140); // SCORE BOARD button
        private Bounds2 creditsButton = new Bounds2(705, 478, 280, 140);    // CREDITS button

        public void LoadContent()
        {
            backgroundImage = Engine.LoadTexture("UI/startscreen.png");
        }

        public GameState Update()
        {
            Vector2 mousePos = Engine.MousePosition;

            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                if (startButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.Playing;
                }
                else if (rulesButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.Rules;
                }
                else if (scoreboardButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.Leaderboard;
                }
                else if (creditsButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.Credits;
                }
            }

            return GameState.StartScreen;
        }

        public void Draw()
        {
            // Draw the background image
            Engine.DrawTexture(backgroundImage, Vector2.Zero);
        }
    }
}