using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class board_space : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        GameObject close = CreateButton(canvas.transform, new Vector2(300, 150), new Vector2(80, 100), close_scene);
        scene = SceneManager.GetSceneAt(1);
        // close.onClick.AddListener(close_scene);
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    public GameObject CreateButton(Transform panel ,Vector2 position, Vector2 size, UnityEngine.Events.UnityAction method) {
        GameObject button = new GameObject();
        // button.transform.parent = panel;
        RectTransform trans = button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.GetComponent<Button>().onClick.AddListener(method);
        trans.transform.SetParent(panel);
        trans.sizeDelta = size;
        trans.localScale = Vector3.one;
        trans.localPosition = position;
        Image image = button.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>("coin_1");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        button.transform.SetParent(panel);
        return button;
}

    void close_scene() {
        SceneManager.UnloadSceneAsync(scene);
    }
}
