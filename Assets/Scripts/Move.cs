using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public string name;
    public int ATK;
    public categories category;
    public string effect;
    public int amountOfTargets;
    public List<Warior> targets = new List<Warior>();
    public Warior user;

    public enum categories
    {
        Attack,
        Heal,
        Spell,
    }
}
