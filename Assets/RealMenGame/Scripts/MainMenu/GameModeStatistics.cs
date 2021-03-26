using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RealMenGame.Scripts.MainMenu
{
    public class GameModeStatistics
    {
        public const int MAXCount = 10;

        private readonly List<ScoreData> _scoreTable;

        public List<ScoreData> ScoreTable => _scoreTable;

        [JsonConstructor]
        private GameModeStatistics(List<ScoreData> scoreTable)
        {
            _scoreTable = scoreTable;
        }

        internal GameModeStatistics()
        {
            _scoreTable = new List<ScoreData>(MAXCount);
            for (int i = 0; i < MAXCount; i++)
            {
                _scoreTable.Add(new ScoreData(0, "-"));
            }
        }

        internal bool TryToAddNewRecord(ScoreData scoreData, out int index)
        {
            if (scoreData.Score == 0)
            {
                index = -1;
                return false;
            }

            int insertIndex = -1;

            bool tryGetGame = false;
            if (!string.IsNullOrEmpty(scoreData.GameId))
            {
                tryGetGame = TryGetGame(scoreData, out int gameIndex);
                if (tryGetGame)
                {
                    if (scoreData.Score == _scoreTable[gameIndex].Score)
                    {
                        index = -1;
                        return false;
                    }

                    if (scoreData.Score < _scoreTable[gameIndex].Score)
                    {
                        Debug.LogError("New score for this game is less than previous!");
                    }

                    _scoreTable.RemoveAt(gameIndex);
                }
            }

            for (int i = 0; i < _scoreTable.Count; i++)
            {
                if (scoreData.Score > _scoreTable[i].Score)
                {
                    _scoreTable.Insert(i, scoreData);
                    insertIndex = i;
                    break;
                }
            }

            if (insertIndex != -1)
            {
                if (!tryGetGame)
                {
                    _scoreTable.RemoveAt(MAXCount);
                }

                index = insertIndex;
                return true;
            }

            if (tryGetGame)
            {
                Debug.LogError("New score for this game is less than all games in the table! Put to the last place");
                _scoreTable[MAXCount - 1] = scoreData;
            }

            index = -1;
            return false;
        }

        private bool TryGetGame(ScoreData newScoreData, out int index)
        {
            index = _scoreTable.FindIndex(data => data.GameId == newScoreData.GameId);
            return index != -1;
        }
    }
}