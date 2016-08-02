using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour 
{
    Animator anim;

    Vector3 lFPos;
    Vector3 rFPos;

    Quaternion lFRot;
    Quaternion rFRot;

    float lFWeight;
    float rFWeight;

    Transform leftFoot;
    Transform rightFoot;

    void Start()
    {
        anim = GetComponent<Animator>();

        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);

        lFRot = leftFoot.rotation;
        rFRot = rightFoot.rotation;
    }

    void Update()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 lPos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rPos = rightFoot.TransformPoint(Vector3.zero);

        if(Physics.Raycast(lPos, -Vector3.up, out leftHit, 1))
        {
            lFPos = leftHit.point;
            lFRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }

        if (Physics.Raycast(rPos, -Vector3.up, out rightHit, 1))
        {
            rFPos = rightHit.point;
            rFRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }
    }

    void OnAnimatorIK()
    {
        lFWeight = anim.GetFloat("LeftFoot");
        rFWeight = anim.GetFloat("RightFoot");

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lFWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rFWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFPos);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rFPos);

        anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, lFWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, rFWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFRot);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rFRot);    
    }
}
