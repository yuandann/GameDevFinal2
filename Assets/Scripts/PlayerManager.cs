using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
[RequireComponent(typeof(CharacterManager))]
public class PlayerManager : MonoBehaviour
{
    public CharacterManager CM;
    public float Player_speed = 1;
    private bool face_left=false;

    public AudioSource punchwhiff;
    public AudioSource kickwhiff;

    private int punchcombo = 0;
    private int kickcombo=0;

    private bool canhit = true;

    private Animator PlayerAnim;
    // Start is called before the first frame update
    void Start()
    {
        CM = GetComponent<CharacterManager>();
        PlayerAnim = GetComponent<Animator>();
        

        //Player_speed = 1;
    }

    // Update is called once per frame
    void Update()
    {


        //CheckHitbox();
        //Debug.Log(transform.position.x +"  "+ (CM.ma.position.x - CM.ma.rect.width / 2));
        //move
        if (!PlayerAnim.GetBool("InCombat"))
        {

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!CM.SR.flipX)
            {
                CM.SR.flipX = true;
                face_left = true;
            }
            transform.Translate(Time.deltaTime * Player_speed * Vector2.left);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (CM.SR.flipX)
            {
                CM.SR.flipX = false;
                face_left = false;
            }
            transform.Translate(Time.deltaTime * Player_speed * Vector2.right);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Time.deltaTime * Player_speed * Vector2.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
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
            //CM.SR.color = Color.red;
            CheckHitBoxAll();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCombo();
            //CM.SR.color = Color.red;
            CheckHitBoxAll();
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
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_punch1") && punchcombo >= 2)
        {
            PlayerAnim.SetInteger("PunchCombo",2);
            canhit = true;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_punch2") && punchcombo > 2)
        {
            PlayerAnim.SetInteger("PunchCombo",1);
            canhit = true;
            punchcombo = 1;
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
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick1") && kickcombo >= 2)
        {
            PlayerAnim.SetInteger("KickCombo",2);
            canhit = true;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick2") && kickcombo > 2)
        {
            PlayerAnim.SetInteger("KickCombo",1);
            canhit = true;
            kickcombo = 1;
        }
        else if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("player_kick2"))
        {
            PlayerAnim.SetInteger("KickCombo",0);
            canhit = true;
            kickcombo = 0;
        }
    }
    

    private void CheckHitBoxAll()
    {
        RaycastHit2D[] boxResult;
        if (face_left)
            boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), new Vector2(1, 4), 0f, new Vector2(-1, 0), 1.5f, 1 << 8);
        else
            boxResult = Physics2D.BoxCastAll(gameObject.transform.position + new Vector3(0, 3f), new Vector2(1, 4), 0f, new Vector2(1, 0), 1.5f, 1 << 8);
        if(boxResult.Length == 0)
        {
                if (Input.GetKeyDown(KeyCode.Z))
                    punchwhiff.Play();
                else if (Input.GetKeyDown(KeyCode.X))
                    kickwhiff.Play(); 
        }

        if (boxResult != null)
        {

            //Debug.Log(boxResult.collider.name);
            //if (boxResult.collider.CompareTag("Enemy"))
            {
                //CharacterManager[] tmp = boxResult.collider.GetComponents<CharacterManager>();
                for (int i = 0; i < boxResult.Length; i++)
                {
                    CharacterManager tmp = boxResult[i].collider.GetComponent<CharacterManager>();
                    tmp.life--;
                    tmp.GetComponent<Enemy>().GetHit(this.GetComponent<AttackScript>());
                    tmp.Checklife();
                }
            }
        }

    }
}
