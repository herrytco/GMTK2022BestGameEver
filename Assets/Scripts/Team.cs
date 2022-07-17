using System;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public Team(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public List<String> characterNames  = new List<string>();

    public string Name { get; set; }

    public int Id { get; set; }

    public Color Color { get; set; } = Color.white;
}