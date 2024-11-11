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
    public void MonsterSelected(string player, string monsterName)
    {
        Debug.Log("Monster selected: " + monsterName + " by " + player);
        if (player.Equals(thisPlayer.ToString()))
        {
            NetworkManager.monsterName = monsterName;
        }
        else
        {
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
        NetworkManager.winner = winner == PhotonNetwork.NickName ? "You" : winner;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            photonView.RPC("EndSessionForAllPlayers", RpcTarget.All);
        }
        else
        {
            StartCoroutine(WaitForDisconnectAndLoadScene());
        }
    }
    [PunRPC]
    public void EndSessionForAllPlayers()
    {
        StartCoroutine(WaitForDisconnectAndLoadScene());
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
