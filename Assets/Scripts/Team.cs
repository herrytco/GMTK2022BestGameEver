using UnityEngine;

public class Team
{
    public Team(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public string Name { get; set; }

    public int Id { get; set; }

    public Color Color { get; set; } = Color.gray;
}