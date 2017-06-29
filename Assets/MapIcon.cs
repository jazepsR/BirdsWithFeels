using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapIcon : MonoBehaviour {
    public GameObject CompleteIcon;
    public Var.Em type;
    public int birdLVL = 1;
    public int length = 1;
    public Transform[] targets;
    LineRenderer lr;
    public bool completed = false;
    SpriteRenderer sr;
    // Use this for initialization
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Helpers.Instance.GetEmotionColor(type);
        CompleteIcon.SetActive(completed);
        lr = GetComponent<LineRenderer>();
        int i = 0;
        lr.numPositions = targets.Length * 2;
        if (completed && targets.Length>0)
        {
            foreach (Transform pos in targets)
            {

                lr.SetPosition(i++, pos.position);
                lr.SetPosition(i++, transform.position);
            }
        }
    }

    void OnMouseDown()
    {
        if (completed)
        {
            SceneManager.LoadScene("NewMain");
        }

    }
}
