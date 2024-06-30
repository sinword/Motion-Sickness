using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Image leftImage;
    public Image rightImage;
    public GameObject circlePrefab;
    private float displayTime = 1.0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(leftImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(leftImage.rectTransform, eventData.pressPosition, eventData.pressEventCamera))
            {
                // Convert to image space
                Rect rect = leftImage.rectTransform.rect;
                float x = (localCursor.x - rect.x) * (leftImage.sprite.texture.width / rect.width);
                float y = (localCursor.y - rect.y) * (leftImage.sprite.texture.height / rect.height);
                Vector2 imagePosition = new Vector2(x, y);

                StartCoroutine(HandleClick(imagePosition, localCursor));

            }

        }
    }

    private IEnumerator HandleClick(Vector2 imagePosition, Vector2 localCursor)
    {
        GameObject leftCircle = Instantiate(circlePrefab, leftImage.transform);
        GameObject rightCircle = Instantiate(circlePrefab, rightImage.transform);
        leftCircle.transform.localPosition = localCursor;
        rightCircle.transform.localPosition = localCursor;

        if (ImageCompare(imagePosition))
        {
            // No diff
            leftCircle.GetComponent<Image>().color = Color.red;
            rightCircle.GetComponent<Image>().color = Color.red;

            yield return new WaitForSeconds(displayTime);
            Destroy(leftCircle);
            Destroy(rightCircle);
        }
        else
        {
            // Diff
            leftCircle.GetComponent<Image>().color = Color.green;
            rightCircle.GetComponent<Image>().color = Color.green;
        }

        // if (leftPixelColor != rightPixelColor)
        // {
        //     leftCircle.GetComponent<Image>().color = Color.green;
        //     rightCircle.GetComponent<Image>().color = Color.green;
        // }
        // else
        // {
        //     leftCircle.GetComponent<Image>().color = Color.red;
        //     rightCircle.GetComponent<Image>().color = Color.red;

        //     yield return new WaitForSeconds(displayTime);

        //     Destroy(leftCircle);
        //     Destroy(rightCircle);
        // }
    }

    private bool ImageCompare(Vector2 position)
    {
        Color leftPixelColor = leftImage.sprite.texture.GetPixel((int)position.x, (int)position.y);
        Color rightPixelColor = rightImage.sprite.texture.GetPixel((int)position.x, (int)position.y);

        Debug.LogWarning("Left click position:" + position + " Left pixel color:" + leftPixelColor);
        Debug.LogWarning("Right click position:" + position + " Right pixel color:" + rightPixelColor); 

        return leftPixelColor == rightPixelColor;
    }
}
