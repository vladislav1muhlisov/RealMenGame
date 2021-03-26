using System;
using System.Linq;
using DG.Tweening;
using RealMenGame.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace RealMenGame.Scripts.Bandits
{
    public class BanditController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] public BanditState CurrentState;
        [SerializeField] private int _damage;
        [SerializeField] private ShaurmaDisplay _shaurmaDisplay;
        [SerializeField] private KebabIngredients Ingredients;

        public Transform[] WayPoints;
        private int _currentWayPoint;

        public int BanditIndex;

        private static readonly int IsKebabRightHash = Animator.StringToHash("IsKebabRight");
        private static readonly int KebabCaughtHash = Animator.StringToHash("KebabCatched");

        public enum BanditState
        {
            Spawned,
            OnWay,
            MovingToStall,
            ReachedStall,
            MovingAway,
            Done
        }

        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<KebabProjectile>();
            var kebabIngredients = projectile.Ingredients;

            if (CurrentState == BanditState.MovingToStall)
            {
                var theSame = Ingredients.Ingredients.Count != 0 &&
                              kebabIngredients.Ingredients.Count != 0 &&
                              Ingredients.Ingredients.All(ingredient =>
                                  kebabIngredients.Ingredients[ingredient.Key] == ingredient.Value);

                _animator.SetBool(IsKebabRightHash, theSame);
                _animator.SetTrigger(KebabCaughtHash);

                if (theSame)
                {
                    CurrentState = BanditState.MovingAway;

                    var awayPoint = SpawnManager.Instance.GetRandomAwayPoint();

                    _navMeshAgent.SetDestination(awayPoint.position);
                    _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                }
            }

            projectile.TweenHandle.Kill();
            other.enabled = false;

            Destroy(other.gameObject);
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
                case BanditState.OnWay:
                    ProcessOnWay();
                    break;
                case BanditState.MovingAway:
                    ProcessMovingAway();
                    break;
                case BanditState.ReachedStall:
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
            Ingredients.Ingredients = IngredientsRandomGeneratorUtil.Generate();
            _shaurmaDisplay.SetIngredients(Ingredients.Ingredients);

            CurrentState = BanditState.OnWay;

            _currentWayPoint = 0;

            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(WayPoints[_currentWayPoint].position);
        }

        private void ProcessOnWay()
        {
            if (IsDestinationReached == false) return;

            ++_currentWayPoint;

            if (_currentWayPoint >= WayPoints.Length)
            {
                CurrentState = BanditState.MovingToStall;
                _navMeshAgent.SetDestination(StallManager.Instance.GetRandomTarget().position);

                return;
            }

            _navMeshAgent.SetDestination(WayPoints[_currentWayPoint].position);
        }

        private void ProcessMovingToStall()
        {
            if (IsDestinationReached == false) return;

            CurrentState = BanditState.ReachedStall;
            
            StallManager.Instance.SetDamage(_damage);

            var awayPoint = SpawnManager.Instance.GetRandomAwayPoint();
            
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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