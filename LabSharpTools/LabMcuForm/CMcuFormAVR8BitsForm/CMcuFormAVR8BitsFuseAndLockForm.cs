using Harry.LabTools.LabGenForm;
using Harry.LabTools.LabMcuFunc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabMcuForm
{
	public partial class CMcuFormAVR8BitsFuseAndLockForm :Form//CMcuFormBaseForm
	{

		#region 变量定义

		/// <summary>
		/// MCU功能的基类
		/// </summary>
		private CMcuFuncAVR8BitsBase defaultCMcuFunc = new CMcuFuncAVR8BitsISP();//new CMcuFuncBase();

		#endregion

		#region 属性定义

		/// <summary>
		/// MCU功能的基类
		/// </summary>
		public virtual CMcuFuncAVR8BitsBase mCMcuFunc
		{
			get
			{
				return this.defaultCMcuFunc;
			}
		}

		/// <summary>
		/// 系统类型
		/// </summary>
		private Boolean IsXpOr2003
		{
			get
			{
				OperatingSystem os = Environment.OSVersion;
				Version vs = os.Version;
				if (os.Platform == PlatformID.Win32NT)
				{
					if ((vs.Major == 5) && (vs.Minor != 0))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}

			}
		}

		/// <summary>
		/// 设置控件窗口创建参数的扩展风格
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				// Turn on WS_EX_COMPOSITED    
				cp.ExStyle |= 0x02000000;
				if (this.IsXpOr2003 == true)
				{
					// Turn on WS_EX_LAYERED  
					cp.ExStyle |= 0x00080000;
					//---设置窗体的不透明度
					this.Opacity = 1;
				}
				return cp;
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 无参构造函数
		/// </summary>
		public CMcuFormAVR8BitsFuseAndLockForm ()
		{
			InitializeComponent();
			//---注册事件函数
			this.RegistrationEventHandler();
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="chipName"></param>
		public CMcuFormAVR8BitsFuseAndLockForm(string chipName)
		{
			InitializeComponent();
			if (this.defaultCMcuFunc==null)
			{
				this.defaultCMcuFunc = new CMcuFuncAVR8BitsBase();
			}
			//---初始化芯片信息
			this.defaultCMcuFunc.mMcuInfoParam.McuTypeInfo(chipName);
			//---初始化控件信息
			this.cMcuFormAVR8BitsFuseAndLockControl1.Init(this.defaultCMcuFunc);
			//---注册事件函数
			this.RegistrationEventHandler();
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="chipCMcue"></param>
		public CMcuFormAVR8BitsFuseAndLockForm(CMcuFuncAVR8BitsBase chipCMcue)
		{
			InitializeComponent();
			//---检查自身类是否为空
			if (this.defaultCMcuFunc == null)
			{
				this.defaultCMcuFunc = new CMcuFuncAVR8BitsBase();
			}
			//---检查传递类是否为空
			if (chipCMcue!=null)
			{
				this.defaultCMcuFunc = chipCMcue;
				//---初始化控件信息
				this.cMcuFormAVR8BitsFuseAndLockControl1.Init(this.defaultCMcuFunc);
			}
			//---注册事件函数
			this.RegistrationEventHandler();
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="chipName"></param>
		/// <param name="chipCMcue"></param>
		public CMcuFormAVR8BitsFuseAndLockForm(string chipName,CMcuFuncAVR8BitsBase chipCMcue)
		{
			InitializeComponent();
			//---检验对象是否存在
			if (this.defaultCMcuFunc == null)
			{
				this.defaultCMcuFunc = new CMcuFuncAVR8BitsBase();
			}
			this.defaultCMcuFunc = chipCMcue;
			//---初始化芯片信息
			this.defaultCMcuFunc.mMcuInfoParam.McuTypeInfo(chipName);
			//---初始化控件信息
			this.cMcuFormAVR8BitsFuseAndLockControl1.Init(this.defaultCMcuFunc);
			//---注册事件函数
			this.RegistrationEventHandler();
		}
		#endregion

		#region 公共函数

		#endregion

		#region 保护函数

		#endregion

		#region 私有函数

		/// <summary>
		/// 注册事件函数
		/// </summary>
		private void RegistrationEventHandler()
		{
			this.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
		}

		#endregion

		#region 事件函数

		/// <summary>
		/// 窗体关闭事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_FormClosing(object sender, EventArgs e)
		{
			//---返回操作完成状态
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			this.defaultCMcuFunc.mMcuInfoParam.McuTypeInfo("atmega8");
			this.cMcuFormAVR8BitsFuseAndLockControl1.Init(this.defaultCMcuFunc);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//---创建窗体
			CMcuFormAVR8BitsFuseAndLockForm cMcuForm = new CMcuFormAVR8BitsFuseAndLockForm();
			//---C# 等待另外一个窗体关闭，再进行主线程的代码
			if (cMcuForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.defaultCMcuFunc = cMcuForm.mCMcuFunc;
				this.cMcuFormAVR8BitsFuseAndLockControl1.Init(this.defaultCMcuFunc);
			}
			//---资源释放
			cMcuForm.Dispose();
			
			//if (cMcuForm.ShowDialog(this.button2,-cMcuForm.Width/2, -cMcuForm.Height) == )
			//{

			//}
			//if (cMcuForm.ShowDialog()== System.Windows.Forms.DialogResult.OK)
			//{

			//}
		}
	}
}
