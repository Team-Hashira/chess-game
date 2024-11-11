using System;
using System.Collections.Generic;
using UnityEngine;

public enum Curse
{
    Destruction,
    Pride,
    Envy,
    Gluttony,
    Belonging,
    Regret,
}
public enum Blessing
{
    Charity,
    Resection,
    Love,
    Penance,
}

[System.Serializable]
public class Card
{
    public Guid guid;
    public List<Curse> curse;
    public List<Blessing> blessing;
}
