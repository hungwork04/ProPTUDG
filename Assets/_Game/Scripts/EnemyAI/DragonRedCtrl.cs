
using System;
using System.Collections.Generic;
using Pathfinding;
using StateMachine;

using UnityEngine;
using UnityEngine.Serialization;
using Utilities;


namespace BossMap
{
    public class DragonRedCtrl : EnemyAI
    {

        

    [Header("Attack")]
   

    public GameObject FireBallPrefab;
   
    
    public Vector3 LeftFirePoint;
    public Vector3 RightFirePoint;


    [Header("Spawn")] 
    public float TimeSpawn = 60;

    public int SpawnAmount = 1;
    public GameObject DemonPrefab;

    public List<Vector3> SpawnPositionAvailables = new List<Vector3>();
    public bool IsSpawn;

    public CountdownTimer SpawnTimeCountDown { get; set; }


    #region State

    public ChaseState ChaseState { get; set; }
    public IdleState IdleState { get; set; }
    public PatrolState PatrolState { get; set; }
    public RangedAttackState RangedAttackState { get; set; }
    public SpawnState SpawnState { get; set; }
    #endregion

  
  

 

    #region Unity Callback Functions

    
    protected override void Awake()
    {
        base.Awake();
        
        
        IdleState = new IdleState(this, "Idle");
        ChaseState = new ChaseState(this, "Move");
        PatrolState = new PatrolState(this, "Move");
        RangedAttackState = new RangedAttackState(this, "Attack");
        SpawnState = new SpawnState(this, "Attack");
        Any(SpawnState, new FuncPredicate(() => IsSpawn));
        Any(RangedAttackState, new FuncPredicate(IsPlayerInAttackRange));
        
        At(RangedAttackState, PatrolState, new FuncPredicate(() => !IsPlayerInAttackRange() && !IsPlayerVisible()));
        At(RangedAttackState, ChaseState, new FuncPredicate(() => !IsPlayerInAttackRange() && IsPlayerVisible()));
        
        At(IdleState, PatrolState, new FuncPredicate(() => FinishIdleState && !IsPlayerVisible()));
        At(IdleState, ChaseState, new FuncPredicate(() => FinishIdleState && IsPlayerVisible()));
        At(ChaseState, PatrolState,new FuncPredicate(() => !IsPlayerVisible()));
        At(PatrolState, ChaseState, new FuncPredicate(IsPlayerVisible));
        
        At(PatrolState, IdleState, new FuncPredicate(() => Target != null && Vector2.Distance(Target.position,transform.position) <= 2));
        
      
    }
    
    
    protected override void OnEnable()
    {
        base.OnEnable();
        this.AttackRange = 8;
        this.ApproachRange = 10;
      
        IsSpawn = false;
        StateMachine.SetState(IdleState);
        if (TimeSpawn > 0)
        {
            SpawnTimeCountDown = new CountdownTimer(TimeSpawn);
            SpawnTimeCountDown.Start();
            SpawnTimeCountDown.OnTimerStop += () => IsSpawn = true;
        }
    }
    
    #endregion


    protected override void Update()
    {
        base.Update();
        SpawnTimeCountDown.Tick(Time.deltaTime);
    }
    

   
    private void OnDrawGizmos()
    {
        if (Player != null) Debug.DrawLine(transform.position, Player.position, Color.red);
        if (SpawnPositionAvailables != null && SpawnPositionAvailables.Count > 0)
        {
            for (int i = 0; i < SpawnPositionAvailables.Count; ++i)
            {
                Vector3 spawnPos = transform.TransformPoint(SpawnPositionAvailables[i]);
                Gizmos.DrawWireSphere(spawnPos, 1f);

            }
        }
    }
}

}
