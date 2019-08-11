using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJump2 : MonoBehaviour
{
	Vector2 movement;
	Vector2 snapshot;
	enum Orientation {VERT, HORIZ};
	enum LaunchDirection {UP=1, DOWN=-1, LEFT=-2, RIGHT=2};
	Orientation orientation;
	LaunchDirection launchDirection;

	public float groundSpeed;
	public float airSpeed;
	public float riseSpeed;
	//public float launchHeight;

	public AnimationCurve slowdownCurve;
	public AnimationCurve speedupCurve;
	public float slowdownPercentage; //percentage of air speed to slow to
	public float speedupPercentage;
	public float slowdownTime; //how long are you allowed to slow down?
	//public float riseSpeed;

	bool slowdownActive = false;
	float slowdownProgress = 1f; //actual percentage of slowdown deceleration completed; 1=full speed, 0=full slow
	bool applyRise = false;
	float riseProgress = 1f; //only for running starts, not straight launches
	float slowdownTimer;

	Rigidbody2D rb;
	bool onSurface;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = Vector2.zero;
        slowdownTimer = slowdownTime;
        orientation = Orientation.HORIZ;
        launchDirection = LaunchDirection.UP;
        snapshot = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Z))
    	{
    		Debug.Log(slowdownActive);
    		Debug.Log(slowdownTimer);
    	}
        if(onSurface)
        {
        	//if VERT, LaunchDirection can only be LEFT or RIGHT
        	//if HORIZ, LaunchDirection can only be UP or DOWN
        	applyRise = false;
        	if(Input.GetKey(KeyCode.Space))
        	{
        		riseProgress = 0;
        		//these conditions basically invent a hypothetical launch platform of orthogonal orientation; yes, it's hacky
        		if(orientation == Orientation.HORIZ)
        		{
        			snapshot = new Vector2(0, Mathf.Clamp((int)launchDirection, -1, 1));
        			if(!(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
        			{
        				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        				{
        					applyRise = true;
        					orientation = Orientation.VERT;
        					if(Input.GetKey(KeyCode.LeftArrow)) snapshot.x = -1;
        					if(Input.GetKey(KeyCode.RightArrow)) snapshot.x = 1;
        				}
        			}
        		}
        		else
        		{
        			snapshot = new Vector2(Mathf.Clamp((int)launchDirection, -1, 1), 0);
        			if(!(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
        			{
        				if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        				{
        					applyRise = true;
        					orientation = Orientation.HORIZ;
        					if(Input.GetKey(KeyCode.UpArrow)) snapshot.y = 1;
        					if(Input.GetKey(KeyCode.DownArrow)) snapshot.y = -1;
        				}
        			}
        		}
        		onSurface = false;
        	}
        	else
        	{
        		if(orientation == Orientation.HORIZ)
        		{
        			//hacky, but I'm not gonna wade into vector addition bugs yet
        			if(!(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
        			{
        				movement = Vector2.zero;
	        			if(Input.GetKey(KeyCode.LeftArrow)) movement = Vector2.left * groundSpeed;
	        			if(Input.GetKey(KeyCode.RightArrow)) movement = Vector2.right * groundSpeed;
	        		}
        		}
        		else
        		{
        			if(!(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
        			{
        				movement = Vector2.zero;
	        			if(Input.GetKey(KeyCode.UpArrow)) movement = Vector2.up * groundSpeed;
	        			if(Input.GetKey(KeyCode.DownArrow)) movement = Vector2.down * groundSpeed;
	        		}
        		}
        	}
        }
        else //in air
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
    		if(riseProgress < 1f && applyRise)
    		{
    			rise = riseSpeed * Mathf.Clamp((int)launchDirection, -1, 1);
    			riseProgress += 0.1f;
    		}
    		if(orientation == Orientation.HORIZ)
    			rb.velocity = new Vector2(rise, snapshot.y * airSpeed);
    		else
    			rb.velocity = new Vector2(snapshot.x * airSpeed, rise);
    		if(slowdownActive && slowdownTimer > 0)
    		{
    			Debug.Log("not implemented yet");
    			//TODO
    			slowdownTimer -= 0.1f;
    		}
    	}
    }

    void OnCollisionEnter2D(Collision2D c)
    {
    	if(c.gameObject.tag == "ground")
    	{
    		onSurface = true;
    		riseProgress = 1f;
    		slowdownTimer = slowdownTime;
    		if((int)c.contacts[0].normal.x == 0)
    		{
    			orientation = Orientation.HORIZ;
    			if(c.contacts[0].normal.y > 0) launchDirection = LaunchDirection.UP;
    			else launchDirection = LaunchDirection.DOWN;
    		}
    		else if((int)c.contacts[0].normal.y == 0)
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
