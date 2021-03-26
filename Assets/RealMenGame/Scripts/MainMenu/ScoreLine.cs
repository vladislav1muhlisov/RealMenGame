using TMPro;
using UnityEngine;

namespace RealMenGame.Scripts.MainMenu
{
    public class ScoreLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI playerName;

        public void SetData(ScoreData scoreData)
        {
            score.text = scoreData.Score.ToString();
            playerName.text = scoreData.Name;
        }
    }
}