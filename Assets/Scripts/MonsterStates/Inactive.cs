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
                Debug.LogError("NetworkManager instance is null");
                return;
            }

            /* This if statement is simply so we don't keep assigning things again in case one phone thinks a mon has despawned and spawned again*/
            if (NetworkManager.monsterName != null && NetworkManager.otherMonsterName != null)
            {
                Debug.Log("Other: " + NetworkManager.otherMonsterName);
                Debug.Log("This: " + NetworkManager.monsterName);
                Debug.Log("Both monsters are already selected");
                return;
            }

            NetworkManager.PLAYER player = NetworkManager.monsterName == null ? NetworkManager.thisPlayer
            : NetworkManager.thisPlayer == NetworkManager.PLAYER.PLAYER1 ? NetworkManager.PLAYER.PLAYER2 : NetworkManager.PLAYER.PLAYER1;

            bool notLegal;

            if (player == NetworkManager.thisPlayer)
            {
                notLegal = NetworkManager.otherMonsterName != null && NetworkManager.otherMonsterName.Equals(monster.monsterName);
            }
            else
            {
                notLegal = NetworkManager.monsterName != null && NetworkManager.monsterName.Equals(monster.monsterName);
            }

            if (notLegal)
            {
                Debug.Log("Monster already selected");
                return;
            }

            NetworkManager.monsterName = monster.monsterName;
            NetworkManager.instance.photonView.RPC("MonsterSelected", RpcTarget.All, player.ToString(), monster.monsterName);

            return;
        }
        prevCardPosition = cardPosition;
        prevCardRotation = cardRotation;
    }
}

