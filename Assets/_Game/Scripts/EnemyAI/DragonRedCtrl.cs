

using System.Collections.Generic;
using Game.Define;
using Game.UI;
using StateMachine;

using UnityEngine;

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
    public float TimeSpawn = 10;

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
    public HurtState HurtState { get; set; }
    public DeadState DeadState { get; set; }
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

        HurtState = new HurtState(this, "Hurt");
        DeadState = new DeadState(this, "Dead");
        
        Any(DeadState, new FuncPredicate(() => OnDead));
        Any(HurtState, new FuncPredicate(() => IsHurt));
        Any(SpawnState, new FuncPredicate(() => IsSpawn));
        Any(RangedAttackState, new FuncPredicate(IsPlayerInAttackRange));
        
        
        At(DeadState, IdleState, new FuncPredicate(() => !OnDead));
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
        this.maxHP = 2;
        if (healthSystem != null)
        {
            healthSystem.Init(maxHP);
            healthSystem.OnHPChange = OnHPChange;
            healthSystem.OnDead = OnDeading;
            TABossMapUI taBossMapUI = UIScreen.Instance as TABossMapUI;
        
            if(taBossMapUI != null) taBossMapUI.OnBossHPChange(healthSystem.curHP/healthSystem.maxHP);
        }
        IsSpawn = false;
        StateMachine.SetState(IdleState);
        if (TimeSpawn > 0)
        {
            SpawnTimeCountDown = new CountdownTimer(TimeSpawn);
            SpawnTimeCountDown.Start();
            SpawnTimeCountDown.OnTimerStop += () => IsSpawn = true;
        }
    }

    private void OnHPChange()
    {
        IsHurt = true;
        TABossMapUI taBossMapUI = UIScreen.Instance as TABossMapUI;
        
        if(taBossMapUI != null) taBossMapUI.OnBossHPChange(healthSystem.curHP/healthSystem.maxHP);
    }

    private void OnDeading()
    {
        OnDead = true;
        ObserverManager<GameEventType>.Notify(GameEventType.Win);
    }
    protected override void Update()
    {
        base.Update();
        SpawnTimeCountDown.Tick(Time.deltaTime);
    }
    
    #endregion

    public override bool IsPlayerVisible()
    {
        if (Player == null) return false;

        Vector2 origin = transform.TransformPoint(BossGFX.localScale.x > 0 ? LeftFirePoint : RightFirePoint) ;

        origin = origin.Add(y: -.35f);

        var position = Player.position;

        position = position.Add(y: -.35f);
        Vector2 direction = ((Vector2)position - origin).normalized;

        RaycastHit2D ray1 = Physics2D.Raycast(origin, direction, Vector3.Distance(origin, position), LayerMask.GetMask("Player", "Ground"));

        origin = origin.Add(y: .7f);
        position = position.Add(y: .7f);
        direction = ((Vector2)position - origin).normalized;

        RaycastHit2D ray2 = Physics2D.Raycast(origin, direction, Vector3.Distance(origin, position), LayerMask.GetMask("Player", "Ground"));
        
        
        bool hit1 = ray1.collider != null;
    

        bool tag1 = hit1 && ray1.collider.CompareTag("Player");
       

        bool hit2 = ray2.collider != null;
     

        bool tag2 = hit2 && ray2.collider.CompareTag("Player");
        

        return tag1 && tag2;

    }


    private void OnDrawGizmos()
    {
        if (Player != null)
        {
            Vector3 origin = transform.TransformPoint(BossGFX.localScale.x > 0 ? LeftFirePoint : RightFirePoint);
            origin = origin.Add(y: -.35f);
            Vector3 position = Player.position;
            position = position.Add(y: -.35f);
            Debug.DrawLine(origin, position, Color.red);
            origin = origin.Add(y: .7f);
            position = position.Add(y: .7f);
           
            Debug.DrawLine(origin, position, Color.red);
        }
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
