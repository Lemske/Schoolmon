using UnityEngine;

public class YOLO : MonoBehaviour
{
    private static YOLO instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
