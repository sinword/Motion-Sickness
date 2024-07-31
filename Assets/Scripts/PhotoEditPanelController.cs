using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoEditPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectButton;

    public delegate void SelectedPhotoClicked();
    public static event SelectedPhotoClicked OnSelectedPhotoClicked;
    private PhotoEditorGameController photoEditorGameController;
    void Start()
    {
        photoEditorGameController = FindObjectOfType<PhotoEditorGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectedPhotoClick()
    {
        if (OnSelectedPhotoClicked != null)
        {
            OnSelectedPhotoClicked?.Invoke();
        }
    }
}