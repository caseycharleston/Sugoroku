using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tile_Script : MonoBehaviour
{
    // public GameControl GC = new GameControl();

    public Tile tile;

    public GameObject japaneseTitle;
    public GameObject translatedTitle;
    public GameObject pageText;
    public GameObject japaneseRule;
    public GameObject translatedRule;
    public GameObject tileImage;
    public GameObject bookImage;

    [SerializeField] Button close;
    [SerializeField] Button forwardArrow;
    [SerializeField] Button backArrow;
    [SerializeField] Button imageForwardArrow;
    [SerializeField] Button imageBackArrow;
    private int pageIndex = 0;
    private int imageIndex = 0;

    void Start()
    {
        close.onClick.AddListener(leave); // listener for close button
        forwardArrow.onClick.AddListener(()=>{pageIndex++;});
        backArrow.onClick.AddListener(()=>{pageIndex--;});
        imageForwardArrow.onClick.AddListener(()=>{imageIndex++;});
        imageBackArrow.onClick.AddListener(()=>{imageIndex--;});       
        // invoke TSV reader
        CSVReader test = gameObject.AddComponent<CSVReader>() as CSVReader;
        test.ReadCSV();
        Debug.Log("querying for " + GameControl.lastRollWayPoint);
        tile = test.queryTile(GameControl.lastRollWayPoint); // get the tile loaded

        // play music associated with the tile
        AudioSource music = gameObject.AddComponent<AudioSource>(); // add audio object
        AudioClip clip = (AudioClip) (Resources.Load("audioTile" + GameControl.lastRollWayPoint)); // get the AudioClip
        if (clip != null) {
             music.PlayOneShot(clip); // play music
        }
       
        TextMeshProUGUI jpTitle = japaneseTitle.GetComponent<TextMeshProUGUI>();
        jpTitle.text = tile.origText[0];

        TextMeshProUGUI enTitle = translatedTitle.GetComponent<TextMeshProUGUI>();
        enTitle.text = tile.transText[0];

        TextMeshProUGUI jpSpecialRule = japaneseRule.GetComponent<TextMeshProUGUI>();
        jpSpecialRule.text = tile.origSpecialRule;

        TextMeshProUGUI enSpecialRule = translatedRule.GetComponent<TextMeshProUGUI>();
        enSpecialRule.text = tile.transSpecialRule;
    }

    void Awake() {
        // update image based on the queried tile
        Image tileImg = tileImage.GetComponent<Image>();
        tileImg.sprite = Resources.Load<Sprite>("ChinaIncidentPieces/tile" + GameControl.lastRollWayPoint);
    }

    void Update()
    {
        bool showBackArrow = pageIndex != 0;
        backArrow.gameObject.SetActive(showBackArrow); // only set false if pageIndex = 0
        bool showForwardArrow = pageIndex != tile.histNotes.Count - 1;
        forwardArrow.gameObject.SetActive(showForwardArrow); // only set false if pageIndex = length of histNotes array - 1

        // update text based on pageIndex
        TextMeshProUGUI bookText = pageText.GetComponent<TextMeshProUGUI>();
        bookText.text = tile.histNotes[pageIndex];

        // Arrow visibility logic for images
        bool showImageBackArrow = imageIndex != 0;
        imageBackArrow.gameObject.SetActive(showImageBackArrow);
        bool showImageForwardArrow = imageIndex != tile.images.Count - 1 && tile.images.Count != 0;
        imageForwardArrow.gameObject.SetActive(showImageForwardArrow);

        Image bookImg = bookImage.GetComponent<Image>();
        // Checks if the current tile has images to load
        // If length == 0, this tile does not have images associated with it
        if (tile.images.Count != 0) {
            Debug.Log(tile.images[imageIndex]);
            bookImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(tile.images[imageIndex]);
        } else {
            bookImg.gameObject.SetActive(false);
        }
    }

    void leave()
    {
        // Initiate.Fade("GameBoard", Color.black, 1f);
        GameControl.dice.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        if (GameControl.isPlayer1Turn) {
            GameControl.isPlayer1Turn = false;
            GameControl.CheckSpecialAfter(GameControl.player1StartWaypoint, 1);
        } else {
            GameControl.CheckSpecialAfter(GameControl.player2StartWaypoint, 2);
        }
        
    }
}
