using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider _mainColider;
    [SerializeField]
    private GameObject _personalRig;
    [SerializeField]
    private Animator _personalAnimator;

    private Collider[] _ragDollColliders;
    private Rigidbody[] _limbsRigidbodies;

    private void Awake()
    {
        GetRagdollBits();
    }
    private void GetRagdollBits()
    {
        _ragDollColliders = _personalRig.GetComponentsInChildren<Collider>();
        _limbsRigidbodies = _personalRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollModeOnOff(bool off)
    {
        foreach (Collider col in _ragDollColliders)
        {
            col.enabled = !off;
        }

        foreach (Rigidbody rigid in _limbsRigidbodies)
        {
            rigid.isKinematic = off;
        }
        _personalAnimator.enabled = off;
        _mainColider.enabled = off;
        GetComponent<Rigidbody>().isKinematic = !off;
    }
}
