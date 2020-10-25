using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VDSetting
{
	public partial class PropertyForm : Form
	{
		private Property Property;
		
		public PropertyForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Property = new Property();
			propertyGrid1.SelectedObject = Property;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenForm();
		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				OpenForm();
			}
		}

		void OpenForm()
		{
			notifyIcon1.Visible = false;
			Visible = true;
			if (WindowState == FormWindowState.Minimized)
			{
				WindowState = FormWindowState.Normal;
			}
			Activate();
		}

		void QuitForm()
		{
			notifyIcon1.Visible = false;
			if (Property.RevertToDefaultOnQuit)
			{
				Property.RevertToDefault();
			}
			Application.Exit();
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QuitForm();
		}

		private void PropertyForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.ApplicationExitCall)
			{
				QuitForm();
			}
		}

		private void PropertyForm_SizeChanged(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				notifyIcon1.Visible = true;
				Visible = false;
			}
		}
	}
}
