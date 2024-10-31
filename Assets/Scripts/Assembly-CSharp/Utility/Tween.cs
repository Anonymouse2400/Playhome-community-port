using System;
using UnityEngine;

namespace Utility
{
	public class Tween
	{
		public static float Spring(float now, float goal, float K, float sec, float minSpeed = 0f, float maxSpeed = 0f)
		{
			float num = goal - now;
			float num2 = num * K * sec;
			if (Mathf.Abs(num) <= Mathf.Abs(num2))
			{
				return goal;
			}
			if (maxSpeed > 0f)
			{
				float num3 = maxSpeed * sec;
				if (Mathf.Abs(num2) > num3)
				{
					num2 = ((!(num2 >= 0f)) ? (0f - num3) : num3);
				}
			}
			if (minSpeed > 0f)
			{
				float num4 = minSpeed * sec;
				if (Mathf.Abs(num2) < num4)
				{
					num2 = ((!(num2 >= 0f)) ? (0f - num4) : num4);
				}
			}
			return now + num2;
		}

		public static float Linear(float nowTime, float endTime, float startVal, float changeVal)
		{
			return changeVal * nowTime / endTime + startVal;
		}

		public static float LinearMove(float now, float goal, float move)
		{
			float num = goal - now;
			if (Mathf.Abs(num) <= move)
			{
				return goal;
			}
			if (num < 0f)
			{
				return now - move;
			}
			return now + move;
		}

		public static float EasyInQuad(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return changeVal * nowTime * nowTime + startVal;
		}

		public static float EasyOutQuad(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return (0f - changeVal) * nowTime * (nowTime - 2f) + startVal;
		}

		public static float Quad(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return changeVal / 2f * nowTime * nowTime + startVal;
			}
			nowTime -= 1f;
			return (0f - changeVal) / 2f * (nowTime * (nowTime - 2f) - 1f) + startVal;
		}

		public static float EasyInCubic(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return changeVal * nowTime * nowTime * nowTime + startVal;
		}

		public static float EasyOutCubic(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			nowTime -= 1f;
			return changeVal * (nowTime * nowTime * nowTime + 1f) + startVal;
		}

		public static float Cubic(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return changeVal / 2f * nowTime * nowTime * nowTime + startVal;
			}
			nowTime -= 2f;
			return changeVal / 2f * (nowTime * nowTime * nowTime + 2f) + startVal;
		}

		public static float EasyInQuart(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return changeVal * nowTime * nowTime * nowTime * nowTime + startVal;
		}

		public static float EasyOutQuart(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			nowTime -= 1f;
			return (0f - changeVal) * (nowTime * nowTime * nowTime * nowTime - 1f) + startVal;
		}

		public static float Quart(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return changeVal / 2f * nowTime * nowTime * nowTime * nowTime + startVal;
			}
			nowTime -= 2f;
			return (0f - changeVal) / 2f * (nowTime * nowTime * nowTime * nowTime - 2f) + startVal;
		}

		public static float EasyInQuint(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return changeVal * nowTime * nowTime * nowTime * nowTime * nowTime + startVal;
		}

		public static float EasyOutQuint(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			nowTime -= 1f;
			return changeVal * (nowTime * nowTime * nowTime * nowTime * nowTime + 1f) + startVal;
		}

		public static float Quint(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return changeVal / 2f * nowTime * nowTime * nowTime * nowTime * nowTime + startVal;
			}
			nowTime -= 2f;
			return changeVal / 2f * (nowTime * nowTime * nowTime * nowTime * nowTime + 2f) + startVal;
		}

		public static float EasyInSine(float nowTime, float endTime, float startVal, float changeVal)
		{
			return (0f - changeVal) * Mathf.Cos(nowTime / endTime * ((float)Math.PI / 2f)) + changeVal + startVal;
		}

		public static float EasyOutSine(float nowTime, float endTime, float startVal, float changeVal)
		{
			return changeVal * Mathf.Sin(nowTime / endTime * ((float)Math.PI / 2f)) + startVal;
		}

		public static float Sine(float nowTime, float endTime, float startVal, float changeVal)
		{
			return (0f - changeVal) / 2f * (Mathf.Cos((float)Math.PI * nowTime / endTime) - 1f) + startVal;
		}

		public static float EasyInExpo(float nowTime, float endTime, float startVal, float changeVal)
		{
			return changeVal * Mathf.Pow(2f, 10f * (nowTime / endTime - 1f)) + startVal;
		}

		public static float EasyOutExpo(float nowTime, float endTime, float startVal, float changeVal)
		{
			return changeVal * (0f - Mathf.Pow(2f, -10f * nowTime / endTime) + 1f) + startVal;
		}

		public static float Expo(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return changeVal / 2f * Mathf.Pow(2f, 10f * (nowTime - 1f)) + startVal;
			}
			nowTime -= 1f;
			return changeVal / 2f * (0f - Mathf.Pow(2f, -10f * nowTime) + 2f) + startVal;
		}

		public static float EasyInCirc(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			return (0f - changeVal) * (Mathf.Sqrt(1f - nowTime * nowTime) - 1f) + startVal;
		}

		public static float EasyOutCirc(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime;
			nowTime -= 1f;
			return changeVal * Mathf.Sqrt(1f - nowTime * nowTime) + startVal;
		}

		public static float Circ(float nowTime, float endTime, float startVal, float changeVal)
		{
			nowTime /= endTime / 2f;
			if (nowTime < 1f)
			{
				return (0f - changeVal) / 2f * (Mathf.Sqrt(1f - nowTime * nowTime) - 1f) + startVal;
			}
			nowTime -= 2f;
			return changeVal / 2f * (Mathf.Sqrt(1f - nowTime * nowTime) + 1f) + startVal;
		}
	}
}
