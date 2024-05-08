using UnityEngine;
using Oculus.Interaction;
using TMPro;

public class AnswerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject interactableAnswer;
    private GameObject choice1;
    private GameObject choice2;
    private GameObject answer;

    private Vector3 choice1OriginalPosition;
    private Vector3 choice2OriginalPosition;

    private Grabbable choice1Grabbable;
    private Grabbable choice2Grabbable;

    void Start()
    {
        choice1 = interactableAnswer.transform.Find("Choice 1").gameObject;
        choice2 = interactableAnswer.transform.Find("Choice 2").gameObject;
        answer = interactableAnswer.transform.Find("Answer").gameObject;
        choice1OriginalPosition = choice1.transform.position;
        choice2OriginalPosition = choice2.transform.position;

        choice1Grabbable = choice1.GetComponent<Grabbable>();
        choice2Grabbable = choice2.GetComponent<Grabbable>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == choice1) {
            SetAnswerText(choice1.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        else if (collision.gameObject == choice2) {
            SetAnswerText(choice2.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }

    void SetAnswerText(string text) {
        answer.GetComponentInChildren<TextMeshProUGUI>().text = text;
        // Displace the material of the answer to the dragged choice
        answer.GetComponent<Renderer>().material = choice1.GetComponent<Renderer>().material;
    }
}
