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
using System.Runtime.InteropServices;

namespace XPExplorerBar
{
	#region NativeMethods

	/// <summary>
	/// A class that provides access to the Win32 API
	/// </summary>
	public sealed class NativeMethods
	{
		/// <summary>
		/// The SendMessage function sends the specified message to a 
		/// window or windows. It calls the window procedure for the 
		/// specified window and does not return until the window 
		/// procedure has processed the message
		/// </summary>
		/// <param name="hwnd">Handle to the window whose window procedure will 
		/// receive the message</param>
		/// <param name="msg">Specifies the message to be sent</param>
		/// <param name="wParam">Specifies additional message-specific information</param>
		/// <param name="lParam">Specifies additional message-specific information</param>
		/// <returns>The return value specifies the result of the message processing; 
		/// it depends on the message sent</returns>
		public static int SendMessage(IntPtr hwnd, WindowMessageFlags msg, IntPtr wParam, IntPtr lParam)
		{
			return InternalSendMessage(hwnd, (int)msg, wParam, lParam);
		}

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int InternalSendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);


		/// <summary>
		/// Implemented by many of the Microsoft® Windows® Shell dynamic-link libraries 
		/// (DLLs) to allow applications to obtain DLL-specific version information
		/// </summary>
		/// <param name="pdvi">Pointer to a DLLVERSIONINFO structure that receives the 
		/// version information. The cbSize member must be filled in before calling 
		/// the function</param>
		/// <returns>Returns NOERROR if successful, or an OLE-defined error value otherwise</returns>
		[DllImport("Comctl32.dll")]
		public static extern int DllGetVersion(ref DLLVERSIONINFO pdvi);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="hgdiobj"></param>
		/// <returns></returns>
		[DllImport("Gdi32.dll")]
		internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hObject"></param>
		/// <returns></returns>
		[DllImport("Gdi32.dll")]
		internal static extern bool DeleteObject(IntPtr hObject);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="lpString"></param>
		/// <param name="nCount"></param>
		/// <param name="lpRect"></param>
		/// <param name="uFormat"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern int DrawText(IntPtr hdc, string lpString, int nCount, ref RECT lpRect, DrawTextFlags uFormat);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="iBkMode"></param>
		/// <returns></returns>
		[DllImport("Gdi32.dll")]
		internal static extern int SetBkMode(IntPtr hdc, int iBkMode);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		[DllImport("Gdi32.dll")]
		internal static extern int SetTextColor(IntPtr hdc, int crColor);
	}

	#endregion



	#region Structs

	/// <summary>
	/// The POINT structure defines the x- and y- coordinates of a point
	/// </summary>
	[Serializable,
	StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// Specifies the x-coordinate of the point
		/// </summary>
		public int x;
			
		/// <summary>
		/// Specifies the y-coordinate of the point
		/// </summary>
		public int y;


		/// <summary>
		/// Creates a new RECT struct with the specified x and y coordinates
		/// </summary>
		/// <param name="x">The x-coordinate of the point</param>
		/// <param name="y">The y-coordinate of the point</param>
		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}


		/// <summary>
		/// Creates a new POINT struct from the specified Point
		/// </summary>
		/// <param name="p">The Point to create the POINT from</param>
		/// <returns>A POINT struct with the same x and y coordinates as 
		/// the specified Point</returns>
		public static POINT FromPoint(Point p)
		{
			return new POINT(p.X, p.Y);
		}


		/// <summary>
		/// Returns a Point with the same x and y coordinates as the POINT
		/// </summary>
		/// <returns>A Point with the same x and y coordinates as the POINT</returns>
		public Point ToPoint()
		{
			return new Point(x, y);
		}
	}


	/// <summary>
	/// The RECT structure defines the coordinates of the upper-left 
	/// and lower-right corners of a rectangle
	/// </summary>
	[Serializable,
	StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		/// <summary>
		/// Specifies the x-coordinate of the upper-left corner of the RECT
		/// </summary>
		public int left;
			
		/// <summary>
		/// Specifies the y-coordinate of the upper-left corner of the RECT
		/// </summary>
		public int top;
			
		/// <summary>
		/// Specifies the x-coordinate of the lower-right corner of the RECT
		/// </summary>
		public int right;
			
		/// <summary>
		/// Specifies the y-coordinate of the lower-right corner of the RECT
		/// </summary>
		public int bottom;


		/// <summary>
		/// Creates a new RECT struct with the specified location and size
		/// </summary>
		/// <param name="left">The x-coordinate of the upper-left corner of the RECT</param>
		/// <param name="top">The y-coordinate of the upper-left corner of the RECT</param>
		/// <param name="right">The x-coordinate of the lower-right corner of the RECT</param>
		/// <param name="bottom">The y-coordinate of the lower-right corner of the RECT</param>
		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}


		/// <summary>
		/// Creates a new RECT struct from the specified Rectangle
		/// </summary>
		/// <param name="rect">The Rectangle to create the RECT from</param>
		/// <returns>A RECT struct with the same location and size as 
		/// the specified Rectangle</returns>
		public static RECT FromRectangle(Rectangle rect)
		{
			return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
		}


		/// <summary>
		/// Creates a new RECT struct with the specified location and size
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the RECT</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the RECT</param>
		/// <param name="width">The width of the RECT</param>
		/// <param name="height">The height of the RECT</param>
		/// <returns>A RECT struct with the specified location and size</returns>
		public static RECT FromXYWH(int x, int y, int width, int height)
		{
			return new RECT(x, y, x + width, y + height);
		}


		/// <summary>
		/// Returns a Rectangle with the same location and size as the RECT
		/// </summary>
		/// <returns>A Rectangle with the same location and size as the RECT</returns>
		public Rectangle ToRectangle()
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}
	}


	/// <summary>
	/// Receives dynamic-link library (DLL)-specific version information. 
	/// It is used with the DllGetVersion function
	/// </summary>
	[Serializable,
	StructLayout(LayoutKind.Sequential)]
	public struct DLLVERSIONINFO
	{
		/// <summary>
		/// Size of the structure, in bytes. This member must be filled 
		/// in before calling the function
		/// </summary>
		public int cbSize;

		/// <summary>
		/// Major version of the DLL. If the DLL's version is 4.0.950, 
		/// this value will be 4
		/// </summary>
		public int dwMajorVersion;

		/// <summary>
		/// Minor version of the DLL. If the DLL's version is 4.0.950, 
		/// this value will be 0
		/// </summary>
		public int dwMinorVersion;

		/// <summary>
		/// Build number of the DLL. If the DLL's version is 4.0.950, 
		/// this value will be 950
		/// </summary>
		public int dwBuildNumber;

		/// <summary>
		/// Identifies the platform for which the DLL was built
		/// </summary>
		public int dwPlatformID;
	}

	#endregion


	#region Flags

	#region Window Messages

	/// <summary>
	/// The WindowMessageFlags enemeration contains Windows messages that the 
	/// XPExplorerBar may be interested in listening for
	/// </summary>
	public enum WindowMessageFlags
	{
		/// <summary>
		/// The WM_PRINT message is sent to a window to request that it draw 
		/// itself in the specified device context, most commonly in a printer 
		/// device context
		/// </summary>
		WM_PRINT = 791,
	}

	#endregion

	#region WmPrint

	/// <summary>
	/// The WmPrintFlags enemeration contains flags that may be sent 
	/// when a WM_PRINT or WM_PRINTCLIENT message is recieved
	/// </summary>
	public enum WmPrintFlags
	{
		/// <summary>
		/// Draws the window only if it is visible
		/// </summary>
		PRF_CHECKVISIBLE = 1,

		/// <summary>
		/// Draws the nonclient area of the window
		/// </summary>
		PRF_NONCLIENT = 2,

		/// <summary>
		/// Draws the client area of the window
		/// </summary>
		PRF_CLIENT = 4,

		/// <summary>
		/// Erases the background before drawing the window
		/// </summary>
		PRF_ERASEBKGND = 8,

		/// <summary>
		/// Draws all visible children windows
		/// </summary>
		PRF_CHILDREN = 16,

		/// <summary>
		/// Draws all owned windows
		/// </summary>
		PRF_OWNED = 32
	}

	#endregion

	#region DrawTextFlags

	/// <summary>
	/// 
	/// </summary>
	public enum DrawTextFlags
	{
		/// <summary>
		/// Justifies the text to the top of the rectangle.
		/// </summary>
		DT_TOP = 0x00000000,

		/// <summary>
		/// Aligns text to the left.
		/// </summary>
		DT_LEFT = 0x00000000,

		/// <summary>
		/// Centers text horizontally in the rectangle
		/// </summary>
		DT_CENTER = 0x00000001,

		/// <summary>
		/// Aligns text to the right
		/// </summary>
		DT_RIGHT = 0x00000002,

		/// <summary>
		/// Centers text vertically. This value is used only with the DT_SINGLELINE value
		/// </summary>
		DT_VCENTER = 0x00000004,

		/// <summary>
		/// Justifies the text to the bottom of the rectangle. This value is used 
		/// only with the DT_SINGLELINE value
		/// </summary>
		DT_BOTTOM = 0x00000008,

		/// <summary>
		/// Breaks words. Lines are automatically broken between words if a word would 
		/// extend past the edge of the rectangle specified by the lpRect parameter. A 
		/// carriage return-line feed sequence also breaks the line. If this is not 
		/// specified, output is on one line
		/// </summary>
		DT_WORDBREAK = 0x00000010,

		/// <summary>
		/// Displays text on a single line only. Carriage returns and line feeds do not 
		/// break the line
		/// </summary>
		DT_SINGLELINE = 0x00000020,

		/// <summary>
		/// Expands tab characters. The default number of characters per tab is eight. 
		/// The DT_WORD_ELLIPSIS, DT_PATH_ELLIPSIS, and DT_END_ELLIPSIS values cannot be 
		/// used with the DT_EXPANDTABS value
		/// </summary>
		DT_EXPANDTABS = 0x00000040,

		/// <summary>
		/// Sets tab stops. Bits 15–8 (high-order byte of the low-order word) of the uFormat 
		/// parameter specify the number of characters for each tab. The default number of 
		/// characters per tab is eight. The DT_CALCRECT, DT_EXTERNALLEADING, DT_INTERNAL, 
		/// DT_NOCLIP, and DT_NOPREFIX values cannot be used with the DT_TABSTOP value
		/// </summary>
		DT_TABSTOP = 0x00000080,

		/// <summary>
		/// Draws without clipping. DrawText is somewhat faster when DT_NOCLIP is used
		/// </summary>
		DT_NOCLIP = 0x00000100,

		/// <summary>
		/// Includes the font external leading in line height. Normally, external leading 
		/// is not included in the height of a line of text
		/// </summary>
		DT_EXTERNALLEADING = 0x00000200,

		/// <summary>
		/// Determines the width and height of the rectangle. If there are multiple lines 
		/// of text, DrawText uses the width of the rectangle pointed to by the lpRect 
		/// parameter and extends the base of the rectangle to bound the last line of text. 
		/// If the largest word is wider than the rectangle, the width is expanded. If the 
		/// text is less than the width of the rectangle, the width is reduced. If there is 
		/// only one line of text, DrawText modifies the right side of the rectangle so that 
		/// it bounds the last character in the line. In either case, DrawText returns the 
		/// height of the formatted text but does not draw the text
		/// </summary>
		DT_CALCRECT = 0x00000400,

		/// <summary>
		/// Turns off processing of prefix characters. Normally, DrawText interprets the 
		/// mnemonic-prefix character &amp; as a directive to underscore the character that 
		/// follows, and the mnemonic-prefix characters &amp;&amp; as a directive to print a 
		/// single &amp;. By specifying DT_NOPREFIX, this processing is turned off
		/// </summary>
		DT_NOPREFIX = 0x00000800,

		/// <summary>
		/// Uses the system font to calculate text metrics
		/// </summary>
		DT_INTERNAL = 0x00001000,

		/// <summary>
		/// Duplicates the text-displaying characteristics of a multiline edit control. 
		/// Specifically, the average character width is calculated in the same manner as 
		/// for an edit control, and the function does not display a partially visible last 
		/// line
		/// </summary>
		DT_EDITCONTROL = 0x00002000,

		/// <summary>
		/// For displayed text, replaces characters in the middle of the string with ellipses 
		/// so that the result fits in the specified rectangle. If the string contains backslash 
		/// (\) characters, DT_PATH_ELLIPSIS preserves as much as possible of the text after 
		/// the last backslash. The string is not modified unless the DT_MODIFYSTRING flag is 
		/// specified
		/// </summary>
		DT_PATH_ELLIPSIS = 0x00004000,

		/// <summary>
		/// For displayed text, if the end of a string does not fit in the rectangle, it is 
		/// truncated and ellipses are added. If a word that is not at the end of the string 
		/// goes beyond the limits of the rectangle, it is truncated without ellipses. The 
		/// string is not modified unless the DT_MODIFYSTRING flag is specified
		/// </summary>
		DT_END_ELLIPSIS = 0x00008000,

		/// <summary>
		/// Modifies the specified string to match the displayed text. This value has no effect 
		/// unless DT_END_ELLIPSIS or DT_PATH_ELLIPSIS is specified
		/// </summary>
		DT_MODIFYSTRING = 0x00010000,

		/// <summary>
		/// Layout in right-to-left reading order for bi-directional text when the font selected 
		/// into the hdc is a Hebrew or Arabic font. The default reading order for all text is 
		/// left-to-right
		/// </summary>
		DT_RTLREADING = 0x00020000,

		/// <summary>
		/// Truncates any word that does not fit in the rectangle and adds ellipses
		/// </summary>
		DT_WORD_ELLIPSIS = 0x00040000
	}

	#endregion

	#endregion
}
