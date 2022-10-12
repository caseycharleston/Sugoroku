using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    /*
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

    private Player player_one;
    private int roll;

    //Classes
    class Player {
        public int cur_pos = 0;
        public string name;
        //public Sprite/Image character;
        public GameObject token;

         public Player(string player_name, GameObject player_token) {
            name = player_name;
            token = player_token;
        }
    }

    //make an array for the boardgame 
    //

   


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

        //Create Player_One token
        GameObject token = new GameObject("player_token");
        //handle sizing and position
        RectTransform trans = token.AddComponent<RectTransform>();
        trans.transform.SetParent(canvas.transform);
        //move position
        trans.localPosition = new Vector2(220, -110);
        trans.localScale = Vector3.one;
        //size of token
        trans.sizeDelta = new Vector2(37, 37);
        //handle image
        Image image = token.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>("coin");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        token.transform.SetParent(canvas.transform);
        
        player_one = new Player("test_name", token);
        
    }

    // Update is called once per frame
    void Update() {
        
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
        // RectTransform trans = player_one.token.GetComponent<RectTransform>();
        // trans.localPosition = new Vector2(trans.localPosition.x - (75 * roll), -110);
        move_pos(player_one, roll);
    }

     void move_pos(Player player, int move) {
        player.cur_pos += move; //TODO don't forget this
        Debug.Log("Player's Current Position: " + player.cur_pos);
        int x = 0;
        switch(player.cur_pos) {
            case 1:
                x = 135;
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                break;
            case 2:
                x = 45; break;
            case 3:
                x = -46; break;
            case 4: 
                x = -126; break;
            case 5:
                x = -222; break;
            default:
                x = 220;
                player.cur_pos = 0;
                break;
        }
        player_one.token.GetComponent<RectTransform>().localPosition = new Vector2(x, -110);

    }

}