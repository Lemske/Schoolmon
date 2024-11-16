using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
[System.Serializable]
public class DespawnState : IMonsterState
{
    private Monster monster;
    private bool isDespawning = false;
    [SerializeField] Vector3 legalPlayZone = new Vector3(0.1f, 0.05f, 0.02f);
    [SerializeField] float shouldBeVisibleRadius = 1f;
    [SerializeField] float nonVisibleDespawnTime = 2f;
    private Camera origin;
    private bool enteredCheckingMode = false;
    private Vector3 cardStartPosition;
    private Quaternion cardStartRotation;
    private bool startPosRosNeedsUpdate = true;

    public void Init(Monster monster)
    {
        this.monster = monster;
        origin = monster.origin;
    }

    public void Update()
    {
        if (IsObjectWithinSphere() && IsTargetWithinView() && !isDespawning)
        {
            if (!enteredCheckingMode)
            {
                enteredCheckingMode = true;
                monster.timeSinceLastCardUpdate = 0;
            }
            if (monster.timeSinceLastCardUpdate >= nonVisibleDespawnTime)
            {
                MandatoryDespawnThings();
            }
        }
        else
        {
            enteredCheckingMode = false;
        }
        if (startPosRosNeedsUpdate)
        {
            cardStartPosition = monster.parentCardPosition;
            cardStartRotation = monster.parentCardRotation;
            startPosRosNeedsUpdate = false;
        }
        if (!WithinLegalPlayZone() || isDespawning)
        {
            if (!isDespawning)
            {
                MandatoryDespawnThings();
            }
            monster.transform.position = Vector3.Lerp(monster.transform.position, monster.parentCardPosition, Time.deltaTime * 2);
            monster.transform.localScale = Vector3.Lerp(monster.transform.localScale, Vector3.zero, Time.deltaTime * 3);
            if (monster.transform.localScale == Vector3.zero)
            {
                isDespawning = false;
                startPosRosNeedsUpdate = true;
                monster.UpdateState(monster.state = monster.inactive);
            }
        }
    }

    private void MandatoryDespawnThings()
    {
        isDespawning = true;
        monster.health.TurnOffHealthBar();
        if (monster.monsterName == NetworkManager.monsterName) //Only the player will despawn their own monster, should be handled better and should use some math to help the two phones to understand the layout
        {
            NetworkManager.instance.photonView.RPC("MonsterDeselected", RpcTarget.All, NetworkManager.thisPlayer.ToString());
        }
    }

    public bool WithinLegalPlayZone()
    {
        Vector3 localPoint = cardStartRotation * (monster.parentCardPosition - cardStartPosition);
        Debug.DrawLine(Vector3.zero, localPoint, Color.red);
        Vector3 halfExtents = legalPlayZone * 0.5f;
        bool isWithin = localPoint.x >= -halfExtents.x && localPoint.x <= halfExtents.x &&
                        localPoint.y >= -halfExtents.y && localPoint.y <= halfExtents.y &&
                        localPoint.z >= -halfExtents.z && localPoint.z <= halfExtents.z;

        return isWithin;
    }

    private bool IsObjectWithinSphere()
    {
        return Vector3.Distance(monster.parentCardPosition, origin.transform.position) <= shouldBeVisibleRadius;
    }

    private bool IsTargetWithinView()
    {
        Vector3 ViewportPoint = origin.WorldToViewportPoint(monster.parentCardPosition);
        return ViewportPoint.x >= 0 && ViewportPoint.x <= 1 && ViewportPoint.y >= 0 && ViewportPoint.y <= 1 && ViewportPoint.z >= 0;
    }

    public void OnDrawGizmos()
    {
        if (cardStartPosition == Vector3.zero || cardStartRotation == Quaternion.identity)
        {
            return;
        }
        Gizmos.color = new Color(1, 0, 0, 0.05f);

        Gizmos.matrix = Matrix4x4.TRS(cardStartPosition, cardStartRotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, legalPlayZone);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, legalPlayZone);
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = new Color(0, 1, 0, 0.01f);
        Gizmos.DrawSphere(monster.parentCardPosition, shouldBeVisibleRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(monster.parentCardPosition, shouldBeVisibleRadius);
    }
}
