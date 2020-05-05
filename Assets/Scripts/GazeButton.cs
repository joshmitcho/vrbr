using UnityEngine;

public abstract class GazeButton : MonoBehaviour {

    public string ButtonName;
    public GameObject highlight;

    protected MeshRenderer hl;

    private void Start()
    {
        hl = highlight.GetComponent<MeshRenderer>();
    }

    public abstract void Click();

    public void HighlightOn()
    {
        hl.enabled = true;
    }

    public void HighlightOff()
    {
        hl.enabled = false;
    }
}
