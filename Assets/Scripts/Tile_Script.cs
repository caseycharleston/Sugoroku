using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile_Script : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Tile tile;
    public TextMeshProUGUI title;
    
    void Start()
    {
        CSVReader test = gameObject.AddComponent<CSVReader>() as CSVReader;
        test.ReadCSV();
        tile = test.queryTile(2);
        title = gameObject.GetComponent<TextMeshProUGUI>();
        title.text = tile.transText[1];
        Debug.Log("This is the thing" + tile.transText[1]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
