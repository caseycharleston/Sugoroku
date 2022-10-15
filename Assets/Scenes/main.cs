using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    /* Online References
      2D Dice Roll:
        https://www.youtube.com/watch?v=JgbJZdXDNtg&ab_channel=AlexanderZotov
    */

    //Instance Variables
    [SerializeField] Canvas canvas;
    [SerializeField] Scrollbar long_map_slider;
    [SerializeField] Button dice;

     // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private Image rend;

    private BoardSpace[] board = {
        new BoardSpace(220, -112, 112, 92),
        new BoardSpace(133, -112, 88, 92),
        new BoardSpace(44, -112, 115.5, 92),
        new BoardSpace(-43, -112, 105, 92),
        new BoardSpace(-127, -112, 85, 92),
        new BoardSpace(-220, -112, 142, 92),
        new BoardSpace(-241, -17, 93.5, 132)
    };

    private Player[] order = new Player[4];
    private int curr_turn = 0;
    private Player player_one;
    private Player player_two;
    private Player player_three;
    private Player player_four;

    private int roll;

    //Classes
    class Player {
        public GameObject token;
        public string name;
        public int id;

        public int curr_pos = 0;        
        public bool reverse_path = false;

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

    class BoardSpace {
        public float center_x;
        public float center_y;
        public double width;
        public double height;
        public Queue<Player> players_on_me = new Queue<Player>();
        public bool lose_a_turn = false;
        //add which transition scene to use?

        public BoardSpace(float x, float y, double w, double h) {
            center_x = x;
            center_y = y;
            width = w;
            height = h;
        }
    }
   

    // Start is called before the first frame update
    void Start() {
        
        long_map_slider.value = 1;

        dice.GetComponent<Button>();
        dice.onClick.AddListener(click_dice);

         // Assign Renderer component
        rend = dice.GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("Dice/");
        rend.sprite = diceSides[0]; 

        //TODO temporary, take this out
        player_one = new Player("test_name", 1, canvas);
        player_two = new Player("test_name", 2, canvas);
        player_three = new Player("test_name", 3, canvas);
        player_four = new Player("test_name", 4, canvas);
        BoardSpace curr_square = board[0];

        //TODO ADD RANDOMIZE ORDER LATER
        order[0] = player_one;
        order[1] = player_two;
        order[2] = player_three;
        order[3] = player_four;

        //TODO YOU NEED TO ENQUEUE IN THE CORRECT RANDOMIZED ORDER AT THE START OF THE GAME FOR THE LOGIC TO WORK
        curr_square.players_on_me.Enqueue(player_one);
        curr_square.players_on_me.Enqueue(player_two);
        curr_square.players_on_me.Enqueue(player_three);
        curr_square.players_on_me.Enqueue(player_four);
        update_player_pos(curr_square);
        
    }


    // Update is called once per frame
    void Update() {
        
    }

    /*
        Given a board space, update the positions of every player on that board space.
    */
    void update_player_pos(BoardSpace curr_square) {
        //TODO really gacky try and fix this
        Queue<Player> player_q = curr_square.players_on_me;
        int pos_count = 0;
        foreach (Player player in player_q) {
            GameObject player_token = player.token;
            if (pos_count == 0) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.center_x - (float) (curr_square.width / 5) , curr_square.center_y + (float) (curr_square.height / 5));
            } else if (pos_count == 1) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.center_x + (float) (curr_square.width / 5) , curr_square.center_y + (float) (curr_square.height / 5));
            } else if (pos_count == 2) {
                player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.center_x - (float) (curr_square.width / 5) , curr_square.center_y - (float) (curr_square.height / 5));
            } else if (pos_count == 3) {
                 player_token.GetComponent<RectTransform>().localPosition = new Vector2(curr_square.center_x + (float) (curr_square.width / 5) , curr_square.center_y - (float) (curr_square.height / 5));
            }
            pos_count++;
        }
    }

      private void click_dice()
    {
        StartCoroutine("dice_roll");
    }

    // Coroutine that rolls the dice
    private IEnumerator dice_roll()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 6 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        roll = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log("Dice Roll: " + roll);

        //dynamic player movement
        Player curr_player = order[curr_turn];
        curr_turn++;
        if (curr_turn >= 4) {
            curr_turn = 0;
        }
        move_pos(curr_player, roll);
    }

     void move_pos(Player player, int move) {
        board[player.curr_pos].players_on_me.Dequeue();
        //TODO uncomment the line below if you want to update the space before moving as well
        // update_player_pos(board[player.curr_pos]);
        player.curr_pos = player.reverse_path ? player.curr_pos -= move : player.curr_pos += move;
        Debug.Log("Player's Current Position On Board: " + (player.curr_pos + 1));
        BoardSpace curr_square = board[player.curr_pos];
        curr_square.players_on_me.Enqueue(player);
        update_player_pos(board[player.curr_pos]);

        // SceneManager.LoadSceneAsync(player.curr_pos + 1, LoadSceneMode.Additive);
        // SceneManager.LoadSceneAsync(player.curr_pos + 1, LoadSceneMode.Single);
 

    }

}