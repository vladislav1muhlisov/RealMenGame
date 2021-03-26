using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RealMenGame.Scripts.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [UsedImplicitly]
        public void OnClick()
        {
            SceneManager.LoadScene("Shaurmyachnaya");
        }
    }
}