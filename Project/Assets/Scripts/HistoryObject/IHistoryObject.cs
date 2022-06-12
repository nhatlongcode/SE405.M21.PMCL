using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHistoryObject : MonoBehaviour
{
    // Returns the id of the save
    public abstract void Save(int id);
    public abstract void Restore(int id);
    public abstract void Initialize();
    public abstract void StopTime();
    public abstract void StartTime();

    public virtual void MyUpdate() { }
}
