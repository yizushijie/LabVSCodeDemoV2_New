using Harry.LabTools.LabCommType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.LabTools.LabMcuFunc
{

	
	/// <summary>
	/// 这里主要是解析设备的参数信息
	/// </summary>
	public partial class CMcuFuncAVR8BitsBase
	{
		#region 变量定义
		
		/// <summary>
		/// 使用的通讯端口信息
		/// </summary>
		private CCommBase defaultCCOMM = null;

		/// <summary>
		/// 使用的MCU信息,默认初始化AVR的8Bit的Mcu
		/// </summary>
		private CMcuFuncInfoAVR8BitsParam defaultMcuInfoParam =null;

		/// <summary>
		/// 消息显示信息
		/// </summary>
		private string defaultMsgText = string.Empty;

		#endregion

		#region 属性定义

		
		/// <summary>
		/// 通讯使用的端口为读写属性
		/// </summary>
		public virtual CCommBase mCCOMM
		{
			get
			{
				return this.defaultCCOMM;
			}
			set
			{
				if (value == null)
				{
					this.defaultCCOMM = new CCommBase();
				}
				this.defaultCCOMM = value;
			}
		}

		/// <summary>
		/// 使用的MCU参数
		/// </summary>
		public virtual CMcuFuncInfoAVR8BitsParam mMcuInfoParam
		{
			get
			{
				return this.defaultMcuInfoParam;
			}
			set
			{
				if (value==null)
				{
					this.defaultMcuInfoParam = new CMcuFuncInfoAVR8BitsParam();
				}
				this.defaultMcuInfoParam = value;
			}
		}

		/// <summary>
		/// 消息提示
		/// </summary>
		public virtual string mMsgText
		{
			get
			{
				return this.defaultMsgText;			
			}
			set
			{
				this.defaultMsgText = value;
			}
		}

		/// <summary>
		/// 编程时钟的属性为读写属性
		/// </summary>
		public virtual string[] mChipClock
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 带参数的构造函数
		/// </summary>
		/// <param name="usedComm">使用的通讯端口</param>
		public CMcuFuncAVR8BitsBase(CCommBase usedComm)
		{
			if (usedComm!=null)
			{
				if (this.defaultCCOMM==null)
				{
					this.defaultCCOMM = new CCommBase();
				}
				this.defaultCCOMM = usedComm;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mcuInfoParam"></param>
		public CMcuFuncAVR8BitsBase(CMcuFuncInfoAVR8BitsParam mcuInfoParam)
		{
			if (mcuInfoParam != null)
			{
				if (this.defaultMcuInfoParam == null)
				{
					this.defaultMcuInfoParam = new CMcuFuncInfoAVR8BitsParam();
				}
				this.defaultMcuInfoParam = mcuInfoParam;
			}
		}

		#endregion

		#region 公有函数

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion
	}
}

