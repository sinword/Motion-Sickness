using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public GameObject choice1;
    public GameObject choice2;
    public GameObject answer;

    public float attachDistance;
    public float attachSpeed;
    public float distanceThreshold;
    private Vector3 originalPosition1;
    private Vector3 originalPosition2;
    private Quaternion originRotation1;
    private Quaternion originRotation2;

    private bool isChoice1Attached = false;
    private bool isChoice2Attached = false;
    

    void Start()
    {
        originalPosition1 = choice1.transform.position;
        originalPosition2 = choice2.transform.position;
        originRotation1 = choice1.transform.rotation;
        originRotation2 = choice2.transform.rotation;
    }

    void Update()
    {
        float distanceToAnswer1 = Vector3.Distance(choice1.transform.position, answer.transform.position);
        float distanceToAnswer2 = Vector3.Distance(choice2.transform.position, answer.transform.position);
        Debug.Log("Distance to answer 1: " + distanceToAnswer1.ToString());
        Debug.Log("Distance to answer 2: " + distanceToAnswer2.ToString());

        if ((!isChoice1Attached) && (distanceToAnswer1 - 0.3 < attachDistance)) {
            answer.transform.Find("PromptText").GetComponent<TextMeshPro>().text = "";
            answer.transform.Find("AnswerText").GetComponent<TextMeshPro>().text = "A";

            choice1.transform.position = Vector3.Lerp(choice1.transform.position, answer.transform.position, Time.deltaTime * attachSpeed);
            choice1.transform.rotation = Quaternion.Lerp(choice1.transform.rotation, answer.transform.rotation, Time.deltaTime * attachSpeed);
            if (distanceToAnswer1 < distanceThreshold) {
                Debug.LogWarning("Attach choice 1 to answer");
                isChoice1Attached = true;
            }
            resetChoice2();
        }
        else if ((!isChoice2Attached) && (distanceToAnswer2 - 0.3 < attachDistance)) {
            answer.transform.Find("PromptText").GetComponent<TextMeshPro>().text = "";
            answer.transform.Find("AnswerText").GetComponent<TextMeshPro>().text = "B";

            choice2.transform.position = Vector3.Lerp(choice2.transform.position, answer.transform.position, Time.deltaTime * attachSpeed);
            choice2.transform.rotation = Quaternion.Lerp(choice2.transform.rotation, answer.transform.rotation, Time.deltaTime * attachSpeed);
            if (distanceToAnswer2 < distanceThreshold) {
                Debug.LogWarning("Attach choice 2 to answer");
                isChoice2Attached = true;
            }
            resetChoice1();
        }
    }

    void resetChoice1() {
        choice1.transform.position = originalPosition1;
        choice1.transform.rotation = originRotation1;
        choice1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isChoice1Attached = false;
    }

    void resetChoice2() {
        choice2.transform.position = originalPosition2;
        choice2.transform.rotation = originRotation2;
        choice2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isChoice2Attached = false;
    }
}
