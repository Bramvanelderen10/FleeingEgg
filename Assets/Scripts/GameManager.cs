using UnityEngine;
using System.Collections;
using QuickTimeEvent;
using System;
using UnityEngine.UI;

public class GameManager : GameComponent {

    public GameObject movingEnviroment;
    public ComboGenerator cg;

    public GameObject startText;    
    public GameObject player_prefab;
    public Transform player_spawn;

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

    private float time;
    private float startTime;
    private Difficulty difficulty;

	// Use this for initialization
	void Start () {
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

        rnd = new System.Random();

        startText.SetActive(true);
        startText.GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
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
            
        UpdateDifficulty(true);

        player.GetComponent<PlayerController>().Activate(active);
        mec.isActive = active;
        isActive = active;
        startText.SetActive(!active);

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
        PlayerController pc = player.GetComponent<PlayerController>();

        if (pc.dead)
        {            
            ResetGamePosition(movingEnviroment.transform.position);
            pc.dead = false;
            player.SetActive(true);
            InitializeGame();
            ActivateGameComponents(false);
            ShowEndGame();
        }

        if (pc.GetMisses() > maxMisses)
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
        score = (int)System.Math.Round(time / 5, 0);

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
}
