using System;
using UnityEngine;

namespace gWeatherFunctions
{
	public static class OrwRandom
	{
		public static float GetRandomFloat(float fMinimum, float fMaximum, int nNumber, int nAverage)
		{
			float result = 0f;
			float num = (float)nNumber;
			float num2 = 0f;
			if (nNumber > 1)
			{
				for (int i = 0; i < nNumber; i++)
				{
					float num3 = UnityEngine.Random.Range(fMinimum, fMaximum);
					num2 += num3;
					result = num2;
				}
				if (nAverage == 1)
				{
					result = num2 / num;
				}
			}
			else
			{
				result = UnityEngine.Random.Range(fMinimum, fMaximum);
			}
			return result;
		}

		public static int GetRandomInteger(int nMinimum, int nMaximum, int nNumber, int nAverage)
		{
			int result = 0;
			int num = 0;
			if (nNumber > 1)
			{
				for (int i = 0; i < nNumber; i++)
				{
					int num2 = UnityEngine.Random.Range(nMinimum, nMaximum);
					num += num2;
					result = num;
				}
				if (nAverage == 1)
				{
					result = num / nNumber;
				}
			}
			else
			{
				result = UnityEngine.Random.Range(nMinimum, nMaximum);
			}
			return result;
		}
	}
}
