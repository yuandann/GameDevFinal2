using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterManager))]
public class PlayerManager : MonoBehaviour
{
    public CharacterManager CM;
    public float Player_speed = 1;
    private bool face_left=false;

    public int maxHp = 100;
    public int currentHp;
    public TMP_Text hpCount;
    public int hitCount=0;

    private int punchcombo = 0;
    private int kickcombo=0;
    private bool canMove;

    private bool canhit = true;
    private float hitStunTimer;
    public float hitStunMax;

    private Animator PlayerAnim;

    public Image playericon;
    public Sprite iconnormal;
    public Sprite iconhit;

    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        CM = GetComponent<CharacterManager>();
        PlayerAnim = GetComponent<Animator>();
        currentHp = maxHp;
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        playericon.sprite = iconnormal;
        //Player_speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        hitStunTimer--;
        if (hitStunTimer <= 0)
        {
            PlayerAnim.SetBool("GotHit",false);
            PlayerAnim.SetBool("KnockedDown",false);
            playericon.sprite = iconnormal;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) && canMove)
        {
            if (!CM.SR.flipX)
            {
                CM.SR.flipX = true;
                face_left = true;
            }
            transform.Translate(Time.deltaTime * Player_speed * Vector2.left);
        }
        if (Input.GetKey(KeyCode.RightArrow) && canMove)
        {
            if (CM.SR.flipX)
            {
                CM.SR.flipX = false;
                face_left = false;
            }
            transform.Translate(Time.deltaTime * Player_speed * Vector2.right);
        }
        if (Input.GetKey(KeyCode.UpArrow) && canMove)
        {
            transform.Translate(Time.deltaTime * Player_speed * Vector2.up);
        }
        if (Input.GetKey(KeyCode.DownArrow) && canMove)
        {
            transform.Translate(Time.deltaTime * Player_speed * Vector2.down);
        }

        if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.DownArrow))
            PlayerAnim.SetBool("Walking",true);
        else
        {
            PlayerAnim.SetBool("Walking",false);
        }
        //show attack
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCombo();
          //  CheckHitBoxAll();
            //CM.SR.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCombo();
           // CheckHitBoxAll();
            //CM.SR.color = Color.red;
        }
 
        
        
//        if (Input.GetKeyUp(KeyCode.Z))
//        {
//            CM.SR.color = Color.white;
//        }
    }

    private void StartCombo()
    {
        if (canhit && Input.GetKeyDown(KeyCode.Z))
        {
            punchcombo++;
            if (punchcombo == 1)
            {
                PlayerAnim.SetInteger("PunchCombo",punchcombo);
            }
        }
        if (canhit && Input.GetKeyDown(KeyCode.X))
        {
            kickcombo++;
            if (kickcombo == 1)
            {
                PlayerAnim.SetInteger("KickCombo",kickcombo);
            }
        }
    }
    private void CheckCombo()
    {
        canhit = false;
        
        if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_punch1") && punchcombo == 1)
        {
            PlayerAnim.SetInteger("PunchCombo",0);
            canhit = true;
            punchcombo = 0;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_punch1") && punchcombo == 2)
        {
            PlayerAnim.SetInteger("PunchCombo",2);
            canhit = true;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_punch2"))
        {
            PlayerAnim.SetInteger("PunchCombo",0);
            canhit = true;
            punchcombo = 0;
        }
        if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick1") && kickcombo == 1)
        {
            PlayerAnim.SetInteger("KickCombo",0);
            canhit = true;
            kickcombo = 0;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick1") && kickcombo == 2)
        {
            PlayerAnim.SetInteger("KickCombo",2);
            canhit = true;
        }
        
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick2"))
        {
            PlayerAnim.SetInteger("KickCombo",0);
            canhit = true;
            kickcombo = 0;
        }
    }
    

    private void CheckHitBoxAll(int type)//0 for punch, 1 for kick
    {
        RaycastHit2D[] boxResult;
        if (face_left)
            boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), new Vector2(1, 4), 0f, new Vector2(-1, 0), 1.5f, 1 << 8);
        else
            boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), new Vector2(1, 4), 0f, new Vector2(1, 0), 1.5f, 1 << 8);
        Debug.Log(boxResult.Length);
        if(boxResult.Length == 0)
        {
            if (type == 0)
            {
                AudioManager.instance.PlayClip("punchwhiff");
                Debug.Log("Punch Whiffed");
            }
            else if(type == 1)
            {
                AudioManager.instance.PlayClip("kickwhiff");
                Debug.Log("Kick Whiffed");
            }
        }
        for (int i = 0; i < boxResult.Length; i++){
            if (boxResult[i].collider != null)
            {
                bool tempPunchedBool;
                CharacterManager tmp = boxResult[i].collider.GetComponent<CharacterManager>();
                tmp.life--;
                if (type == 0)
                {
                    tempPunchedBool = true;
                    tmp.GetComponent<Enemy>().GetHit(GetComponent<AttackScript>(), tempPunchedBool);
                    Debug.Log(tempPunchedBool);
                }
                else if (type == 1)
                {
                    tempPunchedBool = false;
                    tmp.GetComponent<Enemy>().GetHit(GetComponent<AttackScript>(), tempPunchedBool);
                    Debug.Log(tempPunchedBool);
                }
                tmp.Checklife();
               
            }
        }

    }

    public void AffectMovement(int canMoveOrNo) //0 for yes, 1 for no
    {
        if (canMoveOrNo == 0)
        {
            canMove = true;
        }
        else if (canMoveOrNo == 1)
        {
            canMove = false;
        }
    }

    public void GetHit(AttackScript hitBy)
    {
        print("ow");
        Debug.Log(hitBy.name + " " + hitBy.damage);
        GetComponent<CharacterManager>().life -= hitBy.damage;
        currentHp-=hitBy.damage;
        hpCount.text = currentHp.ToString();
        playericon.sprite = iconhit;
        hitStunTimer = hitStunMax;
        PlayerAnim.SetBool("GotHit",true);
        AudioManager.instance.PlayClip("punched");
        if (hitCount < 3)
        {
            PlayerAnim.SetBool("GotHit", true);
            hitCount++;
        }
        else if (hitCount ==3)
        {
            PlayerAnim.SetBool("GotHit", false);
            PlayerAnim.SetBool("KnockedDown", true);
            hitCount = 0;
        }
    }

    private void CheckLife()
    {
        
    }
    
}
