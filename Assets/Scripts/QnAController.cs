using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Dificulty
{ 
    easy,
    medium,
    hard
}

public class QnAController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Image questionSprite;
    [SerializeField] TextMeshProUGUI[] optionsText;
    [SerializeField] Image[] optionsSprite;
    [SerializeField] Button[] optionsButtons;
    [SerializeField] Transform questionTextAlonePos;
    [SerializeField] Transform questionTextWithImagePos;
    Question currentQuestion;
    int currentCuestionIndex = 0;
    [SerializeField] Question[] easyQuestions;
    [SerializeField] Question[] mediumQuestions;
    [SerializeField] Question[] hardQuestions;
    List<Question> questionsData = new List<Question>();
    Dificulty currentDificulty;
    [SerializeField] List<int> questionsOrder = new List<int>();
    [SerializeField] ColorBlock correctColor;
    [SerializeField] ColorBlock incorrectColor;
    [SerializeField] ColorBlock normalColor;
    [SerializeField] GameObject answersPanel;
    [SerializeField] GameObject[] dificultyButtons;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject returnToMenuButton;
    [SerializeField] string instructionsText;
    int maxPointsPerQuestion = 4;
    int pointsThisQuestion = 4;
    int totalPoints = 0;
    int questionsCorrect = 0;
    bool changingQuestion = false;

    void GetRandomQuestionsOrder()
    {
        Question[] questionsToUse;
        switch (currentDificulty)
        {
            case Dificulty.easy:
                questionsToUse = easyQuestions;
                break;
            case Dificulty.medium:
                questionsToUse = mediumQuestions;
                break;
            case Dificulty.hard:
            default:
                questionsToUse = hardQuestions;
                break;
        }
        foreach (Question question in questionsToUse)
        {
            questionsData.Add(question);
        }
        currentCuestionIndex = 0;
        pointsThisQuestion = 4;
        totalPoints = 0;
        questionsCorrect = 0;
        questionsOrder.Clear();
        for (int i = 0; i < questionsData.Count; i++)
        {
            questionsOrder.Add(i);
        }
        //for (int i = 0; i < numOfQuestionsAsked; i++)
        //{
        //    questionsOrder.Add(-1);
        //}
        //List<int> questionsNumbers = new List<int>();
        //for (int i = 0; i < questionsData.Count; i++)
        //{
        //    questionsNumbers.Add(i);
        //}
        //for (int i = 0; i < questionsOrder.Count; i++)
        //{
        //    int questionIndex = Random.Range(0, questionsNumbers.Count);
        //    questionsOrder[i] = questionsNumbers[questionIndex];
        //    questionsNumbers.RemoveAt(questionIndex);
        //}
        currentQuestion = questionsData[questionsOrder[currentCuestionIndex]];
    }

    void SetUIToQuestionData(Question question)
    {
        foreach (Button button in optionsButtons)
        {
            button.gameObject.SetActive(true);
            button.colors = normalColor;
            button.interactable = true;
        }
        questionText.text = question.questionText;
        if (question.questionImage)
        {
            questionSprite.sprite = question.questionImage;
            questionText.transform.position = questionTextWithImagePos.position;
            questionSprite.gameObject.SetActive(true);
        }
        else
        {
            questionSprite.gameObject.SetActive(false);
            questionText.transform.position = questionTextAlonePos.position;
        }

        for (int i = 0; i < optionsText.Length; i++)
        {
            if (question.options.Length - 1 >= i)
            {
                optionsText[i].text = question.options[i].text;
                if (question.options[i].sprite)
                {
                    optionsSprite[i].sprite = question.options[i].sprite;
                    optionsSprite[i].gameObject.SetActive(true);
                }
                else
                {
                    optionsSprite[i].gameObject.SetActive(false);
                }
            }
            else
            {
                optionsButtons[i].gameObject.SetActive(false);
                optionsText[i].text = "";
                optionsSprite[i].gameObject.SetActive(false);
            }
        }
        pointsThisQuestion = 4;
    }

    public void SelectOptionA()
    {
        CheckIfAnswerIsCorrect(0);
    }
    public void SelectOptionB()
    {
        CheckIfAnswerIsCorrect(1);
    }
    public void SelectOptionC()
    {
        CheckIfAnswerIsCorrect(2);
    }
    public void SelectOptionD()
    {
        CheckIfAnswerIsCorrect(3);
    }
    void CheckIfAnswerIsCorrect(int optionPresssed)
    {
        if (changingQuestion)
        {
            return;
        }
        if (currentQuestion.correctOption == (Option)optionPresssed)
        {
            optionsButtons[optionPresssed].colors = correctColor;
            questionsCorrect++;
            //totalPoints += pointsThisQuestion;
        }
        else
        {
            optionsButtons[optionPresssed].colors = incorrectColor;
            optionsButtons[optionPresssed].interactable = false;
            optionsButtons[(int)currentQuestion.correctOption].colors = correctColor;
            //pointsThisQuestion--;
        }
        StartCoroutine(WaitAndChangeQuestion());
    }

    IEnumerator WaitAndChangeQuestion()
    {
        changingQuestion = true;
        yield return new WaitForSeconds(3);
        changingQuestion = false;
        currentCuestionIndex++;
        if (currentCuestionIndex >= questionsOrder.Count)
        {
            EndGame();
            yield break;
        }
        currentQuestion = questionsData[questionsOrder[currentCuestionIndex]];
        SetUIToQuestionData(currentQuestion);
    }

    public void SetDificultyEasy()
    {
        SetDificulty(Dificulty.easy);
    }

    public void SetDificultyMedium()
    {
        SetDificulty(Dificulty.medium);
    }

    public void SetDificultyHard()
    {
        SetDificulty(Dificulty.hard);
    }

    void SetDificulty(Dificulty newDificulty)
    { 
        currentDificulty = newDificulty;
        questionText.text = instructionsText;
        startGameButton.SetActive(true);
        foreach (GameObject button in dificultyButtons)
        {
            button.SetActive(false);
        }
    }

    public void StartGame()
    {
        answersPanel.SetActive(true);
        returnToMenuButton.SetActive(false);
        playAgainButton.SetActive(false);
        startGameButton.SetActive(false);
        GetRandomQuestionsOrder();
        SetUIToQuestionData(currentQuestion);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("QnA");
    }

    void EndGame()
    {
        answersPanel.SetActive(false);
        returnToMenuButton.SetActive(true);
        playAgainButton.SetActive(true);
        questionText.text = "Felicitaciones! Respondiste " + questionsCorrect + " de " + questionsOrder.Count + " preguntas correctamente!";
        //if (totalPoints < maxPointsPerQuestion * questionsOrder.Count)
        //{
        //    questionText.text += " Volve a jugar para mejorar tu puntaje, pero cuidado, las preguntas pueden cambiar!";
        //}
        //else
        //{ 
        //    questionText.text += " Puntaje perfecto!";
        //}
        if (questionsCorrect == questionsOrder.Count)
        {
            questionText.text += " Puntaje perfecto!";
        }

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
