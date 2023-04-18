using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutScript : MonoBehaviour
{
    [SerializeField] GameObject next;
    [SerializeField] GameObject prev;
    [SerializeField] Button exit;
    public AudioSource page_flip;
    public GameObject[] pages;
    int page_index;
    // Start is called before the first frame update
    void Start()
    {
        page_index = 0;
        next.GetComponent<Button>().onClick.AddListener(next_page);
        prev.GetComponent<Button>().onClick.AddListener(prev_page);
        exit.onClick.AddListener(leave);
        pages[0].SetActive(true);
        for (int i = 1; i < pages.Length; i++) {
            pages[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right")) {
            next_page();
        }

        if (Input.GetKeyDown("left")) {
            prev_page();
        }
    }

    void next_page() {
        if (page_index < pages.Length - 1) {
            page_flip.Play();
            pages[page_index].SetActive(false);
            page_index++;
            if (page_index == pages.Length - 1) {
                next.SetActive(false);
            } else {
                prev.SetActive(true);
                next.SetActive(true);
            }
            pages[page_index].SetActive(true);
        }
    }

    void prev_page() {
        if (page_index > 0) {
            page_flip.Play();
            pages[page_index].SetActive(false);
            page_index--;
            if (page_index == 0) {
                prev.SetActive(false);
            } else {
                next.SetActive(true);
                prev.SetActive(true);
            }
            pages[page_index].SetActive(true);
        }
    }

    void leave() {
        Initiate.Fade("GameBoard", Color.black, 1f);
    }
}