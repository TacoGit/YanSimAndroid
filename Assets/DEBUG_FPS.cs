using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this to use the Text component

public class DEBUG_FPS : MonoBehaviour
{
    public Text fpsText;

    // Update is called once per frame
    void Update()
    {
        float fps = 1.0f / Time.deltaTime; // Calculate FPS
        fpsText.text = "D_FPS: <" + Mathf.Ceil(fps).ToString() + ">"; // Update the text
    }
}