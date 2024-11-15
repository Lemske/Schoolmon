using TMPro;
using UnityEngine;

public class SimpleTextRotation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = text.transform.rotation;
    }

    void Update()
    {
        text.transform.rotation = startRotation * Quaternion.Euler(Mathf.Sin(Time.time) * 10, Mathf.Sin(Time.time) * 20 * 2, Mathf.Sin(Time.time) * 4);
    }
}
