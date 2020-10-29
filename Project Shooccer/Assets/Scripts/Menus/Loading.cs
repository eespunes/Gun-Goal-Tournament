using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private float timer = 0;
    

    // Update is called once per frame
    void Update()
    {
        if (timer >= 1)
        {
            SceneManager.LoadScene(3);
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
