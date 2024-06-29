using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class ImageComparisonManager : MonoBehaviour
{
    public ImageComparer comparer;

    void Start()
    {
        List<UnityEngine.Vector2> differences = comparer.CompareImages();
        foreach (UnityEngine.Vector2 diff in differences)
        {
            Debug.Log("Difference at: " + diff);
        }
    }
}
