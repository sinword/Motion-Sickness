using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
    public Sprite left_image { get; set; }
    public Sprite right_image { get; set; }
}

public class ImageManager : MonoBehaviour
{
    public int imageNumber = 10;
    public List<Question> questions = new List<Question>();

    void Awake()
    {
        // Load images
        for (int i = 0; i < imageNumber; i++)
        {
            string leftImagePath = "SpotTheDiffImages/Easy/Q" + i + "/left_image" + i;
            string rightImagePath = "SpotTheDiffImages/Easy/Q" + i + "/right_image" + i;

            Debug.LogWarning("Loading left image: " + leftImagePath);
            Debug.LogWarning("Loading right image: " + rightImagePath);

            Sprite leftSprite = Resources.Load<Sprite>(leftImagePath);
            Sprite rightSprite = Resources.Load<Sprite>(rightImagePath);
    
            questions.Add(new Question { left_image = leftSprite, right_image = rightSprite });
        }
    }

    void Start()
    {
        // Test load
        foreach (Question question in questions)
        {
            Debug.LogWarning("Left image: " + question.left_image);
            Debug.LogWarning("Right image: " + question.right_image);
        }
    }

}
