using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
        
        private GameObject hitfx;
        private SpriteRenderer SR;
        public int hitCount;

        public bool active, vulnerable;
        public enum EnemyState
        {
            Idle,
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
            maxHP = GetComponent<CharacterManager>().life; //HP is called "life" in CharacterManager, setting this up to link with Enemy script
            currentHP = maxHP;
            hitfx = GetComponent<CharacterManager>().hitfx;
            myState = EnemyState.Idle;
            pc = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            SR = GetComponent<SpriteRenderer>();
        }
    
        // Update is called once per frame
        void FixedUpdate()
        {

            switch (myState)
            {
                case EnemyState.Idle:
                    idleTimer--;
                    if (idleTimer <= 0)
                    {
                        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false && Mathf.Abs(pc.transform.position.x - transform.position.x) <= myAttack.horizontalRange &&
                            Mathf.Abs(pc.transform.position.y - transform.position.y) <= myAttack.verticalRange)
                        {
                            EnterState(EnemyState.AttackActive);
                            myState = EnemyState.AttackActive;
                            activeTimer = myAttack.activeTime;
                        }
                        else if(hitCount>=3)
                            EnterState(EnemyState.Airborn);
                        else
                        {
                            EnterState(EnemyState.Walking);
                        }
                    }
                    break;
                case EnemyState.Walking:
                    if (pc.transform.position.x > transform.position.x && !SR.flipX)
                        SR.flipX = true;
                    else if (pc.transform.position.x < transform.position.x && SR.flipX)
                    {
                        SR.flipX = false;
                    }
                    if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false && Mathf.Abs(pc.transform.position.x - transform.position.x) <= myAttack.horizontalRange &&
                        Mathf.Abs(pc.transform.position.y - transform.position.y) <= myAttack.verticalRange)
                    {
                        EnterState(EnemyState.AttackActive);
                    }
                    else if(hitCount>=3)
                        EnterState(EnemyState.Airborn);
                    else
                    {
                        //original code:
                        //transform.Translate(pc.transform.position - transform.position);
                        //note: enemy would instantly teleport to player position
                        //quick fix (need to come up with a better way for enemy movement):
                        //Debug.Log("Enemy Moving");
                        //transform.Translate(Time.fixedDeltaTime*(pc.transform.position - transform.position)/5);
                        //new code:
                        transform.position = Vector3.MoveTowards(transform.position, pc.transform.position, 0.025f);
                    }
                    break;
//                case EnemyState.AttackStartup:
//                    startupTimer--;
//                    if (startupTimer <= 0)
//                    {
//                        EnterState(EnemyState.AttackActive);
//                    }
//                    break;
                case EnemyState.AttackActive:
                    activeTimer--;
                    if (activeTimer <= 0)
                    {
                        EnterState(EnemyState.Idle);
                    }
                    break;
//                case EnemyState.AttackEndlag:
//                    endlagTimer--;
//                    if (endlagTimer <= 0)
//                    {
//                        EnterState(EnemyState.Idle);
//                    }
//                    break;
                case EnemyState.HitStun:
                    if (hitCount >= 3)
                    {
                        EnterState(EnemyState.Airborn);
                        hitCount = 0;
                    }
                    else
                    {
                        hitStunTimer--;
                        if (hitStunTimer <= 0)
                        {
                            EnterState(EnemyState.Idle);
                        }
                    }

                    break;
                case EnemyState.Airborn:
                    proneTimer--;
                    if (proneTimer <= 0)
                    {
                        vulnerable = true;
                        EnterState(EnemyState.Idle);
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
                    myAnim.Play("Idle");
                    idleTimer = idleMax;
                    break;
                case EnemyState.Walking:
                    myAnim.Play("Walking");
                    break;
                case EnemyState.AttackActive:
                    myAnim.Play("Attack");
                    myAttack.enabled = true;
                    myAttack.hitYet = false;
                    activeTimer = myAttack.startupTime;
                    break;
                case EnemyState.HitStun:
                    hitCount++;
                    myAnim.Play("HitStun");
                    break;
                case EnemyState.Airborn:
                    vulnerable = false;
                    proneTimer = proneMax;
                    myAnim.Play("Fall");
                    break;
//                case EnemyState.Prone:
//                    vulnerable = false;
//                    myAnim.Play("Prone");
//                    proneTimer = proneMax;
//                    break;
                case EnemyState.Dying:
                    myAnim.Play("Dying");
                    dyingTimer = 30;
                    break;
            }
        }

        private void FakeCheckHitBox()
        {/*
            if (SR.flipX)
            {
                if(pc.gameObject.GetComponent<CharacterManager>().SR.flipX)
                {
                    Debug.Log("hit!");
                    pc.GetHit(GetComponent<AttackScript>());
                }
                else
                {
                    Debug.Log("miss!");
                    AudioManager.instance.PlayClip("punchwhiff");
                }
            }
            else
            {
                if(!pc.gameObject.GetComponent<CharacterManager>().SR.flipX)
                {
                    Debug.Log("hit!");
                    pc.GetHit(GetComponent<AttackScript>());
                }
                else
                {
                    Debug.Log("miss!");
                    AudioManager.instance.PlayClip("punchwhiff");
                }
            }*/
        }

        private void CheckHitBox()
        {
            RaycastHit2D[] boxResult;
            if(SR.flipX)
                boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), new Vector2(1, 4), 0f,
                new Vector2(-1, 0), 1.5f, 1 << 8);
            else
                boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), 
                    new Vector2(1, 4), 0f, new Vector2(1, 0), 1.5f, 1 << 8);
            Debug.Log(boxResult.Length);
            if (boxResult.Length == 0 && !AudioManager.instance.source.isPlaying)
            {
                AudioManager.instance.PlayClip("punchwhiff");
                Debug.Log("Enemy Punch Whiffed");
            }
            if (boxResult.Length > 0)
            {
                for (int i = 0; i < boxResult.Length; i++)
                {
                    CharacterManager tmp = boxResult[i].collider.GetComponent<CharacterManager>();

                    if (tmp.GetComponent<PlayerManager>() != null)
                    {
                        tmp.life--;
                        tmp.Checklife();
                        tmp.GetComponent<PlayerManager>().GetHit(GetComponent<AttackScript>());
                    }

                }
            }
        }

        public void GetHit(AttackScript hitBy, bool punched)
        {
            if (punched)
            {
                AudioManager.instance.PlayClip("punched");
                Debug.Log("Enemy hit by punch");
            }
            else
            {
                AudioManager.instance.PlayClip("kicked");
                Debug.Log("Enemy hit by kick");
            }
            /*
            if (Input.GetKeyDown(KeyCode.Z))
                AudioManager.instance.PlayClip("punched");
            else if(Input.GetKeyDown(KeyCode.X))
                AudioManager.instance.PlayClip("kicked");*/
            Debug.Log(punched);
            currentHP -= hitBy.damage;
            var particlepos = new Vector2(transform.position.x-1.2f,transform.position.y +3);
            var hitfxclone = Instantiate(hitfx, particlepos, Quaternion.identity);
            hitStunTimer = 120;
            EnterState(EnemyState.HitStun);
            Destroy(hitfxclone, 1f);
        }
}
