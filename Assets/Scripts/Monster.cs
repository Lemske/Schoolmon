using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Monster : MonoBehaviour, IParentCardUpdater
{
    [SerializeField] public Vector3 WantedMonPosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private bool isDeadTESTING = false;
    private bool hasEnded = false;
    public string monsterName { get; set; }
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

    public void Init(Vector3 cardPosition, Quaternion cardRotation, GameObject prefab, string monsterName)
    {
        this.monsterName = monsterName;
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
        if (isDeadTESTING && !hasEnded)
        {
            hasEnded = true;
            Debug.Log("Monster is dead");
            EndGame();
        }
    }

    public Vector3 CalculateWantedMonPosition()
    {
        return parentCardPosition + parentCardRotation * WantedMonPosition;
    }

    private void EndGame()
    {
        if (NetworkManager.instance != null)
        {
            string winner = "Someone";
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName != PhotonNetwork.NickName)
                {
                    winner = player.NickName;
                }
            }
            NetworkManager.instance.photonView.RPC("PlayerLost", RpcTarget.All, winner);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CalculateWantedMonPosition(), 0.01f);

        despawnState.OnDrawGizmos();
    }
}
