using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SetupGame : MonoBehaviour
{
    EventSystem system;
    [SerializeField] Button one_player;
    [SerializeField] Button two_player;
    [SerializeField] Button three_player;
    [SerializeField] Button four_player;
    [SerializeField] GameObject setup_num_players;
    [SerializeField] GameObject setup_player_name_screen;
    [SerializeField] GameObject[] setup_player_cols;
    [SerializeField] GameObject first_setup_player_input;
    [SerializeField] TextMeshProUGUI[] setup_player_names;
    [SerializeField] Button go_to_order;
    [SerializeField] GameObject setup_player_order;
    [SerializeField] GameObject[] show_player_order;
    [SerializeField] TextMeshProUGUI[] show_name_order;
    [SerializeField] Button start_round;
    public AudioSource button_sfx;

    int num_players = 0;
    int tab_index;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        one_player.onClick.AddListener(delegate{set_num_players(1);});
        two_player.onClick.AddListener(delegate{set_num_players(2);});
        three_player.onClick.AddListener(delegate{set_num_players(3);});
        four_player.onClick.AddListener(delegate{set_num_players(4);}); 
        go_to_order.onClick.AddListener(randomize_order);
        start_round.onClick.AddListener(start);       
        tab_index = 0;
    }

    void set_num_players(int players) {
        button_sfx.Play();
        num_players = players;
        GameControl.num_players = players;
        setup_num_players.SetActive(false);
         for (int i = 0; i < players; i++) {
            setup_player_cols[i].SetActive(true);
        }
        setup_player_name_screen.SetActive(true);
        system.SetSelectedGameObject(first_setup_player_input, new BaseEventData(system));
    }

    void randomize_order() {
        button_sfx.Play();
        setup_player_name_screen.SetActive(false);
        List<int> used = new List<int>();
        while (used.Count < num_players) {
             int index = Random.Range(0, num_players);
             if (!used.Contains(index)) {
                used.Add(index);
             }
        }
        string[] names = new string[num_players];
        for (int i = 0; i < num_players; i++) {
            string name = setup_player_names[used[i]].GetComponent<TMP_Text>().text;
            names[i] = name;
            Debug.Log(name);
            show_name_order[i].GetComponent<TMP_Text>().text = name;
            show_player_order[i].SetActive(true);
        }
        GameControl.names = names;
        setup_player_order.SetActive(true);
    }

    void start() {
        button_sfx.Play();
        // SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        Initiate.Fade("MainGame", Color.black, 2f);
    }



    // Update is called once per frame
    void Update() {
         if (Input.GetKeyDown(KeyCode.Return)) {
            if (setup_player_name_screen.activeInHierarchy) {
                randomize_order();
            } else if (setup_player_order.activeInHierarchy) {
                start();
            }
        }
        
        //lmao jank
        if (Input.GetKeyDown("1")) {
            if (setup_num_players.activeInHierarchy) {
                set_num_players(1);
            }
        }
        if (Input.GetKeyDown("2")) {
            if (setup_num_players.activeInHierarchy) {
                set_num_players(2);
            }
        }
        if (Input.GetKeyDown("3")) {
            if (setup_num_players.activeInHierarchy) {
                set_num_players(3);
            }
        }
        if (Input.GetKeyDown("4")) {
             if (setup_num_players.activeInHierarchy) {
                set_num_players(4);
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab)) {
             if (setup_player_name_screen.activeInHierarchy) {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                tab_index++;
                if (next != null && tab_index < num_players) {
                    InputField inputfield = next.GetComponent<InputField>();
                    if (inputfield != null) { 
                        inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
                    }
                    system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                } else {
                    tab_index = 0;
                    next = first_setup_player_input.GetComponent<Selectable>().FindSelectableOnDown();
                    InputField inputfield = next.GetComponent<InputField>();
                    if (inputfield != null) { 
                        inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
                    }
                    system.SetSelectedGameObject(first_setup_player_input, new BaseEventData(system));
                }
            }
        }
    }
}
