using System;
using UnityEngine;

namespace HeadDistanceTravelled
{
    public interface IHeadDistanceTravelledController
    {
        float HMDDistance { get; }
        Vector3 HMDPositon { get; }
        Quaternion HMDRotation { get; }
        float LeftHandDistance { get; }
        float RightHandDistance { get; }
        float TotalHandDistance { get; }
        event HMDDistanceChangedEventHandler OnDistanceChanged;
        event Action<IHeadDistanceTravelledController> OnDestroied;
    }
    public delegate void HMDDistanceChangedEventHandler(float distance, in Vector3 hmdDistance, in Quaternion hmdRotation);
}