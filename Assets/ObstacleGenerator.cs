using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {
    public static ObstacleGenerator Instance { get; private set; }
    public List<LayoutButton> tiles = new List<LayoutButton>();
    public GameObject rock;
    public List<GameObject> obstacles = new List<GameObject>();
	// Use this for initialization
	void Start () {
        Instance = this;
        GenerateObstacles();
    }
	public void clearObstacles()
    {
        foreach (LayoutButton tile in tiles)
        {
            tile.isActive = true;
        }
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }
    }
	// Update is called once per frame
	public void GenerateObstacles()
    {
        //TODO: set rock probability with float
        //TODO: set max rock count
        if (Var.map[GuiContoler.mapPos].hasRocks)
        {
            foreach (LayoutButton tile in tiles)
            {
                if (Random.Range(0.0f, 1.0f) > 0.9f)
                {
                    Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.15f, 100);
                    tile.isActive = false;
                    GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
                    obstacles.Add(rockObj);
                }
            }
        }
    }
}
