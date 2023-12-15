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
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace XPExplorerBar
{
	#region ExplorerBarInfo Class
	
	/// <summary>
	/// A class that contains system defined settings for an XPExplorerBar
	/// </summary>
	public class ExplorerBarInfo : IDisposable
	{
		#region Class Data
		
		/// <summary>
		/// System defined settings for a TaskPane
		/// </summary>
		private TaskPaneInfo taskPane;

		/// <summary>
		/// System defined settings for a TaskItem
		/// </summary>
		private TaskItemInfo taskItem;

		/// <summary>
		/// System defined settings for an Expando
		/// </summary>
		private ExpandoInfo expando;

		/// <summary>
		/// System defined settings for an Expando's header
		/// </summary>
		private HeaderInfo header;

		/// <summary>
		/// Specifies whether the ExplorerBarInfo represents an 
		/// official Windows XP theme
		/// </summary>
		private bool officialTheme;

		/// <summary>
		/// Specifies whether the ExplorerBarInfo represents the 
		/// Windows XP "classic" theme
		/// </summary>
		private bool classicTheme;

		/// <summary>
		/// A string that contains the full path to the ShellStyle.dll 
		/// that the ExplorerBarInfo was loaded from
		/// </summary>
		private string shellStylePath;

		#endregion

		
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the ExplorerBarInfo class with 
		/// default settings
		/// </summary>
		public ExplorerBarInfo()
		{
			taskPane = new TaskPaneInfo();
			taskItem = new TaskItemInfo();
			expando = new ExpandoInfo();
			header = new HeaderInfo();

			officialTheme = false;
			classicTheme = false;
			shellStylePath = null;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Sets the arrow images for use when theming is not supported
		/// </summary>
		public void SetUnthemedArrowImages()
		{
			Header.SetUnthemedArrowImages();
		}


		/// <summary>
		/// Force use of default values
		/// </summary>
		public void UseClassicTheme()
		{
			classicTheme = true;
			
			TaskPane.SetDefaultValues();
			Expando.SetDefaultValues();
			Header.SetDefaultValues();
			TaskItem.SetDefaultValues();

			SetUnthemedArrowImages();
		}


		/// <summary>
		/// Releases all resources used by the ExplorerBarInfo
		/// </summary>
		public void Dispose()
		{
			taskPane.Dispose();
			header.Dispose();
			expando.Dispose();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the ExplorerPane settings
		/// </summary>
		public TaskPaneInfo TaskPane
		{
			get
			{
				return taskPane;
			}

			set
			{
				taskPane = value;
			}
		}


		/// <summary>
		/// Gets the TaskLink settings
		/// </summary>
		public TaskItemInfo TaskItem
		{
			get
			{
				return taskItem;
			}

			set
			{
				taskItem = value;
			}
		}


		/// <summary>
		/// Gets the Group settings
		/// </summary>
		public ExpandoInfo Expando
		{
			get
			{
				return expando;
			}

			set
			{
				expando = value;
			}
		}


		/// <summary>
		/// Gets the Header settings
		/// </summary>
		public HeaderInfo Header
		{
			get
			{
				return header;
			}

			set
			{
				header = value;
			}
		}


		/// <summary>
		/// Gets whether the ExplorerBarInfo contains settings for 
		/// an official Windows XP Visual Style
		/// </summary>
		public bool OfficialTheme
		{
			get
			{
				return officialTheme;
			}

			/*set
			{
				this.officialTheme = value;
			}*/
		}


		/// <summary>
		/// Sets whether the ExplorerBarInfo contains settings for 
		/// an official Windows XP Visual Style
		/// </summary>
		/// <param name="officialTheme">true if the ExplorerBarInfo 
		/// contains settings for an official Windows XP Visual Style, 
		/// otherwise false</param>
		internal void SetOfficialTheme(bool officialTheme)
		{
			this.officialTheme = officialTheme;
		}


		/// <summary>
		/// Gets whether the ExplorerBarInfo contains settings for 
		/// the Windows XP "classic" Visual Style
		/// </summary>
		public bool ClassicTheme
		{
			get
			{
				return classicTheme;
			}
		}


		/// <summary>
		/// Gets or sets a string that specifies the full path to the 
		/// ShellStyle.dll that the ExplorerBarInfo was loaded from
		/// </summary>
		public string ShellStylePath
		{
			get
			{
				return shellStylePath;
			}

			set
			{
				shellStylePath = value;
			}
		}

		#endregion


		#region ExplorerBarInfoSurrogate

		/// <summary>
		/// A class that is serialized instead of an ExplorerBarInfo (as 
		/// ExplorerBarInfos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class ExplorerBarInfoSurrogate : ISerializable
		{
			#region Class Data

			/// <summary>
			/// This member is not intended to be used directly from your code.
			/// </summary>
			public TaskPaneInfo.TaskPaneInfoSurrogate TaskPaneInfoSurrogate;
			
			/// <summary>
			/// This member is not intended to be used directly from your code.
			/// </summary>
			public TaskItemInfo.TaskItemInfoSurrogate TaskItemInfoSurrogate;
			
			/// <summary>
			/// This member is not intended to be used directly from your code.
			/// </summary>
			public ExpandoInfo.ExpandoInfoSurrogate ExpandoInfoSurrogate;
			
			/// <summary>
			/// This member is not intended to be used directly from your code.
			/// </summary>
			public HeaderInfo.HeaderInfoSurrogate HeaderInfoSurrogate;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the ExplorerBarInfoSurrogate class with default settings
			/// </summary>
			public ExplorerBarInfoSurrogate()
			{
				TaskPaneInfoSurrogate = null;
				TaskItemInfoSurrogate = null;
				ExpandoInfoSurrogate = null;
				HeaderInfoSurrogate = null;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the ExplorerBarInfoSurrogate with data that is to be 
			/// serialized from the specified ExplorerBarInfo
			/// </summary>
			/// <param name="explorerBarInfo">The ExplorerBarInfo that contains the data 
			/// to be serialized</param>
			public void Load(ExplorerBarInfo explorerBarInfo)
			{
				TaskPaneInfoSurrogate = new TaskPaneInfo.TaskPaneInfoSurrogate();
				TaskPaneInfoSurrogate.Load(explorerBarInfo.TaskPane);

				TaskItemInfoSurrogate = new TaskItemInfo.TaskItemInfoSurrogate();
				TaskItemInfoSurrogate.Load(explorerBarInfo.TaskItem);

				ExpandoInfoSurrogate = new ExpandoInfo.ExpandoInfoSurrogate();
				ExpandoInfoSurrogate.Load(explorerBarInfo.Expando);

				HeaderInfoSurrogate = new HeaderInfo.HeaderInfoSurrogate();
				HeaderInfoSurrogate.Load(explorerBarInfo.Header);
			}


			/// <summary>
			/// Returns an ExplorerBarInfo that contains the deserialized ExplorerBarInfoSurrogate data
			/// </summary>
			/// <returns>An ExplorerBarInfo that contains the deserialized ExplorerBarInfoSurrogate data</returns>
			public ExplorerBarInfo Save()
			{
				ExplorerBarInfo explorerBarInfo = new ExplorerBarInfo();

				explorerBarInfo.TaskPane = TaskPaneInfoSurrogate.Save();
				explorerBarInfo.TaskItem = TaskItemInfoSurrogate.Save();
				explorerBarInfo.Expando = ExpandoInfoSurrogate.Save();
				explorerBarInfo.Header = HeaderInfoSurrogate.Save();				
				
				return explorerBarInfo;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the ExplorerBarInfoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("TaskPaneInfoSurrogate", TaskPaneInfoSurrogate);
				info.AddValue("TaskItemInfoSurrogate", TaskItemInfoSurrogate);
				info.AddValue("ExpandoInfoSurrogate", ExpandoInfoSurrogate);
				info.AddValue("HeaderInfoSurrogate", HeaderInfoSurrogate);
			}


			/// <summary>
			/// Initializes a new instance of the ExplorerBarInfoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the ExplorerBarInfoSurrogate</param>
			/// <param name="context">The source from which the ExplorerBarInfoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected ExplorerBarInfoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				TaskPaneInfoSurrogate = (TaskPaneInfo.TaskPaneInfoSurrogate) info.GetValue("TaskPaneInfoSurrogate", typeof(TaskPaneInfo.TaskPaneInfoSurrogate));
				TaskItemInfoSurrogate = (TaskItemInfo.TaskItemInfoSurrogate) info.GetValue("TaskItemInfoSurrogate", typeof(TaskItemInfo.TaskItemInfoSurrogate));
				ExpandoInfoSurrogate = (ExpandoInfo.ExpandoInfoSurrogate) info.GetValue("ExpandoInfoSurrogate", typeof(ExpandoInfo.ExpandoInfoSurrogate));
				HeaderInfoSurrogate = (HeaderInfo.HeaderInfoSurrogate) info.GetValue("HeaderInfoSurrogate", typeof(HeaderInfo.HeaderInfoSurrogate));
			}

			#endregion
		}

		#endregion
	}

	#endregion


	#region TaskPaneInfo Class

	/// <summary>
	/// A class that contains system defined settings for TaskPanes
	/// </summary>
	public class TaskPaneInfo : IDisposable
	{
		#region Class Data
		
		/// <summary>
		/// The starting Color for the TaskPane's background gradient
		/// </summary>
		private Color gradientStartColor;
		
		/// <summary>
		/// The ending Color for the TaskPane's background gradient
		/// </summary>
		private Color gradientEndColor;

		/// <summary>
		/// The direction of the TaskPane's gradient background
		/// </summary>
		private LinearGradientMode direction;

		/// <summary>
		/// The amount of space between the Border and Expandos along 
		/// each edge of the TaskPane
		/// </summary>
		private Padding padding;

		/// <summary>
		/// The Image that is used as the TaskPane's background
		/// </summary>
		private Image backImage;

		/// <summary>
		/// Specified how the TaskPane's background Image is drawn
		/// </summary>
		private ImageStretchMode stretchMode;

		/// <summary>
		/// The Image that is used as a watermark
		/// </summary>
		private Image watermark;

		/// <summary>
		/// The alignment of the Image used as a watermark
		/// </summary>
		private ContentAlignment watermarkAlignment;

		/// <summary>
		/// The TaskPane that owns the TaskPaneInfo
		/// </summary>
		private TaskPane owner;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the TaskPaneInfo class with default settings
		/// </summary>
		public TaskPaneInfo()
		{
			// set background values
			gradientStartColor = Color.Transparent;
			gradientEndColor = Color.Transparent;
			direction = LinearGradientMode.Vertical;

			// set padding values
			padding = new Padding(12, 12, 12, 12);

			// images
			backImage = null;
			stretchMode = ImageStretchMode.Tile;

			watermark = null;
			watermarkAlignment = ContentAlignment.BottomCenter;

			owner = null;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Forces the use of default values
		/// </summary>
		public void SetDefaultValues()
		{
			// set background values
			gradientStartColor = SystemColors.Window;
			gradientEndColor = SystemColors.Window;
			direction = LinearGradientMode.Vertical;

			// set padding values
			padding.Left = 12;
			padding.Top = 12;
			padding.Right = 12;
			padding.Bottom = 12;

			// images
			backImage = null;
			stretchMode = ImageStretchMode.Tile;
			watermark = null;
			watermarkAlignment = ContentAlignment.BottomCenter;
		}


		/// <summary>
		/// Forces the use of default empty values
		/// </summary>
		public void SetDefaultEmptyValues()
		{
			// set background values
			gradientStartColor = Color.Empty;
			gradientEndColor = Color.Empty;
			direction = LinearGradientMode.Vertical;

			// set padding values
			padding.Left = 0;
			padding.Top = 0;
			padding.Right = 0;
			padding.Bottom = 0;

			// images
			backImage = null;
			stretchMode = ImageStretchMode.Tile;
			watermark = null;
			watermarkAlignment = ContentAlignment.BottomCenter;
		}


		/// <summary>
		/// Releases all resources used by the TaskPaneInfo
		/// </summary>
		public void Dispose()
		{
			if (backImage != null)
			{
				backImage.Dispose();
				backImage = null;
			}

			if (watermark != null)
			{
				watermark.Dispose();
				watermark = null;
			}
		}

		#endregion


		#region Properties

		#region Background

		/// <summary>
		/// Gets or sets the TaskPane's first gradient background color
		/// </summary>
		[Description("The TaskPane's first gradient background color")]
		public Color GradientStartColor
		{
			get
			{
				return gradientStartColor;
			}

			set
			{
				if (gradientStartColor != value)
				{
					gradientStartColor = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the GradientStartColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the GradientStartColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeGradientStartColor()
		{
			return GradientStartColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the TaskPane's second gradient background color
		/// </summary>
		[Description("The TaskPane's second gradient background color")]
		public Color GradientEndColor
		{
			get
			{
				return gradientEndColor;
			}

			set
			{
				if (gradientEndColor != value)
				{
					gradientEndColor = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the GradientEndColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the GradientEndColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeGradientEndColor()
		{
			return GradientEndColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the direction of the TaskPane's gradient
		/// </summary>
		[DefaultValue(LinearGradientMode.Vertical),
		Description("The direction of the TaskPane's background gradient")]
		public LinearGradientMode GradientDirection
		{
			get
			{
				return direction;
			}

			set
			{
				if (!Enum.IsDefined(typeof(LinearGradientMode), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(LinearGradientMode));
				}

				if (direction != value)
				{
					direction = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}

		#endregion

		#region Images

		/// <summary>
		/// Gets or sets the Image that is used as the TaskPane's background
		/// </summary>
		[DefaultValue(null),
		Description("The Image that is used as the TaskPane's background")]
		public Image BackImage
		{
			get
			{
				return backImage;
			}

			set
			{
				if (backImage != value)
				{
					backImage = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets how the TaskPane's background Image is drawn
		/// </summary>
		[Browsable(false),
		DefaultValue(ImageStretchMode.Tile),
		Description("Specifies how the TaskPane's background Image is drawn")]
		public ImageStretchMode StretchMode
		{
			get
			{
				return stretchMode;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ImageStretchMode), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ImageStretchMode));
				}

				if (stretchMode != value)
				{
					stretchMode = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the Image that is used as the TaskPane's watermark
		/// </summary>
		[DefaultValue(null),
		Description("The Image that is used as the TaskPane's watermark")]
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

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the alignment of the Image that is used as the 
		/// TaskPane's watermark
		/// </summary>
		[DefaultValue(ContentAlignment.BottomCenter),
		Description("The alignment of the Image that is used as the TaskPane's watermark")]
		public ContentAlignment WatermarkAlignment
		{
			get
			{
				return watermarkAlignment;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ContentAlignment), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ContentAlignment));
				}

				if (watermarkAlignment != value)
				{
					watermarkAlignment = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets or sets the TaskPane's padding between the border and any items
		/// </summary>
		[Description("The amount of space between the border and the Expando's along each side of the TaskPane")]
		public Padding Padding
		{
			get
			{
				return padding;
			}

			set
			{
				if (padding != value)
				{
					padding = value;

					if (TaskPane != null)
					{
						TaskPane.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the Padding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the Padding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializePadding()
		{
			return Padding != Padding.Empty;
		}

		#endregion

		#region TaskPane

		/// <summary>
		/// Gets or sets the TaskPane the TaskPaneInfo belongs to
		/// </summary>
		protected internal TaskPane TaskPane
		{
			get
			{
				return owner;
			}
			
			set
			{
				owner = value;
			}
		}

		#endregion

		#endregion


		#region TaskPaneInfoSurrogate

		/// <summary>
		/// A class that is serialized instead of a TaskPaneInfo (as 
		/// TaskPaneInfos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class TaskPaneInfoSurrogate : ISerializable
		{
			#region Class Data
			
			/// <summary>
			/// See TaskPaneInfo.GradientStartColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string GradientStartColor;
			
			/// <summary>
			/// See TaskPaneInfo.GradientEndColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string GradientEndColor;
			
			/// <summary>
			/// See TaskPaneInfo.GradientDirection.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public LinearGradientMode GradientDirection;
			
			/// <summary>
			/// See TaskPaneInfo.Padding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding Padding;
			
			/// <summary>
			/// See TaskPaneInfo.BackImage.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("BackImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] BackImage;
			
			/// <summary>
			/// See TaskPaneInfo.StretchMode.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public ImageStretchMode StretchMode;
			
			/// <summary>
			/// See TaskPaneInfo.Watermark.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("Watermark", typeof(Byte[]), DataType="base64Binary")]
			public byte[] Watermark;
			
			/// <summary>
			/// See TaskPaneInfo.WatermarkAlignment.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public ContentAlignment WatermarkAlignment;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the TaskPaneInfoSurrogate class with default settings
			/// </summary>
			public TaskPaneInfoSurrogate()
			{
				GradientStartColor = ThemeManager.ConvertColorToString(Color.Empty);
				GradientEndColor = ThemeManager.ConvertColorToString(Color.Empty);
				GradientDirection = LinearGradientMode.Vertical;

				Padding = Padding.Empty;

				BackImage = new byte[0];
				StretchMode = ImageStretchMode.Normal;

				Watermark = new byte[0];
				WatermarkAlignment = ContentAlignment.BottomCenter;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the TaskPaneInfoSurrogate with data that is to be 
			/// serialized from the specified TaskPaneInfo
			/// </summary>
			/// <param name="taskPaneInfo">The TaskPaneInfo that contains the data 
			/// to be serialized</param>
			public void Load(TaskPaneInfo taskPaneInfo)
			{
				GradientStartColor = ThemeManager.ConvertColorToString(taskPaneInfo.GradientStartColor);
				GradientEndColor = ThemeManager.ConvertColorToString(taskPaneInfo.GradientEndColor);
				GradientDirection = taskPaneInfo.GradientDirection;

				Padding = taskPaneInfo.Padding;

				BackImage = ThemeManager.ConvertImageToByteArray(taskPaneInfo.BackImage);
				StretchMode = taskPaneInfo.StretchMode;

				Watermark = ThemeManager.ConvertImageToByteArray(taskPaneInfo.Watermark);
				WatermarkAlignment = taskPaneInfo.WatermarkAlignment;
			}


			/// <summary>
			/// Returns a TaskPaneInfo that contains the deserialized TaskPaneInfoSurrogate data
			/// </summary>
			/// <returns>A TaskPaneInfo that contains the deserialized TaskPaneInfoSurrogate data</returns>
			public TaskPaneInfo Save()
			{
				TaskPaneInfo taskPaneInfo = new TaskPaneInfo();

				taskPaneInfo.GradientStartColor = ThemeManager.ConvertStringToColor(GradientStartColor);
				taskPaneInfo.GradientEndColor = ThemeManager.ConvertStringToColor(GradientEndColor);
				taskPaneInfo.GradientDirection = GradientDirection;

				taskPaneInfo.Padding = Padding;

				taskPaneInfo.BackImage = ThemeManager.ConvertByteArrayToImage(BackImage);
				taskPaneInfo.StretchMode = StretchMode;

				taskPaneInfo.Watermark = ThemeManager.ConvertByteArrayToImage(Watermark);
				taskPaneInfo.WatermarkAlignment = WatermarkAlignment;
				
				return taskPaneInfo;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the TaskPaneInfoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("GradientStartColor", GradientStartColor);
				info.AddValue("GradientEndColor", GradientEndColor);
				info.AddValue("GradientDirection", GradientDirection);
				
				info.AddValue("Padding", Padding);
				
				info.AddValue("BackImage", BackImage);
				info.AddValue("StretchMode", StretchMode);
				
				info.AddValue("Watermark", Watermark);
				info.AddValue("WatermarkAlignment", WatermarkAlignment);
			}


			/// <summary>
			/// Initializes a new instance of the TaskPaneInfoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the TaskPaneInfoSurrogate</param>
			/// <param name="context">The source from which the TaskPaneInfoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected TaskPaneInfoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				GradientStartColor = info.GetString("GradientStartColor");
				GradientEndColor = info.GetString("GradientEndColor");
				GradientDirection = (LinearGradientMode) info.GetValue("GradientDirection", typeof(LinearGradientMode));
				
				Padding = (Padding) info.GetValue("Padding", typeof(Padding));

				BackImage = (byte[]) info.GetValue("BackImage", typeof(byte[]));
				StretchMode = (ImageStretchMode) info.GetValue("StretchMode", typeof(ImageStretchMode));

				Watermark = (byte[]) info.GetValue("Watermark", typeof(byte[]));
				WatermarkAlignment = (ContentAlignment) info.GetValue("WatermarkAlignment", typeof(ContentAlignment));
			}

			#endregion
		}

		#endregion
	}


	#region TaskPaneInfoConverter

	/// <summary>
	/// A custom TypeConverter used to help convert TaskPaneInfo from 
	/// one Type to another
	/// </summary>
	internal class TaskPaneInfoConverter : ExpandableObjectConverter
	{
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
			if (destinationType == typeof(string) && value is TaskPaneInfo)
			{
				return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	#endregion

	#endregion


	#region TaskItemInfo Class

	/// <summary>
	/// A class that contains system defined settings for TaskItems
	/// </summary>
	public class TaskItemInfo
	{
		#region Class Data
		
		/// <summary>
		/// The amount of space around the text along each side of 
		/// the TaskItem
		/// </summary>
		private Padding padding; 

		/// <summary>
		/// The amount of space between individual TaskItems 
		/// along each side of the TaskItem
		/// </summary>
		private Margin margin;

		/// <summary>
		/// The Color of the text displayed in the TaskItem
		/// </summary>
		private Color linkNormal;

		/// <summary>
		/// The Color of the text displayed in the TaskItem when 
		/// highlighted
		/// </summary>
		private Color linkHot;

		/// <summary>
		/// The decoration to be used on the text while in a highlighted state
		/// </summary>
		private FontStyle fontDecoration;

		/// <summary>
		/// The TaskItem that owns this TaskItemInfo
		/// </summary>
		private TaskItem owner;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the TaskLinkInfo class with default settings
		/// </summary>
		public TaskItemInfo()
		{
			// set padding values
			padding = new Padding(6, 0, 4, 0);

			// set margin values
			margin = new Margin(0, 4, 0, 0);

			// set text values
			linkNormal = SystemColors.ControlText;
			linkHot = SystemColors.ControlText;

			fontDecoration = FontStyle.Underline;

			owner = null;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Forces the use of default values
		/// </summary>
		public void SetDefaultValues()
		{
			// set padding values
			padding.Left = 6;
			padding.Top = 0;
			padding.Right = 4;
			padding.Bottom = 0;

			// set margin values
			margin.Left = 0;
			margin.Top = 4;
			margin.Right = 0;
			margin.Bottom = 0;

			// set text values
			linkNormal = SystemColors.ControlText;
			linkHot = SystemColors.HotTrack;

			fontDecoration = FontStyle.Underline;
		}


		/// <summary>
		/// Forces the use of default empty values
		/// </summary>
		public void SetDefaultEmptyValues()
		{
			padding = Padding.Empty;
			margin = Margin.Empty;
			linkNormal = Color.Empty;
			linkHot = Color.Empty;
			fontDecoration = FontStyle.Underline;
		}

		#endregion


		#region Properties

		#region Margin

		/// <summary>
		/// Gets or sets the amount of space between individual TaskItems 
		/// along each side of the TaskItem
		/// </summary>
		[Description("The amount of space between individual TaskItems along each side of the TaskItem")]
		public Margin Margin
		{
			get
			{
				return margin;
			}

			set
			{
				if (margin != value)
				{
					margin = value;

					if (TaskItem != null)
					{
						TaskItem.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the Margin property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the Margin property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeMargin()
		{
			return Margin != Margin.Empty;
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets or sets the amount of space around the text along each 
		/// side of the TaskItem
		/// </summary>
		[Description("The amount of space around the text along each side of the TaskItem")]
		public Padding Padding
		{
			get
			{
				return padding;
			}

			set
			{
				if (padding != value)
				{
					padding = value;

					if (TaskItem != null)
					{
						TaskItem.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the Padding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the Padding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializePadding()
		{
			return Padding != Padding.Empty;
		}

		#endregion

		#region Text

		/// <summary>
		/// Gets or sets the foreground color of a normal link
		/// </summary>
		[Description("The foreground color of a normal link")]
		public Color LinkColor
		{
			get
			{
				return linkNormal;
			}

			set
			{
				if (linkNormal != value)
				{
					linkNormal = value;

					if (TaskItem != null)
					{
						TaskItem.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the LinkColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the LinkColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeLinkColor()
		{
			return LinkColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the foreground color of a highlighted link
		/// </summary>
		[Description("The foreground color of a highlighted link")]
		public Color HotLinkColor
		{
			get
			{
				return linkHot;
			}

			set
			{
				if (linkHot != value)
				{
					linkHot = value;

					if (TaskItem != null)
					{
						TaskItem.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the HotLinkColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the HotLinkColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeHotLinkColor()
		{
			return HotLinkColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the font decoration of a link
		/// </summary>
		[DefaultValue(FontStyle.Underline),
		Description("")]
		public FontStyle FontDecoration
		{
			get
			{
				return fontDecoration;
			}

			set
			{
				if (!Enum.IsDefined(typeof(FontStyle), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(FontStyle));
				}

				if (fontDecoration != value)
				{
					fontDecoration = value;

					if (TaskItem != null)
					{
						TaskItem.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}

		#endregion

		#region TaskItem

		/// <summary>
		/// Gets or sets the TaskItem the TaskItemInfo belongs to
		/// </summary>
		protected internal TaskItem TaskItem
		{
			get
			{
				return owner;
			}
			
			set
			{
				owner = value;
			}
		}

		#endregion

		#endregion


		#region TaskItemInfoSurrogate

		/// <summary>
		/// A class that is serialized instead of a TaskItemInfo (as 
		/// TaskItemInfos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class TaskItemInfoSurrogate : ISerializable
		{
			#region Class Data
			
			/// <summary>
			/// See TaskItemInfo.Padding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding Padding; 
			
			/// <summary>
			/// See TaskItemInfo.Margin.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Margin Margin;
			
			/// <summary>
			/// See TaskItemInfo.LinkColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string LinkNormal;
			
			/// <summary>
			/// See TaskItemInfo.HotLinkColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string LinkHot;
			
			/// <summary>
			/// See TaskItemInfo.FontDecoration.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public FontStyle FontDecoration;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the TaskItemInfoSurrogate class with default settings
			/// </summary>
			public TaskItemInfoSurrogate()
			{
				Padding = Padding.Empty;
				Margin = Margin.Empty;

				LinkNormal = ThemeManager.ConvertColorToString(Color.Empty);
				LinkHot = ThemeManager.ConvertColorToString(Color.Empty);

				FontDecoration = FontStyle.Regular;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the TaskItemInfoSurrogate with data that is to be 
			/// serialized from the specified TaskItemInfo
			/// </summary>
			/// <param name="taskItemInfo">The TaskItemInfo that contains the data 
			/// to be serialized</param>
			public void Load(TaskItemInfo taskItemInfo)
			{
				Padding = taskItemInfo.Padding;
				Margin = taskItemInfo.Margin;

				LinkNormal = ThemeManager.ConvertColorToString(taskItemInfo.LinkColor);
				LinkHot = ThemeManager.ConvertColorToString(taskItemInfo.HotLinkColor);

				FontDecoration = taskItemInfo.FontDecoration;
			}


			/// <summary>
			/// Returns a TaskItemInfo that contains the deserialized TaskItemInfoSurrogate data
			/// </summary>
			/// <returns>A TaskItemInfo that contains the deserialized TaskItemInfoSurrogate data</returns>
			public TaskItemInfo Save()
			{
				TaskItemInfo taskItemInfo = new TaskItemInfo();

				taskItemInfo.Padding = Padding;
				taskItemInfo.Margin = Margin;

				taskItemInfo.LinkColor = ThemeManager.ConvertStringToColor(LinkNormal);
				taskItemInfo.HotLinkColor = ThemeManager.ConvertStringToColor(LinkHot);

				taskItemInfo.FontDecoration = FontDecoration;
				
				return taskItemInfo;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the TaskItemInfoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("Padding", Padding);
				info.AddValue("Margin", Margin);

				info.AddValue("LinkNormal", LinkNormal);
				info.AddValue("LinkHot", LinkHot);

				info.AddValue("FontDecoration", FontDecoration);
			}


			/// <summary>
			/// Initializes a new instance of the TaskItemInfoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the TaskItemInfoSurrogate</param>
			/// <param name="context">The source from which the TaskItemInfoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected TaskItemInfoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");
				
				Padding = (Padding) info.GetValue("Padding", typeof(Padding));
				Margin = (Margin) info.GetValue("Margin", typeof(Margin));
				
				LinkNormal = info.GetString("LinkNormal");
				LinkHot = info.GetString("LinkHot");

				FontDecoration = (FontStyle) info.GetValue("FontDecoration", typeof(FontStyle));
			}

			#endregion
		}

		#endregion
	}


	#region TaskItemInfoConverter

	/// <summary>
	/// A custom TypeConverter used to help convert TaskItemInfo from 
	/// one Type to another
	/// </summary>
	internal class TaskItemInfoConverter : ExpandableObjectConverter
	{
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
			if (destinationType == typeof(string) && value is TaskItemInfo)
			{
				return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	#endregion

	#endregion


	#region ExpandoInfo Class

	/// <summary>
	/// A class that contains system defined settings for Expandos
	/// </summary>
	public class ExpandoInfo : IDisposable
	{
		#region Class Data

		/// <summary>
		/// The background Color of an Expando that is a special group
		/// </summary>
		private Color specialBackColor;
		
		/// <summary>
		/// The background Color of an Expando that is a normal group
		/// </summary>
		private Color normalBackColor;

		/// <summary>
		/// The width of the Border along each edge of an Expando that 
		/// is a special group
		/// </summary>
		private Border specialBorder;
		
		/// <summary>
		/// The width of the Border along each edge of an Expando that 
		/// is a normal group
		/// </summary>
		private Border normalBorder;
		
		/// <summary>
		/// The Color of the Border an Expando that is a special group
		/// </summary>
		private Color specialBorderColor;
		
		/// <summary>
		/// The Color of the Border an Expando that is a normal group
		/// </summary>
		private Color normalBorderColor;

		/// <summary>
		/// The amount of space between the Border and items along 
		/// each edge of an Expando that is a special group
		/// </summary>
		private Padding specialPadding;
		
		/// <summary>
		/// The amount of space between the Border and items along 
		/// each edge of an Expando that is a normal group
		/// </summary>
		private Padding normalPadding;

		/// <summary>
		/// The alignment of the Image that is to be used as a watermark
		/// </summary>
		private ContentAlignment watermarkAlignment;

		/// <summary>
		/// The background image used for the content area of a special Expando
		/// </summary>
		private Image specialBackImage;

		/// <summary>
		/// The background image used for the content area of a normal Expando
		/// </summary>
		private Image normalBackImage;

		/// <summary>
		/// The Expando that the ExpandoInfo belongs to
		/// </summary>
		private Expando owner;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the ExpandoInfo class with default settings
		/// </summary>
		public ExpandoInfo()
		{
			// set background color values
			specialBackColor = Color.Transparent;
			normalBackColor = Color.Transparent;

			// set border values
			specialBorder = new Border(1, 0, 1, 1);
			specialBorderColor = Color.Transparent;

			normalBorder = new Border(1, 0, 1, 1);
			normalBorderColor = Color.Transparent;

			// set padding values
			specialPadding = new Padding(12, 10, 12, 10);
			normalPadding = new Padding(12, 10, 12, 10);

			specialBackImage = null;
			normalBackImage = null;

			watermarkAlignment = ContentAlignment.BottomRight;

			owner = null;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Forces the use of default values
		/// </summary>
		public void SetDefaultValues()
		{
			// set background color values
			specialBackColor = SystemColors.Window;
			normalBackColor = SystemColors.Window;

			// set border values
			specialBorder.Left = 1;
			specialBorder.Top = 0;
			specialBorder.Right = 1;
			specialBorder.Bottom = 1;

			specialBorderColor = SystemColors.Highlight;

			normalBorder.Left = 1;
			normalBorder.Top = 0;
			normalBorder.Right = 1;
			normalBorder.Bottom = 1;

			normalBorderColor = SystemColors.Control;

			// set padding values
			specialPadding.Left = 12;
			specialPadding.Top = 10;
			specialPadding.Right = 12;
			specialPadding.Bottom = 10;
			
			normalPadding.Left = 12;
			normalPadding.Top = 10;
			normalPadding.Right = 12;
			normalPadding.Bottom = 10;

			specialBackImage = null;
			normalBackImage = null;

			watermarkAlignment = ContentAlignment.BottomRight;
		}


		/// <summary>
		/// Forces the use of default empty values
		/// </summary>
		public void SetDefaultEmptyValues()
		{
			// set background color values
			specialBackColor = Color.Empty;
			normalBackColor = Color.Empty;

			// set border values
			specialBorder = Border.Empty;
			specialBorderColor = Color.Empty;

			normalBorder = Border.Empty;
			normalBorderColor = Color.Empty;

			// set padding values
			specialPadding = Padding.Empty;
			normalPadding = Padding.Empty;

			specialBackImage = null;
			normalBackImage = null;

			watermarkAlignment = ContentAlignment.BottomRight;
		}


		/// <summary>
		/// Releases all resources used by the ExpandoInfo
		/// </summary>
		public void Dispose()
		{
			if (specialBackImage != null)
			{
				specialBackImage.Dispose();
				specialBackImage = null;
			}

			if (normalBackImage != null)
			{
				normalBackImage.Dispose();
				normalBackImage = null;
			}
		}

		#endregion


		#region Properties

		#region Background

		/// <summary>
		/// Gets or sets the background color of a special expando
		/// </summary>
		[Description("The background color of a special Expando")]
		public Color SpecialBackColor
		{
			get
			{
				return specialBackColor;
			}

			set
			{
				if (specialBackColor != value)
				{
					specialBackColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBackColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBackColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBackColor()
		{
			return SpecialBackColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the background color of a normal expando
		/// </summary>
		[Description("The background color of a normal Expando")]
		public Color NormalBackColor
		{
			get
			{
				return normalBackColor;
			}

			set
			{
				if (normalBackColor != value)
				{
					normalBackColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBackColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBackColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBackColor()
		{
			return NormalBackColor != Color.Empty;
		}

		
		/// <summary>
		/// Gets or sets the alignment for the expando's background image
		/// </summary>
		[DefaultValue(ContentAlignment.BottomRight), 
		Description("The alignment for the expando's background image")]
		public ContentAlignment WatermarkAlignment
		{
			get
			{
				return watermarkAlignment;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ContentAlignment), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ContentAlignment));
				}

				if (watermarkAlignment != value)
				{
					watermarkAlignment = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a special expando's background image
		/// </summary>
		[DefaultValue(null), 
		Description("")]
		public Image SpecialBackImage
		{
			get
			{
				return specialBackImage;
			}

			set
			{
				if (specialBackImage != value)
				{
					specialBackImage = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets a normal expando's background image
		/// </summary>
		[DefaultValue(null), 
		Description("")]
		public Image NormalBackImage
		{
			get
			{
				return normalBackImage;
			}

			set
			{
				if (normalBackImage != value)
				{
					normalBackImage = value;
				}
			}
		}

		#endregion

		#region Border

		/// <summary>
		/// Gets or sets the border for a special expando
		/// </summary>
		[Description("The width of the Border along each side of a special Expando")]
		public Border SpecialBorder
		{
			get
			{
				return specialBorder;
			}

			set
			{
				if (specialBorder != value)
				{
					specialBorder = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBorder property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBorder property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBorder()
		{
			return SpecialBorder != Border.Empty;
		}


		/// <summary>
		/// Gets or sets the border for a normal expando
		/// </summary>
		[Description("The width of the Border along each side of a normal Expando")]
		public Border NormalBorder
		{
			get
			{
				return normalBorder;
			}

			set
			{
				if (normalBorder != value)
				{
					normalBorder = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBorder property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBorder property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBorder()
		{
			return NormalBorder != Border.Empty;
		}


		/// <summary>
		/// Gets or sets the border color for a special expando
		/// </summary>
		[Description("The border color for a special Expando")]
		public Color SpecialBorderColor
		{
			get
			{
				return specialBorderColor;
			}

			set
			{
				if (specialBorderColor != value)
				{
					specialBorderColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBorderColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBorderColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBorderColor()
		{
			return SpecialBorderColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the border color for a normal expando
		/// </summary>
		[Description("The border color for a normal Expando")]
		public Color NormalBorderColor
		{
			get
			{
				return normalBorderColor;
			}

			set
			{
				if (normalBorderColor != value)
				{
					normalBorderColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBorderColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBorderColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBorderColor()
		{
			return NormalBorderColor != Color.Empty;
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets or sets the padding value for a special expando
		/// </summary>
		[Description("The amount of space between the border and items along each side of a special Expando")]
		public Padding SpecialPadding
		{
			get
			{
				return specialPadding;
			}

			set
			{
				if (specialPadding != value)
				{
					specialPadding = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialPadding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialPadding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialPadding()
		{
			return SpecialPadding != Padding.Empty;
		}
		

		/// <summary>
		/// Gets or sets the padding value for a normal expando
		/// </summary>
		[Description("The amount of space between the border and items along each side of a normal Expando")]
		public Padding NormalPadding
		{
			get
			{
				return normalPadding;
			}

			set
			{
				if (normalPadding != value)
				{
					normalPadding = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalPadding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalPadding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalPadding()
		{
			return NormalPadding != Padding.Empty;
		}

		#endregion

		#region Expando

		/// <summary>
		/// Gets or sets the Expando that the ExpandoInfo belongs to
		/// </summary>
		protected internal Expando Expando
		{
			get
			{
				return owner;
			}

			set
			{
				owner = value;
			}
		}

		#endregion

		#endregion


		#region ExpandoInfoSurrogate

		/// <summary>
		/// A class that is serialized instead of an ExpandoInfo (as 
		/// ExpandoInfos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class ExpandoInfoSurrogate : ISerializable
		{
			#region Class Data
			
			/// <summary>
			/// See ExpandoInfo.SpecialBackColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialBackColor;
			
			/// <summary>
			/// See ExpandoInfo.NormalBackColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalBackColor;
			
			/// <summary>
			/// See ExpandoInfo.SpecialBorder.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Border SpecialBorder;
			
			/// <summary>
			/// See ExpandoInfo.NormalBorder.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Border NormalBorder;
			
			/// <summary>
			/// See ExpandoInfo.SpecialBorderColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialBorderColor;
			
			/// <summary>
			/// See ExpandoInfo.NormalBorderColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalBorderColor;
			
			/// <summary>
			/// See ExpandoInfo.SpecialPadding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding SpecialPadding;
			
			/// <summary>
			/// See ExpandoInfo.NormalPadding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding NormalPadding;
			
			/// <summary>
			/// See ExpandoInfo.SpecialBackImage.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialBackImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialBackImage;
			
			/// <summary>
			/// See ExpandoInfo.NormalBackImage.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalBackImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalBackImage;
			
			/// <summary>
			/// See ExpandoInfo.WatermarkAlignment.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public ContentAlignment WatermarkAlignment;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the ExpandoInfoSurrogate class with default settings
			/// </summary>
			public ExpandoInfoSurrogate()
			{
				SpecialBackColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalBackColor = ThemeManager.ConvertColorToString(Color.Empty);

				SpecialBorder = Border.Empty;
				NormalBorder = Border.Empty;

				SpecialBorderColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalBorderColor = ThemeManager.ConvertColorToString(Color.Empty);
				
				SpecialPadding = Padding.Empty;
				NormalPadding = Padding.Empty;

				SpecialBackImage = new byte[0];
				NormalBackImage = new byte[0];

				WatermarkAlignment = ContentAlignment.BottomRight;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the ExpandoInfoSurrogate with data that is to be 
			/// serialized from the specified ExpandoInfo
			/// </summary>
			/// <param name="expandoInfo">The ExpandoInfo that contains the data 
			/// to be serialized</param>
			public void Load(ExpandoInfo expandoInfo)
			{
				SpecialBackColor = ThemeManager.ConvertColorToString(expandoInfo.SpecialBackColor);
				NormalBackColor =ThemeManager.ConvertColorToString( expandoInfo.NormalBackColor);

				SpecialBorder = expandoInfo.SpecialBorder;
				NormalBorder = expandoInfo.NormalBorder;

				SpecialBorderColor = ThemeManager.ConvertColorToString(expandoInfo.SpecialBorderColor);
				NormalBorderColor = ThemeManager.ConvertColorToString(expandoInfo.NormalBorderColor);

				SpecialPadding = expandoInfo.SpecialPadding;
				NormalPadding = expandoInfo.NormalPadding;

				SpecialBackImage = ThemeManager.ConvertImageToByteArray(expandoInfo.SpecialBackImage);
				NormalBackImage = ThemeManager.ConvertImageToByteArray(expandoInfo.NormalBackImage);

				WatermarkAlignment = expandoInfo.WatermarkAlignment;
			}


			/// <summary>
			/// Returns an ExpandoInfo that contains the deserialized ExpandoInfoSurrogate data
			/// </summary>
			/// <returns>An ExpandoInfo that contains the deserialized ExpandoInfoSurrogate data</returns>
			public ExpandoInfo Save()
			{
				ExpandoInfo expandoInfo = new ExpandoInfo();

				expandoInfo.SpecialBackColor = ThemeManager.ConvertStringToColor(SpecialBackColor);
				expandoInfo.NormalBackColor = ThemeManager.ConvertStringToColor(NormalBackColor);

				expandoInfo.SpecialBorder = SpecialBorder;
				expandoInfo.NormalBorder = NormalBorder;

				expandoInfo.SpecialBorderColor = ThemeManager.ConvertStringToColor(SpecialBorderColor);
				expandoInfo.NormalBorderColor = ThemeManager.ConvertStringToColor(NormalBorderColor);
				
				expandoInfo.SpecialPadding = SpecialPadding;
				expandoInfo.NormalPadding = NormalPadding;

				expandoInfo.SpecialBackImage = ThemeManager.ConvertByteArrayToImage(SpecialBackImage);
				expandoInfo.NormalBackImage = ThemeManager.ConvertByteArrayToImage(NormalBackImage);
				
				expandoInfo.WatermarkAlignment = WatermarkAlignment;
				
				return expandoInfo;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the ExpandoInfoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);
				
				info.AddValue("SpecialBackColor", SpecialBackColor);
				info.AddValue("NormalBackColor", NormalBackColor);
				
				info.AddValue("SpecialBorder", SpecialBorder);
				info.AddValue("NormalBorder", NormalBorder);
				
				info.AddValue("SpecialBorderColor", SpecialBorderColor);
				info.AddValue("NormalBorderColor", NormalBorderColor);
				
				info.AddValue("SpecialPadding", SpecialPadding);
				info.AddValue("NormalPadding", NormalPadding);
				
				info.AddValue("SpecialBackImage", SpecialBackImage);
				info.AddValue("NormalBackImage", NormalBackImage);
				
				info.AddValue("WatermarkAlignment", WatermarkAlignment);
			}


			/// <summary>
			/// Initializes a new instance of the ExpandoInfoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the ExpandoInfoSurrogate</param>
			/// <param name="context">The source from which the ExpandoInfoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected ExpandoInfoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");

				SpecialBackColor = info.GetString("SpecialBackColor");
				NormalBackColor = info.GetString("NormalBackColor");

				SpecialBorder = (Border) info.GetValue("SpecialBorder", typeof(Border));
				NormalBorder = (Border) info.GetValue("NormalBorder", typeof(Border));

				SpecialBorderColor = info.GetString("SpecialBorderColor");
				NormalBorderColor = info.GetString("NormalBorderColor");

				SpecialPadding = (Padding) info.GetValue("SpecialPadding", typeof(Padding));
				NormalPadding = (Padding) info.GetValue("NormalPadding", typeof(Padding));

				SpecialBackImage = (byte[]) info.GetValue("SpecialBackImage", typeof(byte[]));
				NormalBackImage = (byte[]) info.GetValue("NormalBackImage", typeof(byte[]));

				WatermarkAlignment = (ContentAlignment) info.GetValue("WatermarkAlignment", typeof(ContentAlignment));
			}

			#endregion
		}

		#endregion
	}


	#region ExpandoInfoConverter

	/// <summary>
	/// A custom TypeConverter used to help convert ExpandoInfos from 
	/// one Type to another
	/// </summary>
	internal class ExpandoInfoConverter : ExpandableObjectConverter
	{
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
			if (destinationType == typeof(string) && value is ExpandoInfo)
			{
				return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}


		/// <summary>
		/// Returns a collection of properties for the type of array specified 
		/// by the value parameter, using the specified context and attributes
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="value">An Object that specifies the type of array for 
		/// which to get properties</param>
		/// <param name="attributes">An array of type Attribute that is used as 
		/// a filter</param>
		/// <returns>A PropertyDescriptorCollection with the properties that are 
		/// exposed for this data type, or a null reference if there are no 
		/// properties</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			// set the order in which the properties appear 
			// in the property window
			
			PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(ExpandoInfo), attributes);

			string[] s = new string[9];
			s[0] = "NormalBackColor";
			s[1] = "SpecialBackColor";
			s[2] = "NormalBorder";
			s[3] = "SpecialBorder";
			s[4] = "NormalBorderColor";
			s[5] = "SpecialBorderColor";
			s[6] = "NormalPadding";
			s[7] = "SpecialPadding";
			s[8] = "WatermarkAlignment";

			return collection.Sort(s);
		}
	}

	#endregion

	#endregion


	#region HeaderInfo Class

	/// <summary>
	/// A class that contains system defined settings for an Expando's 
	/// header section
	/// </summary>
	public class HeaderInfo : IDisposable
	{
		#region Class Data
		
		/// <summary>
		/// The Font used to draw the text on the title bar
		/// </summary>
		private Font titleFont;

		/// <summary>
		/// The Margin around the header
		/// </summary>
		private int margin;

		/// <summary>
		/// The Image used as the title bar's background for a special Expando
		/// </summary>
		private Image specialBackImage;

		/// <summary>
		/// The Image used as the title bar's background for a normal Expando
		/// </summary>
		private Image normalBackImage;

		/// <summary>
		///  The width of the Image used as the title bar's background
		/// </summary>
		private int backImageWidth;

		/// <summary>
		/// The height of the Image used as the title bar's background
		/// </summary>
		private int backImageHeight;

		/// <summary>
		/// The Color of the text on the title bar for a special Expando
		/// </summary>
		private Color specialTitle;
		
		/// <summary>
		/// The Color of the text on the title bar for a normal Expando
		/// </summary>
		private Color normalTitle;

		/// <summary>
		/// The Color of the text on the title bar for a special Expando 
		/// when highlighted
		/// </summary>
		private Color specialTitleHot;

		/// <summary>
		/// The Color of the text on the title bar for a normal Expando 
		/// when highlighted
		/// </summary>
		private Color normalTitleHot;

		/// <summary>
		/// The alignment of the text on the title bar for a special Expando
		/// </summary>
		private ContentAlignment specialAlignment;

		/// <summary>
		/// The alignment of the text on the title bar for a normal Expando
		/// </summary>
		private ContentAlignment normalAlignment;
		
		/// <summary>
		/// The amount of space between the border and items along 
		/// each edge of the title bar for a special Expando
		/// </summary>
		private Padding specialPadding;

		/// <summary>
		/// The amount of space between the border and items along 
		/// each edge of the title bar for a normal Expando
		/// </summary>
		private Padding normalPadding;

		/// <summary>
		/// The width of the Border along each edge of the title bar 
		/// for a special Expando
		/// </summary>
		private Border specialBorder;

		/// <summary>
		/// The width of the Border along each edge of the title bar 
		/// for a normal Expando
		/// </summary>
		private Border normalBorder;

		/// <summary>
		/// The Color of the title bar's Border for a special Expando
		/// </summary>
		private Color specialBorderColor;

		/// <summary>
		/// The Color of the title bar's Border for a normal Expando
		/// </summary>
		private Color normalBorderColor;

		/// <summary>
		/// The Color of the title bar's background for a special Expando
		/// </summary>
		private Color specialBackColor;

		/// <summary>
		/// The Color of the title bar's background for a normal Expando
		/// </summary>
		private Color normalBackColor;

		/// <summary>
		/// The Image that is used as a collapse arrow on the title bar 
		/// for a special Expando
		/// </summary>
		private Image specialArrowUp;
		
		/// <summary>
		/// The Image that is used as a collapse arrow on the title bar 
		/// for a special Expando when highlighted
		/// </summary>
		private Image specialArrowUpHot;
		
		/// <summary>
		/// The Image that is used as an expand arrow on the title bar 
		/// for a special Expando
		/// </summary>
		private Image specialArrowDown;
		
		/// <summary>
		/// The Image that is used as an expand arrow on the title bar 
		/// for a special Expando when highlighted
		/// </summary>
		private Image specialArrowDownHot;
		
		/// <summary>
		/// The Image that is used as a collapse arrow on the title bar 
		/// for a normal Expando
		/// </summary>
		private Image normalArrowUp;
		
		/// <summary>
		/// The Image that is used as a collapse arrow on the title bar 
		/// for a normal Expando when highlighted
		/// </summary>
		private Image normalArrowUpHot;
		
		/// <summary>
		/// The Image that is used as an expand arrow on the title bar 
		/// for a normal Expando
		/// </summary>
		private Image normalArrowDown;
		
		/// <summary>
		/// The Image that is used as an expand arrow on the title bar 
		/// for a normal Expando when highlighted
		/// </summary>
		private Image normalArrowDownHot;

		/// <summary>
		/// Specifies whether the title bar should use a gradient fill
		/// </summary>
		private bool useTitleGradient;

		/// <summary>
		/// The start Color of a title bar's gradient fill for a special 
		/// Expando
		/// </summary>
		private Color specialGradientStartColor;

		/// <summary>
		/// The end Color of a title bar's gradient fill for a special 
		/// Expando
		/// </summary>
		private Color specialGradientEndColor;

		/// <summary>
		/// The start Color of a title bar's gradient fill for a normal 
		/// Expando
		/// </summary>
		private Color normalGradientStartColor;

		/// <summary>
		/// The end Color of a title bar's gradient fill for a normal 
		/// Expando
		/// </summary>
		private Color normalGradientEndColor;

		/// <summary>
		/// How far along the title bar the gradient starts
		/// </summary>
		private float gradientOffset;

		/// <summary>
		/// The radius of the corners on the title bar
		/// </summary>
		private int titleRadius;

		/// <summary>
		/// The Expando that the HeaderInfo belongs to
		/// </summary>
		private Expando owner;

		/// <summary>
		/// 
		/// </summary>
		private bool rightToLeft;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the HeaderInfo class with default settings
		/// </summary>
		public HeaderInfo()
		{
			// work out the default font name for the user's os.
			// this ignores other fonts that may be specified - need 
			// to change parser to get font names
			if (Environment.OSVersion.Version.Major >= 5)
			{
				// Win2k, XP, Server 2003
				titleFont = new Font("Segoe UI", 8.25f, FontStyle.Bold);
			}
			else
			{
				// Win9x, ME, NT
				titleFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
			}

			margin = 15;

			// set title colors and alignment
			specialTitle = Color.Transparent;
			specialTitleHot = Color.Transparent;
			
			normalTitle = Color.Transparent;
			normalTitleHot = Color.Transparent;
			
			specialAlignment = ContentAlignment.MiddleLeft;
			normalAlignment = ContentAlignment.MiddleLeft;

			// set padding values
			specialPadding = new Padding(10, 0, 1, 0);
			normalPadding = new Padding(10, 0, 1, 0);

			// set border values
			specialBorder = new Border(2, 2, 2, 0);
			specialBorderColor = Color.Transparent;

			normalBorder = new Border(2, 2, 2, 0);
			normalBorderColor = Color.Transparent;
			
			specialBackColor = Color.Transparent;
			normalBackColor = Color.Transparent;

			// set background image values
			specialBackImage = null;
			normalBackImage = null;

			backImageWidth = -1;
			backImageHeight = -1;

			// set arrow values
			specialArrowUp = null;
			specialArrowUpHot = null;
			specialArrowDown = null;
			specialArrowDownHot = null;

			normalArrowUp = null;
			normalArrowUpHot = null;
			normalArrowDown = null;
			normalArrowDownHot = null;

			useTitleGradient = false;
			specialGradientStartColor = Color.White;
			specialGradientEndColor = SystemColors.Highlight;
			normalGradientStartColor = Color.White;
			normalGradientEndColor = SystemColors.Highlight;
			gradientOffset = 0.5f;
			titleRadius = 5;

			owner = null;
			rightToLeft = false;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Forces the use of default values
		/// </summary>
		public void SetDefaultValues()
		{
			// work out the default font name for the user's os
			if (Environment.OSVersion.Version.Major >= 5)
			{
				// Win2k, XP, Server 2003
				titleFont = new Font("Segoe UI", 8.25f, FontStyle.Bold);
			}
			else
			{
				// Win9x, ME, NT
				titleFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
			}

			margin = 15;

			// set title colors and alignment
			specialTitle = SystemColors.HighlightText;
			specialTitleHot = SystemColors.HighlightText;
			
			normalTitle = SystemColors.ControlText;
			normalTitleHot = SystemColors.ControlText;
			
			specialAlignment = ContentAlignment.MiddleLeft;
			normalAlignment = ContentAlignment.MiddleLeft;

			// set padding values
			specialPadding.Left = 10;
			specialPadding.Top = 0;
			specialPadding.Right = 1;
			specialPadding.Bottom = 0;

			normalPadding.Left = 10;
			normalPadding.Top = 0;
			normalPadding.Right = 1;
			normalPadding.Bottom = 0;

			// set border values
			specialBorder.Left = 2;
			specialBorder.Top = 2;
			specialBorder.Right = 2;
			specialBorder.Bottom = 0;

			specialBorderColor = SystemColors.Highlight;
			specialBackColor = SystemColors.Highlight;

			normalBorder.Left = 2;
			normalBorder.Top = 2;
			normalBorder.Right = 2;
			normalBorder.Bottom = 0;

			normalBorderColor = SystemColors.Control;
			normalBackColor = SystemColors.Control;

			// set background image values
			specialBackImage = null;
			normalBackImage = null;

			backImageWidth = 186;
			backImageHeight = 25;

			// set arrow values
			specialArrowUp = null;
			specialArrowUpHot = null;
			specialArrowDown = null;
			specialArrowDownHot = null;

			normalArrowUp = null;
			normalArrowUpHot = null;
			normalArrowDown = null;
			normalArrowDownHot = null;

			useTitleGradient = false;
			specialGradientStartColor = Color.White;
			specialGradientEndColor = SystemColors.Highlight;
			normalGradientStartColor = Color.White;
			normalGradientEndColor = SystemColors.Highlight;
			gradientOffset = 0.5f;
			titleRadius = 2;

			rightToLeft = false;
		}
		

		/// <summary>
		/// Forces the use of default empty values
		/// </summary>
		public void SetDefaultEmptyValues()
		{
			// work out the default font name for the user's os
			titleFont = null;

			margin = 15;

			// set title colors and alignment
			specialTitle = Color.Empty;
			specialTitleHot = Color.Empty;
			
			normalTitle = Color.Empty;
			normalTitleHot = Color.Empty;
			
			specialAlignment = ContentAlignment.MiddleLeft;
			normalAlignment = ContentAlignment.MiddleLeft;

			// set padding values
			specialPadding = Padding.Empty;
			normalPadding = Padding.Empty;

			// set border values
			specialBorder = Border.Empty;
			specialBorderColor = Color.Empty;
			specialBackColor = Color.Empty;

			normalBorder = Border.Empty;
			normalBorderColor = Color.Empty;
			normalBackColor = Color.Empty;

			// set background image values
			specialBackImage = null;
			normalBackImage = null;

			backImageWidth = 186;
			backImageHeight = 25;

			// set arrow values
			specialArrowUp = null;
			specialArrowUpHot = null;
			specialArrowDown = null;
			specialArrowDownHot = null;

			normalArrowUp = null;
			normalArrowUpHot = null;
			normalArrowDown = null;
			normalArrowDownHot = null;

			useTitleGradient = false;
			specialGradientStartColor = Color.Empty;
			specialGradientEndColor = Color.Empty;
			normalGradientStartColor = Color.Empty;
			normalGradientEndColor = Color.Empty;
			gradientOffset = 0.5f;
			titleRadius = 2;

			rightToLeft = false;
		}


		/// <summary>
		/// Releases all resources used by the HeaderInfo
		/// </summary>
		public void Dispose()
		{
			if (specialBackImage != null)
			{
				specialBackImage.Dispose();
				specialBackImage = null;
			}

			if (normalBackImage != null)
			{
				normalBackImage.Dispose();
				normalBackImage = null;
			}


			if (specialArrowUp != null)
			{
				specialArrowUp.Dispose();
				specialArrowUp = null;
			}

			if (specialArrowUpHot != null)
			{
				specialArrowUpHot.Dispose();
				specialArrowUpHot = null;
			}

			if (specialArrowDown != null)
			{
				specialArrowDown.Dispose();
				specialArrowDown = null;
			}

			if (specialArrowDownHot != null)
			{
				specialArrowDownHot.Dispose();
				specialArrowDownHot = null;
			}
			
			if (normalArrowUp != null)
			{
				normalArrowUp.Dispose();
				normalArrowUp = null;
			}

			if (normalArrowUpHot != null)
			{
				normalArrowUpHot.Dispose();
				normalArrowUpHot = null;
			}

			if (normalArrowDown != null)
			{
				normalArrowDown.Dispose();
				normalArrowDown = null;
			}

			if (normalArrowDownHot != null)
			{
				normalArrowDownHot.Dispose();
				normalArrowDownHot = null;
			}
		}

		#endregion


		#region Properties

		#region Border

		/// <summary>
		/// Gets or sets the border value for a special header
		/// </summary>
		[Description("The width of the border along each side of a special Expando's Title Bar")]
		public Border SpecialBorder
		{
			get
			{
				return specialBorder;
			}

			set
			{
				if (specialBorder != value)
				{
					specialBorder = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBorder property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBorder property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBorder()
		{
			return SpecialBorder != Border.Empty;
		}


		/// <summary>
		/// Gets or sets the border color for a special header
		/// </summary>
		[Description("The border color for a special Expandos titlebar")]
		public Color SpecialBorderColor
		{
			get
			{
				return specialBorderColor;
			}

			set
			{
				if (specialBorderColor != value)
				{
					specialBorderColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBorderColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBorderColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBorderColor()
		{
			return SpecialBorderColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the background Color for a special header
		/// </summary>
		[Description("The background Color for a special Expandos titlebar")]
		public Color SpecialBackColor
		{
			get
			{
				return specialBackColor;
			}

			set
			{
				if (specialBackColor != value)
				{
					specialBackColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialBackColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialBackColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialBackColor()
		{
			return SpecialBackColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the border value for a normal header
		/// </summary>
		[Description("The width of the border along each side of a normal Expando's Title Bar")]
		public Border NormalBorder
		{
			get
			{
				return normalBorder;
			}

			set
			{
				if (normalBorder != value)
				{
					normalBorder = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBorder property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBorder property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBorder()
		{
			return NormalBorder != Border.Empty;
		}


		/// <summary>
		/// Gets or sets the border color for a normal header
		/// </summary>
		[Description("The border color for a normal Expandos titlebar")]
		public Color NormalBorderColor
		{
			get
			{
				return normalBorderColor;
			}

			set
			{
				if (normalBorderColor != value)
				{
					normalBorderColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBorderColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBorderColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBorderColor()
		{
			return NormalBorderColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the background Color for a normal header
		/// </summary>
		[Description("The background Color for a normal Expandos titlebar")]
		public Color NormalBackColor
		{
			get
			{
				return normalBackColor;
			}

			set
			{
				if (normalBackColor != value)
				{
					normalBackColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalBackColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalBackColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalBackColor()
		{
			return NormalBackColor != Color.Empty;
		}

		#endregion

		#region Fonts

		/// <summary>
		/// Gets the Font used to render the header's text
		/// </summary>
		[DefaultValue(null), 
		Description("The Font used to render the titlebar's text")]
		public Font TitleFont
		{
			get
			{
				return titleFont;
			}

			set
			{
				if (titleFont != value)
				{
					titleFont = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the name of the font used to render the header's text. 
		/// </summary>
		protected internal string FontName
		{
			get
			{
				return TitleFont.Name;
			}

			set
			{
				TitleFont = new Font(value, TitleFont.SizeInPoints, TitleFont.Style);
			}
		}


		/// <summary>
		/// Gets or sets the size of the font used to render the header's text. 
		/// </summary>
		protected internal float FontSize
		{
			get
			{
				return TitleFont.SizeInPoints;
			}

			set
			{
				TitleFont = new Font(TitleFont.Name, value, TitleFont.Style);
			}
		}


		/// <summary>
		/// Gets or sets the weight of the font used to render the header's text. 
		/// </summary>
		protected internal FontStyle FontWeight
		{
			get
			{
				return TitleFont.Style;
			}

			set
			{
				value |= TitleFont.Style;
				
				TitleFont = new Font(TitleFont.Name, TitleFont.SizeInPoints, value);
			}
		}
		
		
		/// <summary>
		/// Gets or sets the style of the Font used to render the header's text. 
		/// </summary>
		protected internal FontStyle FontStyle
		{
			get
			{
				return TitleFont.Style;
			}

			set
			{
				value |= TitleFont.Style;
				
				TitleFont = new Font(TitleFont.Name, TitleFont.SizeInPoints, value);
			}
		}

		#endregion

		#region Images

		/// <summary>
		/// Gets or sets the background image for a special header
		/// </summary>
		[DefaultValue(null), 
		Description("The background image for a special titlebar")]
		public Image SpecialBackImage
		{
			get
			{
				return specialBackImage;
			}

			set
			{
				if (specialBackImage != value)
				{
					specialBackImage = value;

					if (value!= null)
					{
						backImageWidth = value.Width;
						backImageHeight = value.Height;
					}

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the background image for a normal header
		/// </summary>
		[DefaultValue(null), 
		Description("The background image for a normal titlebar")]
		public Image NormalBackImage
		{
			get
			{
				return normalBackImage;
			}

			set
			{
				if (normalBackImage != value)
				{
					normalBackImage = value;

					if (value!= null)
					{
						backImageWidth = value.Width;
						backImageHeight = value.Height;
					}

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the width of the header's background image
		/// </summary>
		protected internal int BackImageWidth
		{
			get
			{
				if (backImageWidth == -1)
				{
					return 186;
				}
				
				return backImageWidth;
			}

			set
			{
				backImageWidth = value;
			}
		}


		/// <summary>
		/// Gets or sets the height of the header's background image
		/// </summary>
		protected internal int BackImageHeight
		{
			get
			{
				if (backImageHeight < 23)
				{
					return 23;
				}
				
				return backImageHeight;
			}

			set
			{
				backImageHeight = value;
			}
		}
		
		
		/// <summary>
		/// Gets or sets a special header's collapse arrow image in it's normal state
		/// </summary>
		[DefaultValue(null), 
		Description("A special Expando's collapse arrow image in it's normal state")]
		public Image SpecialArrowUp
		{
			get
			{
				return specialArrowUp;
			}

			set
			{
				if (specialArrowUp != value)
				{
					specialArrowUp = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a special header's collapse arrow image in it's highlighted state
		/// </summary>
		[DefaultValue(null), 
		Description("A special Expando's collapse arrow image in it's highlighted state")]
		public Image SpecialArrowUpHot
		{
			get
			{
				return specialArrowUpHot;
			}

			set
			{
				if (specialArrowUpHot != value)
				{
					specialArrowUpHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a special header's expand arrow image in it's normal state
		/// </summary>
		[DefaultValue(null), 
		Description("A special Expando's expand arrow image in it's normal state")]
		public Image SpecialArrowDown
		{
			get
			{
				return specialArrowDown;
			}

			set
			{
				if (specialArrowDown != value)
				{
					specialArrowDown = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a special header's expend arrow image in it's highlighted state
		/// </summary>
		[DefaultValue(null), 
		Description("A special Expando's expand arrow image in it's highlighted state")]
		public Image SpecialArrowDownHot
		{
			get
			{
				return specialArrowDownHot;
			}

			set
			{
				if (specialArrowDownHot != value)
				{
					specialArrowDownHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}
		
		
		/// <summary>
		/// Gets or sets a normal header's collapse arrow image in it's normal state
		/// </summary>
		[DefaultValue(null), 
		Description("A normal Expando's collapse arrow image in it's normal state")]
		public Image NormalArrowUp
		{
			get
			{
				return normalArrowUp;
			}

			set
			{
				if (normalArrowUp != value)
				{
					normalArrowUp = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a normal header's collapse arrow image in it's highlighted state
		/// </summary>
		[DefaultValue(null), 
		Description("A normal Expando's collapse arrow image in it's highlighted state")]
		public Image NormalArrowUpHot
		{
			get
			{
				return normalArrowUpHot;
			}

			set
			{
				if (normalArrowUpHot != value)
				{
					normalArrowUpHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a normal header's expand arrow image in it's normal state
		/// </summary>
		[DefaultValue(null), 
		Description("A normal Expando's expand arrow image in it's normal state")]
		public Image NormalArrowDown
		{
			get
			{
				return normalArrowDown;
			}

			set
			{
				if (normalArrowDown != value)
				{
					normalArrowDown = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a normal header's expand arrow image in it's highlighted state
		/// </summary>
		[DefaultValue(null), 
		Description("A normal Expando's expand arrow image in it's highlighted state")]
		public Image NormalArrowDownHot
		{
			get
			{
				return normalArrowDownHot;
			}

			set
			{
				if (normalArrowDownHot != value)
				{
					normalArrowDownHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Sets the arrow images for use when theming is not supported
		/// </summary>
		internal void SetUnthemedArrowImages()
		{
			// get the arrow images resource
			Assembly myAssembly;
			myAssembly = GetType().Assembly;
			ResourceManager myManager = new ResourceManager("XPExplorerBar.ExpandoArrows", myAssembly);
				
			// set the arrow images
			specialArrowDown = new Bitmap((Image) myManager.GetObject("SPECIALGROUPEXPAND"));
			specialArrowDownHot = new Bitmap((Image) myManager.GetObject("SPECIALGROUPEXPANDHOT"));
			specialArrowUp = new Bitmap((Image) myManager.GetObject("SPECIALGROUPCOLLAPSE"));
			specialArrowUpHot = new Bitmap((Image) myManager.GetObject("SPECIALGROUPCOLLAPSEHOT"));
				
			normalArrowDown = new Bitmap((Image) myManager.GetObject("NORMALGROUPEXPAND"));
			normalArrowDownHot = new Bitmap((Image) myManager.GetObject("NORMALGROUPEXPANDHOT"));
			normalArrowUp = new Bitmap((Image) myManager.GetObject("NORMALGROUPCOLLAPSE"));
			normalArrowUpHot = new Bitmap((Image) myManager.GetObject("NORMALGROUPCOLLAPSEHOT"));
		}

		#endregion

		#region Margin

		/// <summary>
		/// Gets or sets the margin around the header
		/// </summary>
		[DefaultValue(15), 
		Description("The margin around the titlebar")]
		public int Margin
		{
			get
			{
				return margin;
			}

			set
			{
				if (margin != value)
				{
					margin = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}

		#endregion

		#region Padding

		/// <summary>
		/// Gets or sets the padding for a special header
		/// </summary>
		[Description("The amount of space between the border and items along each side of a special Expandos Title Bar")]
		public Padding SpecialPadding
		{
			get
			{
				return specialPadding;
			}

			set
			{
				if (specialPadding != value)
				{
					specialPadding = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialPadding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialPadding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialPadding()
		{
			return SpecialPadding != Padding.Empty;
		}


		/// <summary>
		/// Gets or sets the padding for a normal header
		/// </summary>
		[Description("The amount of space between the border and items along each side of a normal Expandos Title Bar")]
		public Padding NormalPadding
		{
			get
			{
				return normalPadding;
			}

			set
			{
				if (normalPadding != value)
				{
					normalPadding = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalPadding property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalPadding property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalPadding()
		{
			return NormalPadding != Padding.Empty;
		}

		#endregion

		#region Title

		/// <summary>
		/// Gets or sets the color of the text displayed in a special 
		/// header in it's normal state
		/// </summary>
		[Description("The color of the text displayed in a special Expandos titlebar in it's normal state")]
		public Color SpecialTitleColor
		{
			get
			{
				return specialTitle;
			}

			set
			{
				if (specialTitle != value)
				{
					specialTitle = value;

					// set the SpecialTitleHotColor as well just in case
					// it isn't/wasn't set during UIFILE parsing
					if (SpecialTitleHotColor == Color.Transparent)
					{
						SpecialTitleHotColor = value;
					}

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialTitleColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialTitleColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialTitleColor()
		{
			return SpecialTitleColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the color of the text displayed in a special 
		/// header in it's highlighted state
		/// </summary>
		[Description("The color of the text displayed in a special Expandos titlebar in it's highlighted state")]
		public Color SpecialTitleHotColor
		{
			get
			{
				return specialTitleHot;
			}

			set
			{
				if (specialTitleHot != value)
				{
					specialTitleHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialTitleHotColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialTitleHotColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialTitleHotColor()
		{
			return SpecialTitleHotColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the color of the text displayed in a normal 
		/// header in it's normal state
		/// </summary>
		[Description("The color of the text displayed in a normal Expandos titlebar in it's normal state")]
		public Color NormalTitleColor
		{
			get
			{
				return normalTitle;
			}

			set
			{
				if (normalTitle != value)
				{
					normalTitle = value;

					// set the NormalTitleHotColor as well just in case
					// it isn't/wasn't set during UIFILE parsing
					if (NormalTitleHotColor == Color.Transparent)
					{
						NormalTitleHotColor = value;
					}

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalTitleColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalTitleColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalTitleColor()
		{
			return NormalTitleColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the color of the text displayed in a normal 
		/// header in it's highlighted state
		/// </summary>
		[Description("The color of the text displayed in a normal Expandos titlebar in it's highlighted state")]
		public Color NormalTitleHotColor
		{
			get
			{
				return normalTitleHot;
			}

			set
			{
				if (normalTitleHot != value)
				{
					normalTitleHot = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalTitleHotColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalTitleHotColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalTitleHotColor()
		{
			return NormalTitleHotColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the alignment of the text displayed in a special header
		/// </summary>
		[DefaultValue(ContentAlignment.MiddleLeft), 
		Description("The alignment of the text displayed in a special Expandos titlebar")]
		public ContentAlignment SpecialAlignment
		{
			get
			{
				return specialAlignment;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ContentAlignment), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ContentAlignment));
				}

				if (specialAlignment != value)
				{
					specialAlignment = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the alignment of the text displayed in a normal header
		/// </summary>
		[DefaultValue(ContentAlignment.MiddleLeft), 
		Description("The alignment of the text displayed in a normal Expandos titlebar")]
		public ContentAlignment NormalAlignment
		{
			get
			{
				return normalAlignment;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ContentAlignment), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ContentAlignment));
				}

				if (normalAlignment != value)
				{
					normalAlignment = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the header's background should use a gradient fill
		/// </summary>
		[DefaultValue(false),
		Description("")]
		public bool TitleGradient
		{
			get
			{
				return useTitleGradient;
			}

			set
			{
				if (useTitleGradient != value)
				{
					useTitleGradient = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the start Color of a header's gradient fill for a special 
		/// Expando
		/// </summary>
		[Description("")]
		public Color SpecialGradientStartColor
		{
			get
			{
				return specialGradientStartColor;
			}

			set
			{
				if (specialGradientStartColor != value)
				{
					specialGradientStartColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialGradientStartColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialGradientStartColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialGradientStartColor()
		{
			return SpecialGradientStartColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the end Color of a header's gradient fill for a special 
		/// Expando
		/// </summary>
		[Description("")]
		public Color SpecialGradientEndColor
		{
			get
			{
				return specialGradientEndColor;
			}

			set
			{
				if (specialGradientEndColor != value)
				{
					specialGradientEndColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the SpecialGradientEndColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the SpecialGradientEndColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeSpecialGradientEndColor()
		{
			return SpecialGradientEndColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the start Color of a header's gradient fill for a normal 
		/// Expando
		/// </summary>
		[Description("")]
		public Color NormalGradientStartColor
		{
			get
			{
				return normalGradientStartColor;
			}

			set
			{
				if (normalGradientStartColor != value)
				{
					normalGradientStartColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalGradientStartColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalGradientStartColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalGradientStartColor()
		{
			return NormalGradientStartColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets the end Color of a header's gradient fill for a normal 
		/// Expando
		/// </summary>
		[Description("")]
		public Color NormalGradientEndColor
		{
			get
			{
				return normalGradientEndColor;
			}

			set
			{
				if (normalGradientEndColor != value)
				{
					normalGradientEndColor = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		/// Specifies whether the NormalGradientEndColor property should be 
		/// serialized at design time
		/// </summary>
		/// <returns>true if the NormalGradientEndColor property should be 
		/// serialized, false otherwise</returns>
		private bool ShouldSerializeNormalGradientEndColor()
		{
			return NormalGradientEndColor != Color.Empty;
		}


		/// <summary>
		/// Gets or sets how far along the header the gradient starts
		/// </summary>
		[DefaultValue(0.5f),
		Description("")]
		public float GradientOffset
		{
			get
			{
				return gradientOffset;
			}

			set
			{
				if (value < 0)
				{
					value = 0f;
				}
				else if (value > 1)
				{
					value = 1f;
				}
				
				if (gradientOffset != value)
				{
					gradientOffset = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}


		/// <summary>
		///Gets or sets the radius of the corners on the header
		/// </summary>
		[DefaultValue(2),
		Description("")]
		public int TitleRadius
		{
			get
			{
				return titleRadius;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > BackImageHeight)
				{
					value = BackImageHeight;
				}
				
				if (titleRadius != value)
				{
					titleRadius = value;

					if (Expando != null)
					{
						Expando.FireCustomSettingsChanged(EventArgs.Empty);
					}
				}
			}
		}

		#endregion

		#region Expando

		/// <summary>
		/// Gets or sets the Expando the HeaderInfo belongs to
		/// </summary>
		protected internal Expando Expando
		{
			get
			{
				return owner;
			}

			set
			{
				owner = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		internal bool RightToLeft
		{
			get
			{
				return rightToLeft;
			}

			set
			{
				rightToLeft = value;
			}
		}

		#endregion

		#endregion


		#region HeaderInfoSurrogate

		/// <summary>
		/// A class that is serialized instead of a HeaderInfo (as 
		/// HeaderInfos contain objects that cause serialization problems)
		/// </summary>
		[Serializable]
			public class HeaderInfoSurrogate : ISerializable
		{
			#region Class Data
			
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
			public FontStyle FontStyle;
			
			/// <summary>
			/// See HeaderInfo.Margin.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public int Margin;
			
			/// <summary>
			/// See HeaderInfo.SpecialBackImage.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialBackImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialBackImage;
			
			/// <summary>
			/// See HeaderInfo.NormalBackImage.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalBackImage", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalBackImage;
			
			/// <summary>
			/// See HeaderInfo.SpecialTitle.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialTitle;
			
			/// <summary>
			/// See HeaderInfo.NormalTitle.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalTitle;
			
			/// <summary>
			/// See HeaderInfo.SpecialTitleHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialTitleHot;
			
			/// <summary>
			/// See HeaderInfo.NormalTitleHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalTitleHot;
			
			/// <summary>
			/// See HeaderInfo.SpecialAlignment.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public ContentAlignment SpecialAlignment;
			
			/// <summary>
			/// See HeaderInfo.NormalAlignment.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public ContentAlignment NormalAlignment;
			
			/// <summary>
			/// See HeaderInfo.SpecialPadding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding SpecialPadding;
			
			/// <summary>
			/// See HeaderInfo.NormalPadding.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Padding NormalPadding;
			
			/// <summary>
			/// See HeaderInfo.SpecialBorder.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Border SpecialBorder;
			
			/// <summary>
			/// See HeaderInfo.NormalBorder.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public Border NormalBorder;
			
			/// <summary>
			/// See HeaderInfo.SpecialBorderColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialBorderColor;
			
			/// <summary>
			/// See HeaderInfo.NormalBorderColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalBorderColor;
			
			/// <summary>
			/// See HeaderInfo.SpecialBackColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialBackColor;
			
			/// <summary>
			/// See HeaderInfo.NormalBackColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalBackColor;
			
			/// <summary>
			/// See HeaderInfo.SpecialArrowUp.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialArrowUp", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialArrowUp;
			
			/// <summary>
			/// See HeaderInfo.SpecialArrowUpHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialArrowUpHot", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialArrowUpHot;
			
			/// <summary>
			/// See HeaderInfo.SpecialArrowDown.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialArrowDown", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialArrowDown;
			
			/// <summary>
			/// See HeaderInfo.SpecialArrowDownHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("SpecialArrowDownHot", typeof(Byte[]), DataType="base64Binary")]
			public byte[] SpecialArrowDownHot;
			
			/// <summary>
			/// See HeaderInfo.NormalArrowUp.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalArrowUp", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalArrowUp;
			
			/// <summary>
			/// See HeaderInfo.NormalArrowUpHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalArrowUpHot", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalArrowUpHot;
			
			/// <summary>
			/// See HeaderInfo.NormalArrowDown.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalArrowDown", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalArrowDown;
			
			/// <summary>
			/// See HeaderInfo.NormalArrowDownHot.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			[XmlElementAttribute("NormalArrowDownHot", typeof(Byte[]), DataType="base64Binary")]
			public byte[] NormalArrowDownHot;
			
			/// <summary>
			/// See HeaderInfo.TitleGradient.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public bool TitleGradient;
			
			/// <summary>
			/// See HeaderInfo.SpecialGradientStartColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialGradientStartColor;
			
			/// <summary>
			/// See HeaderInfo.SpecialGradientEndColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string SpecialGradientEndColor;
			
			/// <summary>
			/// See HeaderInfo.NormalGradientStartColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalGradientStartColor;
			
			/// <summary>
			/// See HeaderInfo.NormalGradientEndColor.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public string NormalGradientEndColor;
			
			/// <summary>
			/// See HeaderInfo.GradientOffset.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public float GradientOffset;
			
			/// <summary>
			/// See HeaderInfo.TitleRadius.  This member is not 
			/// intended to be used directly from your code.
			/// </summary>
			public int TitleRadius;

			/// <summary>
			/// Version number of the surrogate.  This member is not intended 
			/// to be used directly from your code.
			/// </summary>
			public int Version = 3300;

			#endregion


			#region Constructor

			/// <summary>
			/// Initializes a new instance of the HeaderInfoSurrogate class with default settings
			/// </summary>
			public HeaderInfoSurrogate()
			{
				FontName = null;
				FontSize = 8.25f;
				FontStyle = FontStyle.Regular;
				Margin = 15;

				SpecialBackImage = new byte[0];
				NormalBackImage = new byte[0];

				SpecialTitle = ThemeManager.ConvertColorToString(Color.Empty);
				NormalTitle = ThemeManager.ConvertColorToString(Color.Empty);
				SpecialTitleHot = ThemeManager.ConvertColorToString(Color.Empty);
				NormalTitleHot = ThemeManager.ConvertColorToString(Color.Empty);

				SpecialAlignment = ContentAlignment.MiddleLeft;
				NormalAlignment = ContentAlignment.MiddleLeft;

				SpecialPadding = Padding.Empty;
				NormalPadding = Padding.Empty;

				SpecialBorder = Border.Empty;
				NormalBorder = Border.Empty;
				SpecialBorderColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalBorderColor = ThemeManager.ConvertColorToString(Color.Empty);
				
				SpecialBackColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalBackColor = ThemeManager.ConvertColorToString(Color.Empty);

				SpecialArrowUp = new byte[0];
				SpecialArrowUpHot = new byte[0];
				SpecialArrowDown = new byte[0];
				SpecialArrowDownHot = new byte[0];
				NormalArrowUp = new byte[0];
				NormalArrowUpHot = new byte[0];
				NormalArrowDown = new byte[0];
				NormalArrowDownHot = new byte[0];

				TitleGradient = false;
				SpecialGradientStartColor = ThemeManager.ConvertColorToString(Color.Empty);
				SpecialGradientEndColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalGradientStartColor = ThemeManager.ConvertColorToString(Color.Empty);
				NormalGradientEndColor = ThemeManager.ConvertColorToString(Color.Empty);
				GradientOffset = 0.5f;
			}

			#endregion


			#region Methods

			/// <summary>
			/// Populates the HeaderInfoSurrogate with data that is to be 
			/// serialized from the specified HeaderInfo
			/// </summary>
			/// <param name="headerInfo">The HeaderInfo that contains the data 
			/// to be serialized</param>
			public void Load(HeaderInfo headerInfo)
			{
				if (headerInfo.TitleFont != null)
				{
					FontName = headerInfo.TitleFont.Name;
					FontSize = headerInfo.TitleFont.SizeInPoints;
					FontStyle = headerInfo.TitleFont.Style;
				}

				Margin = headerInfo.Margin;

				SpecialBackImage = ThemeManager.ConvertImageToByteArray(headerInfo.SpecialBackImage);
				NormalBackImage = ThemeManager.ConvertImageToByteArray(headerInfo.NormalBackImage);

				SpecialTitle = ThemeManager.ConvertColorToString(headerInfo.SpecialTitleColor);
				NormalTitle = ThemeManager.ConvertColorToString(headerInfo.NormalTitleColor);
				SpecialTitleHot = ThemeManager.ConvertColorToString(headerInfo.SpecialTitleHotColor);
				NormalTitleHot = ThemeManager.ConvertColorToString(headerInfo.NormalTitleHotColor);

				SpecialAlignment = headerInfo.SpecialAlignment;
				NormalAlignment = headerInfo.NormalAlignment;

				SpecialPadding = headerInfo.SpecialPadding;
				NormalPadding = headerInfo.NormalPadding;

				SpecialBorder = headerInfo.SpecialBorder;
				NormalBorder = headerInfo.NormalBorder;
				SpecialBorderColor = ThemeManager.ConvertColorToString(headerInfo.SpecialBorderColor);
				NormalBorderColor = ThemeManager.ConvertColorToString(headerInfo.NormalBorderColor);
				
				SpecialBackColor = ThemeManager.ConvertColorToString(headerInfo.SpecialBackColor);
				NormalBackColor = ThemeManager.ConvertColorToString(headerInfo.NormalBackColor);

				SpecialArrowUp = ThemeManager.ConvertImageToByteArray(headerInfo.SpecialArrowUp);
				SpecialArrowUpHot = ThemeManager.ConvertImageToByteArray(headerInfo.SpecialArrowUpHot);
				SpecialArrowDown = ThemeManager.ConvertImageToByteArray(headerInfo.SpecialArrowDown);
				SpecialArrowDownHot = ThemeManager.ConvertImageToByteArray(headerInfo.SpecialArrowDownHot);
				NormalArrowUp = ThemeManager.ConvertImageToByteArray(headerInfo.NormalArrowUp);
				NormalArrowUpHot = ThemeManager.ConvertImageToByteArray(headerInfo.NormalArrowUpHot);
				NormalArrowDown = ThemeManager.ConvertImageToByteArray(headerInfo.NormalArrowDown);
				NormalArrowDownHot = ThemeManager.ConvertImageToByteArray(headerInfo.NormalArrowDownHot);

				TitleGradient = headerInfo.TitleGradient;
				SpecialGradientStartColor = ThemeManager.ConvertColorToString(headerInfo.SpecialGradientStartColor);
				SpecialGradientEndColor = ThemeManager.ConvertColorToString(headerInfo.SpecialGradientEndColor);
				NormalGradientStartColor = ThemeManager.ConvertColorToString(headerInfo.NormalGradientStartColor);
				NormalGradientEndColor = ThemeManager.ConvertColorToString(headerInfo.NormalGradientEndColor);
				GradientOffset = headerInfo.GradientOffset;
			}


			/// <summary>
			/// Returns a HeaderInfo that contains the deserialized HeaderInfoSurrogate data
			/// </summary>
			/// <returns>A HeaderInfo that contains the deserialized HeaderInfoSurrogate data</returns>
			public HeaderInfo Save()
			{
				HeaderInfo headerInfo = new HeaderInfo();

				if (FontName != null)
				{
					headerInfo.TitleFont = new Font(FontName, FontSize, FontStyle);
				}

				headerInfo.Margin = Margin;

				headerInfo.SpecialBackImage = ThemeManager.ConvertByteArrayToImage(SpecialBackImage);
				headerInfo.NormalBackImage = ThemeManager.ConvertByteArrayToImage(NormalBackImage);

				headerInfo.SpecialTitleColor = ThemeManager.ConvertStringToColor(SpecialTitle);
				headerInfo.NormalTitleColor = ThemeManager.ConvertStringToColor(NormalTitle);
				headerInfo.SpecialTitleHotColor = ThemeManager.ConvertStringToColor(SpecialTitleHot);
				headerInfo.NormalTitleHotColor = ThemeManager.ConvertStringToColor(NormalTitleHot);

				headerInfo.SpecialAlignment = SpecialAlignment;
				headerInfo.NormalAlignment = NormalAlignment;
				
				headerInfo.SpecialPadding = SpecialPadding;
				headerInfo.NormalPadding = NormalPadding;

				headerInfo.SpecialBorder = SpecialBorder;
				headerInfo.NormalBorder = NormalBorder;
				headerInfo.SpecialBorderColor = ThemeManager.ConvertStringToColor(SpecialBorderColor);
				headerInfo.NormalBorderColor = ThemeManager.ConvertStringToColor(NormalBorderColor);

				headerInfo.SpecialBackColor = ThemeManager.ConvertStringToColor(SpecialBackColor);
				headerInfo.NormalBackColor = ThemeManager.ConvertStringToColor(NormalBackColor);

				headerInfo.SpecialArrowUp = ThemeManager.ConvertByteArrayToImage(SpecialArrowUp);
				headerInfo.SpecialArrowUpHot = ThemeManager.ConvertByteArrayToImage(SpecialArrowUpHot);
				headerInfo.SpecialArrowDown = ThemeManager.ConvertByteArrayToImage(SpecialArrowDown);
				headerInfo.SpecialArrowDownHot = ThemeManager.ConvertByteArrayToImage(SpecialArrowDownHot);
				headerInfo.NormalArrowUp = ThemeManager.ConvertByteArrayToImage(NormalArrowUp);
				headerInfo.NormalArrowUpHot = ThemeManager.ConvertByteArrayToImage(NormalArrowUpHot);
				headerInfo.NormalArrowDown = ThemeManager.ConvertByteArrayToImage(NormalArrowDown);
				headerInfo.NormalArrowDownHot = ThemeManager.ConvertByteArrayToImage(NormalArrowDownHot);

				headerInfo.TitleGradient = TitleGradient;
				headerInfo.SpecialGradientStartColor = ThemeManager.ConvertStringToColor(SpecialGradientStartColor);
				headerInfo.SpecialGradientEndColor = ThemeManager.ConvertStringToColor(SpecialGradientEndColor);
				headerInfo.NormalGradientStartColor = ThemeManager.ConvertStringToColor(NormalGradientStartColor);
				headerInfo.NormalGradientEndColor = ThemeManager.ConvertStringToColor(NormalGradientEndColor);
				headerInfo.GradientOffset = GradientOffset;
				
				return headerInfo;
			}


			/// <summary>
			/// Populates a SerializationInfo with the data needed to serialize the HeaderInfoSurrogate
			/// </summary>
			/// <param name="info">The SerializationInfo to populate with data</param>
			/// <param name="context">The destination for this serialization</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("Version", Version);

				info.AddValue("FontName", FontName);
				info.AddValue("FontSize", FontSize);
				info.AddValue("FontStyle", FontStyle);

				info.AddValue("Margin", Margin);

				info.AddValue("SpecialBackImage", SpecialBackImage);
				info.AddValue("NormalBackImage", NormalBackImage);

				info.AddValue("SpecialTitle", SpecialTitle);
				info.AddValue("NormalTitle", NormalTitle);
				info.AddValue("SpecialTitleHot", SpecialTitleHot);
				info.AddValue("NormalTitleHot", NormalTitleHot);

				info.AddValue("SpecialAlignment", SpecialAlignment);
				info.AddValue("NormalAlignment", NormalAlignment);

				info.AddValue("SpecialPadding", SpecialPadding);
				info.AddValue("NormalPadding", NormalPadding);

				info.AddValue("SpecialBorder", SpecialBorder);
				info.AddValue("NormalBorder", NormalBorder);
				info.AddValue("SpecialBorderColor", SpecialBorderColor);
				info.AddValue("NormalBorderColor", NormalBorderColor);

				info.AddValue("SpecialBackColor", SpecialBackColor);
				info.AddValue("NormalBackColor", NormalBackColor);

				info.AddValue("SpecialArrowUp", SpecialArrowUp);
				info.AddValue("SpecialArrowUpHot", SpecialArrowUpHot);
				info.AddValue("SpecialArrowDown", SpecialArrowDown);
				info.AddValue("SpecialArrowDownHot", SpecialArrowDownHot);
				info.AddValue("NormalArrowUp", NormalArrowUp);
				info.AddValue("NormalArrowUpHot", NormalArrowUpHot);
				info.AddValue("NormalArrowDown", NormalArrowDown);
				info.AddValue("NormalArrowDownHot", NormalArrowDownHot);

				info.AddValue("TitleGradient", TitleGradient);
				info.AddValue("SpecialGradientStartColor", SpecialGradientStartColor);
				info.AddValue("SpecialGradientEndColor", SpecialGradientEndColor);
				info.AddValue("NormalGradientStartColor", NormalGradientStartColor);
				info.AddValue("NormalGradientEndColor", NormalGradientEndColor);
				info.AddValue("GradientOffset", GradientOffset);
			}


			/// <summary>
			/// Initializes a new instance of the HeaderInfoSurrogate class using the information 
			/// in the SerializationInfo
			/// </summary>
			/// <param name="info">The information to populate the HeaderInfoSurrogate</param>
			/// <param name="context">The source from which the HeaderInfoSurrogate is deserialized</param>
			[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
			protected HeaderInfoSurrogate(SerializationInfo info, StreamingContext context)
			{
				int version = info.GetInt32("Version");
				
				FontName = info.GetString("FontName");
				FontSize = info.GetSingle("FontSize");
				FontStyle = (FontStyle) info.GetValue("FontStyle", typeof(FontStyle));

				Margin = info.GetInt32("Margin");
				
				SpecialBackImage = (byte[]) info.GetValue("SpecialBackImage", typeof(byte[]));
				NormalBackImage = (byte[]) info.GetValue("NormalBackImage", typeof(byte[]));
				
				SpecialTitle = info.GetString("SpecialTitle");
				NormalTitle = info.GetString("NormalTitle");
				SpecialTitleHot = info.GetString("SpecialTitleHot");
				NormalTitleHot = info.GetString("NormalTitleHot");
				
				SpecialAlignment = (ContentAlignment) info.GetValue("SpecialAlignment", typeof(ContentAlignment));
				NormalAlignment = (ContentAlignment) info.GetValue("NormalAlignment", typeof(ContentAlignment));

				SpecialPadding = (Padding) info.GetValue("SpecialPadding", typeof(Padding));
				NormalPadding = (Padding) info.GetValue("NormalPadding", typeof(Padding));
				
				SpecialBorder = (Border) info.GetValue("SpecialBorder", typeof(Border));
				NormalBorder = (Border) info.GetValue("NormalBorder", typeof(Border));
				SpecialBorderColor = info.GetString("SpecialBorderColor");
				NormalBorderColor = info.GetString("NormalBorderColor");
				
				SpecialBackColor = info.GetString("SpecialBackColor");
				NormalBackColor = info.GetString("NormalBackColor");
				
				SpecialArrowUp = (byte[]) info.GetValue("SpecialArrowUp", typeof(byte[]));
				SpecialArrowUpHot = (byte[]) info.GetValue("SpecialArrowUpHot", typeof(byte[]));
				SpecialArrowDown = (byte[]) info.GetValue("SpecialArrowDown", typeof(byte[]));
				SpecialArrowDownHot = (byte[]) info.GetValue("SpecialArrowDownHot", typeof(byte[]));
				NormalArrowUp = (byte[]) info.GetValue("NormalArrowUp", typeof(byte[]));
				NormalArrowUpHot = (byte[]) info.GetValue("NormalArrowUpHot", typeof(byte[]));
				NormalArrowDown = (byte[]) info.GetValue("NormalArrowDown", typeof(byte[]));
				NormalArrowDownHot = (byte[]) info.GetValue("NormalArrowDownHot", typeof(byte[]));
				
				TitleGradient = info.GetBoolean("TitleGradient");
				SpecialGradientStartColor = info.GetString("SpecialGradientStartColor");
				SpecialGradientEndColor = info.GetString("SpecialGradientEndColor");
				NormalGradientStartColor = info.GetString("NormalGradientStartColor");
				NormalGradientEndColor = info.GetString("NormalGradientEndColor");
				GradientOffset = info.GetSingle("GradientOffset");
			}

			#endregion
		}

		#endregion
	}


	#region HeaderInfoConverter

	/// <summary>
	/// A custom TypeConverter used to help convert HeaderInfos from 
	/// one Type to another
	/// </summary>
	internal class HeaderInfoConverter : ExpandableObjectConverter
	{
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
			if (destinationType == typeof(string) && value is HeaderInfo)
			{
				return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}


		/// <summary>
		/// Returns a collection of properties for the type of array specified 
		/// by the value parameter, using the specified context and attributes
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="value">An Object that specifies the type of array for 
		/// which to get properties</param>
		/// <param name="attributes">An array of type Attribute that is used as 
		/// a filter</param>
		/// <returns>A PropertyDescriptorCollection with the properties that are 
		/// exposed for this data type, or a null reference if there are no 
		/// properties</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			// set the order in which the properties appear 
			// in the property window
			
			PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(HeaderInfo), attributes);

			string[] s = new string[33];
			s[0] = "TitleFont";
			s[1] = "TitleGradient";
			s[2] = "NormalGradientStartColor";
			s[3] = "NormalGradientEndColor";
			s[4] = "SpecialGradientStartColor";
			s[5] = "SpecialGradientEndColor";
			s[6] = "GradientOffset";
			s[7] = "TitleRadius";
			s[8] = "NormalBackImage";
			s[9] = "SpecialBackImage";
			s[10] = "NormalArrowUp";
			s[11] = "NormalArrowUpHot";
			s[12] = "NormalArrowDown";
			s[13] = "NormalArrowDownHot";
			s[14] = "SpecialArrowUp";
			s[15] = "SpecialArrowUpHot";
			s[16] = "SpecialArrowDown";
			s[17] = "SpecialArrowDownHot";
			s[18] = "NormalAlignment";
			s[19] = "SpecialAlignment";
			s[20] = "NormalBackColor";
			s[21] = "SpecialBackColor";
			s[22] = "NormalBorder";
			s[23] = "SpecialBorder";
			s[24] = "NormalBorderColor";
			s[25] = "SpecialBorderColor";
			s[26] = "NormalPadding";
			s[27] = "SpecialPadding";
			s[28] = "NormalTitleColor";
			s[29] = "NormalTitleHotColor";
			s[30] = "SpecialTitleColor";
			s[31] = "SpecialTitleHotColor";
			s[32] = "Margin";

			return collection.Sort(s);
		}
	}

	#endregion

	#endregion


	#region Border Class

	/// <summary>
	/// Specifies the width of the border along each edge of an object
	/// </summary>
	[Serializable,  
	TypeConverter(typeof(BorderConverter))]
	public class Border
	{
		#region Class Data
		
		/// <summary>
		/// Represents a Border structure with its properties 
		/// left uninitialized
		/// </summary>
		[NonSerialized]
		public static readonly Border Empty = new Border(0, 0, 0, 0);
		
		/// <summary>
		/// The width of the left border
		/// </summary>
		private int left;
		
		/// <summary>
		/// The width of the right border
		/// </summary>
		private int right;
		
		/// <summary>
		/// The width of the top border
		/// </summary>
		private int top;
		
		/// <summary>
		/// The width of the bottom border
		/// </summary>
		private int bottom;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the Border class with default settings
		/// </summary>
		public Border() : this(0, 0, 0, 0)
		{

		}


		/// <summary>
		/// Initializes a new instance of the Border class
		/// </summary>
		/// <param name="left">The width of the left border</param>
		/// <param name="top">The Height of the top border</param>
		/// <param name="right">The width of the right border</param>
		/// <param name="bottom">The Height of the bottom border</param>
		public Border(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Tests whether obj is a Border structure with the same values as 
		/// this Border structure
		/// </summary>
		/// <param name="obj">The Object to test</param>
		/// <returns>This method returns true if obj is a Border structure 
		/// and its Left, Top, Right, and Bottom properties are equal to 
		/// the corresponding properties of this Border structure; 
		/// otherwise, false</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Border))
			{
				return false;
			}

			Border border = (Border) obj;

			if (((border.Left == Left) && (border.Top == Top)) && (border.Right == Right))
			{
				return (border.Bottom == Bottom);
			}

			return false;
		}


		/// <summary>
		/// Returns the hash code for this Border structure
		/// </summary>
		/// <returns>An integer that represents the hashcode for this 
		/// border</returns>
		public override int GetHashCode()
		{
			return (((Left ^ ((Top << 13) | (Top >> 0x13))) ^ ((Right << 0x1a) | (Right >> 6))) ^ ((Bottom << 7) | (Bottom >> 0x19)));
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the value of the left border
		/// </summary>
		public int Left
		{
			get
			{
				return left;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				left = value;
			}
		}


		/// <summary>
		/// Gets or sets the value of the right border
		/// </summary>
		public int Right
		{
			get
			{
				return right;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				right = value;
			}
		}


		/// <summary>
		/// Gets or sets the value of the top border
		/// </summary>
		public int Top
		{
			get
			{
				return top;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				top = value;
			}
		}


		/// <summary>
		/// Gets or sets the value of the bottom border
		/// </summary>
		public int Bottom
		{
			get
			{
				return bottom;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				bottom = value;
			}
		}


		/// <summary>
		/// Tests whether all numeric properties of this Border have 
		/// values of zero
		/// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				if (((Left == 0) && (Top == 0)) && (Right == 0))
				{
					return (Bottom == 0);
				}

				return false;
			}
		}

		#endregion


		#region Operators

		/// <summary>
		/// Tests whether two Border structures have equal Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Border structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Border structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if the two Border structures 
		/// have equal Left, Top, Right, and Bottom properties</returns>
		public static bool operator ==(Border left, Border right)
		{
			if (((left.Left == right.Left) && (left.Top == right.Top)) && (left.Right == right.Right))
			{
				return (left.Bottom == right.Bottom);
			}

			return false;
		}


		/// <summary>
		/// Tests whether two Border structures differ in their Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Border structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Border structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if any of the Left, Top, Right, 
		/// and Bottom properties of the two Border structures are unequal; 
		/// otherwise false</returns>
		public static bool operator !=(Border left, Border right)
		{
			return !(left == right);
		}

		#endregion
	}


	#region BorderConverter

	/// <summary>
	/// A custom TypeConverter used to help convert Borders from 
	/// one Type to another
	/// </summary>
	internal class BorderConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert the object to the 
		/// specified type, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides 
		/// a format context</param>
		/// <param name="sourceType">A Type that represents the type you 
		/// want to convert from</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}


		/// <summary>
		/// Returns whether this converter can convert the object to the 
		/// specified type, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="destinationType">A Type that represents the type you 
		/// want to convert to</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			
			return base.CanConvertTo(context, destinationType);
		}


		/// <summary>
		/// Converts the given object to the type of this converter, using 
		/// the specified context and culture information
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="culture">The CultureInfo to use as the current culture</param>
		/// <param name="value">The Object to convert</param>
		/// <returns>An Object that represents the converted value</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string) value).Trim();

				if (text.Length == 0)
				{
					return null;
				}

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				char[] listSeparators = culture.TextInfo.ListSeparator.ToCharArray();

				string[] s = text.Split(listSeparators);

				if (s.Length < 4)
				{
					return null;
				}

				return new Border(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
			}	
			
			return base.ConvertFrom(context, culture, value);
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
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			if ((destinationType == typeof(string)) && (value is Border))
			{
				Border b = (Border) value;

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				string separator = culture.TextInfo.ListSeparator + " ";

				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));

				string[] s = new string[4];

				s[0] = converter.ConvertToString(context, culture, b.Left);
				s[1] = converter.ConvertToString(context, culture, b.Top);
				s[2] = converter.ConvertToString(context, culture, b.Right);
				s[3] = converter.ConvertToString(context, culture, b.Bottom);

				return string.Join(separator, s);
			}

			if ((destinationType == typeof(InstanceDescriptor)) && (value is Border))
			{
				Border b = (Border) value;

				Type[] t = new Type[4];
				t[0] = t[1] = t[2] = t[3] = typeof(int);

				ConstructorInfo info = typeof(Border).GetConstructor(t);

				if (info != null)
				{
					object[] o = new object[4];

					o[0] = b.Left;
					o[1] = b.Top;
					o[2] = b.Right;
					o[3] = b.Bottom;

					return new InstanceDescriptor(info, o);
				}
			}
			
			return base.ConvertTo(context, culture, value, destinationType);
		}


		/// <summary>
		/// Creates an instance of the Type that this TypeConverter is associated 
		/// with, using the specified context, given a set of property values for 
		/// the object
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="propertyValues">An IDictionary of new property values</param>
		/// <returns>An Object representing the given IDictionary, or a null 
		/// reference if the object cannot be created</returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new Border((int) propertyValues["Left"], 
				(int) propertyValues["Top"], 
				(int) propertyValues["Right"], 
				(int) propertyValues["Bottom"]);
		}


		/// <summary>
		/// Returns whether changing a value on this object requires a call to 
		/// CreateInstance to create a new value, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <returns>true if changing a property on this object requires a call 
		/// to CreateInstance to create a new value; otherwise, false</returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		/// <summary>
		/// Returns a collection of properties for the type of array specified 
		/// by the value parameter, using the specified context and attributes
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="value">An Object that specifies the type of array for 
		/// which to get properties</param>
		/// <param name="attributes">An array of type Attribute that is used as 
		/// a filter</param>
		/// <returns>A PropertyDescriptorCollection with the properties that are 
		/// exposed for this data type, or a null reference if there are no 
		/// properties</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(Border), attributes);

			string[] s = new string[4];
			s[0] = "Left";
			s[1] = "Top";
			s[2] = "Right";
			s[3] = "Bottom";

			return collection.Sort(s);
		}


		/// <summary>
		/// Returns whether this object supports properties, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context</param>
		/// <returns>true if GetProperties should be called to find the properties of this 
		/// object; otherwise, false</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

	#endregion

	#endregion


	#region Padding Class

	/// <summary>
	/// Specifies the amount of space between the border and any contained 
	/// items along each edge of an object
	/// </summary>
	[Serializable, 
	TypeConverter(typeof(PaddingConverter))]
	public class Padding
	{
		#region Class Data
		
		/// <summary>
		/// Represents a Padding structure with its properties 
		/// left uninitialized
		/// </summary>
		[NonSerialized]
		public static readonly Padding Empty = new Padding(0, 0, 0, 0);
		
		/// <summary>
		/// The width of the left padding
		/// </summary>
		private int left;
		
		/// <summary>
		/// The width of the right padding
		/// </summary>
		private int right;
		
		/// <summary>
		/// The width of the top padding
		/// </summary>
		private int top;
		
		/// <summary>
		/// The width of the bottom padding
		/// </summary>
		private int bottom;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the Padding class with default settings
		/// </summary>
		public Padding() : this(0, 0, 0, 0)
		{

		}


		/// <summary>
		/// Initializes a new instance of the Padding class
		/// </summary>
		/// <param name="left">The width of the left padding value</param>
		/// <param name="top">The height of top padding value</param>
		/// <param name="right">The width of the right padding value</param>
		/// <param name="bottom">The height of bottom padding value</param>
		public Padding(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Tests whether obj is a Padding structure with the same values as 
		/// this Padding structure
		/// </summary>
		/// <param name="obj">The Object to test</param>
		/// <returns>This method returns true if obj is a Padding structure 
		/// and its Left, Top, Right, and Bottom properties are equal to 
		/// the corresponding properties of this Padding structure; 
		/// otherwise, false</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Padding))
			{
				return false;
			}

			Padding padding = (Padding) obj;

			if (((padding.Left == Left) && (padding.Top == Top)) && (padding.Right == Right))
			{
				return (padding.Bottom == Bottom);
			}

			return false;
		}


		/// <summary>
		/// Returns the hash code for this Padding structure
		/// </summary>
		/// <returns>An integer that represents the hashcode for this 
		/// padding</returns>
		public override int GetHashCode()
		{
			return (((Left ^ ((Top << 13) | (Top >> 0x13))) ^ ((Right << 0x1a) | (Right >> 6))) ^ ((Bottom << 7) | (Bottom >> 0x19)));
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the width of the left padding value
		/// </summary>
		public int Left
		{
			get
			{
				return left;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				left = value;
			}
		}


		/// <summary>
		/// Gets or sets the width of the right padding value
		/// </summary>
		public int Right
		{
			get
			{
				return right;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				right = value;
			}
		}


		/// <summary>
		/// Gets or sets the height of the top padding value
		/// </summary>
		public int Top
		{
			get
			{
				return top;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				top = value;
			}
		}


		/// <summary>
		/// Gets or sets the height of the bottom padding value
		/// </summary>
		public int Bottom
		{
			get
			{
				return bottom;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				bottom = value;
			}
		}


		/// <summary>
		/// Tests whether all numeric properties of this Padding have 
		/// values of zero
		/// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				if (((Left == 0) && (Top == 0)) && (Right == 0))
				{
					return (Bottom == 0);
				}

				return false;
			}
		}

		#endregion


		#region Operators

		/// <summary>
		/// Tests whether two Padding structures have equal Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Padding structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Padding structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if the two Padding structures 
		/// have equal Left, Top, Right, and Bottom properties</returns>
		public static bool operator ==(Padding left, Padding right)
		{
			if (((left.Left == right.Left) && (left.Top == right.Top)) && (left.Right == right.Right))
			{
				return (left.Bottom == right.Bottom);
			}

			return false;
		}


		/// <summary>
		/// Tests whether two Padding structures differ in their Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Padding structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Padding structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if any of the Left, Top, Right, 
		/// and Bottom properties of the two Padding structures are unequal; 
		/// otherwise false</returns>
		public static bool operator !=(Padding left, Padding right)
		{
			return !(left == right);
		}

		#endregion
	}


	#region PaddingConverter

	/// <summary>
	/// A custom TypeConverter used to help convert Padding objects from 
	/// one Type to another
	/// </summary>
	internal class PaddingConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an object of the 
		/// given type to the type of this converter, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides 
		/// a format context</param>
		/// <param name="sourceType">A Type that represents the type you 
		/// want to convert from</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}


		/// <summary>
		/// Returns whether this converter can convert the object to the 
		/// specified type, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="destinationType">A Type that represents the type you 
		/// want to convert to</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			
			return base.CanConvertTo(context, destinationType);
		}


		/// <summary>
		/// Converts the given object to the type of this converter, using 
		/// the specified context and culture information
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="culture">The CultureInfo to use as the current culture</param>
		/// <param name="value">The Object to convert</param>
		/// <returns>An Object that represents the converted value</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string) value).Trim();

				if (text.Length == 0)
				{
					return null;
				}

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				char[] listSeparators = culture.TextInfo.ListSeparator.ToCharArray();

				string[] s = text.Split(listSeparators);

				if (s.Length < 4)
				{
					return null;
				}

				return new Padding(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
			}	
			
			return base.ConvertFrom(context, culture, value);
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
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			if ((destinationType == typeof(string)) && (value is Padding))
			{
				Padding p = (Padding) value;

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				string separator = culture.TextInfo.ListSeparator + " ";

				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));

				string[] s = new string[4];

				s[0] = converter.ConvertToString(context, culture, p.Left);
				s[1] = converter.ConvertToString(context, culture, p.Top);
				s[2] = converter.ConvertToString(context, culture, p.Right);
				s[3] = converter.ConvertToString(context, culture, p.Bottom);

				return string.Join(separator, s);
			}

			if ((destinationType == typeof(InstanceDescriptor)) && (value is Padding))
			{
				Padding p = (Padding) value;

				Type[] t = new Type[4];
				t[0] = t[1] = t[2] = t[3] = typeof(int);

				ConstructorInfo info = typeof(Padding).GetConstructor(t);

				if (info != null)
				{
					object[] o = new object[4];

					o[0] = p.Left;
					o[1] = p.Top;
					o[2] = p.Right;
					o[3] = p.Bottom;

					return new InstanceDescriptor(info, o);
				}
			}
			
			return base.ConvertTo(context, culture, value, destinationType);
		}


		/// <summary>
		/// Creates an instance of the Type that this TypeConverter is associated 
		/// with, using the specified context, given a set of property values for 
		/// the object
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="propertyValues">An IDictionary of new property values</param>
		/// <returns>An Object representing the given IDictionary, or a null 
		/// reference if the object cannot be created</returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new Padding((int) propertyValues["Left"], 
				(int) propertyValues["Top"], 
				(int) propertyValues["Right"], 
				(int) propertyValues["Bottom"]);
		}


		/// <summary>
		/// Returns whether changing a value on this object requires a call to 
		/// CreateInstance to create a new value, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <returns>true if changing a property on this object requires a call 
		/// to CreateInstance to create a new value; otherwise, false</returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		/// <summary>
		/// Returns a collection of properties for the type of array specified 
		/// by the value parameter, using the specified context and attributes
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="value">An Object that specifies the type of array for 
		/// which to get properties</param>
		/// <param name="attributes">An array of type Attribute that is used as 
		/// a filter</param>
		/// <returns>A PropertyDescriptorCollection with the properties that are 
		/// exposed for this data type, or a null reference if there are no 
		/// properties</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(Padding), attributes);

			string[] s = new string[4];
			s[0] = "Left";
			s[1] = "Top";
			s[2] = "Right";
			s[3] = "Bottom";

			return collection.Sort(s);
		}


		/// <summary>
		/// Returns whether this object supports properties, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context</param>
		/// <returns>true if GetProperties should be called to find the properties of this 
		/// object; otherwise, false</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

	#endregion

	#endregion


	#region Margin Class

	/// <summary>
	/// Specifies the amount of space arouund an object along each side
	/// </summary>
	[Serializable,  
	TypeConverter(typeof(MarginConverter))]
	public class Margin
	{
		#region Class Data
		
		/// <summary>
		/// Represents a Margin structure with its properties 
		/// left uninitialized
		/// </summary>
		[NonSerialized]
		public static readonly Margin Empty = new Margin(0, 0, 0, 0);
		
		/// <summary>
		/// The width of the left margin
		/// </summary>
		private int left;
		
		/// <summary>
		/// The width of the right margin
		/// </summary>
		private int right;
		
		/// <summary>
		/// The width of the top margin
		/// </summary>
		private int top;
		
		/// <summary>
		/// The width of the bottom margin
		/// </summary>
		private int bottom;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the Margin class with default settings
		/// </summary>
		public Margin() : this(0, 0, 0, 0)
		{

		}


		/// <summary>
		/// Initializes a new instance of the Margin class
		/// </summary>
		/// <param name="left">The width of the left margin value</param>
		/// <param name="top">The height of the top margin value</param>
		/// <param name="right">The width of the right margin value</param>
		/// <param name="bottom">The height of the bottom margin value</param>
		public Margin(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Tests whether obj is a Margin structure with the same values as 
		/// this Border structure
		/// </summary>
		/// <param name="obj">The Object to test</param>
		/// <returns>This method returns true if obj is a Margin structure 
		/// and its Left, Top, Right, and Bottom properties are equal to 
		/// the corresponding properties of this Margin structure; 
		/// otherwise, false</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Margin))
			{
				return false;
			}

			Margin margin = (Margin) obj;

			if (((margin.Left == Left) && (margin.Top == Top)) && (margin.Right == Right))
			{
				return (margin.Bottom == Bottom);
			}

			return false;
		}


		/// <summary>
		/// Returns the hash code for this Margin structure
		/// </summary>
		/// <returns>An integer that represents the hashcode for this 
		/// margin</returns>
		public override int GetHashCode()
		{
			return (((Left ^ ((Top << 13) | (Top >> 0x13))) ^ ((Right << 0x1a) | (Right >> 6))) ^ ((Bottom << 7) | (Bottom >> 0x19)));
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the left margin value
		/// </summary>
		public int Left
		{
			get
			{
				return left;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				left = value;
			}
		}


		/// <summary>
		/// Gets or sets the right margin value
		/// </summary>
		public int Right
		{
			get
			{
				return right;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				right = value;
			}
		}


		/// <summary>
		/// Gets or sets the top margin value
		/// </summary>
		public int Top
		{
			get
			{
				return top;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				top = value;
			}
		}


		/// <summary>
		/// Gets or sets the bottom margin value
		/// </summary>
		public int Bottom
		{
			get
			{
				return bottom;
			}

			set
			{
				if (value < 0)
				{
					value = 0;
				}

				bottom = value;
			}
		}


		/// <summary>
		/// Tests whether all numeric properties of this Margin have 
		/// values of zero
		/// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				if (((Left == 0) && (Top == 0)) && (Right == 0))
				{
					return (Bottom == 0);
				}

				return false;
			}
		}

		#endregion


		#region Operators

		/// <summary>
		/// Tests whether two Margin structures have equal Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Margin structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Margin structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if the two Margin structures 
		/// have equal Left, Top, Right, and Bottom properties</returns>
		public static bool operator ==(Margin left, Margin right)
		{
			if (((left.Left == right.Left) && (left.Top == right.Top)) && (left.Right == right.Right))
			{
				return (left.Bottom == right.Bottom);
			}

			return false;
		}


		/// <summary>
		/// Tests whether two Margin structures differ in their Left, Top, 
		/// Right, and Bottom properties
		/// </summary>
		/// <param name="left">The Margin structure that is to the left 
		/// of the equality operator</param>
		/// <param name="right">The Margin structure that is to the right 
		/// of the equality operator</param>
		/// <returns>This operator returns true if any of the Left, Top, Right, 
		/// and Bottom properties of the two Margin structures are unequal; 
		/// otherwise false</returns>
		public static bool operator !=(Margin left, Margin right)
		{
			return !(left == right);
		}

		#endregion
	}


	#region MarginConverter

	/// <summary>
	/// A custom TypeConverter used to help convert Margins from 
	/// one Type to another
	/// </summary>
	internal class MarginConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an object of the 
		/// given type to the type of this converter, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides 
		/// a format context</param>
		/// <param name="sourceType">A Type that represents the type you 
		/// want to convert from</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}


		/// <summary>
		/// Returns whether this converter can convert the object to the 
		/// specified type, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="destinationType">A Type that represents the type you 
		/// want to convert to</param>
		/// <returns>true if this converter can perform the conversion; 
		/// otherwise, false</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			
			return base.CanConvertTo(context, destinationType);
		}


		/// <summary>
		/// Converts the given object to the type of this converter, using 
		/// the specified context and culture information
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <param name="culture">The CultureInfo to use as the current culture</param>
		/// <param name="value">The Object to convert</param>
		/// <returns>An Object that represents the converted value</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string) value).Trim();

				if (text.Length == 0)
				{
					return null;
				}

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				char[] listSeparators = culture.TextInfo.ListSeparator.ToCharArray();

				string[] s = text.Split(listSeparators);

				if (s.Length < 4)
				{
					return null;
				}

				return new Margin(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
			}	
			
			return base.ConvertFrom(context, culture, value);
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
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			if ((destinationType == typeof(string)) && (value is Margin))
			{
				Margin m = (Margin) value;

				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				string separator = culture.TextInfo.ListSeparator + " ";

				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));

				string[] s = new string[4];

				s[0] = converter.ConvertToString(context, culture, m.Left);
				s[1] = converter.ConvertToString(context, culture, m.Top);
				s[2] = converter.ConvertToString(context, culture, m.Right);
				s[3] = converter.ConvertToString(context, culture, m.Bottom);

				return string.Join(separator, s);
			}

			if ((destinationType == typeof(InstanceDescriptor)) && (value is Margin))
			{
				Margin m = (Margin) value;

				Type[] t = new Type[4];
				t[0] = t[1] = t[2] = t[3] = typeof(int);

				ConstructorInfo info = typeof(Margin).GetConstructor(t);

				if (info != null)
				{
					object[] o = new object[4];

					o[0] = m.Left;
					o[1] = m.Top;
					o[2] = m.Right;
					o[3] = m.Bottom;

					return new InstanceDescriptor(info, o);
				}
			}
			
			return base.ConvertTo(context, culture, value, destinationType);
		}


		/// <summary>
		/// Creates an instance of the Type that this TypeConverter is associated 
		/// with, using the specified context, given a set of property values for 
		/// the object
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="propertyValues">An IDictionary of new property values</param>
		/// <returns>An Object representing the given IDictionary, or a null 
		/// reference if the object cannot be created</returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new Margin((int) propertyValues["Left"], 
				(int) propertyValues["Top"], 
				(int) propertyValues["Right"], 
				(int) propertyValues["Bottom"]);
		}


		/// <summary>
		/// Returns whether changing a value on this object requires a call to 
		/// CreateInstance to create a new value, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a 
		/// format context</param>
		/// <returns>true if changing a property on this object requires a call 
		/// to CreateInstance to create a new value; otherwise, false</returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		/// <summary>
		/// Returns a collection of properties for the type of array specified 
		/// by the value parameter, using the specified context and attributes
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format 
		/// context</param>
		/// <param name="value">An Object that specifies the type of array for 
		/// which to get properties</param>
		/// <param name="attributes">An array of type Attribute that is used as 
		/// a filter</param>
		/// <returns>A PropertyDescriptorCollection with the properties that are 
		/// exposed for this data type, or a null reference if there are no 
		/// properties</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(Margin), attributes);

			string[] s = new string[4];
			s[0] = "Left";
			s[1] = "Top";
			s[2] = "Right";
			s[3] = "Bottom";

			return collection.Sort(s);
		}


		/// <summary>
		/// Returns whether this object supports properties, using the specified context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context</param>
		/// <returns>true if GetProperties should be called to find the properties of this 
		/// object; otherwise, false</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

	#endregion

	#endregion Margin Class


	#region ImageStretchMode

	/// <summary>
	/// Specifies how images should fill objects
	/// </summary>
	public enum ImageStretchMode
	{
		/// <summary>
		/// Use default settings
		/// </summary>
		Normal = 0,
		
		/// <summary>
		/// The image is transparent
		/// </summary>
		Transparent = 2,
		
		/// <summary>
		/// The image should be tiled
		/// </summary>
		Tile = 3,
		
		/// <summary>
		/// The image should be stretched to fit the objects width 
		/// </summary>
		Horizontal = 5,
		
		/// <summary>
		/// The image should be stretched to fill the object
		/// </summary>
		Stretch = 6,
		
		/// <summary>
		/// The image is stored in ARGB format
		/// </summary>
		ARGBImage = 7
	}

	#endregion
}
