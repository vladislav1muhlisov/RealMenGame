﻿using System;
using UnityEngine;
using UnityEngine.AI;

namespace RealMenGame.Scripts.Bandits
{
    public class BanditController : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        private Transform _target;
        [SerializeField]
        public BanditState CurrentState;

        public int BanditIndex;

        public enum BanditState
        {
            Spawned,
            MovingToStall,
            ReachedStall,
            MovingAway,
            Done
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
            _target = LarekManager.Instance.transform;

            CurrentState = BanditState.MovingToStall;
            
            _navMeshAgent.SetDestination(_target.position);
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