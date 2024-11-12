[System.Serializable]
public class MonsterSelectionDTA
{
    public string monsterName;
    public string player;

    public MonsterSelectionDTA() { }

    public MonsterSelectionDTA(string monsterName, string player)
    {
        this.monsterName = monsterName;
        this.player = player;
    }
}
