using System;

namespace MGFGame
{
    /// <summary>
    /// End screen that shows when player wins or loses
    /// </summary>
    public class EndScreen
    {
        private Texture winImage;
        private Texture loseImage;
        private Font font;
        private bool isWin;
        private int finalScore;

        // Popup dimensions and position
        private const int POPUP_WIDTH = 570;
        private const int POPUP_HEIGHT = 360;
        private Vector2 popupPosition;

        // Button regions RELATIVE to popup position (not absolute screen coords)
        private Vector2 viewScoreboardButtonOffset = new Vector2(118, 263);
        private Vector2 playAgainButtonOffset = new Vector2(268, 263);
        private Vector2 exitGameButtonOffset = new Vector2(418, 263);
        private Vector2 buttonSize = new Vector2(130, 55);

        // Score display position RELATIVE to popup (after "SCORE:")
        private Vector2 scorePositionOffset = new Vector2(418, 195);

        public EndScreen()
        {
            // Calculate centered popup position
            popupPosition = new Vector2((1280 - POPUP_WIDTH) / 2, (720 - POPUP_HEIGHT) / 2);
        }

        public void LoadContent(Font scoreFont)
        {
            font = scoreFont;
            winImage = Engine.LoadTexture("UI/endscreenpopup_winning-removebg-preview.png");
            loseImage = Engine.LoadTexture("UI/endscreenpopup_losing-removebg-preview.png");
        }

        public void Show(bool playerWon, int score)
        {
            isWin = playerWon;
            finalScore = score;

            // Auto-save the score when the end screen is shown
            ScoreManager.SaveScore(finalScore);

            // Play death sound if player lost
            if (!playerWon)
            {
                SoundManager.PlayDeath();
            }
        }

        public GameState Update()
        {
            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                Vector2 mousePos = Engine.MousePosition;

                // Create absolute button bounds based on popup position
                Bounds2 viewScoreboardButton = new Bounds2(
                    popupPosition.X + viewScoreboardButtonOffset.X,
                    popupPosition.Y + viewScoreboardButtonOffset.Y,
                    buttonSize.X,
                    buttonSize.Y
                );

                Bounds2 playAgainButton = new Bounds2(
                    popupPosition.X + playAgainButtonOffset.X,
                    popupPosition.Y + playAgainButtonOffset.Y,
                    buttonSize.X,
                    buttonSize.Y
                );

                Bounds2 exitGameButton = new Bounds2(
                    popupPosition.X + exitGameButtonOffset.X,
                    popupPosition.Y + exitGameButtonOffset.Y,
                    buttonSize.X,
                    buttonSize.Y
                );

                if (viewScoreboardButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.LeaderboardFromEnd;
                }

                if (playAgainButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    return GameState.StartScreen;
                }

                if (exitGameButton.Contains(mousePos))
                {
                    SoundManager.PlayClick();
                    Environment.Exit(0);
                }
            }

            return GameState.EndScreen;
        }

        public void Draw()
        {
            // Draw semi-transparent dark overlay to make popup stand out more
            Engine.DrawRectSolid(new Bounds2(0, 0, 1280, 720), new Color(0, 0, 0, 180));

            // Draw the appropriate popup image
            Texture backgroundImage = isWin ? winImage : loseImage;
            Engine.DrawTexture(backgroundImage, popupPosition);

            // Draw the score with better visibility
            Vector2 scorePos = popupPosition + scorePositionOffset;

            // Draw shadow/outline for score text for better visibility
            Engine.DrawString(finalScore.ToString(), scorePos + new Vector2(2, 2), Color.Black, font);
            Engine.DrawString(finalScore.ToString(), scorePos, new Color(139, 69, 19, 255), font);

            
        }
    }
}