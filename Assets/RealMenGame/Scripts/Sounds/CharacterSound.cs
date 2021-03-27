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
        private readonly int _angerId = Animator.StringToHash("Standing Taunt Battlecry");
        private readonly int _catchId = Animator.StringToHash("StandCatch");
        private readonly int _walkId = Animator.StringToHash("Dwarf Walk");

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
                    .Subscribe(OnAnimatorEvent)
                    .AddTo(_compositeDisposable);
            }
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnAnimatorEvent(ObservableStateMachineTrigger.OnStateInfo info)
        {
            var shortNameHash = info.StateInfo.shortNameHash;
            if (shortNameHash == _eatingId)
            {
                PlayRandomSound(_nomSounds);
            }
            else if (shortNameHash == _angerId)
            {
                PlayRandomSound(_angerSounds);
            }
            else if (shortNameHash == _catchId)
            {
                PlayRandomSound(_catchSounds);
            }
            else if (shortNameHash == _walkId)
            {
                PlayRandomSound(_satisfiedSounds);
            }
        }

        private void PlayRandomSound(List<AudioClip> audioSources)
        {
            var count = audioSources.Count;
            if (count > 0)
            {
                var randomSound = audioSources[Random.Range(0, count)];
                _audioSource.clip = randomSound;
                _audioSource.Play();
            }
        }
    }
}