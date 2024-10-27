using UnityEngine;
[System.Serializable]
public class DespawnState : IMonsterState
{
    private Monster monster;
    [SerializeField] Vector3 legalPlayZone = new Vector3(0.1f, 0.05f, 0.02f);
    public void Init(Monster monster)
    {
        this.monster = monster;
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.TRS(monster.parentCardPosition, monster.parentCardRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, legalPlayZone);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
