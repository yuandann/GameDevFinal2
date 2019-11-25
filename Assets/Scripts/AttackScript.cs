using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public enum Weight
        {
            Light,
            Medium,
            Heavy
        }
    
        public Weight myWeight;
        
        public int startupTime, activeTime, endlagTime; //num of frames
    
        public float damage, horizontalKB;
    
        public BoxCollider2D hitBox;
    
        public float horizontalRange, verticalRange;
    
        public bool playerAttack; //set to true if this is an attack of the player's
        
        public bool hitYet; //set to true on hit, so you don't hit the player multiple times
    
        public Vector2 attackRange;
    
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (playerAttack)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    hitYet = true;
                    other.gameObject.GetComponent<Enemy>().GetHit(this);
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    hitYet = true;
                }
            }
        }
}
