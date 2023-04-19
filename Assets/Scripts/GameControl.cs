using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;
    public static GameObject dice;
    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    // variable for storing last tile landed on for board tile info scene loading
    public static int lastRollWayPoint = 0;
    public bool[] visited = new bool[60];
    public static bool gameOver = false;

    // Use this for initialization
    void Start () 
    {
        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        dice = GameObject.Find("Dice");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;
        player1MoveText.gameObject.SetActive(true);

        whoWinsTextShadow.gameObject.SetActive(false);
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
            if (!visited[player1StartWaypoint]) {
                visited[player1StartWaypoint] = true;
                dice.gameObject.SetActive(false);
                SceneManager.LoadSceneAsync("Base_Tile", LoadSceneMode.Additive);
            } else {
                // ask player if they wanna see it again
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
                dice.gameObject.SetActive(false);
                SceneManager.LoadSceneAsync("Base_Tile", LoadSceneMode.Additive);
            } else {
                // ask player if they wanna see it again
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
}
