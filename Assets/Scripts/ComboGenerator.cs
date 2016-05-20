using UnityEngine;
using System.Collections;
using System;
using Utils;

public class ComboGenerator : MonoBehaviour {

    public GameObject QTEPrefab;
    public Camera camera;


	// Use this for initialization
	void Start () {
        camera = Camera.main;
	}

    void Update()
    {
        
    }

    public VerticalBounds GetBounds()
    {
        Vector3[] corners = new Vector3[4];
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
