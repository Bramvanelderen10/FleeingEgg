using UnityEngine;
using System.Collections;
using QuickTimeEvent;

public class GameManager : GameComponent {

    public GameObject movingEnviroment;

    public GameObject startText;

    public Transform qte_spawn;
    public Transform qte_despawn;
    public Transform player_spawn;

    public GameObject player_prefab;
    public GameObject qte_a_prefab;
    public GameObject qte_b_prefab;
    public GameObject qte_y_prefab;
    public GameObject qte_x_prefab;

    public float qte_distance = 3.0f;
    public float game_speed = 2f;
    public float player_speed_multiplier = 2f;

    private Vector3 last_qte_position;
    private Vector3 last_me_position;

    private MovingEnviromentController mec;

    private System.Random rnd;

    private GameObject player = null;

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
                ActivateGameComponents();
            } else
            {
                return;
            }
        }
        if (!player.activeSelf)
        {
            ResetGamePosition(movingEnviroment.transform.position);
            player.SetActive(true);
            InitializeGame();
            ActivateGameComponents(false);
        }

        print(last_me_position.x);
	    if (movingEnviroment.transform.position.x - last_me_position.x > qte_distance)
        {
            last_me_position = movingEnviroment.transform.position;
            SpawnCombo(qte_spawn.position);
        }
	}

    void SpawnCombo(Vector3 spawnLocation)
    {
        float[,] combo = ComboOld.GetCombo(Difficulty.Easy, rnd.Next(0, ComboOld.GetComboCount()));

        for (int x = 0; x < combo.GetLength(0); x += 1)
        {
            GameObject qte = InstantiateQTE();
            Vector3 qtePosition = spawnLocation;
            qtePosition.x += combo[x, 0];
            qtePosition.y += combo[x, 1];
            qte.transform.position = qtePosition;
        }
    }

    GameObject InstantiateQTE()
    {
        GameObject qte = null;
        switch (rnd.Next(0, 4))
        {
            case 0:
                qte = Instantiate(qte_a_prefab);
                break;
            case 1:
                qte = Instantiate(qte_b_prefab);
                break;
            case 2:
                qte = Instantiate(qte_x_prefab);
                break;
            case 3:
                qte = Instantiate(qte_y_prefab);
                break;
        }

        return qte;
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

        last_qte_position = player_spawn.position;
        last_qte_position.y = 0;

        rnd = new System.Random();

        //Generate next 3 QTE's in advance
        for (int i = 0; i < 3; ++i)
        {

            Vector3 spawnLocation = new Vector3(last_qte_position.x, last_qte_position.y);

            //TODO Give random position to new QTE within the qte_distance value
            spawnLocation.x += qte_distance;
            last_qte_position = spawnLocation;

            SpawnCombo(spawnLocation);
        }
        last_me_position = movingEnviroment.transform.position;

        startText.SetActive(true);
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

        last_me_position.x -= adjustmentValue;
    }

    void ActivateGameComponents(bool active = true)
    {
        player.GetComponent<GameComponent>().isActive = active;
        mec.isActive = active;
        isActive = active;
        startText.SetActive(!active);
    }
}
