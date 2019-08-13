using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJump : MonoBehaviour
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
	//public float speedupPercentage;
	public float slowdownTime; //how long are you allowed to slow down?
	public float slowdownEnvelope; //How much of the slowdown to spend on the easing curves?
	//public float riseSpeed;

	bool slowdownActive = false;
	float slowdownProgress = 0; //actual percentage of slowdown deceleration completed; 0=full speed, 1=full slow
	bool applyRise = false;
	float riseProgress = 1f; //only for running starts, not straight launches
	float slowdownTimer;
	float slowdownDelta;

	Rigidbody2D rb;
    AudioSource audio;
    SpriteRenderer sr;
	bool onSurface;
    bool delay = false;

    public GameObject background;
    public float backgroundOffset;
    public AudioClip jump;
    public AudioClip impact;

    public GameObject powerupSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        movement = Vector2.zero;
        slowdownTimer = slowdownTime;
        slowdownDelta = slowdownTime * slowdownEnvelope;
        orientation = Orientation.HORIZ;
        launchDirection = LaunchDirection.UP;
        snapshot = new Vector2(0,0);
        onSurface = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            Debug.Log(orientation);
            Debug.Log(launchDirection);
            Debug.Log("on ground: " + onSurface);
        }
        background.transform.position = new Vector2(
            background.transform.position.x - (rb.velocity.x * backgroundOffset),
            background.transform.position.y - (rb.velocity.y * backgroundOffset));
        if(onSurface)
        {
        	//if VERT, LaunchDirection can only be LEFT or RIGHT
        	//if HORIZ, LaunchDirection can only be UP or DOWN
        	applyRise = false;
        	if(Input.GetKey(KeyCode.Space))
        	{
                audio.clip = jump;
                audio.Play();
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
	        			if(Input.GetKey(KeyCode.LeftArrow))
                        {
                            movement = Vector2.left * groundSpeed;
                            sr.flipX = true;
                        }
	        			if(Input.GetKey(KeyCode.RightArrow))
                        {
                            movement = Vector2.right * groundSpeed;
                            sr.flipX = false;
                        }
	        		}
        		}
        		else
        		{
        			if(!(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
        			{
        				movement = Vector2.zero;
	        			if(Input.GetKey(KeyCode.UpArrow))
                        {
                            movement = Vector2.up * groundSpeed;
                            sr.flipY = false;
                        }
	        			if(Input.GetKey(KeyCode.DownArrow))
                        {
                            movement = Vector2.down * groundSpeed;
                            sr.flipY = true;
                        }
	        		}
        		}
        	}
        }
        else //in air
        {
            if(!delay)
            {
            	if(Input.GetKeyDown(KeyCode.Space)) slowdownActive = true;
            	if(Input.GetKeyUp(KeyCode.Space)) slowdownActive = false;
            }
        }
    }

    void FixedUpdate()
    {
    	if(onSurface) rb.velocity = movement; //look, Ma, no impulses!
    	else
    	{
    		float rise = 0;
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
                //TODO BREAKING BUG
                //exhausted slowdown timer breaks movement
    			Vector2 slowVelocity = rb.velocity * slowdownPercentage / 100;
    			//temp non-animation fix
    			rb.velocity = slowVelocity;
                slowdownTimer -= 0.1f;
    		}
    	}
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        // Debug.Log(c.gameObject.tag);
        if(c.gameObject.tag == "ground")
        {
            audio.clip = impact;
            audio.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "pickup")
        {
            powerupSound.GetComponent<AudioSource>().Play();
        }
    }

    void OnCollisionStay2D(Collision2D c)
    {
    	if(c.gameObject.tag == "ground")
    	{
            //delay = true;
    		riseProgress = 1f;
    		slowdownTimer = slowdownTime;
    		slowdownDelta = slowdownTime * slowdownEnvelope;
    		slowdownProgress = 0;
    		if(Mathf.RoundToInt(c.contacts[c.contacts.Length-1].normal.x) == 0)
    		{
    			orientation = Orientation.HORIZ;
    			if(c.contacts[c.contacts.Length-1].normal.y > 0)
                {
                    sr.flipY = false;
                    launchDirection = LaunchDirection.UP;
                }
    			else
                {
                    sr.flipY = true;
                    launchDirection = LaunchDirection.DOWN;
                }
    		}
    		else if(Mathf.RoundToInt(c.contacts[c.contacts.Length-1].normal.y) == 0)
    		{
    			orientation = Orientation.VERT;
    			if(c.contacts[c.contacts.Length-1].normal.x < 0)
                {
                    sr.flipX = true;
                    launchDirection = LaunchDirection.LEFT;
                }
    			else
                {
                    sr.flipX = false;
                    launchDirection = LaunchDirection.RIGHT;
                }
    		}
    		else Debug.Log("oops");
            onSurface = true;
            //Debug.Log("landing: " + onSurface);
            //delay = false;
    		//TODO: diagonal platforms?
    		// would need to change launchDirection from an enum to a float, maybe as a stretch goal
    	}
    }

    void OnCollisionExit2D(Collision2D c)
    {
    	onSurface = false;
        // Debug.Log("exiting: " + onSurface);
    }
}
