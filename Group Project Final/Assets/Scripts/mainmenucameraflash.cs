using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmenucameraflash : MonoBehaviour
{


    SpriteRenderer spriterenderer;

    // this is the UI.Text or other UI element you want to toggle


    public float interval = 1f;
    public float startDelay = 0.5f;
    public bool currentState = true;
    public bool defaultState = true;
    bool isBlinking = false;


    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        spriterenderer.enabled = defaultState;
        StartBlink();
    }

    public void StartBlink()
    {
        // do not invoke the blink twice - needed if you need to start the blink from an external object
        if (isBlinking)
            return;

        if (spriterenderer != null)
        {
            isBlinking = true;
            InvokeRepeating("ToggleState", startDelay, interval);
        }
    }

    public void ToggleState()
    {
        spriterenderer.enabled = !spriterenderer.enabled;

       
    }

}


