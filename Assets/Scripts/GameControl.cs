using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    // variable for storing last tile landed on for board tile info scene loading
    public static int lastRollWayPoint = 0;
    public bool[] visited = new bool[60];

    public static bool gameOver = false;
    public Transform[] waypoints;

    // angle to zoom for camera movement
    private int[] direction = 
        {
            0, 0, 0, 0, 0, // tiles 1-5
            -90, -90, -90, -90, -90, // tiles 6-10
            180, 180, 180, 180, 180, 180, 180, // tiles 11-17
            90, 90, 90, 90, // tiles 18-21
            0, 0, 0, 0, 0, 0, // tiles 22-27
            -90, -90, -90, // tiles 28-30
            180, 180, 180, 180, 180, 180, 180, // tiles 31-37
            90, 90, 90, // tiles 38-40
            0, 0, 0, 0, 0, 0, // tiles 41-46
            -90, -90, // tiles 47-48
            180, 180, 180, 180, 180, // tiles 49-53
            90, 90, 90, 90, 90, 90, // tiles 54-59
            0 // tile 60
        };

        private float speed = 1.25f;
        private float speen = 20.0f;

    // Use this for initialization
    void Start () {

        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            Vector3 newPos = new Vector3(waypoints[lastRollWayPoint].transform.position.x, waypoints[lastRollWayPoint].transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, speed * Time.fixedDeltaTime); // move camera to center at waypoint
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(0.0f, 0.0f, direction[0]), speed * Time.fixedDeltaTime); // rotate camera
            while (Camera.main.fieldOfView >= 10.0f){
                Camera.main.fieldOfView -= speen * Time.deltaTime; // zoom in 
            }
            // if (!visited[player1StartWaypoint]) {
            //     visited[player1StartWaypoint] = true;
            //     Initiate.Fade("Base_Tile", Color.black, 1f);
            // } else {
            //     // ask player if they wanna see it again
            // }
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex >= 
            player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex >=
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
}
