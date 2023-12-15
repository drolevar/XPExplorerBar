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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace XPExplorerBar
{
	#region TaskItem
	
	/// <summary>
	/// A Label-like Control used to display text and/or an 
	/// Image in an Expando
	/// </summary>
	[ToolboxItem(true), 
	DesignerAttribute(typeof(TaskItemDesigner))]
	public class TaskItem : Button
	{
		#region Event Handlers

		/// <summary>
		/// Occurs when a value in the CustomSettings proterty changes
		/// </summary>
		public event EventHandler CustomSettingsChanged;

		#endregion
		
		
		#region Class Data

		/// <summary>
		/// System defined settings for the TaskItem
		/// </summary>
		private ExplorerBarInfo systemSettings;

		/// <summary>
		/// The Expando the TaskItem belongs to
		/// </summary>
		private Expando expando;

		/// <summary>
		/// The cached preferred width of the TaskItem
		/// </summary>
		private int preferredWidth;
		
		/// <summary>
		/// The cached preferred height of the TaskItem
		/// </summary>
		private int preferredHeight;

		/// <summary>
		/// The focus state of the TaskItem
		/// </summary>
		private FocusStates focusState;

		/// <summary>
		/// The rectangle where the TaskItems text is drawn
		/// </summary>
		private Rectangle textRect;

		/// <summary>
		/// Specifies whether the TaskItem should draw a focus rectangle 
		/// when it has focus
		/// </summary>
		private bool showFocusCues;

		/// <summary>
		/// Specifies the custom settings for the TaskItem
		/// </summary>
		private TaskItemInfo customSettings;

		/// <summary>
		/// Specifies whether the TaskItem's text should be drawn and measured 
		/// using GDI instead of GDI+
		/// </summary>
		private bool useGdiText;

		/// <summary>
		/// 
		/// </summary>
		private StringFormat stringFormat;

		/// <summary>
		/// 
		/// </summary>
		private DrawTextFlags drawTextFlags;

		#endregion	
		
		
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the TaskItem class with default settings
		/// </summary>
		public TaskItem()
		{
			// set control styles
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Selectable, true);

			TabStop = true;

			BackColor = Color.Transparent;

			// get the system theme settings
			systemSettings = ThemeManager.GetSystemExplorerBarSettings();

			customSettings = new TaskItemInfo();
			customSettings.TaskItem = this;
			customSettings.SetDefaultEmptyValues();

			// preferred size
			preferredWidth = -1;
			preferredHeight = -1;

			// unfocused item
			focusState = FocusStates.None;

			Cursor = Cursors.Hand;

			textRect = new Rectangle();
			TextAlign = ContentAlignment.TopLeft;

			showFocusCues = false;
			useGdiText = false;

			InitStringFormat();
			InitDrawTextFlags();
		}

		#endregion


		#region Properties

		#region Colors

		/// <summary>
		/// Gets the color of the TaskItem's text
		/// </summary>
		[Browsable(false)]
		public Color LinkColor
		{
			get
			{
				if (CustomSettings.LinkColor != Color.Empty)
				{
					return CustomSettings.LinkColor;
				}

				return systemSettings.TaskItem.LinkColor;
			}
		}


		/// <summary>
		/// Gets the color of the TaskItem's text when highlighted.
		/// </summary>
		[Browsable(false)]
		public Color LinkHotColor
		{
			get
			{
				if (CustomSettings.HotLinkColor != Color.Empty)
				{
					return CustomSettings.HotLinkColor;
				}

				return systemSettings.TaskItem.HotLinkColor;
			}
		}


		/// <summary>
		/// Gets the current color of the TaskItem's text
		/// </summary>
		[Browsable(false)]
		public Color FocusLinkColor
		{
			get
			{
				if (FocusState == FocusStates.Mouse)
				{
					return LinkHotColor;
				}

				return LinkColor;
			}
		}

		#endregion

		#region Expando

		/// <summary>
		/// Gets or sets the Expando the TaskItem belongs to
		/// </summary>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Expando Expando
		{
			get
			{
				return expando;
			}

			set
			{
				expando = value;

				if (value != null)
				{
					SystemSettings = expando.SystemSettings;
				}
			}
		}

		#endregion

		#region FlatStyle
	
		/// <summary>
		/// Overrides Button.FlatStyle
		/// </summary>
		public new FlatStyle FlatStyle
		{
			get
			{
				throw new NotSupportedException();
			}

			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion

		#region Focus

		/// <summary>
		/// Gets or sets a value indicating whether the TaskItem should
		/// display focus rectangles
		/// </summary>
		[Category("Appearance"),
		DefaultValue(false),
		Description("Determines whether the TaskItem should display a focus rectangle.")]
		public new bool ShowFocusCues
		{
			get
			{
				return showFocusCues;
			}

			set
			{
				if (showFocusCues != value)
				{
					showFocusCues = value;

					if (Focused)
					{
						Invalidate();
					}
				}
			}
		}

		#endregion

		#region Fonts

		/// <summary>
		/// Gets the decoration to be used on the text when the TaskItem is 
		/// in a highlighted state 
		/// </summary>
		[Browsable(false)]
		public FontStyle FontDecoration
		{
			get
			{
				if (CustomSettings.FontDecoration != FontStyle.Underline)
				{
					return CustomSettings.FontDecoration;
				}

				return systemSettings.TaskItem.FontDecoration;
			}
		}


		/// <summary>
		/// Gets or sets the font of the text displayed by the TaskItem
		/// </summary>
		public override Font Font
		{
			get
			{
				if (FocusState == FocusStates.Mouse)
				{
					return new Font(base.Font.Name, base.Font.SizeInPoints, FontDecoration);
				}
				
				return base.Font;
			}

			set
			{
				base.Font = value;
			}
		}

		#endregion

		#region Images

		/// <summary>
		/// Gets or sets the Image displayed by the TaskItem
		/// </summary>
		public new Image Image
		{
			get
			{
				return base.Image;
			}

			set
			{
				// make sure the image is 16x16
				if (value != null && (value.Width != 16 || value.Height != 16))
				{
					Bitmap bitmap = new Bitmap(value, 16, 16);

					base.Image = bitmap;
				}
				else
				{
					base.Image = value;
				}

				// invalidate the preferred size cache
				preferredWidth = -1;
				preferredHeight = -1;

				textRect.Width = 0;
				textRect.Height = 0;

				if (Expando != null)
				{
					Expando.DoLayout();
				}

				Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets the ImageList that contains the images to 
		/// display in the TaskItem
		/// </summary>
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}

			set
			{
				// make sure the images inside the ImageList are 16x16
				if (value != null && (value.ImageSize.Width != 16 || value.ImageSize.Height != 16))
				{
					// make a copy of the imagelist and resize all the images
					ImageList imageList = new ImageList();
					imageList.ColorDepth = value.ColorDepth;
					imageList.TransparentColor = value.TransparentColor;
					imageList.ImageSize = new Size(16, 16);

					foreach (Image image in value.Images)
					{
						Bitmap bitmap = new Bitmap(image, 16, 16);

						imageList.Images.Add(bitmap);
					}

					base.ImageList = imageList;
				}
				else
				{
					base.ImageList = value;
				}

				// invalidate the preferred size cache
				preferredWidth = -1;
				preferredHeight = -1;

				textRect.Width = 0;
				textRect.Height = 0;

				if (Expando != null)
				{
					Expando.DoLayout();
				}

				Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets the index value of the image displayed on the TaskItem
		/// </summary>
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}

			set
			{
				base.ImageIndex = value;

				// invalidate the preferred size cache
				preferredWidth = -1;
				preferredHeight = -1;

				textRect.Width = 0;
				textRect.Height = 0;

				if (Expando != null)
				{
					Expando.DoLayout();
				}

				Invalidate();
			}
		}

		#endregion

		#region Margins

		/// <summary>
		/// Gets the amount of space between individual TaskItems 
		/// along each side of the TaskItem
		/// </summary>
		[Browsable(false)]
        public new Margin Margin
		{
			get
			{
				if (CustomSettings.Margin != Margin.Empty)
				{
					return CustomSettings.Margin;
				}

				return systemSettings.TaskItem.Margin;
			}
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets the amount of space around the text along each 
		/// side of the TaskItem
		/// </summary>
		[Browsable(false)]
        public new Padding Padding
		{
			get
			{
				if (CustomSettings.Padding != Padding.Empty)
				{
					return CustomSettings.Padding;
				}

				return systemSettings.TaskItem.Padding;
			}
		}

		#endregion

		#region Preferred Size

		/// <summary>
		/// Gets the preferred width of the TaskItem.
		/// Assumes that the text is required to fit on a single line
		/// </summary>
		[Browsable(false)]
		public int PreferredWidth
		{
			get
			{
				//
				if (preferredWidth != -1)
				{
					return preferredWidth;
				}

				//
				if (Text.Length == 0)
				{
					preferredWidth = 0;

					return 0;
				}

				using (Graphics g = CreateGraphics())
				{
					if (UseGdiText)
					{
						preferredWidth = CalcGdiPreferredWidth(g);
					}
					else
					{
						preferredWidth = CalcGdiPlusPreferredWidth(g);
					}
				}

				return preferredWidth;
			}
		}


		/// <summary>
		/// Calculates the preferred width of the TaskItem using GDI+
		/// </summary>
		/// <param name="g">The Graphics used to measure the TaskItem</param>
		/// <returns>The preferred width of the TaskItem</returns>
		protected int CalcGdiPlusPreferredWidth(Graphics g)
		{
			SizeF size = g.MeasureString(Text, Font, new SizeF(0, 0), StringFormat);

			int width = (int) Math.Ceiling(size.Width) + 18 + Padding.Left + Padding.Right;

			return width;
		}


		/// <summary>
		/// Calculates the preferred width of the TaskItem using GDI
		/// </summary>
		/// <param name="g">The Graphics used to measure the TaskItem</param>
		/// <returns>The preferred width of the TaskItem</returns>
		protected int CalcGdiPreferredWidth(Graphics g)
		{
			IntPtr hdc = g.GetHdc();

			int width = 0;

			if (hdc != IntPtr.Zero)
			{
				IntPtr hFont = Font.ToHfont();
				IntPtr oldFont = NativeMethods.SelectObject(hdc, hFont);

				RECT rect = new RECT();

				NativeMethods.DrawText(hdc, Text, Text.Length, ref rect, DrawTextFlags.DT_CALCRECT | DrawTextFlags);

				width = rect.right - rect.left + 18 + Padding.Left + Padding.Right;

				NativeMethods.SelectObject(hdc, oldFont);
				NativeMethods.DeleteObject(hFont);
			}
			else
			{
				width = CalcGdiPlusPreferredWidth(g);
			}

			g.ReleaseHdc(hdc);

			return width;
		}

        
		/// <summary>
		/// Gets the preferred height of the TaskItem.
		/// Assumes that the text is required to fit within the
		/// current width of the TaskItem
		/// </summary>
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				//
				if (preferredHeight != -1)
				{
					return preferredHeight;
				}

				//
				if (Text.Length == 0)
				{
					return 16;
				}

				int textHeight = 0;

				using (Graphics g = CreateGraphics())
				{
					if (UseGdiText)
					{
						textHeight = CalcGdiPreferredHeight(g);
					}
					else
					{
						textHeight = CalcGdiPlusPreferredHeight(g);
					}
				}

				//
				if (textHeight > 16)
				{
					preferredHeight = textHeight;
				}
				else
				{
					preferredHeight = 16;
				}

				return preferredHeight;
			}
		}


		/// <summary>
		/// Calculates the preferred height of the TaskItem using GDI+
		/// </summary>
		/// <param name="g">The Graphics used to measure the TaskItem</param>
		/// <returns>The preferred height of the TaskItem</returns>
		protected int CalcGdiPlusPreferredHeight(Graphics g)
		{
			//
			int width = Width - Padding.Right;

			if (Image != null)
			{
				width -= 16 + Padding.Left;
			}

			//
			SizeF size = g.MeasureString(Text, Font, width, StringFormat);

			//
			int height = (int) Math.Ceiling(size.Height);

			return height;
		}


		/// <summary>
		/// Calculates the preferred height of the TaskItem using GDI
		/// </summary>
		/// <param name="g">The Graphics used to measure the TaskItem</param>
		/// <returns>The preferred height of the TaskItem</returns>
		protected int CalcGdiPreferredHeight(Graphics g)
		{
			IntPtr hdc = g.GetHdc();

			int height = 0;

			if (hdc != IntPtr.Zero)
			{
				IntPtr hFont = Font.ToHfont();
				IntPtr oldFont = NativeMethods.SelectObject(hdc, hFont);

				RECT rect = new RECT();

				int width = Width - Padding.Right;

				if (Image != null)
				{
					width -= 16 + Padding.Left;
				}

				rect.right = width;

				NativeMethods.DrawText(hdc, Text, Text.Length, ref rect, DrawTextFlags.DT_CALCRECT | DrawTextFlags);

				height = rect.bottom - rect.top;

				NativeMethods.SelectObject(hdc, oldFont);
				NativeMethods.DeleteObject(hFont);
			}
			else
			{
				height = CalcGdiPlusPreferredHeight(g);
			}

			g.ReleaseHdc(hdc);

			return height;
		}


		/// <summary>
		/// This member overrides Button.DefaultSize
		/// </summary>
		[Browsable(false)]
		protected override Size DefaultSize
		{
			get
			{
				return new Size(162, 16);
			}
		}

		#endregion

		#region State

		/// <summary>
		/// Gets or sets whether the TaskItem is in a highlighted state.
		/// </summary>
		protected FocusStates FocusState
		{
			get
			{
				return focusState;
			}

			set
			{
				if (focusState != value)
				{
					focusState = value;

					Invalidate();
				}
			}
		}

		#endregion

		#region System Settings

		/// <summary>
		/// Gets or sets System settings for the TaskItem
		/// </summary>
		[Browsable(false)]
		protected internal ExplorerBarInfo SystemSettings
		{
			get
			{
				return systemSettings;
			}
			
			set
			{
				// make sure we have a new value
				if (systemSettings != value)
				{
					SuspendLayout();
					
					// get rid of the old settings
					systemSettings = null;

					// set the new settings
					systemSettings = value;

					ResumeLayout(true);
				}
			}
		}


		/// <summary>
		/// Gets the custom settings for the TaskItem
		/// </summary>
		[Category("Appearance"),
		Description(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		TypeConverter(typeof(TaskItemInfoConverter))]
		public TaskItemInfo CustomSettings
		{
			get
			{
				return customSettings;
			}
		}


		/// <summary>
		/// Resets the custom settings to their default values
		/// </summary>
		public void ResetCustomSettings()
		{
			CustomSettings.SetDefaultEmptyValues();

			FireCustomSettingsChanged(EventArgs.Empty);
		}

		#endregion

		#region Text

		/// <summary>
		/// Gets or sets the text associated with this TaskItem
		/// </summary>
		public override string Text
		{
			get
			{
				return base.Text;
			}

			set
			{
				base.Text = value;

				// reset the preferred width and height
				preferredHeight = -1;
				preferredWidth = -1;

				if (Expando != null)
				{
					Expando.DoLayout();
				}

				Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets whether the TaskItem's text should be drawn 
		/// and measured using GDI instead of GDI+
		/// </summary>
		[Browsable(false), 
		DefaultValue(false)]
		public bool UseGdiText
		{
			get
			{
				return useGdiText;
			}

			set
			{
				if (useGdiText != value)
				{
					useGdiText = value;

					// reset the preferred width and height
					preferredHeight = -1;
					preferredWidth = -1;

					if (Expando != null)
					{
						Expando.DoLayout();
					}

					Invalidate();
				}
			}
		}


		/// <summary>
		/// Gets or sets the alignment of the text on the TaskItem
		/// </summary>
		public override ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}

			set
			{
				if (value != base.TextAlign)
				{
					InitStringFormat();
					InitDrawTextFlags();
					
					// should the text be aligned to the left/center/right
					switch (value)
					{
						case ContentAlignment.MiddleLeft:
						case ContentAlignment.TopLeft:
						case ContentAlignment.BottomLeft:	
						{
							stringFormat.Alignment = StringAlignment.Near;

							drawTextFlags &= ~DrawTextFlags.DT_CENTER;
							drawTextFlags &= ~DrawTextFlags.DT_RIGHT;
							drawTextFlags |= DrawTextFlags.DT_LEFT;

							break;
						}

						case ContentAlignment.MiddleCenter:
						case ContentAlignment.TopCenter:
						case ContentAlignment.BottomCenter:	
						{
							stringFormat.Alignment = StringAlignment.Center;

							drawTextFlags &= ~DrawTextFlags.DT_LEFT;
							drawTextFlags &= ~DrawTextFlags.DT_RIGHT;
							drawTextFlags |= DrawTextFlags.DT_CENTER;

							break;
						}

						case ContentAlignment.MiddleRight:
						case ContentAlignment.TopRight:
						case ContentAlignment.BottomRight:	
						{
							stringFormat.Alignment = StringAlignment.Far;

							drawTextFlags &= ~DrawTextFlags.DT_LEFT;
							drawTextFlags &= ~DrawTextFlags.DT_CENTER;
							drawTextFlags |= DrawTextFlags.DT_RIGHT;

							break;
						}
					}

					base.TextAlign = value;
				}
			}
		}


		/// <summary>
		/// Gets the StringFormat object used to draw the TaskItem's text
		/// </summary>
		protected StringFormat StringFormat
		{
			get
			{
				return stringFormat;
			}
		}


		/// <summary>
		/// Initializes the TaskItem's StringFormat object
		/// </summary>
		private void InitStringFormat()
		{
			if (stringFormat == null)
			{
				stringFormat = new StringFormat();
				stringFormat.LineAlignment = StringAlignment.Near;
				stringFormat.Alignment = StringAlignment.Near;
			}
		}


		/// <summary>
		/// Gets the DrawTextFlags object used to draw the TaskItem's text
		/// </summary>
		protected DrawTextFlags DrawTextFlags
		{
			get
			{
				return drawTextFlags;
			}
		}


		/// <summary>
		/// Initializes the TaskItem's DrawTextFlags object
		/// </summary>
		private void InitDrawTextFlags()
		{
			if (drawTextFlags == 0)
			{
				drawTextFlags = (DrawTextFlags.DT_LEFT | DrawTextFlags.DT_TOP | DrawTextFlags.DT_WORDBREAK);
			}
		}


		/// <summary>
		/// Gets the Rectangle that the TaskItem's text is drawn in
		/// </summary>
		protected Rectangle TextRect
		{
			get
			{
				return textRect;
			}
		}

		#endregion

		#endregion


		#region Events

		#region Custom Settings

		/// <summary>
		/// Raises the CustomSettingsChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		internal void FireCustomSettingsChanged(EventArgs e)
		{
			if (Expando != null)
			{
				Expando.DoLayout();
			}

			Invalidate();

			OnCustomSettingsChanged(e);
		}


		/// <summary>
		/// Raises the CustomSettingsChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected virtual void OnCustomSettingsChanged(EventArgs e)
		{
			if (CustomSettingsChanged != null)
			{
				CustomSettingsChanged(this, e);
			}
		}

		#endregion

		#region Focus

		/// <summary>
		/// Raises the GotFocus event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnGotFocus(EventArgs e)
		{
			// if we get focus and our expando is collapsed, give
			// it focus instead
			if (Expando != null && Expando.Collapsed)
			{
				Expando.Select();
			}
			
			base.OnGotFocus(e);
		}


		/// <summary>
		/// Raises the VisibleChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnVisibleChanged(EventArgs e)
		{
			// if we become invisible and have focus, give the 
			// focus to our expando instead
			if (!Visible && Focused && Expando != null && Expando.Collapsed)
			{
				Expando.Select();
			}
			
			base.OnVisibleChanged(e);
		}

		#endregion

		#region Mouse

		/// <summary>
		/// Raises the MouseEnter event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			FocusState = FocusStates.Mouse;
		}


		/// <summary>
		/// Raises the MouseLeave event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			FocusState = FocusStates.None;
		}

		#endregion

		#region Paint

		/// <summary>
		/// Raises the PaintBackground event
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// don't let windows paint our background as it will be black
			// (we'll paint the background in OnPaint instead)
			//base.OnPaintBackground (pevent);
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			
			//base.OnPaint(e);
			
			// do we have an image to draw
			if (Image != null)
			{
				if (Enabled)
				{
					if (RightToLeft == RightToLeft.Yes)
					{
						e.Graphics.DrawImage(Image, Width-16, 0, 16, 16);
					}
					else
					{
						e.Graphics.DrawImage(Image, 0, 0, 16, 16);
					}
				}
				else
				{
					// fix: use ControlPaint.DrawImageDisabled() to draw 
					//      the disabled image
					//      Brad Jones (brad@bradjones.com)
					//      26/08/2004
					//      v1.3

					if (RightToLeft == RightToLeft.Yes)
					{
						ControlPaint.DrawImageDisabled(e.Graphics, Image, Width-16, 0, BackColor);
					}
					else
					{
						ControlPaint.DrawImageDisabled(e.Graphics, Image, 0, 0, BackColor);
					}
				}
			}

			// do we have any text to draw
			if (Text.Length > 0)
			{
				if (textRect.Width == 0 && textRect.Height == 0)
				{
					textRect.X = 0;
					textRect.Y = 0;
					textRect.Height = PreferredHeight;
					
					if (RightToLeft == RightToLeft.Yes)
					{
						textRect.Width = Width - Padding.Right;

						if (Image != null)
						{
							textRect.Width -= 16;
						}
					}
					else
					{
						if (Image != null)
						{
							textRect.X = 16 + Padding.Left;
						}
					
						textRect.Width = Width - textRect.X - Padding.Right;
					}
				}
				
				if (RightToLeft == RightToLeft.Yes)
				{
					stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
					drawTextFlags |= DrawTextFlags.DT_RTLREADING;
				}
				else
				{
					stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
					drawTextFlags &= ~DrawTextFlags.DT_RTLREADING;
				}

				if (UseGdiText)
				{
					DrawGdiText(e.Graphics);
				}
				else
				{
					DrawText(e.Graphics);
				}
			}

			// check if windows will let us show a focus rectangle 
			// if we have focus
			if (Focused && base.ShowFocusCues)
			{
				if (ShowFocusCues)
				{
					ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		protected void DrawText(Graphics g)
		{
			if (Enabled)
			{
				using (SolidBrush brush = new SolidBrush(FocusLinkColor))
				{
					g.DrawString(Text, Font, brush, TextRect, StringFormat);
				}
			}
			else
			{
				// draw disable text the same way as a Label
				ControlPaint.DrawStringDisabled(g, Text, Font, DisabledColor, TextRect, StringFormat);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		protected void DrawGdiText(Graphics g)
		{
			IntPtr hdc = g.GetHdc();

			if (hdc != IntPtr.Zero)
			{
				IntPtr hFont = Font.ToHfont();
				IntPtr oldFont = NativeMethods.SelectObject(hdc, hFont);

				int oldBkMode = NativeMethods.SetBkMode(hdc, 1);
				
				if (Enabled)
				{
					int oldColor = NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(FocusLinkColor));

					RECT rect = RECT.FromRectangle(TextRect);
				
					NativeMethods.DrawText(hdc, Text, Text.Length, ref rect, DrawTextFlags);

					NativeMethods.SetTextColor(hdc, oldColor);
				}
				else
				{
					Rectangle layoutRectangle = TextRect;
					layoutRectangle.Offset(1, 1);

					Color color = ControlPaint.LightLight(DisabledColor);
			
					int oldColor = NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(color));
					RECT rect = RECT.FromRectangle(layoutRectangle);
					NativeMethods.DrawText(hdc, Text, Text.Length, ref rect, DrawTextFlags);

					layoutRectangle.Offset(-1, -1);
					color = ControlPaint.Dark(DisabledColor);

					NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(color));
					rect = RECT.FromRectangle(layoutRectangle);
					NativeMethods.DrawText(hdc, Text, Text.Length, ref rect, DrawTextFlags);

					NativeMethods.SetTextColor(hdc, oldColor);
				}
				
				NativeMethods.SetBkMode(hdc, oldBkMode);
				NativeMethods.SelectObject(hdc, oldFont);
				NativeMethods.DeleteObject(hFont);
			}
			else
			{
				DrawText(g);
			}

			g.ReleaseHdc(hdc);
		}


		/// <summary>
		/// Calculates the disabled color for text when the control is disabled
		/// </summary>
		internal Color DisabledColor
		{
			get
			{
				if (BackColor.A != 0)
				{
					return BackColor;
				}

				Color c = BackColor;

				for (Control control = Parent; (c.A == 0); control = control.Parent)
				{
					if (control == null)
					{
						return SystemColors.Control;
					}

					c = control.BackColor;
				}

				return c;
			}
		}

		#endregion

		#region Size

		/// <summary>
		/// Raises the SizeChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			// invalidate the preferred size cache
			preferredWidth = -1;
			preferredHeight = -1;

			textRect.Width = 0;
			textRect.Height = 0;
		}

		#endregion

		#endregion


		#region TaskItemSurrogate

		/// <summary>
		/// A class that is serialized instead of a TaskItem (as 
		/// TaskItems contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class TaskItemSurrogate : ISerializable
		{
			#region Class Data

			/// <summary>
			/// See TaskItem.Name.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string Name;

			/// <summary>
			/// See TaskItem.Size.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Size Size;
			
			/// <summary>
			/// See TaskItem.Location.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Point Location;
			
			/// <summary>
			/// See TaskItem.BackColor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string BackColor;
			
			/// <summary>
			/// See TaskItem.CustomSettings.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public TaskItemInfo.TaskItemInfoSurrogate CustomSettings;
			
			/// <summary>
			/// See TaskItem.Text.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string Text;
			
			/// <summary>
			/// See TaskItem.ShowFocusCues.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool ShowFocusCues;

			/// <summary>
			/// See TaskItem.Image.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlElementAttribute("TaskItemImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] Image;
			
			/// <summary>
			/// See TaskItem.Enabled.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Enabled;
			
			/// <summary>
			/// See TaskItem.Visible.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Visible;
			
			/// <summary>
			/// See TaskItem.Anchor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public AnchorStyles Anchor;
			
			/// <summary>
			/// See TaskItem.Dock.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public DockStyle Dock;
			
			/// <summary>
			/// See Font.Name.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string FontName;
			
			/// <summary>
			/// See Font.Size.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public float FontSize;
			
			/// <summary>
			/// See Font.Style.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public FontStyle FontDecoration;

			/// <summary>
			/// See TaskItem.UseGdiText.  This member is not intended to 
			/// be used directly from your code.
			/// </summary>
			public bool UseGdiText;

			/// <summary>
			/// See Control.Tag.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlElementAttribute("Tag", typeof(Byte[]), DataType="base64Binary")]
			public byte[] Tag;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion
			

			#region Constructor
			
			/// <summary>
			/// Initializes a new instance of the TaskItemSurrogate class with default settings
			/// </summary>
			public TaskItemSurrogate()
			{
				Name = null;

				Size = Size.Empty;
				Location = Point.Empty;

				BackColor = ThemeManager.ConvertColorToString(Color.Empty);

				CustomSettings = null;

				Text = null;
				ShowFocusCues = false;
				Image = new byte[0];

				Enabled = true;
				Visible = true;

				Anchor = AnchorStyles.None;
				Dock = DockStyle.None;

				FontName = null;
				FontSize = 8.25f;
				FontDecoration = FontStyle.Regular;
				UseGdiText = false;

				Tag = new byte[0];
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the TaskItemSurrogate with data that is to be 
			/// serialized from the specified TaskItem
			/// </summary>
			/// <param name="taskItem">The TaskItem that contains the data 
			/// to be serialized</param>
			public void Load(TaskItem taskItem)
			{
				Name = taskItem.Name;
				Size = taskItem.Size;
				Location = taskItem.Location;

				BackColor = ThemeManager.ConvertColorToString(taskItem.BackColor);

				CustomSettings = new TaskItemInfo.TaskItemInfoSurrogate();
				CustomSettings.Load(taskItem.CustomSettings);

				Text = taskItem.Text;
				ShowFocusCues = taskItem.ShowFocusCues;
				Image = ThemeManager.ConvertImageToByteArray(taskItem.Image);

				Enabled = taskItem.Enabled;
				Visible = taskItem.Visible;

				Anchor = taskItem.Anchor;
				Dock = taskItem.Dock;

				FontName = taskItem.Font.FontFamily.Name;
				FontSize = taskItem.Font.SizeInPoints;
				FontDecoration = taskItem.Font.Style;
				UseGdiText = taskItem.UseGdiText;

				Tag = ThemeManager.ConvertObjectToByteArray(taskItem.Tag);
			}


			/// <summary>
			/// Returns a TaskItem that contains the deserialized TaskItemSurrogate data
			/// </summary>
			/// <returns>A TaskItem that contains the deserialized TaskItemSurrogate data</returns>
			public TaskItem Save()
			{
				TaskItem taskItem = new TaskItem();

				taskItem.Name = Name;
				taskItem.Size = Size;
				taskItem.Location = Location;

				taskItem.BackColor = ThemeManager.ConvertStringToColor(BackColor);

				taskItem.customSettings = CustomSettings.Save();
				taskItem.customSettings.TaskItem = taskItem;

				taskItem.Text = Text;
				taskItem.ShowFocusCues = ShowFocusCues;
				taskItem.Image = ThemeManager.ConvertByteArrayToImage(Image);

				taskItem.Enabled = Enabled;
				taskItem.Visible = Visible;

				taskItem.Anchor = Anchor;
				taskItem.Dock = Dock;

				taskItem.Font = new Font(FontName, FontSize, FontDecoration);
				taskItem.UseGdiText = UseGdiText;

				taskItem.Tag = ThemeManager.ConvertByteArrayToObject(Tag);

				return taskItem;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the TaskItemSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("Name", Name);
				info.AddValue("Size", Size);
				info.AddValue("Location", Location);

				info.AddValue("BackColor", BackColor);

				info.AddValue("CustomSettings", CustomSettings);

				info.AddValue("Text", Text);
				info.AddValue("ShowFocusCues", ShowFocusCues);
				info.AddValue("Image", Image);

				info.AddValue("Enabled", Enabled);
				info.AddValue("Visible", Visible);

				info.AddValue("Anchor", Anchor);
				info.AddValue("Dock", Dock);
				
				info.AddValue("FontName", FontName);
				info.AddValue("FontSize", FontSize);
				info.AddValue("FontDecoration", FontDecoration);
				info.AddValue("UseGdiText", UseGdiText);
				
				info.AddValue("Tag", Tag);
			}


			/// <summary>
			/// Initializes a new instance of the TaskItemSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the TaskItemSurrogate</param>
			/// <param name="context">The source from which the TaskItemSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected TaskItemSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				Name = info.GetString("Name");
				Size = (Size) info.GetValue("Size", typeof(Size));
				Location = (Point) info.GetValue("Location", typeof(Point));
				
				BackColor = info.GetString("BackColor");

				CustomSettings = (TaskItemInfo.TaskItemInfoSurrogate) info.GetValue("CustomSettings", typeof(TaskItemInfo.TaskItemInfoSurrogate));

				Text = info.GetString("Text");
				ShowFocusCues = info.GetBoolean("ShowFocusCues");
				Image = (byte[]) info.GetValue("Image", typeof(byte[]));

				Enabled = info.GetBoolean("Enabled");
				Visible = info.GetBoolean("Visible");
				
				Anchor = (AnchorStyles) info.GetValue("Anchor", typeof(AnchorStyles));
				Dock = (DockStyle) info.GetValue("Dock", typeof(DockStyle));

				FontName = info.GetString("FontName");
				FontSize = info.GetSingle("FontSize");
				FontDecoration = (FontStyle) info.GetValue("FontDecoration", typeof(FontStyle));

				if (version >= 3300)
				{
					UseGdiText = info.GetBoolean("UseGdiText");
				}

				Tag = (byte[]) info.GetValue("Tag", typeof(byte[]));
			}

			#endregion
		}

		#endregion
	}

	#endregion



	#region TaskItemDesigner

	/// <summary>
	/// A custom designer used by TaskItems to remove unwanted 
	/// properties from the Property window in the designer
	/// </summary>
	internal class TaskItemDesigner : ControlDesigner
	{
		/// <summary>
		/// Initializes a new instance of the TaskItemDesigner class
		/// </summary>
		public TaskItemDesigner()
		{
			
		}


		/// <summary>
		/// Adjusts the set of properties the component exposes through 
		/// a TypeDescriptor
		/// </summary>
		/// <param name="properties">An IDictionary containing the properties 
		/// for the class of the component</param>
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			properties.Remove("BackgroundImage");
			properties.Remove("Cursor");
			properties.Remove("ForeColor");
			properties.Remove("FlatStyle");
		}
	}

	#endregion
}
