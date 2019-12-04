using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int life;
    public int Type;
    public SpriteRenderer SR;

    public AudioSource punch;
    public AudioSource kick;
    public GameObject hitfx;
    
    //[SerializeField]
    //public GameObject move_area;
    //public RectTransform ma;
    //type 1 for player
    //type 2 for enemy

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        //ma = move_area.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Checklife()
    {
        Debug.Log(this.name+" Life: "+ life);
        if (life <= 0)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
