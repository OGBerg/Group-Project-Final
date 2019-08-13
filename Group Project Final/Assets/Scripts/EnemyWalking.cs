using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalking : MonoBehaviour
{
    SpriteRenderer mySprite;

    public float resetTime = 3f; 

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        resetTime -= Time.deltaTime;
        if (resetTime <= 0)
        {
            //mySprite.flipX = !mySprite.flipX; 
            //resetTime = 3f;

            Vector3 flip = transform.localScale;
            flip.x *= -1;
            transform.localScale = flip;

            resetTime = 3f;
        }
       
    }
}
