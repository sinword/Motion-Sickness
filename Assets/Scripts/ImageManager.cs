using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Hard
}

public class Question
{
    public Sprite left_image { get; set; }
    public Sprite right_image { get; set; }
    public List<GameObject> differences { get; set; }

    public Question()
    {
        differences = new List<GameObject>();
    }
}

public class ImageManager : MonoBehaviour
{
    [SerializeField]
    private Difficulty difficulty;

    public int imageNumber = 10;
    public List<Question> questions = new List<Question>();

    void Awake()
    {
        string difficultyString = difficulty.ToString();

        // Load images
        for (int i = 0; i < imageNumber; i++)
        {
            // Load images
            string leftImagePath = $"SpotTheDiffImages/{difficultyString}/Q{i}/left_image";
            string rightImagePath = $"SpotTheDiffImages/{difficultyString}/Q{i}/right_image";

            Debug.Log("Loading left image: " + leftImagePath);
            Debug.Log("Loading right image: " + rightImagePath);

            Sprite leftSprite = Resources.Load<Sprite>(leftImagePath);
            Sprite rightSprite = Resources.Load<Sprite>(rightImagePath);

            Question question = new Question { left_image = leftSprite, right_image = rightSprite };

            // Load differences
            for (int j = 1; j <= 8; j++)
            {
                string buttonName = $"Button ({j})";
                GameObject diff = GameObject.Find(buttonName);
                question.differences.Add(diff);
                Debug.LogWarning("Loading difference: " + buttonName);
            }

            questions.Add(question);
        }
    }

    void Start()
    {
        // Test load
        foreach (Question question in questions)
        {
            Debug.LogWarning("Left image: " + question.left_image);
            Debug.LogWarning("Right image: " + question.right_image);

            foreach (GameObject diff in question.differences)
            {
                Debug.LogWarning("Difference: " + diff.name);
            }
        }
    }
}
