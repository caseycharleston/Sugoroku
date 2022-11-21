using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CloseSpace : MonoBehaviour
{
    bool escaped;
    // Start is called before the first frame update
    void Start()
    {
        escaped = false;
        
    }

    private void OnMouseDown() {
        Debug.Log("Clicked On!");
        GameControl.brain.m_DefaultBlend.m_Time = 3; // 0 Time equals a cut
        GameControl.setup_next = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
    }



    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape") && !escaped) {
            escaped = true;
            GameControl.brain.m_DefaultBlend.m_Time = 3; // 0 Time equals a cut
            GameControl.setup_next = true;
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
    }
}
