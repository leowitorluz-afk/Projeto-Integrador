using UnityEngine;
using TMPro;
using UnityEngine.UI; // <--- OBRIGATÓRIO PARA USAR "Image"
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questionTextUI;
    public TextMeshProUGUI[] answerButtonsText;
    public GameObject[] answerButtons;
    public TextMeshProUGUI timerTextUI;
    public TextMeshProUGUI scoreTextUI;
    public GameObject resultPanel;
    public TextMeshProUGUI finalScoreTextUI;

    [Header("Game Settings")]
    public List<QuestionData> allQuestions;
    public float timeLimit = 60f;
    public int pointsPerCorrectAnswer = 10;
    public float waitTime = 1.5f; // <--- Tempo de espera para ver o resultado

    private List<QuestionData> currentQuestions;
    private int currentQuestionIndex = 0;
    private int currentScore = 0;
    private float currentTime;
    private bool quizActive = false;

    [Header("Configuração dos Ícones")]
    public Image[] answerIcons; // Arraste os objetos "Icone" aqui
    public Sprite iconDefault;  // O Círculo Vazio
    public Sprite iconCorrect;  // O Check (Certo)
    public Sprite iconWrong;    // O X (Errado)

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
        resultPanel.SetActive(false);

        ShuffleQuestions(allQuestions);
        currentQuestions = new List<QuestionData>(allQuestions);

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
            yield return null;
        }

        if (quizActive)
        {
            EndQuiz();
        }
    }

    void DisplayQuestion()
    {
        // Reseta os visuais (volta o círculo vazio e destrava botões)
        ResetButtonVisuals();

        if (currentQuestionIndex < currentQuestions.Count)
        {
            QuestionData question = currentQuestions[currentQuestionIndex];
            questionTextUI.text = question.questionText;

            for (int i = 0; i < answerButtonsText.Length; i++)
            {
                if (i < question.answers.Length)
                {
                    answerButtons[i].SetActive(true);
                    answerButtonsText[i].text = question.answers[i];
                }
                else
                {
                    answerButtons[i].SetActive(false);
                }
            }
        }
        else
        {
            EndQuiz();
        }
    }

    public void OnAnswerSelected(int selectedAnswerIndex)
    {
        if (!quizActive) return;

        // Trava os botões para não clicar duas vezes
        SetButtonsInteractable(false);

        QuestionData currentQuestion = currentQuestions[currentQuestionIndex];

        // Pega a imagem do ícone do botão que foi clicado
        Image clickedIcon = answerIcons[selectedAnswerIndex];

        if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
        {
            currentScore += pointsPerCorrectAnswer;

            // Muda para o ícone de CHECK
            clickedIcon.sprite = iconCorrect;

            Debug.Log("Correto!");
        }
        else
        {
            // Muda para o ícone de X
            clickedIcon.sprite = iconWrong;

            Debug.Log("Incorreto. A resposta correta era: " + currentQuestion.answers[currentQuestion.correctAnswerIndex]);

            // OPCIONAL: Se quiser mostrar qual era a correta marcando com Check também:
            // answerIcons[currentQuestion.correctAnswerIndex].sprite = iconCorrect;
        }

        UpdateScoreUI();

        // Inicia a espera antes de ir para a próxima
        StartCoroutine(NextQuestionRoutine());
    }

    IEnumerator NextQuestionRoutine()
    {
        // Espera o tempo definido (1.5 segundos)
        yield return new WaitForSeconds(waitTime);

        // Avança o índice e mostra a próxima
        currentQuestionIndex++;
        DisplayQuestion();
    }

    // Função para resetar os ícones para "Bolinha Vazia"
    void ResetButtonVisuals()
    {
        // Destrava os botões
        SetButtonsInteractable(true);

        for (int i = 0; i < answerIcons.Length; i++)
        {
            // Garante que existe um ícone configurado antes de tentar mudar
            if (i < answerIcons.Length && answerIcons[i] != null)
            {
                answerIcons[i].sprite = iconDefault; // Volta para o círculo vazio
            }
        }
    }

    // Função auxiliar para travar/destravar cliques
    void SetButtonsInteractable(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button btn = answerButtons[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = state;
            }
        }
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