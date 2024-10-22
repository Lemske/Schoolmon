using UnityEngine;
public interface IParentCardUpdater
{
    public void Init(Vector3 cardPosition, Quaternion cardRotation, GameObject prefab);
    public void UpdateParentCard(Vector3 position, Quaternion rotation);
}