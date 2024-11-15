using System;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class IdleState : IMonsterState
{
    [SerializeField] public float idleMovementSpeed = 1f; //TODO: This should probably be in the monster class
    [SerializeField] public float idleRotationSpeed = 1f; //TODO: This should probably be in the monster class
    private CustomMovement customMovement;
    private Monster monster;


    public void Init(Monster monster)
    {
        customMovement = monster.GetComponent<CustomMovement>();
        this.monster = monster;
        if (customMovement != null)
        {
            customMovement.Init(monster);
        }
    }

    public void Update()
    {
        if (customMovement != null)
        {
            customMovement.Update();

        }
        else
        {
            monster.transform.position = Vector3.Lerp(monster.transform.position, monster.CalculateWantedMonPosition(), Time.deltaTime * idleMovementSpeed);
            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, monster.parentCardRotation, Time.deltaTime * idleRotationSpeed);
        }
        monster.despawnState.Update();
    }

    private void UpdatePosition() //TODO: I wanna do something smart, but me brain tired
    {
        throw new NotImplementedException();
    }
}
