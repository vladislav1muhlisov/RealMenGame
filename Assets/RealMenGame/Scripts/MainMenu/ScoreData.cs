using JetBrains.Annotations;
using Newtonsoft.Json;

namespace RealMenGame.Scripts.MainMenu
{
    public class ScoreData
    {
        [CanBeNull] public readonly string GameId;
        public readonly int Score;
        public string Name;

        [JsonConstructor]
        public ScoreData([NotNull] string gameId, int score, string name)
        {
            GameId = gameId;
            Score = score;
            Name = name;
        }

        public ScoreData(int score, string name)
        {
            GameId = null;
            Score = score;
            Name = name;
        }
    }
}