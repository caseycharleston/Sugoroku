using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SetupGame : MonoBehaviour
{
    [SerializeField] Button one_player;
    [SerializeField] Button two_player;
    [SerializeField] Button three_player;
    [SerializeField] Button four_player;
    [SerializeField] GameObject setup_num_players;
    [SerializeField] GameObject setup_player_name_screen;
    [SerializeField] GameObject[] setup_player_cols;
    [SerializeField] TextMeshProUGUI[] setup_player_names;
    [SerializeField] Button go_to_order;
    [SerializeField] GameObject setup_player_order;
    [SerializeField] GameObject[] show_player_order;
    [SerializeField] TextMeshProUGUI[] show_name_order;
    [SerializeField] Button start_round;

    int num_players = 0;
    // Start is called before the first frame update
    void Start()
    {
        one_player.onClick.AddListener(delegate{set_num_players(1);});
        two_player.onClick.AddListener(delegate{set_num_players(2);});
        three_player.onClick.AddListener(delegate{set_num_players(3);});
        four_player.onClick.AddListener(delegate{set_num_players(4);}); 
        go_to_order.onClick.AddListener(randomize_order);
        start_round.onClick.AddListener(start);       
    }

    void set_num_players(int players) {
        num_players = players;
        GameControl.num_players = players;
        setup_num_players.SetActive(false);
         for (int i = 0; i < players; i++) {
            setup_player_cols[i].SetActive(true);
        }
        setup_player_name_screen.SetActive(true);
        
    }

    void randomize_order() {
        setup_player_name_screen.SetActive(false);
        List<int> used = new List<int>();
        while (used.Count < num_players) {
             int index = Random.Range(0, num_players);
             if (!used.Contains(index)) {
                used.Add(index);
             }
        }
        string[] names = new string[num_players];
        // BoardSpace curr_square = board[0];
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
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }



    // Update is called once per frame
    void Update()
    {
    
    }
}
