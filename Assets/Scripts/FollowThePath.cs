using UnityEngine;

public class FollowThePath : MonoBehaviour {

    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 2f; //for the 16 code for the value of the speed

    [HideInInspector] //need to be reviewed and fixed
    public int waypointIndex = 0; // waypoint setting value

    public bool moveAllowed = false; 

	// Use this for initialization
	private void Start () {
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed)
            Move();
	}

    private void Move()
    {
        if (waypointIndex < waypoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }
}
