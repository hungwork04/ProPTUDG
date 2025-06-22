using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class EnemyAI : Entity
    {
        
        public string CurrentState;
        public Transform Player;
        public Transform Target;
        public List<Transform> PatrolPosition = new List<Transform>();
        public Transform BossGFX;
        
        
        [Header("Idle")] public float TimeIdleDelay = 3f;
        [Header("Attack")] 
        public float AttackRange = 8f;

        public float ApproachRange = 10;
        private bool isInAttackRange;

        
        public bool FinishIdleState { get; set; }
        #region AStart PathFinding Component

        public AIDestinationSetter AIDestinationSetter;
        public AIPath AIPath;

        #endregion
        public override void LoadComponent()
        {
            base.LoadComponent();
      
            if (BossGFX == null) BossGFX = transform.Find("Model");
            if (AIDestinationSetter == null) AIDestinationSetter = GetComponent<AIDestinationSetter>();
            if (AIPath == null) AIPath = GetComponent<AIPath>();
        
        }

        protected virtual void OnEnable()
        {
          
            isInAttackRange = false;

            PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();

            if (playerAttack != null && Player == null) Player = playerAttack.transform;
           
        }

        public void SetFacing(bool isFacingRight)
        {
            Vector3 currentGFXScale = BossGFX.localScale;
            BossGFX.localScale = new Vector3((isFacingRight ? -1 : 1) * Mathf.Abs(currentGFXScale.x), currentGFXScale.y, currentGFXScale.z);
        }
        public bool IsPlayerVisible()
        {
            if (Player == null) return false;

            Vector2 origin = transform.position;
            Vector2 direction = (Player.position - transform.position).normalized;

            RaycastHit2D ray = Physics2D.Raycast(origin, direction, Vector3.Distance(origin, Player.position), LayerMask.GetMask("Player", "Ground"));
      
            return ray.collider != null && ray.collider.CompareTag("Player");
        
        }
        protected bool IsPlayerInAttackRange()
        {
            if (!IsPlayerVisible()) return false;
       
            float distance = Vector2.Distance(transform.position, Player.position);
            if (isInAttackRange) isInAttackRange = distance <= ApproachRange;
            else isInAttackRange = distance <= AttackRange;
            return isInAttackRange;
        
       
        }
        
    }
}

