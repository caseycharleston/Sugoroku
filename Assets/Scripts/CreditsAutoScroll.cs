using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAutoScroll : MonoBehaviour
{

    float speed = 5f;
    float beginPos = -2000f;
    float endPos = 655f;

    public RectTransform container;
    // Start is called before the first frame update
    void Start()
    {
        container.localPosition = new Vector2(0, beginPos);
        StartCoroutine(AutoScrollText());
    }

    IEnumerator AutoScrollText() {
        while (container.localPosition.y < endPos) {
            container.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        Initiate.Fade("TitleScreen", Color.black, 1f);
    }

}
