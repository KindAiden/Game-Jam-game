using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Creature
{
    public float speed;
    public Vector2[] path;
    public string text;
    public bool battleable;
    public bool autoBattle;
    public List<Warior> enemies = new List<Warior>();
}
