using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionData : ScriptableObject
{
    public string questionText;
    public string[] answers; // Array de respostas
    public int correctAnswerIndex; // Índice da resposta correta no array
}
