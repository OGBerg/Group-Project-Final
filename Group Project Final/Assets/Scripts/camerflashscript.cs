using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class camerflashscript : MonoBehaviour
{

    SpriteRenderer spriterenderer;
    public float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        reloadTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriterenderer.enabled = true;
            ReloadCurrentScene();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriterenderer.enabled &= collision.gameObject.tag != "Player";
    }

    public void ReloadCurrentScene()
    {
       
        string sceneName = SceneManager.GetActiveScene().name;

        
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

