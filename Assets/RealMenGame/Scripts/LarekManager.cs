using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class LarekManager : MonoBehaviourSingleton<LarekManager>
    {
        public ReactiveProperty<int> Health = new ReactiveProperty<int>();

        public void SetDamage()
        {
            
        }
    }
}
