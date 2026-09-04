using System;

namespace MGFGame
{
    /// <summary>
    /// Screen displaying game rules using custom image
    /// </summary>
    public class RulesScreen
    {
        private Texture backgroundImage;

        // Return button region (bottom right)
        private Bounds2 returnButton = new Bounds2(1000, 600, 280, 120);

        public void LoadContent()
        {
            backgroundImage = Engine.LoadTexture("UI/rulesscreen.png");
        }

        public GameState Update()
        {
            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                Vector2 mousePos = Engine.MousePosition;
                if (returnButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.StartScreen;
                }
            }

            return GameState.Rules;
        }

        public void Draw()
        {
            // Draw the background image
            Engine.DrawTexture(backgroundImage, Vector2.Zero);
        }
    }
}