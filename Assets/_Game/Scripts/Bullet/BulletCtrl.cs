
using StateMachine;
using UnityEngine;
using Utilities;


namespace BossMap
{
    public class BulletCtrl : Entity
    {

 
        [Header("Modifier")]
        public float Speed = 10f;

        public float Damage = 5;
        public Vector3 Direction = Vector3.right;
        public Rigidbody2D RB;


        public float TimeDespawn = 50f;
        private CountdownTimer countdownTimer;
        
        #region States

        private BulletExplodeState explodeState;
        private BulletMoveState moveState;
        private BulletDespawnState despawnState;

        #endregion

        #region Check Variables

        public bool IsDespawn { get; set; }
        private bool isExplode;

        #endregion
        public Transform Target { get; set; }

        
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (RB == null) RB = GetComponent<Rigidbody2D>();
        }

        #region Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            explodeState = new BulletExplodeState(this, "Explode");
            moveState = new BulletMoveState(this, "Idle");
            despawnState = new BulletDespawnState(this, string.Empty);
            
            Any(despawnState, new FuncPredicate(() => IsDespawn));
            Any(explodeState, new FuncPredicate(() => isExplode && !IsDespawn));
            Any(moveState, new FuncPredicate(() => !(IsDespawn && isExplode)));
            
        }

        private void OnEnable()
        {
            IsDespawn = false;
            isExplode = false;
            StateMachine.SetState(moveState);
            if (TimeDespawn >= 0)
            {
                countdownTimer = new CountdownTimer(TimeDespawn);
                countdownTimer.Start();
                countdownTimer.OnTimerStop += () => IsDespawn = true;
            }
            
        }

        protected override void Update()
        {
            base.Update();
            if(countdownTimer != null) countdownTimer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Ground"))
            {
                Target = other.transform;
                isExplode = true;
            }
        }

        #endregion
        
    }
}

