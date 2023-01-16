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

    private static string space_two_fast_text = "Entranced by the telegraph office, you make a spur-of-the-moment decision to send a telegraph to an old friend who recently moved to Yokohama in hopes of benefiting from the treaty port’s mercantile boom. With each beep! you jump, and a smile stretches across your face. \nHow extraordinary! \nYou are amazed at how something as complex as language could be boiled down to an operator’s methodical tapping. You whiz up to the Yokohama Telegraph Office where your friend is waiting for you.";

    private static string space_four_fast_text = "You board the ship, clutching the edge of the railing as it rocks you back and forth. A noisy horn reverberates across the sea. As the water laps against the hull, you feel the boat pulling away from the harbor, churning your stomach as if it were the paddlewheel sucking water into the ship’s boiler.";

    private static string space_six_seven_fast_text = "You fish for the ticket in your pocket, ready to present it to the ticket collector. You extract the ticket and look over its destination before handing it to the collector. Giving thanks to him, you take your seat near the window and look out to the ever-expanding sea. As you take in the endless expanse, the train begins to move. Within seconds, you are gliding along the ocean. Words cannot describe the sensation. For the remainder of the trip, you sit silent, eyes fixated on the glass.";

    private static string space_thirteen_fast_text = "You walk around the dock until you come across a wharf. \n“Excuse me,” you politely ask. “Where does this boat lead?”\n“Where do you think?” a man replies in a gruff tone. He appears to be irritated by your ignorance. \n“We are at the crossing between Namamugi and Yokohama, where all sorts of goods are transported to and from, now climb aboard!” \nYou clumsily stumble onto the boat. \nAt last, you think, hardly able to contain your smile. I am about to reach Yokohama!";

    //array of boardspaces
    private static BoardSpace[] board = {
        new BoardSpace(false, no_fast, "Tokyo Nihonbashi Notice Board"),
        new BoardSpace(false, space_two_fast, "Tokyo Telegraph Office"),
        new BoardSpace(false, no_fast, "Mitsui Exchange Company"),
        new BoardSpace(false, space_four_fast, "Tuskiji Steam Ship Dock"),
        new BoardSpace(false, no_fast, "Shinbashi Iron Bridge"),
        new BoardSpace(false, space_six_fast, "Shibaguchi Steam Train"),
        new BoardSpace(false, space_seven_fast, "Takanawa Train Station"),
        new BoardSpace(true, no_fast, "Shinagawa Inn"),
        new BoardSpace(false, no_fast, "Rokugō Crossing"),
        new BoardSpace(false, no_fast, "Kawasaki Train Station"),
        new BoardSpace(true, no_fast, "Daishi Kawasaki Heikenji Temple"),
        new BoardSpace(false, no_fast, "Tsurumi"),
        new BoardSpace(false, space_thirteen_fast, "Namamugi"),
        new BoardSpace(true, no_fast, "Kanagawa Inn"),
        new BoardSpace(false, no_fast, "Kanagawa Train Station"),
        new BoardSpace(false, no_fast, "Nōge Bridge"),
        new BoardSpace(false, no_fast, "Oda Mountain"),
        new BoardSpace(false, no_fast, "Kabuki Theater"),
        new BoardSpace(false, no_fast, "Benzaiten Temple"),
        new BoardSpace(false, no_fast, "Yokohama Iron Bridge"),
        new BoardSpace(false, no_fast, "Bashamichi Street"),
        new BoardSpace(false, no_fast, "Yokohama Shoreline"),
        new BoardSpace(false, no_fast, "Going on a carriage ride"),
        new BoardSpace(false, no_fast, "Foreign Restaurant"),
        new BoardSpace(false, no_fast, "Yokohama Telegraph Office"),
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
    public static GameObject on_success;
    public static GameObject success_text, success_title;
    public GameObject[] intro_popups;
    [SerializeField] Button success_exit;
    private Sprite[] diceSides;

    //pause
    [SerializeField] Button pause_button, resume_button, howtoplay_button, exit_button;

    //containers
    private static GameObject player_text, player_text_con;
    private static GameObject fast_travel_con, fast_travel_cols;
    private GameObject space_name_con;
    public TextMeshProUGUI space_text;
    private static GameObject double_land_con, pause_con;

    //audio
    private static AudioSource wow_sfx, pause_sfx, zoom_sfx, steps_sfx, kabuki4, fast_travel_sfx;
    public AudioSource[] board_space_sfxs;
    private static AudioSource forward_bgm, backward_bgm, bass_drum_loop, gong_sfx;

    //players and turn
    private static GameObject player1, player2, player3, player4;
    private static GameObject curr_player;
    private static GameObject[] order = new GameObject[4];
    public static string[] names;
    private static int turn = 0;
    public static int num_players;
    private static int[] race_placings;
    private static PlayerInfo[] player_standings;
    private static Color[] standing_colors = {new Color32(247, 214, 54, 255), new Color32(142, 142, 142, 255), new Color32(152, 96, 34, 255), Color.black};
    private static string[] rankings = {"1st", "2nd", "3rd", "4th"};
    public HashSet<int> places_visited = new HashSet<int>();    //board spaces already visited

    //mario party
    public GameObject[] mario_party_con;
    private static GameObject[] static_mario_party_con;
    public TextMeshProUGUI[] mario_party_names;
    public TextMeshProUGUI[] mario_party_positions;
    private static TextMeshProUGUI[] static_mario_party_positions; 

    //cameras
    private static GameObject main_camera, static_camera, follow_camera, zoom_camera;
    public static CinemachineBrain brain;
    public Transform[] center_waypoints;
    public static float zoom_speed = 2.5f;

    //for each player
    public static int diceSideThrown = 0;
    public static int new_pos = 1;
    public static int old_pos = 0;
    public static int square_pos = 0;
    public static int fast_travel_space = 0;

    //flags
    public static bool stop_move = false;
    public static bool finish_move = false;
    public static bool gameOver = false;
    public static bool setup_next = false;
    public static bool fast_travel = false;
    public static bool second_fast_travel = false;
    private bool first_reverse = false;
    private bool after_intro = true;

    // Use this for initialization
    void Start () {
        //find all the gameobjects
        player_text = GameObject.Find("player_text");
        player_text_con = GameObject.Find("player_text_con");
        double_land_con = GameObject.Find("double_land_con");
        fast_travel_con = GameObject.Find("fast_travel_con");
        pause_con = GameObject.Find("pause_con");
        fast_title = GameObject.Find("fast_travel_title");
        success_title = GameObject.Find("success_title");
        success_text = GameObject.Find("success_text");
        on_success = GameObject.Find("on_success");
        fast_travel_cols = GameObject.Find("fast_travel_cols");
        space_name_con = GameObject.Find("space_name_con");

        main_camera = GameObject.Find("Main Camera");
        static_camera = GameObject.Find("static_camera");
        follow_camera = GameObject.Find("follow_camera");
        zoom_camera = GameObject.Find("extra_zoom");
        brain = FindObjectOfType<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = zoom_speed; // 0 Time equals a cut

        forward_bgm = GameObject.Find("ForwardRun").GetComponent<AudioSource>();
        backward_bgm = GameObject.Find("BackwardRun").GetComponent<AudioSource>();
        pause_sfx = GameObject.Find("PauseSound").GetComponent<AudioSource>();
        zoom_sfx = GameObject.Find("ZoomSFX").GetComponent<AudioSource>();
        steps_sfx = GameObject.Find("Footsteps").GetComponent<AudioSource>();
        kabuki4 = GameObject.Find("KabukiP4").GetComponent<AudioSource>();
        fast_travel_sfx = GameObject.Find("FastTravelPopup").GetComponent<AudioSource>();
        bass_drum_loop = GameObject.Find("BassDrumLoop").GetComponent<AudioSource>();
        gong_sfx = GameObject.Find("Gong").GetComponent<AudioSource>();

        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        yes_repeat.onClick.AddListener(delegate{repeat_turn(true);});
        no_repeat.onClick.AddListener(delegate{repeat_turn(false);});
        success_exit.onClick.AddListener(start_exit_success_fast_travel);
        pause_button.onClick.AddListener(pause);
        resume_button.onClick.AddListener(unpause);
        howtoplay_button.onClick.AddListener(howtoplay);
        exit_button.onClick.AddListener(exit_game);

        double_land_con.SetActive(false);
        fast_travel_con.SetActive(false);
        space_name_con.SetActive(false);
        pause_con.SetActive(false);

        setup_next = false;        
        gameOver = false;
        fast_travel = false;
        stop_move = false;
        finish_move = false;
        second_fast_travel = false;
        first_reverse = false;
        after_intro = true;

        turn = 0;
        new_pos = 1;
        old_pos = 0;
        square_pos = 0;
        fast_travel_space = 0;
        
        BoardSpace curr_square = board[0];
        //clear all the players on each board space
        for (int i = 0; i < board.Length; i++) {
            board[i].players_on_me.Clear();
        }
        places_visited.Clear();


        //REAL CODE, COMMENT THIS OUT WHEN DEBUG
        // set up the player tokens
        player_standings = new PlayerInfo[num_players];
        race_placings = new int[num_players];
        static_mario_party_positions = new TextMeshProUGUI[num_players];
        static_mario_party_con = new GameObject[num_players];
        for (int i = 0; i < num_players; i++) {
            order[i] = GameObject.Find("coin_" + (i + 1));
            order[i].GetComponent<PlayerInfo>().player_name = names[i];
            mario_party_names[i].GetComponent<TMP_Text>().text = names[i];
            mario_party_positions[i].GetComponent<TMP_Text>().text = rankings[i];
            mario_party_positions[i].GetComponent<TMP_Text>().color = standing_colors[i];
            static_mario_party_positions[i] = mario_party_positions[i];
            mario_party_con[i].SetActive(true);
            static_mario_party_con[i] = GameObject.Find("MarioParty" + (i + 1));
            curr_square.players_on_me.Enqueue(order[i]); // probably can take this line out, don't need to enqueue the first square
            race_placings[i] = 0;
            player_standings[i] = order[i].GetComponent<PlayerInfo>();    
        }


        //DEBUG, UNCOMMENT THIS TO AVOID GOING THROUGH SETUP SCREEN
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

        // player_standings = new PlayerInfo[4];
        // static_mario_party_positions = new TextMeshProUGUI[4];
        // static_mario_party_con = new GameObject[4];
        // race_placings = new int[4];
        // for (int i = 0; i < 4; i++) {
        //     mario_party_names[i].GetComponent<TMP_Text>().text =  order[i].GetComponent<PlayerInfo>().player_name;
        //     mario_party_positions[i].GetComponent<TMP_Text>().text = rankings[i];
        //     mario_party_positions[i].GetComponent<TMP_Text>().color = standing_colors[i];
        //     static_mario_party_positions[i] = mario_party_positions[i];
        //     mario_party_con[i].SetActive(true);
        //     static_mario_party_con[i] = GameObject.Find("MarioParty" + (i + 1));
        //     race_placings[i] = 0;
        //     player_standings[i] = order[i].GetComponent<PlayerInfo>();    
        // }
        // num_players = 1;
        // END OF DEBUG

        Time.timeScale = 1;
        curr_player = order[0];
        follow_camera.GetComponent<CinemachineVirtualCamera>().Follow = curr_player.transform;
        follow_camera.GetComponent<CinemachineVirtualCamera>().LookAt = curr_player.transform;

        Dice.coroutineAllowed = false;
        StartCoroutine("intro");

        //DEBUG SKIP INTRO
        // player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + "'s turn";
        // player_text_con.SetActive(true);
        // after_intro = false;
    }

    private IEnumerator intro() {
        yield return new WaitForSeconds(1f);
        zoom_camera.GetComponent<CinemachineVirtualCamera>().Follow = center_waypoints[0];
        zoom_camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 5f;
        static_camera.SetActive(false);
        follow_camera.SetActive(false);
        zoom_sfx.Play();
        yield return new WaitForSeconds(2f);
        InitiateAdd.Fade("Board_1", Color.black, 2f);
    }

    // Update is called once per frame
    void Update() {
        //player has reached destination
        if (finish_move) {
            finish_move = false;
            curr_player.GetComponent<FollowThePath>().moveAllowed = false;
            curr_player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (new_pos == 32) { //reverse path
                zoom_camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 9f;
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
            
            follow_camera.SetActive(false);
            StartCoroutine("delay_info_space");
        }

        //activated when infospace is closed out of
        if (setup_next) {
            setup_next = false;
            if (after_intro) {
                StartCoroutine("zoom_out_intro");
            } else {
                StartCoroutine("zoom_out_next_turn");
            }
        }

    }

    IEnumerator zoom_out_intro() {
        after_intro = false;
        yield return new WaitForSeconds(0.1f);
        static_camera.SetActive(true);
        follow_camera.SetActive(true);
        zoom_sfx.Play();
        yield return new WaitForSeconds(zoom_speed + 1f);
        for (int i = 0; i < 30; i++) {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().reduce_audio(0.02f);
            yield return new WaitForSeconds(0.05f);
        }
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
        Destroy(GameObject.FindGameObjectWithTag("Music"));
        bass_drum_loop.Play();
        int popup_index = 0;
        while (popup_index < 3) {
            GameObject curr_object = intro_popups[popup_index];
            curr_object.SetActive(true);
            Image curr_image = curr_object.GetComponent<Image>();
            float wait_time = 0.005f;
            if (popup_index == 2) wait_time = 0.0025f;
            for (int i = 0; i < 100; i++) {
                Color temp = curr_image.color;
                temp.a += 0.01f;
                curr_image.color = temp;
                if (popup_index == 2 && i == 30) {
                    gong_sfx.Play();
                    forward_bgm.Play();
                    bass_drum_loop.Stop();
                }
                yield return new WaitForSeconds(wait_time);
            }
            if (popup_index == 2) {
                yield return new WaitForSeconds(6f);
            } else {
                yield return new WaitForSeconds(2f + popup_index + 0.4f * (popup_index));
            }
            for (int i = 0; i < 100; i++) {
                Color temp = curr_image.color;
                temp.a -= 0.01f;
                curr_image.color = temp;
                yield return new WaitForSeconds(0.0025f);
            }
            yield return new WaitForSeconds(0.5f);
            curr_object.SetActive(false);
            popup_index++;
            if (popup_index != 2) {   
                for (int i = 0; i < 35; i++) {
                    bass_drum_loop.volume += 0.01f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        Dice.coroutineAllowed = true;
         player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + "'s turn";
        player_text_con.SetActive(true);
    }

    //sets up next turn
    IEnumerator zoom_out_next_turn() {
        yield return new WaitForSeconds(0.1f);
        static_camera.SetActive(true);
        follow_camera.SetActive(true);
        zoom_sfx.Play();
        yield return new WaitForSeconds(zoom_speed);
        Dice.coroutineAllowed = false;
        if (fast_travel) {
            show_fast_travel();
            yield break;
        }
        if (new_pos == 32 && !first_reverse) {
                first_reverse = true;
                forward_bgm.Stop();
                backward_bgm.Play();
            } 
        player_text_con.SetActive(true);    
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        zoom_camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 5f;
         if (player_info.lose_a_turn) {                                                                                                     
            player_text.GetComponent<TMP_Text>().text = player_info.player_name + " loses a turn!";
            yield return new WaitForSeconds(zoom_speed);
        }

        //change mario party container positions based on new position
        MoveContainer my_con = mario_party_con[player_info.id - 1].GetComponent<MoveContainer>();
        race_placings[player_info.curr_place] = new_pos;

        for (int i = player_info.curr_place - 1; i >= 0; i--) {
            bool check;
            if (player_info.reverse_path && new_pos != 32) {
                check = new_pos < race_placings[i];
            } else {
                check = (!player_standings[i].reverse_path) && (new_pos > race_placings[i]);
            }
            if (check) {
                Debug.Log("Swap Places!");
                player_info.curr_place--;
                player_standings[i].curr_place++;
                MoveContainer their_con = mario_party_con[player_standings[i].id - 1].GetComponent<MoveContainer>();

                my_con.wp_index--;
                their_con.wp_index++;
                my_con.move_allowed = true;
                their_con.move_allowed = true;
                race_placings[i + 1] = race_placings[i];
                race_placings[i] = new_pos;
                player_standings[i + 1] = player_standings[i];
                player_standings[i] = player_info;
                yield return new WaitForSeconds(1f);
            }
        }

        for (int i = 0; i < num_players; i++) {
            mario_party_positions[player_standings[i].id - 1].GetComponent<TMP_Text>().text = rankings[i];
            mario_party_positions[player_standings[i].id - 1].GetComponent<TMP_Text>().color = standing_colors[i];
        }
        choose_next_player();
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + "'s turn";
        Dice.coroutineAllowed = true;
    }

    //sets up going into info space, loads correct infospace scene
    IEnumerator delay_info_space() {
        steps_sfx.Play();
        yield return new WaitForSeconds(zoom_speed);
        steps_sfx.Stop();
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        if (gameOver) {
            hidden_calculate_standings();
            GameOver.rankings = player_standings;
            Initiate.Fade("GameOver", Color.black, 1f);
            yield break;
        } else if (places_visited.Contains(player_info.curr_pos)){
            if (player_info.curr_pos == 19 && player_info.reverse_path){
                // need to use going back script
                InitiateAdd.Fade("Board_20_again", Color.black, 2f);
            }
            else {
                double_land_con.SetActive(true);
                yield break;
            }
        } else {
            places_visited.Add(player_info.curr_pos);
            brain.m_DefaultBlend.m_Time = 0; // 0 Time equals a cut
            space_text.GetComponent<TMP_Text>().text = "You've landed on\n" + board[player_info.curr_pos].name + "!";
            space_name_con.SetActive(true);
            board_space_sfxs[player_info.curr_pos].Play();
            yield return new WaitForSeconds(zoom_speed + 1f);
            // test_sfx.Stop();
            space_name_con.SetActive(false);
            if (player_info.curr_pos == 19 && player_info.reverse_path == true){
                SceneManager.LoadSceneAsync("Board_20_again", LoadSceneMode.Additive);
            } else {
                // SceneManager.LoadSceneAsync("Board_" + (player_info.curr_pos + 1), LoadSceneMode.Additive);
                InitiateAdd.Fade("Board_" + (player_info.curr_pos + 1), Color.black, 2f);
                // SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
            }
        }
    }

    //called for when the winner has arrived at the end
    void hidden_calculate_standings() {
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        race_placings[player_info.curr_place] = new_pos;
        for (int i = player_info.curr_place - 1; i >= 0; i--) {
            if (new_pos < race_placings[i]) {
                Debug.Log("Swap Places!");
                player_info.curr_place--;
                player_standings[i].curr_place++;
                race_placings[i + 1] = race_placings[i];
                race_placings[i] = new_pos;
                player_standings[i + 1] = player_standings[i];
                player_standings[i] = player_info;
            }
        }
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
            // player_text_con.SetActive(false);
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
                if (space == 7) {
                    second_fast_travel = true;
                }
                setup_move(space - 1);
                StaticCoroutine.DoCoroutine(success_fast_travel(space));
            } else {
                StaticCoroutine.DoCoroutine(failed_fast_travel());
            }
        }
    }

    //Starts player movement
    static IEnumerator delay_zoomin() {
        zoom_sfx.Play();
        yield return new WaitForSeconds(zoom_speed);
        List<Transform[]> waypoints = curr_player.GetComponent<FollowThePath>().wp;
        update_board_space(board[new_pos], waypoints, new_pos); //update board space before move to square 
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    //Handles successful fast travel
    static IEnumerator success_fast_travel(int space) {
        fast_travel_cols.SetActive(false);
        on_success.SetActive(true);
        success_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Fast Travel to " + board[space - 1].name;
        switch (old_pos) {
            case 1:
                success_text.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = space_two_fast_text;
                break;
            case 3:
                success_text.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = space_four_fast_text;
                break;
            case 5:
                success_text.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = space_six_seven_fast_text;
                break;
            case 6:
                success_text.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = space_six_seven_fast_text;
                break;
            case 12:
                success_text.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = space_thirteen_fast_text;
                break;
        }
        yield break;
    }

    void start_exit_success_fast_travel() {
        kabuki4.Play();
        StartCoroutine("exit_success_fast_travel");
    }

    private IEnumerator exit_success_fast_travel() {
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
        zoom_sfx.Play();
        yield return new WaitForSeconds(zoom_speed);
        curr_player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    //Handles failed fast travel
    static IEnumerator failed_fast_travel() {
        fast_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Failure...";
        yield return new WaitForSeconds(zoom_speed + 1f);
        fast_travel_con.SetActive(false);

        //copied and pasted from the delay_next_turn()
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        MoveContainer my_con = static_mario_party_con[player_info.id - 1].GetComponent<MoveContainer>();
        race_placings[player_info.curr_place] = new_pos;

        for (int i = player_info.curr_place - 1; i >= 0; i--) {
            bool check;
            if (player_info.reverse_path && new_pos != 32) {
                check = new_pos < race_placings[i];
            } else {
                check = (!player_standings[i].reverse_path) && (new_pos > race_placings[i]);
            }
            if (check) {
                Debug.Log("Swap Places!");
                player_info.curr_place--;
                player_standings[i].curr_place++;
                MoveContainer their_con = static_mario_party_con[player_standings[i].id - 1].GetComponent<MoveContainer>();

                my_con.wp_index--;
                their_con.wp_index++;
                my_con.move_allowed = true;
                their_con.move_allowed = true;
                race_placings[i + 1] = race_placings[i];
                race_placings[i] = new_pos;
                player_standings[i + 1] = player_standings[i];
                player_standings[i] = player_info;
                yield return new WaitForSeconds(1f);
            }
        }
        for (int i = 0; i < num_players; i++) {
            static_mario_party_positions[player_standings[i].id - 1].GetComponent<TMP_Text>().text = rankings[i];
            static_mario_party_positions[player_standings[i].id - 1].GetComponent<TMP_Text>().color = standing_colors[i];
        }

        choose_next_player();
        player_text.GetComponent<TMP_Text>().text = curr_player.GetComponent<PlayerInfo>().player_name + "'s turn";
        kabuki4.Play();
        player_text_con.SetActive(true);
        Dice.coroutineAllowed = true;
    }

    //Sets up neccesary move information
     private static void setup_move(int fast_travel_sp) {
        PlayerInfo player_info = curr_player.GetComponent<PlayerInfo>();
        board[player_info.curr_pos].players_on_me.Dequeue();
        old_pos = player_info.curr_pos;
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
        if (curr_square.fast_travels.Length > 0 && !player_info.reverse_path && !second_fast_travel) { 
            Debug.Log("Fast Travel!");
            fast_travel = true; //DEBUG take this out when you want no fast travel to happen
        }
        if (second_fast_travel) { //pretty jank but stops the double fast travel from happening
            second_fast_travel = false;
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
            SceneManager.LoadSceneAsync("Board_" + (curr_player.GetComponent<PlayerInfo>().curr_pos + 1), LoadSceneMode.Additive); //TODO adjust the new pos
        } else {
            setup_next = true;
        }
    }

    //handle when player lands on a fast travel square
    void show_fast_travel() {
        for (int i = 0; i < 3; i++) {
            fast_cols[i].SetActive(false);
        }
        fast_travel_sfx.Play();
        fast_travel_cols.SetActive(true);
        on_success.SetActive(false);
        fast_title.GetComponent<TextMeshProUGUI>().GetComponent<TMP_Text>().text = "Fast Travel Chance!";
        int[] fast_travels = board[new_pos].fast_travels;
        List<int> indexes = new List<int>();
        for (int i = 0; i < fast_travels.Length; i++) {
            int val = fast_travels[i];
            Debug.Log(fast_travels[i]);
            if (val != 0) {
                indexes.Add(i);
            }
        }

        if (indexes.Count == 1) {
            fast_cols[1].SetActive(true);
            fast_dice[1].GetComponent<Image>().sprite = diceSides[indexes[0]];
            fast_texts[1].GetComponent<TMP_Text>().text = board[fast_travels[indexes[0]] - 1].name;
        } else {
            for (int i = 0; i < indexes.Count; i++) {
                fast_cols[i].SetActive(true);
                fast_dice[i].GetComponent<Image>().sprite = diceSides[indexes[i]];
                fast_texts[i].GetComponent<TMP_Text>().text = board[fast_travels[indexes[i]] - 1].name;
            }
        }
        fast_travel_con.SetActive(true);
        Dice.coroutineAllowed = true;
    }

    void pause() {
        Dice.coroutineAllowed = false;
        pause_sfx.Play();
        pause_con.SetActive(true);
        Time.timeScale = 0;
    }

    void unpause() {
        Dice.coroutineAllowed = true;
        pause_sfx.Play();
        pause_con.SetActive(false);
        Time.timeScale = 1;
    }

    void howtoplay() {
        pause_sfx.Play();
        HowToPlayScript.title_screen = false;
        SceneManager.LoadSceneAsync("HowToPlayGame", LoadSceneMode.Additive);
        //  InitiateAdd.Fade("HowToPlayGame", Color.black, 1f);
    }

    void exit_game() {
         pause_sfx.Play();
        // SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        Time.timeScale = 1;
        Initiate.Fade("TitleScreen", Color.black, 0.5f);
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

