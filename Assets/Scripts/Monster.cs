using UnityEngine;

public class Monster : MonoBehaviour, IParentCardUpdater
{
    [SerializeField] public Vector3 WantedMonPosition = new Vector3(0, 0.5f, 0);
    public Inactive inactive = new Inactive();
    public SpawningTest spawningTest = new SpawningTest();
    public IdleState idleState = new IdleState();
    public DespawnState despawnState = new DespawnState();
    public GameObject prefab { get; set; }
    public IMonsterState state { get; set; }
    public Vector3 parentCardPosition { get; set; }
    public Quaternion parentCardRotation { get; set; }

    void Start()
    {
        state = inactive;
    }

    public void UpdateState(IMonsterState state)
    {
        this.state = state;
    }

    public void Init(Vector3 cardPosition, Quaternion cardRotation, GameObject prefab)
    {
        this.prefab = prefab;
        this.parentCardPosition = cardPosition;
        this.parentCardRotation = cardRotation;
        inactive.Init(this);
        spawningTest.Init(this);
        idleState.Init(this);
        despawnState.Init(this);
    }

    public void UpdateParentCard(Vector3 position, Quaternion rotation)
    {
        parentCardPosition = position;
        parentCardRotation = rotation;
    }

    void Update()
    {
        state.Update();
    }

    public Vector3 CalculateWantedMonPosition()
    {
        return parentCardPosition + parentCardRotation * WantedMonPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CalculateWantedMonPosition(), 0.01f);

        despawnState.OnDrawGizmos();
    }
}
