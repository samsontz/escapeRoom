using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class AppControl : MonoBehaviour
{
    [System.Serializable]
    public class QuizQuestion
    {
        [TextArea(2, 3)] public string questionText;
        public List<string> answers;      // 4 possible answers
        public int correctAnswerIndex;    // index of the correct answer in the answers list
    }

    [Header("UI References")]
    public TMP_Text questionTextUI;
    public List<Button> answerButtons;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject roomClosedPanel;
    public GameObject quizCompletePanel;

    [Header("Quiz Data")]
    public List<QuizQuestion> questions;  // Should contain 5 questions
    private int currentQuestionIndex;
    private int score;

    [Header("Settings")]
    public float totalTimeInSeconds = 180f; // 3 minutes

    private float timer;
    private bool quizEnded = false;

    void Start()
    {
        timer = totalTimeInSeconds;
        currentQuestionIndex = 0;
        score = 0;

        // Hide end panels at start
        if (roomClosedPanel) roomClosedPanel.SetActive(false);
        if (quizCompletePanel) quizCompletePanel.SetActive(false);

        DisplayQuestion();
        UpdateScoreText();
    }

    void Update()
    {
        if (!quizEnded)
        {
            // Decrement timer
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                timer = 0;
                quizEnded = true;
                TimeIsUp();
            }
        }
    }

    void DisplayQuestion()
    {
        // If we've already answered all questions
        if (currentQuestionIndex >= questions.Count)
        {
            quizEnded = true;
            ShowQuizCompletePanel();
            return;
        }

        // Get current question data
        QuizQuestion questionData = questions[currentQuestionIndex];

        // Display the question text
        questionTextUI.text = questionData.questionText;

        // Shuffle answer indices
        List<int> shuffleIndices = new List<int>() { 0, 1, 2, 3 };
        shuffleIndices = shuffleIndices.OrderBy(x => Random.value).ToList();

        // Assign answers to the buttons
        for (int i = 0; i < answerButtons.Count; i++)
        {
            Button button = answerButtons[i];
            Text buttonText = button.GetComponentInChildren<Text>();

            int shuffledIndex = shuffleIndices[i];
            buttonText.text = questionData.answers[shuffledIndex];

            // Clear old listeners
            button.onClick.RemoveAllListeners();

            // If it's the correct answer
            if (shuffledIndex == questionData.correctAnswerIndex)
            {
                button.onClick.AddListener(() => OnCorrectAnswer());
            }
            else
            {
                button.onClick.AddListener(() => OnWrongAnswer());
            }
        }
    }

    void OnCorrectAnswer()
    {
        score++;
        UpdateScoreText();
        NextQuestion();
    }

    void OnWrongAnswer()
    {
        NextQuestion();
    }

    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            DisplayQuestion();
        }
        else
        {
            quizEnded = true;
            ShowQuizCompletePanel();
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score + " / " + questions.Count;
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimeIsUp()
    {
        if (roomClosedPanel)
            roomClosedPanel.SetActive(true);

        // Disable answer buttons
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }
    }

    void ShowQuizCompletePanel()
    {
        if (quizCompletePanel)
            quizCompletePanel.SetActive(true);

        // Optionally, show final score here
        // quizCompletePanel.GetComponentInChildren<Text>().text = "You scored " + score + "!";
    }
}
