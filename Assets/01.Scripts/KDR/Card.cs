using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Curse
{
    Destruction,
    Pride,
    Envy,
    Gluttony,
    Belonging,
    Regret,
}
[System.Serializable]
public enum Blessing
{
    Charity,
    Resection,
    Love,
    Penance,
}

public class Card : MonoBehaviour
{
    public List<Curse> curse;
    public List<Blessing> blessing;

    public int Cost { get; private set; }
}
