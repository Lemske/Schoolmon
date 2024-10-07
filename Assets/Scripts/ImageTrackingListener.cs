using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingListener : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imgTracker;
    [SerializeField] XRReferenceImageLibrary library; //TODO: This should be done in some testing script
    [SerializeField] MonsterManager monsterManager;
    void OnEnable() => imgTracker.trackablesChanged.AddListener(OnChanged);
    void OnDisable() => imgTracker.trackablesChanged.RemoveListener(OnChanged);

    void Start()
    {
        //TODO: This should be done in some testing script
        List<string> imgNames = new List<string>();
        foreach (var img in library)
        {
            imgNames.Add(img.name);
        }
        monsterManager.CheckIfMissing(imgNames);
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> evtArgs)
    {
        foreach (var newImage in evtArgs.added)
        {
            monsterManager.InstantiateMonster(newImage.referenceImage.name, newImage.transform.position, newImage.transform.rotation);
        }
        foreach (var updatedImage in evtArgs.updated)
        {
            monsterManager.UpdateMonsterPosition(updatedImage.referenceImage.name, updatedImage.transform.position, updatedImage.transform.rotation);
        }
        foreach (var removedImage in evtArgs.removed)
        {
            Debug.Log($"Image removed: {removedImage.Key}");
        }
    }
}
