using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {
    	if(Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(0);
    }
}
