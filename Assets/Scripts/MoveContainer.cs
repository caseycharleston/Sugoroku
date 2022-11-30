using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveContainer : MonoBehaviour
{
    public Transform[] wp;
    public bool move_allowed;
    // Start is called before the first frame update
    void Start()
    {
        move_allowed = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move_allowed) 
            Move();
    }

    void Move() {
        transform.position = Vector2.MoveTowards(transform.position, wp[0].transform.position, 5f * Time.deltaTime);
    }
}
