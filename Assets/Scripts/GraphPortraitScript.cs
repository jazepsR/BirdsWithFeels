using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPortraitScript : MonoBehaviour {

    // Use this for initialization
    LineRenderer lr;
    Vector3 firstPos;
    void Starter () {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.sortingOrder = 200;
        lr.SetWidth(0.045f, 0.045f);
        lr.textureMode = LineTextureMode.Tile;
        lr.widthMultiplier = 3f;
        firstPos = new Vector3(transform.position.x,transform.position.y, 0);
        lr.SetPosition(0, firstPos);       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGraph( Vector3 target)
    {
        Starter();
        lr.material = Resources.Load<Material>("mat");
        transform.localPosition = target;
        Vector3 pos = transform.position;
        transform.position = firstPos;
        LeanTween.value(gameObject,MovePoint, transform.position, pos, 1.35f);
        

    }

    public void MovePoint(Vector3 pos)
    {        
        transform.position = new Vector3(pos.x, pos.y, 0);
        lr.SetPosition(1, (new Vector3(pos.x,pos.y,0)));
    }
}
