using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public static class Extensions
    {
		public static T Random<T>(this IList<T> list)
		{
			return list.Count > 0 ? list[UnityEngine.Random.Range(0, list.Count)] : default;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			var n = list.Count;

			while (n > 1)
			{
				n--;
				var k = UnityEngine.Random.Range(0, n);
				(list[n], list[k]) = (list[k], list[n]);
			}
		}

		public static void Release(this GameObject target)
		{
			target.GetComponent<PoolMember>().Release();
		}
	}
}