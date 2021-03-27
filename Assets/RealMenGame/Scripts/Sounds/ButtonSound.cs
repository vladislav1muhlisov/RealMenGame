using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RealMenGame.Scripts.Sounds
{
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioClipSettings _audioClip;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            _button.onClick.AsObservable().Subscribe(OnClick).AddTo(gameObject);
        }

        private void OnClick(Unit _)
        {
            UISound.Instance.Play(_audioClip);
        }
    }
}