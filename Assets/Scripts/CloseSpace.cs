using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CloseSpace : MonoBehaviour
{
    bool escaped;
    private AudioSource button_sfx;
    // Start is called before the first frame update
    void Start()
    {
        button_sfx = GameObject.Find("PageFlipSFX").GetComponent<AudioSource>();
        escaped = false;   
    }

    private void OnMouseDown() {
        button_sfx.Play();
        GameControl.brain.m_DefaultBlend.m_Time = GameControl.zoom_speed; // 0 Time equals a cut
        GameControl.setup_next = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
    }



    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape") && !escaped) {
            escaped = true;
            button_sfx.Play();
            GameControl.brain.m_DefaultBlend.m_Time = GameControl.zoom_speed; // 0 Time equals a cut
            GameControl.setup_next = true;
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
    }
}
