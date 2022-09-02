using System;
using UnityEngine;

namespace gWeatherFunctions
{
	public static class OrwConversions
	{
		public static int FloatToInt(float fVal)
		{
			return (int)fVal;
		}

		public static float IntToFloat(int nVal)
		{
			return (float)nVal;
		}

		public static float IntToPercent(int nValue, int nRange)
		{
			float num = (float)nValue / (float)nRange * 100f;
			return Mathf.Round(num * 10f) / 10f;
		}

		public static float FloatToRounded(float fValue, int nDecimals)
		{
			float num = 0f;
			if (nDecimals <= 1)
			{
				num = fValue * 10f;
				int num2 = (int)num;
				num = (float)num2 * 0.1f;
			}
			if (nDecimals == 2)
			{
				num = fValue * 100f;
				int num2 = (int)num;
				num = (float)num2 * 0.01f;
			}
			if (nDecimals == 3)
			{
				num = fValue * 1000f;
				int num2 = (int)num;
				num = (float)num2 * 0.001f;
			}
			if (nDecimals >= 4)
			{
				num = fValue * 10000f;
				int num2 = (int)num;
				num = (float)num2 * 0.0001f;
			}
			return num;
		}

		public static string FloatToString(float fValue, int nDecimals)
		{
			string empty = string.Empty;
			return OrwConversions.FloatToRounded(fValue, nDecimals).ToString();
		}

		public static float StringToFloat(string sValue)
		{
			float result;
			try
			{
				float num = float.Parse(sValue);
				result = num;
			}
			catch
			{
				result = -1f;
			}
			return result;
		}

		public static int StringToInt(string sString)
		{
			int result;
			try
			{
				int num = int.Parse(sString);
				result = num;
			}
			catch
			{
				result = -1;
			}
			return result;
		}

		public static string IntToString(int nVal)
		{
			return nVal.ToString();
		}

		public static int ScaleValueInt(int nValue, int nInputMin, int nInputMax, int nScaleMin, int nScaleMax)
		{
			int num = nInputMax - nInputMin;
			float num2 = (float)num;
			float num3 = (float)nScaleMin;
			float num4 = (float)nScaleMax;
			float num5 = num4 - num3;
			float num6 = num5 / num2;
			if (nValue < nInputMin)
			{
				nValue = nInputMin;
			}
			if (nValue > nInputMax)
			{
				nValue = nInputMax;
			}
			float num7 = ((float)nValue - (float)nInputMin) * num6;
			float num8 = num7 + num3;
			return (int)num8;
		}

		public static float ScaleValueFloat(float fValue, float fInputMin, float fInputMax, float fScaleMin, float fScaleMax)
		{
			float num = fInputMax - fInputMin;
			float num2 = fScaleMax - fScaleMin;
			float num3 = num2 / num;
			if (fValue < fInputMin)
			{
				fValue = fInputMin;
			}
			if (fValue > fInputMax)
			{
				fValue = fInputMax;
			}
			float num4 = (fValue - fInputMin) * num3;
			return num4 + fScaleMin;
		}
	}
}
