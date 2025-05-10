using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string nome;
    public Sprite icone;

    public abstract void Aplicar();
}
