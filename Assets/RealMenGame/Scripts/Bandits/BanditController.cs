//using System;

using System;
using System.Linq;
using DG.Tweening;
using RealMenGame.Scripts.Settings;
using RealMenGame.Scripts.UI;
using UniRx;
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
        [SerializeField] private int _score = 100;
        [SerializeField] private ShaurmaDisplay _shaurmaDisplay;
        [SerializeField] private KebabIngredients Ingredients;

        public Mood Mood;

        public BanditSettings Settings;

        public Vector3[] WayPoints;
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
            Debug.Log("TRIGGER");
            
            var projectile = other.GetComponent<KebabProjectile>();
            var kebabIngredients = projectile.Ingredients;

            if ((CurrentState == BanditState.OnWay || CurrentState == BanditState.MovingToStall) &&
                _navMeshAgent.enabled)
            {
                var theSame = Ingredients.Ingredients.Count != 0 &&
                              kebabIngredients.Ingredients.Count != 0 &&
                              Ingredients.Ingredients.All(ingredient =>
                                  kebabIngredients.Ingredients[ingredient.Key] == ingredient.Value);

                if (theSame)
                {
                    projectile.RaiseOnSuccess(_score);
                }

                _animator.SetBool(IsKebabRightHash, theSame);
                _animator.SetTrigger(KebabCaughtHash);

                _navMeshAgent.enabled = false;

                var delay = TimeSpan.FromSeconds(theSame ? Settings.AnimationRightDelay : Settings.AnimationWrongDelay);

                if (theSame)
                {
                    CurrentState = BanditState.MovingAway;

                    Observable.Timer(delay).Subscribe(_ =>
                    {
                        _shaurmaDisplay.gameObject.SetActive(false);
                        Mood.gameObject.SetActive(true);
                        Mood.SetMood(true);

                        var awayPoint = SpawnManager.Instance.GetRandomAwayPoint();

                        _navMeshAgent.enabled = true;
                        _navMeshAgent.speed = Settings.AwaySpeed;

                        _navMeshAgent.SetDestination(awayPoint.position);
                        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    });
                }
                else
                {
                    Observable.Timer(delay).Subscribe(_ => _navMeshAgent.enabled = true);
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
                if (_navMeshAgent.enabled == false) return false;
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

            _shaurmaDisplay.gameObject.SetActive(true);
            _shaurmaDisplay.SetIngredients(Ingredients.Ingredients);

            Mood.gameObject.SetActive(false);

            CurrentState = BanditState.OnWay;

            _currentWayPoint = 0;

            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = Settings.NormalSpeed;
            _navMeshAgent.SetDestination(WayPoints[_currentWayPoint]);
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

            _navMeshAgent.SetDestination(WayPoints[_currentWayPoint]);
        }

        private void ProcessMovingToStall()
        {
            if (IsDestinationReached == false) return;

            CurrentState = BanditState.ReachedStall;

            StallManager.Instance.SetDamage(_damage);
            _shaurmaDisplay.gameObject.SetActive(false);

            Mood.gameObject.SetActive(true);
            Mood.SetMood(false);

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