using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class title_screen : MonoBehaviour
{
    [SerializeField] Button start;
    [SerializeField] Button about;
    [SerializeField] Button credits;
    [SerializeField] Button exit;
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(start_game);
        about.onClick.AddListener(show_about);
        credits.onClick.AddListener(view_credits);
        exit.onClick.AddListener(quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start_game() {
        Initiate.Fade("Setup_Game", Color.black, 1f);
    }

    private void show_about() {
        Initiate.Fade("AboutGame", Color.black, 1f);
    }

    private void view_credits() {

    }

    private void quit() {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}
