using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SevereSceneGameController : MonoBehaviour
{
    // Create a timer that counts down from 20 seconds
    public float readingTime;
    public float answeringTime;
    public float feedbackTime;
    [SerializeField]
    private GameObject readingCanvas;
    [SerializeField]
    private GameObject answeringCanvas;
    [SerializeField]
    private GameObject feedbackCanvas;
    [SerializeField]
    private GameObject interactableAnswer;
    [SerializeField]
    private TextMeshPro promptText;
    [SerializeField]
    private TextMeshPro answerText;
    private GameObject choice1;
    private GameObject choice2;

    private TextMeshProUGUI readingTimeText;
    private TextMeshProUGUI answeringTimeText;
    private TextMeshProUGUI feedbackTimeText;

    // Rotation and position of choice 1 and choice 2
    private Vector3 choice1Position;
    private Vector3 choice2Position;
    private Quaternion choice1Rotation;
    private Quaternion choice2Rotation;


    private List<string> answersLog = new List<string>();
    private float timeLeft;
    private enum GameState { Reading, Answer, Feedback };
    private GameState state = GameState.Reading;

    void Start() {
        readingTimeText = readingCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answeringTimeText = answeringCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        feedbackTimeText = feedbackCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        choice1 = interactableAnswer.transform.Find("Choice 1").gameObject;
        choice2 = interactableAnswer.transform.Find("Choice 2").gameObject;
        choice1Position = choice1.transform.position;
        choice2Position = choice2.transform.position;
        choice1Rotation = choice1.transform.rotation;
        choice2Rotation = choice2.transform.rotation;

        readingCanvas.SetActive(true);
        answeringCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        interactableAnswer.SetActive(false);
        timeLeft = readingTime;
    }

    void Update()
    {
        if (state == GameState.Reading) {
            timeLeft -= Time.deltaTime;
            readingTimeText.text = ((int)timeLeft).ToString();
            if (timeLeft <= 0) {
                state = GameState.Answer;
                readingTimeText.text = ((int)timeLeft).ToString();
                readingCanvas.SetActive(false);
                answeringCanvas.SetActive(true);
                interactableAnswer.SetActive(true);
                timeLeft = answeringTime;
            }
        } else if (state == GameState.Answer) {
            timeLeft -= Time.deltaTime;
            answeringTimeText.text = ((int)timeLeft).ToString();
            if (timeLeft <= 0) {
                state = GameState.Feedback;
                answeringTimeText.text = ((int)timeLeft).ToString();
                answeringCanvas.SetActive(false);
                interactableAnswer.SetActive(false);
                feedbackCanvas.SetActive(true);

                timeLeft = feedbackTime;
                SaveAnswer(answerText.text);
                promptText.text = "Drag your answer here.";
                answerText.text = "";
                
                PrintAnswer();
                ResetButtonPosition();
            }
        }
        else if (state == GameState.Feedback) {
            timeLeft -= Time.deltaTime;
            feedbackTimeText.text = ((int)timeLeft).ToString();
            if (timeLeft <= 0) {
                state = GameState.Reading;
                feedbackTimeText.text = ((int)timeLeft).ToString();
                feedbackCanvas.SetActive(false);
                readingCanvas.SetActive(true);
                timeLeft = readingTime;
            }
        }
    }


    void ResetButtonPosition() {
        // Reset choice 1 and choice 2
        choice1.transform.position = choice1Position;
        choice2.transform.position = choice2Position;
        choice1.transform.rotation = choice1Rotation;
        choice2.transform.rotation = choice2Rotation;
    }

    void SaveAnswer(string answerText) {
        answersLog.Add(answerText);
    }

    void PrintAnswer() {
        string tmp = "";
        foreach (string answer in answersLog) {
            tmp += answer + " ";
        }
        Debug.LogWarning(tmp);
    }
}
