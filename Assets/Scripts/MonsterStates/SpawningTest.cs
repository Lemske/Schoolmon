using UnityEngine;
[System.Serializable]
public class SpawningTest : IMonsterState
{
    [SerializeField] private float heightOverWantedPosition = 0.5f;
    [SerializeField] private float curveSpeed = 1.0f;
    private Monster monster;
    private Vector3 firstPosition;
    private float scaleSpeed = 1.0f;
    private float t = 0.0f;
    private Vector3 controlPoint;

    public void Init(Monster monster)
    {
        this.monster = monster;
        this.firstPosition = monster.transform.position;
    }

    public void Update()
    {
        UpdateControlPoint();

        monster.transform.rotation = monster.parentCardRotation;
        if (monster.transform.localScale != monster.prefab.transform.localScale)
        {
            monster.transform.localScale = Vector3.Lerp(monster.transform.localScale, monster.prefab.transform.localScale, Time.deltaTime * scaleSpeed);
        }
        monster.transform.position = CalculateBezierCurve(monster.parentCardPosition, monster.CalculateWantedMonPosition(), controlPoint, t);
        t += Time.deltaTime * curveSpeed;
        if (t >= 1.0f)
        {
            monster.UpdateState(monster.idleState);
            monster.despawnState.Init(monster);
            t = 0.0f;
        }
    }

    private void UpdateControlPoint()
    {
        Vector3 wantedMonPosition = monster.CalculateWantedMonPosition();
        Vector3 midway = (firstPosition + wantedMonPosition) / 2;
        midway = new Vector3(midway.x, wantedMonPosition.y + heightOverWantedPosition, midway.z);
        controlPoint = midway;
    }

    private Vector3 CalculateBezierCurve(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        return Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * control + Mathf.Pow(t, 2) * end;
    }
}
