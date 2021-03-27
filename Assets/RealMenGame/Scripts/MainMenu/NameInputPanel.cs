using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RealMenGame.Scripts.MainMenu
{
    public class NameInputPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private GameObject _root;

        [SerializeField] private Button _button;

        // private const int LimitMax = 25;
        private const int LimitMin = 3;

        private UniTaskCompletionSource<string> _taskCompletionSource;

        private void Awake()
        {
            _root.SetActive(false);
            OnTextChanged(_inputField.text);
            _inputField.onValueChanged.AsObservable().Subscribe(OnTextChanged).AddTo(gameObject);
            _button.onClick.AsObservable().Subscribe(s => OnClick()).AddTo(gameObject);
        }

        private void OnTextChanged(string text)
        {
            _button.interactable = LimitMin < text.Length;
        }

        public async UniTask<string> GetName()
        {
            _root.SetActive(true);
            _taskCompletionSource = new UniTaskCompletionSource<string>();
            var yourName = await _taskCompletionSource.Task;
            _root.SetActive(false);
            return yourName;
        }

        private void OnClick()
        {
            _taskCompletionSource.TrySetResult(_inputField.text);
        }
    }
}