using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {
    public static ObstacleGenerator Instance { get; private set; }
    public List<LayoutButton> tiles = new List<LayoutButton>();
    GameObject rock= null;
    public GameObject powerTile;
    public GameObject LonelyTile;
    public GameObject FirendTile;
    public GameObject CourageTile;
    public GameObject ScaredTile;
    public GameObject healthTile;
    public GameObject dmgTile;
    public GameObject BattleArea;
    public List<GameObject> obstacles = new List<GameObject>();
	// Use this for initialization
    void Awake()
    {
        Instance = this;
    }
	void Start () {
        if(rock == null)
        {
            rock = Resources.Load<GameObject>("prefabs/rock");
        }
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
        if (Var.isTutorial)
            return;
        //TODO: set rock probability with float
        //TODO: set max rock count
        foreach (LayoutButton tile in tiles)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if (Var.map[GuiContoler.mapPos].hasRocks && rand > 0.9f)
            {          
                Vector3 pos = new Vector3(tile.transform.position.x+0.05f, tile.transform.position.y + 0.3f, 20);
                tile.isActive = false;
                GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
                rockObj.transform.parent = BattleArea.transform;
                tile.gameObject.SetActive(false);
                obstacles.Add(rockObj);              
            }
            if(rand>0.8f && rand < 0.9f && Var.map[GuiContoler.mapPos].powerUps.Count>0)
            {
                List<Var.Em> powerUps = Var.map[GuiContoler.mapPos].powerUps;
                Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 20);
                Var.Em emotion = powerUps[Random.Range(0, powerUps.Count)];
                GameObject obj;
                switch (emotion)
                {
                    case Var.Em.Confident:
                        obj = CourageTile;
                        break;
                    case Var.Em.Friendly:
                        obj = FirendTile;
                        break;
                    case Var.Em.Lonely:
                        obj = LonelyTile;
                        break;
                    case Var.Em.Scared:
                        obj = ScaredTile;
                        break;
                    default:
                        obj = powerTile;
                        break;
                }
                GameObject powerObj = Instantiate(obj, pos, Quaternion.identity);
                powerObj.transform.parent = BattleArea.transform;
                tile.power = powerObj.GetComponent<powerTile>();
                powerObj.GetComponent<powerTile>().SetColor(emotion);
                obstacles.Add(powerObj);
            }
            if(rand>0.7f && rand < 0.8f && Var.map[GuiContoler.mapPos].powers != null)
            {
                try
                {
                    List<Var.PowerUps> pow = Var.map[GuiContoler.mapPos].powers;
                    Var.PowerUps type = pow[Random.Range(0, pow.Count)];
                    GameObject powerUp = null;
                    Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 20);
                    switch (type)
                    {
                        case Var.PowerUps.dmg:
                            powerUp = Instantiate(healthTile, pos, Quaternion.identity);
                            break;
                        case Var.PowerUps.heal:
                            powerUp = Instantiate(dmgTile, pos, Quaternion.identity);
                            break;
                    }
                    powerUp.transform.parent = BattleArea.transform;
                    obstacles.Add(powerUp);
                    tile.power = powerUp.GetComponent<powerTile>();
                }
                catch
                {
                    Debug.Log("failed to add powerUp");
                }
            }
        }
    }
}
