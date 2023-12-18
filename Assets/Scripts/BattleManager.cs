using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<Warior> good = new List<Warior>(); // good guys
    public List<Warior> bad = new List<Warior>(); //  enemies
    public string mode; // mode of the battle manager
    public List<Warior> turnOrder = new List<Warior>(); // order in which everyone in battle moves
    public List<Move> moves = new List<Move>(); // the moves that will get played
    public Transform textArea;
    public TMPro.TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        //testing
        Warior newWarior = new Warior();
        newWarior.HP = 100;
        newWarior.ATK = 2;
        newWarior.DEF = 1;
        newWarior.SPD = 4;
        newWarior.EXP = 20;
        newWarior.level = 2;
        Move punch = new Move();
        punch.name = "punch";
        punch.ATK = 3;
        punch.category = Move.categories.Attack;
        punch.amountOfTargets = 1;
        Move kick = new Move();
        kick.name = "kick";
        kick.ATK = 5;
        kick.category = Move.categories.Attack;
        kick.amountOfTargets = 1;


        foreach (Warior warior in GameManager.playerParty)
            good.Add(warior);

        mode = "Start";
        textArea = GameObject.Find("Canvas/Text").transform;
        text = textArea.GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case "Start":
                text.text = $"{bad[0].name} attacks!";
                mode = "Wait";
                break;

            case "Wait":
                break;

            case "GetTurnOrder":
                GetTurnOrder();
                mode = "GetMoves";
                break;

            case "GetMoves":
                GetMoves();
                mode = "PlayMoves";
                break;

            case "PlayMoves":
                StartCoroutine(PlayMove());
                break;

            case "Win":
                break;

            case "Lose":
                break;
        }
    }

    IEnumerator PlayMove()
    {
        foreach (Move move in moves)
        {
            foreach (Warior target in move.targets)
            {
                if (move.category == Move.categories.Attack)
                    text.text = $"{move.user.name} uses {move.name} on {target.name}!";
                else
                    text.text = $"{move.user.name} casts {move.name} on {target.name}!";

                //wait for player input
                while (!Input.GetKey(KeyCode.Space))
                    yield return null;

                int attack = Mathf.Max(move.ATK - target.DEF, 0);
                target.HP -= attack;

                if (move.category == Move.categories.Attack)
                    text.text = $"{target.name} takes {attack} amount of damage!";
                else
                    text.text = $"{target.name} heals {attack} amount of damage!";

                // check if the target is dead
                if (target.HP <= 0)
                {
                    if (good.Contains(target))
                        good.Remove(target);
                    else
                        bad.Remove(target);

                    if (good.Count == 0)
                        mode = "Lose";
                    if (bad.Count == 0)
                        mode = "Win";
                }
                    
            }
            //wait for player input
            while (!Input.GetKey(KeyCode.Space))
                yield return null;
        }
    }

    void GetMoves()
    {
        foreach (Warior warior in turnOrder)
        {
            Move move = warior.equipedMoves[Random.Range(0, warior.equipedMoves.Count)];
            move.user = warior;
            move.ATK += warior.ATK;
            if (good.Contains(warior))
                for (int i = 0; i < move.amountOfTargets; i++)
                    move.targets.Add(bad[Random.Range(0, bad.Count)]);
            else
                for (int i = 0; i < move.amountOfTargets; i++)
                    move.targets.Add(good[Random.Range(0, good.Count)]);
            moves.Add(move);
        }
    }

    void GetTurnOrder()
    {
        turnOrder = new List<Warior>();

        // Add all characters and enemies to turn order list
        foreach (Warior character in good)
            turnOrder.Add(character);
        foreach (Warior character in bad)
            turnOrder.Add(character);

        turnOrder.Sort((a, b) => b.SPD.CompareTo(a.SPD));
    }

    public void Run()
    {

    }

    public void Attack()
    {
        mode = "GetTurnOrder";
    }
}
