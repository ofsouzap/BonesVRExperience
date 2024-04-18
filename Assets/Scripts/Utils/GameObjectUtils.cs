using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BonesVr.Utils
{
    public static class GameObjectUtils
    {
        public static string GetTransformRelativePathFrom(Transform t, Transform from)
        {
            IList<string> pathPartsRev = new List<string>();

            Transform curr = t;

            while (curr != from && curr != null)
            {
                pathPartsRev.Add(curr.name);
                curr = curr.transform.parent;
            }

            if (curr == null)
                throw new System.ArgumentException("from transform isn't an ancestor of query transform");
            else
                return string.Join('/', pathPartsRev.Reverse());
        }
    }
}
