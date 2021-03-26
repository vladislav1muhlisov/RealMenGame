using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RealMenGame.Scripts.Bandits
{
    public class BanditController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] public BanditState CurrentState;
        [SerializeField] private int _damage;

        public int BanditIndex;

        private static readonly int IsKebabRightHash = Animator.StringToHash("IsKebabRight");
        private static readonly int KebabCaughtHash = Animator.StringToHash("KebabCatched");

        public enum BanditState
        {
            Spawned,
            MovingToStall,
            ReachedStall,
            MovingAway,
            Done
        }

        private void OnTriggerEnter(Collider other)
        {
            //var kebab = other.GetComponent<Shaurma>();
            //TODO: Проверить, правильная ли шаурма
            var isKebabRight = Random.Range(0, 1) == 1;

            _animator.SetBool(IsKebabRightHash, isKebabRight);
            _animator.SetTrigger(KebabCaughtHash);
        }

        private bool IsDestinationReached
        {
            get
            {
                if (_navMeshAgent.pathPending) return false;
                if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return false;

                return _navMeshAgent.hasPath == false || _navMeshAgent.velocity.sqrMagnitude == 0f;
            }
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case BanditState.Spawned:
                    ProcessSpawned();
                    break;
                case BanditState.MovingAway:
                    ProcessMovingAway();
                    break;
                case BanditState.ReachedStall:
                    StallManager.Instance.SetDamage(_damage);
                    CurrentState = BanditState.MovingAway;
                    break;
                case BanditState.MovingToStall:
                    ProcessMovingToStall();
                    break;
                case BanditState.Done:
                    SpawnManager.Instance.DeSpawnBandit(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessSpawned()
        {
            CurrentState = BanditState.MovingToStall;

            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(StallManager.Instance.GetRandomTarget().position);
        }

        private void ProcessMovingToStall()
        {
            if (IsDestinationReached == false) return;
            
            CurrentState = BanditState.ReachedStall;
                
            var awayPoint = SpawnManager.Instance.GetRandomAwayPoint();

            _navMeshAgent.SetDestination(awayPoint.position);
        }

        private void ProcessMovingAway()
        {
            if (IsDestinationReached == false) return;

            CurrentState = BanditState.Done;
            _navMeshAgent.enabled = false;
        }

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();

            if (_navMeshAgent == null)
            {
                _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }
        }
    }
}