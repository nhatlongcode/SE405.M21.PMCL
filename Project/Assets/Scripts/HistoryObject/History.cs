using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : IHistoryObject
{
    private static History inst;

    public float time;
    public float deltaTime;
    public bool IsTimeStopped;

    public bool save;
    public bool restore;

    public List<IHistoryObject> ObjectList = new List<IHistoryObject>();
    public int saveCheckpoint = 0;
    public List<float> SaveTime;

    public static History Inst { get => inst; }

    History()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTimeStopped)
        {
            time += Time.deltaTime;
            deltaTime = Time.deltaTime;
        }
        else
        {
            deltaTime = 0;
        }

        if (PrevIsTimeStopped != IsTimeStopped)
        {
            if (IsTimeStopped)
                _StopTime();
            else
                _StartTime();
        }

        if (save)
        {
            Save(saveCheckpoint);

            save = false;
            saveCheckpoint++;
        }    
        if (restore)
        {
            Restore(saveCheckpoint - 1);
            restore = false;
        }

        List<IHistoryObject> objListClone1 = new List<IHistoryObject>(ObjectList);
        foreach (var item in objListClone1)
        {
            item.MyUpdate();
        }
    }

    public override void Save(int id)
    {
        foreach (var item in ObjectList)
        {
            item.Save(id);
        }
        SaveTime.Add(time);
    }

    public override void Restore(int id)
    {
        List<IHistoryObject> objListClone1 = new List<IHistoryObject>(ObjectList);
        foreach (var item in objListClone1)
            item.gameObject.SetActive(false);
        
        foreach (var item in objListClone1)
            item.Restore(id);

        List<IHistoryObject> objListClone2 = new List<IHistoryObject>(ObjectList);
        foreach (var item in objListClone2)
            item.gameObject.SetActive(true);
        time = SaveTime[id];
    }

    public override void Initialize()
    {
        
    }

    public bool PrevIsTimeStopped;
    private void _StopTime()
    {
        PrevIsTimeStopped = true;
        foreach (var item in ObjectList)
            item.StopTime();
    }

    private void _StartTime()
    {
        PrevIsTimeStopped = false;
        List<IHistoryObject> objListClone1 = new List<IHistoryObject>(ObjectList);
        foreach (var item in objListClone1)
            item.StartTime();
    }

    public override void StopTime()
    {
        IsTimeStopped = true;
    }

    public override void StartTime()
    {
        IsTimeStopped = false;
    }
}
