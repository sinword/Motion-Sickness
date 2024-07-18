using UnityEngine.UI;
using UnityEngine;

public class ImageUploader : MonoBehaviour
{
    // Upload image when button is clicked
    [SerializeField]
    private Image imageArea;
    public void UploadImage()
    {
        Debug.Log("Image uploaded");
        imageArea.sprite = Resources.Load<Sprite>("SamplePhoto");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
