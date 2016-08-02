using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoneReplacer : MonoBehaviour
{
    public GameObject target;
    SkinnedMeshRenderer myRenderer;
    Transform[] newBones;
    SkinnedMeshRenderer targetRenderer;
    Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();

    void Start()
    {
        target = transform.parent.parent.GetChild(0).gameObject;
        targetRenderer = target.GetComponent<SkinnedMeshRenderer>();

        foreach (Transform bone in targetRenderer.bones)
        {
            boneMap[bone.gameObject.name] = bone;
        }

        myRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        newBones = new Transform[myRenderer.bones.Length];

        for (int i = 0; i < myRenderer.bones.Length; i++)
        {
            GameObject bone = myRenderer.bones[i].gameObject;
            if (!boneMap.TryGetValue(bone.name, out newBones[i]))
            {
                Debug.Log("Unable to map bone ~" + bone.name + "~ to target skeleton!");
                break;
            }
        }

        myRenderer.bones = newBones;
    }
}