using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SevereSceneGameController : MonoBehaviour
{
    // Create a timer that counts down from 20 seconds
    public float readingTime;
    public float answeringTime;
    [SerializeField]
    private GameObject readingCanvas;
    [SerializeField]
    private GameObject answeringCanvas;
    [SerializeField]
    private GameObject interactableAnswer;


    private TextMeshProUGUI readingTimeText;
    private TextMeshProUGUI answeringTimeText;
    private TextMeshProUGUI answerText;

    private List<string> answersLog = new List<string>();
    private float timeLeft;
    private enum GameState { Reading, Answer };
    private GameState state = GameState.Reading;
    void Start() {
        readingTimeText = readingCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answeringTimeText = answeringCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answerText = answeringCanvas.transform.Find("Answer").gameObject.GetComponentInChildren<TextMeshProUGUI>();

        readingCanvas.SetActive(true);
        answeringCanvas.SetActive(false);
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
                state = GameState.Reading;
                answeringTimeText.text = ((int)timeLeft).ToString();
                readingCanvas.SetActive(true);
                answeringCanvas.SetActive(false);
                interactableAnswer.SetActive(false);
                timeLeft = readingTime;

                answersLog.Add(answerText.text);
                string tmp = "";
                foreach (string answer in answersLog) {
                    tmp += answer + " ";
                }
                Debug.LogWarning(tmp);
            }

        }
    }
}
