using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmenuscroll : MonoBehaviour
{

    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.x += scrollSpeed;
        transform.position = pos;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "SpecialWall")
        {
            Debug.Log("hi");
            var pos = transform.position;
            pos.x = -5.7f;
            pos.y = 1.053613f;
            transform.position = pos;



        }
    }
}
