using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private List<Texture> kits;
    [SerializeField] private Material homeMaterial;
    [SerializeField] private Material awayMaterial;

    [SerializeField] private GameObject singlePlayerMesh, splitScreenMesh;
    [SerializeField] private GameObject singlePlayerCanvas, splitScreenCanvas;

    private int _homeKitCounter = 0;
    private int _awayKitCounter;

    private bool _isSplitScreen;

    // Start is called before the first frame update
    void Start()
    {
        _homeKitCounter = PlayerPrefs.GetInt("Home Kit");
        _awayKitCounter = PlayerPrefs.GetInt("Away Kit");
        homeMaterial.mainTexture = kits[_homeKitCounter];
        awayMaterial.mainTexture = kits[_awayKitCounter];
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextKit()
    {
        _homeKitCounter++;
        if (_homeKitCounter >= kits.Count)
            _homeKitCounter = 0;

        homeMaterial.mainTexture = kits[_homeKitCounter];
    }

    public void PreviousKit()
    {
        _homeKitCounter--;
        if (_homeKitCounter < 0)
            _homeKitCounter = kits.Count - 1;

        homeMaterial.mainTexture = kits[_homeKitCounter];
    }

    public void NextAwayKit()
    {
        _awayKitCounter++;
        if (_awayKitCounter >= kits.Count)
            _awayKitCounter = 0;
        
        awayMaterial.mainTexture = kits[_awayKitCounter];
    }

    public void PreviousAwayKit()
    {
        _awayKitCounter--;
        if (_awayKitCounter < 0)
            _awayKitCounter = kits.Count - 1;

        awayMaterial.mainTexture = kits[_awayKitCounter];
    }

    public void SinglePlayerAndSplitScreenTransition()
    {
        _isSplitScreen = !_isSplitScreen;
        singlePlayerMesh.SetActive(!_isSplitScreen);
        singlePlayerCanvas.SetActive(!_isSplitScreen);
        splitScreenMesh.SetActive(_isSplitScreen);
        splitScreenCanvas.SetActive(_isSplitScreen);
    }

    public void Play()
    {
        if (!_isSplitScreen)
        {
            int random = Random.Range(0, kits.Count - 1);
            while (random == _homeKitCounter)
                random = Random.Range(0, kits.Count - 1);

            awayMaterial.mainTexture = kits[random];
        }
        else
            PlayerPrefs.SetInt("Away Kit", _awayKitCounter);

        PlayerPrefs.SetInt("Home Kit", _homeKitCounter);

        MatchController.GetInstance().SplitScreen = _isSplitScreen;


        SceneManager.LoadScene(1);
    }
}