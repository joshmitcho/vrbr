using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrevButton : GazeButton
{
    public Text txt;
    Renderer rend;
    Collider coll;

    public override void Click() { }

    void Start()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<BoxCollider>();
        setButtonText();
    }

    private void OnMenuOpen(Scene scene, LoadSceneMode mode)
    {
        setButtonText();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnMenuOpen;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMenuOpen;
    }

    private void setButtonText()
    {
        if (Gaze.labNum == -1)
        {
            rend.enabled = false;
            coll.enabled = false;
            txt.enabled = false;
        }
        else
        {
            txt.text = "Back to Lab " + Gaze.labNum.ToString();
            rend.enabled = true;
            coll.enabled = true;
            txt.enabled = true;
        }
    }

}
