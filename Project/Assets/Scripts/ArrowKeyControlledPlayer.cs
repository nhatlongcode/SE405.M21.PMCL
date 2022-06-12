using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyControlledPlayer : HistoryObject
{
    public enum Command
    {
        Up, Down, Left, Right, Space, Enter, 
    }
    enum State
    {
        OnLadder = 1,           // TODO: Add snap to ladder feature
        OnGround = 2,           // TODO: Add snap to ground feature

    }
    State state;

    public Dictionary<Command, List<KeyCode>> validKeys = new Dictionary<Command, List<KeyCode>>
    {
        {Command.Down , new List<KeyCode>{KeyCode.S, KeyCode.DownArrow} },
        {Command.Up   , new List<KeyCode>{KeyCode.W, KeyCode.UpArrow} },
        {Command.Left , new List<KeyCode>{KeyCode.A, KeyCode.LeftArrow} },
        {Command.Right, new List<KeyCode>{KeyCode.D, KeyCode.RightArrow} },
        {Command.Space, new List<KeyCode>{KeyCode.Q, KeyCode.Space} },
        {Command.Enter, new List<KeyCode>{KeyCode.E, KeyCode.Return} },
    };

    Vector2 direction(Command dir)
    {
        switch (dir)
        {
            case Command.Up:
                return new Vector2(0, 1);
            case Command.Down:
                return new Vector2(0, -1);
            case Command.Left:
                return new Vector2(-1, 0);
            case Command.Right:
                return new Vector2(1, 0);
            default:
                return new Vector2(0, 0);
        } 
    }

    public new Collider2D collider;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (collider == null)
            collider = GetComponent<Collider2D>();
    }

    public const float accel = 10f;
    // Update is called once per frame
    public override void MyUpdate()
    {
        if (MovementKeyProcess())
        {
            History.Inst.StartTime();
            rigidbody.AddForce(movementCommands.GetAction(time) * deltaTime * accel);
        }
        else
        {
            History.Inst.StopTime();
        }
    }

    bool checkInput(Command cmd)
    {
        foreach (KeyCode key in validKeys[cmd])
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }

    int prevLRdir = 0;
    float currentLRDuration = 0;
    TimelineList<Vector2> movementCommands = new TimelineList<Vector2>();
    //Returns any actions valid for Movement
    protected bool MovementKeyProcess()
    {
        Vector2 dir = new Vector2(0 , 0);
        bool anyMovementKeyPressed = false;
        List<Command> movementCommandList = new List<Command>
        { 
            Command.Up, 
            Command.Down, 
            Command.Left, 
            Command.Right, 
            Command.Space 
        };

        foreach (var key in movementCommandList)
        {
            if (checkInput(key))
            {
                dir += direction(key);
                anyMovementKeyPressed = true;
            }
        }

        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        movementCommands.Add(time, dir);

        return anyMovementKeyPressed;
    }
}