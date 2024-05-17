using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.CodeDom.Compiler;

public class MildPressButtonSceneGameController : MonoBehaviour
{
    public float readingTime;
    public float answeringTime;
    public float feedbackTime;
    [SerializeField]
    private GameObject readingCanvas;
    [SerializeField]
    private GameObject answeringCanvas;
    [SerializeField]
    private GameObject feedbackCanvas;
    private TextMeshProUGUI articleTitle;
    private TextMeshProUGUI questionTitle;
    private TextMeshProUGUI articleContent;
    private TextMeshProUGUI questionContent;
    private TextMeshProUGUI choice1Text;
    private TextMeshProUGUI choice2Text;
    private TextMeshProUGUI readingTimeText;
    private TextMeshProUGUI answeringTimeText;
    private TextMeshProUGUI feedbackTimeText;
    private TextMeshProUGUI answerText;

    [SerializeField]
    private StoryManager storyManager;

    private List<string> answersLog = new List<string>();
    private float timeLeft;
    private enum GameState { Reading, Answer, Feedback };
    private GameState state = GameState.Reading;
    private int index;
    void Start() {
        index = 0;
        articleTitle = readingCanvas.transform.Find("Title").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        articleContent = readingCanvas.transform.Find("Content").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        questionTitle = answeringCanvas.transform.Find("Title").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        questionContent = answeringCanvas.transform.Find("Question").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        choice1Text = answeringCanvas.transform.Find("Choices/Choice 1/Text (TMP)").GetComponent<TextMeshProUGUI>();
        choice2Text = answeringCanvas.transform.Find("Choices/Choice 2/Text (TMP)").GetComponent<TextMeshProUGUI>();
        readingTimeText = readingCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answeringTimeText = answeringCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answerText = answeringCanvas.transform.Find("Answer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        feedbackTimeText = feedbackCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();

        readingCanvas.SetActive(true);
        answeringCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        
        timeLeft = readingTime;

        articleTitle.text = "Article " + (index + 1);
        articleContent.text = storyManager.articles[index];
    }
    void Update()
    {
        if (index < storyManager.storyNumber) {
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
                    answerText.fontSize = 15;

                    questionTitle.text = "Question " + (index + 1);
                    questionContent.text = storyManager.LCLQuestion[index];
                    choice1Text.text = storyManager.LCLChoice1[index];
                    choice2Text.text = storyManager.LCLChoice2[index];
                }
            } else if (state == GameState.Answer) {
                timeLeft -= Time.deltaTime;
                answeringTimeText.text = ((int)timeLeft).ToString();
                if (OVRInput.GetDown(OVRInput.Button.One)) {
                    answerText.text = "A";
                    answerText.fontSize = 30;
                }
                else if (OVRInput.GetDown(OVRInput.Button.Two)) {
                    answerText.text = "B";
                    answerText.fontSize = 30;
                }
                if (timeLeft <= 0) {
                    state = GameState.Feedback;
                    answeringTimeText.text = ((int)timeLeft).ToString();
                    answeringCanvas.SetActive(false);
                    feedbackCanvas.SetActive(true);
                    timeLeft = feedbackTime;
                    SaveAnswer(answerText.text);
                    PrintAnswer();
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
                    index++;
                    if (index < storyManager.storyNumber) {
                        articleTitle.text = "Article " + (index + 1);
                        articleContent.text = storyManager.articles[index];
                    }
                    else {
                        readingCanvas.SetActive(false);
                        answeringCanvas.SetActive(false);
                        feedbackCanvas.SetActive(false);
                        int correct = 0;
                        for (int i = 0; i < storyManager.storyNumber; i++) {
                            if (answersLog[i] == storyManager.answers[i]) {
                                correct++;
                            }
                        }
                    }   
                }
            }
        }
    }

    void SaveAnswer(string answer) {
        if (answer != "A" && answer != "B") {
            answer = "X";
        }
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
