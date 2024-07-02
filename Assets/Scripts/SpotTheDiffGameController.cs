using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Import the UI namespace

public class SpotTheDiffGameController : MonoBehaviour
{
    public float answeringTime;
    public float feedbackTime;
    
    [SerializeField]
    private GameObject interactableCanvas;
    [SerializeField]
    private ImageManager imageManager;
    [SerializeField]
    private ImageClickManager imageClickManager;
    private GameObject spotTheDiffCanvas;
    private GameObject feedbackCanvas;
    private TextMeshProUGUI questionTitle;
    private TextMeshProUGUI answerTimeText;
    private TextMeshProUGUI feedbackTimeText;
    private float timeLeft;
    private enum GameState { Answer, Feedback };
    private GameState state;
    private int index;
    private Sprite leftImage;
    private Sprite rightImage;
    private Image leftUIImage;
    private Image rightUIImage;
    
    void Start()
    {
        index = 0;
        spotTheDiffCanvas = interactableCanvas.transform.Find("SpotTheDiff Canvas").gameObject;
        feedbackCanvas = interactableCanvas.transform.Find("Feedback Canvas").gameObject;
        questionTitle = spotTheDiffCanvas.transform.Find("Title").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        answerTimeText = spotTheDiffCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        feedbackTimeText = feedbackCanvas.transform.Find("Timer").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        // Get the Images for the images
        leftUIImage = spotTheDiffCanvas.transform.Find("Left Image").GetComponent<Image>();
        rightUIImage = spotTheDiffCanvas.transform.Find("Right Image").GetComponent<Image>();

        // Check if the Images are attached
        if (leftUIImage == null)
        {
            Debug.LogError("Missing Image component on Left Image");
            return;
        }

        if (rightUIImage == null)
        {
            Debug.LogError("Missing Image component on Right Image");
            return;
        }

        if (index < imageManager.questions.Count)
        {
            leftImage = imageManager.questions[index].left_image;
            rightImage = imageManager.questions[index].right_image;
            leftUIImage.sprite = leftImage;
            rightUIImage.sprite = rightImage;
        }
        else
        {
            Debug.LogError("Index out of range for imageManager.questions");
            return;
        }
        
        state = GameState.Answer;
        interactableCanvas.SetActive(true);
        feedbackCanvas.SetActive(false);
        timeLeft = answeringTime;
    }

    void Update()
    {
        if (index < imageManager.questions.Count) 
        {
            if (state == GameState.Answer)
            {
                timeLeft -= Time.deltaTime;
                answerTimeText.text = ((int)timeLeft).ToString();
                if (timeLeft <= 0)
                {
                    imageClickManager.ClearCircles();
                    state = GameState.Feedback;
                    interactableCanvas.SetActive(false);
                    feedbackCanvas.SetActive(true);
                    timeLeft = feedbackTime;
                }
            }
            else if (state == GameState.Feedback)
            {
                timeLeft -= Time.deltaTime;
                feedbackTimeText.text = ((int)timeLeft).ToString();
                if (timeLeft <= 0)
                {
                    index++;
                    if (index < imageManager.questions.Count)
                    {
                        leftImage = imageManager.questions[index].left_image;
                        rightImage = imageManager.questions[index].right_image;
                        leftUIImage.sprite = leftImage;
                        rightUIImage.sprite = rightImage;
                        state = GameState.Answer;
                        interactableCanvas.SetActive(true);
                        feedbackCanvas.SetActive(false);
                        timeLeft = answeringTime;
                        questionTitle.text = "Question " + (index + 1);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No more questions available.");
        }
    }
}
