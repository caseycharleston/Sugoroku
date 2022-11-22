using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public static class InitiateAdd
{
    static bool areWeFading = false;

    //Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(string scene, Color col, float multiplier)
    {
        if (areWeFading)
        {
            Debug.Log("Already Fading");
            return;
        }

        GameObject init = new GameObject();
        init.name = "Fader";
        Canvas myCanvas = init.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        init.AddComponent<FaderAdd>();
        init.AddComponent<CanvasGroup>();
        init.AddComponent<Image>();

        FaderAdd scr = init.GetComponent<FaderAdd>();
        scr.fadeDamp = multiplier;
        scr.fadeScene = scene;
        scr.fadeColor = col;
        scr.start = true;
        areWeFading = true;
        scr.InitiateFaderAdd();
        
    }

    public static void DoneFading() {
        areWeFading = false;
    }
}
