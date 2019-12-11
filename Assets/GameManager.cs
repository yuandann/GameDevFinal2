using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    private GameObject upper_scene;
    [SerializeField]
    private GameObject lower_scene;
    [SerializeField]
    private GameObject player;
    public GameObject Camera;
    public Transform cameraParent;
    private Vector3 offset;

    public int enemiesdefeated;
    private int instantiatecount;

    public GameObject Instantiator1;
    public GameObject Instantiator2;
    public GameObject Detector;
    public Animator gameoverpanel;

    public GameObject[] EnemyList;

    //Important! Decide whether and when does the camera stop following player and the enemy starts instantiating
    //basically set up an empty object in the game
    //and once the player get past that object
    //this flag becomes true
    [SerializeField]
    private bool inCombat;

    public float Lerping = 0;
    private bool Start_Making_Enemy = false;
    private bool recalibrate_camera = false;

    private float shaketimer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        offset = cameraParent.position - player.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        shaketimer--;
        if (shaketimer <= 0)
        {
            Camera.GetComponent<Animator>().SetBool("shaking",false);
        }
    
        //if(inCombat)
        //lockCamera();
        //EnemyInstantiation();
        //else()
        if (!inCombat)
        {
            if (recalibrate_camera)
            {
                if (Lerping <= 1)
                {
                    Lerping += 0.05f;
                    StartCoroutine(CameraPositionRecalibration());
                }
                else
                {
                    StopCoroutine(CameraPositionRecalibration());
                    recalibrate_camera = false;
                    Lerping = 0;
                }
            }
            else
                CameraFollow();
        }
        else
        {
            if (!Start_Making_Enemy)
            {
                Start_Making_Enemy = true;
                InstantiateEnemy();
            }
            if (Lerping <= 1)
            {
                Lerping += 0.05f;
            }
            if (Lerping <= 1)
            {
                StartCoroutine(CombatCameraPositionFix());
            }
            else
            {
                StopCoroutine(CombatCameraPositionFix());
                CheckInCombat();
            }
            
        }

        //if(inCombat)
        //Instantiator1.instantiateEnemy...
        //...stuffs...

        PlayerPositionCheck();
    }
    void CameraFollow()
    {
        cameraParent.position = player.transform.position + offset;
    }

    //this will later change to ienumerator for smooth animation
    IEnumerator CombatCameraPositionFix()
    {
        Vector3 StartPosition = cameraParent.localPosition;
        Vector3 EndPosition = new Vector3(cameraParent.localPosition.x, 0f, -10);
        //while (Camera.transform.position.y <= 0)
        while (Lerping <= 1)
        {
            Debug.Log("Fixing Camera!");
            //Camera.transform.position.Set(Camera.transform.position.x, 0f, Camera.transform.position.z);
            cameraParent.localPosition = Vector3.Lerp(StartPosition, EndPosition, Lerping);
            yield return null;
        }
    }

    public void ScreenShake()
    {
        Camera.GetComponent<Animator>().SetBool("shaking",true);
        print("shaking");
        shaketimer = 10;
    }
    

    IEnumerator CameraPositionRecalibration()
    {
        Vector3 StartPosition = cameraParent.localPosition;
        //Vector3 EndPosition = new Vector3(Camera.transform.localPosition.x, 0f, -10);
        Vector3 EndPosition = player.transform.position + offset;
        //while (Camera.transform.position.y <= 0)
        while (Lerping <= 1)
        {
            Debug.Log("Fixing Camera!");
            //Camera.transform.position.Set(Camera.transform.position.x, 0f, Camera.transform.position.z);
            cameraParent.localPosition = Vector3.Lerp(StartPosition, EndPosition, Lerping);
            yield return null;
        }
    }

    void InstantiateEnemy()
    {
        if(instantiatecount<3)
        {
            instantiatecount++;
            Instantiator1.GetComponent<InstantiatorManager>().MakeEnemy();
        }
    }

    void CheckInCombat()
    {
        //Do Something to InCombat
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (EnemyList.Length == 0)
        {
            inCombat = false;
            Start_Making_Enemy = false;
            Lerping = 0;
            Detector.transform.position += new Vector3(10f,0f);
            recalibrate_camera = true;
        }
    }

    void PlayerPositionCheck()
    {
        if (player.transform.position.x >= Detector.transform.position.x)
        {
            inCombat = true;
        }
    }

    public void GameOver()
    {
        gameoverpanel.SetTrigger("fadein");
    }

    public void CheckWin()
    {
        
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
