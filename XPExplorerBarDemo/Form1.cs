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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
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
	public partial class DemoForm : System.Windows.Forms.Form
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
			this.customTaskPaneDescriptor = new TaskPaneDescriptor(this.customTaskPane);
			this.customExpandoDescriptor = new ExpandoDescriptor(this.customExpando);
			this.customTaskItem1Descriptor = new TaskItemDescriptor(this.customTaskItem1);
			this.customTaskItem2Descriptor = new TaskItemDescriptor(this.customTaskItem2);
			this.customTaskItem3Descriptor = new TaskItemDescriptor(this.customTaskItem3);
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
		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void animateMenuItem_Click(object sender, System.EventArgs e)
		{
			this.animateMenuItem.Checked = !this.animateMenuItem.Checked;

			this.pictureTasksExpando.Animate = this.animateMenuItem.Checked;
			this.fileAndFolderTasksExpando.Animate = this.animateMenuItem.Checked;
			this.otherPlacesExpando.Animate = this.animateMenuItem.Checked;
			this.detailsExpando.Animate = this.animateMenuItem.Checked;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cycleMenuItem_Click(object sender, System.EventArgs e)
		{
			this.systemTaskPane.Expandos.MoveToBottom(this.systemTaskPane.Expandos[0]);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void expandoDraggingMenuItem_Click(object sender, System.EventArgs e)
		{
			this.expandoDraggingMenuItem.Checked = !this.expandoDraggingMenuItem.Checked;

			this.systemTaskPane.AllowExpandoDragging = this.expandoDraggingMenuItem.Checked;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void classicMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = true;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = false;

			this.systemTaskPane.UseClassicTheme();
			this.customTaskPane.UseClassicTheme();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void blueMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = true;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = false;

			// foreverblue.dll is a cut down version of the the 
			// forever blue theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original forever blue theme 
			// can be found at http://www.themexp.org/
			this.systemTaskPane.UseCustomTheme("foreverblue.dll");
			this.customTaskPane.UseCustomTheme("foreverblue.dll");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void itunesMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = true;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = false;

			// itunes.dll is a cut down version of the the 
			// iTunes theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original iTunes theme 
			// can be found at http://www.themexp.org/
			this.systemTaskPane.UseCustomTheme("itunes.dll");
			this.customTaskPane.UseCustomTheme("itunes.dll");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pantherMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = true;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = false;

			// panther.dll is a cut down version of the the 
			// OS X Panther theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original OS X Panther theme 
			// can be found at http://www.themexp.org/
			this.systemTaskPane.UseCustomTheme("panther.dll");
			this.customTaskPane.UseCustomTheme("panther.dll");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bwMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = true;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = false;

			this.systemTaskPane.UseCustomTheme("bw.dll");
			this.customTaskPane.UseCustomTheme("bw.dll");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xboxMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = true;
			this.defaultMenuItem.Checked = false;

			// xbox.dll is a cut down version of the the 
			// XtremeXP theme. do not attempt to use this as 
			// a proper theme for XP as Windows may crash due to 
			// several images being removed from the dll to keep
			// file sizes down.  the original XtremeXP theme 
			// can be found at http://www.themexp.org/
			this.systemTaskPane.UseCustomTheme("xbox.dll");
			this.customTaskPane.UseCustomTheme("xbox.dll");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void defaultMenuItem_Click(object sender, System.EventArgs e)
		{
			this.classicMenuItem.Checked = false;
			this.blueMenuItem.Checked = false;
			this.itunesMenuItem.Checked = false;
			this.pantherMenuItem.Checked = false;
			this.bwMenuItem.Checked = false;
			this.xboxMenuItem.Checked = false;
			this.defaultMenuItem.Checked = true;

			this.systemTaskPane.UseCustomTheme("default.dll");
			this.customTaskPane.UseCustomTheme("default.dll");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myPicturesMenuItem_Click(object sender, System.EventArgs e)
		{
			if (this.otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			this.myPicturesMenuItem.Checked = !this.myPicturesMenuItem.Checked;
			
			if (!this.myPicturesMenuItem.Checked)
			{
				this.otherPlacesExpando.HideControl(this.myPicturesTaskItem);
			}
			else
			{
				this.otherPlacesExpando.ShowControl(this.myPicturesTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myComputerMenuItem_Click(object sender, System.EventArgs e)
		{
			if (this.otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			this.myComputerMenuItem.Checked = !this.myComputerMenuItem.Checked;
			
			if (!this.myComputerMenuItem.Checked)
			{
				this.otherPlacesExpando.HideControl(this.myComputerTaskItem);
			}
			else
			{
				this.otherPlacesExpando.ShowControl(this.myComputerTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myNetworkMenuItem_Click(object sender, System.EventArgs e)
		{
			if (this.otherPlacesExpando.Collapsed)
			{
				return;
			}
			
			this.myNetworkMenuItem.Checked = !this.myNetworkMenuItem.Checked;
			
			if (!this.myNetworkMenuItem.Checked)
			{
				this.otherPlacesExpando.HideControl(this.myNetworkPlacesTaskItem);
			}
			else
			{
				this.otherPlacesExpando.ShowControl(this.myNetworkPlacesTaskItem);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showFocusMenuItem_Click(object sender, System.EventArgs e)
		{
			this.showFocusMenuItem.Checked = !this.showFocusMenuItem.Checked;
			bool showFocus = this.showFocusMenuItem.Checked;

			this.pictureTasksExpando.ShowFocusCues = showFocus;
			this.slideShowTaskItem.ShowFocusCues = showFocus;
			this.orderOnlineTaskItem.ShowFocusCues = showFocus;
			this.printPicturesTaskItem.ShowFocusCues = showFocus;
			this.copyToCDTaskItem.ShowFocusCues = showFocus;

			this.fileAndFolderTasksExpando.ShowFocusCues = showFocus;
			this.newFolderTaskItem.ShowFocusCues = showFocus;
			this.publishToWebTaskItem.ShowFocusCues = showFocus;
			this.shareFolderTaskItem.ShowFocusCues = showFocus;

			this.otherPlacesExpando.ShowFocusCues = showFocus;
			this.myDocumentsTaskItem.ShowFocusCues = showFocus;
			this.myPicturesTaskItem.ShowFocusCues = showFocus;
			this.myComputerTaskItem.ShowFocusCues = showFocus;
			this.myNetworkPlacesTaskItem.ShowFocusCues = showFocus;

			this.detailsExpando.ShowFocusCues = showFocus;
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
			if (this.controlComboBox.SelectedItem.ToString().Equals("TaskPane"))
			{
				this.customPropertyGrid.SelectedObject = this.customTaskPaneDescriptor;
			}
			else if (this.controlComboBox.SelectedItem.ToString().Equals("Expando"))
			{
				this.customPropertyGrid.SelectedObject = this.customExpandoDescriptor;
			}
			else if (this.controlComboBox.SelectedItem.ToString().Equals("TaskItem1"))
			{
				this.customPropertyGrid.SelectedObject = this.customTaskItem1Descriptor;
			}
			else if (this.controlComboBox.SelectedItem.ToString().Equals("TaskItem2"))
			{
				this.customPropertyGrid.SelectedObject = this.customTaskItem2Descriptor;
			}
			else if (this.controlComboBox.SelectedItem.ToString().Equals("TaskItem3"))
			{
				this.customPropertyGrid.SelectedObject = this.customTaskItem3Descriptor;
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void serializeFileButton_Click(object sender, System.EventArgs e)
		{
			IFormatter formatter = new BinaryFormatter();
			
			Stream stream = null;
			
			try
			{
				stream = new FileStream("Serialized XPExplorerBar.txt", FileMode.Create, FileAccess.Write, FileShare.None);
				
				TaskPane.TaskPaneSurrogate serializeTaskPaneSurrogate = new TaskPane.TaskPaneSurrogate(); 
				serializeTaskPaneSurrogate.Load(this.serializeTaskPane);
				
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
		private void deserializeFileButton_Click(object sender, System.EventArgs e)
		{
			if (this.serializeGroupBox.Controls.Count == 8)
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
				
				this.serializeGroupBox.Controls.Add(taskpane);
			}
			catch (System.Reflection.TargetInvocationException tie)
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
		private void serializeMemoryButton_Click(object sender, System.EventArgs e)
		{
			if (this.serializeGroupBox.Controls.Count == 8)
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
				serializeTaskPaneSurrogate.Load(this.serializeTaskPane);
				
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
				
				this.serializeGroupBox.Controls.Add(taskpane);
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
		private void removeButton_Click(object sender, System.EventArgs e)
		{
			foreach (Control control in this.serializeGroupBox.Controls)
			{
				if (control is TaskPane && control.Name.Equals("SerializedTaskPane"))
				{
					this.serializeGroupBox.Controls.Remove(control);

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
			 return TypeDescriptor.GetConverter(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this.owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.owner, attributes, true);
			
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
		PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
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
			return TypeDescriptor.GetEditor(this.owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this.owner, true);
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
			return TypeDescriptor.GetConverter(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this.owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.owner, attributes, true);
			
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
		PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
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
			return TypeDescriptor.GetEditor(this.owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this.owner, true);
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
			return TypeDescriptor.GetConverter(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this.owner, attributes, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pd"></param>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.owner;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.owner, attributes, true);
			
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
		PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
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
			return TypeDescriptor.GetEditor(this.owner, editorBaseType, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this.owner, true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this.owner, true);
		}
	}

	#endregion

	#endregion
}
