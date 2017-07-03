using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {
    public static ObstacleGenerator Instance { get; private set; }
    public List<LayoutButton> tiles = new List<LayoutButton>();
    public GameObject rock;
    public GameObject powerTile;
    public GameObject healthTile;
    public GameObject dmgTile;
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
        foreach (LayoutButton tile in tiles)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if (Var.map[GuiContoler.mapPos].hasRocks && rand > 0.9f)
            {          
                Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.15f, 100);
                tile.isActive = false;
                GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
                obstacles.Add(rockObj);              
            }
            if(rand>0.8f && rand < 0.9f && Var.map[GuiContoler.mapPos].powerUps.Count>0)
            {
                List<Var.Em> powerUps = Var.map[GuiContoler.mapPos].powerUps;
                Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 100);
                GameObject powerObj = Instantiate(powerTile, pos, Quaternion.identity);
                tile.power = powerObj.GetComponent<powerTile>();
                powerObj.GetComponent<powerTile>().SetColor(powerUps[Random.Range(0, powerUps.Count)]);
                obstacles.Add(powerObj);
            }
            if(rand>0.7f && rand < 0.8f && Var.map[GuiContoler.mapPos].powers != null && Var.map[GuiContoler.mapPos].powers.Count > 0)
            {
                List<Var.PowerUps> pow = Var.map[GuiContoler.mapPos].powers;
                Var.PowerUps type = pow[Random.Range(0, pow.Count)];
                GameObject powerUp=null;
                Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 100);
                switch (type)
                {
                    case Var.PowerUps.dmg:
                        powerUp = Instantiate(healthTile, pos, Quaternion.identity);
                        break;
                    case Var.PowerUps.heal:
                        powerUp = Instantiate(dmgTile, pos, Quaternion.identity);
                        break;                    
                }
               
                obstacles.Add(powerUp);
                tile.power = powerUp.GetComponent<powerTile>();
            }
        }
    }
}
