using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BattleManager : MonoBehaviour
{
    public bool waiting;
    public TMPro.TMP_Text battleText;
    public List<Warior> playerParty = new List<Warior>();
    public List<Warior> enemyParty = new List<Warior>();
    public List<Move> moves = new List<Move>();

    void Start()
    {
        // Get necessary stuff - player party, enemies, battle text
        // Initialize player and enemy parties
        Move punch = new Move();
        punch.name = "punch";
        punch.ATK = 3;
        punch.category = Move.categories.Attack;
        punch.amountOfTargets = 1;

        Warior newWarior = new Warior();
        newWarior.name = "Test Guy";
        newWarior.HP = 100;
        newWarior.ATK = 2;
        newWarior.DEF = 1;
        newWarior.SPD = 4;
        newWarior.EXP = 20;
        newWarior.level = 2;
        newWarior.equipedMoves = new List<Move> { punch };
        Warior newWarior1 = new Warior();
        newWarior1.name = "enemy 1";
        newWarior1.HP = 100;
        newWarior1.ATK = 2;
        newWarior1.DEF = 1;
        newWarior1.SPD = 2;
        newWarior1.EXP = 20;
        newWarior1.level = 2;
        newWarior1.equipedMoves = new List<Move> { punch };
        Warior newWarior2 = new Warior();
        newWarior2.name = "enemy 2";
        newWarior2.HP = 100;
        newWarior2.ATK = 2;
        newWarior2.DEF = 1;
        newWarior2.SPD = 2;
        newWarior2.EXP = 20;
        newWarior2.level = 2;
        newWarior2.equipedMoves = new List<Move> { punch };

        playerParty.Add(newWarior);
        // Add more players/enemies as needed...

        enemyParty.Add(newWarior1);
        enemyParty.Add(newWarior2);
        // Add more enemies...

        // Start the battle
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        // Display initial battle UI, player stats, enemy stats, etc.
        battleText.text = "Battle Start!";

        yield return new WaitForSeconds(1f); // Wait for a second before starting the battle

        // Start the battle loop
        while (playerParty.Count > 0 && enemyParty.Count > 0)
        {
            moves.Clear();
            while (waiting)
                yield return null;

            // Decide turn order based on SPD stat
            List<Warior> allWarriors = new List<Warior>();
            allWarriors.AddRange(playerParty);
            allWarriors.AddRange(enemyParty);
            allWarriors.Sort((x, y) => y.SPD.CompareTo(x.SPD)); // Sort by SPD, higher goes first

            // Get moves
            foreach (var warrior in allWarriors)
            {
                if (warrior.HP > 0)
                {
                    // Choose a random move from equipedMoves
                    Move move = warrior.equipedMoves[Random.Range(0, warrior.equipedMoves.Count)];
                    move.user = warrior;

                    List<Warior> targets = new List<Warior>();
                    if (playerParty.Contains(warrior))
                    {
                        Debug.Log(warrior.name + "picked enemy");
                        targets = enemyParty;
                    }
                    else
                    {
                        Debug.Log(warrior.name + "picked player");
                        targets = playerParty;
                    }

                    for (int i = 0; i < move.amountOfTargets; i++)
                    {
                        move.targets.Add(targets[Random.Range(0, targets.Count)]);
                    }

                    //attack
                    foreach (var target in move.targets)
                    {
                        Debug.Log(target.name);
                        battleText.text = $"{move.user.name} attacks {target.name}!";

                        while (!Input.GetKeyDown(KeyCode.Space))
                            yield return null;

                        int damage = Mathf.Max(0, move.ATK + move.user.ATK - target.DEF);
                        battleText.text = $"{target.name} takes {damage} damage!";
                        target.HP -= damage;
                        Debug.Log(damage + ", " + move.ATK + ", " + warrior.ATK + ", " + target.HP);

                        if (target.HP <= 0)
                        {
                            battleText.text = $"{target.name} was defeated!";

                            while (!Input.GetKeyDown(KeyCode.Space))
                                yield return null;

                            if (playerParty.Contains(target))
                                playerParty.Remove(target);
                            else
                                enemyParty.Remove(target);
                        }
                        while (!Input.GetKeyDown(KeyCode.Space))
                            yield return null;
                    }
                }
            }

        }

        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        // Check battle result - player party or enemy party defeated
        if (playerParty.Count == 0)
        {
            battleText.text = "You lost!";
        }
        else
        {
            battleText.text = "You won!";
        }
    }
}