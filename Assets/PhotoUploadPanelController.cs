using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class PhotoUploadPanelController : MonoBehaviour
{
    // Load image from the Assets/Resources/Images folder
    // Display the images in the photo grid
    [SerializeField]
    private GameObject photoGrids;
    [SerializeField]
    private Button uploadButton;

    private Button[][] photoButtons = new Button[5][];
    private string pathToImages = "ImageEditor/Images";

    void Start()
    {   
        
        // Load the images from the Resource folder
        int count = 1;
        for (int i = 0; i < 5; i++)
        {
            photoButtons[i] = new Button[4];

            for (int j = 0; j < 4; j++) 
            {
                Transform photoRowTransform = photoGrids.transform.Find("PhotoRow" + (i + 1));
                if (photoRowTransform != null)
                {
                    Transform photoButtonTransform = photoRowTransform.Find("PhotoButton" + (j + 1));
                    if (photoButtonTransform != null)
                    {
                        photoButtons[i][j] = photoButtonTransform.GetComponent<Button>();
                        Debug.Log("Image Path: " + pathToImages + "/Image" + count);
                        Sprite loadedSprite = Resources.Load<Sprite>(pathToImages + "/Image" + count);
                        if (loadedSprite != null)
                        {
                            photoButtons[i][j].image.sprite = loadedSprite;
                        }
                        else
                        {
                            Debug.LogWarning("Could not load image: " + pathToImages + "/Image" + count);
                        }
                        count++;
                    }
                }
                else
                {
                    Debug.LogWarning("Could not find PhotoRow" + (i + 1));
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Print the image name and turn the button color to blue
    public void OnPhotoButtonClick()
    {
        Debug.Log("Photo button clicked");
        // Get image from the button itself
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // Enable the text under the button
        Debug.LogWarning("Image name: " + clickedButton.image.sprite.name);
        // After any button is clicked, change the color of the upload button to blue
        // Blue color(0.035f, 0.521f, 0.972f, 1.0f);
        ColorBlock colorBlock = uploadButton.colors;
        colorBlock.normalColor = new Color(0.035f, 0.521f, 0.972f, 1.0f);
        uploadButton.colors = colorBlock;
            
    }

    public void OnUploadButtonClick()
    {
        Debug.LogWarning("Upload button clicked");
        // Upload the selected image
    }

}
    