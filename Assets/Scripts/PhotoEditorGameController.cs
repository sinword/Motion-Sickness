using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoEditorGameController : MonoBehaviour
{
    [SerializeField]
    private GameObject editPanel;
    [SerializeField]
    private GameObject uploadPanel;
    private GameObject editArea;
    private GameObject effectSliders;

    private Sprite selectedPhoto;

    void OnEnable()
    {
        PhotoUploadPanelController.OnUploadButtonClicked += HandleUploadButtonClicked;
        PhotoEditPanelController.OnSelectedPhotoClicked += HandleSelectedPhotoClicked;
    }

    void OnDisable()
    {
        PhotoUploadPanelController.OnUploadButtonClicked -= HandleUploadButtonClicked;
        PhotoEditPanelController.OnSelectedPhotoClicked -= HandleSelectedPhotoClicked;
    }

    void Start()
    {
        editPanel.SetActive(true);
        uploadPanel.SetActive(false);
        editArea = editPanel.transform.Find("EditArea").gameObject;
        effectSliders = editArea.transform.Find("EffectSliders").gameObject;
    }

    void Update()
    {
        
    }

    public void SetSelectedPhoto(Sprite photo)
    {
        selectedPhoto = photo;
        Debug.Log("Selected photo: " + selectedPhoto.name);
    }

    private void HandleUploadButtonClicked()
    {
        if (selectedPhoto != null)
        {
            Debug.Log("Selected photo: " + selectedPhoto.name);
            editPanel.SetActive(true);
            uploadPanel.SetActive(false);

            Transform photoAreaTransform = editPanel.transform.Find("PhotoArea");
            GameObject photoArea = photoAreaTransform.gameObject;
            photoArea.transform.Find("SelectButton").gameObject.SetActive(false);

            // Set the selected photo to the Image component
            Image photoAreaImage = photoArea.GetComponent<Image>();
            if (photoAreaImage != null)
            {
                photoAreaImage.sprite = selectedPhoto;
            }
            else
            {
                Debug.LogWarning("Image component not found on PhotoArea");
            }
        }
    }

    private void HandleSelectedPhotoClicked()
    {
        editPanel.SetActive(false);
        uploadPanel.SetActive(true);
    }
}
