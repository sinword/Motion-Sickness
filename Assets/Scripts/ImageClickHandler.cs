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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(leftImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor);

        // Transform the localCursor to the image's pivot point
        Rect rect = leftImage.rectTransform.rect;
        localCursor.x =  (localCursor.x - rect.x) / rect.width * leftImage.sprite.texture.width;
        localCursor.y =  (localCursor.y - rect.y) / rect.height * leftImage.sprite.texture.height;

        StartCoroutine(HandleClick(localCursor));
    }

    private IEnumerator HandleClick(Vector2 position)
    {
        Color leftPixelColor = leftImage.sprite.texture.GetPixel((int)position.x, (int)position.y);
        Color rightPixelColor = rightImage.sprite.texture.GetPixel((int)position.x, (int)position.y);

        GameObject leftCircle = Instantiate(circlePrefab, leftImage.transform);
        GameObject rightCircle = Instantiate(circlePrefab, rightImage.transform);
        leftCircle.transform.localPosition = position;
        rightCircle.transform.localPosition = position;

        if (leftPixelColor != rightPixelColor)
        {
            leftCircle.GetComponent<Image>().color = Color.green;
            rightCircle.GetComponent<Image>().color = Color.green;
        }
        else
        {
            leftCircle.GetComponent<Image>().color = Color.red;
            rightCircle.GetComponent<Image>().color = Color.red;

            yield return new WaitForSeconds(displayTime);

            Destroy(leftCircle);
            Destroy(rightCircle);
        }
    }
}
