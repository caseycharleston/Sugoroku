using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static PlayerInfo[] rankings;
    public static string winner;
    [SerializeField] TextMeshProUGUI text_box;
    [SerializeField] Button exit;

    // Start is called before the first frame update
    void Start() {
        text_box.GetComponent<TMP_Text>().text = winner;
        exit.onClick.AddListener(title_screen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void title_screen() {
        // SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);
        Initiate.Fade("TitleScreen", Color.black, 1f);
    }
}
