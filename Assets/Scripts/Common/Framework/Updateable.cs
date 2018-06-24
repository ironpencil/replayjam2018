using UnityEngine;

public abstract class Updateable : ScriptableObject
{
    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
}