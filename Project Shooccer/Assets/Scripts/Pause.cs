using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    bool paused = false;
    [SerializeField] GameObject pauseCanvas, optionsCanvas;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused&&Input.GetKeyDown(KeyCode.P))
            pause();
    }


    void pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseCanvas.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }

    public void resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        paused = false;
        pauseCanvas.SetActive(false);
    }

    public void options()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void returnFromOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
}
