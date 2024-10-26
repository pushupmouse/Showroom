using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private TargetHolder holder;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void GetHolder(TargetHolder holder)
    {
        this.holder = holder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(other.gameObject);
            holder.OnTargetHit();
            Deactivate();
        }
    }
}
