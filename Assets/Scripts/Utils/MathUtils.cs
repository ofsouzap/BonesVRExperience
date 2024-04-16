using UnityEngine;

namespace BonesVr.Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// Wrap a integer from being in the range [-inf,inf] to being in the range [0,<paramref name="r"/>].
        /// This is done so that going negative wraps back to the right and going too positive wraps back round from the left.
        /// </summary>
        /// <param name="x">The value to wrap</param>
        /// <param name="r">The range's upper bound to wrap it to</param>
        public static int WrapInt(int x, int r)
        {
            if (x >= 0)
                return x % r;
            else
                return r - (Mathf.Abs(x) % r);
        }
    }
}
