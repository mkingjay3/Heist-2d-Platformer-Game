using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace MGFGame
{
    public class Leaderboard
    {
        private Font font;
        private Texture backgroundImage;
        private List<(string, int)> scores;

        private Bounds2 returnButton = new Bounds2(1107, 631, 190, 85);

       // private Bounds2 scoreboardArea = new Bounds2(305, 180, 765, 430);

        public Leaderboard()
        {
            LoadScores();
        }

        public void LoadContent(Font regularFont, Font titleFont)
        {
            font = regularFont;
            backgroundImage = Engine.LoadTexture("UI/scoreboardscreen.png");
        }

        public void LoadScores()
        {
            scores = ScoreManager.LoadLeaderboard();
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

            return GameState.Leaderboard;
        }

        public void Draw()
        {
            Engine.DrawTexture(backgroundImage, Vector2.Zero);

            if (scores.Count == 0)
            {
                Engine.DrawString("No scores yet!", new Vector2(640, 380), Color.Black, font, TextAlignment.Center);
            }
            else
            {
                int yPosition = 250; 
                int displayCount = Math.Min(8, scores.Count); // only top 8
                int lineHeight = 40; 

                for (int i = 0; i < displayCount; i++)
                {
                    var entry = scores[i];
                    Color rankColor = Color.Black;

                    if (i == 0) rankColor = new Color(218, 165, 32, 255); // Gold
                    else if (i == 1) rankColor = new Color(128, 128, 128, 255); // Silver
                    else if (i == 2) rankColor = new Color(139, 90, 43, 255); // Bronze

                    // Draw rank, player, and score with proper spacing
                    Engine.DrawString($"{i + 1}", new Vector2(365, yPosition), rankColor, font);
                    Engine.DrawString(entry.Item1, new Vector2(648, yPosition), rankColor, font, TextAlignment.Center);
                    Engine.DrawString(entry.Item2.ToString(), new Vector2(910, yPosition), rankColor, font);

                    yPosition += lineHeight;
                }
            }
        }
    }
}