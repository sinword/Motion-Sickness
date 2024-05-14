using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MildSceneGameController : MonoBehaviour
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

    private TextMeshProUGUI readingTimeText;
    private TextMeshProUGUI answeringTimeText;
    private TextMeshProUGUI feedbackTimeText;
    private TextMeshProUGUI answerText;

    private List<string> answersLog = new List<string>();
    private float timeLeft;
    private enum GameState { Reading, Answer, Feedback };
    private GameState state = GameState.Reading;
    void Start() {
        readingTimeText = readingCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answeringTimeText = answeringCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answerText = answeringCanvas.transform.Find("Answer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        feedbackTimeText = feedbackCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();

        readingCanvas.SetActive(true);
        answeringCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        
        timeLeft = readingTime;
    }
    void Update()
    {
        // foreach (OVRInput.Button button in Enum.GetValues(typeof(OVRInput.Button)))
        // {
        //     if (OVRInput.GetDown(button))
        //     {
        //         Debug.LogWarning("Button pressed: " + button.ToString());
        //     }
        // }

        if (state == GameState.Reading) {
            timeLeft -= Time.deltaTime;
            readingTimeText.text = ((int)timeLeft).ToString();
            if (timeLeft <= 0) {
                state = GameState.Answer;
                readingTimeText.text = ((int)timeLeft).ToString();
                readingCanvas.SetActive(false);
                answeringCanvas.SetActive(true);
                timeLeft = answeringTime;
                answerText.text = "Press the button on the cotroller.";
                answerText.fontSize = 36;
            }
        } else if (state == GameState.Answer) {
            timeLeft -= Time.deltaTime;
            answeringTimeText.text = ((int)timeLeft).ToString();
            if (timeLeft <= 0) {
                state = GameState.Feedback;
                answeringTimeText.text = ((int)timeLeft).ToString();
                answeringCanvas.SetActive(false);
                feedbackCanvas.SetActive(true);
                timeLeft = feedbackTime;
                SaveAnswer(answerText.text);
                PrintAnswer();
            }
            if (OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.GetDown(OVRInput.Button.Four)) { // Left controller
                answerText.text = "A";
                answerText.fontSize = 100;
            }
            else if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Two)) { // Right controller
                answerText.text = "B";
                answerText.fontSize = 100;
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

    void SaveAnswer(string answer) {
        answersLog.Add(answer);
    }

    void PrintAnswer() {
        string tmp = "";
        foreach (string answer in answersLog) {
            tmp += answer + " ";
        }
        Debug.LogWarning(tmp);
    }
}
