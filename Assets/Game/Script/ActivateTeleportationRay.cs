using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportationRay : MonoBehaviour
{
    [SerializeField] private GameObject leftTeleportationRay;
    [SerializeField] private GameObject rightTeleportationRay;
    [SerializeField] private InputActionProperty leftActivate;
    [SerializeField] private InputActionProperty rightActivate;
    [SerializeField] private InputActionProperty leftCancel;
    [SerializeField] private InputActionProperty rightCancel;
    [SerializeField] private XRRayInteractor leftRayInteractor;
    [SerializeField] private XRRayInteractor rightRayInteractor;

    private bool isLeftRayHovering, isRightRayHovering;

    private void Update()
    {
        isLeftRayHovering = leftRayInteractor.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftLine, out bool leftValid);
        isRightRayHovering = rightRayInteractor.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightLine, out bool rightValid);

        leftTeleportationRay.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportationRay.SetActive(!isRightRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
