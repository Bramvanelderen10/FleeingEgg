using UnityEngine;
using System.Collections;
using System;
using Utils;
using System.Collections.Generic;

public class ComboGenerator : MonoBehaviour {

    enum ComboType
    {
        DIAGONAL,
        CURVED,
        STRAIGHT,
    }

    enum Difficulty
    {
        EASY,
        NORMAL,
        HARD,
        INSANE
    }

    public GameManager gameManager;
    public GameObject QTEPrefab;
    public GameObject QTESpawn;

    public Sprite a, b, x, y;

    public float minHorizontalDis = 1.3f;

    private System.Random rnd;
    private Combo combo;
    private QuickTimeEventController lastQTE;
    private bool comboInProgress = false;
    private float DistanceUntilNextCombo = 0f;

    private bool isStarted = false;

    private VerticalBounds bounds;

    private Vector3 enviromentPos;
    private Vector3 lastEnviromentPos;

    private float startTime;
    private float time;

    private Difficulty difficulty = Difficulty.EASY;

    void Awake()
    {
        startTime = Time.time;
        enviromentPos = lastEnviromentPos = new Vector3(0, 0, 0);
    }

	// Use this for initialization
	void Start () {
        rnd = new System.Random();
        bounds = GetBounds();
    }

    void Update()
    {
        if (isStarted)
        {
            time = Time.time - startTime;
            DetermineDifficulty();

            if (CanSpawnNewCombo())
            {
                StartCombo((ComboType)rnd.Next(0, Enum.GetNames(typeof(ComboType)).Length - 1));
            }
        }        
    }    

    private void StartCombo(ComboType type)
    {
        float distance = minHorizontalDis;
        List<QuickTimeEvent.Type> qteTypes = new List<QuickTimeEvent.Type>();
        Vector3 start = QTESpawn.transform.position;

        switch (difficulty)
        {
            case Difficulty.EASY:
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                distance *= 3;
                DistanceUntilNextCombo = distance * 1.5f;
                break;
            case Difficulty.NORMAL:
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                distance *= 2.2f;
                DistanceUntilNextCombo = distance * 1.5f;
                break;
            case Difficulty.HARD:
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                distance *= 1.7f;
                DistanceUntilNextCombo = distance * 1.5f;
                break;
            case Difficulty.INSANE:
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                qteTypes.Add(QuickTimeEvent.Utils.GetRandomType(rnd, qteTypes));
                distance *= 1f;
                DistanceUntilNextCombo = distance * 1.5f;
                break;            
        }

       switch (type)
        {
            case ComboType.STRAIGHT:
                Vector3 pos;
                if (!lastQTE)
                {
                    pos = start;
                    pos.y = rnd.Next((int)bounds.bottom, (int)bounds.top);
                } else
                {
                    pos = lastQTE.transform.position;
                    pos.y = rnd.Next((int)bounds.bottom, (int)bounds.top);
                    pos.x += DistanceUntilNextCombo;
                }
                int typeCounter = 0;
                for (int i = 0; i< rnd.Next(3, 8); i++)
                {
                    GameObject spawn = Instantiate(QTEPrefab);
                    Vector3 spawnPosition = pos;
                    spawnPosition.x += distance * i;
                    spawn.transform.position = spawnPosition;
                    QuickTimeEventController qteC = spawn.GetComponent<QuickTimeEventController>();
                    if (difficulty == Difficulty.EASY || difficulty == Difficulty.NORMAL)
                    {
                        
                        qteC.type = qteTypes[typeCounter];
                        if (typeCounter + 1 > qteTypes.Count)
                        {
                            typeCounter = 0;
                        } else
                        {
                            typeCounter++;
                        }
                    } else
                    {
                        qteC.type = qteTypes[rnd.Next(0, qteTypes.Count - 1)];
                    }
                }

                break;
            case ComboType.CURVED:

                break;
            case ComboType.DIAGONAL:

                break;
        }
    }

    private bool CanSpawnNewCombo()
    {
        bool result = false;

        if (combo == null || !combo.inProgress)
        {
            result = ((enviromentPos.x - lastEnviromentPos.x) > minHorizontalDis);
        }

        return result;
    }

    private void DetermineDifficulty()
    {
        if (time > 30)
        {
            startTime = Time.time;
            difficulty++;

            if ((int)difficulty > Enum.GetNames(typeof(Difficulty)).Length - 1)
            {
                difficulty = (Difficulty)Enum.GetNames(typeof(Difficulty)).Length - 1;
            }
        }
    }

    public void StartGenerator()
    {

    }

    public void UpdateGameVariables(Vector3 position, float time)
    {
        enviromentPos = position;
        this.time = time;
    }

    public void ApplyAdjustmentValue(float value)
    {
        lastEnviromentPos.x -= value;
    }

    private void SaveEnviromentPosition()
    {
        lastEnviromentPos = enviromentPos;
    }

    public VerticalBounds GetBounds()
    {
        Vector3[] corners = new Vector3[4];
        Camera camera = Camera.main;
        GetCorners(camera, 0, ref corners);

        float distance_x = Vector3.Distance(corners[0], corners[1]);
        float distance_y = Vector3.Distance(corners[0], corners[2]);

        float distance = distance_y / 2;

        VerticalBounds bounds = new VerticalBounds();
        bounds.bottom = Camera.main.transform.position.y - distance;
        bounds.top = Camera.main.transform.position.y + distance;

        return bounds;
    }

    public static void GetCorners(Camera camera, float distance, ref Vector3[] corners)
    {
        Array.Resize(ref corners, 4);

        // Top left
        corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 1, distance));

        // Top right
        corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

        // Bottom left
        corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));

        // Bottom right
        corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 0, distance));
    }
}
