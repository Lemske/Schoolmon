using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Monster : MonoBehaviour, IParentCardUpdater
{
    [SerializeField] public Vector3 WantedMonPosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private bool isDeadTESTING = false;
    public Health health { get; private set; }
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
    public float timeSinceLastCardUpdate { get; set; }
    public Camera origin { get; private set; }
    public Vector3 cardzone = new Vector3(0.0859f, 0.005f, 0.1199141f);//TODO: Should get this from the card reference
    private GameObject cube;
    void Start()
    {
        state = inactive;
        health = GetComponent<Health>();
        origin = FindObjectOfType<Camera>();
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

        /*Temp*/
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = cardzone;
        cube.transform.position = parentCardPosition - new Vector3(0, 0, 0.1f);
        cube.transform.rotation = parentCardRotation;
        cube.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        this.cube = cube;
    }

    public void UpdateParentCard(Vector3 position, Quaternion rotation)
    {
        parentCardPosition = position;
        parentCardRotation = rotation;
        timeSinceLastCardUpdate = 0;
    }

    void Update()
    {
        cube.transform.position = parentCardPosition - new Vector3(0, 0, 0.1f);
        cube.transform.rotation = parentCardRotation;

        state.Update();
        if (isDeadTESTING && !hasEnded)
        {
            hasEnded = true;
            Debug.Log("Monster is dead");
            EndGame();
        }
        timeSinceLastCardUpdate += Time.deltaTime;
    }

    public Vector3 CalculateWantedMonPosition()
    {
        return parentCardPosition + parentCardRotation * WantedMonPosition;
    }

    public void EndGame()
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
        Gizmos.color = Color.grey;
        Gizmos.matrix = Matrix4x4.TRS(parentCardPosition, parentCardRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, cardzone);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CalculateWantedMonPosition(), 0.01f);

        despawnState.OnDrawGizmos();
    }
}
