using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    // variable for storing last tile landed on for board tile info scene loading
    public static int lastRollWayPoint = 0;
    public bool[] visited = new bool[60];

    public bool isPlayer1Turn = true;

    public static bool gameOver = false;

    // Use this for initialization
    void Start () {
        visited[0] = true;


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
            Debug.Log("player1StartWaypoint: " + player1StartWaypoint);
            lastRollWayPoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            Debug.Log("lastRollWayPoint: " + lastRollWayPoint);
            if (!visited[player1StartWaypoint]) {
                visited[player1StartWaypoint] = true;
                // Initiate.Fade("Base_Tile", Color.black, 1f);
                isPlayer1Turn = true;
                SceneManager.LoadSceneAsync("Base_Tile", LoadSceneMode.Additive);
                // CheckSpecialAfter(player1StartWaypoint, 1);
            } else {
                // ask player if they wanna see it again
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
                visited[player2StartWaypoint] = true;
                // Initiate.Fade("Base_Tile", Color.black, 1f);
                // isPlayer1Turn = false;
                SceneManager.LoadSceneAsync("Base_Tile", LoadSceneMode.Additive);
                // CheckSpecialAfter(player2StartWaypoint, 2);
            } else {
                // ask player if they wanna see it again
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
