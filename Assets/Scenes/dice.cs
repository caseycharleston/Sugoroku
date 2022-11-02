using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class dice : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_box;
    [SerializeField] Button dice_button;

      // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;
    // Reference to sprite renderer to change sprites
    private Image rend;

    Player curr_player = main.curr_player;
    bool enable_click;
    public static bool paused;

    public static int roll = 0;
    // Start is called before the first frame update
    void Start() {
        enable_click = false;
        if (main.failed_fast) {
            text_box.GetComponent<TMP_Text>().text = "You failed...";
            Invoke("after_fail", 3f);
        } else {
            if (main.fast_travel) {
                text_box.GetComponent<TMP_Text>().fontSize = 30;
                text_box.GetComponent<TMP_Text>().text = "You landed on a fast travel spot! Roll again!";
            } else {
                text_box.GetComponent<TMP_Text>().fontSize = 45;
                text_box.GetComponent<TMP_Text>().text = curr_player.name + " GO!"; //Sets Player Starting Text
            }
            enable_click = true;
        }
        
        dice_button.GetComponent<Button>();
        dice_button.onClick.AddListener(click_dice);

         // Assign Renderer component
        rend = dice_button.GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("Dice/");
        rend.sprite = diceSides[0]; 
        paused = false;
    }

    private void after_fail() {
        text_box.GetComponent<TMP_Text>().fontSize = 45;
        text_box.GetComponent<TMP_Text>().text = curr_player.name + " GO!"; //Sets Player Starting Text
        enable_click = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

     private void click_dice() {
        if (enable_click && !paused) { //only allow click once per scene load
            enable_click = false;
            StartCoroutine("dice_roll");
        }
    }

     private IEnumerator dice_roll() {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

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
        if (paused) {
            enable_click = true;
        } else {
            StartCoroutine(exit_dice()); //Could also call Invoke but eh
        }
    }

    public static void set_pause(bool value) {
        paused = value;
    }

    //Returns the roll obtained from rolling the dice.
    public static int get_roll() {
        return roll;
    }

    private IEnumerator exit_dice() {
         yield return new WaitForSeconds(1f);
         if (paused) {
            enable_click = true;
         } else {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            main.set_dice_exit(true);
         }
    }

}
