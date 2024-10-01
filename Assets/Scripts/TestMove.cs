using UnityEngine;

public class TestMove : MonoBehaviour
{
    Vector3 originalLocation;
    float moveDistance = 1.0f;
    void Start()
    {
        originalLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object back and forth
        transform.position = originalLocation + new Vector3(Mathf.Sin(Time.time) * moveDistance, 0, 0);
    }
}
