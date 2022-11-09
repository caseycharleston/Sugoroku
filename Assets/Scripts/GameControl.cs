using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameControl : MonoBehaviour {

        private static int[] no_fast = {};
        private static int[] space_two_fast = {0, 25, 0, 0, 0, 0};
        private static int[] space_four_fast = {0, 0, 0, 22, 0, 0};
        private static int[] space_six_fast = {7, 0, 10, 0, 27, 0};
        private static int[] space_seven_fast = {0, 10, 0, 15, 0, 27};
        private static int[] space_thirteen_fast = {22, 0, 0, 0, 0, 0};

       private static BoardSpace[] board = {
        new BoardSpace(false, no_fast),
        new BoardSpace(false, space_two_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, space_four_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, space_six_fast),
        new BoardSpace(false, space_seven_fast),
        new BoardSpace(true, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(true, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, space_thirteen_fast),
        new BoardSpace(true, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast),
        new BoardSpace(false, no_fast)
    };

     public Transform[] center_waypoints;

    private static GameObject player_text, player_text_con;
    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;
    private static GameObject trans_bg;

    private static GameObject player1, player2, player3, player4;
    private static GameObject curr_player;

    private static AudioSource wow_sfx;

    private static int turn = 0;
    private static GameObject[] order = new GameObject[4];
    public static int num_players;
    public static string[] names;

    private static GameObject main_camera, static_camera, follow_camera, zoom_camera;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int new_pos = 1;
    public static int square_pos = 0;
    public static int fast_travel_space = 0;

    public static bool stop_move = false;
    public static bool gameOver = false;
    public static bool setup_next = false;
    public static bool fast_travel = false;
    public static bool failed_fast = false;

    // Use this for initialization
    void Start () {
        //find all the gameobjects
        player_text = GameObject.Find("player_text");
        player_text_con = GameObject.Find("player_text_con");
        trans_bg = GameObject.Find("TransBG");
        main_camera = GameObject.Find("Main Camera");
        static_camera = GameObject.Find("static_camera");
        follow_camera = GameObject.Find("follow_camera");
        zoom_camera = GameObject.Find("extra_zoom");
        wow_sfx = GameObject.Find("AnimeWowSFX").GetComponent<AudioSource>();

        BoardSpace curr_square = board[0];
        //clear all the players on each board space
        for (int i = 0; i < board.Length; i++) {
            board[i].players_on_me.Clear();
        }

        //set up the player tokens
        for (int i = 0; i < num_players; i++) {
            order[i] = GameObject.Find("coin_" + (i + 1));
            order[i].GetComponent<PlayerInfo>().player_name = names[i];
            curr_square.players_on_me.Enqueue(order[i]);
        }
        // player1 = GameObject.Find("coin_1");
        // player2 = GameObject.Find("coin_2");
        // player3 = GameObject.Find("coin_3");
        // player4 = GameObject.Find("coin_4");
        // player1.GetComponent<FollowThePath>().moveAllowed = false;
        // player2.GetComponent<FollowThePath>().moveAllowed = false;
        // player3.GetComponent<FollowThePath>().moveAllowed = false;
        // player4.GetComponent<FollowThePath>().moveAllowed = false;
        // curr_square.players_on_me.Enqueue(player1);
        // curr_square.players_on_me.Enqueue(player2);
        // curr_square.players_on_me.Enqueue(player3);
        // curr_square.players_on_me.Enqueue(player4);
        // order[0] = player1;
        // order[1] = player2;
        // order[2] = player3;
        // order[3] = player4;

        curr_player = order[0];
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        trans_bg.SetActive(false);
        player_text_con.SetActive(true);
        setup_next = false;        
    }

    // Update is called once per frame
    void Update() {
        //player has reached destination
        if (curr_player.GetComponent<FollowThePath>().waypointIndex > new_pos) {
            int real_pos = new_pos;
            new_pos = 34;
            curr_player.GetComponent<FollowThePath>().moveAllowed = false;
            curr_player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            BoardSpace curr_square = board[curr_player.GetComponent<PlayerInfo>().curr_pos];
            stop_move = true;
            Debug.Log("Real Position: " + real_pos);
            zoom_camera.GetComponent<CinemachineVirtualCamera>().Follow = center_waypoints[real_pos];
            if (real_pos < 6) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else if (real_pos < 10) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            } else if (real_pos < 16) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            } else if (real_pos < 20) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            } else if (real_pos < 25) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else if (real_pos < 27) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            } else if (real_pos < 30) {
                zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            } else if (real_pos < 32) {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            } else {
                 zoom_camera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            if (curr_square.rest_square) {
                wow_sfx.Play();
            }
            follow_camera.SetActive(false);
            StartCoroutine("delay_next_turn");
        }

        if (setup_next) {
            setup_next = false;
            trans_bg.SetActive(false);
            main_camera.SetActive(true);
            StartCoroutine("zoom_out_next_turn");
        }

        if (curr_player.GetComponent<FollowThePath>().waypointIndex == curr_player.GetComponent<FollowThePath>().tl_waypoints.Length) {
            gameOver = true;
        }
    }

    IEnumerator zoom_out_next_turn() {
        yield return new WaitForSeconds(0.1f);
        static_camera.SetActive(true);
        follow_camera.SetActive(true);
        yield return new WaitForSeconds(2f);
        Dice.coroutineAllowed = false;
        player_text_con.SetActive(true);    
        if (fast_travel) {
            player_text.GetComponent<TMP_Text>().text = "Fast Travel Chance!";
        } else if (curr_player.GetComponent<PlayerInfo>().lose_a_turn) {
            player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " loses a turn!";
            yield return new WaitForSeconds(2f);
        }
        if (!fast_travel) {
            choose_next_player();
            player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        }
        Dice.coroutineAllowed = true;
    }

    IEnumerator delay_next_turn() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.1f);
        trans_bg.SetActive(true);
        main_camera.SetActive(false);
    }

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
                failed_fast = false;
                setup_move(space - 1);
                StaticCoroutine.DoCoroutine(success_fast_travel(space));
            } else {
                failed_fast = true;
                StaticCoroutine.DoCoroutine(failed_fast_travel());
            }
        }
    }

    static IEnumerator delay_zoomin() {
        yield return new WaitForSeconds(2f);
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    static IEnumerator success_fast_travel(int space) {
        player_text.GetComponent<TMP_Text>().text = "Success! Fast Travel to " + space;
        yield return new WaitForSeconds(2f);
        //copied from MovePlayer(), gacky af please think of a better way to do this (setupmove(0) was removed though)
        stop_move = false;
        player_text_con.SetActive(false);
        curr_player.GetComponent<SpriteRenderer>().sortingOrder = 2;
        follow_camera.SetActive(true);
        follow_camera.GetComponent<CinemachineVirtualCamera>().Follow = curr_player.transform;
        follow_camera.GetComponent<CinemachineVirtualCamera>().LookAt = curr_player.transform;
        static_camera.SetActive(false);
        yield return new WaitForSeconds(2f);
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    static IEnumerator failed_fast_travel() {
        player_text.GetComponent<TMP_Text>().text = "Failure...";
        yield return new WaitForSeconds(2f);
        choose_next_player();
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + " turn";
        Dice.coroutineAllowed = true;
    }

     private static void setup_move(int fast_travel_sp) {
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        board[player_info.curr_pos].players_on_me.Dequeue();
        if (fast_travel_sp != 0) {
            new_pos = fast_travel_sp;
        }  else {
            new_pos = player_info.reverse_path ? player_info.curr_pos -= diceSideThrown : player_info.curr_pos += diceSideThrown;
        }
        BoardSpace curr_square = board[new_pos];
        player_info.places_visited.Add(new_pos);
        curr_square.players_on_me.Enqueue(curr_player);
        square_pos = curr_square.players_on_me.Count;
        player_info.curr_pos = new_pos;
        if (curr_square.rest_square) { //check board space if player must lose turn 
            player_info.lose_a_turn = true;
        } 
        if (curr_square.fast_travels.Length > 0 && !player_info.reverse_path) { //only allow fast travel on the way to Yokohama, not on the way back.
            Debug.Log("Fast Travel!");
            fast_travel = true;
        }
        //  if (new_pos >= 33) { //player reached center, must allow player to move back to the start 
        //     new_pos = 32;
        //     player_info.reverse_path = true;
        // } else if (new_pos <= 0) { //player has reached the end goal
        //     new_pos = 0;
        //     Debug.Log("It's Over!");
        //     gameOver = true;
        //     //END GAME
        // }
    }
}

public class BoardSpace {
    public Queue<GameObject> players_on_me = new Queue<GameObject>(); //players on this space
    public bool rest_square = false; //if rest space or not
    public int[] fast_travels;       //fast travel spots
    //add which transition scene to use?

    //Boardspace Constructor
    public BoardSpace(bool rest, int[] fast) {
        rest_square = rest;
        fast_travels = fast;
    }
}

