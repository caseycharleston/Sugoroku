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
        Tile shownTile = new Tile();
        shownTile.origText = new string[4];
        shownTile.transText = new string[4];
        shownTile.histNotes = new string[3];
        for (int j = 0; j < shownTile.origText.Length; j++) 
        {
            shownTile.origText[j] = database[i][j + 1];
            shownTile.transText[j] = database[i][j + 5];
        }
        for (int j = 0; j < shownTile.histNotes.Length; j++) {
            shownTile.histNotes[j] = database[i][j + 9];
        }
        return shownTile;
    }
}

[System.Serializable]
    public class Tile 
    {
        public string[] origText; // japanese text on tile
        public string[] transText; // translation of text on tile (one-to-one w/ japText)
        public string[] histNotes; // historical notes levels 1 - 3
    }
