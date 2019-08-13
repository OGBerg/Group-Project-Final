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
            mySprite.flipX = false; 
            resetTime = 3f;
            

        }
        //else
        //{
        //    mySprite.flipX = false;
        //}
    }
}
