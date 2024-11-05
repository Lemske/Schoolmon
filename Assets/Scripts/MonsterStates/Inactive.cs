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

            if (NetworkManager.instance == null)
            {
                Debug.LogError("NetworkManager instance is null");
                return;
            }

            if (NetworkManager.monsterName != null && NetworkManager.otherMonsterName != null)
            {
                Debug.LogError("Both monsters are already selected");
                return;
            }

            NetworkManager.PLAYER player = NetworkManager.monsterName == null ? NetworkManager.PLAYER.PLAYER1 : NetworkManager.PLAYER.PLAYER2;

            bool notLegal;

            if (player == NetworkManager.thisPlayer)
            {
                notLegal = NetworkManager.otherMonsterName != null || NetworkManager.otherMonsterName.Equals(monster.monsterName);
            }
            else
            {
                notLegal = NetworkManager.monsterName != null || NetworkManager.monsterName.Equals(monster.monsterName);
            }

            if (notLegal)
            {
                Debug.LogError("Monster already selected");
                return;
            }

            NetworkManager.monsterName = monster.monsterName;
            NetworkManager.instance.photonView.RPC("MonsterSelected", RpcTarget.All, monster.monsterName, player.ToString());

            return;
        }
        prevCardPosition = cardPosition;
        prevCardRotation = cardRotation;
    }
}

