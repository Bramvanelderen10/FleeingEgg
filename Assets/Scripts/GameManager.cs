using UnityEngine;
using System.Collections;
using QuickTimeEvent;

public class GameManager : GameComponent {

    public GameObject movingEnviroment;
    public ComboGenerator cg;

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
        player.GetComponent<GameComponent>().isActive = active;
        mec.isActive = active;
        isActive = active;
        startText.SetActive(!active);

        cg.StartGenerator(active);
    }
}
