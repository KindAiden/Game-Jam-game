using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                break;

            case "GetTurnOrder":
                GetTurnOrder();
                mode = "PlayMoves";
                break;

            case "GetMoves":
                GetMoves();
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

    public IEnumerator PlayMove()
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
                    good.Remove(target);
                    bad.Remove(target);
            }
            //wait for player input
            while (!Input.GetKey(KeyCode.Space))
                yield return null;
        }
    }

    public void GetMoves()
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

    public void GetTurnOrder()
    {
        turnOrder = new List<Warior>();

        // Add all characters and enemies to turn order list
        foreach (Warior character in good)
            turnOrder.Add(character);
        foreach (Warior character in bad)
            turnOrder.Add(character);

        turnOrder.Sort((a, b) => b.SPD.CompareTo(a.SPD));
    }
}
