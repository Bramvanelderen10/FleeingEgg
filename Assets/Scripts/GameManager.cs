using UnityEngine;
using Difficulty;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

//TODO Split UI, leaderboards an score system into different components
public class GameManager : GameComponent {

    public GameObject _env;
    public ComboGenerator _comboGen;

    public GameObject _startButton, _leaderboardsButton;    
    public GameObject _playerPrefab, _leaderboardsTextPrefab;
    public Transform _playerSpawn;

    public GameObject _mobile_canvas, _ingame_canvas, _leaderboard_canvas, _menu_canvas;
    public Text _hits, _scoreText;
    public int _maxMisses;

    public GameObject EndGameCanvas;
    private GameObject canvas = null;
    private String name = null;
    private int score;

    public float game_speed = 2f;
    public float player_speed_multiplier = 2f;

    private MovingEnviromentController mec;
    private GameObject player = null;
    private PlayerController pc;

    private float time;
    private float startTime;
    private DifficultyLevel difficulty;
    private float difficultyTimer = 5;

    private List<GameObject> rankings;

    void Awake()
    {        
        Application.targetFrameRate = 60;
        new DifficultyManager();
    }

	// Use this for initialization
	void Start () {
        _startButton.GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
        _leaderboardsButton.GetComponent<Button>().onClick.AddListener(delegate { ShowLeaderboards(); });
        InitializeGame();
        AdManager.Instance.ShowBanner();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetButtonDown("Start"))
            {
                StartGame();
            } else
            {
                return;
            }
        } else
        {
            DifficultyManager.Instance.Update(Time.time);

            _hits.text = pc.GetHits().ToString();
            _scoreText.text = GetScore().ToString();
        }
        CheckPlayerStatus();
	}

    void FixedUpdate()
    {
        Vector3 position = _env.transform.position;

        if (position.x > 100)
        {
            ResetGamePosition(position);
        }
    }

    void InitializeGame()
    {
        foreach (GameObject qte  in GameObject.FindGameObjectsWithTag("QTE"))
        {
            Destroy(qte);
        }

        if (!mec)
            mec = _env.GetComponent<MovingEnviromentController>();
        mec.speed = game_speed;
        //Initialise player and QTE's

        //Spawn player slightly above the spawn point
        if (!player)
            player = Instantiate(_playerPrefab);
        Vector3 position = _playerSpawn.position;
        position.y += 0.5f;
        player.transform.position = position;
        player.GetComponent<PlayerController>().SetSpeed(game_speed * player_speed_multiplier);
        pc = player.GetComponent<PlayerController>();

        _startButton.SetActive(true);
        

        _leaderboardsButton.SetActive(true);

        _ingame_canvas.SetActive(false);
        _mobile_canvas.GetComponent<MobileControls>().SetActive(false);

        _leaderboard_canvas.SetActive(false);

        SoundManager.Instance.PlayMenuSoundtrack();
    }

    void ResetGamePosition(Vector3 position)
    {
        Vector3 mePosition = _env.transform.position;
        float adjustmentValue = mePosition.x;
        position.x -= adjustmentValue;
        _env.transform.position = position;

        Vector3 playerPosition = player.transform.position;
        playerPosition.x -= adjustmentValue;
        player.transform.position = playerPosition;

        foreach (GameObject qte in GameObject.FindGameObjectsWithTag("QTE"))
        {
            Vector3 qtePosition = qte.transform.position;
            qtePosition.x -= adjustmentValue;
            qte.transform.position = qtePosition;
        }
    }

    void ActivateGameComponents(bool active = true)
    {
        if (active)
        {
            startTime = time = Time.time;
            
            if (canvas)
            {
                Destroy(canvas);
            }

            SoundManager.Instance.PlayGameSoundtrack();
        } else
        {
            SoundManager.Instance.PlayMenuSoundtrack();
        }

        _ingame_canvas.SetActive(active);
        _mobile_canvas.GetComponent<MobileControls>().SetActive(active);
        MobileInput.Instance.Reset();

        DifficultyManager.Instance.Start(Time.time);

        player.GetComponent<PlayerController>().Activate(active);
        mec.isActive = active;
        isActive = active;
        _startButton.SetActive(!active);
        _leaderboardsButton.SetActive(!active);

        _comboGen.StartGenerator(active);        
    }

    void CheckPlayerStatus()
    {
        if (pc.dead)
        {            
            ResetGamePosition(_env.transform.position);
            pc.dead = false;
            player.SetActive(true);
            InitializeGame();
            ActivateGameComponents(false);
            ShowEndGame();
        }

        if (pc.GetMisses() >= _maxMisses)
        {
            ResetGamePosition(_env.transform.position);
            pc.dead = false;
            player.SetActive(true);
            InitializeGame();
            ActivateGameComponents(false);
            ShowEndGame();
        }
    }

    void ShowEndGame()
    {
        float time = Time.time - startTime;
        AdManager.Instance.AddTime(time);
        AdManager.Instance.ShowBanner();
        score = GetScore();

        canvas = Instantiate(EndGameCanvas);

        if (name != null)
        {
            GameObject nameInput = canvas.transform.FindChild("Name").gameObject;
            InputField input = nameInput.GetComponent<InputField>();
            input.text = name;
        }

        GameObject scoreText = canvas.transform.FindChild("Score").gameObject;
        scoreText.GetComponent<Text>().text = score.ToString();
        GameObject saveButton = canvas.transform.FindChild("Save").gameObject;
        Button save = saveButton.GetComponent<Button>();
        save.onClick.AddListener(delegate() { Save(); });
    }

    public void Save()
    {
        if (canvas)
        {
            GameObject nameInput = canvas.transform.FindChild("Name").gameObject;
            InputField input = nameInput.GetComponent<InputField>();
            name = input.text;

            DatabaseManager.Instance.InsertScore(name, score);
            Destroy(canvas);
        }        
    }

    public void StartGame()
    {
        AdManager.Instance.ShowVideo();
        AdManager.Instance.HideBanner();
        ActivateGameComponents();
    }

    public void ShowLeaderboards()
    {
        _leaderboard_canvas.SetActive(true);
        _leaderboard_canvas.transform.FindChild("Back").GetComponent<Button>().onClick.AddListener(delegate { HideLeaderboards(); });
        _menu_canvas.SetActive(false);
        if (canvas)
        {
            canvas.SetActive(false);
        }
        Vector3 startPos = new Vector3(0, -35, 0);
        List<KeyValuePair<string, int>> topScores = DatabaseManager.Instance.RetrieveTopScores(10);

        rankings = new List<GameObject>();

        int i = 0;
        foreach( KeyValuePair<string, int> topScore in topScores)
        {
            int adjustment = 0 + (i * 30);
            Vector3 pos = new Vector3();
            pos.y = startPos.y - adjustment;

            GameObject leaderboardsObject = Instantiate(_leaderboardsTextPrefab);
            rankings.Add(leaderboardsObject);

            Text text = leaderboardsObject.GetComponent<Text>();
            text.transform.SetParent(_leaderboard_canvas.transform);

            text.rectTransform.anchoredPosition = pos;
            text.text = (i + 1).ToString() + " " + topScore.Key + " " + topScore.Value.ToString();
            

            i++;
        }
    }

    public void HideLeaderboards()
    {
        if (canvas)
        {
            canvas.SetActive(true);
        }
        _menu_canvas.SetActive(true);
        _leaderboard_canvas.SetActive(false);
        foreach (GameObject rank in rankings)
        {
            Destroy(rank);
        }
    }

    public int GetScore()
    {
        int hits = pc.GetHits();
        int misses = pc.GetMisses();
        int time = Convert.ToInt32(Time.time - startTime);
        
        float positive = (hits / 10) + (time / 4);
        float negative = misses;

        int score = Convert.ToInt32(positive - negative);

        return score;
    }
}
