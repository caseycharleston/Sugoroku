using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string player_name;         //name of player
    public int id;              //player order id

    public int curr_pos;    //current board position
    public int curr_place;    //current race placing 
    // public HashSet<int> places_visited = new HashSet<int>();    //board spaces already visited
    public bool reverse_path;   //if player must go backwards
    public bool lose_a_turn;    //if player lost a turn
    // Start is called before the first frame update
    void Start() {
        // places_visited.Clear();
        reverse_path = false;
        lose_a_turn = false;
        curr_pos = 0;
        curr_place = id - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
