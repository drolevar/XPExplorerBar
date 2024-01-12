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
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace XPExplorerBar
{
	#region TaskPane
	
	/// <summary>
	/// A ScrollableControl that can contain Expandos
	/// </summary>
	[ToolboxItem(true),
	DesignerAttribute(typeof(TaskPaneDesigner))]
	public class TaskPane : ScrollableControl, ISupportInitialize
	{
		#region Event Handlers

		/// <summary>
		/// Occurs when an Expando is added to the TaskPane
		/// </summary>
		public event ExpandoEventHandler ExpandoAdded; 

		/// <summary>
		/// Occurs when an Expando is removed from the TaskPane
		/// </summary>
		public event ExpandoEventHandler ExpandoRemoved; 

		/// <summary>
		/// Occurs when a value in the CustomSettings proterty changes
		/// </summary>
		public event EventHandler CustomSettingsChanged;

		#endregion
			
		
		#region Class Data
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components;

		/// <summary>
		/// Internal list of Expandos contained in the TaskPane
		/// </summary>
		private ExpandoCollection expandoCollection;
		
		/// <summary>
		/// System defined settings for the TaskBar
		/// </summary>
		private ExplorerBarInfo systemSettings;

		/// <summary>
		/// Specifies whether the TaskPane is currently initialising
		/// </summary>
		private bool initialising;

		/// <summary>
		/// Specifies whether the TaskPane and its children should render 
		/// themselves using a theme similar to the Windows XP Classic theme
		/// </summary>
		private bool classicTheme;
		
		/// <summary>
		/// Specifies whether the TaskPane and its children should render 
		/// themselves using a non-official Windows XP theme
		/// </summary>
		private bool customTheme;

		/// <summary>
		/// A Rectangle that specifies the size and location of the watermark
		/// </summary>
		private Rectangle watermarkRect;

		/// <summary>
		/// Specifies whether the TaskPane is currently performing a 
		/// layout operation
		/// </summary>
		private bool layout;

		/// <summary>
		/// 
		/// </summary>
		private int beginUpdateCount;

		/// <summary>
		/// Specifies the custom settings for the TaskPane
		/// </summary>
		private TaskPaneInfo customSettings;

		/// <summary>
		/// 
		/// </summary>
		private bool allowExpandoDragging;

		/// <summary>
		/// 
		/// </summary>
		private Point dropPoint;

		/// <summary>
		/// 
		/// </summary>
		private Color dropIndicatorColor;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the TaskPane class with default settings
		/// </summary>
		public TaskPane()
		{
			// This call is required by the Windows.Forms Form Designer.
			components = new Container();

			// set control styles
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			expandoCollection = new ExpandoCollection(this);

			// get the system theme settings
			systemSettings = ThemeManager.GetSystemExplorerBarSettings();

			customSettings = new TaskPaneInfo();
			customSettings.TaskPane = this;
			customSettings.SetDefaultEmptyValues();

			BackColor = systemSettings.TaskPane.GradientStartColor;
			BackgroundImage = BackImage;

			classicTheme = false;
			customTheme = false;

			// size
			int width = (systemSettings.TaskPane.Padding.Left + 
				systemSettings.TaskPane.Padding.Right + 
				systemSettings.Header.BackImageWidth);
			int height = width;
			Size = new Size(width, height);

			// setup sutoscrolling
			AutoScroll = false;
			AutoScrollMargin = new Size(systemSettings.TaskPane.Padding.Right, 
				systemSettings.TaskPane.Padding.Bottom);

			// Listen for changes to the parent
			ParentChanged += OnParentChanged;

			allowExpandoDragging = false;
			dropPoint = Point.Empty;
			dropIndicatorColor = Color.Red;

			beginUpdateCount = 0;

			initialising = false;
			layout = false;
		}

		#endregion


		#region Methods

		#region Appearance

		/// <summary>
		/// Forces the TaskPane and all it's Expandos to use a theme
		/// equivalent to Windows XPs classic theme 
		/// </summary>
		public void UseClassicTheme()
		{
			classicTheme = true;
			customTheme = false;
			
			ExplorerBarInfo settings = ThemeManager.GetSystemExplorerBarSettings(true);

			systemSettings.Dispose();
			systemSettings = null;

			SystemSettings = settings;
		}


		/// <summary>
		/// Forces the TaskPane and all it's Expandos to use the 
		/// specified theme
		/// </summary>
		/// <param name="stylePath">The path to the custom 
		/// shellstyle.dll to use</param>
		public void UseCustomTheme(string stylePath)
		{
			customTheme = true;
			classicTheme = false;
			
			ExplorerBarInfo settings = ThemeManager.GetSystemExplorerBarSettings(stylePath);
			
			systemSettings.Dispose();
			systemSettings = null;

			SystemSettings = settings;
		}


		/// <summary>
		/// Forces the TaskPane and all it's Expandos to use the 
		/// specified theme
		/// </summary>
		/// <param name="stylePath">The path to the custom 
		/// shellstyle.dll to use</param>
		public void UseCustomTheme(Theme theme)
		{
			customTheme = true;
			classicTheme = false;
			
			ExplorerBarInfo settings = ThemeManager.GetSystemExplorerBarSettings(theme);
			
			systemSettings.Dispose();
			systemSettings = null;

			SystemSettings = settings;
		}


		/// <summary>
		/// Forces the TaskPane and all it's Expandos to use the 
		/// current system theme
		/// </summary>
		public void UseDefaultTheme()
		{
			customTheme = false;
			classicTheme = false;
			
			ExplorerBarInfo settings = ThemeManager.GetSystemExplorerBarSettings();

			systemSettings.Dispose();
			systemSettings = null;

			SystemSettings = settings;
		}

		#endregion

		#region Dispose

		/// <summary> 
		/// Releases the unmanaged resources used by the TaskPane and 
		/// optionally releases the managed resources
		/// </summary>
		/// <param name="disposing">True to release both managed and unmanaged 
		/// resources; false to release only unmanaged resources</param>
		protected override void Dispose( bool disposing )
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
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Expandos

		/// <summary>
		/// Collaspes all the Expandos contained in the TaskPane
		/// </summary>
		// suggested by: PaleyX (jmpalethorpe@tiscali.co.uk)
		//               03/06/2004
		//               v1.1
		public void CollapseAll()
		{
			foreach (Expando expando in Expandos)
			{
				expando.Collapsed = true;
			}
		}


		/// <summary>
		/// Expands all the Expandos contained in the TaskPane
		/// </summary>
		// suggested by: PaleyX (jmpalethorpe@tiscali.co.uk)
		//               03/06/2004
		//               v1.1
		public void ExpandAll()
		{
			foreach (Expando expando in Expandos)
			{
				expando.Collapsed = false;
			}
		}


		/// <summary>
		/// Collaspes all the Expandos contained in the TaskPane, 
		/// except for the specified Expando which is expanded
		/// </summary>
		/// <param name="expando">The Expando that is to be expanded</param>
		// suggested by: PaleyX (jmpalethorpe@tiscali.co.uk)
		//               03/06/2004
		//               v1.1
		public void CollapseAllButOne(Expando expando)
		{
			foreach (Expando e in Expandos)
			{
				if (e != expando)
				{
					e.Collapsed = true;
				}
				else
				{
					expando.Collapsed = false;
				}
			}
		}


		/// <summary>
		/// Calculates the Point that the currently dragged Expando will 
		/// dropped at based on the specified mouse position
		/// </summary>
		/// <param name="point">The current position of the mouse in screen 
		/// co-ordinates</param>
		internal void UpdateDropPoint(Point point)
		{
			Point p = PointToClient(point);
			
			if (ClientRectangle.Contains(p))
			{
				if (p.Y <= Expandos[0].Top)
				{
					dropPoint.Y = Padding.Top / 2;
				}
				else if (p.Y >= Expandos[Expandos.Count - 1].Bottom)
				{
					dropPoint.Y = Expandos[Expandos.Count - 1].Bottom + (Padding.Top / 2);
				}
				else
				{
					for (int i=0; i<Expandos.Count; i++)
					{
						if (p.Y >= Expandos[i].Top && p.Y <= Expandos[i].Bottom)
						{
							if (p.Y <= Expandos[i].Top + (Expandos[i].Height / 2))
							{
								if (i == 0)
								{
									dropPoint.Y = Padding.Top / 2;
								}
								else
								{
									dropPoint.Y = Expandos[i].Top - ((Expandos[i].Top - Expandos[i-1].Bottom) / 2);
								}
							}
							else
							{	
								if (i == Expandos.Count - 1)
								{
									dropPoint.Y = Expandos[Expandos.Count - 1].Bottom + (Padding.Top / 2);
								}
								else
								{
									dropPoint.Y = Expandos[i].Bottom + ((Expandos[i+1].Top - Expandos[i].Bottom) / 2);
								}
							}

							break;
						}
					}
				}
			}
			else
			{
				dropPoint = Point.Empty;
			}

			Invalidate(false);
		}


		/// <summary>
		/// "Drops" the specified Expando and moves it to the current drop point
		/// </summary>
		/// <param name="expando">The Expando to be "dropped"</param>
		internal void DropExpando(Expando expando)
		{
			if (dropPoint == Point.Empty)
			{
				return;
			}
			
			if (expando != null && expando.TaskPane == this)
			{
				int i = 0;
				int expandoIndex = Expandos.IndexOf(expando);
				
				for (; i<Expandos.Count; i++)
				{
					if (dropPoint.Y <= Expandos[i].Top)
					{
						if (i > expandoIndex)
						{
							Expandos.Move(expando, i-1);
						}
						else if (i < expandoIndex)
						{
							Expandos.Move(expando, i);
						}

						break;
					}
				}

				if (i == Expandos.Count)
				{
					Expandos.Move(expando, i);
				}
			}

			dropPoint = Point.Empty;

			Invalidate(false);
		}

		#endregion

		#region ISupportInitialize Members

		/// <summary>
		/// Signals the TaskPane that initialization is starting
		/// </summary>
		public void BeginInit()
		{
			initialising = true;
		}


		/// <summary>
		/// Signals the TaskPane that initialization is complete
		/// </summary>
		public void EndInit()
		{
			initialising = false;

			DoLayout();
		}


		/// <summary>
		/// Gets whether the TaskPane is currently initialising
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

		#region Layout

		// fix: Added BeginUpdate() and EndUpdate() so that DoLayout() 
		//      isn't called everytime something happens with Expandos
		//      Brian Nottingham (nottinbe@slu.edu)
		//      22/12/2004
		//      v3.0
		
		/// <summary>
		/// Prevents the TaskPane from drawing until the EndUpdate method is called
		/// </summary>
		public void BeginUpdate()
		{
			beginUpdateCount++;
		}


		/// <summary>
		/// Resumes drawing of the TaskPane after drawing is suspended by the 
		/// BeginUpdate method
		/// </summary>
		public void EndUpdate()
		{
			beginUpdateCount = Math.Max(beginUpdateCount--, 0);

			if (beginUpdateCount == 0)
			{
				DoLayout(true);
			}
		}


		/// <summary>
		/// Forces the TaskPane to apply layout logic to child Expandos, 
		/// and adjusts the Size and Location of the Expandos if necessary
		/// </summary>
		public void DoLayout()
		{
			DoLayout(false);
		}


		// fix: Added DoLayout(bool performRealLayout) to improve 
		//      TaskPane scroll behavior
		//      Jewlin (jewlin88@hotmail.com)
		//      22/10/2004
		//      v3.0

		/// <summary>
		/// Forces the TaskPane to apply layout logic to child Expandos, 
		/// and adjusts the Size and Location of the Expandos if necessary
		/// </summary>
		/// <param name="performRealLayout">true to execute pending layout 
		/// requests; otherwise, false</param>
		public void DoLayout(bool performRealLayout)
		{
			// fix: take into account beginUpdateCount
			//      Brian Nottingham (nottinbe@slu.edu)
			//      22/12/2004
			//      v3.0
			//if (this.layout)
			if (layout || beginUpdateCount > 0)
			{
				return;
			}

			layout = true;
			
			// stop the layout engine
			SuspendLayout();
			
			Expando e;
			Point p;
			
			// work out how wide to make the controls, and where
			// the top of the first control should be
			int y = DisplayRectangle.Y + Padding.Top;
			int width = ClientSize.Width - Padding.Left - Padding.Right;

			// for each control in our list...
			for (int i=0; i<Expandos.Count; i++)
			{
				e = Expandos[i];

				// go to the next expando if this one is invisible and 
				// it's parent is visible
				if (!e.Visible && e.Parent != null && e.Parent.Visible)
				{
					continue;
				}

				p = new Point(Padding.Left, y);

				// set the width and location of the control
				e.Location = p;
				e.Width = width;

				// update the next starting point
				y += e.Height + Padding.Bottom;
			}

			// restart the layout engine
			ResumeLayout(performRealLayout);

			layout = false;
		}


		/// <summary>
		/// Calculates where the specified Expando should be located
		/// </summary>
		/// <returns>A Point that specifies where the Expando should 
		/// be located</returns>
		protected internal Point CalcExpandoLocation(Expando target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			int targetIndex = Expandos.IndexOf(target);

			Expando e;
			Point p;
			
			int y = DisplayRectangle.Y + Padding.Top;
			int width = ClientSize.Width - Padding.Left - Padding.Right;

			for (int i=0; i<targetIndex; i++)
			{
				e = Expandos[i];

				if (!e.Visible)
				{
					continue;
				}

				p = new Point(Padding.Left, y);
				y += e.Height + Padding.Bottom;
			}
			
			return new Point(Padding.Left, y);
		}


		/// <summary>
		/// Updates the layout of the Expandos while in design mode, and 
		/// adds/removes Expandos from the ControlCollection as necessary
		/// </summary>
		internal void UpdateExpandos()
		{
			if (Expandos.Count == Controls.Count)
			{
				// make sure the the expandos index in the ControlCollection 
				// are the same as in the ExpandoCollection (indexes in the 
				// ExpandoCollection may have changed due to the user moving 
				// them around in the editor)
				MatchControlCollToExpandoColl();				
				
				return;
			}

			// were any expandos added
			if (Expandos.Count > Controls.Count)
			{
				// add any extra expandos in the ExpandoCollection to the 
				// ControlCollection
				for (int i=0; i<Expandos.Count; i++)
				{
					if (!Controls.Contains(Expandos[i]))
					{
						OnExpandoAdded(new ExpandoEventArgs(Expandos[i]));
					}
				}
			}
			else
			{
				// expandos were removed
				int i = 0;
				Expando expando;

				// remove any extra expandos from the ControlCollection
				while (i < Controls.Count)
				{
					expando = (Expando) Controls[i];
					
					if (!Expandos.Contains(expando))
					{
						OnExpandoRemoved(new ExpandoEventArgs(expando));
					}
					else
					{
						i++;
					}
				}
			}
		}


		/// <summary>
		/// Make sure the the expandos index in the ControlCollection 
		/// are the same as in the ExpandoCollection (indexes in the 
		/// ExpandoCollection may have changed due to the user moving 
		/// them around in the editor or calling ExpandoCollection.Move())
		/// </summary>
		internal void MatchControlCollToExpandoColl()
		{
			SuspendLayout();
				
			for (int i=0; i<Expandos.Count; i++)
			{
				Controls.SetChildIndex(Expandos[i], i);
			}

			ResumeLayout(false);
				
			DoLayout(true);

			Invalidate(true);
		}

		#endregion

		#endregion


		#region Properties

		#region Colors

		/// <summary>
		/// Gets the first color of the TaskPane's background gradient fill.
		/// </summary>
		[Browsable(false)]
		public Color GradientStartColor
		{
			get
			{
				if (CustomSettings.GradientStartColor != Color.Empty)
				{
					return CustomSettings.GradientStartColor;
				}

				return systemSettings.TaskPane.GradientStartColor;
			}
		}


		/// <summary>
		/// Gets the second color of the TaskPane's background gradient fill.
		/// </summary>
		[Browsable(false)]
		public Color GradientEndColor
		{
			get
			{
				if (CustomSettings.GradientEndColor != Color.Empty)
				{
					return CustomSettings.GradientEndColor;
				}

				return systemSettings.TaskPane.GradientEndColor;
			}
		}


		/// <summary>
		/// Gets the direction of the TaskPane's background gradient fill.
		/// </summary>
		[Browsable(false)]
		public LinearGradientMode GradientDirection
		{
			get
			{
				if (CustomSettings.GradientStartColor != Color.Empty && 
					CustomSettings.GradientEndColor != Color.Empty)
				{
					return CustomSettings.GradientDirection;
				}

				return systemSettings.TaskPane.GradientDirection;
			}
		}

		#endregion

		#region Expandos

		/// <summary>
		/// A TaskPane.ExpandoCollection representing the collection of 
		/// Expandos contained within the TaskPane
		/// </summary>
		[Category("Behavior"),
		DefaultValue(null), 
		Description("The Expandos contained in the TaskPane"), 
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content), 
		Editor(typeof(ExpandoCollectionEditor), typeof(UITypeEditor))]
		public ExpandoCollection Expandos
		{
			get
			{
				return expandoCollection;
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


		/// <summary>
		/// Gets or sets whether Expandos can be dragged around the TaskPane
		/// </summary>
		[Category("Behavior"),
		DefaultValue(false), 
		Description("Indicates whether Expandos can be dragged around the TaskPane")]
		public bool AllowExpandoDragging
		{
			get
			{
				return allowExpandoDragging;
			}

			set
			{
				allowExpandoDragging = value;
			}
		}


		/// <summary>
		/// Gets or sets the Color that the Expando drop point indicator is drawn in
		/// </summary>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color ExpandoDropIndicatorColor
		{
			get
			{
				return dropIndicatorColor;
			}

			set
			{
				dropIndicatorColor = value;
			}
		}

		#endregion

		#region Images

		/// <summary>
		/// Gets the Image used as the TaskPane's background
		/// </summary>
		[Browsable(false)]
		public Image BackImage
		{
			get
			{
				if (CustomSettings.BackImage != null)
				{
					return CustomSettings.BackImage;
				}

				return systemSettings.TaskPane.BackImage;
			}
		}


		/// <summary>
		/// Gets how the TaskPane's background Image is to be drawn
		/// </summary>
		[Browsable(false)]
		public ImageStretchMode StretchMode
		{
			get
			{
				if (CustomSettings.BackImage != null)
				{
					return CustomSettings.StretchMode;
				}

				return systemSettings.TaskPane.StretchMode;
			}
		}


		/// <summary>
		/// Gets the Image that is used as a watermark in the TaskPane's 
		/// client area
		/// </summary>
		[Browsable(false)]
		public Image Watermark
		{
			get
			{
				if (CustomSettings.Watermark != null)
				{
					return CustomSettings.Watermark;
				}

				return systemSettings.TaskPane.Watermark;
			}
		}


		/// <summary>
		/// Gets the alignment of the TaskPane's watermark
		/// </summary>
		[Browsable(false)]
		public ContentAlignment WatermarkAlignment
		{
			get
			{
				if (CustomSettings.Watermark != null)
				{
					return CustomSettings.WatermarkAlignment;
				}

				return systemSettings.TaskPane.WatermarkAlignment;
			}
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets the amount of space between the border and the 
		/// Expando's along each side of the TaskPane.
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

				return systemSettings.TaskPane.Padding;
			}
		}

		#endregion

		#region SystemSettings

		/// <summary>
		/// Gets or sets the system defined settings for the TaskPane
		/// </summary>
		protected internal ExplorerBarInfo SystemSettings
		{
			get
			{
				return systemSettings;
			}

			set
			{
				// ignore null values
				if (value == null)
				{
					return;
				}
				
				if (systemSettings != value)
				{
					SuspendLayout();
					
					if (systemSettings != null)
					{
						systemSettings.Dispose();
						systemSettings = null;
					}

					watermarkRect = Rectangle.Empty;

					systemSettings = value;
					BackColor = GradientStartColor;
					BackgroundImage = BackImage;

					foreach (Expando expando in Expandos)
					{
						expando.SystemSettings = systemSettings;
						expando.DoLayout();
					}

					DoLayout();

					ResumeLayout(true);

					Invalidate(true);
				}
			}
		}


		/// <summary>
		/// Gets the custom settings for the TaskPane
		/// </summary>
		[Category("Appearance"),
		Description(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		TypeConverter(typeof(TaskPaneInfoConverter))]
		public TaskPaneInfo CustomSettings
		{
			get
			{
				return customSettings;
			}

			set
			{
				customSettings = value;
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

		#endregion


		#region Events

		#region Controls

		/// <summary>
		/// Raises the ControlAdded event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// make sure the control is an Expando
			if ((e.Control as Expando) == null)
			{
				// remove the control
				Controls.Remove(e.Control);

				// throw a hissy fit
				throw new InvalidCastException("Only Expando's can be added to the TaskPane");
			}
			
			base.OnControlAdded(e);

			// add the expando to the ExpandoCollection if necessary
			if (!Expandos.Contains((Expando) e.Control))
			{
				Expandos.Add((Expando) e.Control);
			}
		}


		/// <summary>
		/// Raises the ControlRemoved event
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved (e);

			// remove the control from the itemList
			if (Expandos.Contains(e.Control))
			{
				Expandos.Remove((Expando) e.Control);
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
			BackColor = GradientStartColor;
			BackgroundImage = BackImage;
				
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

		#region Expandos

		/// <summary> 
		/// Event handler for the Expando StateChanged event
		/// </summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		private void expando_StateChanged(object sender, ExpandoEventArgs e)
		{
			OnExpandoStateChanged(e);
		}


		/// <summary>
		/// Occurs when the value of an Expandos Collapsed property changes
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnExpandoStateChanged(ExpandoEventArgs e)
		{
			DoLayout(true);
		}


		/// <summary>
		/// Raises the ExpandoAdded event
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnExpandoAdded(ExpandoEventArgs e)
		{
			// add the expando to the ControlCollection if it hasn't already
			if (!Controls.Contains(e.Expando))
			{
				Controls.Add(e.Expando);
			}

			// set anchor styles
			e.Expando.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);		
			
			// tell the Expando who's its daddy...
			e.Expando.TaskPane = this;
			e.Expando.SystemSettings = systemSettings;

			// listen for collapse/expand events
			e.Expando.StateChanged += expando_StateChanged;

			// update the layout of the controls
			DoLayout();

			//
			if (ExpandoAdded != null)
			{
				ExpandoAdded(this, e);
			}
		}


		/// <summary>
		/// Raises the ExpandoRemoved event
		/// </summary>
		/// <param name="e">An ExpandoEventArgs that contains the event data</param>
		protected virtual void OnExpandoRemoved(ExpandoEventArgs e)
		{
			// remove the control from the ControlCollection if it hasn't already
			if (Controls.Contains(e.Expando))
			{
				Controls.Remove(e.Expando);
			}

			// remove the StateChanged listener
			e.Expando.StateChanged -= expando_StateChanged;

			// update the layout of the controls
			DoLayout();

			//
			if (ExpandoRemoved != null)
			{
				ExpandoRemoved(this, e);
			}
		}

		#endregion

		#region Paint

		/// <summary> 
		/// Raises the PaintBackground event
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// paint background
			if (BackImage != null)
			{
				//base.OnPaintBackground(e);

				WrapMode wrap = WrapMode.Clamp;
				
				if ((StretchMode == ImageStretchMode.Tile) || (StretchMode == ImageStretchMode.Horizontal))
				{
					wrap = WrapMode.Tile;
				}

				using (TextureBrush brush = new TextureBrush(BackImage, wrap))
				{
					e.Graphics.FillRectangle(brush, ClientRectangle);
				}
			}
			else
			{
				if (GradientStartColor != GradientEndColor)
				{
					using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, 
							   GradientStartColor, 
							   GradientEndColor, 
							   GradientDirection))
					{
						e.Graphics.FillRectangle(brush, ClientRectangle);
					}
				}
				else
				{
					using (SolidBrush brush = new SolidBrush(GradientStartColor))
					{
						e.Graphics.FillRectangle(brush, ClientRectangle);
					}
				}
			}

			// draw the watermark if we have one
			if (Watermark != null)
			{
				Rectangle rect = new Rectangle(0, 0, Watermark.Width, Watermark.Height);

				// work out a rough location of where the watermark should go

				switch (WatermarkAlignment)
				{
					case ContentAlignment.BottomCenter:
					case ContentAlignment.BottomLeft:
					case ContentAlignment.BottomRight:
					{
						rect.Y = DisplayRectangle.Bottom - Watermark.Height;
						
						break;
					}

					case ContentAlignment.MiddleCenter:
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.MiddleRight:
					{
						rect.Y = DisplayRectangle.Top + ((DisplayRectangle.Height - Watermark.Height) / 2);
						
						break;
					}
				}

				switch (WatermarkAlignment)
				{
					case ContentAlignment.BottomRight:
					case ContentAlignment.MiddleRight:
					case ContentAlignment.TopRight:
					{
						rect.X = ClientRectangle.Right - Watermark.Width;
						
						break;
					}

					case ContentAlignment.BottomCenter:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.TopCenter:
					{
						rect.X = ClientRectangle.Left + ((ClientRectangle.Width - Watermark.Width) / 2);
						
						break;
					}
				}

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
				e.Graphics.DrawImage(Watermark, rect);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (dropPoint != Point.Empty)
			{
				int width = ClientSize.Width - Padding.Left - Padding.Right;

				using (Brush brush = new SolidBrush(ExpandoDropIndicatorColor))
				{
					e.Graphics.FillRectangle(brush, Padding.Left, dropPoint.Y, width, 1);

					e.Graphics.FillPolygon(brush, new[] { new Point(Padding.Left, dropPoint.Y - 4), 
																  new Point(Padding.Left + 4, dropPoint.Y), 
																  new Point(Padding.Left, dropPoint.Y + 4)});

					e.Graphics.FillPolygon(brush, new[] { new Point(Width - Padding.Right, dropPoint.Y - 4), 
																  new Point(Width - Padding.Right - 4, dropPoint.Y), 
																  new Point(Width - Padding.Right, dropPoint.Y + 4)});
				}
			}
		}

		#endregion

		#region Parents

		// fix: TaskPane will now perform a layout when its 
		//      parent becomes visible
		//      Brian Nottingham (nottinbe@slu.edu)
		//      22/12/2004
		//      v3.0
		
		/// <summary>
		/// Event handler for the ParentChanged event
		/// </summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">An EventArgs that contains the event data</param>
		private void OnParentChanged(object sender, EventArgs e)
		{
			if (Parent != null)
			{
				Parent.VisibleChanged += OnParentVisibleChanged;
			}
		}


		/// <summary>
		/// Event handler for the ParentVisibleChanged event
		/// </summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">An EventArgs that contains the event data</param>
		private void OnParentVisibleChanged(object sender, EventArgs e)
		{
			if (sender != Parent)
			{
				((Control) sender).VisibleChanged -= OnParentVisibleChanged;
				
				return;
			}

			if (Parent.Visible)
			{
				DoLayout();
			}
		}

		#endregion

		#region System Colors

		/// <summary> 
		/// Raises the SystemColorsChanged event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);

			// don't go any further if we are explicitly using
			// the classic or a custom theme
			if (classicTheme || customTheme)
			{
				return;
			}

			SuspendLayout();

			// get rid of the current system theme info
			systemSettings.Dispose();
			systemSettings = null;

			// get a new system theme info for the new theme
			systemSettings = ThemeManager.GetSystemExplorerBarSettings();
			
			BackgroundImage = BackImage;


			// update the system settings for each expando
			foreach (Control control in Controls)
			{
				if (control is Expando)
				{
					Expando expando = (Expando) control;
					
					expando.SystemSettings = systemSettings;
				}
			}

			// update the layout of the controls
			DoLayout();

			ResumeLayout(true);
		}

		#endregion

		#region Size

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			DoLayout();
		}

		#endregion

		#endregion


		#region ExpandoCollection

		/// <summary>
		/// Represents a collection of Expando objects
		/// </summary>
		public class ExpandoCollection : CollectionBase
		{
			#region Class Data

			/// <summary>
			/// The TaskPane that owns this ExpandoCollection
			/// </summary>
			private TaskPane owner;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the TaskPane.ExpandoCollection class
			/// </summary>
			/// <param name="owner">A TaskPane representing the taskpane that owns 
			/// the Expando collection</param>
			public ExpandoCollection(TaskPane owner)
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
			/// Adds the specified expando to the expando collection
			/// </summary>
			/// <param name="value">The Expando to add to the expando collection</param>
			public void Add(Expando value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				List.Add(value);
				owner.Controls.Add(value);

				owner.OnExpandoAdded(new ExpandoEventArgs(value));
			}


			/// <summary>
			/// Adds an array of expando objects to the collection
			/// </summary>
			/// <param name="expandos">An array of Expando objects to add 
			/// to the collection</param>
			public void AddRange(Expando[] expandos)
			{
				if (expandos == null)
				{
					throw new ArgumentNullException("expandos");
				}

				for (int i=0; i<expandos.Length; i++)
				{
					Add(expandos[i]);
				}
			}
			
			
			/// <summary>
			/// Removes all expandos from the collection
			/// </summary>
			public new void Clear()
			{
				while (Count > 0)
				{
					RemoveAt(0);
				}
			}


			/// <summary>
			/// Determines whether the specified expando is a member of the 
			/// collection
			/// </summary>
			/// <param name="expando">The Expando to locate in the collection</param>
			/// <returns>true if the Expando is a member of the collection; 
			/// otherwise, false</returns>
			public bool Contains(Expando expando)
			{
				if (expando == null)
				{
					throw new ArgumentNullException("expando");
				}

				return (IndexOf(expando) != -1);
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
				if (!(control is Expando))
				{
					return false;
				}

				return Contains((Expando) control);
			}


			/// <summary>
			/// Retrieves the index of the specified expando in the expando 
			/// collection
			/// </summary>
			/// <param name="expando">The Expando to locate in the collection</param>
			/// <returns>A zero-based index value that represents the position 
			/// of the specified Expando in the TaskPane.ExpandoCollection</returns>
			public int IndexOf(Expando expando)
			{
				if (expando == null)
				{
					throw new ArgumentNullException("expando");
				}
				
				for (int i=0; i<Count; i++)
				{
					if (this[i] == expando)
					{
						return i;
					}
				}

				return -1;
			}


			/// <summary>
			/// Removes the specified expando from the expando collection
			/// </summary>
			/// <param name="value">The Expando to remove from the 
			/// TaskPane.ExpandoCollection</param>
			public void Remove(Expando value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				List.Remove(value);

				owner.Controls.Remove(value);

				owner.OnExpandoRemoved(new ExpandoEventArgs(value));
			}

			
			/// <summary>
			/// Removes an expando from the expando collection at the 
			/// specified indexed location
			/// </summary>
			/// <param name="index">The index value of the Expando to 
			/// remove</param>
			public new void RemoveAt(int index)
			{
				Remove(this[index]);
			}


			/// <summary>
			/// Moves the specified expando to the specified indexed location 
			/// in the expando collection
			/// </summary>
			/// <param name="value">The expando to be moved</param>
			/// <param name="index">The indexed location in the expando collection 
			/// that the specified expando will be moved to</param>
			public void Move(Expando value, int index)
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
				owner.MatchControlCollToExpandoColl();
			}


			/// <summary>
			/// Moves the specified expando to the top of the expando collection
			/// </summary>
			/// <param name="value">The expando to be moved</param>
			public void MoveToTop(Expando value)
			{
				Move(value, 0);
			}


			/// <summary>
			/// Moves the specified expando to the bottom of the expando collection
			/// </summary>
			/// <param name="value">The expando to be moved</param>
			public void MoveToBottom(Expando value)
			{
				Move(value, Count);
			}

			#endregion


			#region Properties

			/// <summary>
			/// The Expando located at the specified index location within 
			/// the expando collection
			/// </summary>
			/// <param name="index">The index of the expando to retrieve 
			/// from the expando collection</param>
			public virtual Expando this[int index]
			{
				get
				{
					return List[index] as Expando;
				}
			}

			#endregion
		}

		#endregion
	
	
		#region ExpandoCollectionEditor

		/// <summary>
		/// A custom CollectionEditor for editing ExpandoCollections
		/// </summary>
		internal class ExpandoCollectionEditor : CollectionEditor
		{
			/// <summary>
			/// Initializes a new instance of the CollectionEditor class 
			/// using the specified collection type
			/// </summary>
			/// <param name="type"></param>
			public ExpandoCollectionEditor(Type type) : base(type)
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
				TaskPane originalControl = (TaskPane) context.Instance;

				object returnObject = base.EditValue(context, isp, value);

				originalControl.UpdateExpandos();

				return returnObject;
			}


			/// <summary>
			/// Creates a new instance of the specified collection item type
			/// </summary>
			/// <param name="itemType">The type of item to create</param>
			/// <returns>A new instance of the specified object</returns>
			protected override object CreateInstance(Type itemType)
			{
				object expando = base.CreateInstance(itemType);
			
				((Expando) expando).Name = "expando";
			
				return expando;
			}
		}

		#endregion


		#region TaskPaneSurrogate

		/// <summary>
		/// A class that is serialized instead of a TaskPane (as 
		/// TaskPanes contain objects that cause serialization problems)
		/// </summary>
		[Serializable,
			XmlRoot("TaskPaneSurrogate", Namespace="", IsNullable=false)]
			public class TaskPaneSurrogate : ISerializable
		{
			#region Class Data

			/// <summary>
			/// See TaskPane.Name.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string Name;

			/// <summary>
			/// See TaskPane.Size.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Size Size;
			
			/// <summary>
			/// See TaskPane.Location.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Point Location;
			
			/// <summary>
			/// See TaskPane.BackColor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string BackColor;
			
			/// <summary>
			/// See TaskPane.CustomSettings.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public TaskPaneInfo.TaskPaneInfoSurrogate CustomSettings;
			
			/// <summary>
			/// See TaskPane.AutoScroll.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool AutoScroll;
			
			/// <summary>
			/// See TaskPane.AutoScrollMargin.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public Size AutoScrollMargin;
			
			/// <summary>
			/// See TaskPane.Enabled.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Enabled;
			
			/// <summary>
			/// See TaskPane.Visible.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool Visible;
			
			/// <summary>
			/// See TaskPane.Anchor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public AnchorStyles Anchor;
			
			/// <summary>
			/// See TaskPane.Dock.  This member is not intended to be used 
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
			/// See TaskPane.Expandos.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlArray("Expandos"), XmlArrayItem("ExpandoSurrogate", typeof(Expando.ExpandoSurrogate))]
			public ArrayList Expandos;

			/// <summary>
			/// See Control.Tag.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			[XmlElementAttribute("Tag", typeof(Byte[]), DataType="base64Binary")]
			public byte[] Tag;

			/// <summary>
			/// See TaskPane.AllowExpandoDragging.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public bool AllowExpandoDragging;

			/// <summary>
			/// See TaskPane.ExpandoDropIndicatorColor.  This member is not intended to be used 
			/// directly from your code.
			/// </summary>
			public string ExpandoDropIndicatorColor;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the TaskPaneSurrogate class with default settings
			/// </summary>
			public TaskPaneSurrogate()
			{
				Name = null;

				Size = Size.Empty;
				Location = Point.Empty;

				BackColor = ThemeManager.ConvertColorToString(SystemColors.Control);

				CustomSettings = null;

				AutoScroll = false;
				AutoScrollMargin = Size.Empty;

				Enabled = true;
				Visible = true;

				Anchor = AnchorStyles.None;
				Dock = DockStyle.None;

				FontName = "Segoe UI";
				FontSize = 8.25f;
				FontDecoration = FontStyle.Regular;

				Tag = new byte[0];

				AllowExpandoDragging = false;
				ExpandoDropIndicatorColor = ThemeManager.ConvertColorToString(Color.Red);

				Expandos = new ArrayList();
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the TaskPaneSurrogate with data that is to be 
			/// serialized from the specified TaskPane
			/// </summary>
			/// <param name="taskPane">The TaskPane that contains the data 
			/// to be serialized</param>
			public void Load(TaskPane taskPane)
			{
				Name = taskPane.Name;
				Size = taskPane.Size;
				Location = taskPane.Location;

				BackColor = ThemeManager.ConvertColorToString(taskPane.BackColor);

				CustomSettings = new TaskPaneInfo.TaskPaneInfoSurrogate();
				CustomSettings.Load(taskPane.CustomSettings);

				AutoScroll = taskPane.AutoScroll;
				AutoScrollMargin = taskPane.AutoScrollMargin;

				Enabled = taskPane.Enabled;
				Visible = taskPane.Visible;

				Anchor = taskPane.Anchor;
				Dock = taskPane.Dock;

				FontName = taskPane.Font.FontFamily.Name;
				FontSize = taskPane.Font.SizeInPoints;
				FontDecoration = taskPane.Font.Style;

				AllowExpandoDragging = taskPane.AllowExpandoDragging;
				ExpandoDropIndicatorColor =  ThemeManager.ConvertColorToString(taskPane.ExpandoDropIndicatorColor);

				Tag = ThemeManager.ConvertObjectToByteArray(taskPane.Tag);

				foreach (Expando expando in taskPane.Expandos)
				{
					Expando.ExpandoSurrogate es = new Expando.ExpandoSurrogate();

					es.Load(expando);

					Expandos.Add(es);
				}
			}


			/// <summary>
			/// Returns a TaskPane that contains the deserialized TaskPaneSurrogate data
			/// </summary>
			/// <returns>A TaskPane that contains the deserialized TaskPaneSurrogate data</returns>
			public TaskPane Save()
			{
				TaskPane taskPane = new TaskPane();
				((ISupportInitialize) taskPane).BeginInit();
				taskPane.SuspendLayout();

				taskPane.Name = Name;
				taskPane.Size = Size;
				taskPane.Location = Location;

				taskPane.BackColor = ThemeManager.ConvertStringToColor(BackColor);

				taskPane.customSettings = CustomSettings.Save();
				taskPane.customSettings.TaskPane = taskPane;

				taskPane.AutoScroll = AutoScroll;
				taskPane.AutoScrollMargin = AutoScrollMargin;

				taskPane.Enabled = Enabled;
				taskPane.Visible = Visible;

				taskPane.Anchor = Anchor;
				taskPane.Dock = Dock;

				taskPane.Font = new Font(FontName, FontSize, FontDecoration);

				taskPane.Tag = ThemeManager.ConvertByteArrayToObject(Tag);

				taskPane.AllowExpandoDragging = AllowExpandoDragging;
				taskPane.ExpandoDropIndicatorColor = ThemeManager.ConvertStringToColor(ExpandoDropIndicatorColor);

				foreach (Object o in Expandos)
				{
					Expando e = ((Expando.ExpandoSurrogate) o).Save();
					
					taskPane.Expandos.Add(e);
				}

				((ISupportInitialize) taskPane).EndInit();
				taskPane.ResumeLayout(false);

				return taskPane;
			}

			
			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the TaskPaneSurrogate
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

				info.AddValue("AutoScroll", AutoScroll);
				info.AddValue("AutoScrollMargin", AutoScrollMargin);

				info.AddValue("Enabled", Enabled);
				info.AddValue("Visible", Visible);

				info.AddValue("Anchor", Anchor);
				info.AddValue("Dock", Dock);
				
				info.AddValue("FontName", FontName);
				info.AddValue("FontSize", FontSize);
				info.AddValue("FontDecoration", FontDecoration);

				info.AddValue("AllowExpandoDragging", AllowExpandoDragging);
				info.AddValue("ExpandoDropIndicatorColor", ExpandoDropIndicatorColor);
				
				info.AddValue("Tag", Tag);
				
				info.AddValue("Expandos", Expandos);
			}


			/// <summary>
			/// Initializes a new instance of the TaskPaneSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the TaskPaneSurrogate</param>
			/// <param name="context">The source from which the TaskPaneSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected TaskPaneSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				Name = info.GetString("Name");
				Size = (Size) info.GetValue("Size", typeof(Size));
				Location = (Point) info.GetValue("Location", typeof(Point));

				BackColor = info.GetString("BackColor");

				CustomSettings = (TaskPaneInfo.TaskPaneInfoSurrogate) info.GetValue("CustomSettings", typeof(TaskPaneInfo.TaskPaneInfoSurrogate));

				AutoScroll = info.GetBoolean("AutoScroll");
				AutoScrollMargin = (Size) info.GetValue("AutoScrollMargin", typeof(Size));

				Enabled = info.GetBoolean("Enabled");
				Visible = info.GetBoolean("Visible");
				
				Anchor = (AnchorStyles) info.GetValue("Anchor", typeof(AnchorStyles));
				Dock = (DockStyle) info.GetValue("Dock", typeof(DockStyle));

				FontName = info.GetString("FontName");
				FontSize = info.GetSingle("FontSize");
				FontDecoration = (FontStyle) info.GetValue("FontDecoration", typeof(FontStyle));

				if (version >= 3300)
				{
					AllowExpandoDragging = info.GetBoolean("AllowExpandoDragging");
					ExpandoDropIndicatorColor = info.GetString("ExpandoDropIndicatorColor");
				}

				Tag = (byte[]) info.GetValue("Tag", typeof(byte[]));

				Expandos = (ArrayList) info.GetValue("Expandos", typeof(ArrayList));
			}

			#endregion
		}

		#endregion
	}

	#endregion



	#region TaskPaneDesigner

	/// <summary>
	/// A custom designer used by TaskPanes to remove unwanted 
	/// properties from the Property window in the designer
	/// </summary>
	internal class TaskPaneDesigner : ScrollableControlDesigner
	{
		/// <summary>
		/// Initializes a new instance of the TaskPaneDesigner class
		/// </summary>
		public TaskPaneDesigner()
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
			properties.Remove("Cursor");
			properties.Remove("ForeColor");
		}
	}

	#endregion
}
