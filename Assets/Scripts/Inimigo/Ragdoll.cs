using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody rigid;
    List<Rigidbody> ragdolRigids = new List<Rigidbody>();
    List<Collider> ragdolColliders = new List<Collider>();

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void DesativaRagdoll()
    {
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rigs)
        {
            if (rb == rigid) continue;

            ragdolRigids.Add(rb);
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Collider col = rb.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;
                ragdolColliders.Add(col);
            }
        }
    }

    public void AtivaRagdoll()
    {
        foreach (var rb in ragdolRigids)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        foreach (var col in ragdolColliders)
        {
            col.isTrigger = false;
        }

        rigid.isKinematic = true;

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            capsule.enabled = false;
        }

        StartCoroutine(FinalizaAnimacao());
    }

    IEnumerator FinalizaAnimacao()
    {
        yield return new WaitForEndOfFrame();
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;

        this.enabled = false;
    }
}
