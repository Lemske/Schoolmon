using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        quizCanvas.SetActive(false);
        StartCoroutine(WaitForMeshToSpawn());
        LoadQuestionsFromJson();
    }

    private IEnumerator WaitForMeshToSpawn()
    {
        // Keep waiting until the Health component is found
        while (targetHealth == null)
        {
            targetHealth = FindObjectOfType<Health>();
            yield return null;  // Wait a frame before checking again
        }

        Debug.Log("Health component assigned to QuizManager");
        generateQuestion();
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
            questionsAndAnswers = JsonUtility.FromJson<ListWrapper>(jsonFile.text).questions;
            Debug.Log("Questions loaded successfully");
        }
        else
        {
            Debug.LogError("Questions file not found!");
        }
    }

    // A helper wrapper class for deserialization
    [System.Serializable]
    private class ListWrapper
    {
        public List<QuestionsAndAnswers> questions;
    }

    public void correct()
    {
        // Apply damage to the target mesh
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            if (targetHealth.currentHealth <= 0)
            {
                //Destroy(targetHealth.gameObject);
                quizCanvas.SetActive(false);
            }
        }
        questionsAndAnswers.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        questionsAndAnswers.RemoveAt(currentQuestion);
        generateQuestion();
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

    void generateQuestion()
    {
        if (questionsAndAnswers.Count > 0)
        {
            currentQuestion = Random.Range(0, questionsAndAnswers.Count);

            questionText.text = questionsAndAnswers[currentQuestion].question;
            SetAnswers();
        }
        else
        {
            questionText.text = "Out of questions";
        }
    }
}
