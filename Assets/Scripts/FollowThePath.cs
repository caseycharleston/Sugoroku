using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FollowThePath : MonoBehaviour {

    private static AudioSource move_sfx;
    public int player_id;
    public Transform[] tl_waypoints;
    public Transform[] tr_waypoints;
    public Transform[] bl_waypoints;
    public Transform[] br_waypoints;
    public Transform[] waypoints;
    [HideInInspector] public List<Transform[]> wp = new List<Transform[]>();

    [SerializeField] private float moveSpeed = 1f;
    [HideInInspector] public int waypointIndex = 0;
    public bool moveAllowed = false;
    const float pause_move_time = .2f;

	// Use this for initialization
	private void Start () {
        move_sfx = GameObject.Find("PlayerMove").GetComponent<AudioSource>();
        moveAllowed = false;
        wp.Clear();
        wp.Add(tl_waypoints); wp.Add(tr_waypoints); wp.Add(bl_waypoints); wp.Add(br_waypoints);
        if (!GameControl.debug) {
            if (player_id <= GameControl.num_players) {
                transform.position = wp[player_id - 1][waypointIndex].transform.position;
            }
        } else {
            transform.position = wp[player_id - 1][waypointIndex].transform.position;
        }
        waypointIndex = 1;
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed)
            Move();
	}

    private void Move() {
        if (waypointIndex < tl_waypoints.Length) {
            transform.position = Vector2.MoveTowards(transform.position,
            wp[GameControl.square_pos - 1][waypointIndex].transform.position, moveSpeed * Time.deltaTime);
            if (transform.position ==  wp[GameControl.square_pos - 1][waypointIndex].transform.position) {
                if (GetComponent<PlayerInfo>().reverse_path) {
                    waypointIndex -= 1;
                } else {
                    waypointIndex += 1;
                }
                moveAllowed = false;
                move_sfx.Play();
                Invoke("delay_move", pause_move_time);
            }
        }
    }

     //Allow player to move again.
    private void delay_move() {
            if (!GameControl.stop_move) {
                moveAllowed = true;
            }
            int offset = GetComponent<PlayerInfo>().reverse_path ? 1 : -1;
            if ((waypointIndex + offset) == GameControl.new_pos) {
                GameControl.finish_move = true;
            } 
    }

}
