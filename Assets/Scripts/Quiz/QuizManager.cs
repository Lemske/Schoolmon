using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public List<QuestionsAndAnswers> questionsAndAnswers;
    public GameObject[] options;
    public int currentQuestion;
    public TextMeshProUGUI questionText;
    public Health targetHealth;
    public GameObject quizCanvas;
    public int damageAmount;  // Damage amount to apply for a correct answer
    public float fadeDuration = 1f; // Duration of the fade effect

    private bool playerFinishedTurn = false;
    private bool otherPlayerFinishedTurn = false;
    [SerializeField] private TextMeshProUGUI waitingText;

    private void Start()
    {
        quizCanvas.SetActive(false);
        StartCoroutine(WaitForMeshToSpawn());
    }

    private IEnumerator WaitForMeshToSpawn()
    {
        // Keep waiting until the Health component is found
        while (targetHealth == null)
        {
            if (NetworkManager.monsterName == null || NetworkManager.otherMonsterName == null)
            {
                yield return null;
            }
            Health[] healthComponents = FindObjectsOfType<Health>();
            foreach (Health health in healthComponents)
            {
                Monster monster = health.GetComponentInParent<Monster>();
                if (monster != null && monster.monsterName == NetworkManager.monsterName && NetworkManager.pl1Ready && NetworkManager.pl2Ready)
                {
                    LoadQuestionsFromJson();
                    targetHealth = health;
                    break;
                }
            }
            yield return null;
        }

        Debug.Log("Health component assigned to QuizManager");
        GenerateQuestion();
        yield return StartCoroutine(FadeInCanvas()); // Wait for the canvas to fade in


    }

    private IEnumerator FadeInCanvas()
    {
        CanvasGroup canvasGroup = quizCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = quizCanvas.AddComponent<CanvasGroup>(); // Add CanvasGroup if not present
        }

        canvasGroup.alpha = 0; // Start with the canvas completely transparent
        quizCanvas.SetActive(true); // Activate the canvas

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timeElapsed / fadeDuration); // Gradually increase alpha
            yield return null; // Wait for the next frame
        }

        canvasGroup.alpha = 1; // Ensure the canvas is fully opaque at the end
    }

    private void LoadQuestionsFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions");  // Load "questions.json" from Resources
        if (jsonFile != null)
        {
            string wrappedJson = "{\"items\":" + jsonFile.text + "}";
            ListWrapperWrapper monsterData = JsonUtility.FromJson<ListWrapperWrapper>(wrappedJson);
            Debug.Log(monsterData.items.Count);
            foreach (var monster in monsterData.items)
            {
                Debug.Log("Monster: " + monster.monsterName);
                Debug.Log("NetworkManager: " + NetworkManager.monsterName);
                if (monster.monsterName == NetworkManager.monsterName)
                {
                    questionsAndAnswers = monster.questions;
                    Debug.Log("Questions loaded successfully");
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Questions file not found!");
        }
    }
    [System.Serializable]
    private class ListWrapperWrapper
    {
        public List<MonsterWithQuestions> items;
    }

    // A helper wrapper class for deserialization
    [System.Serializable]
    private class ListWrapper
    {
        public List<QuestionsAndAnswers> questions;
    }

    public void correct()
    {
        questionsAndAnswers.RemoveAt(currentQuestion);
        GenerateQuestion();
        AnsweredQuestion();
        NetworkManager.instance.photonView.RPC("DealtDamage", RpcTarget.All, damageAmount, NetworkManager.thisPlayer.ToString());
    }

    public void wrong()
    {
        questionsAndAnswers.RemoveAt(currentQuestion);
        GenerateQuestion();
        AnsweredQuestion();
        NetworkManager.instance.photonView.RPC("DealtDamage", RpcTarget.All, 0, NetworkManager.thisPlayer.ToString());
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questionsAndAnswers[currentQuestion].answers[i];

            if (questionsAndAnswers[currentQuestion].correctAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(amount);
            if (targetHealth.currentHealth <= 0)
            {
                quizCanvas.SetActive(false);
            }
        }
        turnManage(false);
    }

    public void DealtDamage(int amount)
    {
        Health[] healthComponents = FindObjectsOfType<Health>();
        foreach (Health health in healthComponents)
        {
            Monster monster = health.GetComponentInParent<Monster>();
            if (monster.monsterName == NetworkManager.otherMonsterName)
            {
                health.TakeDamage(amount);
                if (health.currentHealth <= 0)
                {
                    quizCanvas.SetActive(false);
                }
            }
            Health.pendingDamage += amount;
            turnManage(true);
        }
    }

    void GenerateQuestion()
    {
        if (questionsAndAnswers.Count > 0)
        {
            currentQuestion = UnityEngine.Random.Range(0, questionsAndAnswers.Count);

            questionText.text = questionsAndAnswers[currentQuestion].question;
            SetAnswers();
        }
        else
        {
            questionText.text = "Out of questions";
        }
    }

    private void AnsweredQuestion()
    {
        quizCanvas.SetActive(false);
        waitingText.gameObject.SetActive(true);
    }

    private void turnManage(bool thisPlayer)
    {
        if (thisPlayer)
        {
            playerFinishedTurn = true;
        }
        else
        {
            otherPlayerFinishedTurn = true;
        }
        if (playerFinishedTurn && otherPlayerFinishedTurn)
        {
            playerFinishedTurn = false;
            otherPlayerFinishedTurn = false;
            waitingText.gameObject.SetActive(false);
            quizCanvas.SetActive(true);
        }
    }
}
