using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int life;
    public int Type;
    public SpriteRenderer SR;
    public Transform shadow;
    public GameObject hitfx;
    private Animator thisanim;
    
    //[SerializeField]
    //public GameObject move_area;
    //public RectTransform ma;
    //type 1 for player
    //type 2 for enemy

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        shadow = transform.Find("shadow").GetComponent<Transform>();
        thisanim = GetComponent<Animator>();

        //ma = move_area.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var shadowRotation = shadow.localRotation;
        var shadowPosition = shadow.localPosition;
        if (SR.flipX)
        {
            shadowRotation.y = 180;
;            shadowPosition.x *= -1;
            shadow.localRotation = shadowRotation;
            shadow.localPosition = shadowPosition;
        }
        else
        {
            shadow.localRotation = shadowRotation;
            shadow.localPosition = shadowPosition;
        }
    }
    public void Checklife()
    {
        print(life);
        Debug.Log(this.name+" Life: "+ life);
    }
    
}
