using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDSetting
{
	public class PropertyData
	{
		public uint DragWidth
		{
			get;
			set;
		}

		public uint DragHeight
		{
			get;
			set;
		}

		public uint DoubleClickWidth
		{
			get;
			set;
		}

		public uint DoubleClickHeight
		{
			get;
			set;
		}

		public bool RevertToDefaultOnQuit
		{
			get;
			set;
		}
	}
}
