using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public void BackToMainScene()
    {
        // Now it just end the game
        Application.Quit();
    }

    public void Awake()
    {
        if (NetworkManager.winner == null)
        {
            text.text = "hmmm";
            return;
        }
        text.text = NetworkManager.winner;
    }
}
