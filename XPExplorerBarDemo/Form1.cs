/*
 * Copyright (c) 2004, Mathew Hall
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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Windows.Forms;
using XPExplorerBar;

namespace XPExplorerBarDemo
{
	#region DemoForm
	
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class DemoForm : Form
	{
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public DemoForm()
		{
			InitializeComponent();
			
			InitialiseDescriptors();
		}


		/// <summary>
		/// 
		/// </summary>
		private void InitialiseDescriptors()
		{
			customTaskPaneDescriptor = new TaskPaneDescriptor(customTaskPane);
			customExpandoDescriptor = new ExpandoDescriptor(customExpando);
			customTaskItem1Descriptor = new TaskItemDescriptor(customTaskItem1);
			customTaskItem2Descriptor = new TaskItemDescriptor(customTaskItem2);
			customTaskItem3Descriptor = new TaskItemDescriptor(customTaskItem3);
		}

		#endregion


		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DemoForm());
		}

		#endregion


		#region Events

		#region Menu
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void animateMenuItem_Click(object sender, EventArgs e)
		{
			animateMenuItem.Checked = !animateMenuItem.Checked;

			pictureTasksExpando.Animate = animateMenuItem.Checked;
			fileAndFolderTasksExpando.Animate = animateMenuItem.Checked;
			otherPlacesExpando.Animate = animateMenuItem.Checked;
			detailsExpando.Animate = animateMenuItem.Checked;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cycleMenuItem_Click(object sender, EventArgs e)
		{
			systemTaskPane.Expandos.MoveToBottom(systemTaskPane.Expandos[0]);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void expandoDraggingMenuItem_Click(object sender, EventArgs e)
		{
			expandoDraggingMenuItem.Checked = !expandoDraggingMenuItem.Checked;

			systemTaskPane.AllowExpandoDragging = expandoDraggingMenuItem.Checked;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void classicMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = true;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = false;

			systemTaskPane.UseClassicTheme();
			customTaskPane.UseClassicTheme();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void blueMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = true;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = false;

			// foreverblue.dll is a cut down version of the the 
			// forever blue theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original forever blue theme 
			// can be found at http://www.themexp.org/
			systemTaskPane.UseCustomTheme(Theme.ForeverBlue);
			customTaskPane.UseCustomTheme(Theme.ForeverBlue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void itunesMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = true;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = false;

			// itunes.dll is a cut down version of the the 
			// iTunes theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original iTunes theme 
			// can be found at http://www.themexp.org/
			systemTaskPane.UseCustomTheme(Theme.ITunes);
			customTaskPane.UseCustomTheme(Theme.ITunes);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pantherMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = true;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = false;

			// panther.dll is a cut down version of the the 
			// OS X Panther theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original OS X Panther theme 
			// can be found at http://www.themexp.org/
			systemTaskPane.UseCustomTheme(Theme.Panther);
			customTaskPane.UseCustomTheme(Theme.Panther);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bwMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = true;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = false;

			systemTaskPane.UseCustomTheme(Theme.BW);
			customTaskPane.UseCustomTheme(Theme.BW);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xboxMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = true;
			defaultMenuItem.Checked = false;

			// xbox.dll is a cut down version of the the 
			// XtremeXP theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original XtremeXP theme 
			// can be found at http://www.themexp.org/
			systemTaskPane.UseCustomTheme(Theme.Xbox);
			customTaskPane.UseCustomTheme(Theme.Xbox);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void defaultMenuItem_Click(object sender, EventArgs e)
		{
			classicMenuItem.Checked = false;
			blueMenuItem.Checked = false;
			itunesMenuItem.Checked = false;
			pantherMenuItem.Checked = false;
			bwMenuItem.Checked = false;
			xboxMenuItem.Checked = false;
			defaultMenuItem.Checked = true;

			systemTaskPane.UseDefaultTheme();
			customTaskPane.UseDefaultTheme();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myPicturesMenuItem_Click(object sender, EventArgs e)
		{
			if (otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			myPicturesMenuItem.Checked = !myPicturesMenuItem.Checked;
			
			if (!myPicturesMenuItem.Checked)
			{
				otherPlacesExpando.HideControl(myPicturesTaskItem);
			}
			else
			{
				otherPlacesExpando.ShowControl(myPicturesTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myComputerMenuItem_Click(object sender, EventArgs e)
		{
			if (otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			myComputerMenuItem.Checked = !myComputerMenuItem.Checked;
			
			if (!myComputerMenuItem.Checked)
			{
				otherPlacesExpando.HideControl(myComputerTaskItem);
			}
			else
			{
				otherPlacesExpando.ShowControl(myComputerTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myNetworkMenuItem_Click(object sender, EventArgs e)
		{
			if (otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			myNetworkMenuItem.Checked = !myNetworkMenuItem.Checked;
			
			if (!myNetworkMenuItem.Checked)
			{
				otherPlacesExpando.HideControl(myNetworkPlacesTaskItem);
			}
			else
			{
				otherPlacesExpando.ShowControl(myNetworkPlacesTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showFocusMenuItem_Click(object sender, EventArgs e)
		{
			showFocusMenuItem.Checked = !showFocusMenuItem.Checked;
			bool showFocus = showFocusMenuItem.Checked;

			pictureTasksExpando.ShowFocusCues = showFocus;
			slideShowTaskItem.ShowFocusCues = showFocus;
			orderOnlineTaskItem.ShowFocusCues = showFocus;
			printPicturesTaskItem.ShowFocusCues = showFocus;
			copyToCDTaskItem.ShowFocusCues = showFocus;

			fileAndFolderTasksExpando.ShowFocusCues = showFocus;
			newFolderTaskItem.ShowFocusCues = showFocus;
			publishToWebTaskItem.ShowFocusCues = showFocus;
			shareFolderTaskItem.ShowFocusCues = showFocus;

			otherPlacesExpando.ShowFocusCues = showFocus;
			myDocumentsTaskItem.ShowFocusCues = showFocus;
			myPicturesTaskItem.ShowFocusCues = showFocus;
			myComputerTaskItem.ShowFocusCues = showFocus;
			myNetworkPlacesTaskItem.ShowFocusCues = showFocus;

			detailsExpando.ShowFocusCues = showFocus;
		}

		#endregion

		#region Custom Settings

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void controlComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (controlComboBox.SelectedItem.ToString().Equals("TaskPane"))
			{
				customPropertyGrid.SelectedObject = customTaskPaneDescriptor;
			}
			else if (controlComboBox.SelectedItem.ToString().Equals("Expando"))
			{
				customPropertyGrid.SelectedObject = customExpandoDescriptor;
			}
			else if (controlComboBox.SelectedItem.ToString().Equals("TaskItem1"))
			{
				customPropertyGrid.SelectedObject = customTaskItem1Descriptor;
			}
			else if (controlComboBox.SelectedItem.ToString().Equals("TaskItem2"))
			{
				customPropertyGrid.SelectedObject = customTaskItem2Descriptor;
			}
			else if (controlComboBox.SelectedItem.ToString().Equals("TaskItem3"))
			{
				customPropertyGrid.SelectedObject = customTaskItem3Descriptor;
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void serializeFileButton_Click(object sender, EventArgs e)
		{
			IFormatter formatter = new BinaryFormatter();
			
			Stream stream = null;
			
			try
			{
				stream = new FileStream("Serialized XPExplorerBar.txt", FileMode.Create, FileAccess.Write, FileShare.None);
				
				TaskPane.TaskPaneSurrogate serializeTaskPaneSurrogate = new TaskPane.TaskPaneSurrogate(); 
				serializeTaskPaneSurrogate.Load(serializeTaskPane);
				
				formatter.Serialize(stream, serializeTaskPaneSurrogate);

				MessageBox.Show(this, 
					"XPExplorerBar successfully serialized to '" + Application.StartupPath + "\\Serialized XPExplorerBar.txt'", 
					"XPExplorerBar Serialized", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Information);
			}
			catch (ArgumentNullException ane)
			{
				MessageBox.Show(this, ane.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SerializationException se)
			{
				MessageBox.Show(this, se.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SecurityException sece)
			{
				MessageBox.Show(this, sece.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (IOException ioe)
			{
				MessageBox.Show(this, ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void deserializeFileButton_Click(object sender, EventArgs e)
		{
			if (serializeGroupBox.Controls.Count == 8)
			{
				MessageBox.Show(this, 
					"XPExplorerBar cannot be deserialized as there is an existing deserialized XPExplorerBar.\r\nPlease remove the existing deserialized XPExplorerBar first by using the 'Remove' button", 
					"Cannot Deserialize", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				
				return;
			}
			
			IFormatter formatter = new BinaryFormatter();
			
			Stream stream = null;
			
			try
			{
				stream = new FileStream("Serialized XPExplorerBar.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
			
				TaskPane.TaskPaneSurrogate serializeTaskPaneSurrogate = (TaskPane.TaskPaneSurrogate) formatter.Deserialize(stream);  

				TaskPane taskpane = serializeTaskPaneSurrogate.Save();
				taskpane.Name = "SerializedTaskPane";
				taskpane.Location = new Point(8, 350);
				
				serializeGroupBox.Controls.Add(taskpane);
			}
			catch (TargetInvocationException tie)
			{
				MessageBox.Show(this, tie.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (ArgumentNullException ane)
			{
				MessageBox.Show(this, ane.Message, "Error" + ane.ParamName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SerializationException se)
			{
				MessageBox.Show(this, se.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SecurityException sece)
			{
				MessageBox.Show(this, sece.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (IOException ioe)
			{
				MessageBox.Show(this, ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void serializeMemoryButton_Click(object sender, EventArgs e)
		{
			if (serializeGroupBox.Controls.Count == 8)
			{
				MessageBox.Show(this, 
					"XPExplorerBar cannot be serialized as there is an existing deserialized XPExplorerBar.\r\nPlease remove the existing deserialized XPExplorerBar first by using the 'Remove' button", 
					"Cannot serialize", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				
				return;
			}

			MemoryStream stream1 = null;
			MemoryStream stream2 = null;
			
			try
			{
				stream1 = new MemoryStream();
				
				IFormatter formatter = new BinaryFormatter();
				
				//formatter.Serialize(stream1, this.serializeTaskPane);

				TaskPane.TaskPaneSurrogate serializeTaskPaneSurrogate = new TaskPane.TaskPaneSurrogate(); 
				serializeTaskPaneSurrogate.Load(serializeTaskPane);
				
				formatter.Serialize(stream1, serializeTaskPaneSurrogate);

				byte[] bytes = stream1.ToArray();

				stream1.Flush();
				stream1.Close();
				stream1 = null;

				stream2 = new MemoryStream(bytes);
				stream2.Position = 0;

				//TaskPane taskpane = (TaskPane) formatter.Deserialize(stream2);
				TaskPane.TaskPaneSurrogate deserializeTaskPaneSurrogate = (TaskPane.TaskPaneSurrogate) formatter.Deserialize(stream2);  

				stream2.Close();
				stream2 = null;

				TaskPane taskpane = deserializeTaskPaneSurrogate.Save();
				taskpane.Name = "SerializedTaskPane";
				taskpane.Location = new Point(8, 350);
				
				serializeGroupBox.Controls.Add(taskpane);
			}
			catch (ArgumentNullException ane)
			{
				MessageBox.Show(this, ane.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SerializationException se)
			{
				MessageBox.Show(this, se.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (SecurityException sece)
			{
				MessageBox.Show(this, sece.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (IOException ioe)
			{
				MessageBox.Show(this, ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (stream1 != null)
				{
					stream1.Close();
				}

				if (stream2 != null)
				{
					stream2.Close();
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (Control control in serializeGroupBox.Controls)
			{
				if (control is TaskPane && control.Name.Equals("SerializedTaskPane"))
				{
					serializeGroupBox.Controls.Remove(control);

					return;
				}
			}
		}

		#endregion

		#endregion
	}

	#endregion


	#region ICustomTypeDescriptors

	#region TaskPaneDescriptor
	
	/// <summary>
	/// 
	/// </summary>
	public class TaskPaneDescriptor : ICustomTypeDescriptor
	{
		/// <summary>
		/// 
		/// </summary>
		private TaskPane owner;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		public TaskPaneDescriptor(TaskPane owner)
		{
			this.owner = owner;
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetConverter()
		{
			 return TypeDescriptor.GetConverter(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(owner, attributes, true);
			
			PropertyDescriptor[] pd = new PropertyDescriptor[1];
			
			for (int i=0; i<pdc.Count; i++)
			{
				if (pdc[i].Name.Equals("CustomSettings"))
				{
					pd[0] = pdc[i];

					break;
				}
			}

			return new PropertyDescriptorCollection(pd);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor) this).GetProperties(new Attribute[0]);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="editorBaseType"></param>
		/// <returns></returns>
		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(owner, true);
		}
	}

	#endregion


	#region ExpandoDescriptor
	
	/// <summary>
	/// 
	/// </summary>
	public class ExpandoDescriptor : ICustomTypeDescriptor
	{
		/// <summary>
		/// 
		/// </summary>
		private Expando owner;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		public ExpandoDescriptor(Expando owner)
		{
			this.owner = owner;
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(owner, attributes, true);
			
			PropertyDescriptor[] pd = new PropertyDescriptor[9];
			
			for (int i=0; i<pdc.Count; i++)
			{
				if (pdc[i].Name.Equals("Animate"))
				{
					pd[0] = pdc[i];
				}
				else if (pdc[i].Name.Equals("Collapsed"))
				{
					pd[1] = pdc[i];
				}
				else if (pdc[i].Name.Equals("CustomHeaderSettings"))
				{
					pd[2] = pdc[i];
				}
				else if (pdc[i].Name.Equals("CustomSettings"))
				{
					pd[3] = pdc[i];
				}
				else if (pdc[i].Name.Equals("SpecialGroup"))
				{
					pd[4] = pdc[i];
				}
				else if (pdc[i].Name.Equals("TitleImage"))
				{
					pd[5] = pdc[i];
				}
				else if (pdc[i].Name.Equals("Watermark"))
				{
					pd[6] = pdc[i];
				}
				else if (pdc[i].Name.Equals("CanCollapse"))
				{
					pd[7] = pdc[i];
				}
				else if (pdc[i].Name.Equals("Enabled"))
				{
					pd[8] = pdc[i];
				}
			}

			return new PropertyDescriptorCollection(pd);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor) this).GetProperties(new Attribute[0]);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="editorBaseType"></param>
		/// <returns></returns>
		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(owner, true);
		}
	}

	#endregion


	#region TaskItemDescriptor
	
	/// <summary>
	/// 
	/// </summary>
	public class TaskItemDescriptor : ICustomTypeDescriptor
	{
		/// <summary>
		/// 
		/// </summary>
		private TaskItem owner;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		public TaskItemDescriptor(TaskItem owner)
		{
			this.owner = owner;
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(owner, attributes, true);
			
			PropertyDescriptor[] pd = new PropertyDescriptor[1];
			
			for (int i=0; i<pdc.Count; i++)
			{
				if (pdc[i].Name.Equals("CustomSettings"))
				{
					pd[0] = pdc[i];

					break;
				}
			}

			return new PropertyDescriptorCollection(pd);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor) this).GetProperties(new Attribute[0]);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="editorBaseType"></param>
		/// <returns></returns>
		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(owner, true);
		}
	}

	#endregion

	#endregion
}
