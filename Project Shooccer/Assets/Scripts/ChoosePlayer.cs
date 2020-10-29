using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChoosePlayer : MonoBehaviour
{
    [SerializeField] private List<Texture> kits;
    [SerializeField] private Material homeMaterial;
    [SerializeField] private Material awayMaterial;

    [SerializeField] private GameObject singlePlayerMesh, splitScreenMesh;
    [SerializeField] private Transform[] playerInputPositions;


    private int _homeKitCounter = 0;
    private int _awayKitCounter;

    private bool _isSplitScreen;

    private static ChoosePlayer _instance;

    private List<GameObject> _playerInputs;
    private bool[] playersReady;

    public static ChoosePlayer Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

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

    public void NextHomeKit()
    {
        _homeKitCounter++;
        if (_homeKitCounter >= kits.Count)
            _homeKitCounter = 0;

        homeMaterial.mainTexture = kits[_homeKitCounter];
    }

    public void PreviousHomeKit()
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

    public void SinglePlayer()
    {
        _isSplitScreen = !_isSplitScreen;
        singlePlayerMesh.SetActive(true);

        splitScreenMesh.SetActive(false);
    }

    public void SplitScreen()
    {
        _isSplitScreen = !_isSplitScreen;
        singlePlayerMesh.SetActive(false);

        splitScreenMesh.SetActive(true);
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


        SceneManager.LoadScene(2);
    }

    public void AddPlayerInput(GameObject go)
    {
        if (_playerInputs == null)
            _playerInputs = new List<GameObject>();
        _playerInputs.Add(go);
        MovePlayerInputs();
    }

    private void MovePlayerInputs()
    {
        if (_playerInputs.Count == 1)
        {
            var transform1 = playerInputPositions[0].transform;
            _playerInputs[0].transform.position = transform1.position;
            _playerInputs[0].transform.rotation = transform1.rotation;
            SinglePlayer();
            playersReady = new bool[1];
        }
        else
        {
            int counter = 1;
            foreach (GameObject playerInput in _playerInputs)
            {
                var transform1 = playerInputPositions[counter].transform;
                playerInput.transform.position = transform1.position;
                playerInput.transform.rotation = transform1.rotation;
                counter++;
            }

            playersReady = new bool[_playerInputs.Count];

            SplitScreen();
        }
    }

    public void RemovePlayerInput(GameObject go)
    {
        _playerInputs.Remove(go);
        MovePlayerInputs();
    }

    public void SetStart(int i, bool start)
    {
        playersReady[i] = start;
        Debug.Log("Start " + i);
        if (AllPlayersReady())
            Play();
    }

    private bool AllPlayersReady()
    {
        foreach (bool ready in playersReady)
        {
            if (!ready) return false;
        }

        return true;
    }
}