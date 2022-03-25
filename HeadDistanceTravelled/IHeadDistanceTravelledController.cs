using UnityEngine;

namespace HeadDistanceTravelled
{
    public interface IHeadDistanceTravelledController
    {
        float HMDDistance { get; }
        Vector3 HMDPositon { get; }
        Quaternion HMDRotation { get; }
        event HMDDistanceChangedEventHandler OnDistanceChanged;
    }
    public delegate void HMDDistanceChangedEventHandler(float distance, in Vector3 hmdDistance, in Quaternion hmdRotation);
}