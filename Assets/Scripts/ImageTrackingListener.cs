using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingListener : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imgTracker;
    [SerializeField] XRReferenceImageLibrary library; //TODO: This should be done in some testing script
    [SerializeField] GameObject[] prefabs;
    private List<GameObject> instantiatedPrefabs;
    void OnEnable() => imgTracker.trackablesChanged.AddListener(OnChanged);
    void OnDisable() => imgTracker.trackablesChanged.RemoveListener(OnChanged);

    void Start()
    {
        instantiatedPrefabs = new List<GameObject>();
        Debug.Log("ImageTrackingListener Start");
        //TODO: should be done in some testing script
        HashSet<string> prefabNames = new HashSet<string>();
        foreach (var prefab in prefabs)
        {
            prefabNames.Add(prefab.name);
        }
        ArrayList missingTrackables = new ArrayList();
        foreach (var trackedImage in library)
        {
            name = trackedImage.name;
            if (prefabNames.Contains(name))
            {
                prefabNames.Remove(name);
            }
            else
            {
                missingTrackables.Add("Missing Prefab: " + name);
            }
        }
        foreach (string prefabName in prefabNames)
        {
            missingTrackables.Add("Missing Image: " + prefabName);
        }
        if (missingTrackables.Count > 0)
        {
            throw new System.Exception("\n" + string.Join("\n", missingTrackables.ToArray()));
        }
        //TODO: should be done in some testing script
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> evtArgs)
    {
        foreach (var newImage in evtArgs.added)
        {
            Debug.Log($"Image added: {newImage.referenceImage.name}");
            foreach (var prefab in prefabs)
            {
                if (newImage.referenceImage.name == prefab.name)
                {
                    GameObject instantiated = Instantiate(prefab, newImage.transform.position, newImage.transform.rotation);
                    instantiatedPrefabs.Add(instantiated);
                }
            }
        }

        foreach (var updatedImage in evtArgs.updated)
        {
            Debug.Log($"Image updated: {updatedImage.referenceImage.name}");
            foreach (var instantiatedPrefab in instantiatedPrefabs)
            {
                if (updatedImage.referenceImage.name == instantiatedPrefab.name)
                {
                    instantiatedPrefab.transform.position = updatedImage.transform.position;
                }
            }

        }

        foreach (var removedImage in evtArgs.removed)
        {
            Debug.Log($"Image removed: {removedImage.Key}");
        }
    }
}
