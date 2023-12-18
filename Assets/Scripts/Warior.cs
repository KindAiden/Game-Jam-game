using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warior : Creature
{
    public int maxHP;
    public int HP;
    public int ATK;
    public int DEF;
    public int SPD;
    public int EXP;
    public int level;
    public List<Move> allMoves = new List<Move>();
    public List<Move> equipedMoves = new List<Move>();
}
