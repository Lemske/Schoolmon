using System;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class IdleState : IMonsterState
{
    [SerializeField] private float idleMovementSpeed = 1f; //TODO: This should probably be in the monster class
    [SerializeField] private float idleRotationSpeed = 1f; //TODO: This should probably be in the monster class
    private Monster monster;


    public void Init(Monster monster)
    {
        this.monster = monster;
    }

    public void Update()
    {
        monster.transform.position = Vector3.Lerp(monster.transform.position, monster.CalculateWantedMonPosition(), Time.deltaTime * idleMovementSpeed);
        monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, monster.parentCardRotation, Time.deltaTime * idleRotationSpeed);
        monster.despawnState.Update();
    }

    private void UpdatePosition() //TODO: I wanna do something smart, but me brain tired
    {
        throw new NotImplementedException();
    }
}
