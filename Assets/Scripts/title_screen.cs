using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class title_screen : MonoBehaviour
{
    [SerializeField] Button start;
    [SerializeField] Button credits;
    [SerializeField] Button exit;
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(start_game);
        credits.onClick.AddListener(view_credits);
        exit.onClick.AddListener(quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start_game() {
        SceneManager.LoadSceneAsync("Setup_Game", LoadSceneMode.Single);
    }

    private void view_credits() {

    }

    private void quit() {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}
