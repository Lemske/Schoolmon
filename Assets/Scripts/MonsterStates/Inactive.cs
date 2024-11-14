using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class Inactive : IMonsterState
{
    private Monster monster;
    private Vector3 prevCardPosition;
    private Quaternion prevCardRotation;
    [SerializeField] private float legalCardMovementRadius = 0.01f;
    [SerializeField] private float legalCardRotationRadius = 0.01f;
    [SerializeField] private float timeNeeded = 4f;
    private float currentTimeNeeded;

    public void Init(Monster monster)
    {
        monster.transform.localScale = Vector3.zero;
        currentTimeNeeded = timeNeeded;
        this.monster = monster;
        this.prevCardPosition = monster.parentCardPosition;
        this.prevCardRotation = monster.parentCardRotation;
    }

    public void Update()
    {
        Vector3 cardPosition = monster.parentCardPosition;
        Quaternion cardRotation = monster.parentCardRotation;

        //Debug.Log(Vector3.Distance(prevCardPosition, cardPosition) + " && " + Quaternion.Angle(prevCardRotation.normalized, cardRotation.normalized));
        if (Vector3.Distance(prevCardPosition, cardPosition) <= legalCardMovementRadius && Quaternion.Angle(prevCardRotation.normalized, cardRotation.normalized) <= legalCardRotationRadius)
        {
            currentTimeNeeded -= Time.deltaTime;
        }
        else
        {
            currentTimeNeeded = timeNeeded;
        }

        if (currentTimeNeeded <= 0)
        {
            monster.state = monster.spawningTest;
            currentTimeNeeded = timeNeeded;

            if (NetworkManager.instance == null) //For test scene
            {
                Debug.Log("NetworkManager instance is null");
                NetworkManager.monsterName = NetworkManager.monsterName == null ? monster.monsterName : NetworkManager.monsterName;
                return;
            }

            if (monster.monsterName.Equals(NetworkManager.monsterName))
            {
                Debug.Log("Monster already found by other player");
                return;
            }

            if (NetworkManager.monsterName == null || NetworkManager.monsterName.Equals(monster.monsterName))
            {
                NetworkManager.instance.photonView.RPC("MonsterSelected", RpcTarget.All, NetworkManager.thisPlayer.ToString(), monster.monsterName, NetworkManager.thisPlayer.ToString());
                return;
            }

            NetworkManager.PLAYER otherPlayer = NetworkManager.thisPlayer == NetworkManager.PLAYER.PLAYER1 ? NetworkManager.PLAYER.PLAYER2 : NetworkManager.PLAYER.PLAYER1;
            NetworkManager.instance.photonView.RPC("MonsterSelected", RpcTarget.All, otherPlayer.ToString(), monster.monsterName, NetworkManager.thisPlayer.ToString());

            return;
        }
        prevCardPosition = cardPosition;
        prevCardRotation = cardRotation;
    }
}

