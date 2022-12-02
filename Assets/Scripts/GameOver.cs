using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameOver : MonoBehaviour
{
    //3rd place: 7 2nd place:9 1st place:13
    public static PlayerInfo[] rankings;
    public static string winner;
    [SerializeField] TextMeshProUGUI[] names;
    [SerializeField] GameObject main_camera, static_camera;
    public AudioSource[] dumb_sfx;
    public Transform[] waypoints;
    public int[] orthosizes = {13, 9, 7};
    int length;

    // Start is called before the first frame update
    void Start() {
        // text_box.GetComponent<TMP_Text>().text = winner;
        int length = rankings.Length;
        if (rankings.Length == 4) {
            length = 3;
        }
        for (int i = length - 1; i >= 0; i--) {
            names[i].GetComponent<TMP_Text>().text = rankings[i].player_name;
        }
        dumb_sfx[0].Play();
        Invoke("title_screen", 10f);
        // StartCoroutine("finishthis");
    }

    IEnumerator finishthis() {
        for (int i = length - 1; i >= 0; i--) {
            main_camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = orthosizes[i];
            main_camera.GetComponent<CinemachineVirtualCamera>().Follow = waypoints[i];
            main_camera.GetComponent<CinemachineVirtualCamera>().LookAt = waypoints[i];
            yield return new WaitForSeconds(1f);
            dumb_sfx[i].Play();
            yield return new WaitForSeconds(3f);
            if (i == 0) {
                static_camera.SetActive(true);
                yield return new WaitForSeconds(3f);
            } 
            dumb_sfx[i].Stop();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void title_screen() {
        Debug.Log("HELLO");
        // SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);
        Initiate.Fade("TitleScreen", Color.black, 1f);
    }
}
