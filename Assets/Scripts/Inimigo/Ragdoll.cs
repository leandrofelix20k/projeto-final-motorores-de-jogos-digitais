using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    List<Rigidbody> ragdolRigids = new List<Rigidbody>();
    public Rigidbody rigid;
    List<Collider>  ragdolColliders = new List<Collider>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void DesativaRagdoll()
    {
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rigs.Length; i++)
        {
            if (rigs[i] == rigid)
            {
                continue;
            }
            ragdolRigids.Add(rigs[i]);
            rigs[i].isKinematic = true;

            Collider col = rigs[i].gameObject.GetComponent<Collider>();
            col.isTrigger = true;
            ragdolColliders.Add(col);
        }
    }

    public void AtivaRagdoll()
    {
        for(int i=0; i<ragdolRigids.Count; i++)
        {
            ragdolRigids[i].isKinematic = false;
            ragdolColliders[i].isTrigger = false;
        }

        rigid.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine("FinalizaAnimacao");
    }

    IEnumerator FinalizaAnimacao()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}
