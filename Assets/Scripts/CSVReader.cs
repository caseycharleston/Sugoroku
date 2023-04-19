using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVReader : MonoBehaviour
{


    // Class for storing data for each game tile
    

    // Stores all entries in the spreadsheet
    // Row - data for one tile
    // Column - different types of text (original, translated, historical)
    public string[][] database;

    public Tile myTile;

    void Start()
    {
        ReadCSV();
        for (int i = 0; i < database.Length; i++) 
        {
            Debug.Log("Tile " + (i));
            Debug.Log(String.Format("[{0}]", string.Join(", ", database[i]))); 
        }
    }

    public void ReadCSV()
    {
        string fileName = @"./Assets/Resources/cropped_data.tsv";
        string[] lines = File.ReadAllLines(fileName); // Read every line in spreadsheet based on carriage return
        database = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            // Split tile data (row of spreadsheet) by tab and store into new array
            string[] tileData = lines[i].Split("\t");
            database[i] = tileData;
        }
    }

    public Tile queryTile(int i) {
        Debug.Log(String.Format("[{0}]", string.Join(", ", database[i]))); 
        Tile shownTile = new Tile();
        shownTile.origText = new List<string>();
        shownTile.transText = new List<string>();
        shownTile.histNotes = new List<string>();
        shownTile.images = new List<string>();
        // makes the text for special rule equal to special rule text if it exists. Otherwise, sets it to nothing
        shownTile.origSpecialRule = !(database[i][3].Equals("NA")) ? database[i][3] : "";
        shownTile.transSpecialRule = !(database[i][7].Equals("NA")) ? database[i][7] : "";
        // Max size of each array is 3
        for (int j = 0; j < 3; j++) 
        {
            // Only add text to list if it does not equal the default value (NA), indicating no text exists
            if (!database[i][j].Equals("NA")) {shownTile.origText.Add(database[i][j]); }
            if (!database[i][j + 4].Equals("NA")) {shownTile.transText.Add(database[i][j + 4]); }
            if (!database[i][j + 8].Equals("NA")) {shownTile.histNotes.Add(database[i][j + 8]); }
        }
        string fileGetter = "#" + i + ".*";
        string[] filePaths = Directory.GetFiles("Assets/Resources/BoardImages/", fileGetter,
                                         SearchOption.TopDirectoryOnly);
        foreach (string file in filePaths) 
        {
            // Checks if the file in the directory is a meta image and ignores it
            // Debug.Log(file.Substring(file.Length - 4));
            if (!file.Substring(file.Length - 4).Equals("meta"))
            {
                Debug.Log(file.Substring(17, 16));
                // The first number removes "Assets/Resources/" from the file string. The end number
                // effectively removes ".jpg" or ".png" from the end of the string. This is necessary
                // For the image sprite to load properly in Tile_Script line 90.
                shownTile.images.Add(file.Substring(17, 16));
            }
        }
        return shownTile;
    }
}

    public class Tile 
    {
        public List<string> origText; // japanese text on tile
        public List<string> transText; // translation of text on tile (one-to-one w/ japText)
        public string origSpecialRule; // japanese text for special rules (skip turn, move to x tile, etc)
        public string transSpecialRule; // translated version of above
        public List<string> histNotes; // historical notes levels 1 - 3
        public List<string> images; // store file names for related images
    }
