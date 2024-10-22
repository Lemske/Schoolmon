using UnityEngine;

[System.Serializable]
public class Inactive : IMonsterState
{
    private GameObject monster;
    private GameObject prefab;
    private Vector3 prevCardPosition;
    private Quaternion prevCardRotation;
    private int iteration = 0;
    [SerializeField] private float legalCardMovementRadius = 0.1f;
    [SerializeField] private float legalCardRotationRadius = 0.1f;
    [SerializeField] private int iterationNeeded = 5;

    public void Init(GameObject monster, Vector3 cardPosition, Quaternion cardRotation, GameObject prefab)
    {
        monster.transform.localScale = Vector3.zero;
        this.monster = monster;
        prevCardPosition = cardPosition;
        prevCardRotation = cardRotation;
        this.prefab = prefab;
    }

    public void Update(Vector3 cardPosition, Quaternion cardRotation)
    {
        Debug.Log(Vector3.Distance(prevCardPosition, cardPosition) + " && " + Quaternion.Angle(prevCardRotation.normalized, cardRotation.normalized));
        if (Vector3.Distance(prevCardPosition, cardPosition) <= legalCardMovementRadius && Quaternion.Angle(prevCardRotation.normalized, cardRotation.normalized) <= legalCardRotationRadius)
        {
            Debug.Log("Inactive");
            iteration++;
        }

        if (iteration >= iterationNeeded)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            SpawningTest spawningTest = new SpawningTest();
            spawningTest.Init(monster, prevCardPosition, prevCardRotation, prefab);
            monsterScript.UpdateState(spawningTest);
        }
        prevCardPosition = cardPosition;
        prevCardRotation = cardRotation;
    }
}

