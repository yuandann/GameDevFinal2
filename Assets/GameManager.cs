using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

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
    private Vector3 offset;

    public GameObject Instantiator1;
    public GameObject Instantiator2;
    public GameObject Detector;

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

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        offset = Camera.transform.position - player.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
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
        Camera.transform.position = player.transform.position + offset;
    }

    //this will later change to ienumerator for smooth animation
    IEnumerator CombatCameraPositionFix()
    {
        Vector3 StartPosition = Camera.transform.localPosition;
        Vector3 EndPosition = new Vector3(Camera.transform.localPosition.x, 0f, -10);
        //while (Camera.transform.position.y <= 0)
        while (Lerping <= 1)
        {
            Debug.Log("Fixing Camera!");
            //Camera.transform.position.Set(Camera.transform.position.x, 0f, Camera.transform.position.z);
            Camera.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, Lerping);
            yield return null;
        }
    }

    public IEnumerator ScreenShake()
    {
        Vector3 originalPos = Camera.transform.localPosition;
        float shaketime = 10f;
        if (shaketime > 0f)
        {
            shaketime -= Time.deltaTime;
            Camera.transform.localPosition = originalPos + Random.insideUnitSphere * 0.7f;
        }
        else
        {
            shaketime = 0f;
            Camera.transform.localPosition = originalPos;
            yield return null;
        }

    }

    IEnumerator CameraPositionRecalibration()
    {
        Vector3 StartPosition = Camera.transform.localPosition;
        //Vector3 EndPosition = new Vector3(Camera.transform.localPosition.x, 0f, -10);
        Vector3 EndPosition = player.transform.position + offset;
        //while (Camera.transform.position.y <= 0)
        while (Lerping <= 1)
        {
            Debug.Log("Fixing Camera!");
            //Camera.transform.position.Set(Camera.transform.position.x, 0f, Camera.transform.position.z);
            Camera.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, Lerping);
            yield return null;
        }
    }

    void InstantiateEnemy()
    {
        Instantiator1.GetComponent<InstantiatorManager>().MakeEnemy();
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
}
