using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class SeekCoverService
    {
        public Transform GetBestCoverPoint(Transform unit ,List<Transform> coverPoints)
        {
            Transform bestCover = null;
            var closestDistance = Mathf.Infinity;

            foreach (Transform coverPoint in coverPoints)
            {
                var distanceToPoint = Vector3.Distance(coverPoint.position, unit.position);
                if (distanceToPoint < closestDistance)
                {
                    bestCover = coverPoint;
                    closestDistance = distanceToPoint;
                }
            }

            return bestCover;
        }
    }
}