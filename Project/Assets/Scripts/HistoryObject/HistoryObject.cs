using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryObject : IHistoryObject
{
    Dictionary<int, Vector2> position = new Dictionary<int, Vector2>();
    Dictionary<int, float> rotation = new Dictionary<int, float>();
    Dictionary<int, Vector2> velocity = new Dictionary<int, Vector2>();
    Dictionary<int, float> angularVelocity = new Dictionary<int, float>();
    public new Rigidbody2D rigidbody;

    Vector2 tempVelocity;
    float tempAngularVelocity;

    public float time { get => History.Inst.time; }
    public float deltaTime { get => History.Inst.deltaTime; }
    public bool IsTimeStopped;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
        History.Inst.ObjectList.Add(this);
    }

    public override void Save(int id)
    {
        if (position.ContainsKey(id))
        {
            Debug.LogError("Duplicate save key.");
            return;
        }
        if (rigidbody != null)
        {
            position[id] = rigidbody.position;
            velocity[id] = rigidbody.velocity + tempVelocity;
            angularVelocity[id] = rigidbody.angularVelocity + tempAngularVelocity;
            rotation[id] = rigidbody.rotation;
        }
        else
        {
            position[id] = transform.position;
            rotation[id] = transform.rotation.z;
        }
    }

    public override void Restore(int id)
    {
        if (!position.ContainsKey(id))
        {
            History.Inst.ObjectList.Remove(this);
            Destroy(this.gameObject);
            return;
        }
        if (rigidbody != null)
        {
            rigidbody.position = position[id];
            rigidbody.rotation = rotation[id];
            if (IsTimeStopped)
            {
                tempVelocity = velocity[id];
                tempAngularVelocity = angularVelocity[id];
            }
            else
            {
                rigidbody.velocity = velocity[id];
                rigidbody.angularVelocity = angularVelocity[id];
            }
        }
        transform.position = position[id];
        transform.rotation = Quaternion.Euler(0, 0, rotation[id]);
    }

    public override void Initialize()
    {
        
    }

    public override void StopTime()
    {
        if (rigidbody != null)
        {
            tempVelocity = rigidbody.velocity;
            rigidbody.velocity = new Vector2(0, 0);
            tempAngularVelocity = rigidbody.angularVelocity;
            rigidbody.angularVelocity = 0;
        }
        IsTimeStopped = true;
    }

    public override void StartTime()
    {
        if (rigidbody != null)
        {
            rigidbody.velocity = tempVelocity;
            tempVelocity = new Vector2(0, 0);
            rigidbody.angularVelocity = tempAngularVelocity;
            tempAngularVelocity = 0;
        }
        IsTimeStopped = false;
    }
}
