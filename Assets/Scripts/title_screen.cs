using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class title_screen : MonoBehaviour
{
    [SerializeField] Button start;
    [SerializeField] Button howToPlay;
    [SerializeField] Button credits;
    // [SerializeField] Button exit;
    public AudioSource sfx;
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(start_game);
        howToPlay.onClick.AddListener(howtoplay);
        credits.onClick.AddListener(view_credits);
        // exit.onClick.AddListener(quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start_game() {
        sfx.Play();
        Initiate.Fade("Setup_Game", Color.black, 1f);
    }

    private void howtoplay() {
        sfx.Play();
        HowToPlayScript.title_screen = true;
        Initiate.Fade("HowToPlayGame", Color.black, 1f);
    }

    private void view_credits() {
        sfx.Play();
        Initiate.Fade("Credits", Color.black, 1f);
    }

}
