using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetInfo
{
    /// <summary>
    /// If a target is in range, this method will return true. You can provide a position and direction for the ray, as well as the range and the layer mask. Provides what was hit as HitInfo.
    /// </summary>
    /// <param name="rayPosition"></param>
    /// <param name="rayDirection"></param>
    /// <param name="HitInfo"></param>
    /// <param name="range"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static bool IsTargetInRange(Vector3 rayPosition, Vector3 rayDirection, out RaycastHit HitInfo, float range, LayerMask mask)
    {
        return (Physics.Raycast(rayPosition, rayDirection, out HitInfo, range, mask));
    }
}
