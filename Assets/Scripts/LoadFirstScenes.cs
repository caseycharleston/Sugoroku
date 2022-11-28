using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFirstScenes : MonoBehaviour
{

    public string scene_to_load;
    public float wait_time;
    public float fade_time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("load_scene", wait_time);
    }

    void load_scene() {
        Initiate.Fade(scene_to_load, Color.white, fade_time);
    }
}
