using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MGFGame
{
    public class ScoreManager
    {
        private const string LEADERBOARD_FILE = "leaderboard.txt";
        private static int currentPlayerNumber = 0;

        public static void SaveScore(int score)
        {
            try
            {
                int playerNumber = GetNextPlayerNumber();

                string scoreEntry = $"Player{playerNumber},{score}"; // appends score to the file

                string path = LEADERBOARD_FILE;
                if (!Path.IsPathRooted(path))
                {
                    string rootAssetPath = "Assets";
                    while (!Directory.Exists(rootAssetPath))
                    {
                        rootAssetPath = Path.Combine("..", rootAssetPath);
                        if (rootAssetPath.Length > 100) break;
                    }
                    path = Path.Combine(rootAssetPath, path);
                }

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(scoreEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving score: {ex.Message}");
            }
        }

        public static List<(string, int)> LoadLeaderboard()
        {
            List<(string, int)> leaderboard = new List<(string, int)>();

            try
            {
                string path = LEADERBOARD_FILE;
                if (!Path.IsPathRooted(path))
                {
                    string rootAssetPath = "Assets";
                    while (!Directory.Exists(rootAssetPath))
                    {
                        rootAssetPath = Path.Combine("..", rootAssetPath);
                        if (rootAssetPath.Length > 100) break;
                    }
                    path = Path.Combine(rootAssetPath, path);
                }

                if (!File.Exists(path))
                {
                    return leaderboard; 
                }

                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        string playerName = parts[0].Trim();
                        if (int.TryParse(parts[1].Trim(), out int score))
                        {
                            leaderboard.Add((playerName, score));
                        }
                    }
                }

                // sort by score descending (highest first)
                leaderboard = leaderboard.OrderByDescending(entry => entry.Item2).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading leaderboard: {ex.Message}");
            }

            return leaderboard;
        }

        private static int GetNextPlayerNumber()
        {
            int maxPlayerNumber = 0;

            try
            {
                string path = LEADERBOARD_FILE;
                if (!Path.IsPathRooted(path))
                {
                    string rootAssetPath = "Assets";
                    while (!Directory.Exists(rootAssetPath))
                    {
                        rootAssetPath = Path.Combine("..", rootAssetPath);
                        if (rootAssetPath.Length > 100) break;
                    }
                    path = Path.Combine(rootAssetPath, path);
                }

                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(',');
                        if (parts.Length == 2)
                        {
                            string playerName = parts[0].Trim();
                            if (playerName.StartsWith("Player"))
                            {
                                string numberPart = playerName.Substring(6); 
                                if (int.TryParse(numberPart, out int playerNum))
                                {
                                    maxPlayerNumber = Math.Max(maxPlayerNumber, playerNum);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error determining player number: {ex.Message}");
            }

            return maxPlayerNumber + 1;
        }

        public static void ClearLeaderboard()
        {
            try
            {
                string path = LEADERBOARD_FILE;
                if (!Path.IsPathRooted(path))
                {
                    string rootAssetPath = "Assets";
                    while (!Directory.Exists(rootAssetPath))
                    {
                        rootAssetPath = Path.Combine("..", rootAssetPath);
                        if (rootAssetPath.Length > 100) break;
                    }
                    path = Path.Combine(rootAssetPath, path);
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing leaderboard: {ex.Message}");
            }
        }
    }
}