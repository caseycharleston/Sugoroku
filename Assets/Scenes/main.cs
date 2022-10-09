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
    [SerializeField] Scrollbar long_map_slider;
    [SerializeField] Button dice;

     // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private Image rend;


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
        int finalSide = 0;

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
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log("Dice Roll: " + finalSide);
    }

}
