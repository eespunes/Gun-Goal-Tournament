using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Slider = UnityEngine.UIElements.Slider;

public class PauseManager : MonoBehaviour
{
    private bool _paused = false;
    [SerializeField] GameObject pauseCanvas, optionsCanvas;
    [SerializeField] private Scrollbar volumeSlider, sensitivitySlider;



    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseCanvas.SetActive(true);
        _paused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _paused = false;
        pauseCanvas.SetActive(false);
    }

    public void Options()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);

        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensibility") - .5f;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void ReturnFromOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void ChangeSensibility(float value)
    {
        PlayerPrefs.SetFloat("Sensibility", 1 + (value - .5f));
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}