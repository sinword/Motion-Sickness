using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Image rightImage;
    public Image leftImage;
    public GameObject circlePrefab;
    private float displayTime = 1.0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rightImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rightImage.rectTransform, eventData.pressPosition, eventData.pressEventCamera))
            {
                // Convert to image space
                Rect rect = rightImage.rectTransform.rect;
                float x = (localCursor.x - rect.x) * (rightImage.sprite.texture.width / rect.width);
                float y = (localCursor.y - rect.y) * (rightImage.sprite.texture.height / rect.height);
                Vector2 imagePosition = new Vector2(x, y);

                StartCoroutine(HandleClick(imagePosition, localCursor));
            }
        }
    }

    private IEnumerator HandleClick(Vector2 imagePosition, Vector2 localCursor)
    {
        GameObject rightCircle = Instantiate(circlePrefab, rightImage.transform);
        GameObject leftCircle = Instantiate(circlePrefab, leftImage.transform);
        rightCircle.transform.localPosition = localCursor;
        leftCircle.transform.localPosition = localCursor;

        if (ImageCompare(imagePosition))
        {
            // Diff
            rightCircle.GetComponent<Image>().color = Color.green;
            leftCircle.GetComponent<Image>().color = Color.green;
        }
        else
        {
            // No diff
            rightCircle.GetComponent<Image>().color = Color.red;
            leftCircle.GetComponent<Image>().color = Color.red;

            yield return new WaitForSeconds(displayTime);
            Destroy(rightCircle);
            Destroy(leftCircle);
        }
    }

    private bool ImageCompare(Vector2 position)
    {
        Color rightPixelColor = rightImage.sprite.texture.GetPixel((int)position.x, (int)position.y) * 255;
        Color leftPixelColor = leftImage.sprite.texture.GetPixel((int)position.x, (int)position.y) * 255;
        
        int pixelThreshold = 50;
        if (Mathf.Abs(rightPixelColor.r - leftPixelColor.r) > pixelThreshold ||
            Mathf.Abs(rightPixelColor.g - leftPixelColor.g) > pixelThreshold ||
            Mathf.Abs(rightPixelColor.b - leftPixelColor.b) > pixelThreshold)
        {
            Debug.LogWarning("Right: " + rightPixelColor);
            Debug.LogWarning("Left: " + leftPixelColor);
            return true;
        }
        else
        {
            return false;
        }

    }
}
