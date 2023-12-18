using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<Warior> good = new List<Warior>();
    public List<Warior> bad = new List<Warior>();
    public string mode;
    public Transform textArea;
    public TMPro.TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        good = GameManager.playerParty;
        mode = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case "Start":
                break;

            case "GetMoves":
                break;

            case "GetTurnOrder":
                break;

            case "PlayMoves":
                break;

            case "Win":
                break;

            case "Lose":
                break;
        }
    }
}
