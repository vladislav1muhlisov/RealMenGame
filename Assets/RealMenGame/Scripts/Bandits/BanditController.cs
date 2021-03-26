using System;
using UnityEngine;
using UnityEngine.AI;

namespace RealMenGame.Scripts.Bandits
{
    public class BanditController : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        private Transform _target;
        private BanditState _currentState;

        private enum BanditState
        {
            MovingToStall,
            ReachedStall,
            MovingAway
        }

        private void Start()
        {
            _target = LarekManager.Instance.transform;

            _currentState = BanditState.MovingToStall;
            
            _navMeshAgent.SetDestination(_target.position);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case BanditState.MovingAway:
                    break;
                case BanditState.ReachedStall:
                    break;
                case BanditState.MovingToStall:
                    ProcessMovingToStall();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessMovingToStall()
        {
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                _currentState = BanditState.ReachedStall;
            }
                    
            _navMeshAgent.SetDestination(_target.position);
        }

        private void ProcessMovingAway()
        {
            var awayPoint = SpawnPointsManager.Instance.GetRandomAwayPoint();

            _navMeshAgent.SetDestination(awayPoint.position);
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