using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class board_space : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Player curr_player = main.curr_player;

    // Start is called before the first frame update
    void Start()
    {                                                   //sets position             sets size        onclick function
        GameObject close = CreateButton(canvas.transform, new Vector2(300, 150), new Vector2(80, 100), close_scene);
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    //Creates a Button.
    public GameObject CreateButton(Transform panel ,Vector2 position, Vector2 size, UnityEngine.Events.UnityAction method) {
        GameObject button = new GameObject();
        RectTransform trans = button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.GetComponent<Button>().onClick.AddListener(method);
        trans.transform.SetParent(panel);
        trans.sizeDelta = size;
        trans.localScale = Vector3.one;
        trans.localPosition = position;
        Image image = button.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>("coin_1"); //Image file name of the button.
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        button.transform.SetParent(panel);
        return button;
}

    //Closes the board_space scene.
    void close_scene() {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        main.set_space_exit(true);
    }
}
