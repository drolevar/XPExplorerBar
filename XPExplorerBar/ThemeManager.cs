/*
 * Copyright © 2004-2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice,
 *      this list of conditions and the following disclaimer.
 *
 *    - Redistributions in binary form must reproduce the above copyright notice,
 *      this list of conditions and the following disclaimer in the documentation
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY
 * OF SUCH DAMAGE.
 */


using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace XPExplorerBar
{
	#region ThemeManager Class

	/// <summary>
	/// A class that extracts theme settings from Windows XP shellstyle dlls
	/// </summary>
	public class ThemeManager
	{
		/// <summary>
		/// pointer to a shellstyle dll
		/// </summary>
		private static IntPtr hModule = IntPtr.Zero;

		/// <summary>
		/// cached version of the current shellstyle in use
		/// </summary>
		private static ExplorerBarInfo currentShellStyle;



		/// <summary>
		/// Gets the System defined settings for the ExplorerBar according
		/// to the current System theme
		/// </summary>
		/// <returns>An ExplorerBarInfo object that contains the System defined 
		/// settings for the ExplorerBar according to the current System theme</returns>
		public static ExplorerBarInfo GetSystemExplorerBarSettings()
		{
			return GetSystemExplorerBarSettings(false);
		}


		/// <summary>
		/// Gets the System defined settings for the ExplorerBar according
		/// to the current System theme
		/// </summary>
		/// <param name="useClassicTheme">Specifies whether the current system theme 
		/// should be ignored and return unthemed settings</param>
		/// <returns>An ExplorerBarInfo object that contains the System defined 
		/// settings for the ExplorerBar according to the current System theme</returns>
		public static ExplorerBarInfo GetSystemExplorerBarSettings(bool useClassicTheme)
		{
			// check if we can return the cached theme
			// note: caching a classic theme seems to cause a few
			//       problems i haven't been able to resolve, so 
			//       for the moment always return a new 
			//       ExplorerBarInfo if useClassicTheme is true
			if (currentShellStyle != null && !useClassicTheme)
			{
				if (currentShellStyle.ShellStylePath is "default.xml")
				{
					return currentShellStyle;
				}
			}
            
			ExplorerBarInfo systemTheme;

			if (useClassicTheme)
			{
				// no themes available, so use default settings
				systemTheme = new ExplorerBarInfo();
				systemTheme.UseClassicTheme();
				systemTheme.SetOfficialTheme(true);

				// add non-themed arrows as the ExplorerBar will
				// look funny without them.
				systemTheme.SetUnthemedArrowImages();
			}
			else
			{
				try
				{
					systemTheme = GetSystemExplorerBarSettings(Theme.Default);
					systemTheme.SetOfficialTheme(true);
				}
				catch
				{
					// something went wrong, so use default settings
					systemTheme = new ExplorerBarInfo();
					systemTheme.UseClassicTheme();
					systemTheme.SetOfficialTheme(true);

					// add non-themed arrows as the ExplorerBar will
					// look funny without them.
					systemTheme.SetUnthemedArrowImages();
				}
			}

			// cache the theme
			currentShellStyle = systemTheme;

			return systemTheme;
		}


		/// <summary>
		/// Gets the System defined settings for the ExplorerBar specified
		/// by the shellstyle.dll at the specified path
		/// </summary>
		/// <param name="stylePath">The path to the shellstyle.dll</param>
		/// <returns>An ExplorerBarInfo object that contains the settings for 
		/// the ExplorerBar specified by the shellstyle.dll at the specified path</returns>
		public static ExplorerBarInfo GetSystemExplorerBarSettings(string stylePath)
		{
			// check if we can return the cached theme
			if (currentShellStyle != null)
			{
				if (!currentShellStyle.ClassicTheme && currentShellStyle.ShellStylePath != null && currentShellStyle.ShellStylePath.Equals(stylePath))
				{
					return currentShellStyle;
				}
			}
            
			ExplorerBarInfo systemTheme;

			try
			{
				using StreamReader reader = new StreamReader(stylePath);
				XmlSerializer serializer = new XmlSerializer(typeof(ExplorerBarInfo.ExplorerBarInfoSurrogate));
				var surrogate = (ExplorerBarInfo.ExplorerBarInfoSurrogate)serializer.Deserialize(reader);
				systemTheme = surrogate.Save();
				systemTheme.SetOfficialTheme(false);
				systemTheme.ShellStylePath = stylePath;
			}
			catch (Exception)
			{
				// no themes available, so use default settings
				systemTheme = new ExplorerBarInfo();
				systemTheme.UseClassicTheme();
				systemTheme.SetOfficialTheme(true);

				// add non-themed arrows as the ExplorerBar will
				// look funny without them.
				systemTheme.SetUnthemedArrowImages();
			}

			// cache the theme
			currentShellStyle = systemTheme;

			return systemTheme;
		}
		
		/// <summary>
		/// Gets the System defined settings for the ExplorerBar specified
		/// by the shellstyle.dll at the specified path
		/// </summary>
		/// <param name="stylePath">The path to the shellstyle.dll</param>
		/// <returns>An ExplorerBarInfo object that contains the settings for 
		/// the ExplorerBar specified by the shellstyle.dll at the specified path</returns>
		public static ExplorerBarInfo GetSystemExplorerBarSettings(Theme theme)
		{
			string stylePath = $"XPExplorerBar.Themes.{theme.ToString().ToLower()}.xml";

			// check if we can return the cached theme
			if (currentShellStyle != null)
			{
				if (!currentShellStyle.ClassicTheme && currentShellStyle.ShellStylePath != null && currentShellStyle.ShellStylePath.Equals(stylePath))
				{
					return currentShellStyle;
				}
			}
            
			ExplorerBarInfo systemTheme;

			try
			{
				// Read the resource into a stream
				using Stream stream = typeof(ThemeManager).Assembly.GetManifestResourceStream(stylePath);
				using StreamReader reader = new StreamReader(stream);
				XmlSerializer serializer = new XmlSerializer(typeof(ExplorerBarInfo.ExplorerBarInfoSurrogate));
				var surrogate = (ExplorerBarInfo.ExplorerBarInfoSurrogate)serializer.Deserialize(reader);
				systemTheme = surrogate.Save();
				systemTheme.SetOfficialTheme(false);
				systemTheme.ShellStylePath = stylePath;
			}
			catch (Exception)
			{
				// no themes available, so use default settings
				systemTheme = new ExplorerBarInfo();
				systemTheme.UseClassicTheme();
				systemTheme.SetOfficialTheme(true);

				// add non-themed arrows as the ExplorerBar will
				// look funny without them.
				systemTheme.SetUnthemedArrowImages();
			}

			// cache the theme
			currentShellStyle = systemTheme;

			return systemTheme;
		}


		#region Resources

		/// <summary>
		/// Converts an Image to a byte array
		/// </summary>
		/// <param name="image">The image to be converted</param>
		/// <returns>A byte array that contains the converted image</returns>
		internal static byte[] ConvertImageToByteArray(Image image)
		{
			if (image == null)
			{
				return new byte[0];
			}

			MemoryStream ms = new MemoryStream();

			image.Save(ms, ImageFormat.Png);

			return ms.ToArray();
		}


		/// <summary>
		/// Converts a byte array to an Image
		/// </summary>
		/// <param name="bytes">The array of bytes to be converted</param>
		/// <returns>An Image that represents the byte array</returns>
		internal static Image ConvertByteArrayToImage(byte[] bytes)
		{
			if (bytes.Length == 0)
			{
				return null;
			}

			MemoryStream ms = new MemoryStream(bytes);

			return Image.FromStream(ms);
		}


		/// <summary>
		/// Converts a Color to a string representation
		/// </summary>
		/// <param name="color">The Color to be converted</param>
		/// <returns>A string that represents the specified color</returns>
		internal static string ConvertColorToString(Color color)
		{
			if (color == Color.Empty)
			{
				return null;
			}

			return "" + color.A + ":" + color.R + ":" + color.G + ":" + color.B;
		}


		/// <summary>
		/// Converts a string to a color
		/// </summary>
		/// <param name="col">The string to be converted</param>
		/// <returns>The converted Color</returns>
		internal static Color ConvertStringToColor(string col)
		{
			if (col == null)
			{
				return Color.Empty;
			}

			string[] s = col.Split(':');

			if (s.Length != 4)
			{
				return Color.Empty;
			}

			return Color.FromArgb(Int32.Parse(s[0]), Int32.Parse(s[1]), Int32.Parse(s[2]), Int32.Parse(s[3]));
		}


		/// <summary>
		/// Converts an object to a byte array
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <returns>A byte array that contains the converted object</returns>
		internal static byte[] ConvertObjectToByteArray(object obj)
		{
			if (obj == null)
			{
				return new byte[0];
			}

			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, obj);

			byte[] bytes = stream.ToArray();

			stream.Flush();
			stream.Close();

			return bytes;
		}


		/// <summary>
		/// Converts a byte array to an object
		/// </summary>
		/// <param name="bytes">The array of bytes to be converted</param>
		/// <returns>An object that represents the byte array</returns>
		internal static object ConvertByteArrayToObject(byte[] bytes)
		{
			if (bytes.Length == 0)
			{
				return null;
			}

			MemoryStream stream = new MemoryStream(bytes);
			stream.Position = 0;

			IFormatter formatter = new BinaryFormatter();

			object obj = formatter.Deserialize(stream);

			stream.Close();

			return obj;
		}

		#endregion
	}

	#endregion



	#region UxTheme

	/// <summary>
	/// A class that wraps Windows XPs UxTheme.dll
	/// </summary>
	public static class UxTheme
	{

		#region Win32 Methods

		/// <summary>
		/// Opens the theme data for a window and its associated class
		/// </summary>
		/// <param name="hwnd">Handle of the window for which theme data 
		/// is required</param>
		/// <param name="pszClassList">Pointer to a string that contains 
		/// a semicolon-separated list of classes</param>
		/// <returns>OpenThemeData tries to match each class, one at a 
		/// time, to a class data section in the active theme. If a match 
		/// is found, an associated HTHEME handle is returned. If no match 
		/// is found NULL is returned</returns>
		[DllImport("UxTheme.dll")]
		public static extern IntPtr OpenThemeData(IntPtr hwnd, [MarshalAs(UnmanagedType.LPTStr)] string pszClassList);


		/// <summary>
		/// Closes the theme data handle
		/// </summary>
		/// <param name="hTheme">Handle to a window's specified theme data. 
		/// Use OpenThemeData to create an HTHEME</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise</returns>
		[DllImport("UxTheme.dll")]
		public static extern int CloseThemeData(IntPtr hTheme);


		/// <summary>
		/// Draws the background image defined by the visual style for the 
		/// specified control part
		/// </summary>
		/// <param name="hTheme">Handle to a window's specified theme data. 
		/// Use OpenThemeData to create an HTHEME</param>
		/// <param name="hdc">Handle to a device context (HDC) used for 
		/// drawing the theme-defined background image</param>
		/// <param name="iPartId">Value of type int that specifies the part 
		/// to draw</param>
		/// <param name="iStateId">Value of type int that specifies the state 
		/// of the part to draw</param>
		/// <param name="pRect">Pointer to a RECT structure that contains the 
		/// rectangle, in logical coordinates, in which the background image 
		/// is drawn</param>
		/// <param name="pClipRect">Pointer to a RECT structure that contains 
		/// a clipping rectangle. This parameter may be set to NULL</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise</returns>
		[DllImport("UxTheme.dll")]
		public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);


		/// <summary>
		/// Tests if a visual style for the current application is active
		/// </summary>
		/// <returns>TRUE if a visual style is enabled, and windows with 
		/// visual styles applied should call OpenThemeData to start using 
		/// theme drawing services, FALSE otherwise</returns>
		[DllImport("UxTheme.dll")]
		public static extern bool IsThemeActive();


		/// <summary>
		/// Reports whether the current application's user interface 
		/// displays using visual styles
		/// </summary>
		/// <returns>TRUE if the application has a visual style applied,
		/// FALSE otherwise</returns>
		[DllImport("UxTheme.dll")]
		public static extern bool IsAppThemed();


		/// <summary>
		/// Draws the part of a parent control that is covered by a 
		/// partially-transparent or alpha-blended child control
		/// </summary>
		/// <param name="hwnd">Handle of the child control</param>
		/// <param name="hdc">Handle to the child control's device context </param>
		/// <param name="prc">Pointer to a RECT structure that defines the 
		/// area to be drawn. The rectangle is in the child window's coordinates. 
		/// This parameter may be set to NULL</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise</returns>
		[DllImport("UxTheme.dll")]
		public static extern int DrawThemeParentBackground(IntPtr hwnd, IntPtr hdc, ref RECT prc);

		#endregion



		#region WindowClasses

		/// <summary>
		/// Window class IDs used by UxTheme.dll to draw controls
		/// </summary>
		public class WindowClasses
		{
			/// <summary>
			/// TextBox class
			/// </summary>
			public static readonly string Edit = "EDIT";

			/// <summary>
			/// ListView class
			/// </summary>
			public static readonly string ListView = "LISTVIEW";

			/// <summary>
			/// TreeView class
			/// </summary>
			public static readonly string TreeView = "TREEVIEW";
		}

		#endregion



		#region Parts

		/// <summary>
		/// Window parts IDs used by UxTheme.dll to draw controls
		/// </summary>
		public class Parts
		{
			#region Edit

			/// <summary>
			/// TextBox parts
			/// </summary>
			public enum Edit
			{
				/// <summary>
				/// TextBox
				/// </summary>
				EditText = 1
			}

			#endregion


			#region ListView

			/// <summary>
			/// ListView parts
			/// </summary>
			public enum ListView
			{
				/// <summary>
				/// ListView
				/// </summary>
				ListItem = 1
			}

			#endregion


			#region TreeView

			/// <summary>
			/// TreeView parts
			/// </summary>
			public enum TreeView
			{
				/// <summary>
				/// TreeView
				/// </summary>
				TreeItem = 1
			}

			#endregion
		}

		#endregion



		#region PartStates

		/// <summary>
		/// Window part state IDs used by UxTheme.dll to draw controls
		/// </summary>
		public class PartStates
		{
			#region EditParts

			/// <summary>
			/// TextBox part states
			/// </summary>
			public enum EditText
			{
				/// <summary>
				/// The TextBox is in its normal state
				/// </summary>
				Normal = 1,

				/// <summary>
				/// The mouse is over the TextBox
				/// </summary>
				Hot = 2,

				/// <summary>
				/// The TextBox is selected
				/// </summary>
				Selected = 3,

				/// <summary>
				/// The TextBox is disabled
				/// </summary>
				Disabled = 4,

				/// <summary>
				/// The TextBox currently has focus
				/// </summary>
				Focused = 5,

				/// <summary>
				/// The TextBox is readonly
				/// </summary>
				Readonly = 6
			}

			#endregion


			#region ListViewParts

			/// <summary>
			/// ListView part states
			/// </summary>
			public enum ListItem
			{
				/// <summary>
				/// The ListView is in its normal state
				/// </summary>
				Normal = 1,

				/// <summary>
				/// The mouse is over the ListView
				/// </summary>
				Hot = 2,

				/// <summary>
				/// The ListView is selected
				/// </summary>
				Selected = 3,

				/// <summary>
				/// The ListView is disabled
				/// </summary>
				Disabled = 4,

				/// <summary>
				/// The ListView is selected but currently does not have focus
				/// </summary>
				SelectedNotFocused = 5
			}

			#endregion


			#region TreeViewParts

			/// <summary>
			/// TreeView part states
			/// </summary>
			public enum TreeItem
			{
				/// <summary>
				/// The TreeView is in its normal state
				/// </summary>
				Normal = 1,

				/// <summary>
				/// The mouse is over the TreeView
				/// </summary>
				Hot = 2,

				/// <summary>
				/// The TreeView is selected
				/// </summary>
				Selected = 3,

				/// <summary>
				/// The TreeView is disabled
				/// </summary>
				Disabled = 4,

				/// <summary>
				/// The TreeView is selected but currently does not have focus
				/// </summary>
				SelectedNotFocused = 5
			}

			#endregion
		}

		#endregion
	}

	#endregion
}