using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    //Important! Decide whether and when does the camera stop following player and the enemy starts instantiating
    //basically set up an empty object in the game
    //and once the player get past that object
    //this flag becomes true
    [SerializeField]
    private bool inCombat;

    // Start is called before the first frame update
    void Start()
    {
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
            CameraFollow();
        else
        {
            CombatCameraPositionFix();
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
    void CombatCameraPositionFix()
    {
        if (Camera.transform.position.y != 0)
        {
            Debug.Log("Fixing Camera!");
            Vector3 StartPosition = Camera.transform.localPosition;
            Vector3 EndPosition = new Vector3(Camera.transform.localPosition.x, 0f, Camera.transform.localPosition.z);
            //Camera.transform.position.Set(Camera.transform.position.x, 0f, Camera.transform.position.z);
            Camera.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, 1);
        }
    }

    void CheckInCombat()
    {
        //Do Something to InCombat
    }

    void PlayerPositionCheck()
    {
        if (player.transform.position.x >= Detector.transform.position.x)
        {
            inCombat = true;
        }
    }
}
