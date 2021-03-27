using TMPro;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts.UI
{
    public class ScoreIndicator : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [SerializeField] private TMP_Text scoreText;

        private void Awake()
        {
            GameManager.Instance.Score.Subscribe(OnScoreChanged).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}