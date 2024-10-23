using UnityEngine;

public class SpawningTest : IMonsterState
{
    private Monster monster;
    private float scaleSpeed = 1.0f;

    public void Init(Monster monster)
    {
        this.monster = monster;
    }

    public void Update()
    {
        Vector3 cardPosition = monster.parentCardPosition;
        monster.transform.position = cardPosition;
        monster.transform.rotation = monster.parentCardRotation;
        if (monster.transform.localScale != monster.prefab.transform.localScale)
        {
            monster.transform.localScale = Vector3.Lerp(monster.transform.localScale, monster.prefab.transform.localScale, Time.deltaTime * scaleSpeed);
        }
    }
}
