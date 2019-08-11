using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJump2 : MonoBehaviour
{
	Vector2 movement;
	enum Orientation {VERT, HORIZ};
	enum LaunchDirection {UP=1, DOWN=-1, LEFT=1, RIGHT=-1};
	Orientation orientation = Orientation.HORIZ;
	LaunchDirection launchDirection = LaunchDirection.UP;

	public float groundSpeed;
	public float airSpeed;
	public float launchHeight;

	public AnimationCurve slowdownCurve;
	public AnimationCurve speedupCurve;
	public float slowdownPercentage; //percentage of air speed to slow to
	public float speedupPercentage;
	public float slowdownTime; //how long are you allowed to slow down?
	public float riseSpeed;

	bool slowdownActive = false;
	float slowdownProgress = 1f; //actual percentage of slowdown deceleration completed; 1=full speed, 0=full slow
	float riseProgress = 1f; //only for running starts, not straight launches
	float slowdownTimer = slowdownTime;

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
        	//if VERT, LaunchDirection can only be LEFT or RIGHT
        	//if HORIZ, LaunchDirection can only be UP or DOWN
        	if(Input.GetKey(KeyCode.Space))
        	{
        		onSurface = false;
        		riseProgress = 0;
        		//these conditions basically invent a hypothetical launch platform of orthogonal orientation; yes, it's hacky
        		if(orientation == Orientation.HORIZ)
        		{
        			if(!(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(Input.RightArrow)))
        			{
        				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        					orientation = Orientation.VERT;
        			}
        		}
        		else
        		{
        			if(!(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
        			{
        				if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        					orientation = Orientation.HORIZ;
        			}
        		}
        	}
        	else
        	{
        		if(orientation == Orientation.HORIZ)
        		{
        			//hacky, but I'm not gonna wade into vector addition bugs yet
        			if(!(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
        			{
	        			if(Input.GetKey(KeyCode.LeftArrow)) movement = Vector2.left;
	        			if(Input.GetKey(KeyCode.RightArrow)) movement = Vector2.right;
	        		}
        		}
        		else
        		{
        			if(!(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
        			{
	        			if(Input.GetKey(KeyCode.UpArrow)) movement = Vector2.up;
	        			if(Input.GetKey(KeyCode.DownArrow)) movement = Vector2.down;
	        		}
        		}
        	}
        }
        else
        {
        	if(Input.GetKeyDown(KeyCode.Space)) slowdownActive = true;
        	if(Input.GetKeyUp(KeyCode.Space)) slowdownActive = false;
        }
    }

    void FixedUpdate()
    {
    	if(onSurface) rb.velocity = movement; //look, Ma, no impulses!
    	else
    	{
    		float rise = 0;
    		slowdownTimer = slowdownTime;
    		if(riseProgress < 1f)
    		{
    			rise = riseSpeed * launchDirection;
    			riseProgress += 0.1;
    		}
    		if(orientation == Orientation.HORIZ)
    			rb.velocity = new Vector2(rise, (int)launchDirection * airSpeed);
    		else
    			rb.velocity = new Vector2((int)launchDirection * airSpeed, rise);
    		if(slowdownActive && slowdownTimer > 0)
    		{
    			//slowdown stuff here
    			slowdownTimer -= 0.1;
    		}
    	}
    }

    void OnCollisionEnter2D(Collision2D c)
    {
    	if(c.gameObject.tag == "ground")
    	{
    		onSurface = true;
    		riseProgress = 1;
    		slowdownTimer = slowdownTime;
    		if(c.contacts[0].normal.x == 0)
    		{
    			orientation = Orientation.HORIZ;
    			if(c.contacts[0].normal.y > 0) launchDirection = LaunchDirection.UP;
    			else launchDirection = LaunchDirection.DOWN;
    		}
    		else if(c.contacts[0].normal.y == 0)
    		{
    			orientation = Orientation.VERT;
    			if(c.contacts[0].normal.x < 0) launchDirection = LaunchDirection.LEFT;
    			else launchDirection = LaunchDirection.RIGHT;
    		}
    		else Debug.Log("oops");
    		//TODO: diagonal platforms?
    		// would need to change launchDirection from an enum to a float, maybe as a stretch goal
    	}
    }

    void OnCollisionExit2D(Collision2D c)
    {
    	onSurface = false;
    }
}
