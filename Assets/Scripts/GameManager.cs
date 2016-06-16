using UnityEngine;
using System.Collections;
using QuickTimeEvent;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

//TODO Split UI, leaderboards an score system into different components
public class GameManager : GameComponent {

    public GameObject movingEnviroment;
    public ComboGenerator cg;

    public GameObject startButton, leaderboardsButton, leaderboardsText;    
    public GameObject player_prefab;
    public Transform player_spawn;

    public GameObject mobile_canvas, ingame_canvas, leaderboard_canvas, menu_canvas;
    public Text hits, misses, scoreText;
    public int maxMisses;

    public UnityAds unityAds;
    public DatabaseManager dbm;
    public GameObject EndGameCanvas;
    private GameObject canvas = null;
    private String name = null;
    private int score;

    public float game_speed = 2f;
    public float player_speed_multiplier = 2f;

    private MovingEnviromentController mec;
    private System.Random rnd;
    private GameObject player = null;
    private PlayerController pc;

    private float time;
    private float startTime;
    private Difficulty difficulty;

    private List<GameObject> rankings;

	// Use this for initialization
	void Start () {
        startButton.GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
        leaderboardsButton.GetComponent<Button>().onClick.AddListener(delegate { ShowLeaderboards(); });
        InitializeGame();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive)
        {
            if (Input.GetButtonDown("Start"))
            {
                StartGame();
            } else
            {
                return;
            }
        } else
        {
            hits.text = pc.GetHits().ToString();
            misses.text = pc.GetMisses().ToString();
            scoreText.text = GetScore().ToString();
        }
        CheckPlayerStatus();
	}

    void FixedUpdate()
    {
        Vector3 position = movingEnviroment.transform.position;

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
            mec = movingEnviroment.GetComponent<MovingEnviromentController>();
        mec.speed = game_speed;
        //Initialise player and QTE's

        //Spawn player slightly above the spawn point
        if (!player)
            player = Instantiate(player_prefab);
        Vector3 position = player_spawn.position;
        position.y += 0.5f;
        player.transform.position = position;
        player.GetComponent<PlayerController>().SetSpeed(game_speed * player_speed_multiplier);
        pc = player.GetComponent<PlayerController>();

        rnd = new System.Random();

        startButton.SetActive(true);
        

        leaderboardsButton.SetActive(true);

        ingame_canvas.SetActive(false);
        mobile_canvas.GetComponent<MobileControls>().SetActive(false);

        leaderboard_canvas.SetActive(false);
    }

    void ResetGamePosition(Vector3 position)
    {
        Vector3 mePosition = movingEnviroment.transform.position;
        float adjustmentValue = mePosition.x;
        position.x -= adjustmentValue;
        movingEnviroment.transform.position = position;

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
        }

        ingame_canvas.SetActive(active);
        mobile_canvas.GetComponent<MobileControls>().SetActive(active);

        UpdateDifficulty(true);

        player.GetComponent<PlayerController>().Activate(active);
        mec.isActive = active;
        isActive = active;
        startButton.SetActive(!active);
        leaderboardsButton.SetActive(!active);

        cg.StartGenerator(active);        
    }

    void UpdateDifficulty(bool init = false)
    {
        if (init)
        {
            difficulty = Difficulty.EASY;
        } else
        {
            if (time > 30)
            {
                time = Time.time;
                difficulty++;

                if ((int)difficulty > Enum.GetNames(typeof(Difficulty)).Length - 1)
                {
                    difficulty = (Difficulty)Enum.GetNames(typeof(Difficulty)).Length - 1;
                }
            }
        }
        
    }

    void CheckPlayerStatus()
    {
        if (pc.dead)
        {            
            ResetGamePosition(movingEnviroment.transform.position);
            pc.dead = false;
            player.SetActive(true);
            InitializeGame();
            ActivateGameComponents(false);
            ShowEndGame();
        }

        if (pc.GetMisses() >= maxMisses)
        {
            ResetGamePosition(movingEnviroment.transform.position);
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
        unityAds.AddTime(time);
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

            dbm.InsertScore(name, score);
            Destroy(canvas);
        }        
    }

    public void StartGame()
    {
        unityAds.ShowAd();
        ActivateGameComponents();
    }

    public void ShowLeaderboards()
    {
        leaderboard_canvas.SetActive(true);
        leaderboard_canvas.transform.FindChild("Back").GetComponent<Button>().onClick.AddListener(delegate { HideLeaderboards(); });
        menu_canvas.SetActive(false);
        if (canvas)
        {
            canvas.SetActive(false);
        }
        Vector3 startPos = new Vector3(0, -35, 0);
        List<KeyValuePair<string, int>> topScores = dbm.RetrieveTopScores(10);

        rankings = new List<GameObject>();

        int i = 0;
        foreach( KeyValuePair<string, int> topScore in topScores)
        {
            int adjustment = 0 + (i * 30);
            Vector3 pos = new Vector3();
            pos.y = startPos.y - adjustment;

            GameObject leaderboardsObject = Instantiate(leaderboardsText);
            rankings.Add(leaderboardsObject);

            Text text = leaderboardsObject.GetComponent<Text>();
            text.transform.parent = leaderboard_canvas.transform;

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
        menu_canvas.SetActive(true);
        leaderboard_canvas.SetActive(false);
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
