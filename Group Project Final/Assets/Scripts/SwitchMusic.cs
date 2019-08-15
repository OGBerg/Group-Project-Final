using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
	public AudioClip sound;
	GameObject music;
	AudioSource audio;

    void Awake()
    {
    	music = GameObject.FindWithTag("music");
    	audio = music.GetComponent<AudioSource>();
    	float tmp = audio.time;
    	audio.clip = sound;
    	audio.time = tmp;
    	audio.Play();
    }
}
