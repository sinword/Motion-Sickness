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
    private GameObject choice1;
    private GameObject choice2;

    private TextMeshProUGUI readingTimeText;
    private TextMeshProUGUI answeringTimeText;
    private TextMeshProUGUI answerText;
    private Vector3 choice1Position;
    private Vector3 choice2Position;
    // Rotation of choice 1 and choice 2
    private Vector3 choice1Rotation;
    private Vector3 choice2Rotation;

    private List<string> answersLog = new List<string>();
    private float timeLeft;
    private enum GameState { Reading, Answer };
    private GameState state = GameState.Reading;
    void Start() {
        readingTimeText = readingCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answeringTimeText = answeringCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answerText = answeringCanvas.transform.Find("Answer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        choice1 = interactableAnswer.transform.Find("Choice 1").gameObject;
        choice2 = interactableAnswer.transform.Find("Choice 2").gameObject;
        choice1Position = choice1.transform.position;
        choice2Position = choice2.transform.position;
        choice1Rotation = choice1.transform.rotation.eulerAngles;
        choice2Rotation = choice2.transform.rotation.eulerAngles;

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
                ResetButtonPosition();
                
                answersLog.Add(answerText.text);
                string tmp = "";
                foreach (string answer in answersLog) {
                    tmp += answer + " ";
                }
                Debug.LogWarning(tmp);
            }

        }
    }

    void ResetButtonPosition() {
        // Reset choice 1 and choice 2
        choice1.transform.position = choice1Position;
        choice2.transform.position = choice2Position;
        choice1.transform.rotation = Quaternion.Euler(choice1Rotation);
        choice2.transform.rotation = Quaternion.Euler(choice2Rotation);
    }
}
