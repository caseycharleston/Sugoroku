using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    [SerializeField] Button resume;
    [SerializeField] Button exit;

    // Start is called before the first frame update
    void Start()
    {
        resume.onClick.AddListener(resume_game);
        exit.onClick.AddListener(exit_game);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void resume_game() {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        main.set_move(true);
        main.set_pause(false);
    } 

    private void exit_game() {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
