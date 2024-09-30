using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingListener : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imgTracker;
    [SerializeField] GameObject[] prefabs;
    void OnEnable() => imgTracker.trackablesChanged.AddListener(OnChanged);
    void OnDisable() => imgTracker.trackablesChanged.RemoveListener(OnChanged);

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> evtArgs)
    {
        foreach (var newImage in evtArgs.added)
        {
            Debug.Log($"Image added: {newImage.referenceImage.name}");
            foreach (var prefab in prefabs)
            {
                if (newImage.referenceImage.name == prefab.name)
                {
                    Instantiate(prefab, newImage.transform.position, newImage.transform.rotation);
                }
            }
        }

        foreach (var updatedImage in evtArgs.updated)
        {
            Debug.Log($"Image updated: {updatedImage.referenceImage.name}");
        }

        foreach (var removedImage in evtArgs.removed)
        {
            Debug.Log($"Image removed: {removedImage.Key}");
        }
    }
}
