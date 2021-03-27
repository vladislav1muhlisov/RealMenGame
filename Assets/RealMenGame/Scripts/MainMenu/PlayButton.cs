using JetBrains.Annotations;
using UnityEngine;

namespace RealMenGame.Scripts.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [UsedImplicitly]
        public void OnClick()
        {
            GameManager.Instance.NewGame();
        }
    }
}