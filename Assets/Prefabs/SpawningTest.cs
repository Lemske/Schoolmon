using UnityEngine;

public class SpawningTest : MonoBehaviour
{
    private Vector3 originalScale;
    private float scaleSpeed = 1.0f;
    void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
        transform.Rotate(Vector3.up, Time.deltaTime * 30.0f);
    }
}
