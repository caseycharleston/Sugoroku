using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

     private static AudioSource dice_land, dice_shake;

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    public static bool coroutineAllowed = true;
    public GameObject dramatic_camera;

	// Use this for initialization
	private void Start() {
        dice_land = GameObject.Find("DiceLand").GetComponent<AudioSource>();
        dice_shake = GameObject.Find("DiceShake").GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
	}

    private void Update() {
        if (Input.GetKeyDown("space")) {
            StartCoroutine("RollTheDice");
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
        Debug.Log("oh no");
        dice_shake.Play();
        int randomDiceSide = 0;
        if (GameControl.fast_travel) {
            GameControl.brain.m_DefaultBlend.m_Time = 10; // 0 Time equals a cut
            dramatic_camera.SetActive(true);
            for (int i = 0; i <= 200; i++) {
                 randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
        } else {
            for (int i = 0; i <= 25; i++) {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
        }
        Debug.Log("Rolled: " + (randomDiceSide + 1));
        GameControl.diceSideThrown = randomDiceSide + 1;
        // GameControl.diceSideThrown = 17; //DEBUG: force the dice roll value
        dice_land.Play();
        yield return new WaitForSeconds(1f);
        if (GameControl.fast_travel) {
            GameControl.brain.m_DefaultBlend.m_Time = 0; // 0 Time equals a cut
            dramatic_camera.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            GameControl.brain.m_DefaultBlend.m_Time = 3; // 0 Time equals a cut
        }
        GameControl.MovePlayer();
        whosTurn *= -1;
        coroutineAllowed = true;
    }
}
