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
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace XPExplorerBar
{
	#region Delegates

	/// <summary>
	/// Represents the method that will handle the StateChanged, ExpandoAdded, 
	/// and ExpandoRemoved events of an Expando or TaskPane
	/// </summary>
	/// <param name="sender">The source of the event</param>
	/// <param name="e">A ExpandoEventArgs that contains the event data</param>
	public delegate void ExpandoEventHandler(object sender, ExpandoEventArgs e);
	
	#endregion


	
	#region Expando

	/// <summary>
	/// A Control that replicates the collapsable panels found in 
	/// Windows XP's Explorer Bar
	/// </summary>
	[ToolboxItem(true), 
	DefaultEvent("StateChanged"), 
	DesignerAttribute(typeof(ExpandoDesigner))]
	public class Expando : Control, ISupportInitialize
	{
		#region EventHandlers
		
		/// <summary>
		/// Occurs when the value of the Collapsed property changes
		/// </summary>
		public event ExpandoEventHandler StateChanged;
		
		/// <summary>
		/// Occurs when the value of the TitleImage property changes
		/// </summary>
		public event ExpandoEventHandler TitleImageChanged;
		
		/// <summary>
		/// Occurs when the value of the SpecialGroup property changes
		/// </summary>
		public event ExpandoEventHandler SpecialGroupChanged;
		
		/// <summary>
		/// Occurs when the value of the Watermark property changes
		/// </summary>
		public event ExpandoEventHandler WatermarkChanged;

		/// <summary>
		/// Occurs when an item (Control) is added to the Expando
		/// </summary>
		public event ControlEventHandler ItemAdded;

		/// <summary>
		/// Occurs when an item (Control) is removed from the Expando
		/// </summary>
		public event ControlEventHandler ItemRemoved;

		/// <summary>
		/// Occurs when a value in the CustomSettings or CustomHeaderSettings 
		/// proterties changes
		/// </summary>
		public event EventHandler CustomSettingsChanged;

		#endregion	
		
		
		#region Class Data

		/// <summary>
		/// Required designer variable
		/// </summary>
		private Container components;

		/// <summary>
		/// System settings for the Expando
		/// </summary>
		private ExplorerBarInfo systemSettings;

		/// <summary>
		/// Is the Expando a special group
		/// </summary>
		private bool specialGroup;

		/// <summary>
		/// The height of the Expando in its expanded state
		/// </summary>
		private int expandedHeight;

		/// <summary>
		/// The image displayed on the left side of the titlebar
		/// </summary>
		private Image titleImage;

		/// <summary>
		/// The height of the header section 
		/// (includes titlebar and title image)
		/// </summary>
		private int headerHeight;

		/// <summary>
		/// Is the Expando collapsed
		/// </summary>
		private bool collapsed;

		/// <summary>
		/// The state of the titlebar
		/// </summary>
		private FocusStates focusState;

		/// <summary>
		/// The height of the titlebar
		/// </summary>
		private int titleBarHeight;

		/// <summary>
		/// Specifies whether the Expando is allowed to animate
		/// </summary>
		private bool animate;

		/// <summary>
		/// Spcifies whether the Expando is currently animating a fade
		/// </summary>
		private bool animatingFade;

		/// <summary>
		/// Spcifies whether the Expando is we currently animating a slide
		/// </summary>
		private bool animatingSlide;

		/// <summary>
		/// An image of the "client area" which is used 
		/// during a fade animation
		/// </summary>
		private Image animationImage;

		/// <summary>
		/// An AnimationHelper that help the Expando to animate
		/// </summary>
		private AnimationHelper animationHelper;

		/// <summary>
		/// The TaskPane the Expando belongs to
		/// </summary>
		private TaskPane taskpane;

		/// <summary>
		/// Should the Expando layout its items itself
		/// </summary>
		private bool autoLayout;

		/// <summary>
		/// The last known width of the Expando 
		/// (used while animating)
		/// </summary>
		private int oldWidth;

		/// <summary>
		/// Specifies whether the Expando is currently initialising
		/// </summary>
		private bool initialising;

		/// <summary>
		/// Internal list of items contained in the Expando
		/// </summary>
		private ItemCollection itemList;

		/// <summary>
		/// Internal list of controls that have been hidden
		/// </summary>
		private ArrayList hiddenControls;

		/// <summary>
		/// A panel the Expando can move its controls onto when it is 
		/// animating from collapsed to expanded.
		/// </summary>
		private AnimationPanel dummyPanel;

		/// <summary>
		/// Specifies whether the Expando is allowed to collapse
		/// </summary>
		private bool canCollapse;

		/// <summary>
		/// The height of the Expando at the end of its slide animation
		/// </summary>
		private int slideEndHeight;

		/// <summary>
		/// The index of the Image that is used as a watermark
		/// </summary>
		private Image watermark;

		/// <summary>
		/// Specifies whether the Expando should draw a focus rectangle 
		/// when it has focus
		/// </summary>
		private bool showFocusCues;

		/// <summary>
		/// Specifies whether the Expando is currently performing a 
		/// layout operation
		/// </summary>
		private bool layout;

		/// <summary>
		/// Specifies the custom settings for the Expando
		/// </summary>
		private ExpandoInfo customSettings;

		/// <summary>
		/// Specifies the custom header settings for the Expando
		/// </summary>
		private HeaderInfo customHeaderSettings;

		/// <summary>
		/// An array of pre-determined heights for use during a 
		/// fade animation
		/// </summary>
		private int[] fadeHeights;

		/// <summary>
		/// Specifies whether the Expando should use Windows 
		/// defsult Tab handling mechanism
		/// </summary>
		private bool useDefaultTabHandling;

		/// <summary>
		/// Specifies the number of times BeginUpdate() has been called
		/// </summary>
		private int beginUpdateCount;

		/// <summary>
		/// Specifies whether slide animations should be batched
		/// </summary>
		private bool slideAnimationBatched;

		/// <summary>
		/// Specifies whether the Expando is currently being dragged
		/// </summary>
		private bool dragging;

		/// <summary>
		/// Specifies the Point that a drag operation started at
		/// </summary>
		private Point dragStart;

		#endregion


		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the Expando class with default settings
		/// </summary>
		public Expando()
		{
			// This call is required by the Windows.Forms Form Designer.
			components = new Container();

			// set control styles
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.Selectable, true);
			TabStop = true;

			// get the system theme settings
			systemSettings = ThemeManager.GetSystemExplorerBarSettings();

			customSettings = new ExpandoInfo();
			customSettings.Expando = this;
			customSettings.SetDefaultEmptyValues();

			customHeaderSettings = new HeaderInfo();
			customHeaderSettings.Expando = this;
			customHeaderSettings.SetDefaultEmptyValues();

			BackColor = systemSettings.Expando.NormalBackColor;

			// the height of the Expando in the expanded state
			expandedHeight = 100;

			// animation
			animate = false;
			animatingFade = false;
			animatingSlide = false;
			animationImage = null;
			slideEndHeight = -1;
			animationHelper = null;
			fadeHeights = new int[AnimationHelper.NumAnimationFrames];

			// size
			Size = new Size(systemSettings.Header.BackImageWidth, expandedHeight);
			titleBarHeight = systemSettings.Header.BackImageHeight;
			headerHeight = titleBarHeight;
			oldWidth = Width;

			// start expanded
			collapsed = false;
			
			// not a special group
			specialGroup = false;

			// unfocused titlebar
			focusState = FocusStates.None;

			// no title image
			titleImage = null;
			watermark = null;

			Font = new Font(TitleFont.Name, 8.25f, FontStyle.Regular);

			// don't get the Expando to layout its items itself
			autoLayout = false;

			// don't know which TaskPane we belong to
			taskpane = null;

			// internal list of items
			itemList = new ItemCollection(this);
			hiddenControls = new ArrayList();

			// initialise the dummyPanel
			dummyPanel = new AnimationPanel();
			dummyPanel.Size = Size;
			dummyPanel.Location = new Point(-1000, 0);

			canCollapse = true;

			showFocusCues = false;
			useDefaultTabHandling = true;

			CalcAnimationHeights();

			slideAnimationBatched = false;

			dragging = false;
			dragStart = Point.Empty;

			beginUpdateCount = 0;

			initialising = false;
			layout = false;
		}

		#endregion


		#region Methods

		#region Animation

		#region Fade Collapse/Expand

		/// <summary>
		/// Collapses the group without any animation.  
		/// </summary>
		public void Collapse()
		{
			collapsed = true;
			
			if (!Animating && Height != HeaderHeight)
			{
				Height = headerHeight;

				// fix: Raise StateChanged event
				//      Jewlin (jewlin88@hotmail.com)
				//      22/10/2004
				//      v3.0
				OnStateChanged(new ExpandoEventArgs(this));
			}
		}


		/// <summary>
		/// Expands the group without any animation.  
		/// </summary>
		public void Expand()
		{
			collapsed = false;
			
			if (!Animating && Height != ExpandedHeight)
			{
				Height = ExpandedHeight;

				// fix: Raise StateChanged event
				//      Jewlin (jewlin88@hotmail.com)
				//      22/10/2004
				//      v3.0
				OnStateChanged(new ExpandoEventArgs(this));
			}
		}


		/// <summary>
		/// Gets the Expando ready to start its collapse/expand animation
		/// </summary>
		protected void StartFadeAnimation()
		{
			//
			animatingFade = true;

			//
			SuspendLayout();

			// get an image of the client area that we can
			// use for alpha-blending in our animation
			animationImage = GetFadeAnimationImage();

			// set each control invisible (otherwise they
			// appear to slide off the bottom of the group)
			foreach (Control control in Controls)
			{
				control.Visible = false;
			}

			// restart the layout engine
			ResumeLayout(false);
		}


		/// <summary>
		/// Updates the next "frame" of the animation
		/// </summary>
		/// <param name="animationStepNum">The current step in the animation</param>
		/// <param name="numAnimationSteps">The total number of steps in the animation</param>
		protected void UpdateFadeAnimation(int animationStepNum, int numAnimationSteps)
		{
			// fix: use the precalculated heights to determine 
			//      the correct height
			//      David Nissimoff (dudi_001@yahoo.com.br)
			//      22/10/2004
			//      v3.0
			
			// set the height of the group
			if (collapsed)
			{
				Height = fadeHeights[animationStepNum-1] + headerHeight;
			}
			else
			{
				Height = (ExpandedHeight - HeaderHeight) - fadeHeights[animationStepNum-1] + HeaderHeight - 1;
			}

			if (TaskPane != null)
			{
				TaskPane.DoLayout();
			}
			else
			{
				// draw the next frame
				Invalidate();
			}
		}


		/// <summary>
		/// Gets the Expando to stop its animation
		/// </summary>
		protected void StopFadeAnimation()
		{
			//
			animatingFade = false;

			//
			SuspendLayout();

			// get rid of the image used for the animation
			animationImage.Dispose();
			animationImage = null;

			// set the final height of the group, depending on
			// whether we are collapsed or expanded
			if (collapsed)
			{
				Height = HeaderHeight;
			}
			else
			{
				Height = ExpandedHeight;
			}

			// set each control visible again
			foreach (Control control in Controls)
			{
				control.Visible = !hiddenControls.Contains(control);
			}

			//
			ResumeLayout(true);

			if (TaskPane != null)
			{
				TaskPane.DoLayout();
			}
		}


		/// <summary>
		/// Returns an image of the group's display area to be used
		/// in the fade animation
		/// </summary>
		/// <returns>The Image to use during the fade animation</returns>
		protected Image GetFadeAnimationImage()
		{
			if (Height == ExpandedHeight)
			{
				return GetExpandedImage();
			}

			return GetCollapsedImage();
		}


		/// <summary>
		/// Gets the image to be used in the animation while the 
		/// Expando is in its expanded state
		/// </summary>
		/// <returns>The Image to use during the fade animation</returns>
		protected Image GetExpandedImage()
		{
			// create a new image to draw into
			Image image = new Bitmap(Width, Height);

			// get a graphics object we can draw into
			Graphics g = Graphics.FromImage(image);
			IntPtr hDC = g.GetHdc();

			// some flags to tell the control how to draw itself
			IntPtr flags = (IntPtr) (WmPrintFlags.PRF_CLIENT | WmPrintFlags.PRF_CHILDREN | WmPrintFlags.PRF_ERASEBKGND);
			
			// tell the control to draw itself
			NativeMethods.SendMessage(Handle, WindowMessageFlags.WM_PRINT, hDC, flags);

			// clean up resources
			g.ReleaseHdc(hDC);
			g.Dispose();

			// return the completed animation image
			return image;
		}


		/// <summary>
		/// Gets the image to be used in the animation while the 
		/// Expando is in its collapsed state
		/// </summary>
		/// <returns>The Image to use during the fade animation</returns>
		protected Image GetCollapsedImage()
		{
			// this is pretty nasty.  after much experimentation, 
			// this is the least preferred way to get the image as
			// it is a pain in the backside, but it stops any 
			// flickering and it gets xp themed controls to draw 
			// their borders properly.
			// we have to do this in two stages:
			//    1) pretend we're expanded and draw our background,
			//       borders and "client area" into a bitmap
			//    2) set the bitmap as our dummyPanel's background, 
			//       move all our controls onto the dummyPanel and 
			//       get the dummyPanel to print itself

			int width = Width;
			int height = ExpandedHeight;
			
			
			// create a new image to draw that is the same
			// size we would be if we were expanded
			Image backImage = new Bitmap(width, height);

			// get a graphics object we can draw into
			Graphics g = Graphics.FromImage(backImage);

			// draw our parents background
			PaintTransparentBackground(g, new Rectangle(0, 0, width, height));

			// don't need to draw the titlebar as it is ignored 
			// when we paint with the animation image, but we do 
			// need to draw the borders and "client area"

			OnPaintTitleBarBackground(g);
			OnPaintTitleBar(g);

			// borders
			using (SolidBrush brush = new SolidBrush(BorderColor))
			{
				// top border
				g.FillRectangle(brush, 
					Border.Left, 
					HeaderHeight, 
					width - Border.Left - Border.Right, 
					Border.Top); 
				
				// left border
				g.FillRectangle(brush, 
					0, 
					HeaderHeight, 
					Border.Left, 
					height - HeaderHeight); 
				
				// right border
				g.FillRectangle(brush, 
					width - Border.Right, 
					HeaderHeight, 
					Border.Right, 
					height - HeaderHeight); 
				
				// bottom border
				g.FillRectangle(brush, 
					Border.Left, 
					height - Border.Bottom, 
					width - Border.Left - Border.Right, 
					Border.Bottom); 
			}

			// "client area"
			using (SolidBrush brush = new SolidBrush(BackColor))
			{
				g.FillRectangle(brush, 
					Border.Left, 
					HeaderHeight, 
					width - Border.Left - Border.Right,
					height - HeaderHeight - Border.Bottom - Border.Top);
			}
			
			// check if we have a background image
			if (BackImage != null)
			{
				// tile the backImage
				using (TextureBrush brush = new TextureBrush(BackImage, WrapMode.Tile))
				{
					g.FillRectangle(brush, 
						Border.Left, 
						HeaderHeight, 
						width - Border.Left - Border.Right,
						height - HeaderHeight - Border.Bottom - Border.Top);
				}
			}

			// watermark
			if (Watermark != null)
			{
				// work out a rough location of where the watermark should go
				Rectangle rect = new Rectangle(0, 0, Watermark.Width, Watermark.Height);
				rect.X = width - Border.Right - Watermark.Width;
				rect.Y = height - Border.Bottom - Watermark.Height;

				// shrink the destination rect if necesary so that we
				// can see all of the image
				
				if (rect.X < 0)
				{
					rect.X = 0;
				}

				if (rect.Width > ClientRectangle.Width)
				{
					rect.Width = ClientRectangle.Width;
				}

				if (rect.Y < DisplayRectangle.Top)
				{
					rect.Y = DisplayRectangle.Top;
				}

				if (rect.Height > DisplayRectangle.Height)
				{
					rect.Height = DisplayRectangle.Height;
				}

				// draw the watermark
				g.DrawImage(Watermark, rect);
			}

			// cleanup resources;
			g.Dispose();


			// make sure the dummyPanel is the same size as our image
			// (we don't want any tiling of the image)
			dummyPanel.Size = new Size(width, height);
			dummyPanel.HeaderHeight = HeaderHeight;
			dummyPanel.Border = Border;
			
			// set the image as the dummyPanels background
			dummyPanel.BackImage = backImage;

			// move all our controls to the dummyPanel, and then add
			// the dummyPanel to us
			while (Controls.Count > 0)
			{
				Control control = Controls[0];

				Controls.RemoveAt(0);
				dummyPanel.Controls.Add(control);

				control.Visible = !hiddenControls.Contains(control);
			}
			Controls.Add(dummyPanel);


			// create a new image for the dummyPanel to draw itself into
			Image image = new Bitmap(width, height);

			// get a graphics object we can draw into
			g = Graphics.FromImage(image);
			IntPtr hDC = g.GetHdc();

			// some flags to tell the control how to draw itself
			IntPtr flags = (IntPtr) (WmPrintFlags.PRF_CLIENT | WmPrintFlags.PRF_CHILDREN);
			
			// tell the control to draw itself
			NativeMethods.SendMessage(dummyPanel.Handle, WindowMessageFlags.WM_PRINT, hDC, flags);

			// clean up resources
			g.ReleaseHdc(hDC);
			g.Dispose();

			Controls.Remove(dummyPanel);

			// get our controls back
			while (dummyPanel.Controls.Count > 0)
			{
				Control control = dummyPanel.Controls[0];

				control.Visible = false;
				
				dummyPanel.Controls.RemoveAt(0);
				Controls.Add(control);
			}

			// dispose of the background image
			dummyPanel.BackImage = null;
			backImage.Dispose();

			return image;
		}


		// Added: CalcAnimationHeights()
		//        David Nissimoff (dudi_001@yahoo.com.br)
		//        22/10/2004
		//        v3.0
		
		/// <summary>
		/// Caches the heights that the Expando should be for each frame 
		/// of a fade animation
		/// </summary>
		internal void CalcAnimationHeights()
		{
			// Windows XP uses a Bezier curve to calculate the height of 
			// an Expando during a fade animation, so here we precalculate 
			// the height of the "client area" for each frame.
			// 
			// I can't describe what's happening better than David Nissimoff, 
			// so here's David's description of what goes on:
			//
			//   "The only thing that I've noticed is that the animation routine 
			// doesn't completely simulate the one used in Windows. After 2 days 
			// of endless tests I have finally discovered what should've been written 
			// to accurately simulate Windows XP behaviour.
			//   I first created a simple application in VB that would copy an 
			// area of the screen (set to one of the Windows' expandos) every time 
			// it changed. Having that information, analyzing every frame of the 
			// animation I could see that it would always be formed of 23 steps.
			//    Once having all of the animation, frame by frame, I could see 
			// that the expando's height obeyed to a bézier curve. For testing 
			// purposes, I have created an application that draws the bézier curve 
			// on top of the frames put side by side, and it matches 100%.
			//    The height of the expando in each step would be the vertical 
			// position of the bézier in the horizontal position(i.e. the step).
			//    A bézier should be drawn into a Graphics object, with x1 set to 
			// 0 (initial step = 0) and y1 to the initial height of the expando to 
			// be animated. The first control point (x2,y2) is defined by:
			//    x2 = (numAnimationSteps / 4) * 3
			//    y2 = (HeightVariation / 4) * 3
			// The second control point (x3,y3) is defined as follows:
			//    x3 = numAnimationSteps / 4
			//    y3 = HeightVariation / 4
			// The end point (x3,y3) would be:
			//    x4 = 22 --> 23 steps = 0 to 22
			//    y4 = FinalAnimationHeight
			// Then, to get the height of the expando on any desired step, you 
			// should call the Bitmap used to create the Graphics and look pixel by 
			// pixel in the column of the step number until you find the curve."
			//
			// I hope that helps ;)
			
			using (Bitmap bitmap = new Bitmap(fadeHeights.Length, ExpandedHeight - HeaderHeight))
			{
				// draw the bezier curve
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					g.Clear(Color.White);
					g.DrawBezier(new Pen(Color.Black),
						0,
						bitmap.Height - 1,
						bitmap.Width / 4 * 3,
						bitmap.Height / 4 * 3,
						bitmap.Width / 4,
						bitmap.Height / 4,
						bitmap.Width - 1,
						0);
				}

				// extract heights
				for (int i=0; i<bitmap.Width; i++)
				{
					int j = bitmap.Height - 1;

					for (; j>0; j--)
					{
						if (bitmap.GetPixel(i, j).R == 0)
						{
							break;
						}
					}

					fadeHeights[i] = j;
				}
			}
		}

		#endregion

		#region Slide Show/Hide

		/// <summary>
		/// Gets the Expando ready to start its show/hide animation
		/// </summary>
		protected internal void StartSlideAnimation()
		{
			animatingSlide = true;
			
			slideEndHeight = CalcHeightAndLayout();
		}


		/// <summary>
		/// Updates the next "frame" of a slide animation
		/// </summary>
		/// <param name="animationStepNum">The current step in the animation</param>
		/// <param name="numAnimationSteps">The total number of steps in the animation</param>
		protected internal void UpdateSlideAnimation(int animationStepNum, int numAnimationSteps)
		{
			// the percentage we need to adjust our height by
			// double step = (1 / (double) numAnimationSteps) * animationStepNum;
			// replacement by: Joel Holdsworth (joel@airwebreathe.org.uk)
			//                 Paolo Messina (ppescher@hotmail.com)
			//                 05/06/2004
			//                 v1.1
			double step = (1.0 - Math.Cos(Math.PI * animationStepNum / numAnimationSteps)) / 2.0;
			
			// set the height of the group
			Height = expandedHeight + (int) ((slideEndHeight - expandedHeight) * step);

			if (TaskPane != null)
			{
				TaskPane.DoLayout();
			}
			else
			{
				// draw the next frame
				Invalidate();
			}
		}


		/// <summary>
		/// Gets the Expando to stop its animation
		/// </summary>
		protected internal void StopSlideAnimation()
		{
			animatingSlide = false;

			// make sure we're the right height
			Height = slideEndHeight;
			slideEndHeight = -1;

			DoLayout();
		}

		#endregion

		#endregion

		#region Controls

		/// <summary>
		/// Hides the specified Control
		/// </summary>
		/// <param name="control">The Control to hide</param>
		public void HideControl(Control control)
		{
			HideControl(new[] {control});
		}


		/// <summary>
		/// Hides the Controls contained in the specified array
		/// </summary>
		/// <param name="controls">The array Controls to hide</param>
		public void HideControl(Control[] controls)
		{
			// don't bother if we are animating
			if (Animating || Collapsed)
			{
				return;
			}
			
			SuspendLayout();
			
			// flag to check if we actually hid any controls
			bool anyHidden = false;
			
			foreach (Control control in controls)
			{
				// hide the control if we own it and it is not already hidden
				if (Controls.Contains(control) && !hiddenControls.Contains(control))
				{
					anyHidden = true;

					control.Visible = false;
					hiddenControls.Add(control);
				}
			}

			ResumeLayout(false);

			// if we didn't hide any, get out of here
			if (!anyHidden)
			{
				return;
			}

			//
			if (beginUpdateCount > 0)
			{
				slideAnimationBatched = true;
				
				return;
			}
			
			// are we able to animate?
			if (!AutoLayout || !Animate)
			{
				// guess not
				DoLayout();
			}
			else
			{
				if (animationHelper != null)
				{
					animationHelper.Dispose();
					animationHelper = null;
				}

				animationHelper = new AnimationHelper(this, AnimationHelper.SlideAnimation);

				animationHelper.StartAnimation();
			}
		}


		/// <summary>
		/// Shows the specified Control
		/// </summary>
		/// <param name="control">The Control to show</param>
		public void ShowControl(Control control)
		{
			ShowControl(new[] {control});
		}


		/// <summary>
		/// Shows the Controls contained in the specified array
		/// </summary>
		/// <param name="controls">The array Controls to show</param>
		public void ShowControl(Control[] controls)
		{
			// don't bother if we are animating
			if (Animating || Collapsed)
			{
				return;
			}
			
			SuspendLayout();
			
			// flag to check if any controls were shown
			bool anyHidden = false;
			
			foreach (Control control in controls)
			{
				// show the control if we own it and it is not already shown
				if (Controls.Contains(control) && hiddenControls.Contains(control))
				{
					anyHidden = true;

					control.Visible = true;
					hiddenControls.Remove(control);
				}
			}

			ResumeLayout(false);

			// if we didn't show any, get out of here
			if (!anyHidden)
			{
				return;
			}

			//
			if (beginUpdateCount > 0)
			{
				slideAnimationBatched = true;
				
				return;
			}

			// are we able to animate?
			if (!AutoLayout || !Animate)
			{
				// guess not
				DoLayout();
			}
			else
			{
				if (animationHelper != null)
				{
					animationHelper.Dispose();
					animationHelper = null;
				}

				animationHelper = new AnimationHelper(this, AnimationHelper.SlideAnimation);

				animationHelper.StartAnimation();
			}
		}

		#endregion

		#region Dispose

		/// <summary> 
		/// Releases the unmanaged resources used by the Expando and 
		/// optionally releases the managed resources
		/// </summary>
		/// <param name="disposing">True to release both managed and unmanaged 
		/// resources; false to release only unmanaged resources</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				if (systemSettings != null)
				{
					systemSettings.Dispose();
					systemSettings = null;
				}

				if (animationHelper != null)
				{
					animationHelper.Dispose();
					animationHelper = null;
				}
			}

			base.Dispose(disposing);
		}

		#endregion
		
		#region Invalidation

		/// <summary>
		/// Invalidates the titlebar area
		/// </summary>
		protected void InvalidateTitleBar()
		{
			Invalidate(new Rectangle(0, 0, Width, headerHeight), false);
		}

		#endregion

		#region ISupportInitialize Members

		/// <summary>
		/// Signals the object that initialization is starting
		/// </summary>
		public void BeginInit()
		{
			initialising = true;
		}


		/// <summary>
		/// Signals the object that initialization is complete
		/// </summary>
		public void EndInit()
		{
			initialising = false;

			DoLayout();

			CalcAnimationHeights();
		}


		/// <summary>
		/// Gets whether the Expando is currently initializing
		/// </summary>
		[Browsable(false)]
		public bool Initialising
		{
			get
			{
				return initialising;
			}
		}

		#endregion

		#region Keys

		/// <summary>
		/// Processes a dialog key
		/// </summary>
		/// <param name="keyData">One of the Keys values that represents 
		/// the key to process</param>
		/// <returns>true if the key was processed by the control; 
		/// otherwise, false</returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (UseDefaultTabHandling || Parent == null || !(Parent is TaskPane))
			{
				return base.ProcessDialogKey(keyData);
			}
			
			Keys key = keyData & Keys.KeyCode;

			if (key != Keys.Tab)
			{
				switch (key)
				{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
					{
						if (ProcessArrowKey(((key == Keys.Right) ? true : (key == Keys.Down))))
						{
							return true;
						}
						
						break;
					}
				}

				return base.ProcessDialogKey(keyData);
			}
			
			if (key == Keys.Tab)
			{
				if (ProcessTabKey(((keyData & Keys.Shift) == Keys.None)))
				{
					return true;
				}
			}
			
			return base.ProcessDialogKey(keyData);
		}


		/// <summary>
		/// Selects the next available control and makes it the active control
		/// </summary>
		/// <param name="forward">true to cycle forward through the controls in 
		/// the Expando; otherwise, false</param>
		/// <returns>true if a control is selected; otherwise, false</returns>
		protected virtual bool ProcessTabKey(bool forward)
		{
			if (forward)
			{
				if ((Focused && !Collapsed) || Items.Count == 0)
				{
					return SelectNextControl(this, forward, true, true, false);
				}

				return Parent.SelectNextControl(Items[Items.Count-1], forward, true, true, false);
			}

			if (Focused || Items.Count == 0 || Collapsed)
			{
				return Parent.SelectNextControl(this, forward, true, true, false);
			}

			Select();
					
			return Focused;
		}


		/// <summary>
		/// Selects the next available control and makes it the active control
		/// </summary>
		/// <param name="forward">true to cycle forward through the controls in 
		/// the Expando; otherwise, false</param>
		/// <returns>true if a control is selected; otherwise, false</returns>
		protected virtual bool ProcessArrowKey(bool forward)
		{
			if (forward)
			{
				if (Focused && !Collapsed)
				{
					return SelectNextControl(this, forward, true, true, false);
				}

				if ((Items.Count > 0 && Items[Items.Count-1].Focused) || Collapsed)
				{
					int index = TaskPane.Expandos.IndexOf(this);
					
					if (index < TaskPane.Expandos.Count-1)
					{
						TaskPane.Expandos[index+1].Select();

						return TaskPane.Expandos[index+1].Focused;
					}

					return true;
				}
			}
			else
			{
				if (Focused)
				{
					int index = TaskPane.Expandos.IndexOf(this);
					
					if (index > 0)
					{
						return Parent.SelectNextControl(this, forward, true, true, false);
					}

					return true;
				}

				if (Items.Count > 0)
				{
					if (Items[0].Focused)
					{
						Select();
					
						return Focused;
					}

					return Parent.SelectNextControl(FindFocusedChild(), forward, true, true, false);
				}
			}

			return false;
		}


		/// <summary>
		/// Gets the control contained in the Expando that currently has focus
		/// </summary>
		/// <returns>The control contained in the Expando that currently has focus, 
		/// or null if no child controls have focus</returns>
		protected Control FindFocusedChild()
		{
			if (Controls.Count == 0)
			{
				return null;
			}

			foreach (Control control in Controls)
			{
				if (control.ContainsFocus)
				{
					return control;
				}
			}

			return null;
		}

		#endregion

		#region Layout

		/// <summary>
		/// Prevents the Expando from drawing until the EndUpdate method is called
		/// </summary>
		public void BeginUpdate()
		{
			beginUpdateCount++;
		}


		/// <summary>
		/// Resumes drawing of the Expando after drawing is suspended by the 
		/// BeginUpdate method
		/// </summary>
		public void EndUpdate()
		{
			beginUpdateCount = Math.Max(--beginUpdateCount, 0);

			if (beginUpdateCount == 0)
			{
				if (slideAnimationBatched)
				{
					slideAnimationBatched = false;

					if (Animate && AutoLayout)
					{
						if (animationHelper != null)
						{
							animationHelper.Dispose();
							animationHelper = null;
						}

						animationHelper = new AnimationHelper(this, AnimationHelper.SlideAnimation);

						animationHelper.StartAnimation();
					}
					else
					{
						DoLayout(true);
					}
				}
				else
				{
					DoLayout(true);
				}
			}
		}


		/// <summary>
		/// Forces the control to apply layout logic to child controls, 
		/// and adjusts the height of the Expando if necessary
		/// </summary>
		public void DoLayout()
		{
			DoLayout(true);
		}


		/// <summary>
		/// Forces the control to apply layout logic to child controls, 
		/// and adjusts the height of the Expando if necessary
		/// </summary>
		public virtual void DoLayout(bool performRealLayout)
		{
			if (layout)
			{
				return;
			}

			layout = true;
			
			// stop the layout engine
			SuspendLayout();

			// work out the height of the header section

			// is there an image to display on the titlebar
			if (titleImage != null)
			{
				// is the image bigger than the height of the titlebar
				if (titleImage.Height > titleBarHeight)
				{
					headerHeight = titleImage.Height;
				}
					// is the image smaller than the height of the titlebar
				else if (titleImage.Height < titleBarHeight)
				{
					headerHeight = titleBarHeight;
				}
					// is the image smaller than the current header height
				else if (titleImage.Height < headerHeight)
				{
					headerHeight = titleImage.Height;
				}
			}
			else
			{
				headerHeight = titleBarHeight;
			}

			// do we need to layout our items
			if (AutoLayout)
			{
				Control c;
				TaskItem ti;
				Point p;

				// work out how wide to make the controls, and where
				// the top of the first control should be
				int y = DisplayRectangle.Y + Padding.Top;
				int width = PseudoClientRect.Width - Padding.Left - Padding.Right;

				// for each control in our list...
				for (int i=0; i<itemList.Count; i++)
				{
					c = itemList[i];

					if (hiddenControls.Contains(c))
					{
						continue;
					}

					// set the starting point
					p = new Point(Padding.Left, y);

					// is the control a TaskItem?  if so, we may
					// need to take into account the margins
					if (c is TaskItem)
					{
						ti = (TaskItem) c;
						
						// only adjust the y co-ord if this isn't the first item 
						if (i > 0)
						{
							y += ti.Margin.Top;

							p.Y = y;
						}

						// adjust and set the width and height
						ti.Width = width;
						ti.Height = ti.PreferredHeight;
					}
					else
					{
						y += systemSettings.TaskItem.Margin.Top;

						p.Y = y;
					}					

					// set the location of the control
					c.Location = p;

					// update the next starting point.
					y += c.Height;

					// is the control a TaskItem?  if so, we may
					// need to take into account the bottom margin
					if (i < itemList.Count-1)
					{
						if (c is TaskItem)
						{
							ti = (TaskItem) c;
							
							y += ti.Margin.Bottom;
						}
						else
						{
							y += systemSettings.TaskItem.Margin.Bottom;
						}
					}
				}

				// workout where the bottom of the Expando should be
				y += Padding.Bottom + Border.Bottom;

				// adjust the ExpandedHeight if they're not the same
				if (y != ExpandedHeight)
				{
					ExpandedHeight = y;

					// if we're not collapsed then we had better change
					// our height as well
					if (!Collapsed)
					{
						Height = ExpandedHeight;

						// if we belong to a TaskPane then it needs to
						// re-layout its Expandos
						if (TaskPane != null)
						{
							TaskPane.DoLayout(true);
						}
					}
				}
			}

			if (Collapsed)
			{
				Height = HeaderHeight;
			}

			// restart the layout engine
			ResumeLayout(performRealLayout);

			layout = false;
		}


		/// <summary>
		/// Calculates the height that the Expando would be if a 
		/// call to DoLayout() were made
		/// </summary>
		/// <returns>The height that the Expando would be if a 
		/// call to DoLayout() were made</returns>
		internal int CalcHeightAndLayout()
		{
			// stop the layout engine
			SuspendLayout();

			// work out the height of the header section

			// is there an image to display on the titlebar
			if (titleImage != null)
			{
				// is the image bigger than the height of the titlebar
				if (titleImage.Height > titleBarHeight)
				{
					headerHeight = titleImage.Height;
				}
					// is the image smaller than the height of the titlebar
				else if (titleImage.Height < titleBarHeight)
				{
					headerHeight = titleBarHeight;
				}
					// is the image smaller than the current header height
				else if (titleImage.Height < headerHeight)
				{
					headerHeight = titleImage.Height;
				}
			}
			else
			{
				headerHeight = titleBarHeight;
			}

			int y = -1;

			// do we need to layout our items
			if (AutoLayout)
			{
				Control c;
				TaskItem ti;
				Point p;

				// work out how wide to make the controls, and where
				// the top of the first control should be
				y = DisplayRectangle.Y + Padding.Top;
				int width = PseudoClientRect.Width - Padding.Left - Padding.Right;

				// for each control in our list...
				for (int i=0; i<itemList.Count; i++)
				{
					c = itemList[i];

					if (hiddenControls.Contains(c))
					{
						continue;
					}

					// set the starting point
					p = new Point(Padding.Left, y);

					// is the control a TaskItem?  if so, we may
					// need to take into account the margins
					if (c is TaskItem)
					{
						ti = (TaskItem) c;
						
						// only adjust the y co-ord if this isn't the first item 
						if (i > 0)
						{
							y += ti.Margin.Top;

							p.Y = y;
						}

						// adjust and set the width and height
						ti.Width = width;
						ti.Height = ti.PreferredHeight;
					}	
					else
					{
						y += systemSettings.TaskItem.Margin.Top;

						p.Y = y;
					}				

					// set the location of the control
					c.Location = p;

					// update the next starting point.
					y += c.Height;

					// is the control a TaskItem?  if so, we may
					// need to take into account the bottom margin
					if (i < itemList.Count-1)
					{
						if (c is TaskItem)
						{
							ti = (TaskItem) c;
							
							y += ti.Margin.Bottom;
						}
						else
						{
							y += systemSettings.TaskItem.Margin.Bottom;
						}
					}
				}

				// workout where the bottom of the Expando should be
				y += Padding.Bottom + Border.Bottom;
			}

			// restart the layout engine
			ResumeLayout(true);

			return y;
		}


		/// <summary>
		/// Updates the layout of the Expandos items while in design mode, and 
		/// adds/removes itemss from the ControlCollection as necessary
		/// </summary>
		internal void UpdateItems()
		{
			if (Items.Count == Controls.Count)
			{
				// make sure the the items index in the ControlCollection 
				// are the same as in the ItemCollection (indexes in the 
				// ItemCollection may have changed due to the user moving 
				// them around in the editor)
				MatchControlCollToItemColl();				
				
				return;
			}

			// were any items added
			if (Items.Count > Controls.Count)
			{
				// add any extra items in the ItemCollection to the 
				// ControlCollection
				for (int i=0; i<Items.Count; i++)
				{
					if (!Controls.Contains(Items[i]))
					{
						OnItemAdded(new ControlEventArgs(Items[i]));
					}
				}
			}
			else
			{
				// items were removed
				int i = 0;
				Control control;

				// remove any extra items from the ControlCollection
				while (i < Controls.Count)
				{
					control = Controls[i];
					
					if (!Items.Contains(control))
					{
						OnItemRemoved(new ControlEventArgs(control));
					}
					else
					{
						i++;
					}
				}
			}

			Invalidate(true);
		}


		/// <summary>
		/// Make sure the controls index in the ControlCollection 
		/// are the same as in the ItemCollection (indexes in the 
		/// ItemCollection may have changed due to the user moving 
		/// them around in the editor or calling ItemCollection.Move())
		/// </summary>
		internal void MatchControlCollToItemColl()
		{
			SuspendLayout();
				
			for (int i=0; i<Items.Count; i++)
			{
				Controls.SetChildIndex(Items[i], i);
			}

			ResumeLayout(false);
				
			DoLayout();

			Invalidate(true);
		}


		/// <summary>
		/// Performs the work of scaling the entire control and any child controls
		/// </summary>
		/// <param name="dx">The ratio by which to scale the control horizontally</param>
		/// <param name="dy">The ratio by which to scale the control vertically</param>
		protected override void ScaleCore(float dx, float dy)
		{
			// fix: need to adjust expanded height when scaling
			//      AndrewEames (andrew@cognex.com)
			//      14/09/2005
			//      v3.3

			base.ScaleCore(dx, dy);

			expandedHeight = (int)(expandedHeight * dy);
		}

		#endregion

		#endregion


		#region Properties

		#region Alignment

		/// <summary>
		/// Gets the alignment of the text in the title bar.
		/// </summary>
		[Browsable(false)]
		public ContentAlignment TitleAlignment
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialAlignment != ContentAlignment.MiddleLeft)
					{		
						return CustomHeaderSettings.SpecialAlignment;
					}

					return SystemSettings.Header.SpecialAlignment;
				}
				
				if (CustomHeaderSettings.NormalAlignment != ContentAlignment.MiddleLeft)
				{		
					return CustomHeaderSettings.NormalAlignment;
				}

				return SystemSettings.Header.NormalAlignment;
			}
		}

		#endregion

		#region Animation

		/// <summary>
		/// Gets or sets whether the Expando is allowed to animate
		/// </summary>
		[Category("Appearance"), 
		DefaultValue(false),
		Description("Specifies whether the Expando is allowed to animate")]
		public bool Animate
		{
			get
			{
				return animate;
			}

			set
			{
				if (animate != value)
				{
					animate = value;
				}
			}
		}


		/// <summary>
		/// Gets whether the Expando is currently animating
		/// </summary>
		[Browsable(false)]
		public bool Animating
		{
			get
			{
				return (animatingFade || animatingSlide);
			}
		}


		/// <summary>
		/// Gets the Image used by the Expando while it is animating
		/// </summary>
		protected Image AnimationImage
		{
			get
			{
				return animationImage;
			}
		}


		/// <summary>
		/// Gets the height that the Expando should be at the end of its 
		/// slide animation
		/// </summary>
		protected int SlideEndHeight
		{
			get
			{
				return slideEndHeight;
			}
		}

		#endregion

		#region Border

		/// <summary>
		/// Gets the width of the border along each side of the Expando's pane.
		/// </summary>
		[Browsable(false)]
		public Border Border
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomSettings.SpecialBorder != Border.Empty)
					{
						return CustomSettings.SpecialBorder;
					}

					return SystemSettings.Expando.SpecialBorder;
				}

				if (CustomSettings.NormalBorder != Border.Empty)
				{
					return CustomSettings.NormalBorder;
				}

				return SystemSettings.Expando.NormalBorder;
			}
		}


		/// <summary>
		/// Gets the color of the border along each side of the Expando's pane.
		/// </summary>
		[Browsable(false)]
		public Color BorderColor
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomSettings.SpecialBorderColor != Color.Empty)
					{
						return CustomSettings.SpecialBorderColor;
					}

					return SystemSettings.Expando.SpecialBorderColor;
				}

				if (CustomSettings.NormalBorderColor != Color.Empty)
				{
					return CustomSettings.NormalBorderColor;
				}

				return SystemSettings.Expando.NormalBorderColor;
			}
		}


		/// <summary>
		/// Gets the width of the border along each side of the Expando's Title Bar.
		/// </summary>
		[Browsable(false)]
		public Border TitleBorder
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialBorder != Border.Empty)
					{
						return CustomHeaderSettings.SpecialBorder;
					}

					return SystemSettings.Header.SpecialBorder;
				}

				if (CustomHeaderSettings.NormalBorder != Border.Empty)
				{
					return CustomHeaderSettings.NormalBorder;
				}

				return SystemSettings.Header.NormalBorder;
			}
		}

		#endregion

		#region Color

		/// <summary>
		/// Gets the background color of the titlebar
		/// </summary>
		[Browsable(false)]
		public Color TitleBackColor
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialBackColor != Color.Empty && 
						CustomHeaderSettings.SpecialBackColor != Color.Transparent)
					{
						return CustomHeaderSettings.SpecialBackColor;
					}

					if (CustomHeaderSettings.SpecialBorderColor != Color.Empty)
					{
						return CustomHeaderSettings.SpecialBorderColor;
					}

					if (SystemSettings.Header.SpecialBackColor != Color.Transparent)
					{
						return systemSettings.Header.SpecialBackColor;
					}
					
					return SystemSettings.Header.SpecialBorderColor;
				}

				if (CustomHeaderSettings.NormalBackColor != Color.Empty && 
					CustomHeaderSettings.NormalBackColor != Color.Transparent)
				{
					return CustomHeaderSettings.NormalBackColor;
				}

				if (CustomHeaderSettings.NormalBorderColor != Color.Empty)
				{
					return CustomHeaderSettings.NormalBorderColor;
				}

				if (SystemSettings.Header.NormalBackColor != Color.Transparent)
				{
					return systemSettings.Header.NormalBackColor;
				}
					
				return SystemSettings.Header.NormalBorderColor;
			}
		}


		/// <summary>
		/// Gets whether any of the title bar's gradient colors are empty colors
		/// </summary>
		protected bool AnyCustomTitleGradientsEmpty
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialGradientStartColor == Color.Empty)
					{
						return true;
					}

					if (CustomHeaderSettings.SpecialGradientEndColor == Color.Empty)
					{
						return true;
					}
				}
				else
				{
					if (CustomHeaderSettings.NormalGradientStartColor == Color.Empty)
					{
						return true;
					}

					if (CustomHeaderSettings.NormalGradientEndColor == Color.Empty)
					{
						return true;
					}
				}

				return false;
			}
		}

		#endregion

		#region Client Rectangle

		/// <summary>
		/// Returns a fake Client Rectangle.  
		/// The rectangle takes into account the size of the titlebar 
		/// and borders (these are actually parts of the real 
		/// ClientRectangle)
		/// </summary>
		protected Rectangle PseudoClientRect
		{
			get
			{
				return new Rectangle(Border.Left, 
					HeaderHeight + Border.Top,
					Width - Border.Left - Border.Right,
					Height - HeaderHeight - Border.Top - Border.Bottom);
			}
		}


		/// <summary>
		/// Returns the height of the fake client rectangle
		/// </summary>
		protected int PseudoClientHeight
		{	
			get
			{
				return Height - HeaderHeight - Border.Top - Border.Bottom;
			}
		}

		#endregion

		#region Display Rectangle

		/// <summary>
		/// Overrides DisplayRectangle so that docked controls
		/// don't cover the titlebar or borders
		/// </summary>
		[Browsable(false)]
		public override Rectangle DisplayRectangle
		{
			get
			{
				return new Rectangle(Border.Left, 
					HeaderHeight + Border.Top,
					Width - Border.Left - Border.Right,
					ExpandedHeight - HeaderHeight - Border.Top - Border.Bottom);
			}
		}


		/// <summary>
		/// Gets a rectangle that contains the titlebar area
		/// </summary>
		protected Rectangle TitleBarRectangle
		{
			get
			{
				return new Rectangle(0,
					HeaderHeight - TitleBarHeight,
					Width,
					TitleBarHeight);
			}
		}

		#endregion

		#region Focus

		/// <summary>
		/// Gets or sets a value indicating whether the Expando should display 
		/// focus rectangles
		/// </summary>
		[Category("Appearance"),
		DefaultValue(false),
		Description("Determines whether the Expando should display a focus rectangle.")]
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
						InvalidateTitleBar();
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the Expando should use Windows 
		/// default Tab handling mechanism
		/// </summary>
		[Category("Appearance"), 
		DefaultValue(true),
		Description("Specifies whether the Expando should use Windows default Tab handling mechanism")]
		public bool UseDefaultTabHandling
		{
			get
			{
				return useDefaultTabHandling;
			}

			set
			{
				useDefaultTabHandling = value;
			}
		}

		#endregion

		#region Fonts

		/// <summary>
		/// Gets the color of the Title Bar's text.
		/// </summary>
		[Browsable(false)]
		public Color TitleForeColor
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialTitleColor != Color.Empty)
					{
						return CustomHeaderSettings.SpecialTitleColor;
					}

					return SystemSettings.Header.SpecialTitleColor;
				}

				if (CustomHeaderSettings.NormalTitleColor != Color.Empty)
				{
					return CustomHeaderSettings.NormalTitleColor;
				}

				return SystemSettings.Header.NormalTitleColor;
			}
		}


		/// <summary>
		/// Gets the color of the Title Bar's text when highlighted.
		/// </summary>
		[Browsable(false)]
		public Color TitleHotForeColor
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialTitleHotColor != Color.Empty)
					{
						return CustomHeaderSettings.SpecialTitleHotColor;
					}

					return SystemSettings.Header.SpecialTitleHotColor;
				}

				if (CustomHeaderSettings.NormalTitleHotColor != Color.Empty)
				{
					return CustomHeaderSettings.NormalTitleHotColor;
				}

				return SystemSettings.Header.NormalTitleHotColor;
			}
		}


		/// <summary>
		/// Gets the current color of the Title Bar's text, depending 
		/// on the current state of the Expando
		/// </summary>
		[Browsable(false)]
		public Color TitleColor
		{
			get
			{
				if (FocusState == FocusStates.Mouse)
				{
					return TitleHotForeColor;
				}

				return TitleForeColor;
			}
		}


		/// <summary>
		/// Gets the font used to render the Title Bar's text.
		/// </summary>
		[Browsable(false)]
		public Font TitleFont
		{
			get
			{
				if (CustomHeaderSettings.TitleFont != null)
				{
					return CustomHeaderSettings.TitleFont;
				}

				return SystemSettings.Header.TitleFont;
			}
		}

		#endregion		

		#region Images

		/// <summary>
		/// Gets the expand/collapse arrow image currently displayed 
		/// in the title bar, depending on the current state of the Expando
		/// </summary>
		[Browsable(false)]
		public Image ArrowImage
		{
			get
			{
				// fix: return null if the Expando isn't allowed to 
				//      collapse (this will stop an expand/collapse 
				//      arrow appearing on the titlebar
				//      dani kenan (dani_k@netvision.net.il)
				//      11/10/2004
				//      v2.1
				if(!CanCollapse)
				{
					return null;
				}
				
				if (SpecialGroup)
				{
					if (collapsed)
					{
						if (FocusState == FocusStates.None)
						{
							if (CustomHeaderSettings.SpecialArrowDown != null)
							{
								return CustomHeaderSettings.SpecialArrowDown;
							}

							return SystemSettings.Header.SpecialArrowDown;
						}

						if (CustomHeaderSettings.SpecialArrowDownHot != null)
						{
							return CustomHeaderSettings.SpecialArrowDownHot;
						}

						return SystemSettings.Header.SpecialArrowDownHot;
					}

					if (FocusState == FocusStates.None)
					{
						if (CustomHeaderSettings.SpecialArrowUp != null)
						{
							return CustomHeaderSettings.SpecialArrowUp;
						}

						return SystemSettings.Header.SpecialArrowUp;
					}

					if (CustomHeaderSettings.SpecialArrowUpHot != null)
					{
						return CustomHeaderSettings.SpecialArrowUpHot;
					}

					return SystemSettings.Header.SpecialArrowUpHot;
				}

				if (collapsed)
				{
					if (FocusState == FocusStates.None)
					{
						if (CustomHeaderSettings.NormalArrowDown != null)
						{
							return CustomHeaderSettings.NormalArrowDown;
						}

						return SystemSettings.Header.NormalArrowDown;
					}

					if (CustomHeaderSettings.NormalArrowDownHot != null)
					{
						return CustomHeaderSettings.NormalArrowDownHot;
					}

					return SystemSettings.Header.NormalArrowDownHot;
				}

				if (FocusState == FocusStates.None)
				{
					if (CustomHeaderSettings.NormalArrowUp != null)
					{
						return CustomHeaderSettings.NormalArrowUp;
					}

					return SystemSettings.Header.NormalArrowUp;
				}

				if (CustomHeaderSettings.NormalArrowUpHot != null)
				{
					return CustomHeaderSettings.NormalArrowUpHot;
				}

				return SystemSettings.Header.NormalArrowUpHot;
			}
		}


		/// <summary>
		/// Gets the width of the expand/collapse arrow image 
		/// currently displayed in the title bar
		/// </summary>
		protected int ArrowImageWidth
		{
			get
			{
				if (ArrowImage == null)
				{
					return 0;
				}

				return ArrowImage.Width;
			}
		}


		/// <summary>
		/// Gets the height of the expand/collapse arrow image 
		/// currently displayed in the title bar
		/// </summary>
		protected int ArrowImageHeight
		{
			get
			{
				if (ArrowImage == null)
				{
					return 0;
				}
			
				return ArrowImage.Height;
			}
		}


		/// <summary>
		/// The background image used for the Title Bar.
		/// </summary>
		[Browsable(false)]
		public Image TitleBackImage
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialBackImage != null)
					{
						return CustomHeaderSettings.SpecialBackImage;
					}

					return SystemSettings.Header.SpecialBackImage;
				}

				if (CustomHeaderSettings.NormalBackImage != null)
				{
					return CustomHeaderSettings.NormalBackImage;
				}

				return SystemSettings.Header.NormalBackImage;
			}
		}


		/// <summary>
		/// Gets the height of the background image used for the Title Bar.
		/// </summary>
		protected int TitleBackImageHeight
		{
			get
			{
				return SystemSettings.Header.BackImageHeight;
			}
		}


		/// <summary>
		/// The image used on the left side of the Title Bar.
		/// </summary>
		[Category("Appearance"),
		DefaultValue(null),
		Description("The image used on the left side of the Title Bar.")]
		public Image TitleImage
		{
			get
			{
				return titleImage;
			}

			set
			{
				titleImage = value;

				DoLayout();

				InvalidateTitleBar();

				OnTitleImageChanged(new ExpandoEventArgs(this));
			}
		}


		/// <summary>
		/// The width of the image used on the left side of the Title Bar.
		/// </summary>
		protected int TitleImageWidth
		{
			get
			{
				if (TitleImage == null)
				{
					return 0;
				}
	
				return TitleImage.Width;
			}
		}


		/// <summary>
		/// The height of the image used on the left side of the Title Bar.
		/// </summary>
		protected int TitleImageHeight
		{
			get
			{
				if (TitleImage == null)
				{
					return 0;
				}
			
				return TitleImage.Height;
			}
		}


		/// <summary>
		/// Gets the Image that is used as a watermark in the Expando's 
		/// client area
		/// </summary>
		[Category("Appearance"),
		DefaultValue(null),
		Description("The Image used as a watermark in the client area of the Expando.")]
		public Image Watermark
		{
			get
			{
				return watermark;
			}

			set
			{
				if (watermark != value)
				{
					watermark = value;

					Invalidate();

					OnWatermarkChanged(new ExpandoEventArgs(this));
				}
			}
		}


		/// <summary>
		/// The background image used for the Expandos content area.
		/// </summary>
		[Browsable(false)]
		public Image BackImage
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomSettings.SpecialBackImage != null)
					{
						return CustomSettings.SpecialBackImage;
					}

					return SystemSettings.Expando.SpecialBackImage;
				}

				if (CustomSettings.NormalBackImage != null)
				{
					return CustomSettings.NormalBackImage;
				}

				return SystemSettings.Expando.NormalBackImage;
			}
		}

		#endregion

		#region Items

		/// <summary>
		/// An Expando.ItemCollection representing the collection of 
		/// Controls contained within the Expando
		/// </summary>
		[Category("Behavior"),
		DefaultValue(null), 
		Description("The Controls contained in the Expando"), 
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content), 
		Editor(typeof(ItemCollectionEditor), typeof(UITypeEditor))]
		public ItemCollection Items
		{
			get
			{
				return itemList;
			}
		}


		/// <summary>
		/// A Control.ControlCollection representing the collection of 
		/// controls contained within the control
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		#endregion

		#region Layout

		/// <summary>
		/// Gets or sets whether the Expando will automagically layout its items
		/// </summary>
		[Bindable(true),
		Category("Layout"),
		DefaultValue(false),
		Description("The AutoLayout property determines whether the Expando will automagically layout its items.")]
		public bool AutoLayout
		{
			get
			{
				return autoLayout;
			}

			set
			{
				autoLayout = value;

				if (autoLayout)
				{
					DoLayout();
				}
			}
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets the amount of space between the border and items along 
		/// each side of the Expando.
		/// </summary>
		[Browsable(false)]
        public new Padding Padding
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomSettings.SpecialPadding != Padding.Empty)
					{
						return CustomSettings.SpecialPadding;
					}

					return SystemSettings.Expando.SpecialPadding;
				}

				if (CustomSettings.NormalPadding != Padding.Empty)
				{
					return CustomSettings.NormalPadding;
				}

				return SystemSettings.Expando.NormalPadding;
			}
		}


		/// <summary>
		/// Gets the amount of space between the border and items along 
		/// each side of the Title Bar.
		/// </summary>
		[Browsable(false)]
		public Padding TitlePadding
		{
			get
			{
				if (SpecialGroup)
				{
					if (CustomHeaderSettings.SpecialPadding != Padding.Empty)
					{
						return CustomHeaderSettings.SpecialPadding;
					}

					return SystemSettings.Header.SpecialPadding;
				}

				if (CustomHeaderSettings.NormalPadding != Padding.Empty)
				{
					return CustomHeaderSettings.NormalPadding;
				}

				return SystemSettings.Header.NormalPadding;
			}
		}

		#endregion

		#region Size

		/// <summary>
		/// Gets or sets the height and width of the control
		/// </summary>
		public new Size Size
		{
			get
			{
				return base.Size;
			}

			set
			{
				if (!Size.Equals(value))
				{
					if (!Animating)
					{
						Width = value.Width;

						if (!Initialising)
						{
							ExpandedHeight = value.Height;
						}
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the Size property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the Size property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSize()
		{
			return TaskPane != null;
		}

		
		/// <summary>
		/// Gets the height of the Expando in its expanded state
		/// </summary>
		[Bindable(true),
		Category("Layout"),
		DefaultValue(100),
		Description("The height of the Expando in its expanded state.")]
		public int ExpandedHeight
		{
			get
			{
				return expandedHeight;
			}

			set
			{
				expandedHeight = value;

				CalcAnimationHeights();
						
				if (!Collapsed && !Animating)
				{
					Height = expandedHeight;

					if (TaskPane != null)
					{
						TaskPane.DoLayout();
					}
				}
			}
		}


		/// <summary>
		/// Gets the height of the header section of the Expando
		/// </summary>
		protected int HeaderHeight
		{
			get
			{
				return headerHeight;
			}
		}


		/// <summary>
		/// Gets the height of the titlebar
		/// </summary>
		protected int TitleBarHeight
		{
			get
			{
				return titleBarHeight;
			}
		}

		#endregion

		#region Special Groups

		/// <summary>
		/// Gets or sets whether the Expando should be rendered as a Special Group.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"),
		DefaultValue(false),
		Description("The SpecialGroup property determines whether the Expando will be rendered as a SpecialGroup.")]
		public bool SpecialGroup
		{
			get
			{
				return specialGroup;
			}

			set
			{
				specialGroup = value;

				DoLayout();

				if (specialGroup)
				{
					if (CustomSettings.SpecialBackColor != Color.Empty)
					{
						BackColor = CustomSettings.SpecialBackColor;
					}
					else
					{
						BackColor = SystemSettings.Expando.SpecialBackColor;
					}
				}
				else
				{
					if (CustomSettings.NormalBackColor != Color.Empty)
					{
						BackColor = CustomSettings.NormalBackColor;
					}
					else
					{
						BackColor = SystemSettings.Expando.NormalBackColor;
					}
				}
				
				Invalidate();

				OnSpecialGroupChanged(new ExpandoEventArgs(this));
			}
		}

		#endregion

		#region State

		/// <summary>
		/// Gets or sets whether the Expando is collapsed.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"),
		DefaultValue(false),
		Description("The Collapsed property determines whether the Expando is collapsed.")]
		public bool Collapsed
		{
			get
			{
				return collapsed;
			}

			set
			{
				if (collapsed != value)
				{
					// if we're supposed to collapse, check if we can
					if (value && !CanCollapse)
					{
						// looks like we can't so time to bail
						return;
					}
					
					collapsed = value;

					// only animate if we're allowed to, we're not in 
					// design mode and we're not initialising
					if (Animate && !DesignMode && !Initialising)
					{
						if (animationHelper != null)
						{
							animationHelper.Dispose();
							animationHelper = null;
						}
							
						animationHelper = new AnimationHelper(this, AnimationHelper.FadeAnimation);

						OnStateChanged(new ExpandoEventArgs(this));

						animationHelper.StartAnimation();
					}
					else
					{
						if (collapsed)
						{
							Collapse();
						}
						else
						{
							Expand();
						}

						// don't need to raise OnStateChanged as 
						// Collapse() or Expand() will do it for us
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the title bar is in a highlighted state.
		/// </summary>
		[Browsable(false)]
		protected internal FocusStates FocusState
		{
			get
			{
				return focusState;
			}

			set
			{
				// fix: if the Expando isn't allowed to collapse, 
				//      don't update the titlebar highlight
				//      dani kenan (dani_k@netvision.net.il)
				//      11/10/2004
				//      v2.1
				if (!CanCollapse)
				{
					value = FocusStates.None;
				}
				
				if (focusState != value)
				{
					focusState = value;

					InvalidateTitleBar();

					if (focusState == FocusStates.Mouse)
					{
						Cursor = Cursors.Hand;
					}
					else
					{
						Cursor = Cursors.Default;
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the Expando is able to collapse
		/// </summary>
		[Bindable(true), 
		Category("Behavior"),
		DefaultValue(true),
		Description("The CanCollapse property determines whether the Expando is able to collapse.")]
		public bool CanCollapse
		{
			get
			{ 
				return canCollapse; 
			}
			
			set
			{ 
				if (canCollapse != value)
				{
					canCollapse = value; 

					// if the Expando is collapsed and it's not allowed 
					// to collapse, then we had better expand it
					if (!canCollapse && Collapsed)
					{
						Collapsed = false;
					}

					InvalidateTitleBar();
				}
			}
		}

		#endregion

		#region System Settings

		/// <summary>
		/// Gets or sets the system settings for the Expando
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
					if (systemSettings != null)
					{
						systemSettings.Dispose();
						systemSettings = null;
					}

					// set the new settings
					systemSettings = value;

					titleBarHeight = systemSettings.Header.BackImageHeight;

					// is there an image to display on the titlebar
					if (titleImage != null)
					{
						// is the image bigger than the height of the titlebar
						if (titleImage.Height > titleBarHeight)
						{
							headerHeight = titleImage.Height;
						}
							// is the image smaller than the height of the titlebar
						else if (titleImage.Height < titleBarHeight)
						{
							headerHeight = titleBarHeight;
						}
							// is the image smaller than the current header height
						else if (titleImage.Height < headerHeight)
						{
							headerHeight = titleImage.Height;
						}
					}
					else
					{
						headerHeight = titleBarHeight;
					}

					if (SpecialGroup)
					{
						if (CustomSettings.SpecialBackColor != Color.Empty)
						{
							BackColor = CustomSettings.SpecialBackColor;
						}
						else
						{
							BackColor = SystemSettings.Expando.SpecialBackColor;
						}
					}
					else
					{
						if (CustomSettings.NormalBackColor != Color.Empty)
						{
							BackColor = CustomSettings.NormalBackColor;
						}
						else
						{
							BackColor = SystemSettings.Expando.NormalBackColor;
						}
					}

					// update the system settings for each TaskItem
					for (int i=0; i<itemList.Count; i++)
					{
						Control control = itemList[i];

						if (control is TaskItem)
						{
							((TaskItem) control).SystemSettings = systemSettings;
						}
					}

					ResumeLayout(false);

					// if our parent is not an TaskPane then re-layout the 
					// Expando (don't need to do this if our parent is a 
					// TaskPane as it will tell us when to do it)
					if (TaskPane == null)
					{
						DoLayout();
					}
				}
			}
		}


		/// <summary>
		/// Gets the custom settings for the Expando
		/// </summary>
		[Category("Appearance"),
		Description(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		TypeConverter(typeof(ExpandoInfoConverter))]
		public ExpandoInfo CustomSettings
		{
			get
			{
				return customSettings;
			}
		}


		/// <summary>
		/// Gets the custom header settings for the Expando
		/// </summary>
		[Category("Appearance"),
		Description(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		TypeConverter(typeof(HeaderInfoConverter))]
		public HeaderInfo CustomHeaderSettings
		{
			get
			{
				return customHeaderSettings;
			}
		}


		/// <summary>
		/// Resets the custom settings to their default values
		/// </summary>
		public void ResetCustomSettings()
		{
			CustomSettings.SetDefaultEmptyValues();
			CustomHeaderSettings.SetDefaultEmptyValues();

			FireCustomSettingsChanged(EventArgs.Empty);
		}

		#endregion
	
		#region TaskPane

		/// <summary>
		/// Gets or sets the TaskPane the Expando belongs to
		/// </summary>
		protected internal TaskPane TaskPane
		{
			get
			{
				return taskpane;
			}

			set
			{
				taskpane = value;

				if (value != null)
				{
					SystemSettings = TaskPane.SystemSettings;
				}
			}
		}

		#endregion

		#region Text

		/// <summary>
		/// Gets or sets the text displayed on the titlebar
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

				InvalidateTitleBar();
			}
		}

		#endregion

		#region Visible

		/// <summary>
		/// Gets or sets a value indicating whether the Expando is displayed
		/// </summary>
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}

			set
			{
				// fix: TaskPane will now perform a layout if the 
				//      Expando is to become invisible and the TaskPane 
				//      is currently invisible
				//      Brian Nottingham (nottinbe@slu.edu)
				//      22/12/2004
				//      v3.0
				//if (base.Visible != value)
				if (base.Visible != value || (!value && Parent != null && !Parent.Visible))
				{
					base.Visible = value;

					if (TaskPane != null)
					{
						TaskPane.DoLayout();
					}
				}
			}
		}
	
		#endregion

		#endregion


		#region Events

		#region Controls

		/// <summary>
		/// Raises the ControlAdded event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// don't do anything if we are animating
			// (as we're probably the ones who added the control)
			if (Animating)
			{
				return;
			}
			
			base.OnControlAdded(e);
			
			// add the control to the ItemCollection if necessary
			if (!Items.Contains(e.Control))
			{
				Items.Add(e.Control);
			}
		}


		/// <summary>
		/// Raises the ControlRemoved event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			// don't do anything if we are animating 
			// (as we're probably the ones who removed the control)
			if (Animating)
			{
				return;
			}
			
			base.OnControlRemoved(e);

			// remove the control from the itemList
			if (Items.Contains(e.Control))
			{
				Items.Remove(e.Control);
			}

			// update the layout of the controls
			DoLayout();
		}

		#endregion

		#region Custom Settings

		/// <summary>
		/// Raises the CustomSettingsChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		internal void FireCustomSettingsChanged(EventArgs e)
		{
			titleBarHeight = TitleBackImageHeight;

			// is there an image to display on the titlebar
			if (titleImage != null)
			{
				// is the image bigger than the height of the titlebar
				if (titleImage.Height > titleBarHeight)
				{
					headerHeight = titleImage.Height;
				}
					// is the image smaller than the height of the titlebar
				else if (titleImage.Height < titleBarHeight)
				{
					headerHeight = titleBarHeight;
				}
					// is the image smaller than the current header height
				else if (titleImage.Height < headerHeight)
				{
					headerHeight = titleImage.Height;
				}
			}
			else
			{
				headerHeight = titleBarHeight;
			}

			if (SpecialGroup)
			{
				if (CustomSettings.SpecialBackColor != Color.Empty)
				{
					BackColor = CustomSettings.SpecialBackColor;
				}
				else
				{
					BackColor = SystemSettings.Expando.SpecialBackColor;
				}
			}
			else
			{
				if (CustomSettings.NormalBackColor != Color.Empty)
				{
					BackColor = CustomSettings.NormalBackColor;
				}
				else
				{
					BackColor = SystemSettings.Expando.NormalBackColor;
				}
			}

			DoLayout();

			Invalidate(true);

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

		#region Expando

		/// <summary>
		/// Raises the StateChanged event
		/// </summary>
		/// <param name="e">An ExpandoStateChangedEventArgs that contains the event data</param>
		protected virtual void OnStateChanged(ExpandoEventArgs e)
		{
			if (StateChanged != null)
			{
				StateChanged(this, e);
			}
		}


		/// <summary>
		/// Raises the TitleImageChanged event
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnTitleImageChanged(ExpandoEventArgs e)
		{
			if (TitleImageChanged != null)
			{
				TitleImageChanged(this, e);
			}
		}


		/// <summary>
		/// Raises the SpecialGroupChanged event
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnSpecialGroupChanged(ExpandoEventArgs e)
		{
			if (SpecialGroupChanged != null)
			{
				SpecialGroupChanged(this, e);
			}
		}


		/// <summary>
		/// Raises the WatermarkChanged event
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnWatermarkChanged(ExpandoEventArgs e)
		{
			if (WatermarkChanged != null)
			{
				WatermarkChanged(this, e);
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
			base.OnGotFocus(e);

			InvalidateTitleBar();
		}


		/// <summary>
		/// Raises the LostFocus event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			InvalidateTitleBar();
		}

		#endregion

		#region Items

		/// <summary>
		/// Raises the ItemAdded event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected virtual void OnItemAdded(ControlEventArgs e)
		{
			// add the expando to the ControlCollection if it hasn't already
			if (!Controls.Contains(e.Control))
			{
				Controls.Add(e.Control);
			}

			// check if the control is a TaskItem
			if (e.Control is TaskItem)
			{
				TaskItem item = (TaskItem) e.Control;
			
				// set anchor styles
				item.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
			
				// tell the TaskItem who's its daddy...
				item.Expando = this;
				item.SystemSettings = systemSettings;
			}

			// update the layout of the controls
			DoLayout();

			//
			if (ItemAdded != null)
			{
				ItemAdded(this, e);
			}
		}


		/// <summary>
		/// Raises the ItemRemoved event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected virtual void OnItemRemoved(ControlEventArgs e)
		{
			// remove the control from the ControlCollection if it hasn't already
			if (Controls.Contains(e.Control))
			{
				Controls.Remove(e.Control);
			}

			// update the layout of the controls
			DoLayout();

			//
			if (ItemRemoved != null)
			{
				ItemRemoved(this, e);
			}
		}

		#endregion

		#region Keys

		/// <summary>
		/// Raises the KeyUp event
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data</param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			// fix: should call OnKeyUp instead of OnKeyDown
			//      Simon Cropp (simonc@avanade.com)
			//      14/09/2005
			//      v3.3
			base.OnKeyUp(e);

			if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
			{
				Collapsed = !Collapsed;
			}
		}

		#endregion

		#region Location

		/// <summary>
		/// Raises the LocationChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);

			// sometimes the title image gets cropped (why???) if the 
			// expando is scrolled from off-screen to on-screen so we'll 
			// repaint the titlebar if the expando has a titlebar image 
			// and it is taller then the titlebar
			if (TitleImage != null && TitleImageHeight > TitleBarHeight)
			{
				InvalidateTitleBar();
			}
		}

		#endregion

		#region Mouse

		/// <summary>
		/// Raises the MouseUp event
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			// was it the left mouse button
			if (e.Button == MouseButtons.Left)
			{
				if (dragging)
				{
					Cursor = Cursors.Default;

					dragging = false;

					TaskPane.DropExpando(this);
				}
				else
				{
					// was it in the titlebar area
					if (e.Y < HeaderHeight && e.Y > (HeaderHeight - TitleBarHeight))
					{
						// make sure that our taskPane (if we have one) is not animating
						if (!Animating)
						{
							// collapse/expand the group
							Collapsed = !Collapsed;
						}

						if (CanCollapse)
						{
							Select();
						}
					}
				}

				dragStart = Point.Empty;
			}
		}


		/// <summary>
		/// Raises the MouseDown event
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			// we're not doing anything here yet...
			// but we might later :)

			if (e.Button == MouseButtons.Left)
			{
				if (TaskPane != null && TaskPane.AllowExpandoDragging && !Animating)
				{
					dragStart = PointToScreen(new Point(e.X, e.Y));
				}
			}
		}


		/// <summary>
		/// Raises the MouseMove event
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.Button == MouseButtons.Left && dragStart != Point.Empty)
			{
				Point p = PointToScreen(new Point(e.X, e.Y));

				if (!dragging)
				{
					if (Math.Abs(dragStart.X - p.X) > 8 || Math.Abs(dragStart.Y - p.Y) > 8)
					{
						dragging = true;

						FocusState = FocusStates.None;
					}
				}

				if (dragging)
				{
					if (TaskPane.ClientRectangle.Contains(TaskPane.PointToClient(p)))
					{
						Cursor = Cursors.Default;
					}
					else
					{
						Cursor = Cursors.No;
					}

					TaskPane.UpdateDropPoint(p);
					
					return;
				}
			}

			// check if the mouse is moving in the titlebar area
			if (e.Y < HeaderHeight && e.Y > (HeaderHeight - TitleBarHeight))
			{
				// change the cursor to a hand and highlight the titlebar
				FocusState = FocusStates.Mouse;
			}
			else
			{
				// reset the titlebar highlight and cursor if they haven't already
				FocusState = FocusStates.None;
			}
		}


		/// <summary>
		/// Raises the MouseLeave event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			// reset the titlebar highlight if it hasn't already
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
			// we may have a solid background color, but the titlebar back image
			// might have treansparent bits, so instead we draw our own 
			// transparent background (rather than getting windows to draw 
			// a solid background)
			PaintTransparentBackground(e.Graphics, e.ClipRectangle);

			// paint the titlebar background
			if (TitleBarRectangle.IntersectsWith(e.ClipRectangle))
			{
				OnPaintTitleBarBackground(e.Graphics);
			}

			// only paint the border and "display rect" if we are not collapsed
			if (Height != headerHeight)
			{
				if (PseudoClientRect.IntersectsWith(e.ClipRectangle))
				{
					OnPaintBorder(e.Graphics);

					OnPaintDisplayRect(e.Graphics);
				}
			}
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// paint the titlebar
			if (TitleBarRectangle.IntersectsWith(e.ClipRectangle))
			{
				OnPaintTitleBar(e.Graphics);
			}
		}


		#region TitleBar

		/// <summary>
		/// Paints the title bar background
		/// </summary>
		/// <param name="g">The Graphics used to paint the titlebar</param>
		protected void OnPaintTitleBarBackground(Graphics g)
		{
			// fix: draw grayscale titlebar when disabled
			//      Brad Jones (brad@bradjones.com)
			//      20/08/2004
			//      v1.21
			
			int y = 0;
			
			// work out where the top of the titleBar actually is
			if (HeaderHeight > TitleBarHeight)
			{
				y = HeaderHeight - TitleBarHeight;
			}

			if (CustomHeaderSettings.TitleGradient && !AnyCustomTitleGradientsEmpty)
			{
				// gradient titlebar
				Color start = CustomHeaderSettings.NormalGradientStartColor;
				if (SpecialGroup)
				{
					start = CustomHeaderSettings.SpecialGradientStartColor;
				}

				Color end = CustomHeaderSettings.NormalGradientEndColor;
				if (SpecialGroup)
				{
					end = CustomHeaderSettings.SpecialGradientEndColor;
				}

				if (!Enabled)
				{
					// simulate saturation of 0

					start = Color.FromArgb((int) (start.GetBrightness() * 255), 
						(int) (start.GetBrightness() * 255), 
						(int) (start.GetBrightness() * 255));
					end = Color.FromArgb((int) (end.GetBrightness() * 255), 
						(int) (end.GetBrightness() * 255), 
						(int) (end.GetBrightness() * 255));
				}

				using (LinearGradientBrush brush = new LinearGradientBrush(TitleBarRectangle, start, end, LinearGradientMode.Horizontal))
				{
					// work out where the gradient starts
					if (CustomHeaderSettings.GradientOffset > 0f && CustomHeaderSettings.GradientOffset < 1f)
					{
						ColorBlend colorBlend = new ColorBlend() ;
						colorBlend.Colors = new[] {brush.LinearColors[0], brush.LinearColors[0], brush.LinearColors[1]} ;
						colorBlend.Positions = new[] {0f, CustomHeaderSettings.GradientOffset, 1f} ;
						brush.InterpolationColors = colorBlend ;
					}
						
					// check if we need round corners
					if (CustomHeaderSettings.TitleRadius > 0)
					{
						GraphicsPath path = new GraphicsPath();
							
						// top
						path.AddLine(TitleBarRectangle.Left + CustomHeaderSettings.TitleRadius, 
							TitleBarRectangle.Top, 
							TitleBarRectangle.Right - (CustomHeaderSettings.TitleRadius * 2) - 1, 
							TitleBarRectangle.Top);
							
						// right corner
						path.AddArc(TitleBarRectangle.Right - (CustomHeaderSettings.TitleRadius * 2) - 1, 
							TitleBarRectangle.Top, 
							CustomHeaderSettings.TitleRadius * 2, 
							CustomHeaderSettings.TitleRadius * 2, 
							270, 
							90);
							
						// right
						path.AddLine(TitleBarRectangle.Right, 
							TitleBarRectangle.Top + CustomHeaderSettings.TitleRadius, 
							TitleBarRectangle.Right, 
							TitleBarRectangle.Bottom);
							
						// bottom
						path.AddLine(TitleBarRectangle.Right, 
							TitleBarRectangle.Bottom, 
							TitleBarRectangle.Left - 1, 
							TitleBarRectangle.Bottom);
							
						// left corner
						path.AddArc(TitleBarRectangle.Left, 
							TitleBarRectangle.Top, 
							CustomHeaderSettings.TitleRadius * 2, 
							CustomHeaderSettings.TitleRadius * 2, 
							180, 
							90);
							
						g.SmoothingMode = SmoothingMode.AntiAlias;

						g.FillPath(brush, path);

						g.SmoothingMode = SmoothingMode.Default;
					}
					else
					{
						g.FillRectangle(brush, TitleBarRectangle);
					}
				}
			}
			else if (TitleBackImage != null)
			{
				// check if the system header background images have different 
				// RightToLeft values compared to what we do.  if they are different, 
				// then we had better mirror them
				if ((RightToLeft == RightToLeft.Yes && !SystemSettings.Header.RightToLeft) || 
					(RightToLeft == RightToLeft.No && SystemSettings.Header.RightToLeft))
				{
					if (SystemSettings.Header.NormalBackImage != null)
					{
						SystemSettings.Header.NormalBackImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
					}

					if (SystemSettings.Header.SpecialBackImage != null)
					{
						SystemSettings.Header.SpecialBackImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
					}

					SystemSettings.Header.RightToLeft = (RightToLeft == RightToLeft.Yes);
				}
					
				if (Enabled)
				{
					if (SystemSettings.OfficialTheme)
					{
						// left edge
						g.DrawImage(TitleBackImage, 
							new Rectangle(0, y, 5, TitleBarHeight),
							new Rectangle(0, 0, 5, TitleBackImage.Height), 
							GraphicsUnit.Pixel);

						// right edge
						g.DrawImage(TitleBackImage, 
							new Rectangle(Width-5, y, 5, TitleBarHeight),
							new Rectangle(TitleBackImage.Width-5, 0, 5, TitleBackImage.Height), 
							GraphicsUnit.Pixel);

						// middle
						g.DrawImage(TitleBackImage, 
							new Rectangle(5, y, Width-10, TitleBarHeight),
							new Rectangle(5, 0, TitleBackImage.Width-10, TitleBackImage.Height), 
							GraphicsUnit.Pixel);
					}
					else
					{
						g.DrawImage(TitleBackImage, 0, y, Width, TitleBarHeight);
					}
				}
				else
				{
					if (SystemSettings.OfficialTheme)
					{
						using (Image image = new Bitmap(Width, TitleBarHeight))
						{
							using (Graphics g2 = Graphics.FromImage(image))
							{
								// left edge
								g2.DrawImage(TitleBackImage, 
									new Rectangle(0, y, 5, TitleBarHeight),
									new Rectangle(0, 0, 5, TitleBackImage.Height), 
									GraphicsUnit.Pixel);
						

								// right edge
								g2.DrawImage(TitleBackImage, 
									new Rectangle(Width-5, y, 5, TitleBarHeight),
									new Rectangle(TitleBackImage.Width-5, 0, 5, TitleBackImage.Height), 
									GraphicsUnit.Pixel);

								// middle
								g2.DrawImage(TitleBackImage, 
									new Rectangle(5, y, Width-10, TitleBarHeight),
									new Rectangle(5, 0, TitleBackImage.Width-10, TitleBackImage.Height), 
									GraphicsUnit.Pixel);
							}

							ControlPaint.DrawImageDisabled(g, image, 0, y, TitleBackColor);
						}
					}
					else
					{
						// first stretch the background image for ControlPaint.
						using (Image image = new Bitmap(TitleBackImage, Width, TitleBarHeight))
						{
							ControlPaint.DrawImageDisabled(g, image, 0, y, TitleBackColor);
						}
					}
				}
			}		
			else
			{
				// single color titlebar
				using (SolidBrush brush = new SolidBrush(TitleBackColor))
				{
					g.FillRectangle(brush, 0, y, Width, TitleBarHeight);
				}
			}
		}


		/// <summary>
		/// Paints the title bar
		/// </summary>
		/// <param name="g">The Graphics used to paint the titlebar</param>
		protected void OnPaintTitleBar(Graphics g)
		{
			int y = 0;
			
			// work out where the top of the titleBar actually is
			if (HeaderHeight > TitleBarHeight)
			{
				y = HeaderHeight - TitleBarHeight;
			}

			// draw the titlebar image if we have one
			if (TitleImage != null)
			{
				int x = 0;
				//int y = 0;
				
				if (RightToLeft == RightToLeft.Yes)
				{
					x = Width - TitleImage.Width;
				}
				
				if (Enabled)
				{
					g.DrawImage(TitleImage, x, 0);
				}
				else
				{
					ControlPaint.DrawImageDisabled(g, TitleImage, x, 0, TitleBackColor);
				}
			}

			// get which collapse/expand arrow we should draw
			Image arrowImage = ArrowImage;

			// get the titlebar's border and padding
			Border border = TitleBorder;
			Padding padding = TitlePadding;

			// draw the arrow if we have one
			if (arrowImage != null)
			{
				// work out where to position the arrow
				int x = Width - arrowImage.Width - border.Right - padding.Right;
				y += border.Top + padding.Top;

				if (RightToLeft == RightToLeft.Yes)
				{
					x = border.Right + padding.Right;
				}

				// draw it...
				if (Enabled)
				{
					g.DrawImage(arrowImage, x, y);
				}
				else
				{
					ControlPaint.DrawImageDisabled(g, arrowImage, x, y, TitleBackColor);
				}
			}

			// check if we have any text to draw in the titlebar
			if (Text.Length > 0)
			{
				// a rectangle that will contain our text
				Rectangle rect = new Rectangle();
				
				// work out the x coordinate
				if (TitleImage == null)
				{
					rect.X = border.Left + padding.Left;
				}
				else
				{
					rect.X = TitleImage.Width + border.Left;
				}

				// work out the y coordinate
				ContentAlignment alignment = TitleAlignment;

				switch (alignment)
				{
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.MiddleRight:	rect.Y = ((HeaderHeight - TitleFont.Height) / 2) + ((HeaderHeight - TitleBarHeight) / 2) + border.Top + padding.Top;
						break;

					case ContentAlignment.TopLeft:
					case ContentAlignment.TopCenter:
					case ContentAlignment.TopRight:		rect.Y = (HeaderHeight - TitleBarHeight) + border.Top + padding.Top;
						break;

					case ContentAlignment.BottomLeft:
					case ContentAlignment.BottomCenter:
					case ContentAlignment.BottomRight:	rect.Y = HeaderHeight - TitleFont.Height;
						break;
				}

				// the height of the rectangle
				rect.Height = TitleFont.Height;

				// make sure the text stays inside the header
				if (rect.Bottom > HeaderHeight)
				{
					rect.Y -= rect.Bottom - HeaderHeight;
				}
					
				// work out how wide the rectangle should be
				if (arrowImage != null)
				{
					rect.Width = Width - arrowImage.Width - border.Right - padding.Right - rect.X;
				}
				else
				{
					rect.Width = Width - border.Right - padding.Right - rect.X;
				}

				// don't wrap the string, and use an ellipsis if
				// the string is too big to fit the rectangle
				StringFormat sf = new StringFormat();
				sf.FormatFlags = StringFormatFlags.NoWrap;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				// should the string be aligned to the left/center/right
				switch (alignment)
				{
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.TopLeft:
					case ContentAlignment.BottomLeft:	sf.Alignment = StringAlignment.Near;
						break;

					case ContentAlignment.MiddleCenter:
					case ContentAlignment.TopCenter:
					case ContentAlignment.BottomCenter:	sf.Alignment = StringAlignment.Center;
						break;

					case ContentAlignment.MiddleRight:
					case ContentAlignment.TopRight:
					case ContentAlignment.BottomRight:	sf.Alignment = StringAlignment.Far;
						break;
				}

				if (RightToLeft == RightToLeft.Yes)
				{
					sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

					if (TitleImage == null)
					{
						rect.X = Width - rect.Width - border.Left - padding.Left;
					}
					else
					{
						rect.X = Width - rect.Width - TitleImage.Width - border.Left;
					}
				}

				// draw the text
				using (SolidBrush brush = new SolidBrush(TitleColor))
				{
					//g.DrawString(this.Text, this.TitleFont, brush, rect, sf);
					if (Enabled)
					{
						g.DrawString(Text, TitleFont, brush, rect, sf);
					}
					else
					{
						ControlPaint.DrawStringDisabled(g, Text, TitleFont, SystemColors.ControlLightLight, rect, sf);
					}
				}
			}

			// check if windows will let us show a focus rectangle 
			// if we have focus
			if (Focused && base.ShowFocusCues)
			{
				if (ShowFocusCues)
				{
					// for some reason, if CanCollapse is false the focus rectangle 
					// will be drawn 2 pixels higher than it should be, so move it down
					if (!CanCollapse)
					{
						y += 2;
					}
					
					ControlPaint.DrawFocusRectangle(g, new Rectangle(2, y, Width - 4, TitleBarHeight - 3));
				}
			}
		}

		#endregion

		#region DisplayRect

		/// <summary>
		/// Paints the "Display Rectangle".  This is the dockable
		/// area of the control (ie non-titlebar/border area).  This is
		/// also the same as the PseudoClientRect.
		/// </summary>
		/// <param name="g">The Graphics used to paint the DisplayRectangle</param>
		protected void OnPaintDisplayRect(Graphics g)
		{
			// are we animating a fade
			if (animatingFade && AnimationImage != null)
			{
				// calculate the transparency value for the animation image
				float alpha = ((Height - HeaderHeight) / ((float) (ExpandedHeight - HeaderHeight)));
				
				float[][] ptsArray = {new float[] {1, 0, 0, 0, 0},
										 new float[] {0, 1, 0, 0, 0},
										 new float[] {0, 0, 1, 0, 0},
										 new[] {0, 0, 0, alpha, 0}, 
										 new float[] {0, 0, 0, 0, 1}}; 

				ColorMatrix colorMatrix = new ColorMatrix(ptsArray);
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				// work out how far up the animation image we need to start
				int y = AnimationImage.Height - PseudoClientHeight - Border.Bottom;

				// draw the image
				g.DrawImage(AnimationImage,
					new Rectangle(0, HeaderHeight, Width, Height - HeaderHeight),
					0,
					y,
					AnimationImage.Width, 
					AnimationImage.Height - y,
					GraphicsUnit.Pixel,
					imageAttributes);
			}
				// are we animating a slide
			else if (animatingSlide)
			{
				// check if we have a background image
				if (BackImage != null)
				{
					// tile the backImage
					using (TextureBrush brush = new TextureBrush(BackImage, WrapMode.Tile))
					{
						g.FillRectangle(brush, DisplayRectangle);
					}
				}
				else
				{
					// just paint the area with a solid brush
					using (SolidBrush brush = new SolidBrush(BackColor))
					{
						g.FillRectangle(brush, 
							Border.Left, 
							HeaderHeight + Border.Top, 
							Width - Border.Left - Border.Right,
							Height - HeaderHeight - Border.Top - Border.Bottom);
					}
				}
			}
			else
			{
				// check if we have a background image
				if (BackImage != null)
				{
					// tile the backImage
					using (TextureBrush brush = new TextureBrush(BackImage, WrapMode.Tile))
					{
						g.FillRectangle(brush, DisplayRectangle);
					}
				}
				else
				{
					// just paint the area with a solid brush
					using (SolidBrush brush = new SolidBrush(BackColor))
					{
						g.FillRectangle(brush, DisplayRectangle);
					}
				}

				if (Watermark != null)
				{
					// work out a rough location of where the watermark should go
					Rectangle rect = new Rectangle(0, 0, Watermark.Width, Watermark.Height);
					rect.X = PseudoClientRect.Right - Watermark.Width;
					rect.Y = DisplayRectangle.Bottom - Watermark.Height;

					// shrink the destination rect if necesary so that we
					// can see all of the image
				
					if (rect.X < 0)
					{
						rect.X = 0;
					}

					if (rect.Width > ClientRectangle.Width)
					{
						rect.Width = ClientRectangle.Width;
					}

					if (rect.Y < DisplayRectangle.Top)
					{
						rect.Y = DisplayRectangle.Top;
					}

					if (rect.Height > DisplayRectangle.Height)
					{
						rect.Height = DisplayRectangle.Height;
					}

					// draw the watermark
					g.DrawImage(Watermark, rect);
				}
			}
		}

		#endregion

		#region Borders

		/// <summary>
		/// Paints the borders
		/// </summary>
		/// <param name="g">The Graphics used to paint the border</param>
		protected void OnPaintBorder(Graphics g)
		{
			// get the current border and border colors
			Border border = Border;
			Color c = BorderColor;

			// check if we are currently animating a fade
			if (animatingFade)
			{
				// calculate the alpha value for the color
				int alpha = (int) (255 * ((Height - HeaderHeight) / ((float) (ExpandedHeight - HeaderHeight))));

				// make sure it doesn't go past 0 or 255
				if (alpha < 0)
				{
					alpha = 0;
				}
				else if (alpha > 255)
				{
					alpha = 255;
				}

				// update the color with the alpha value
				c = Color.FromArgb(alpha, c.R, c.G, c.B);
			}
			
			// draw the borders
			using (SolidBrush brush = new SolidBrush(c))
			{
				g.FillRectangle(brush, border.Left, HeaderHeight, Width-border.Left-border.Right, border.Top); // top border
				g.FillRectangle(brush, 0, HeaderHeight, border.Left, Height-HeaderHeight); // left border
				g.FillRectangle(brush, Width-border.Right, HeaderHeight, border.Right, Height-HeaderHeight); // right border
				g.FillRectangle(brush, border.Left, Height-border.Bottom, Width-border.Left-border.Right, border.Bottom); // bottom border
			}
		}

		#endregion

		#region TransparentBackground

		/// <summary>
		/// Simulates a transparent background by getting the Expandos parent 
		/// to paint its background and foreground into the specified Graphics 
		/// at the specified location
		/// </summary>
		/// <param name="g">The Graphics used to paint the background</param>
		/// <param name="clipRect">The Rectangle that represents the rectangle 
		/// in which to paint</param>
		protected void PaintTransparentBackground(Graphics g, Rectangle clipRect)
		{
			// check if we have a parent
			if (Parent != null)
			{
				// convert the clipRects coordinates from ours to our parents
				clipRect.Offset(Location);

				PaintEventArgs e = new PaintEventArgs(g, clipRect);

				// save the graphics state so that if anything goes wrong 
				// we're not fubar
				GraphicsState state = g.Save();

				try
				{
					// move the graphics object so that we are drawing in 
					// the correct place
					g.TranslateTransform(-Location.X, -Location.Y);
					
					// draw the parents background and foreground
					InvokePaintBackground(Parent, e);
					InvokePaint(Parent, e);

					return;
				}
				finally
				{
					// reset everything back to where they were before
					g.Restore(state);
					clipRect.Offset(-Location.X, -Location.Y);
				}
			}

			// we don't have a parent, so fill the rect with
			// the default control color
			g.FillRectangle(SystemBrushes.Control, clipRect);
		}

		#endregion

		#endregion

		#region Parent

		/// <summary>
		/// Raises the ParentChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnParentChanged(EventArgs e)
		{
			if (Parent == null)
			{
				TaskPane = null;
			}
			else if (Parent is TaskPane)
			{
				TaskPane = (TaskPane) Parent;

				Location = TaskPane.CalcExpandoLocation(this);
			}
			
			base.OnParentChanged(e);
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
			
			// if we are currently animating and the width of the
			// group has changed (eg. due to a scrollbar on the 
			// TaskPane appearing/disappearing), get a new image 
			// to use for the animation. (if we were to continue to 
			// use the old image it would be shrunk or stretched making 
			// the animation look wierd)
			if (Animating && Width != oldWidth)
			{
				// if the width or height of the group is zero it probably 
				// means that our parent form has been minimized so we should 
				// immediately stop animating
				if (Width == 0)
				{
					animationHelper.StopAnimation();
				}
				else
				{
					oldWidth = Width;
				
					if (AnimationImage != null)
					{
						// get the new animationImage
						animationImage = GetFadeAnimationImage();
					}
				}
			}
				// check if the width has changed.  if it has re-layout
				// the group so that the TaskItems can resize themselves
				// if neccessary
			else if (Width != oldWidth)
			{
				oldWidth = Width;
				
				DoLayout();
			}
		}

		#endregion

		#endregion


		#region ItemCollection

		/// <summary>
		/// Represents a collection of Control objects
		/// </summary>
		public class ItemCollection : CollectionBase
		{
			#region Class Data

			/// <summary>
			/// The Expando that owns this ControlCollection
			/// </summary>
			private Expando owner;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the Expando.ItemCollection class
			/// </summary>
			/// <param name="owner">An Expando representing the expando that owns 
			/// the Control collection</param>
			public ItemCollection(Expando owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}
				
				this.owner = owner;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Adds the specified control to the control collection
			/// </summary>
			/// <param name="value">The Control to add to the control collection</param>
			public void Add(Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				List.Add(value);
				owner.Controls.Add(value);

				owner.OnItemAdded(new ControlEventArgs(value));
			}


			/// <summary>
			/// Adds an array of control objects to the collection
			/// </summary>
			/// <param name="controls">An array of Control objects to add 
			/// to the collection</param>
			public void AddRange(Control[] controls)
			{
				if (controls == null)
				{
					throw new ArgumentNullException("controls");
				}

				for (int i=0; i<controls.Length; i++)
				{
					Add(controls[i]);
				}
			}
			
			
			/// <summary>
			/// Removes all controls from the collection
			/// </summary>
			public new void Clear()
			{
				while (Count > 0)
				{
					RemoveAt(0);
				}
			}


			/// <summary>
			/// Determines whether the specified control is a member of the 
			/// collection
			/// </summary>
			/// <param name="control">The Control to locate in the collection</param>
			/// <returns>true if the Control is a member of the collection; 
			/// otherwise, false</returns>
			public bool Contains(Control control)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}

				return (IndexOf(control) != -1);
			}


			/// <summary>
			/// Retrieves the index of the specified control in the control 
			/// collection
			/// </summary>
			/// <param name="control">The Control to locate in the collection</param>
			/// <returns>A zero-based index value that represents the position 
			/// of the specified Control in the Expando.ItemCollection</returns>
			public int IndexOf(Control control)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				
				for (int i=0; i<Count; i++)
				{
					if (this[i] == control)
					{
						return i;
					}
				}

				return -1;
			}


			/// <summary>
			/// Removes the specified control from the control collection
			/// </summary>
			/// <param name="value">The Control to remove from the 
			/// Expando.ItemCollection</param>
			public void Remove(Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				List.Remove(value);
				owner.Controls.Remove(value);

				owner.OnItemRemoved(new ControlEventArgs(value));
			}

			
			/// <summary>
			/// Removes a control from the control collection at the 
			/// specified indexed location
			/// </summary>
			/// <param name="index">The index value of the Control to 
			/// remove</param>
			public new void RemoveAt(int index)
			{
				Remove(this[index]);
			}


			/// <summary>
			/// Moves the specified control to the specified indexed location 
			/// in the control collection
			/// </summary>
			/// <param name="value">The control to be moved</param>
			/// <param name="index">The indexed location in the control collection 
			/// that the specified control will be moved to</param>
			public void Move(Control value, int index)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				// make sure the index is within range
				if (index < 0)
				{
					index = 0;
				}
				else if (index > Count)
				{
					index = Count;
				}

				// don't go any further if the expando is already 
				// in the desired position or we don't contain it
				if (!Contains(value) || IndexOf(value) == index)
				{
					return;
				}

				List.Remove(value);

				// if the index we're supposed to move the expando to
				// is now greater to the number of expandos contained, 
				// add it to the end of the list, otherwise insert it at 
				// the specified index
				if (index > Count)
				{
					List.Add(value);
				}
				else
				{
					List.Insert(index, value);
				}

				// re-layout the controls
				owner.MatchControlCollToItemColl();
			}


			/// <summary>
			/// Moves the specified control to the top of the control collection
			/// </summary>
			/// <param name="value">The control to be moved</param>
			public void MoveToTop(Control value)
			{
				Move(value, 0);
			}


			/// <summary>
			/// Moves the specified control to the bottom of the control collection
			/// </summary>
			/// <param name="value">The control to be moved</param>
			public void MoveToBottom(Control value)
			{
				Move(value, Count);
			}

			#endregion


			#region Properties

			/// <summary>
			/// The Control located at the specified index location within 
			/// the control collection
			/// </summary>
			/// <param name="index">The index of the control to retrieve 
			/// from the control collection</param>
			public virtual Control this[int index]
			{
				get
				{
					return List[index] as Control;
				}
			}

			#endregion
		}

		#endregion
	
	
		#region ItemCollectionEditor

		/// <summary>
		/// A custom CollectionEditor for editing ItemCollections
		/// </summary>
		internal class ItemCollectionEditor : CollectionEditor
		{
			/// <summary>
			/// Initializes a new instance of the CollectionEditor class 
			/// using the specified collection type
			/// </summary>
			/// <param name="type"></param>
			public ItemCollectionEditor(Type type) : base(type)
			{
			
			}


			/// <summary>
			/// Edits the value of the specified object using the specified 
			/// service provider and context
			/// </summary>
			/// <param name="context">An ITypeDescriptorContext that can be 
			/// used to gain additional context information</param>
			/// <param name="isp">A service provider object through which 
			/// editing services can be obtained</param>
			/// <param name="value">The object to edit the value of</param>
			/// <returns>The new value of the object. If the value of the 
			/// object has not changed, this should return the same object 
			/// it was passed</returns>
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider isp, object value)
			{
				Expando originalControl = (Expando) context.Instance;

				object returnObject = base.EditValue(context, isp, value);

				originalControl.UpdateItems();

				return returnObject;
			}


			/// <summary>
			/// Gets the data types that this collection editor can contain
			/// </summary>
			/// <returns>An array of data types that this collection can contain</returns>
			protected override Type[] CreateNewItemTypes()
			{
				return new[] {typeof(TaskItem),
									  typeof(Button),
									  typeof(CheckBox),
									  typeof(CheckedListBox),
									  typeof(ComboBox),
									  typeof(DateTimePicker),
									  typeof(Label),
									  typeof(LinkLabel),
									  typeof(ListBox),
									  typeof(ListView),
									  typeof(Panel),
									  typeof(ProgressBar),
									  typeof(RadioButton),
									  typeof(TabControl),
									  typeof(TextBox),
									  typeof(TreeView)};
			}


			/// <summary>
			/// Creates a new instance of the specified collection item type
			/// </summary>
			/// <param name="itemType">The type of item to create</param>
			/// <returns>A new instance of the specified object</returns>
			protected override object CreateInstance(Type itemType)
			{
				// if the item we're supposed to create is one of the 
				// types that doesn't correctly draw themed borders 
				// during animation, substitute it for our customised 
				// versions which will.

				if (itemType == typeof(TextBox))
				{
					itemType = typeof(XPTextBox);
				}
				else if (itemType == typeof(CheckedListBox))
				{
					itemType = typeof(XPCheckedListBox);
				}
				else if (itemType == typeof(ListBox))
				{
					itemType = typeof(XPListBox);
				}
				else if (itemType == typeof(ListView))
				{
					itemType = typeof(XPListView);
				}
				else if (itemType == typeof(TreeView))
				{
					itemType = typeof(XPTreeView);
				}
				else if (itemType == typeof(DateTimePicker))
				{
					itemType = typeof(XPDateTimePicker);
				}

				return base.CreateInstance(itemType);
			}
		}

		#endregion


		#region AnimationPanel

		/// <summary>
		/// An extremely stripped down version of an Expando that an 
		/// Expando can use instead of itself to get an image of its 
		/// "client area" and child controls
		/// </summary>
		internal class AnimationPanel : Panel
		{
			#region Class Data

			/// <summary>
			/// The height of the header section 
			/// (includes titlebar and title image)
			/// </summary>
			protected int headerHeight;
 
			/// <summary>
			/// The border around the "client area"
			/// </summary>
			protected Border border;

			/// <summary>
			/// The background image displayed in the control
			/// </summary>
			protected Image backImage;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the AnimationPanel class with default settings
			/// </summary>
			public AnimationPanel()
			{
				headerHeight = 0;
				border = new Border();
				backImage = null;
			}

			#endregion


			#region Properties

			/// <summary>
			/// Overrides AutoScroll to disable scrolling
			/// </summary>
			public new bool AutoScroll
			{
				get
				{
					return false;
				}

				set
				{

				}
			}


			/// <summary>
			/// Gets or sets the height of the header section of the Expando
			/// </summary>
			public int HeaderHeight
			{
				get
				{
					return headerHeight;
				}

				set
				{
					headerHeight = value;
				}
			}


			/// <summary>
			/// Gets or sets the border around the "client area"
			/// </summary>
			public Border Border
			{
				get
				{
					return border;
				}

				set
				{
					border = value;
				}
			}


			/// <summary>
			/// Gets or sets the background image displayed in the control
			/// </summary>
			public Image BackImage
			{
				get
				{
					return backImage;
				}

				set
				{
					backImage = value;
				}
			}


			/// <summary>
			/// Overrides DisplayRectangle so that docked controls
			/// don't cover the titlebar or borders
			/// </summary>
			public override Rectangle DisplayRectangle
			{
				get
				{
					return new Rectangle(Border.Left, 
						HeaderHeight + Border.Top,
						Width - Border.Left - Border.Right,
						Height - HeaderHeight - Border.Top - Border.Bottom);
				}
			}

			#endregion


			#region Events

			/// <summary>
			/// Raises the Paint event
			/// </summary>
			/// <param name="e">A PaintEventArgs that contains the event data</param>
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);

				if (BackImage != null)
				{
					e.Graphics.DrawImageUnscaled(BackImage, 0, 0);
				}
			}

			#endregion
		}

		#endregion


		#region AnimationHelper

		/// <summary>
		/// A class that helps Expandos animate
		/// </summary>
		public class AnimationHelper : IDisposable
		{
			#region Class Data

			/// <summary>
			/// The number of frames in an animation
			/// </summary>
			public static readonly int NumAnimationFrames = 23;

			/// <summary>
			/// Specifes that a fade animation is to be performed
			/// </summary>
			public static int FadeAnimation = 1;
		
			/// <summary>
			/// Specifes that a slide animation is to be performed
			/// </summary>
			public static int SlideAnimation = 2;

			/// <summary>
			/// The type of animation to perform
			/// </summary>
			private int animationType;

			/// <summary>
			/// The Expando to animate
			/// </summary>
			private Expando expando;

			/// <summary>
			/// The current frame in animation
			/// </summary>
			private int animationStepNum;

			/// <summary>
			/// The number of frames in the animation
			/// </summary>
			private int numAnimationSteps;

			/// <summary>
			/// The amount of time each frame is shown (in milliseconds)
			/// </summary>
			private int animationFrameInterval;

			/// <summary>
			/// Specifies whether an animation is being performed
			/// </summary>
			private bool animating;

			/// <summary>
			/// A timer that notifies the helper when the next frame is due
			/// </summary>
			private Timer animationTimer;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the AnimationHelper class with the specified settings
			/// </summary>
			/// <param name="expando">The Expando to be animated</param>
			/// <param name="animationType">The type of animation to perform</param>
			public AnimationHelper(Expando expando, int animationType)
			{
				this.expando = expando;
				this.animationType = animationType;

				animating = false;

				numAnimationSteps = NumAnimationFrames;
				animationFrameInterval = 10;

				// I know that this isn't the best way to do this, but I 
				// haven't quite worked out how to do it with threads so 
				// this will have to do for the moment
				animationTimer = new Timer();
				animationTimer.Tick += animationTimer_Tick;
				animationTimer.Interval = animationFrameInterval;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Releases all resources used by the AnimationHelper
			/// </summary>
			public void Dispose()
			{
				if (animationTimer != null)
				{
					animationTimer.Stop();
					animationTimer.Dispose();
					animationTimer = null;
				}

				expando = null;
			}

			
			/// <summary>
			/// Starts the Expando collapse/expand animation
			/// </summary>
			public void StartAnimation()
			{
				// don't bother going any further if we are already animating
				if (Animating)
				{
					return;
				}
			
				animationStepNum = 0;

				// tell the expando to get ready to animate
				if (AnimationType == FadeAnimation)
				{
					expando.StartFadeAnimation();
				}
				else
				{
					expando.StartSlideAnimation();
				}

				// start the animation timer
				animationTimer.Start();
			}


			/// <summary>
			/// Updates the animation for the Expando
			/// </summary>
			protected void PerformAnimation()
			{
				// if we have more animation steps to perform
				if (animationStepNum < numAnimationSteps)
				{
					// update the animation step number
					animationStepNum++;

					// tell the animating expando to update the animation
					if (AnimationType == FadeAnimation)
					{
						expando.UpdateFadeAnimation(animationStepNum, numAnimationSteps);
					}
					else
					{
						expando.UpdateSlideAnimation(animationStepNum, numAnimationSteps);
					}
				}
				else
				{
					StopAnimation();
				}
			}


			/// <summary>
			/// Stops the Expando collapse/expand animation
			/// </summary>
			public void StopAnimation()
			{
				// stop the animation
				animationTimer.Stop();
				animationTimer.Dispose();

				if (AnimationType == FadeAnimation)
				{
					expando.StopFadeAnimation();
				}
				else
				{
					expando.StopSlideAnimation();
				}
			}

			#endregion


			#region Properties

			/// <summary>
			/// Gets the Expando that is te be animated
			/// </summary>
			public Expando Expando
			{
				get
				{
					return expando;
				}
			}


			/// <summary>
			/// Gets or sets the number of steps that are needed for the Expando 
			/// to get from fully expanded to fully collapsed, or visa versa
			/// </summary>
			public int NumAnimationSteps
			{
				get
				{
					return numAnimationSteps;
				}

				set
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("value", "NumAnimationSteps must be at least 0");
					}
				
					// only change this if we are not currently animating
					// (if we are animating, changing this could cause things
					// to screw up big time)
					if (!animating)
					{
						numAnimationSteps = value;
					}
				}
			}


			/// <summary>
			/// Gets or sets the number of milliseconds that each "frame" 
			/// of the animation stays on the screen
			/// </summary>
			public int AnimationFrameInterval
			{
				get
				{
					return animationFrameInterval;
				}

				set
				{
					animationFrameInterval = value;
				}
			}


			/// <summary>
			/// Gets whether the Expando is currently animating
			/// </summary>
			public bool Animating
			{
				get
				{
					return animating;
				}
			}


			/// <summary>
			/// Gets the type of animation to perform
			/// </summary>
			public int AnimationType
			{
				get
				{
					return animationType;
				}
			}

			#endregion


			#region Events

			/// <summary>
			/// Event handler for the animation timer tick event
			/// </summary>
			/// <param name="sender">The object that fired the event</param>
			/// <param name="e">An EventArgs that contains the event data</param>
			private void animationTimer_Tick(object sender, EventArgs e)
			{
				// do the next bit of the aniation
				PerformAnimation();
			}

			#endregion
		}

		#endregion


		#region ExpandoSurrogate

		/// <summary>
		/// A class that is serialized instead of an Expando (as 
		/// Expandos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class ExpandoSurrogate : ISerializable
		{
			#region Class Data

			/// <summary>
			/// See Expando.Name.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string Name;

			/// <summary>
			/// See Expando.Text.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string Text;

			/// <summary>
			/// See Expando.Size.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Size Size;
			
			/// <summary>
			/// See Expando.Location.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Point Location;
			
			/// <summary>
			/// See Expando.BackColor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string BackColor;
			
			/// <summary>
			/// See Expando.ExpandedHeight.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public int ExpandedHeight;
			
			/// <summary>
			/// See Expando.CustomSettings.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public ExpandoInfo.ExpandoInfoSurrogate CustomSettings;
			
			/// <summary>
			/// See Expando.CustomHeaderSettings.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public HeaderInfo.HeaderInfoSurrogate CustomHeaderSettings;
			
			/// <summary>
			/// See Expando.Animate.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Animate;
			
			/// <summary>
			/// See Expando.ShowFocusCues.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool ShowFocusCues;
			
			/// <summary>
			/// See Expando.Collapsed.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Collapsed;
			
			/// <summary>
			/// See Expando.CanCollapse.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool CanCollapse;
			
			/// <summary>
			/// See Expando.SpecialGroup.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool SpecialGroup;

			/// <summary>
			/// See Expando.TitleImage.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlElement("TitleImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] TitleImage;

			/// <summary>
			/// See Expando.Watermark.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlElement("Watermark", typeof(Byte[]), DataType="base64Binary")]
			public byte[] Watermark;
			
			/// <summary>
			/// See Expando.Enabled.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Enabled;
			
			/// <summary>
			/// See Expando.Visible.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Visible;

			/// <summary>
			/// See Expando.AutoLayout.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool AutoLayout;
			
			/// <summary>
			/// See Expando.Anchor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public AnchorStyles Anchor;
			
			/// <summary>
			/// See Expando.Dock.  This member is not intended to be used 
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
			/// See Expando.Items.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlArray("Items"), XmlArrayItem("TaskItemSurrogate", typeof(TaskItem.TaskItemSurrogate))]
			public ArrayList Items;

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
			/// Initializes a new instance of the ExpandoSurrogate class with default settings
			/// </summary>
			public ExpandoSurrogate()
			{
				Name = null;
				Text = null;
				Size = Size.Empty;
				Location = Point.Empty;

				BackColor = ThemeManager.ConvertColorToString(SystemColors.Control);
				ExpandedHeight = -1;
				
				CustomSettings = null;
				CustomHeaderSettings = null;

				Animate = false;
				ShowFocusCues = false;
				Collapsed = false;
				CanCollapse = true;
				SpecialGroup = false;

				TitleImage = new byte[0];
				Watermark = new byte[0];

				Enabled = true;
				Visible = true;
				AutoLayout = false;

				Anchor = AnchorStyles.None;
				Dock = DockStyle.None;

				FontName = "Tahoma";
				FontSize = 8.25f;
				FontDecoration = FontStyle.Regular;

				Items = new ArrayList();

				Tag = new byte[0];
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the ExpandoSurrogate with data that is to be 
			/// serialized from the specified Expando
			/// </summary>
			/// <param name="expando">The Expando that contains the data 
			/// to be serialized</param>
			public void Load(Expando expando)
			{
				Name = expando.Name;
				Text = expando.Text;
				Size = expando.Size;
				Location = expando.Location;

				BackColor = ThemeManager.ConvertColorToString(expando.BackColor);
				ExpandedHeight = expando.ExpandedHeight;

				CustomSettings = new ExpandoInfo.ExpandoInfoSurrogate();
				CustomSettings.Load(expando.CustomSettings);
				CustomHeaderSettings = new HeaderInfo.HeaderInfoSurrogate();
				CustomHeaderSettings.Load(expando.CustomHeaderSettings);

				Animate = expando.Animate;
				ShowFocusCues = expando.ShowFocusCues;
				Collapsed = expando.Collapsed;
				CanCollapse = expando.CanCollapse;
				SpecialGroup = expando.SpecialGroup;

				TitleImage = ThemeManager.ConvertImageToByteArray(expando.TitleImage);
				Watermark = ThemeManager.ConvertImageToByteArray(expando.Watermark);

				Enabled = expando.Enabled;
				Visible = expando.Visible;
				AutoLayout = expando.AutoLayout;

				Anchor = expando.Anchor;
				Dock = expando.Dock;

				FontName = expando.Font.FontFamily.Name;
				FontSize = expando.Font.SizeInPoints;
				FontDecoration = expando.Font.Style;

				Tag = ThemeManager.ConvertObjectToByteArray(expando.Tag);

				for (int i=0; i<expando.Items.Count; i++)
				{
					if (expando.Items[i] is TaskItem)
					{
						TaskItem.TaskItemSurrogate tis = new TaskItem.TaskItemSurrogate();

						tis.Load((TaskItem) expando.Items[i]);

						Items.Add(tis);
					}
				}
			}


			/// <summary>
			/// Returns an Expando that contains the deserialized ExpandoSurrogate data
			/// </summary>
			/// <returns>An Expando that contains the deserialized ExpandoSurrogate data</returns>
			public Expando Save()
			{
				Expando expando = new Expando();
				((ISupportInitialize) expando).BeginInit();
				expando.SuspendLayout();

				expando.Name = Name;
				expando.Text = Text;
				expando.Size = Size;
				expando.Location = Location;

				expando.BackColor = ThemeManager.ConvertStringToColor(BackColor);
				expando.ExpandedHeight = ExpandedHeight;

				expando.customSettings = CustomSettings.Save();
				expando.customSettings.Expando = expando;
				expando.customHeaderSettings = CustomHeaderSettings.Save();
				expando.customHeaderSettings.Expando = expando;

				expando.TitleImage = ThemeManager.ConvertByteArrayToImage(TitleImage);
				expando.Watermark = ThemeManager.ConvertByteArrayToImage(Watermark);

				expando.Animate = Animate;
				expando.ShowFocusCues = ShowFocusCues;
				expando.Collapsed = Collapsed;
				expando.CanCollapse = CanCollapse;
				expando.SpecialGroup = SpecialGroup;

				expando.Enabled = Enabled;
				expando.Visible = Visible;
				expando.AutoLayout = AutoLayout;

				expando.Anchor = Anchor;
				expando.Dock = Dock;

				expando.Font = new Font(FontName, FontSize, FontDecoration);

				expando.Tag = ThemeManager.ConvertByteArrayToObject(Tag);

				foreach (Object o in Items)
				{
					TaskItem ti = ((TaskItem.TaskItemSurrogate) o).Save();

					expando.Items.Add(ti);
				}

				((ISupportInitialize) expando).EndInit();
				expando.ResumeLayout(false);

				return expando;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the ExpandoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("Name", Name);
				info.AddValue("Text", Text);
				info.AddValue("Size", Size);
				info.AddValue("Location", Location);

				info.AddValue("BackColor", BackColor);
				info.AddValue("ExpandedHeight", ExpandedHeight);

				info.AddValue("CustomSettings", CustomSettings);
				info.AddValue("CustomHeaderSettings", CustomHeaderSettings);
				
				info.AddValue("Animate", Animate);
				info.AddValue("ShowFocusCues", ShowFocusCues);
				info.AddValue("Collapsed", Collapsed);
				info.AddValue("CanCollapse", CanCollapse);
				info.AddValue("SpecialGroup", SpecialGroup);

				info.AddValue("TitleImage", TitleImage);
				info.AddValue("Watermark", Watermark);
				
				info.AddValue("Enabled", Enabled);
				info.AddValue("Visible", Visible);
				info.AddValue("AutoLayout", AutoLayout);

				info.AddValue("Anchor", Anchor);
				info.AddValue("Dock", Dock);
				
				info.AddValue("FontName", FontName);
				info.AddValue("FontSize", FontSize);
				info.AddValue("FontDecoration", FontDecoration);
				
				info.AddValue("Tag", Tag);
				
				info.AddValue("Items", Items);
			}


			/// <summary>
			/// Initializes a new instance of the ExpandoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the ExpandoSurrogate</param>
			/// <param name="context">The source from which the ExpandoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected ExpandoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				Name = info.GetString("Name");
				Text = info.GetString("Text");
				Size = (Size) info.GetValue("Size", typeof(Size));
				Location = (Point) info.GetValue("Location", typeof(Point));

				BackColor = info.GetString("BackColor");
				ExpandedHeight = info.GetInt32("ExpandedHeight");

				CustomSettings = (ExpandoInfo.ExpandoInfoSurrogate) info.GetValue("CustomSettings", typeof(ExpandoInfo.ExpandoInfoSurrogate));
				CustomHeaderSettings = (HeaderInfo.HeaderInfoSurrogate) info.GetValue("CustomHeaderSettings", typeof(HeaderInfo.HeaderInfoSurrogate));

				Animate = info.GetBoolean("Animate");
				ShowFocusCues = info.GetBoolean("ShowFocusCues");
				Collapsed = info.GetBoolean("Collapsed");
				CanCollapse = info.GetBoolean("CanCollapse");
				SpecialGroup = info.GetBoolean("SpecialGroup");

				TitleImage = (byte[]) info.GetValue("TitleImage", typeof(byte[]));
				Watermark = (byte[]) info.GetValue("Watermark", typeof(byte[]));

				Enabled = info.GetBoolean("Enabled");
				Visible = info.GetBoolean("Visible");
				AutoLayout = info.GetBoolean("AutoLayout");
				
				Anchor = (AnchorStyles) info.GetValue("Anchor", typeof(AnchorStyles));
				Dock = (DockStyle) info.GetValue("Dock", typeof(DockStyle));

				FontName = info.GetString("FontName");
				FontSize = info.GetSingle("FontSize");
				FontDecoration = (FontStyle) info.GetValue("FontDecoration", typeof(FontStyle));

				Tag = (byte[]) info.GetValue("Tag", typeof(byte[]));

				Items = (ArrayList) info.GetValue("Items", typeof(ArrayList));
			}

			#endregion
		}

		#endregion
	}

	#endregion



	#region ExpandoEventArgs

	/// <summary>
	/// Provides data for the StateChanged, ExpandoAdded and 
	/// ExpandoRemoved events
	/// </summary>
	public class ExpandoEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The Expando that generated the event
		/// </summary>
		private Expando expando;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the ExpandoEventArgs class with default settings
		/// </summary>
		public ExpandoEventArgs()
		{
			expando = null;
		}


		/// <summary>
		/// Initializes a new instance of the ExpandoEventArgs class with specific Expando
		/// </summary>
		/// <param name="expando">The Expando that generated the event</param>
		public ExpandoEventArgs(Expando expando)
		{
			this.expando = expando;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Expando that generated the event
		/// </summary>
		public Expando Expando
		{
			get
			{
				return expando;
			}
		}


		/// <summary>
		/// Gets whether the Expando is collapsed
		/// </summary>
		public bool Collapsed
		{
			get
			{
				return expando.Collapsed;
			}
		}

		#endregion
	}

	#endregion



	#region ExpandoConverter

	/// <summary>
	/// A custom TypeConverter used to help convert Expandos from 
	/// one Type to another
	/// </summary>
	internal class ExpandoConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert the object to the 
		/// specified type, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="destinationType">A Type that represents the type 
		/// you want to convert to</param>
		/// <returns>true if this converter can perform the conversion; o
		/// therwise, false</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}


		/// <summary>
		/// Converts the given value object to the specified type, using 
		/// the specified context and culture information
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides 
		/// a format context</param>
		/// <param name="culture">A CultureInfo object. If a null reference 
		/// is passed, the current culture is assumed</param>
		/// <param name="value">The Object to convert</param>
		/// <param name="destinationType">The Type to convert the value 
		/// parameter to</param>
		/// <returns>An Object that represents the converted value</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor) && value is Expando)
			{
				ConstructorInfo ci = typeof(Expando).GetConstructor(new Type[] {});

				if (ci != null)
				{
					return new InstanceDescriptor(ci, null, false);
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	#endregion



	#region ExpandoDesigner

	/// <summary>
	/// A custom designer used by Expandos to remove unwanted 
	/// properties from the Property window in the designer
	/// </summary>
	internal class ExpandoDesigner : ParentControlDesigner
	{
		/// <summary>
		/// Initializes a new instance of the ExpandoDesigner class
		/// </summary>
		public ExpandoDesigner()
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

			properties.Remove("BackColor");
			properties.Remove("BackgroundImage");
			properties.Remove("BorderStyle");
			properties.Remove("Cursor");
			properties.Remove("BackgroundImage");
		}
	}

	#endregion



	#region FocusStates

	/// <summary>
	/// Defines the state of an Expandos title bar
	/// </summary>
	public enum FocusStates
	{
		/// <summary>
		/// Normal state
		/// </summary>
		None = 0,	
		
		/// <summary>
		/// The mouse is over the title bar
		/// </summary>
		Mouse = 1
	}

	#endregion
}
