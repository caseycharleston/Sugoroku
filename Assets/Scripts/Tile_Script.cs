using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile_Script : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Tile tile;

    public GameObject japaneseTitle;
    public GameObject translatedTitle;
    public GameObject levelOne;
    public GameObject levelTwo;
    public GameObject rulesText;
    public GameObject resources;
    public GameObject tileImage;
    public GameObject resourceImage;

    
    void Start()
    {
        // invoke TSV reader
        CSVReader test = gameObject.AddComponent<CSVReader>() as CSVReader;
        test.ReadCSV();
        tile = test.queryTile(2); // get the tile loaded
        TextMeshProUGUI jpTitle = japaneseTitle.GetComponent<TextMeshProUGUI>();
        jpTitle.text = tile.origText[1];

        TextMeshProUGUI enTitle = translatedTitle.GetComponent<TextMeshProUGUI>();
        enTitle.text = tile.transText[1];

        TextMeshProUGUI levelOneTxt = levelOne.GetComponent<TextMeshProUGUI>();
        levelOneTxt.text = tile.histNotes[2];

        TextMeshProUGUI levelTwoTxt = levelTwo.GetComponent<TextMeshProUGUI>();
        levelTwoTxt.text = tile.histNotes[3];

        // For images it seems like we'd have to get a variable of every image
        // then load the correct one


        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
