using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterManager))]
public class Enemy : MonoBehaviour
{
    public PlayerManager pc;
        public AttackScript myAttack;
        public Animator myAnim;
        public int idleTimer, proneTimer, startupTimer, activeTimer, endlagTimer, hitStunTimer, idleMax, proneMax, dyingTimer;
        public float groundLevel, fallSpeed;
        public float currentHP, maxHP;
    
        public bool active, vulnerable;
        public enum EnemyState
        {
            Idle,
            AttackIdle,
            Walking,
            AttackStartup,
            AttackActive,
            AttackEndlag,
            HitStun,
            Airborn,
            Prone,
            Dying
        }
    
        public EnemyState myState;
        
        // Start is called before the first frame update
        void Start()
        {
            currentHP = maxHP;
            myState = EnemyState.AttackIdle;
        }
    
        // Update is called once per frame
        void FixedUpdate()
        {
            switch (myState)
            {
                case EnemyState.AttackIdle:
                    idleTimer--;
                    if (idleTimer <= 0)
                    {
                        if (Mathf.Abs(pc.transform.position.x - transform.position.x) <= myAttack.horizontalRange &&
                            Mathf.Abs(pc.transform.position.y - transform.position.y) <= myAttack.verticalRange)
                        {
                            EnterState(EnemyState.AttackStartup);
                            myState = EnemyState.AttackStartup;
                            startupTimer = myAttack.startupTime;
                        }
                        else
                        {
                            EnterState(EnemyState.Walking);
                        }
                    }
                    break;
                case EnemyState.Walking:
                    if (Mathf.Abs(pc.transform.position.x - transform.position.x) <= myAttack.horizontalRange &&
                        Mathf.Abs(pc.transform.position.y - transform.position.y) <= myAttack.verticalRange)
                    {
                        EnterState(EnemyState.AttackStartup);
                    }
                    else
                    {
                        transform.Translate(pc.transform.position - transform.position);
                    }
                    break;
                case EnemyState.AttackStartup:
                    startupTimer--;
                    if (startupTimer <= 0)
                    {
                        EnterState(EnemyState.AttackActive);
                    }
                    break;
                case EnemyState.AttackActive:
                    activeTimer--;
                    if (activeTimer <= 0)
                    {
                        EnterState(EnemyState.AttackEndlag);
                    }
                    break;
                case EnemyState.AttackEndlag:
                    endlagTimer--;
                    if (endlagTimer <= 0)
                    {
                        EnterState(EnemyState.AttackIdle);
                    }
                    break;
                case EnemyState.HitStun:
                    hitStunTimer--;
                    if (hitStunTimer <= 0)
                    {
                        EnterState(EnemyState.AttackIdle);
                    }
                    break;
                case EnemyState.Airborn:
                    transform.Translate(transform.position.x, transform.position.y - fallSpeed, transform.position.x);
                    if (transform.position.y <= groundLevel)
                    {
                        EnterState(EnemyState.Prone);
                    }
                    break;
                case EnemyState.Prone:
                    proneTimer--;
                    if (proneTimer <= 0)
                    {
                        vulnerable = true;
                        EnterState(EnemyState.AttackIdle);
                    }
                    break;
                case EnemyState.Dying:
                    dyingTimer--;
                    if (dyingTimer <= 0)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    
        public void EnterState(EnemyState endState)
        {
            myState = endState;
            switch (endState)
            {
                case EnemyState.Idle:
                    myAnim.Play("Idle Animation");
                    idleTimer = idleMax;
                    break;
                case EnemyState.Walking:
                    myAnim.Play("Walking Animation");
                    break;
                case EnemyState.AttackStartup:
                    myAnim.Play("Attack Startup Animation");
                    startupTimer = myAttack.startupTime;
                    break;
                case EnemyState.AttackActive:
                    myAnim.Play("Attack Active Animation");
                    myAttack.enabled = true;
                    myAttack.hitYet = false;
                    activeTimer = myAttack.startupTime;
                    break;
                case EnemyState.AttackEndlag:
                    myAnim.Play("Attack Endlag Animation");
                    myAttack.enabled = false;
                    endlagTimer = myAttack.startupTime;
                    break;
                case EnemyState.HitStun:
                    myAnim.Play("HitStun Animation");
                    break;
                case EnemyState.Airborn:
                    groundLevel = transform.position.x;
                    myAnim.Play("Airborn Animation Animation");
                    break;
                case EnemyState.Prone:
                    vulnerable = false;
                    myAnim.Play("Prone Animation");
                    proneTimer = proneMax;
                    break;
                case EnemyState.Dying:
                    myAnim.Play("Dying Animation");
                    dyingTimer = 30;
                    break;
            }
        }
    
        public void GetHit(AttackScript hitBy)
        {
            currentHP -= hitBy.damage;
            hitStunTimer = 120;
            EnterState(EnemyState.HitStun);
        }
}
