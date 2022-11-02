using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fast_success : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text_box;
    
    // Start is called before the first frame update
    void Start()
    {
        text_box.GetComponent<TMP_Text>().text = "Success! Fast Travel to " + main.fast_space;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
