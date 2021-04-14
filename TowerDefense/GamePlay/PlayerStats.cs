using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class PlayerStats
    {
        public PlayerStats() { }
        public PlayerStats(List<float> highScore, int level)
        {
            this.HighScores = highScore;
            this.Level = level;
        }
        public List<float> HighScores;
        public int Level;

        public void AddScore(float Score)
        {
            if(HighScores.Count != 5)
            {
                HighScores.Add(Score);
                HighScores.Sort();
                HighScores.Reverse();
                return;
            }
            float temp = -1;
            for(int x = 0; x < 5; x++)
            {
                if(temp != -1)
                {
                    var tempHighScore = HighScores[x];
                    HighScores[x] = temp;
                    temp = tempHighScore;
                }
                if(Score > HighScores[x])
                {
                    temp = HighScores[x];
                    HighScores[x] = Score;

                }
            }
        }
    }
}
