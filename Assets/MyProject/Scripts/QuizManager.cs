using UnityEngine;
using TMPro; // Não esqueça de adicionar para usar TextMeshPro
using System.Collections;
using System.Collections.Generic; // Para usar List

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questionTextUI;
    public TextMeshProUGUI[] answerButtonsText; // Array para os textos dos botões de resposta
    public GameObject[] answerButtons; // Array para os GameObjects dos botões
    public TextMeshProUGUI timerTextUI;
    public TextMeshProUGUI scoreTextUI;
    public GameObject resultPanel;
    public TextMeshProUGUI finalScoreTextUI;

    [Header("Game Settings")]
    public List<QuestionData> allQuestions; // Lista de todas as perguntas
    public float timeLimit = 60f; // Limite de tempo para o quiz inteiro
    public int pointsPerCorrectAnswer = 10;

    private List<QuestionData> currentQuestions; // Perguntas para a rodada atual
    private int currentQuestionIndex = 0;
    private int currentScore = 0;
    private float currentTime;
    private bool quizActive = false;

    void Start()
    {
        InitializeQuiz();
    }

    void InitializeQuiz()
    {
        currentScore = 0;
        currentTime = timeLimit;
        currentQuestionIndex = 0;
        quizActive = true;
        resultPanel.SetActive(false); // Garante que o painel de resultados está desativado

        // Embaralha as perguntas (opcional, mas bom para rejogabilidade)
        ShuffleQuestions(allQuestions);
        currentQuestions = new List<QuestionData>(allQuestions); // Cria uma cópia para a rodada

        UpdateScoreUI();
        StartCoroutine(StartTimer());
        DisplayQuestion();
    }

    void ShuffleQuestions(List<QuestionData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            QuestionData temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    IEnumerator StartTimer()
    {
        while (currentTime > 0 && quizActive)
        {
            currentTime -= Time.deltaTime;
            timerTextUI.text = "Tempo: " + Mathf.Max(0, Mathf.FloorToInt(currentTime)).ToString() + "s";
            yield return null; // Espera o próximo frame
        }

        if (quizActive) // Verifica se o quiz ainda está ativo (não terminou por responder todas as perguntas)
        {
            EndQuiz();
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < currentQuestions.Count)
        {
            QuestionData question = currentQuestions[currentQuestionIndex];
            questionTextUI.text = question.questionText;

            for (int i = 0; i < answerButtonsText.Length; i++)
            {
                if (i < question.answers.Length)
                {
                    answerButtons[i].SetActive(true); // Ativa o botão
                    answerButtonsText[i].text = question.answers[i];
                }
                else
                {
                    answerButtons[i].SetActive(false); // Desativa botões extras, se houver
                }
            }
        }
        else
        {
            EndQuiz(); // Todas as perguntas foram respondidas
        }
    }

    public void OnAnswerSelected(int selectedAnswerIndex)
    {
        if (!quizActive) return;

        QuestionData currentQuestion = currentQuestions[currentQuestionIndex];

        if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
        {
            currentScore += pointsPerCorrectAnswer;
            Debug.Log("Correto!");
        }
        else
        {
            Debug.Log("Incorreto. A resposta correta era: " + currentQuestion.answers[currentQuestion.correctAnswerIndex]);
        }

        UpdateScoreUI();
        currentQuestionIndex++;
        DisplayQuestion();
    }

    void UpdateScoreUI()
    {
        scoreTextUI.text = "Pontuação: " + currentScore.ToString();
    }

    void EndQuiz()
    {
        quizActive = false;
        resultPanel.SetActive(true);
        finalScoreTextUI.text = "Sua pontuação final: " + currentScore.ToString();
        Debug.Log("Fim do Quiz! Pontuação: " + currentScore);
    }

    public void RestartQuiz()
    {
        InitializeQuiz();
    }
}