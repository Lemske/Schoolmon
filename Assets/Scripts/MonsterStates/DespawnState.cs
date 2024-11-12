using Photon.Pun;
using UnityEngine;
[System.Serializable]
public class DespawnState : IMonsterState
{
    private Monster monster;
    private bool isDespawning = false;
    private Vector3 lastCardPosition;
    private Quaternion lastCardRotation;
    [SerializeField] Vector3 legalPlayZone = new Vector3(0.1f, 0.05f, 0.02f);
    public void Init(Monster monster)
    {
        this.monster = monster;
        lastCardPosition = monster.parentCardPosition;
        lastCardRotation = monster.parentCardRotation;
    }

    public void Update()
    {
        if (!WithinLegalPlayZone() || isDespawning)
        {
            if (!isDespawning)
            {
                isDespawning = true;
                if (monster.monsterName == NetworkManager.monsterName) //Only the player will despawn their own monster, should be handled better and should use some math to help the two phones to understand the layout
                {
                    NetworkManager.instance.photonView.RPC("MonsterDeselected", RpcTarget.All, NetworkManager.thisPlayer.ToString());
                }
            }
            monster.transform.position = Vector3.Lerp(monster.transform.position, monster.parentCardPosition, Time.deltaTime * 2);
            monster.transform.localScale = Vector3.Lerp(monster.transform.localScale, Vector3.zero, Time.deltaTime * 3);
            if (monster.transform.localScale == Vector3.zero)
            {
                isDespawning = false;
                monster.UpdateState(monster.state = monster.inactive);
            }
        }
        lastCardPosition = monster.parentCardPosition;
        lastCardRotation = monster.parentCardRotation;
    }

    public bool WithinLegalPlayZone()
    {
        Vector3 localPoint = Quaternion.Inverse(lastCardRotation) * (monster.parentCardPosition - lastCardPosition);
        Debug.DrawLine(Vector3.zero, localPoint, Color.red);
        Vector3 halfExtents = legalPlayZone;
        bool isWithin = localPoint.x >= -halfExtents.x && localPoint.x <= halfExtents.x &&
                        localPoint.y >= -halfExtents.y && localPoint.y <= halfExtents.y &&
                        localPoint.z >= -halfExtents.z && localPoint.z <= halfExtents.z;

        return isWithin;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.05f);

        Gizmos.matrix = Matrix4x4.TRS(monster.parentCardPosition, monster.parentCardRotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, legalPlayZone);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, legalPlayZone);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
