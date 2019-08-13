using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
	public float offset;
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)) transform.position = new Vector2(transform.position.x, transform.position.y - offset);
        if(Input.GetKey(KeyCode.DownArrow)) transform.position = new Vector2(transform.position.x, transform.position.y + offset);
        if(Input.GetKey(KeyCode.LeftArrow)) transform.position = new Vector2(transform.position.x + offset, transform.position.y);
        if(Input.GetKey(KeyCode.RightArrow)) transform.position = new Vector2(transform.position.x - offset, transform.position.y);
    }
}
