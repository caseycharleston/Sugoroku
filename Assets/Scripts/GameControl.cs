using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public bool isPlayer1Turn = true;

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


    private static int theWayPoint;

    private Vector3 origCameraVector;
    private Quaternion origCameraRotation;
    private float origCameraZoom;
    

    // Use this for initialization
    void Start () {
        visited[0] = true;
        origCameraVector = Camera.main.transform.position;
        origCameraRotation = Camera.main.transform.rotation;
        origCameraZoom = Camera.main.fieldOfView;



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
            SceneManager.LoadSceneAsync("Base_Tile", LoadSceneMode.Additive);
            Camera.main.transform.position = origCameraVector;
            Camera.main.transform.rotation = origCameraRotation;
            Camera.main.fieldOfView = origCameraZoom;
        }
        zoomPercentage = zoomPercentage <= 1 
        ? zoomPercentage + 0.000275f
        : 1;
        movePercentage = movePercentage <= 1 
        ? movePercentage + 0.0005f
        : 1;
        rotatePercentage = rotatePercentage <= 1 
        ? rotatePercentage + 0.001f
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
                isPlayer1Turn = true;
                return true;
            } else {
                CheckSpecialAfter(player1StartWaypoint, 1);
            }
            
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
            lastRollWayPoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
            if (!visited[player2StartWaypoint]) {
                theWayPoint = lastRollWayPoint;
                doneMoving = true;
                return true;
            } else {
                CheckSpecialAfter(player2StartWaypoint, 2);
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

    public void CheckSpecialAfter(int playerWaypoint, int player)
    {
        switch (playerWaypoint) {
            case 4:
                AdjustStatus(player, 0);
                break;
            case 7:
                AdjustStatus(player, 17);
                break;
            case 8:
                AdjustStatus(player, 14);
                break;
            case 11:
                AdjustStatus(player, 18);
                break;
            case 12:
                AdjustStatus(player, playerWaypoint - diceSideThrown);
                break;
            case 16:
                AdjustStatus(player, 12);
                break;
            case 21:
                AdjustStatus(player, 30);
                break;
            case 26:
                AdjustStatus(player, 31);
                break;
            case 27:
                AdjustStatus(player, 30);
                break;
            case 35:
                AdjustStatus(player, 42);
                break;
            case 44:
                AdjustStatus(player, 33);
                break;
            case 53:
                AdjustStatus(player, 49);
                break;
            default:
                break;
        }
    }

    void AdjustStatus(int player, int NewTile) 
    {
        if (player == 1) {
            player1.GetComponent<FollowThePath>().waypointIndex = NewTile;
            diceSideThrown = 0;
            player1StartWaypoint = NewTile;
            MovePlayer(player);
        } else {
            player2.GetComponent<FollowThePath>().waypointIndex = NewTile;
            diceSideThrown = 0;
            player2StartWaypoint = NewTile;
            MovePlayer(player);
        }
    }
}
