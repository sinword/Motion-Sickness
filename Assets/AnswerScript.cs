using UnityEngine;
using Oculus.Interaction;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class AnswerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject interactableAnswer;
    [SerializeField]
    private TextMeshPro answerText;
    [SerializeField]
    private GameObject choice1;
    [SerializeField]
    private GameObject choice2;
    private Vector3 choice1Position;
    private Vector3 choice2Position;
    // Rotation of choice 1 and choice 2
    private Vector3 choice1Rotation;
    private Vector3 choice2Rotation;
    private GameObject answer;

    void Start()
    {
        // choice1 = interactableAnswer.transform.Find("Choice 1").gameObject;
        // choice2 = interactableAnswer.transform.Find("Choice 2").gameObject;
        answer = interactableAnswer.transform.Find("Answer").gameObject;
        choice1Position = choice1.transform.position;
        choice2Position = choice2.transform.position;
        choice1Rotation = choice1.transform.rotation.eulerAngles;
        choice2Rotation = choice2.transform.rotation.eulerAngles;
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision) {
        Debug.LogWarning(collision.gameObject.name);
        if (collision.gameObject.name == "Choice 1") {
            Debug.LogWarning("Collision with Choice 1 detected.");
            answerText.text = "A";
            choice1.transform.position = choice1Position;
            choice1.transform.rotation = Quaternion.Euler(choice1Rotation);
        }
        if (collision.gameObject.name == "Choice 2") {
            Debug.LogWarning("Collision with Choice 1 detected.");
            answerText.text = "B";
            choice2.transform.position = choice2Position;
            choice2.transform.rotation = Quaternion.Euler(choice2Rotation);
        }
    }
}
