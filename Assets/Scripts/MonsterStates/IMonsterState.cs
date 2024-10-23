using UnityEngine;

public interface IMonsterState
{
    public void Update();
    public void Init(Monster monster);
}