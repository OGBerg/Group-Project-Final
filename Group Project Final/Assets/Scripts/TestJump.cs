using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJump : MonoBehaviour
{
	public float speed;
	public float airSpeed;
	public float launchSpeed;

	public AnimationCurve slowdownCurve;
	public float slowdownMagnitude; //keep at 1?
	public AnimationCurve speedupCurve;
	public float speedupMagnitude; //keep at 1?
	//x += (target - x) * 0.3f;

	Vector2 movement;
	Vector2 launchDirection;

	Rigidbody2D rb;
	bool onGround = true;
	bool isLaunching = false;
	bool slowdown = false;
	bool speedup = false;
	bool impact = false;

	float slowdownStartTime;
	public float slowdownMaxDuration;
	float speedupStartTime;
	public float speedupMaxDuration;

	GameObject rotator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotator = GameObject.Find("rotator");
        movement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(onGround)
        {
        	launchDirection = Vector2.zero;
        	if(Input.GetKey(KeyCode.LeftArrow)) launchDirection += Vector2.left;
        	if(Input.GetKey(KeyCode.RightArrow)) launchDirection += Vector2.right;
        	movement = launchDirection;
        	if(Input.GetKey(KeyCode.Space))
        	{
        		isLaunching = true;
        		// onGround = false;
        	}
        }
        else
        {
        	if(Input.GetKeyDown(KeyCode.Space))
        	{
        		speedup = false;
        		slowdown = true;
        		slowdownStartTime = Time.time;
        	}
        	if(Input.GetKeyUp(KeyCode.Space))
        	{
        		slowdown = false;
        		speedup = true;
        		speedupStartTime = Time.time;
        	}
        }
    }

    void FixedUpdate()
    {
    	if(isLaunching)
    	{
    		rb.AddForce(Vector2.up * launchSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    		isLaunching = false;
    	}

    	if(onGround)
    	{
    		slowdown = false;
    		speedup = false;
    		if(impact)
    		{
    			rb.velocity = Vector2.zero; //add curve?
    			//play anim here
    			impact = false;
    		}
     		// Debug.Log("move v: " + (movement * speed * Time.fixedDeltaTime));
    		rb.AddForce(movement * speed * Time.fixedDeltaTime);
    	}
    	else
    	{
    		rb.velocity = new Vector2(launchDirection.x * airSpeed, rb.velocity.y);
    		if(rb.velocity.y < 0)
    		{
    			Vector2 velocity = rb.velocity;
    			velocity.y = 0;
    			rb.velocity = velocity;
    			rb.gravityScale = 0;
    		}
    		impact = true;
    		if(slowdown)
    		{
    			Vector2 velocity = rb.velocity;
    			velocity.x -= slowdownCurve.Evaluate(Mathf.Clamp01((Time.time - slowdownStartTime) / slowdownMaxDuration)) * slowdownMagnitude;
    			rb.velocity = velocity;
    		}
    		if(speedup)
    		{
    			Vector2 velocity = rb.velocity;
    			velocity.x += speedupCurve.Evaluate(Mathf.Clamp01((Time.time - speedupStartTime) / speedupMaxDuration)) * speedupMagnitude;
    			rb.velocity = velocity;
    		}
    	}
    }

    void OnCollisionEnter2D(Collision2D c)
    {
    	if(c.gameObject.tag == "ground") onGround = true;
    	Debug.Log(c.contacts[0].normal);
    	rotator.transform.eulerAngles =
    		new Vector3(0,0,Mathf.Floor(rotator.transform.eulerAngles.z + c.contacts[0].normal.x * 90));
    }

    void OnCollisionExit2D(Collision2D c)
    {
    	if(c.gameObject.tag == "ground") onGround = false;
    }
}
