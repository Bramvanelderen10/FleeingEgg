using UnityEngine;
using System.Collections;
using Utils;
using System;

public class EnemyPositioning : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HorizontalBounds bounds = GetBounds();
        Vector3 pos = transform.position;
        pos.x = bounds.left;
        transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public HorizontalBounds GetBounds()
    {
        Vector3[] corners = new Vector3[4];
        Camera camera = Camera.main;
        GetCorners(camera, 0, ref corners);

        float distance_x = Vector3.Distance(corners[0], corners[1]);
        float distance_y = Vector3.Distance(corners[0], corners[2]);

        float distance = distance_x / 2;

        HorizontalBounds bounds = new HorizontalBounds();
        bounds.left = Camera.main.transform.position.x - distance;
        bounds.right = Camera.main.transform.position.x + distance;

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
