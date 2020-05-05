using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    Transform t;
    Renderer rend;

    public Text txt;

    void Start()
    {
        t = gameObject.transform;
        rend = gameObject.GetComponent<MeshRenderer>();
        rend.enabled = false;
        txt.enabled = false;
    }

    public void Grow(string buttonName)
    {
        rend.enabled = true;
        txt.enabled = true;

        txt.text = "Loading " + buttonName;

        if (t.localScale.x < 10)
        {
            t.localScale += new Vector3(10f / Gaze.loadingTime, 0, 0);
            t.localPosition += new Vector3(5f / Gaze.loadingTime, 0, 0);
        }
    }
    public void Shrink()
    {
        t.localScale = new Vector3(0, 0.5f, 0.5f);
        t.localPosition = new Vector3(-5, -4, 12);
        rend.enabled = false;
        txt.enabled = false;
    }
}
