using UnityEngine;

public class Monster : MonoBehaviour, IParentCardUpdater
{
    private IMonsterState state = new Inactive();
    private Vector3 parentCardPosition;
    private Quaternion parentCardRotation;
    public void UpdateState(IMonsterState state)
    {
        this.state = state;
    }

    public void Init(Vector3 cardPosition, Quaternion cardRotation, GameObject prefab)
    {
        parentCardPosition = cardPosition;
        parentCardRotation = cardRotation;
        state.Init(gameObject, parentCardPosition, cardRotation, prefab);
    }

    public void UpdateParentCard(Vector3 position, Quaternion rotation)
    {
        parentCardPosition = position;
        parentCardRotation = rotation;
    }

    void Update()
    {
        state.Update(parentCardPosition, parentCardRotation);
    }
}
