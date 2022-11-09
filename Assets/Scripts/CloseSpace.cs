using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CloseSpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown() {
        Debug.Log("Clicked On!");
        GameControl.setup_next = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
