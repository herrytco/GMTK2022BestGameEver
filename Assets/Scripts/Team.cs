using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public Team(string name, int id)
    {
        this.name = name;
        this.id = id;
    }

    string name;
    int id; //iterate for each team
    Color color = Color.white;

    public string Name { get => name; set => name = value; }
    public int Id { get => id; set => id = value; }
    public Color Color { get => color; set => color = value; }
}
