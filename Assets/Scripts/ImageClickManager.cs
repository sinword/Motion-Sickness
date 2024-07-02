using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageClickManager : MonoBehaviour
{
    public ImageClickHandler imageClickHandler1;
    public ImageClickHandler imageClickHandler2;
    public GameObject circlePrefab;

    private float displayTime = 1.0f;
    private List<GameObject> circles = new List<GameObject>();

    void Start()
    {
        if (imageClickHandler1 != null)
        {
            imageClickHandler1.OnImageClick += HandleImageClick;
        }
        
        if (imageClickHandler2 != null)
        {
            imageClickHandler2.OnImageClick += HandleImageClick;
        }
    }

    private void HandleImageClick(Vector2 localCursor, Vector2 imagePosition, Image currentImage, Image anotherImage)
    {
        StartCoroutine(HandleClick(localCursor, imagePosition, currentImage, anotherImage));
    }

    private IEnumerator HandleClick(Vector2 localCursor, Vector2 imagePosition, Image currentImage, Image anotherImage)
    {
        GameObject currentCircle = Instantiate(circlePrefab, currentImage.transform);
        GameObject anotherCircle = Instantiate(circlePrefab, anotherImage.transform);
        currentCircle.transform.localPosition = localCursor;
        anotherCircle.transform.localPosition = localCursor;

        if (ImageCompare(currentImage, anotherImage, imagePosition))
        {
            currentCircle.GetComponent<Image>().color = Color.green;
            anotherCircle.GetComponent<Image>().color = Color.green;
            circles.Add(currentCircle);
            circles.Add(anotherCircle);
        }
        else
        {
            currentCircle.GetComponent<Image>().color = Color.red;
            anotherCircle.GetComponent<Image>().color = Color.red;

            yield return new WaitForSeconds(displayTime);
            Destroy(currentCircle);
            Destroy(anotherCircle);
        }
    }

    private bool ImageCompare(Image currentImage, Image anotherImage, Vector2 position)
    {
        Color currentPixelColor = currentImage.sprite.texture.GetPixel((int)position.x, (int)position.y) * 255;
        Color anotherPixelColor = anotherImage.sprite.texture.GetPixel((int)position.x, (int)position.y) * 255;

        int pixelThreshold = 40;
        if (Mathf.Abs(currentPixelColor.r - anotherPixelColor.r) > pixelThreshold ||
            Mathf.Abs(currentPixelColor.g - anotherPixelColor.g) > pixelThreshold ||
            Mathf.Abs(currentPixelColor.b - anotherPixelColor.b) > pixelThreshold)
        {
            Debug.LogWarning("Right: " + currentPixelColor);
            Debug.LogWarning("Left: " + anotherPixelColor);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearCircles()
    {
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();
    }
}
