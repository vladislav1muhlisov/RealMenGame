using UnityEngine;

namespace RealMenGame.Scripts.MainMenu
{
    public class RecordsPanel : MonoBehaviour
    {
        [SerializeField] private ScoreLine[] scores;
        private readonly StatisticsManager _statisticsManager = new StatisticsManager();

        private void Awake()
        {
            _statisticsManager.Initialize();
            LoadAllScore();
        }

        private void LoadAllScore()
        {
            var modeStatistics = _statisticsManager.LocalStatistics.GameModeStatistics[GameMode.Easy];
            for (int i = 0; i < modeStatistics.ScoreTable.Count; i++)
            {
                scores[i].SetData(modeStatistics.ScoreTable[i]);
            }
        }

        public void AddRecord(ScoreData scoreData)
        {
            if (_statisticsManager.TryToAddNewRecord(GameMode.Easy, scoreData, out _))
            {
                _statisticsManager.Save();
                UpdateTable();
            }
        }

        private void UpdateTable()
        {
            GameModeStatistics statistics = _statisticsManager.LocalStatistics.GameModeStatistics[GameMode.Easy];
            for (int i = 0; i < statistics.ScoreTable.Count; i++)
            {
                scores[i].SetData(statistics.ScoreTable[i]);
            }
        }
    }
}