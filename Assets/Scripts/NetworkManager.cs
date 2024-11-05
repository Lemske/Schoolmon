using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public enum PLAYER { PLAYER1, PLAYER2 };
    public static PLAYER thisPlayer;
    public static string monsterName;
    public static string otherMonsterName;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
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
            monsterName = null;
        }
        else
        {
            otherMonsterName = null;
        }
    }
}
