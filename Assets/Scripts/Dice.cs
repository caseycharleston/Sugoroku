using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    private static AudioSource dice_land, dice_shake, kabuki1, kabuki2, kabuki3, kabuki_full;
    public GameObject real_dice, fast_dice;
    private Sprite[] diceSides;
    private Image rend;
    public static bool coroutineAllowed = true;
    public GameObject dramatic_camera;

    //used for debug, force a seed of dice rolls (includes fast travel dice rolls if fast travel is not turned off)
    // private int[] debug_rolls = {2, 32, 32, 32, 1, 1, 32, 1, 1};
    // private int debug_index = 0;

	// Use this for initialization
	private void Start() {
        dice_land = GameObject.Find("DiceLand").GetComponent<AudioSource>();
        dice_shake = GameObject.Find("DiceShake").GetComponent<AudioSource>();
        kabuki1 = GameObject.Find("KabukiP1").GetComponent<AudioSource>();
        kabuki2 = GameObject.Find("KabukiP2").GetComponent<AudioSource>();
        kabuki3 = GameObject.Find("KabukiP3").GetComponent<AudioSource>();
        kabuki_full = GameObject.Find("KabukiFull").GetComponent<AudioSource>();

        rend = GetComponent<Image>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
	}

    private void Update() {
        if (Input.GetKeyDown("space")) {
            if (coroutineAllowed) {
                StartCoroutine("RollTheDice");
            }
        }
    }

    private void OnMouseDown()
    {
        if (coroutineAllowed) {
            StartCoroutine("RollTheDice");
        }
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        if (GameControl.fast_travel) {
            GameControl.brain.m_DefaultBlend.m_Time = 5; // 0 Time equals a cut
            dramatic_camera.SetActive(true);
            dice_shake.loop = true;
            dice_shake.Play();
            kabuki1.Play();
            rend = fast_dice.GetComponent<Image>();
            // kabuki_full.Play();
            for (int i = 0; i <= 95; i++) {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
            dice_shake.loop = false;
            dice_shake.Stop();
            kabuki2.Play();
        } else {
            dice_shake.Play();
            rend = real_dice.GetComponent<Image>();
            for (int i = 0; i <= 25; i++) {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
        }
        // Debug.Log("Rolled: " + (randomDiceSide + 1));
        GameControl.diceSideThrown = randomDiceSide + 1;
        // GameControl.diceSideThrown = 32; //DEBUG: force the dice roll value
        // GameControl.diceSideThrown = debug_rolls[debug_index]; debug_index++; //  DEBUG force a dice roll seed
        dice_land.Play();
        yield return new WaitForSeconds(1f);
     
        if (GameControl.fast_travel) {
            GameControl.brain.m_DefaultBlend.m_Time = 0; // 0 Time equals a cut
            dramatic_camera.SetActive(false);
            yield return new WaitForSeconds(1f);
            kabuki3.Play();
            GameControl.brain.m_DefaultBlend.m_Time = GameControl.zoom_speed; // 0 Time equals a cut
        }
        GameControl.MovePlayer();
        coroutineAllowed = true;
    }
}

