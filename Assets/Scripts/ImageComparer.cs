using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageComparer : MonoBehaviour
{
    public Image leftImage;
    public Image rightImage;

    public List<Vector2> CompareImages()
    {
        Texture2D leftTexture = leftImage.sprite.texture;
        Texture2D rightTexture = rightImage.sprite.texture;

        List<Vector2> differentPixels = new List<Vector2>();

        for (int y = 0; y < leftTexture.height; y++)
        {
            for (int x = 0; x < leftTexture.width; x++)
            {
                Color leftPixelColor = leftTexture.GetPixel(x, y);
                Color rightPixelColor = rightTexture.GetPixel(x, y);

                if (leftPixelColor != rightPixelColor)
                {
                    differentPixels.Add(new Vector2(x, y));
                }
            }
        }

        return differentPixels;
    }
}
