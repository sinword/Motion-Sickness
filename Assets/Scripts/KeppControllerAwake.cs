using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepControllerAwake : MonoBehaviour
{
    public Transform controller;
    public float checkInterval = 0.05f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0;
            Debug.LogWarning("Position of controller: " + controller.position);
        }
    }
}
