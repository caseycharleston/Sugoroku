using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Classes
    //Player class
    public class Player {
        public GameObject token;    //image or main representing object
        public string name;         //name of player
        public int id;              //player order id

        public int curr_pos = 0;    //current board position
        public HashSet<int> places_visited = new HashSet<int>();    //board spaces already visited
        public bool reverse_path = false;   //if player must go backwards
        public bool lose_a_turn = false;    //if player lost a turn

        //Player Constructor
        public Player(string player_name, int id, Canvas canvas) {
            name = player_name;
            this.id = id;

            //create token
            GameObject tok = new GameObject("player_" + id + "_token");
            //handle sizing and position
            RectTransform trans = tok.AddComponent<RectTransform>();
            trans.transform.SetParent(canvas.transform);
            //move position
            trans.localScale = Vector3.one;
            trans.localPosition = new Vector2(0, -1000); //load offscreen
            //size of token
            trans.sizeDelta = new Vector2(25, 25);
            //handle image
            Image image = tok.AddComponent<Image>();
            Texture2D tex = Resources.Load<Texture2D>("coin_" + id);
            image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            tok.transform.SetParent(canvas.transform);
            token = tok;
        }
    }

    //Boardspace class
    public class BoardSpace {
        public float x;     //center x value
        public float y;     //center y value
        public double w;    //width
        public double h;    //height
        public Queue<Player> players_on_me = new Queue<Player>(); //players on this space
        public bool rest_square = false; //if rest space or not
        public int[] fast_travels;       //fast travel spots
        //add which transition scene to use?

        //Boardspace Constructor
        public BoardSpace(float center_x, float center_y, double width, double height, bool rest, int[] fast) {
            x = center_x;
            y = center_y;
            w = width;
            h = height;
            rest_square = rest;
            fast_travels = fast;
        }
    }

public class main : MonoBehaviour
{

    /* Online References
      2D Dice Roll:
        https://www.youtube.com/watch?v=JgbJZdXDNtg&ab_channel=AlexanderZotov
    */

    //Unity Variables

    //Setup Screen
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

    
    //Game Screen
    [SerializeField] GameObject game_screen;
    [SerializeField] Canvas canvas;
    [SerializeField] Scrollbar long_map_slider;
    [SerializeField] Button pause;
    

    //this is really jank but it works now
    //Fast Travel Arrays
    private static int[] no_fast = {};
    private static int[] space_two_fast = {0, 25, 0, 0, 0, 0};
    private static int[] space_four_fast = {0, 0, 0, 22, 0, 0};
    private static int[] space_six_fast = {7, 0, 10, 0, 27, 0};
    private static int[] space_seven_fast = {0, 10, 0, 15, 0, 27};
    private static int[] space_thirteen_fast = {22, 0, 0, 0, 0, 0};


    //Every single board space
    private static BoardSpace[] board = {
        new BoardSpace(220, -112, 112, 92, false, no_fast),
        new BoardSpace(133, -112, 88, 92, false, space_two_fast),
        new BoardSpace(44, -112, 115.5, 92, false, no_fast),
        new BoardSpace(-43, -112, 105, 92, false, space_four_fast),
        new BoardSpace(-127, -112, 85, 92, false, no_fast),
        new BoardSpace(-220, -112, 142, 92, false, space_six_fast),
        new BoardSpace(-241, -17, 93.5, 132, false, space_seven_fast),
        new BoardSpace(-241, 67, 83.5, 54.5, true, no_fast),
        new BoardSpace(-241, 114, 83.5, 84, false, no_fast),
        new BoardSpace(-241, 195, 74.5, 84, false, no_fast),
        new BoardSpace(-163, 195, 65, 84, true, no_fast),
        new BoardSpace(-95, 195, 65, 84, false, no_fast),
        new BoardSpace(-32, 195, 79, 84, false, space_thirteen_fast),
        new BoardSpace(39, 195, 88, 84, true, no_fast),
        new BoardSpace(116, 195, 82, 84, false, no_fast),
        new BoardSpace(199, 195, 82, 54.5, false, no_fast),
        new BoardSpace(199, 128, 82, 58, false, no_fast),
        new BoardSpace(199, 82, 82, 54, false, no_fast),
        new BoardSpace(199, 20, 82, 82.5, false, no_fast),
        new BoardSpace(199, -45, 82, 82.5, false, no_fast),
        new BoardSpace(119, -45, 70, 82.5, false, no_fast),
        new BoardSpace(44, -45, 64, 82.5, false, no_fast),
        new BoardSpace(-18, -45, 84, 84, false, no_fast),
        new BoardSpace(-93, -45, 82.5, 84, false, no_fast),
        new BoardSpace(-175, -45, 82.5, 51, false, no_fast),
        new BoardSpace(-175, 16, 82.5, 132, false, no_fast),
        new BoardSpace(-175, 108, 64, 86, false, no_fast),
        new BoardSpace(-102, 129, 82, 86, false, no_fast),
        new BoardSpace(-28, 129, 106.5, 83, false, no_fast),
        new BoardSpace(56, 129, 81, 113.5, false, no_fast),
        new BoardSpace(134, 101, 81, 70, false, no_fast),
        new BoardSpace(134, 17, 80, 70, false, no_fast),
        new BoardSpace(-20, 43, 308, 135, false, no_fast)
    };

    //Player Order and Information
    private static Player[] order = new Player[4];
    private static int curr_turn;
    private static int num_players;
    private Player player_one;
    private Player player_two;
    private Player player_three;
    private Player player_four;
    public static Player curr_player; 
    public static int fast_space;

    //flags
    public static bool dice_exit;
    public static bool next_scene;
    public static bool space_exit;
    private static bool move;
    public static bool fast_travel;
    public static bool paused;
    public static bool failed_fast;
    public static bool suceed_fast;

    //stuff for move
    int old_pos = 0;
    int next_pos = 0;
    int new_pos = 0;
    int square_pos = 0;
    const float pause_move_time = .2f;
    const float speed = 200f;

    // Start is called before the first frame update
    void Start() {
        one_player.onClick.AddListener(delegate{set_num_players(1);});
        two_player.onClick.AddListener(delegate{set_num_players(2);});
        three_player.onClick.AddListener(delegate{set_num_players(3);});
        four_player.onClick.AddListener(delegate{set_num_players(4);});

        go_to_order.onClick.AddListener(randomize_order);

        start_round.onClick.AddListener(start);

        pause.onClick.AddListener(pause_game);
        long_map_slider.value = 1;

        for (int i = 0; i < board.Length; i++) {
            board[i].players_on_me.Clear();
        }

        dice_exit = false;
        next_scene = false;
        space_exit = false;
        move = false;
        fast_travel = false;
        paused = false;
        failed_fast = false;
        
        // // //TODO temporary, take this out
        // player_one = new Player("test_name", 1, canvas);
        // player_two = new Player("test_name", 2, canvas);
        // player_three = new Player("test_name", 3, canvas);
        // player_four = new Player("test_name", 4, canvas);

        // BoardSpace curr_square = board[0];

        // //TODO ADD RANDOMIZE ORDER LATER
        // order[0] = player_one;
        // order[1] = player_two;
        // order[2] = player_three;
        // order[3] = player_four;

        // curr_player = order[0];

        // for (int i = 0; i < order.Length; i++) {
        //     curr_square.players_on_me.Enqueue(order[i]);
        // }
       
        // update_player_pos(curr_square);

        // load_dice();        
    }

    void set_num_players(int players) {
        Debug.Log("Number of Players!: " + players);
        setup_num_players.SetActive(false);
        for (int i = 0; i < players; i++) {
            setup_player_cols[i].SetActive(true);
        }
        setup_player_name_screen.SetActive(true);
        num_players = players;
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
        BoardSpace curr_square = board[0];
        for (int i = 0; i < num_players; i++) {
            string name = setup_player_names[used[i]].GetComponent<TMP_Text>().text;
            Debug.Log(name);
            order[i] = new Player(name, i + 1, canvas);
            show_name_order[i].GetComponent<TMP_Text>().text = name;
            curr_square.players_on_me.Enqueue(order[i]);
            show_player_order[i].SetActive(true);
            // Debug.Log("Player:" + setup_player_names[i].GetComponent<TMP_Text>().text);
        }
        player_one = order[0];
        player_two = order[1];
        player_three = order[2];
        player_four = order[3];

        curr_player = order[0];
   
        setup_player_order.SetActive(true);
    }

    void start() {
        setup_player_order.SetActive(false);
        update_player_pos(board[0]);
        curr_turn = -1;
        SceneManager.LoadSceneAsync(new_pos + 2, LoadSceneMode.Additive);
        game_screen.SetActive(true);

        // load_dice();        
    }




    // Update is called once per frame
    void Update() {
        //once the dice scene is exited
        if (dice_exit) {
            dice_exit = false;
            failed_fast = false;
            int curr_roll = dice.get_roll();
            int space = 0;
            if (fast_travel) { //handles fast travel
                int curr_pos = curr_player.curr_pos;
                space = board[curr_pos].fast_travels[curr_roll - 1];
                if (space != 0) {
                    curr_roll = (space - curr_pos) - 1;
                    fast_space = space;
                } else {
                    curr_roll = 0;
                }

            } 
            if (curr_roll != 0 && fast_travel) {
                fast_travel = false;
                SceneManager.LoadSceneAsync(37, LoadSceneMode.Additive);
                setup_move(curr_player, curr_roll);
                Invoke("fast_travel_success", 3f);
            } else if (curr_roll != 0) { 
                fast_travel = false;
                setup_move(curr_player, curr_roll);
                move = true;
            } else {
                fast_travel = false;
                failed_fast = true;
                next_roll();
                // Invoke("next_roll", 1f);
            }
        }

        //once a token is able to move
        if (move) {
            move_func();
        }


        //once a board space scene has exited
        if (space_exit) {
            space_exit = false;
            if (fast_travel) {
                load_dice();
            } else {
                next_roll();
            }
        }
    }

    void fast_travel_success() {
        // success_screen.SetActive(false);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        move = true;
    }

    private void move_func() {
        Vector2 next_target;
        if (next_pos == new_pos) { //if next position is final, adjust the movement to the specific corner depending on number of players
            next_target = get_next_vector(board[next_pos], square_pos);
        } else { // else move to center of each space
            next_target = new Vector2(board[next_pos].x, board[next_pos].y);
        }
        curr_player.token.transform.localPosition = Vector2.MoveTowards(curr_player.token.transform.localPosition, 
            next_target, speed * Time.deltaTime);
        if ((Vector2) curr_player.token.transform.localPosition == next_target) {
            move = false;
            if (next_pos == new_pos) { //reached target, load board space
                if (new_pos == 32) {
                    curr_player.reverse_path = true;
                } 
                if (!move) {
                    SceneManager.LoadSceneAsync(new_pos + 2, LoadSceneMode.Additive);
                }
            } else { //reached an internal board space, delay the next move and update next and old position 
                old_pos = next_pos;
                next_pos = curr_player.reverse_path ? next_pos - 1 : next_pos + 1;
                Invoke("delay_move", pause_move_time);
            }
        }
    }

    //Obtains the next vector given the next board space's coordinates and the current player's position on the target space.
    private Vector2 get_next_vector(BoardSpace space, int pos) {
        if (pos == 1) {
            return new Vector2(space.x - (float) (space.w / 5) , space.y + (float) (space.h / 5));
        } else if (pos == 2) {
            return new Vector2(space.x + (float) (space.w / 5) , space.y + (float) (space.h / 5));
        } else if (pos == 3) {
            return new Vector2(space.x - (float) (space.w / 5) , space.y - (float) (space.h / 5));
        } else if (pos == 4) {
            return new Vector2(space.x + (float) (space.w / 5) , space.y - (float) (space.h / 5));
        }
        return new Vector2(0, 0);
    }

    //Allow player to move again.
    private void delay_move() {
        if (!paused) {
            move = true;
        }
    }



    /*
        Given a board space, update the positions of every player on that board space.
    */
    static void update_player_pos(BoardSpace curr_square) {
        //TODO really gacky try and fix this
        Queue<Player> player_q = curr_square.players_on_me;
        int pos_count = 0;
        foreach (Player player in player_q) {
            GameObject player_token = player.token;
            if (pos_count == 0) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.x - (float) (curr_square.w / 5) , curr_square.y + (float) (curr_square.h / 5));
            } else if (pos_count == 1) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.x + (float) (curr_square.w / 5) , curr_square.y + (float) (curr_square.h / 5));
            } else if (pos_count == 2) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.x - (float) (curr_square.w / 5) , curr_square.y - (float) (curr_square.h / 5));
            } else if (pos_count == 3) {
                 player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.x + (float) (curr_square.w / 5) , curr_square.y - (float) (curr_square.h / 5));
            }
            pos_count++;
        }
    }

    //Sets up neccesary information about the player moving to the next location.
    private void setup_move(Player player, int move) {
        board[player.curr_pos].players_on_me.Dequeue(); //remove player off of current board space
        old_pos = player.curr_pos;
        next_pos = player.reverse_path ? player.curr_pos - 1 : player.curr_pos + 1;
        new_pos = player.reverse_path ? player.curr_pos -= move : player.curr_pos += move;
        if (new_pos >= 33) { //player reached center, must allow player to move back to the start 
            new_pos = 32;
            // player.reverse_path = true;
        } else if (new_pos <= 0) { //player has reached the end goal
            player.curr_pos = 0;
            //END GAME
        }
        Debug.Log("Player's Current Position On Board: " + (new_pos + 1));
        BoardSpace curr_square = board[new_pos];
        player.places_visited.Add(new_pos); //add board space to player's visited list
        if (curr_square.fast_travels.Length > 0 && !player.reverse_path) { //only allow fast travel on the way to Yokohama, not on the way back.
            Debug.Log("Fast Travel!");
            fast_travel = true;
        }
        curr_square.players_on_me.Enqueue(player); //add player onto new board space
        square_pos = curr_square.players_on_me.Count; //which corner of the new board space the player will land on
        if (curr_square.rest_square) { //check board space if player must lose turn 
            player.lose_a_turn = true;
        }
        player.curr_pos = new_pos;
    }

    //Gets the next player from the turn order and loads the dice.
    private void next_roll() {
        curr_turn++;
        if (curr_turn >= num_players) {
            curr_turn = 0;
        }
        curr_player = order[curr_turn];     
         if (curr_player.lose_a_turn) { //Skip a player
            while(curr_player.lose_a_turn) { //Find a player that isn't resting.
                Debug.Log("You Rested!");
                curr_player.lose_a_turn = false; //If find a player that is resting, make them unrested.
                curr_turn++;
                if (curr_turn >= num_players) {
                    curr_turn = 0;
                }
                curr_player = order[curr_turn];
            }
        }
        // Invoke("load_dice", .5f);   //Delays the loading of the dice by .5 seconds.
        load_dice();
    }

    //Loads the dice scene.
     private void load_dice() {
        SceneManager.LoadSceneAsync(35, LoadSceneMode.Additive);
    }

    //Set when the dice scene is exited in the dice class (dice.cs)
    public static void set_dice_exit(bool val) {
        dice_exit = val;
    }

    //Set when the board space scene is exited in the board space class (board_space.cs)
    //I just realized I named a class board_space and another class BoardSpace. Oops.
    public static void set_space_exit(bool val) {
        space_exit = val;
    }

    public static void set_move(bool val) {
        move = val;
    }

    private void pause_game() {
        set_pause(true);
        move = false;
        SceneManager.LoadSceneAsync(36, LoadSceneMode.Additive);
    }

    public static void set_pause(bool val) {
        paused = val;
    }

}