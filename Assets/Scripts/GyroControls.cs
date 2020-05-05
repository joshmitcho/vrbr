using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GyroControls : MonoBehaviour {

    public static bool showIcons = true;

    float tilt;
    Vector3 flipV;
    float flip;

    bool down;
    bool up;
    float flipTimer;
    float flipDuration;

    public GameObject L;
    public GameObject R;
    SplitPic lScript;
    SplitPic rScript;

    public Text log;

    public Image black;
    public Animator anim;

    public Image rImage;
    public Image lImage;
    bool press;

    float lowPassFilterFactor;
    Vector3 baselineV;
    float baseline;
    float upLock;

    // Use this for initialization
    void Start () {
        down = false;
        up = false;
        flipTimer = 0f;
        flipDuration = 0.8f;

        lScript = L.GetComponent<SplitPic>();
        rScript = R.GetComponent<SplitPic>();

        press = false;

        lowPassFilterFactor = 5.0f / 21.0f;
        baselineV = Vector3.zero;
        baseline = 0.0f;
        flipV = Vector3.zero;
        flip = 0.0f;
        upLock = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Android close icon or back button tapped.
            Application.Quit();
        }


       
        if (Input.touches.Length != 0)
        {
            if (!press)
            {
                showIcons = !showIcons;

                rImage.enabled = showIcons;
                lImage.enabled = showIcons;
                press = true;
            }
            
        }
        else
        {
            press = false;
        }

        baselineV = new Vector3(0, 0, Mathf.Clamp(LowPassFilterAccelerometer(baselineV, 1.0f/35.0f).z, -0.3f, 0.3f));
        baseline = baselineV.z;

        tilt = Input.acceleration.x;

        //log.text = tilt.ToString();

        flipV = LowPassFilterAccelerometer(flipV, lowPassFilterFactor);
        flip = flipV.z;

        upLock += Time.deltaTime;
        flipTimer += Time.deltaTime;

        //Zoom
        if (Mathf.Abs(tilt) > 0.2)
        {
            //Zoom out
            if (transform.position.z > -6f && tilt < 0f)
            {
                transform.Translate(0, 0, tilt * 0.3f);
            }

            //Zoom in
            if (transform.position.z < 8f && tilt > 0f)
            {
                transform.Translate(0, 0, tilt * 0.3f);
            }
        }

        //Flip down to switch picture
        if (!down && flip < baseline - 0.5f)
        {
            down = true;
            up = false;
            upLock = 0.0f;
            flipTimer = 0f;
        }
        if (down && Mathf.Abs(flip) < Mathf.Abs(baseline - 0.15f))
        {
            if (flipTimer < flipDuration)
            {
                //switch picture
                upLock = 0.0f;
                lScript.ChangePicture();
                rScript.ChangePicture();
            }
            down = false;
            up = false;
        }

        //Flip up to open Menu
        if (!up && flip > baseline + 0.4f && upLock >= 0.8f)
        {
            up = true;
            down = false;
            flipTimer = 0f;
        }
        if (up && Mathf.Abs(flip) < Mathf.Abs(baseline - 0.15f))
        {
            if (flipTimer < flipDuration)
            {
                //open Menu
                StartCoroutine(Fading());
            }
            up = false;
            down = false;
        }
    }

    private Vector3 LowPassFilterAccelerometer(Vector3 oldV, float factor)
    {
        Vector3 newV = Vector3.Lerp(oldV, Input.acceleration, factor);
        return newV;
    }

    //Fade screen transition coroutine
    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene("Menu");
    }
}