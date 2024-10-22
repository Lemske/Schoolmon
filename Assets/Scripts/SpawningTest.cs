using UnityEngine;

public class SpawningTest : IMonsterState
{
    private GameObject monster;
    private GameObject prefab;
    private float scaleSpeed = 1.0f;

    public void Init(GameObject monster, Vector3 cardPosition, Quaternion cardRotation, GameObject prefab)
    {
        this.monster = monster;
        this.prefab = prefab;
    }

    public void Update(Vector3 cardPosition, Quaternion cardRotation)
    {
        monster.transform.position = cardPosition;
        if (monster.transform.localScale != prefab.transform.localScale)
        {
            monster.transform.localScale = Vector3.Lerp(monster.transform.localScale, prefab.transform.localScale, Time.deltaTime * scaleSpeed);

        }
        monster.transform.Rotate(Vector3.up, Time.deltaTime * 30.0f);
    }
}
