using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameControl : MonoBehaviour {

    //used for creating boardspace and fast travel
    private static int[] no_fast = {};
    private static int[] space_two_fast = {0, 25, 0, 0, 0, 0};
    private static int[] space_four_fast = {0, 0, 0, 22, 0, 0};
    private static int[] space_six_fast = {7, 0, 10, 0, 27, 0};
    private static int[] space_seven_fast = {0, 10, 0, 15, 0, 27};
    private static int[] space_thirteen_fast = {22, 0, 0, 0, 0, 0};

    //array of boardspaces
    private static BoardSpace[] board = {
        new BoardSpace(false, no_fast, "Tokyo Nihonbashi Notice Board"),
        new BoardSpace(false, space_two_fast, "Tokyo Telegraph Office"),
        new BoardSpace(false, no_fast, "Bridge Mitsui Exchange Company"),
        new BoardSpace(false, space_four_fast, "Tuskiji Steam Ship Dock"),
        new BoardSpace(false, no_fast, "Shinbashi Iron Bridge"),
        new BoardSpace(false, space_six_fast, "Shibaguchi Steam Train"),
        new BoardSpace(false, space_seven_fast, "Takanawa Railroad Company"),
        new BoardSpace(true, no_fast, "Shinagawa Inn trip"),
        new BoardSpace(false, no_fast, "Rokugō Crossing"),
        new BoardSpace(false, no_fast, "Kawasaki Steam Train Company"),
        new BoardSpace(true, no_fast, "Daishi Kawasaki Heikenji Temple"),
        new BoardSpace(false, no_fast, "Moving through Tsurumi"),
        new BoardSpace(false, space_thirteen_fast, "Namamugi crossing to Yokohama"),
        new BoardSpace(true, no_fast, "Kanagawa Trip"),
        new BoardSpace(false, no_fast, "Kanagawa Steam Train Company"),
        new BoardSpace(false, no_fast, "Nōge Bridge"),
        new BoardSpace(false, no_fast, "Oda Mountain"),
        new BoardSpace(false, no_fast, "Government Theater"),
        new BoardSpace(false, no_fast, "Benzaiten Temple"),
        new BoardSpace(false, no_fast, "Yokohama Iron Bridge"),
        new BoardSpace(false, no_fast, "Bashamichi Street"),
        new BoardSpace(false, no_fast, "Yokohama Shoreline"),
        new BoardSpace(false, no_fast, "Going on a carriage ride"),
        new BoardSpace(false, no_fast, "Foreign Restaurant"),
        new BoardSpace(false, no_fast, "Telegraph Office, Customs Office Building"),
        new BoardSpace(false, no_fast, "Yokohama Honchōdōri Shopping Place"),
        new BoardSpace(false, no_fast, "Yokohama Company Steam Train"),
        new BoardSpace(false, no_fast, "Yokohama Fish Market"),
        new BoardSpace(false, no_fast, "Horse Race Track"),
        new BoardSpace(false, no_fast, "Foreign Merchant Houses"),
        new BoardSpace(false, no_fast, "Iseyama"),
        new BoardSpace(false, no_fast, "Seishōkōdo Shrine"),
        new BoardSpace(false, no_fast, "Yoshiwara") 
    };

     //extra popups, for fast travel and repeating square
    [SerializeField] Button yes_repeat, no_repeat;
    public GameObject[] fast_cols;
    public GameObject[] fast_dice;
    public TextMeshProUGUI[] fast_texts;
    public static GameObject fast_title;
    private Sprite[] diceSides;

    //containers
    private static GameObject player_text, player_text_con;
    private static GameObject trans_bg;
    private static GameObject double_land_con, fast_travel_con;

    //audio
    private static AudioSource wow_sfx;

    //players and turn
    private static GameObject player1, player2, player3, player4;
    private static GameObject curr_player;
    private static GameObject[] order = new GameObject[4];
    public static string[] names;
    private static int turn = 0;
    public static int num_players;

    //cameras
    private static GameObject main_camera, static_camera, follow_camera, zoom_camera;
    public Transform[] center_waypoints;

    //for each player
    public static int diceSideThrown = 0;
    public static int new_pos = 1;
    public static int square_pos = 0;
    public static int fast_travel_space = 0;

    //flags
    public static bool stop_move = false;
    public static bool finish_move = false;
    public static bool gameOver = false;
    public static bool setup_next = false;
    public static bool fast_travel = false;

    // Use this for initialization
    void Start () {
        //find all the gameobjects
        player_text = GameObject.Find("player_text");
        player_text_con = GameObject.Find("player_text_con");
        double_land_con = GameObject.Find("double_land_con");
        fast_travel_con = GameObject.Find("fast_travel_con");
        fast_title = GameObject.Find("fast_travel_title");
        trans_bg = GameObject.Find("TransBG");
        main_camera = GameObject.Find("Main Camera");
        static_camera = GameObject.Find("static_camera");
        follow_camera = GameObject.Find("follow_camera");
        zoom_camera = GameObject.Find("extra_zoom");
        wow_sfx = GameObject.Find("AnimeWowSFX").GetComponent<AudioSource>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        yes_repeat.onClick.AddListener(delegate{repeat_turn(true);});
        no_repeat.onClick.AddListener(delegate{repeat_turn(false);});

        BoardSpace curr_square = board[0];
        //clear all the players on each board space
        for (int i = 0; i < board.Length; i++) {
            board[i].players_on_me.Clear();
        }

        //REAL CODE, COMMENT THIS OUT WHEN DEBUG
        // set up the player tokens
        // for (int i = 0; i < num_players; i++) {
        //     order[i] = GameObject.Find("coin_" + (i + 1));
        //     order[i].GetComponent<PlayerInfo>().player_name = names[i];
        //     curr_square.players_on_me.Enqueue(order[i]);
        // }

        //DEBUG, UNCOMMENT THIS TO AVOID GOING THROUGH SETUP SCREEN
        player1 = GameObject.Find("coin_1");
        player2 = GameObject.Find("coin_2");
        player3 = GameObject.Find("coin_3");
        player4 = GameObject.Find("coin_4");
        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;
        player3.GetComponent<FollowThePath>().moveAllowed = false;
        player4.GetComponent<FollowThePath>().moveAllowed = false;
        curr_square.players_on_me.Enqueue(player1);
        curr_square.players_on_me.Enqueue(player2);
        curr_square.players_on_me.Enqueue(player3);
        curr_square.players_on_me.Enqueue(player4);
        order[0] = player1;
        order[1] = player2;
        order[2] = player3;
        order[3] = player4;
        num_players = 4;
        //END OF DEBUG

        curr_player = order[0];
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        trans_bg.SetActive(false);
        double_land_con.SetActive(false);
        fast_travel_con.SetActive(false);
        player_text_con.SetActive(true);
        setup_next = false;        
        gameOver = false;
    }

    // Update is called once per frame
    void Update() {

        //player has reached destination
        if (finish_move) {
            finish_move = false;
            curr_player.GetComponent<FollowThePath>().moveAllowed = false;
            curr_player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (new_pos == 32) { //reverse path
                curr_player.GetComponent<PlayerInfo>().reverse_path = true;
                curr_player.GetComponent<FollowThePath>().waypointIndex -= 1;
            } else if (new_pos == 0) { //reached end
                gameOver = true;
            }
            stop_move = true;
            Debug.Log("Real Position: " + new_pos);
            //set zoom to specific boardspace
            zoom_camera.GetComponent<CinemachineVirtualCamera>().Follow = center_waypoints[new_pos];

            //rotate camera when needed
            if (new_pos < 6) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else if (new_pos < 10) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            } else if (new_pos < 16) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            } else if (new_pos < 20) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            } else if (new_pos < 25) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else if (new_pos < 27) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            } else if (new_pos < 30) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            } else if (new_pos < 32) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            } else {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            
            //play wow sfx on rest board square
            if (board[new_pos].rest_square) {
                wow_sfx.Play();
                Debug.Log("hello?");
            }
            follow_camera.SetActive(false);
            StartCoroutine("delay_info_space");
        }

        //activated when infospace is closed out of
        if (setup_next) {
            setup_next = false;
            trans_bg.SetActive(false);
            main_camera.SetActive(true);
            StartCoroutine("zoom_out_next_turn");
        }

    }

    //sets up next turn
    IEnumerator zoom_out_next_turn() {
        yield return new WaitForSeconds(0.1f);
        static_camera.SetActive(true);
        follow_camera.SetActive(true);
        yield return new WaitForSeconds(2f);
        Dice.coroutineAllowed = false;
        if (fast_travel) {
            show_fast_travel();
            yield break;
        } 
        player_text_con.SetActive(true);    
         if (curr_player.GetComponent<PlayerInfo>().lose_a_turn) {
            player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " loses a turn!";
            yield return new WaitForSeconds(2f);
        }
        choose_next_player();
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        Dice.coroutineAllowed = true;
    }

    //sets up going into info space, loads correct infospace scene
    IEnumerator delay_info_space() {
        yield return new WaitForSeconds(2f);
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        if (gameOver) {
            GameOver.winner = player_info.player_name;
            SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);
            yield break;
        } else if (player_info.places_visited.Contains(player_info.curr_pos)){
            //player has already visited this location
            double_land_con.SetActive(true);
            yield break;
        } else {
            player_info.places_visited.Add(player_info.curr_pos);
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        }
        yield return new WaitForSeconds(0.1f);
        trans_bg.SetActive(true);
        main_camera.SetActive(false);
    }

    //sets up correct player for next turn
    static void choose_next_player() {
        turn++;
        if (turn >= num_players) {
            turn = 0;
        }
        curr_player = order[turn];
        if (curr_player.GetComponent<PlayerInfo>().lose_a_turn) { //Skip a player
            while(curr_player.GetComponent<PlayerInfo>().lose_a_turn) { //Find a player that isn't resting.
                Debug.Log("You Rested!");
                curr_player.GetComponent<PlayerInfo>().lose_a_turn = false; //If find a player that is resting, make them unrested.
                turn++;
                if (turn >= num_players) {
                    turn = 0;
                }
                curr_player = order[turn];
            }
        }
    }

    //Called by any dice once it has finished rolling.
    //Before actually moving the player, sets up camera work and move information
    //if fast travel, handle if successful or false here.
    public static void MovePlayer() {
        if (!fast_travel) {
            stop_move = false;
            player_text_con.SetActive(false);
            curr_player.GetComponent<SpriteRenderer>().sortingOrder = 2;
            follow_camera.GetComponent<CinemachineVirtualCamera>().Follow = curr_player.transform;
            follow_camera.GetComponent<CinemachineVirtualCamera>().LookAt = curr_player.transform;
            setup_move(0);
            static_camera.SetActive(false);
            StaticCoroutine.DoCoroutine(delay_zoomin());
        } else {
            Dice.coroutineAllowed = false;
            fast_travel = false;
            int curr_pos = curr_player.GetComponent<PlayerInfo>().curr_pos;
            int space = board[curr_pos].fast_travels[diceSideThrown - 1];
            // int space = board[curr_pos].fast_travels[0];

            fast_travel_space = space;
            if (space != 0) {
                setup_move(space - 1);
                StaticCoroutine.DoCoroutine(success_fast_travel(space));
            } else {
                StaticCoroutine.DoCoroutine(failed_fast_travel());
            }
        }
    }

    //Starts player movement
    static IEnumerator delay_zoomin() {
        yield return new WaitForSeconds(2f);
        List<Transform[]> waypoints = curr_player.GetComponent<FollowThePath>().wp;
        update_board_space(board[new_pos], waypoints, new_pos); //update board space before move to square 
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    //Handles successful fast travel
    static IEnumerator success_fast_travel(int space) {
        fast_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Success! Fast Travel to " + space;
        yield return new WaitForSeconds(2f);
        List<Transform[]> waypoints = curr_player.GetComponent<FollowThePath>().wp;
        update_board_space(board[new_pos], waypoints, new_pos); //update board space before move to square 
        //copied from MovePlayer(), gacky af please think of a better way to do this (setupmove(0) was removed though)
        stop_move = false;
        fast_travel_con.SetActive(false);
        curr_player.GetComponent<SpriteRenderer>().sortingOrder = 2;
        follow_camera.SetActive(true);
        follow_camera.GetComponent<CinemachineVirtualCamera>().Follow = curr_player.transform;
        follow_camera.GetComponent<CinemachineVirtualCamera>().LookAt = curr_player.transform;
        static_camera.SetActive(false);
        yield return new WaitForSeconds(2f);
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    //Handles failed fast travel
    static IEnumerator failed_fast_travel() {
        fast_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Failure...";
        yield return new WaitForSeconds(2f);
        fast_travel_con.SetActive(false);
        choose_next_player();
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        player_text_con.SetActive(true);
        Dice.coroutineAllowed = true;
    }

    //Sets up neccesary move information
     private static void setup_move(int fast_travel_sp) {
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        board[player_info.curr_pos].players_on_me.Dequeue();
        if (fast_travel_sp != 0) {
            new_pos = fast_travel_sp;
        }  else {
            new_pos = player_info.reverse_path ? player_info.curr_pos -= diceSideThrown : player_info.curr_pos += diceSideThrown;
        }
         if (new_pos >= 33) { //player reached center, must allow player to move back to the start 
            new_pos = 32;
        } else if (new_pos <= 0) { //player has reached the end goal
            new_pos = 0;
            Debug.Log("It's Over!");
            //END GAME
        }

        Debug.Log("new_pos" + new_pos);
        BoardSpace curr_square = board[new_pos];
        curr_square.players_on_me.Enqueue(curr_player);

        square_pos = curr_square.players_on_me.Count;
        player_info.curr_pos = new_pos;

        if (curr_square.rest_square) { //check board space if player must lose turn 
            player_info.lose_a_turn = true;
        } 

        //only allow fast travel on the way to Yokohama, not on the way back.
        if (curr_square.fast_travels.Length > 0 && !player_info.reverse_path) { 
            Debug.Log("Fast Travel!");
            fast_travel = true;
        }
    }

    //given a board space, update the players' positions on it
    static void update_board_space(BoardSpace square, List<Transform[]> waypoints, int pos) {
        Queue<GameObject> players = square.players_on_me;
        int pos_count = 0;
        foreach (GameObject player in players) {
            if (pos_count > (players.Count - 2)) { //
                break;
            }
            player.transform.position = waypoints[pos_count][pos].transform.position;
            pos_count++;
        }
    }

    //handle when a player lands on the same board space again
    void repeat_turn(bool repeat) {
        double_land_con.SetActive(false);
        if (repeat) {
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive); //TODO adjust the new pos
            trans_bg.SetActive(true);
            main_camera.SetActive(false);
        } else {
            setup_next = true;
        }
    }

    //handle when player lands on a fast travel square
    void show_fast_travel() {
        fast_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Fast Travel Chance!";
        int[] fast_travels = board[new_pos].fast_travels;
        List<int> indexes = new List<int>();
        for (int i = 0; i < fast_travels.Length; i++) {
            int val = fast_travels[i];
            if (val != 0) {
                indexes.Add(i);
            }
        }

        if (indexes.Count == 1) {
            fast_cols[1].SetActive(true);
            fast_dice[1].GetComponent<SpriteRenderer>().sprite = diceSides[indexes[0]];
            // fast_texts[1].GetComponent<TMP_Text>().text = fast_travels[indexes[0]] + "";
            fast_texts[1].GetComponent<TMP_Text>().text = board[fast_travels[indexes[0]] - 1].name;

        } else {
            for (int i = 0; i < indexes.Count; i++) {
                fast_cols[i].SetActive(true);
                fast_dice[i].GetComponent<SpriteRenderer>().sprite = diceSides[indexes[i]];
                fast_texts[i].GetComponent<TMP_Text>().text = board[fast_travels[indexes[i]] - 1].name;

            }
        }
        fast_travel_con.SetActive(true);
        Dice.coroutineAllowed = true;
    }
} //end of GameControl class

//BoardSpace class
public class BoardSpace {
    public Queue<GameObject> players_on_me = new Queue<GameObject>(); //players on this space
    public bool rest_square = false; //if rest space or not
    public int[] fast_travels;       //fast travel spots
    public string name;
    //add which transition scene to use?

    //Boardspace Constructor
    public BoardSpace(bool rest, int[] fast, string name) {
        rest_square = rest;
        fast_travels = fast;
        this.name = name;
    }
}

