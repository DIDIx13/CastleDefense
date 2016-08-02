using UnityEngine;
using System.Collections;

public class RagdollManager : MonoBehaviour 
{
    public Collider[] cols;
    public Rigidbody[] rigids;

    Animator anim;
    bool goRagdoll = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();

        rigids = GetComponentsInChildren<Rigidbody>();
        cols = GetComponentsInChildren<Collider>();

        foreach(Rigidbody rig in rigids)
        {
            if (rig.gameObject.layer == 9)
            {
                rig.isKinematic = true;
            }
        }

        foreach (Collider col in cols)
        {
            if (col.gameObject.layer == 9)
            {
                col.isTrigger = true;
            }
        }
    }

    public void RagdollPlayer()
    {
        if(!goRagdoll)
        {
            anim.enabled = false;

            foreach (Rigidbody rig in rigids)
            {
                if (rig.gameObject.layer == 9)
                {
                    rig.isKinematic = false;
                }
            }

            foreach (Collider col in cols)
            {
                if (col.gameObject.layer == 9)
                {
                    col.isTrigger = false;
                }
            }

            goRagdoll = true;
        }
    }
}
