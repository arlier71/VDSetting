using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace VDSetting
{
	public class Property
	{
		private PropertyData _PropertyData;

		[Browsable(false)]
		public uint DefaultDragWidth
		{
			get;
			set;
		}

		[Browsable(false)]
		public uint DefaultDragHeight
		{
			get;
			set;
		}

		[Browsable(false)]
		public uint DefaultDoubleClickWidth
		{
			get;
			set;
		}

		[Browsable(false)]
		public uint DefaultDoubleClickHeight
		{
			get;
			set;
		}

		public bool RevertToDefaultOnQuit
		{
			get
			{
				return _PropertyData.RevertToDefaultOnQuit;
			}

			set
			{
				_PropertyData.RevertToDefaultOnQuit = value;
				Save();
			}
		}

		public uint DragWidth
		{
			get
			{
				return _PropertyData.DragWidth;
			}
			set
			{
				_PropertyData.DragWidth = value;
				ApplyDragWidth(value);
				Save();
			}
		}

		public uint DragHeight
		{
			get
			{
				return _PropertyData.DragHeight;
			}
			set
			{
				_PropertyData.DragHeight = value;
				ApplyDragHeight(value);
				Save();
			}
		}

		public uint DoubleClickWidth
		{
			get
			{
				return _PropertyData.DoubleClickWidth;
			}
			set
			{
				_PropertyData.DoubleClickWidth = value;
				ApplyDoubleClickWidth(value);
				Save();
			}
		}

		public uint DoubleClickHeight
		{
			get
			{
				return _PropertyData.DoubleClickHeight;
			}
			set
			{
				_PropertyData.DoubleClickHeight = value;
				ApplyDoubleClickHeight(value);
				Save();
			}
		}

		private string _ConfFilePath;

		public Property()
		{
			DefaultDragWidth = (uint)GetSystemMetrics(SystemMetric.SM_CXDRAG);
			DefaultDragHeight = (uint)GetSystemMetrics(SystemMetric.SM_CYDRAG);
			DefaultDoubleClickWidth = (uint)GetSystemMetrics(SystemMetric.SM_CXDOUBLECLK);
			DefaultDoubleClickHeight = (uint)GetSystemMetrics(SystemMetric.SM_CYDOUBLECLK);

			string exePath = Application.ExecutablePath;
			string exeDir = Path.GetDirectoryName(exePath);
			_ConfFilePath = Path.Combine(exeDir, "config.json");

			if (!Load())
			{
				_PropertyData = new PropertyData();

				_PropertyData.RevertToDefaultOnQuit = true;

				_PropertyData.DragWidth = DefaultDragWidth;
				_PropertyData.DragHeight = DefaultDragHeight;
				_PropertyData.DoubleClickWidth = DefaultDoubleClickWidth;
				_PropertyData.DoubleClickHeight = DefaultDoubleClickHeight;
			}
		}

		bool Load()
		{
			if (!File.Exists(_ConfFilePath))
			{
				return false;
			}

			string jsonString = File.ReadAllText(_ConfFilePath);
			_PropertyData = JsonSerializer.Deserialize<PropertyData>(jsonString);
			Apply();

			return true;
		}

		void Save()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};
			string jsonString = JsonSerializer.Serialize<PropertyData>(_PropertyData, options);
			File.WriteAllText(_ConfFilePath, jsonString);
		}

		void Apply()
		{
			ApplyDragWidth(_PropertyData.DragWidth);
			ApplyDragHeight(_PropertyData.DragHeight);
			ApplyDoubleClickWidth(_PropertyData.DoubleClickWidth);
			ApplyDoubleClickHeight(_PropertyData.DoubleClickHeight);
		}

		void ApplyDragWidth(uint value)
		{
			SystemParametersInfoSet(SPI.SPI_SETDRAGWIDTH, value, 0, SPIF.SPIF_SENDCHANGE);
		}

		void ApplyDragHeight(uint value)
		{
			SystemParametersInfoSet(SPI.SPI_SETDRAGHEIGHT, value, 0, SPIF.SPIF_SENDCHANGE);
		}

		void ApplyDoubleClickWidth(uint value)
		{
			SystemParametersInfoSet(SPI.SPI_SETDOUBLECLKWIDTH, value, 0, SPIF.SPIF_SENDCHANGE);
		}

		void ApplyDoubleClickHeight(uint value)
		{
			SystemParametersInfoSet(SPI.SPI_SETDOUBLECLKHEIGHT, value, 0, SPIF.SPIF_SENDCHANGE);
		}

		public void RevertToDefault()
		{
			ApplyDragWidth(DefaultDragWidth);
			ApplyDragHeight(DefaultDragHeight);
			ApplyDoubleClickWidth(DefaultDoubleClickWidth);
			ApplyDoubleClickHeight(DefaultDoubleClickHeight);
		}

		private bool ShouldSerializeDragWidth()
		{
			return DragWidth != DefaultDragWidth;
		}

		private bool ShouldSerializeDragHeight()
		{
			return DragHeight != DefaultDragHeight;
		}

		private bool ShouldSerializeDoubleClickWidth()
		{
			return DoubleClickWidth != DefaultDoubleClickWidth;
		}

		private bool ShouldSerializeDoubleClickHeight()
		{
			return DoubleClickHeight != DefaultDoubleClickHeight;
		}

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(SystemMetric smIndex);

		[DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SystemParametersInfoSet(SPI action, uint param, uint vparam, SPIF init);
	}
}
