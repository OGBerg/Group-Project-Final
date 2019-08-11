using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJump2 : MonoBehaviour
{
	Vector2 movement;
	enum Orientation {VERT, HORIZ};
	Orientation orientation = Orientation.HORIZ;

	public float groundSpeed;
	public float airSpeed;
	public float launchHeight;

	public AnimationCurve slowdownCurve;
	public AnimationCurve speedupCurve;
	public float slowdownPercentage;
	public float speedupPercentage;

	Rigidbody2D rb;
	bool onSurface;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(onSurface)
        {

        }
        else
        {

        }
    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D c)
    {
    	onSurface = true;
    	//check if ground tag, duh
    	//c.contacts[0].normal should give you all the info about orientation you need
    }

    void OnCollisionExit2D(Collision2D c)
    {
    	onSurface = false;
    }
}
