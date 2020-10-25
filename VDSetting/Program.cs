using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDSetting
{
	static class Program
	{
		private static readonly string s_MutexName = "caitsithware.VDSetting";
		private static PropertyForm PropertyForm;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool createdNew;
			System.Threading.Mutex mutex = new System.Threading.Mutex(true, s_MutexName, out createdNew);

			if (!createdNew)
			{
				MessageBox.Show("多重起動はできません。", "VDSetting");
				mutex.Close();
				return;
			}

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				PropertyForm = new PropertyForm();
				Application.Run(PropertyForm);
			}
			finally
			{
				mutex.ReleaseMutex();
				mutex.Close();
			}
		}
	}
}
