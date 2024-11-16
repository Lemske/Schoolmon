using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public static string winner;
    public enum PLAYER { PLAYER1, PLAYER2 };
    public static PLAYER thisPlayer;
    public static string monsterName;
    public static string otherMonsterName;
    public static bool pl1Ready = false;
    public static bool pl2Ready = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void CreateRoom(string roomName)
    {
        thisPlayer = PLAYER.PLAYER1; ;
        PhotonNetwork.CreateRoom(roomName);
    }
    public void JoinRoom(string roomName)
    {
        thisPlayer = PLAYER.PLAYER2;
        PhotonNetwork.JoinRoom(roomName);
    }
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    [PunRPC]
    public void MonsterSelected(string player, string monsterName, string sendFrom)
    {
        Debug.Log("Monster selected: " + monsterName + " by " + player);
        if (player.Equals(thisPlayer.ToString()))
        {
            pl1Ready = player.Equals(sendFrom);
            NetworkManager.monsterName = monsterName;
        }
        else
        {
            pl2Ready = player.Equals(sendFrom);
            otherMonsterName = monsterName;
        }
    }

    [PunRPC]
    public void MonsterDeselected(string player)
    {
        Debug.Log("Monster deselected by " + player);
        if (player.Equals(thisPlayer.ToString()))
        {
            Debug.Log("Monster deselected " + monsterName);
            monsterName = null;
        }
        else
        {
            Debug.Log("Monster deselected " + otherMonsterName);
            otherMonsterName = null;
        }
    }

    [PunRPC]
    public void PlayerLost(string winner)
    {
        Debug.Log("Player lost: " + winner);
        SceneManager.LoadScene("EndGame");
    }
    [PunRPC]
    public void EndSessionForAllPlayers()
    {
        StartCoroutine(WaitForDisconnectAndLoadScene());
    }

    [PunRPC]
    public void DealtDamage(int amount, string from)
    {
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        if (from.Equals(thisPlayer.ToString()))
        {
            quizManager.DealtDamage(amount);
        }
        else
        {
            quizManager.TakeDamage(amount);
        }
    }

    private IEnumerator WaitForDisconnectAndLoadScene()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene("EndGame");
    }
}
