using UnityEngine;
using System.Collections;

public class Dev_AnimTest : MonoBehaviour 
{
    public bool attLeft;
    public bool attLeftHit;
    public bool attRight;
    public bool attRightHit;
    public bool thrust;

    public Animator anim;

	void Update () 
    {
        if (attLeft)
        {
            anim.SetTrigger("attackLeft");
            attLeft = false;
        }

        if (attLeftHit)
        {
            anim.SetTrigger("attackLeftHit");
            attLeftHit = false;
        }

        if (attRight)
        {
            anim.SetTrigger("attackRight");
            attRight = false;
        }

        if (attRightHit)
        {
            anim.SetTrigger("attackRightHit");
            attRightHit = false;
        }

        if (thrust)
        {
            anim.SetTrigger("thrust");
            thrust = false;
        }
	}
}
