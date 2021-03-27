using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

namespace RealMenGame.Scripts.Sounds
{
    public class CharacterSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Animator _animator;

        [SerializeField] private List<AudioClip> _catchSounds;
        [SerializeField] private List<AudioClip> _nomSounds;
        [SerializeField] private List<AudioClip> _satisfiedSounds;
        [SerializeField] private List<AudioClip> _angerSounds;

        private readonly int _eatingId = Animator.StringToHash("Eating");

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            var triggers = _animator.GetBehaviours<ObservableStateMachineTrigger>();

            foreach (ObservableStateMachineTrigger trigger in triggers)
            {
                trigger.OnStateEnterAsObservable()
                    .Subscribe(x =>
                    {
                        Debug.LogFormat("state name:{0} nameHash:{1} layerIndex:{2}", x.Animator.name,
                            x.StateInfo.shortNameHash, x.LayerIndex);
                        OnAnimatorEvent(x);
                    })
                    .AddTo(_compositeDisposable);
            }
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnAnimatorEvent(ObservableStateMachineTrigger.OnStateInfo info)
        {
            if (info.StateInfo.shortNameHash == _eatingId)
            {
                OnEatBegin();
            }
        }

        private void OnEatBegin()
        {
            var nomSoundsCount = _nomSounds.Count;
            if (nomSoundsCount > 0)
            {
                var randomSound = _nomSounds[Random.Range(0, nomSoundsCount)];
                _audioSource.clip = randomSound;
                _audioSource.Play();
            }
        }
    }
}