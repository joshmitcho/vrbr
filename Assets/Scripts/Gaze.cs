using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gaze : MonoBehaviour {

    public static int labNum = -1;
    public static int loadingTime;

    Transform t;
    RaycastHit hit;

    float timer;
    bool didHit;
    int duration;
    GazeButton button;
    WheelButton upDown;
    string focusedButton;

    public Text log;

    public GameObject wheel;
    public GameObject rearWheel;

    public WheelButton upButton;
    public WheelButton rearUpButton;

    public WheelButton downButton;
    public WheelButton rearDownButton;

    List<GazeButton> buttons;

    public LoadingBar loadingBar;
    bool isWheel;

    public Image black;
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        //how many successive Gaze Ray collisions (every 0.05 seconds) necessary to click the button
        loadingTime = 60;

        //labNum = -1; fix this tomorrow

        t = GetComponent<Transform>();
        timer = 0f;
        duration = 0;
        focusedButton = "None";

        //create a big list of all the buttons that can be highlighted
        LabButton[] buttonsArray = wheel.GetComponentsInChildren<LabButton>();
        LabButton[] rearButtonsArray = rearWheel.GetComponentsInChildren<LabButton>();
        buttons = new List<GazeButton>();
        for (int i = 0; i < buttonsArray.Length; i++)
        {
            buttons.Add(buttonsArray[i]);
        }
        for (int i = 0; i < rearButtonsArray.Length; i++)
        {
            buttons.Add(rearButtonsArray[i]);
        }
        
        buttons.Add(upButton);
        buttons.Add(downButton);

        buttons.Add(rearUpButton);
        buttons.Add(rearDownButton);
    }

    private void lookAtMenu(Scene scene, LoadSceneMode mode)
    {
        t.rotation = new Quaternion(0, 0, 0, 0);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += lookAtMenu;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= lookAtMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Android close icon or back button tapped.
            Application.Quit();
        }

        //shoot a Ray every 0.05 seconds
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            Shoot();
            timer = 0f;
        }
    }

    //Fire a Ray to see what you're gazing at
    void Shoot()
    {
        didHit = Physics.Raycast(t.position, t.forward, out hit, 20f);

        //if you gaze at something that wasn't the front wall or the title card
        if (didHit && hit.collider.gameObject.GetComponent<Orb>() == null)
        {
            button = hit.collider.gameObject.GetComponent<GazeButton>();

            //if you're gazing at an up/down button, scroll the menu
            isWheel = false;
            try
            {
                upDown = (WheelButton)button;
                wheel.transform.Rotate(0f, upDown.speed, 0f);
                rearWheel.transform.Rotate(0f, upDown.speed, 0f);
                isWheel = true;
            } catch { }
        
            //if you're still gazing at the same button
            if (focusedButton == button.ButtonName && !isWheel)
            {
                duration++;
                loadingBar.Grow(button.ButtonName);
            }
            else //if you're looking at a new button
            {
                duration = 0;
                loadingBar.Shrink();
            }

            focusedButton = button.ButtonName;

            //if you've been gazing at a button long enough
            if (duration > loadingTime)
            {
                duration = 0;
                button.Click();
                StartCoroutine(Fading());
            }
          
        }
        else //if you aren't gazing at anything important
        {
            duration = 0;
            focusedButton = "None";
            loadingBar.Shrink();
        }

        //Highlight only the button you're gazing at
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].ButtonName == focusedButton)
            {
                buttons[i].HighlightOn();
            }
            else
            {
                buttons[i].HighlightOff();
            }
        }
    }

    //Fade screen transition coroutine
    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene("VRBR");
    }
}
