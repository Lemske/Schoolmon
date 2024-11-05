using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterInstance[] monsters;
    public void InstantiateMonster(string imgName, Vector3 position, Quaternion rotation)
    {
        MonsterInstance monster = FindMonsterFromImage(imgName);
        if (monster != null)
        {
            //Debug.Log("Instantiating Monster: " + monster.trackedImage.name + " at " + position);
            Debug.Log("prefab scale: " + monster.prefab.transform.localScale);
            GameObject monsterInstance = Instantiate(monster.prefab, position, rotation);
            IParentCardUpdater parentCardUpdater = monsterInstance.GetComponent<IParentCardUpdater>();
            if (parentCardUpdater != null)
            {
                parentCardUpdater.Init(position, rotation, monster.prefab);
            }
            else
            {
                throw new System.Exception("Missing IParentCardUpdater in Monster Prefab: " + monster.prefab.name);
            }
            monster.Instance = parentCardUpdater;
        }
    }

    public void UpdateMonsterPosition(string imgName, Vector3 position, Quaternion rotation)
    {
        MonsterInstance monster = FindMonsterFromImage(imgName);
        if (monster != null)
        {
            //            Debug.Log("Updating Monster: " + monster.trackedImage.name + " at " + position);
            //monster.Instance.transform.position = position;
            monster.Instance.UpdateParentCard(position, rotation);
        }
    }

    private MonsterInstance FindMonsterFromImage(string imgName)
    {
        foreach (var monster in monsters)
        {
            if (imgName == monster.trackedImage.name)
            {
                return monster;
            }
        }
        throw new System.Exception("Missing Image in Manager: " + imgName + "\nCheck the testing for failures");
    }

    public void CheckIfMissing(List<string> imgNames)
    {
        List<string> missingImages = new List<string>();
        foreach (var monster in monsters)
        {
            if (imgNames.Contains(monster.trackedImage.name))
            {
                imgNames.Remove(monster.trackedImage.name);
            }
            else
            {
                Debug.Log("Missing Image from Tracking: " + monster.trackedImage.name);
            }
        }
        if (missingImages.Count > 0)
        {
            throw new System.Exception("Missing Images in Manager: " + string.Join(", ", missingImages.ToArray()));
        }
    }

    [System.Serializable]
    public class MonsterInstance
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public Texture2D trackedImage;
        private IParentCardUpdater instance;

        public IParentCardUpdater Instance
        {
            get => instance;
            set => instance = value;
        }
    }
}
