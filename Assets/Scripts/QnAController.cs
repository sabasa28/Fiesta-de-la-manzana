using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class QnAController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Image questionSprite;
    [SerializeField] TextMeshProUGUI[] optionsText;
    [SerializeField] Image[] optionsSprite;
    [SerializeField] GameObject[] optionsButtons;
    [SerializeField] Transform questionTextAlonePos;
    [SerializeField] Transform questionTextWithImagePos;
    Question currentQuestion;
    int currentCuestionIndex = 0;
    [SerializeField] Question[] questionsData;
    [SerializeField] List<int> questionsOrder = new List<int>();
    [SerializeField] int numOfQuestionsAsked;
    bool changingQuestion = false;
    void Start()
    {
        questionsOrder.Clear();
        for (int i = 0; i < numOfQuestionsAsked; i++)
        {
            questionsOrder.Add(-1);
        }
        List<int> questionsNumbers = new List<int>();
        for (int i = 0; i < questionsData.Length; i++)
        {
            questionsNumbers.Add(i);
        }
        for (int i = 0; i < questionsOrder.Count; i++)
        {
            int questionIndex = Random.Range(0, questionsNumbers.Count);
            questionsOrder[i] = questionsNumbers[questionIndex];
            questionsNumbers.RemoveAt(questionIndex);
        }

        currentQuestion = questionsData[questionsOrder[currentCuestionIndex]];
        SetUIToQuestionData(currentQuestion);
    }

    void SetUIToQuestionData(Question question)
    {
        foreach (GameObject button in optionsButtons)
        {
            button.SetActive(true);
        }
        questionText.text = question.questionText;
        if (question.questionImage)
        {
            questionSprite.sprite = question.questionImage;
            questionText.transform.position = questionTextWithImagePos.position;
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
                }
                else
                {
                    optionsSprite[i].gameObject.SetActive(false);
                }
            }
            else
            {
                optionsButtons[i].SetActive(false);
                optionsText[i].text = "";
                optionsSprite[i].gameObject.SetActive(false);
            }
        }
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
            Debug.Log("CORRECTOOOO");
            StartCoroutine(WaitAndChangeQuestion());
        }
        else
        {
            Debug.Log("MAAAAAAAAAL");
            StartCoroutine(WaitAndChangeQuestion());
        }
    }

    IEnumerator WaitAndChangeQuestion()
    {
        changingQuestion = true;
        yield return new WaitForSeconds(3);
        changingQuestion = false;
        currentCuestionIndex++;
        if (currentCuestionIndex >= questionsOrder.Count)
        {
            yield break;
        }
        currentQuestion = questionsData[questionsOrder[currentCuestionIndex]];
        SetUIToQuestionData(currentQuestion);
    }
}
