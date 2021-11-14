using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingEventStructureScript : MonoBehaviour
{
    // Start is called before the first frame update

    public EventScript EventToPlayBeforeBattle;
    public Dialogue DialogueToPlayBeforeBattle;
    public EventScript EventToPlayOnceBirdsArePlaced;
    public bool VulturesShouldGoToLeftSideThisTurn;
    public Var.Em[] EnemiesToSpawn;
    public bool isFinalBattle;

}
