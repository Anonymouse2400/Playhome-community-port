using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	public static class Helper
	{
		public const string ScriptVersion = "1.6.10";

		public static string GetName(Platform platform)
		{
			string text = "Unknown";
			return platform.ToString();
		}

		public static string GetErrorMessage(ErrorCode code)
		{
			string text = string.Empty;
			switch (code)
			{
			case ErrorCode.None:
				text = "No Error";
				break;
			case ErrorCode.LoadFailed:
				text = "Loading failed.  Codec not supported or video resolution too high or insufficient system resources.";
				if (SystemInfo.operatingSystem.StartsWith("Windows XP") || SystemInfo.operatingSystem.StartsWith("Windows Vista"))
				{
					text += " NOTE: Windows XP and Vista don't have native support for H.264 codec.  Consider using an older codec such as DivX or installing 3rd party codecs such as LAV Filters.";
				}
				break;
			case ErrorCode.DecodeFailed:
				text = "Decode failed.  Possible codec not supported, video resolution too high or insufficient system resources.";
				break;
			}
			return text;
		}

		public static string[] GetPlatformNames()
		{
			return new string[8]
			{
				GetName(Platform.Windows),
				GetName(Platform.MacOSX),
				GetName(Platform.iOS),
				GetName(Platform.tvOS),
				GetName(Platform.Android),
				GetName(Platform.WindowsPhone),
				GetName(Platform.WindowsUWP),
				GetName(Platform.WebGL)
			};
		}

		public static void LogInfo(string message, UnityEngine.Object context = null)
		{
			if (context == null)
			{
				Debug.Log("[AVProVideo] " + message);
			}
			else
			{
				Debug.Log("[AVProVideo] " + message, context);
			}
		}

		public static string GetTimeString(float totalSeconds, bool showMilliseconds = false)
		{
			int num = Mathf.FloorToInt(totalSeconds / 3600f);
			float num2 = (float)num * 60f * 60f;
			int num3 = Mathf.FloorToInt((totalSeconds - num2) / 60f);
			num2 += (float)num3 * 60f;
			int num4 = Mathf.FloorToInt(totalSeconds - num2);
			if (num <= 0)
			{
				if (showMilliseconds)
				{
					int num5 = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
					return string.Format("{0:00}:{1:00}:{2:000}", num3, num4, num5);
				}
				return string.Format("{0:00}:{1:00}", num3, num4);
			}
			if (showMilliseconds)
			{
				int num6 = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
				return string.Format("{2}:{0:00}:{1:00}:{3:000}", num3, num4, num, num6);
			}
			return string.Format("{2}:{0:00}:{1:00}", num3, num4, num);
		}

		public static Orientation GetOrientation(float[] t)
		{
			Orientation result = Orientation.Landscape;
			if (t[0] == 0f && t[1] == 1f && t[2] == -1f && t[3] == 0f)
			{
				result = Orientation.Portrait;
			}
			else if (t[0] == 0f && t[1] == -1f && t[2] == 1f && t[3] == 0f)
			{
				result = Orientation.PortraitFlipped;
			}
			else if (t[0] == 1f && t[1] == 0f && t[2] == 0f && t[3] == 1f)
			{
				result = Orientation.Landscape;
			}
			else if (t[0] == -1f && t[1] == 0f && t[2] == 0f && t[3] == -1f)
			{
				result = Orientation.LandscapeFlipped;
			}
			return result;
		}

		public static Matrix4x4 GetMatrixForOrientation(Orientation ori)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 1f, 0f), Quaternion.Euler(0f, 0f, -90f), Vector3.one);
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(new Vector3(1f, 0f, 0f), Quaternion.Euler(0f, 0f, 90f), Vector3.one);
			Matrix4x4 matrix4x3 = Matrix4x4.TRS(new Vector3(1f, 1f, 0f), Quaternion.identity, new Vector3(-1f, -1f, 1f));
			Matrix4x4 result = Matrix4x4.identity;
			switch (ori)
			{
			case Orientation.LandscapeFlipped:
				result = matrix4x3;
				break;
			case Orientation.Portrait:
				result = matrix4x;
				break;
			case Orientation.PortraitFlipped:
				result = matrix4x2;
				break;
			}
			return result;
		}

		public static void SetupStereoMaterial(Material material, StereoPacking packing, bool displayDebugTinting)
		{
			material.DisableKeyword("STEREO_CUSTOM_UV");
			material.DisableKeyword("STEREO_TOP_BOTTOM");
			material.DisableKeyword("STEREO_LEFT_RIGHT");
			material.DisableKeyword("MONOSCOPIC");
			switch (packing)
			{
			case StereoPacking.TopBottom:
				material.EnableKeyword("STEREO_TOP_BOTTOM");
				break;
			case StereoPacking.LeftRight:
				material.EnableKeyword("STEREO_LEFT_RIGHT");
				break;
			case StereoPacking.CustomUV:
				material.EnableKeyword("STEREO_CUSTOM_UV");
				break;
			}
			if (displayDebugTinting)
			{
				material.EnableKeyword("STEREO_DEBUG");
			}
			else
			{
				material.DisableKeyword("STEREO_DEBUG");
			}
		}

		public static void SetupAlphaPackedMaterial(Material material, AlphaPacking packing)
		{
			material.DisableKeyword("ALPHAPACK_TOP_BOTTOM");
			material.DisableKeyword("ALPHAPACK_LEFT_RIGHT");
			material.DisableKeyword("ALPHAPACK_NONE");
			switch (packing)
			{
			case AlphaPacking.TopBottom:
				material.EnableKeyword("ALPHAPACK_TOP_BOTTOM");
				break;
			case AlphaPacking.LeftRight:
				material.EnableKeyword("ALPHAPACK_LEFT_RIGHT");
				break;
			}
		}

		public static void SetupGammaMaterial(Material material, bool playerSupportsLinear)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear && !playerSupportsLinear)
			{
				material.EnableKeyword("APPLY_GAMMA");
			}
			else
			{
				material.DisableKeyword("APPLY_GAMMA");
			}
		}

		public static int ConvertTimeSecondsToFrame(float seconds, float frameRate)
		{
			return Mathf.FloorToInt(frameRate * seconds);
		}

		public static float ConvertFrameToTimeSeconds(int frame, float frameRate)
		{
			float num = 1f / frameRate;
			return (float)frame * num + num * 0.5f;
		}

		public static void DrawTexture(Rect screenRect, Texture texture, ScaleMode scaleMode, AlphaPacking alphaPacking, Material material)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			float num = texture.width;
			float num2 = texture.height;
			switch (alphaPacking)
			{
			case AlphaPacking.LeftRight:
				num *= 0.5f;
				break;
			case AlphaPacking.TopBottom:
				num2 *= 0.5f;
				break;
			}
			float num3 = num / num2;
			Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
			switch (scaleMode)
			{
			case ScaleMode.ScaleAndCrop:
			{
				float num7 = screenRect.width / screenRect.height;
				if (num7 > num3)
				{
					float num8 = num3 / num7;
					sourceRect = new Rect(0f, (1f - num8) * 0.5f, 1f, num8);
				}
				else
				{
					float num9 = num7 / num3;
					sourceRect = new Rect(0.5f - num9 * 0.5f, 0f, num9, 1f);
				}
				break;
			}
			case ScaleMode.ScaleToFit:
			{
				float num4 = screenRect.width / screenRect.height;
				if (num4 > num3)
				{
					float num5 = num3 / num4;
					screenRect = new Rect(screenRect.xMin + screenRect.width * (1f - num5) * 0.5f, screenRect.yMin, num5 * screenRect.width, screenRect.height);
				}
				else
				{
					float num6 = num4 / num3;
					screenRect = new Rect(screenRect.xMin, screenRect.yMin + screenRect.height * (1f - num6) * 0.5f, screenRect.width, num6 * screenRect.height);
				}
				break;
			}
			}
			Graphics.DrawTexture(screenRect, texture, sourceRect, 0, 0, 0, 0, GUI.color, material);
		}

		public static Texture2D GetReadableTexture(Texture inputTexture, bool requiresVerticalFlip, Orientation ori, Texture2D targetTexture)
		{
			Texture2D texture2D = targetTexture;
			RenderTexture active = RenderTexture.active;
			int width = inputTexture.width;
			int height = inputTexture.height;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
			if (ori == Orientation.Landscape)
			{
				if (!requiresVerticalFlip)
				{
					Graphics.Blit(inputTexture, temporary);
				}
				else
				{
					GL.PushMatrix();
					RenderTexture.active = temporary;
					GL.LoadPixelMatrix(0f, temporary.width, 0f, temporary.height);
					Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
					Rect screenRect = new Rect(0f, -1f, temporary.width, temporary.height);
					Graphics.DrawTexture(screenRect, inputTexture, sourceRect, 0, 0, 0, 0);
					GL.PopMatrix();
					GL.InvalidateState();
				}
			}
			if (texture2D == null)
			{
				texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
			}
			RenderTexture.active = temporary;
			texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, false);
			texture2D.Apply(false, false);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = active;
			return texture2D;
		}

		private static int ParseTimeToMs(string text)
		{
			int result = 0;
			string[] array = text.Split(':', ',');
			if (array.Length == 4)
			{
				int num = int.Parse(array[0]);
				int num2 = int.Parse(array[1]);
				int num3 = int.Parse(array[2]);
				int num4 = int.Parse(array[3]);
				result = num4 + (num3 + (num2 + num * 60) * 60) * 1000;
			}
			return result;
		}

		public static List<Subtitle> LoadSubtitlesSRT(string data)
		{
			List<Subtitle> list = null;
			if (!string.IsNullOrEmpty(data))
			{
				data = data.Trim();
				string[] array = data.Split(new string[4] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);
				if (array.Length >= 3)
				{
					list = new List<Subtitle>(256);
					int num = 0;
					int num2 = 0;
					Subtitle subtitle = null;
					for (int i = 0; i < array.Length; i++)
					{
						switch (num2)
						{
						case 0:
							subtitle = new Subtitle();
							subtitle.index = num;
							break;
						case 1:
						{
							string[] array2 = array[i].Split(new string[1] { " --> " }, StringSplitOptions.RemoveEmptyEntries);
							if (array2.Length == 2)
							{
								subtitle.timeStartMs = ParseTimeToMs(array2[0]);
								subtitle.timeEndMs = ParseTimeToMs(array2[1]);
							}
							break;
						}
						default:
							if (!string.IsNullOrEmpty(array[i]))
							{
								if (num2 == 2)
								{
									subtitle.text = array[i];
									break;
								}
								Subtitle subtitle2 = subtitle;
								subtitle2.text = subtitle2.text + "\n" + array[i];
							}
							break;
						}
						if (string.IsNullOrEmpty(array[i]) && num2 > 1)
						{
							list.Add(subtitle);
							num2 = 0;
							num++;
							subtitle = null;
						}
						else
						{
							num2++;
						}
					}
					if (subtitle != null)
					{
						list.Add(subtitle);
						subtitle = null;
					}
				}
				else
				{
					Debug.LogWarning("[AVProVideo] SRT format doesn't appear to be valid");
				}
			}
			return list;
		}
	}
}
