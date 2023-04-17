using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MatchWidth : MonoBehaviour {

    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = 105;

    CinemachineVirtualCamera vcam;

    int old_width = 0;

    void Start() {
        vcam = GetComponent<CinemachineVirtualCamera>();
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        vcam.m_Lens.OrthographicSize = desiredHalfHeight;
        old_width = Screen.width;
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update() {
        if (old_width != Screen.width) {
            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
            vcam.m_Lens.OrthographicSize = desiredHalfHeight;
            // Debug.Log(vcam.m_Lens.OrthographicSize);
            old_width = Screen.width;
        }
    }
}