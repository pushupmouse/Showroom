using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHolder : MonoBehaviour
{
    [SerializeField] private Target target;

    private TargetSpawner spawner;
    private int x;
    private int y;

    private void Start()
    {
        target.GetHolder(this);
    }

    public void Setup(TargetSpawner spawner, int x, int y)
    {
        DeactivateTarget();
        this.spawner = spawner;
        this.x = x;
        this.y = y;
    }

    public void OnTargetHit()
    {
        spawner.SpawnRandomTarget(x, y);
        spawner.OnTargetHit();
        ActivateTarget();
    }

    public void ActivateTarget()
    {
        target.Activate();
    }

    public void DeactivateTarget()
    {
        target.Deactivate();
    }
}
