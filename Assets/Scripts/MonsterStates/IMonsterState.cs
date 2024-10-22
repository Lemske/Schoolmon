using UnityEngine;

public interface IMonsterState
{
    public void Update(Vector3 cardPosition, Quaternion cardRotation);
    public void Init(GameObject monster, Vector3 cardPosition, Quaternion cardRotation, GameObject prefab);
}