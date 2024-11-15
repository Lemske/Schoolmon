using UnityEngine;
[System.Serializable]
public class BugMovement : MonoBehaviour, CustomMovement
{
    [SerializeField] private Vector3 topDirectionOffset = new Vector3(0, 0.1f, 0);
    [SerializeField] private float topDirectionSpeed = 3f;
    private Monster monster;
    private Vector3 wantedPosition;
    public void Init(Monster monster)
    {
        this.monster = monster;
        wantedPosition = monster.WantedMonPosition;
    }

    public void Update()
    {
        monster.transform.position = Vector3.Lerp(monster.transform.position, monster.CalculateWantedMonPosition(), Time.deltaTime * monster.idleState.idleMovementSpeed);
        monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, monster.parentCardRotation, Time.deltaTime * monster.idleState.idleMovementSpeed);

        Vector3 topDirectionPosition = CalculateTopDirectionPosition();
        Debug.DrawLine(monster.transform.position, topDirectionPosition, Color.green);

        // Make the monster's up vector point toward the target position
        Vector3 directionToLook = topDirectionPosition - monster.transform.position;
        if (directionToLook.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(monster.transform.up, directionToLook) * monster.transform.rotation;
            monster.transform.rotation = Quaternion.Lerp(
                monster.transform.rotation,
                targetRotation,
                Time.deltaTime * topDirectionSpeed
            );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(CalculateTopDirectionPosition(), 0.005f);
    }

    private Vector3 CalculateTopDirectionPosition()
    {
        Vector3 cardPosition = monster.parentCardPosition;
        Quaternion cardRotation = monster.parentCardRotation;
        return cardPosition + cardRotation * (wantedPosition + topDirectionOffset);
    }
}
