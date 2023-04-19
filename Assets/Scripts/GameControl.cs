using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    // variable for storing last tile landed on for board tile info scene loading
    public static int lastRollWayPoint = 0;
    public Transform[] waypoints;
    public bool[] visited = new bool[60];

    public static bool gameOver = false;
    

    // angle to zoom for camera movement
    private static int[] direction = 
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

    private static float speed = 0.8f;

    private static int theWayPoint;

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
        StartCoroutine(cameraZoom());
        /*if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
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
        }*/
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

    /*private static void MoveCamera(int lastRollWayPoint) {
        Transform camPos = Camera.main.transform;
        Debug.Log(typeof(waypoints));

        
        Debug.Log(waypoints[lastRollWayPoint].transform.position.x);
        Debug.Log(waypoints[lastRollWayPoint].transform.position.y);
        Debug.Log(camPos.z);
        Vector3 newPos = new Vector3(waypoints[lastRollWayPoint].transform.position.x, waypoints[lastRollWayPoint].transform.position.y, camPos.z);
        camPos = Vector3.Lerp(camPos, newPos, speed * Time.fixedDeltaTime); // move camera to center at waypoint
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(0.0f, 0.0f, direction[lastRollWayPoint]), speed * Time.fixedDeltaTime); // rotate camera
        while (Camera.main.fieldOfView >= 10.0f){
            Camera.main.fieldOfView -= speen * Time.deltaTime; // zoom in 
        }
    }*/

    private bool doneMoving = false;
    private float zoomPercentage = 0.0f;
    private float movePercentage = 0.0f;
    private float rotatePercentage = 0.0f;
    private IEnumerator cameraZoom() {
        yield return new WaitUntil(() => moveyPart());
        Transform camPos = Camera.main.transform;
        Vector3 newPos = new Vector3(waypoints[theWayPoint - 1].transform.position.x, waypoints[theWayPoint - 1].transform.position.y, camPos.position.z);
        camPos.position = Vector3.MoveTowards(camPos.position, newPos, movePercentage);//Vector3.Lerp(camPos.position, newPos, movePercentage); 
        // move camera to center at waypoint
        Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(0.0f, 0.0f, direction[theWayPoint - 1]), rotatePercentage); // rotate camera
        Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 10, zoomPercentage);
        if (Camera.main.fieldOfView == 10) {
            doneMoving = false;
            zoomPercentage = 0.0f;
            movePercentage = 0.0f;
            rotatePercentage = 0.0f;
            visited[theWayPoint] = true;
            yield return new WaitForSecondsRealtime(0.5f);
            Initiate.Fade("Base_Tile", Color.black, 1f);
        }
        zoomPercentage = zoomPercentage <= 1 
        ? zoomPercentage + 0.000275f
        : 1;
        movePercentage = movePercentage <= 1 
        ? movePercentage + 0.0005f
        : 1;
        rotatePercentage = rotatePercentage <= 1 
        ? rotatePercentage + 0.00075f
        : 1;

    }

    private bool moveyPart() {
        if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            if (!visited[player1StartWaypoint]) {
                theWayPoint = lastRollWayPoint;
                doneMoving = true;
                return true;
            //     visited[player1StartWaypoint] = true;
            //     Initiate.Fade("Base_Tile", Color.black, 1f);
            // } else {
            //     // ask player if they wanna see it again
            }
            
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            if (!visited[player2StartWaypoint]) {
                theWayPoint = lastRollWayPoint;
                doneMoving = true;
                return true;
            }
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
        return doneMoving;
    }
}
