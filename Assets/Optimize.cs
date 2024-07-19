using UnityEngine;

public class ForceOptimize : MonoBehaviour
{
    void Start()
    { 
        // Set lower resolution, high end screens will run the game on 4k without this
        Screen.SetResolution(1280, 720, true);

        // Set target frame rate to 60 to save battery and reduce heat
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        // Force disable filters, will toggle if not mentioned
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.antiAliasing = 0;
        QualitySettings.lodBias = 0.5f; // might make the game look good?
    }
}