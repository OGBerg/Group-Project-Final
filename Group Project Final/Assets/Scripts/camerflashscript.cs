using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerflashscript : MonoBehaviour
{

    SpriteRenderer spriterenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriterenderer.enabled |= collision.gameObject.tag == "Player";
    }
}
