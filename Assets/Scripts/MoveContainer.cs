using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveContainer : MonoBehaviour
{
    public Transform[] wp;
    public bool move_allowed;
    public int start_index;
    public int wp_index;
    // Start is called before the first frame update
    void Start()
    {
        move_allowed = false;
        wp_index = start_index;
        transform.position = wp[wp_index].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (move_allowed)
            Move();
    }

    void Move() {
        transform.position = Vector2.MoveTowards(transform.position, wp[wp_index].transform.position, 12f * Time.deltaTime);
        if (transform.position == wp[wp_index].transform.position) {
            move_allowed = false;
        }
    }
}
