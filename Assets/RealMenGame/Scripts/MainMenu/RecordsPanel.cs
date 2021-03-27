using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RealMenGame.Scripts.MainMenu
{
    public class RecordsPanel : MonoBehaviour
    {
        [SerializeField] private ScoreLine[] scores;
        [SerializeField] private NameInputPanel inputPanel;
        private readonly StatisticsManager _statisticsManager = new StatisticsManager();

        private void Awake()
        {
            _statisticsManager.Initialize();
            LoadAllScore();
            var scoreValue = GameManager.Instance.Score.Value;
            if (scoreValue > 0)
            {
                AddNewRecord(scoreValue).Forget();
            }
        }

        private async UniTask AddNewRecord(int score)
        {
            var yourName = await inputPanel.GetName();
            AddRecord(new ScoreData(score, yourName));
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