using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    public delegate void ClickEventHandler(Vector2 localCursor, Vector2 imagePosition, Image currentImage, Image anotherImage);
    public event ClickEventHandler OnImageClick;

    public Image currentImage;
    public Image anotherImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(currentImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(currentImage.rectTransform, eventData.pressPosition, eventData.pressEventCamera))
            {
                Rect rect = currentImage.rectTransform.rect;
                float x = (localCursor.x - rect.x) * (currentImage.sprite.texture.width / rect.width);
                float y = (localCursor.y - rect.y) * (currentImage.sprite.texture.height / rect.height);
                Vector2 imagePosition = new Vector2(x, y);

                OnImageClick?.Invoke(localCursor, imagePosition, currentImage, anotherImage);
            }
        }
    }  
}
