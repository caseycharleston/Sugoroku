using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoSpace : MonoBehaviour
{
    [SerializeField] Button forward_arrow;
    [SerializeField] Button back_arrow;
    [SerializeField] GameObject secondary_con;
    public static GameObject first_img;

    // Start is called before the first frame update
    void Start()
    {
        first_img = GameObject.Find("Image");
        forward_arrow.onClick.AddListener(go_forward);
        back_arrow.onClick.AddListener(go_backward);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void go_forward() {
        first_img.SetActive(false);
        secondary_con.SetActive(true);

    }

    void go_backward() {
        secondary_con.SetActive(false);
        first_img.SetActive(true);
    }
}
