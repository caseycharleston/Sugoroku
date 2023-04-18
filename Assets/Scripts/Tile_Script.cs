using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile_Script : MonoBehaviour
{

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
    private int pageIndex = 0;

    void Start()
    {
        close.onClick.AddListener(leave); // listener for close button
        forwardArrow.onClick.AddListener(()=>{pageIndex++;});
        backArrow.onClick.AddListener(()=>{pageIndex--;});
        // invoke TSV reader
        CSVReader test = gameObject.AddComponent<CSVReader>() as CSVReader;
        test.ReadCSV();
        Debug.Log("querying for " + GameControl.lastRollWayPoint);
        tile = test.queryTile(GameControl.lastRollWayPoint); // get the tile loaded

        // play music associated with the tile
        AudioSource music = gameObject.AddComponent<AudioSource>(); // add audio object
        AudioClip clip = (AudioClip) (Resources.Load("audioTile" + GameControl.lastRollWayPoint)); // get the AudioClip
        music.PlayOneShot(clip); // play music

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
        Debug.Log("In awake in Tile_Script");
        Image tileImg = tileImage.GetComponent<Image>();
        Debug.Log("GameControl.lastRollWayPoint = " + GameControl.lastRollWayPoint + " for image get");
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
    }

    void leave()
    {
        Initiate.Fade("GameBoard", Color.black, 1f);
    }
}
