using Harry.LabTools.LabControlPlus;
using Harry.LabTools.LabGenFunc;
using Harry.LabTools.LabHexEdit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabMcuFunc
{
	public partial class CMcuFuncAVR8BitsBase
	{
		#region 变量定义

		#endregion

		#region 属性定义

		/// <summary>
		/// 功能函数的执行步序1
		/// </summary>
		public virtual int mFuncStep1Mask
		{
			get;
			set;
		}

		/// <summary>
		/// 功能函数的执行步序2
		/// </summary>
		public virtual int mFuncStep2Mask
		{
			get;
			set;
		}

		#endregion

		#region 构造函数

		#endregion

		#region 公有函数

		#region Hex文件操作

		#region 加载Flash

		/// <summary>
		/// 加载Flash文件
		/// </summary>
		/// <param name="flash"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadFlashHexFile(ref byte[] flash, RichTextBox msg)
		{
			int _return = -1;
			OpenFileDialog flashFile = new OpenFileDialog();
			flashFile.AddExtension = true;
			flashFile.DefaultExt = "hex";
			flashFile.Filter = "Intel Hex(*.hex)|*.hex|All files(*.*)|*.*";
			flashFile.FilterIndex = 0;
			//---选择文件
			if ((flashFile.ShowDialog() == DialogResult.OK) && (!string.IsNullOrEmpty(flashFile.FileName)))
			{
				CHexFile loadFlash = new CHexFile(flashFile.FileName);
				//---校验文件的解析
				if (loadFlash.mIsOK)
				{
					flash = new byte[loadFlash.mSTOPAddr];
					//---填充默认数据是0xFF
					CGenFuncMem.GenFuncMemset(ref flash, 0xFF);
					//---数组拷贝
					Array.Copy(loadFlash.mDataMap, 0, flash, loadFlash.mSTARTAddr, loadFlash.mDataMap.Length);
					//---消息显示
					if (msg != null)
					{
						CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, "调入Flash文件:" + flashFile.FileName, Color.Black);
					}
					_return = 0;
				}
				else
				{
					MessageBox.Show("加载Flash文件失败", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return _return;
		}

		/// <summary>
		/// 加载Flash文件
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadFlashHexFile(CHexBox chb, RichTextBox msg)
		{
			byte[] flash = null;
			//---加载Hex的数据
			int _return = this.CMcuFunc_LoadFlashHexFile(ref flash, msg);
			//---校验结果
			if ((_return == 0) && (flash != null))
			{
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
										(delegate
										{
											chb.AddData(flash, this.mMcuInfoParam.mChipFlashByteSize);
										}));
					}
					else
					{
						chb.AddData(flash, this.mMcuInfoParam.mChipFlashByteSize);
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 加载Flash文件
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadFlashHexFile(CHexBox chb, Label flashSize, RichTextBox msg)
		{
			byte[] flash = null;
			//---加载Hex的数据
			int _return = this.CMcuFunc_LoadFlashHexFile(ref flash, msg);
			//---校验结果
			if ((_return == 0) && (flash != null))
			{
				//---显示固件的大小
				if (flashSize != null)
				{
					if (flashSize.InvokeRequired)
					{
						flashSize.BeginInvoke((EventHandler)
										(delegate
										{
											flashSize.Text = flash.Length.ToString() + "/" + this.mMcuInfoParam.mChipFlashByteSize.ToString();
										}));
					}
					else
					{
						flashSize.Text = flash.Length.ToString() + "/" + this.mMcuInfoParam.mChipFlashByteSize.ToString();
					}
				}

				//---刷新数据到Hex控件中
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
										(delegate
										{
											chb.AddData(flash, this.mMcuInfoParam.mChipFlashByteSize);
										}));
					}
					else
					{
						chb.AddData(flash, this.mMcuInfoParam.mChipFlashByteSize);
					}
				}
			}
			return _return;
		}

		#endregion

		#region 保存Flash

		/// <summary>
		/// 保存Flash文件
		/// </summary>
		/// <param name="flash"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_SaveFlashHexFile(byte[] flash, RichTextBox msg)
		{
			if ((flash != null) && (flash.Length > 1))
			{
				SaveFileDialog flashFile = new SaveFileDialog();
				//---文件存在是是否提示覆盖
				flashFile.OverwritePrompt = false; 
				flashFile.DefaultExt = "hex";
				flashFile.Filter = "Intel Hex(*.hex)|*.hex|All files(*.*)|*.*";
				if (flashFile.ShowDialog() == DialogResult.OK)
				{
					//---Hex文件类
					CHexFile flashHexFile = new CHexFile();
					//---获取保存的数据
					string[] _return = flashHexFile.SaveHexLine(flash);
					//---将数据保存到指定文本中
					using (StreamWriter sw = new StreamWriter(flashFile.FileName))
					{
						for (int i = 0; i < _return.Length; i++)
						{
							sw.Write(_return[i]);
						}
						sw.Close();
					}
					//---消息显示
					if (msg != null)
					{
						CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, "保存Flash文件:" + flashFile.FileName, Color.Black);
					}
					return 0;
				}	
			}
			else
			{
				MessageBox.Show("保存Flash文件失败", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return -1;
		}

		/// <summary>
		/// 保存Flash文件
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_SaveFlashHexFile( CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_SaveFlashHexFile(chb.mDataMap,msg);
		}

		#endregion

		#region 加载Eeprom

		/// <summary>
		/// 加载Eeprom
		/// </summary>
		/// <param name="flash"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadEepromHexFile(ref byte[] eeprom, RichTextBox msg)
		{
			int _return = -1;
			OpenFileDialog eepromFile = new OpenFileDialog();
			eepromFile.AddExtension = true;
			eepromFile.DefaultExt = "hex";
			eepromFile.Filter = "Intel Hex(*.hex)|*.hex|All files(*.*)|*.*";
			eepromFile.FilterIndex = 0;
			//---选择文件
			if ((eepromFile.ShowDialog() == DialogResult.OK) && (!string.IsNullOrEmpty(eepromFile.FileName)))
			{
				CHexFile loadEeprom = new CHexFile(eepromFile.FileName);
				//---校验文件的解析
				if (loadEeprom.mIsOK)
				{
					eeprom = new byte[loadEeprom.mSTOPAddr];
					//---填充默认数据是0xFF
					CGenFuncMem.GenFuncMemset(ref eeprom, 0xFF);
					//---数组拷贝
					Array.Copy(loadEeprom.mDataMap, 0, eeprom, loadEeprom.mSTARTAddr, loadEeprom.mDataMap.Length);
					//---消息显示
					if (msg != null)
					{
						CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, "调入Eeprom文件:" + eepromFile.FileName, Color.Black);
					}
					_return = 0;
				}
				else
				{
					MessageBox.Show("Eeprom文件加载失败", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return _return;
		}

		/// <summary>
		/// 加载Eeprom
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadEepromHexFile(CHexBox chb, RichTextBox msg)
		{
			byte[] eeprom = null;
			//---加载Hex的数据
			int _return = this.CMcuFunc_LoadEepromHexFile(ref eeprom, msg);
			//---校验结果
			if ((_return == 0) && (eeprom != null))
			{
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
										(delegate
										{
											chb.AddData(eeprom, this.mMcuInfoParam.mChipEepromByteSize);
										}));
					}
					else
					{
						chb.AddData(eeprom, this.mMcuInfoParam.mChipEepromByteSize);
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 加载Eeprom
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_LoadEepromHexFile(CHexBox chb, Label eepromSize, RichTextBox msg)
		{
			byte[] eeprom = null;
			//---加载Hex的数据
			int _return = this.CMcuFunc_LoadEepromHexFile(ref eeprom, msg);
			//---校验结果
			if ((_return == 0) && (eeprom != null))
			{
				//---显示固件的大小
				if (eepromSize != null)
				{
					if (eepromSize.InvokeRequired)
					{
						eepromSize.BeginInvoke((EventHandler)
										(delegate
										{
											eepromSize.Text = eeprom.Length.ToString() + "/" + this.mMcuInfoParam.mChipEepromByteSize.ToString();
										}));
					}
					else
					{
						eepromSize.Text = eeprom.Length.ToString() + "/" + this.mMcuInfoParam.mChipEepromByteSize.ToString();
					}
				}

				//---刷新数据到Hex控件中
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
										(delegate
										{
											chb.AddData(eeprom, this.mMcuInfoParam.mChipEepromByteSize);
										}));
					}
					else
					{
						chb.AddData(eeprom, this.mMcuInfoParam.mChipEepromByteSize);
					}
				}
			}
			return _return;
		}

		#endregion

		#region 保存Eeprom

		/// <summary>
		/// 保存Flash文件
		/// </summary>
		/// <param name="eeprom"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_SaveEepromHexFile(byte[] eeprom, RichTextBox msg)
		{
			if ((eeprom != null) && (eeprom.Length > 1))
			{
				SaveFileDialog eepromFile = new SaveFileDialog();
				//---文件存在不提示提示覆盖
				eepromFile.OverwritePrompt = false;
				eepromFile.DefaultExt = "hex";
				eepromFile.Filter = "Intel Hex(*.hex)|*.hex|All files(*.*)|*.*";
				if (eepromFile.ShowDialog() == DialogResult.OK)
				{
					//---Hex文件类
					CHexFile flashHexFile = new CHexFile();
					//---获取保存的数据
					string[] _return = flashHexFile.SaveHexLine(eeprom);
					//---将数据保存到指定文本中
					using (StreamWriter sw = new StreamWriter(eepromFile.FileName))
					{
						for (int i = 0; i < _return.Length; i++)
						{
							sw.Write(_return[i]);
						}
						sw.Close();
					}
					//---消息显示
					if (msg != null)
					{
						CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, "保存Eeprom文件:" + eepromFile.FileName, Color.Black);
					}
					return 0;
				}
			}
			else
			{
				MessageBox.Show("保存Eeprom文件失败", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return -1;
		}

		/// <summary>
		/// 保存Flash文件
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_SaveEepromHexFile(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_SaveEepromHexFile(chb.mDataMap, msg);
		}

		#endregion

		#endregion

		#region 编程常规使用函数

		/// <summary>
		/// 打开连接
		/// </summary>
		/// <returns></returns>
		public virtual int CMcuFunc_OpenConnect(RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 关闭连接
		/// </summary>
		/// <returns></returns>
		public virtual int CMcuFunc_CloseConnect(RichTextBox msg)
		{
			return -1;
		}


		/// <summary>
		/// 芯片擦除
		/// </summary>
		/// <returns></returns>
		public virtual int CMcuFunc_EraseChip( RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 芯片擦除
		/// </summary>
		/// <returns></returns>
		public virtual int CMcuFunc_EraseChip(TextBox lockFuse,RichTextBox msg)
		{
			//---擦除芯片
			int _return = this.CMcuFunc_EraseChip(msg);
			//---校验结果
			if ((_return == 0) && (lockFuse != null))
			{
				if (lockFuse.InvokeRequired)
				{
					lockFuse.BeginInvoke((EventHandler)
											(delegate
											{
												lockFuse.Text = "FF";
											}));
				}
				else
				{
					lockFuse.Text = "FF";
				}
			}
			return _return;
		}

		/// <summary>
		/// 读取Flash
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFlash(ref byte[] chipFlash, RichTextBox msg,ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "读取Flash")
		{
			return -1;
		}

		/// <summary>
		/// 读取Flash
		/// </summary>
		/// <param name="chipFlash"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFlash(ref byte[] chipFlash, RichTextBox msg)
		{
			return this.CMcuFunc_ReadChipFlash(ref chipFlash, msg, null, null, null);
		}
		
		/// <summary>
		/// 读取Flash
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <param name="workState"></param>
		/// <param name="workTime"></param>
		/// <param name="workBar"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFlash(CHexBox chb, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "读取Flash")
		{
			byte[] flash = null;
			//---读取Flash数据
			int _return = this.CMcuFunc_ReadChipFlash(ref flash, msg,workState,workTime,workBar,str);
			//---校验结果
			if ((_return == 0) && (flash != null))
			{
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
												(delegate
												{
													chb.AddData(flash);
												}));
					}
					else
					{
						chb.AddData(flash);
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 读取Flash
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFlash(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_ReadChipFlash(chb, msg, null, null, null);
		}
		
		/// <summary>
		/// 编程Flash
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFlash(byte[] chipFlash, RichTextBox msg, bool isAuto = false, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "编程Flash")
		{
			return -1;
		}

		/// <summary>
		/// 编程Flash
		/// </summary>
		/// <param name="chipFlash"></param>
		/// <param name="msg"></param>
		/// <param name="isAuto"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFlash(byte[] chipFlash, RichTextBox msg, bool isAuto = false)
		{
			return this.CMcuFunc_WriteChipFlash(chipFlash, msg, isAuto, null, null, null);
		}


		/// <summary>
		/// 编程Flash
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFlash(CHexBox chb, RichTextBox msg, bool isAuto = false, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "编程Flash")
		{
			return this.CMcuFunc_WriteChipFlash(chb.mDataMap, msg, isAuto, workState,workTime,workBar,str);
		}

		/// <summary>
		/// 编程Flash
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFlash(CHexBox chb, RichTextBox msg, bool isAuto = false)
		{
			return this.CMcuFunc_WriteChipFlash(chb, msg,isAuto,null,null,null);
		}

		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipFlash(byte[] chipFlash, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "校验Flash")
		{
			return -1;
		}

		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="chipFlash"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipFlash(byte[] chipFlash, RichTextBox msg)
		{
			return this.CMcuFunc_CheckChipFlash(chipFlash,msg,null,null,null);
		}

		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipFlash(CHexBox chb, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "校验Flash")
		{
			return this.CMcuFunc_CheckChipFlash(chb.mDataMap, msg,workState,workTime,workBar,str);
		}

		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipFlash(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_CheckChipFlash(chb, msg,null,null,null);
		}

		/// <summary>
		/// 校验Flash为空
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipFlashEmpty(RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取Eeprom
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipEeprom(ref byte[] chipEeprom, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "读取Eeprom")
		{
			return -1;
		}

		/// <summary>
		/// 读取Eeprom
		/// </summary>
		/// <param name="chipEeprom"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipEeprom(ref byte[] chipEeprom, RichTextBox msg)
		{
			return this.CMcuFunc_ReadChipEeprom(ref chipEeprom,msg,null,null,null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <param name="workState"></param>
		/// <param name="workTime"></param>
		/// <param name="workBar"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipEeprom(CHexBox chb, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "读取Eeprom")
		{
			byte[] eeprom = null;
			//---读取Eeprom数据
			int _return = this.CMcuFunc_ReadChipEeprom(ref eeprom, msg,workState,workTime,workBar,str);
			//---校验结果
			if ((_return == 0) && (eeprom != null))
			{
				if (chb != null)
				{
					if (chb.InvokeRequired)
					{
						chb.BeginInvoke((EventHandler)
												(delegate
												{
													chb.AddData(eeprom);
												}));
					}
					else
					{
						chb.AddData(eeprom);
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 读取Eeprom
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipEeprom(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_ReadChipEeprom(chb, msg, null, null, null);
		}

		/// <summary>
		/// 编程Eeprom
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipEeprom(byte[] chipEeprom, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null,string str = "编程Eeprom")
		{
			return -1;
		}

		/// <summary>
		/// 读取Eeprom
		/// </summary>
		/// <param name="chipEeprom"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipEeprom(byte[] chipEeprom, RichTextBox msg)
		{
			return this.CMcuFunc_WriteChipEeprom(chipEeprom,msg,null,null,null);
		}

		/// <summary>
		/// 编程Eeprom
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipEeprom(CHexBox chb, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "编程Eeprom")
		{
			return this.CMcuFunc_WriteChipEeprom(chb.mDataMap, msg,workState,workTime,workBar,str);
		}

		/// <summary>
		/// 读取Eeprom
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipEeprom(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_WriteChipEeprom(chb, msg, null,null,null);
		}


		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="flash"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipEeprom(byte[] chipEeprom, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "校验Eeprom")
		{
			return -1;
		}

		/// <summary>
		/// 校验eeprom
		/// </summary>
		/// <param name="chipEeprom"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipEeprom(byte[] chipEeprom, RichTextBox msg)
		{
			return this.CMcuFunc_CheckChipEeprom(chipEeprom, msg, null, null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chb"></param>
		/// <param name="msg"></param>
		/// <param name="workState"></param>
		/// <param name="workTime"></param>
		/// <param name="workBar"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipEeprom(CHexBox chb, RichTextBox msg, ToolStripLabel workState = null, ToolStripLabel workTime = null, ToolStripProgressBar workBar = null, string str = "校验Eeprom")
		{
			return this.CMcuFunc_CheckChipEeprom(chb.mDataMap, msg, workState, workTime, workBar,str);
		}

		/// <summary>
		/// 校验Flash
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipEeprom(CHexBox chb, RichTextBox msg)
		{
			return this.CMcuFunc_CheckChipEeprom(chb, msg, null, null, null);
		}

		/// <summary>
		/// 校验Eeprom为空
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_CheckChipEepromEmpty(RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取熔丝位
		/// </summary>
		/// <param name="fuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFuse(ref byte[] chipFuse, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取熔丝位
		/// </summary>
		/// <param name="chipFuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipFuse(TextBox lowFuse, TextBox highFuse, TextBox externFuse, RichTextBox msg)
		{
			byte[] fuse = null;
			//---读取熔丝位
			int _return = this.CMcuFunc_ReadChipFuse(ref fuse, msg);
			//---判断读取结果
			if ((_return == 0) && (fuse != null) && (fuse.Length > 1))
			{
				//---低位熔丝位
				if (lowFuse != null)
				{
					if (lowFuse.InvokeRequired)
					{
						lowFuse.BeginInvoke((EventHandler)
											(delegate
											{
												lowFuse.Text = fuse[0].ToString("X2");
											}));
					}
					else
					{
						lowFuse.Text = fuse[0].ToString("X2");
					}
				}

				//---高位熔丝位
				if (highFuse != null)
				{
					if (highFuse.InvokeRequired)
					{
						highFuse.BeginInvoke((EventHandler)
												(delegate
												{
													highFuse.Text = fuse[1].ToString("X2");
												}));
					}
					else
					{
						highFuse.Text = fuse[1].ToString("X2");
					}
				}

				//---拓展位熔丝位
				if (externFuse != null)
				{
					int tempFuse = 0;
					if (fuse.Length > 2)
					{
						tempFuse = fuse[2];
					}
					else
					{
						tempFuse = 0;
					}
					if (externFuse.InvokeRequired)
					{
						externFuse.BeginInvoke((EventHandler)
											(delegate
											{
												externFuse.Text = tempFuse.ToString("X2");
											}));
					}
					else
					{
						externFuse.Text = tempFuse.ToString("X2");
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 默认熔丝位
		/// </summary>
		/// <param name="chipFuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_DefaultChipFuse(TextBox lowFuse, TextBox highFuse, TextBox externFuse, RichTextBox msg)
		{
			int _return = 0;
			//---获取熔丝位
			int[] fuse = this.mMcuInfoParam.McuDefaultFuseInfo();
			//---校验熔丝位
			if ((fuse == null) || (fuse.Length < 2))
			{
				_return = 1;
			}
			else
			{
				//---低位熔丝位
				if (lowFuse != null)
				{
					if (lowFuse.InvokeRequired)
					{
						lowFuse.BeginInvoke((EventHandler)
											(delegate
											{
												lowFuse.Text = fuse[0].ToString("X2");
											}));
					}
					else
					{
						lowFuse.Text = fuse[0].ToString("X2");
					}
				}

				//---高位熔丝位
				if (highFuse != null)
				{
					if (highFuse.InvokeRequired)
					{
						highFuse.BeginInvoke((EventHandler)
												(delegate
												{
													highFuse.Text = fuse[1].ToString("X2");
												}));
					}
					else
					{
						highFuse.Text = fuse[1].ToString("X2");
					}
				}

				//---拓展位熔丝位
				if (externFuse != null)
				{
					int tempFuse = 0;
					if (fuse.Length > 2)
					{
						tempFuse = fuse[2];
					}
					else
					{
						tempFuse = 0;
					}
					if (externFuse.InvokeRequired)
					{
						externFuse.BeginInvoke((EventHandler)
											(delegate
											{
												externFuse.Text = tempFuse.ToString("X2");
											}));
					}
					else
					{
						externFuse.Text = tempFuse.ToString("X2");
					}
				}

				if (msg != null)
				{
					CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, "默认熔丝位恢复成功!", Color.Black);
				}
			}
			return _return;
		}

		/// <summary>
		/// 读取加密位
		/// </summary>
		/// <param name="fuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipLock(ref byte chipLock, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取加密位
		/// </summary>
		/// <param name="lockFuse"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipLock(TextBox lockFuse, RichTextBox msg)
		{
			byte tempLock = 0x00;
			//---读取加密位
			int _return = this.CMcuFunc_ReadChipLock(ref tempLock, msg);
			//---校验读取结果
			if (_return != 0)
			{
				tempLock = 0xFF;
			}
			//---加密位信息
			if (lockFuse != null)
			{
				//---低位熔丝位
				if (lockFuse.InvokeRequired)
				{
					lockFuse.BeginInvoke((EventHandler)
										(delegate
										{
											lockFuse.Text = tempLock.ToString("X2");
										}));
				}
				else
				{
					lockFuse.Text = tempLock.ToString("X2");
				}
			}
			return _return;
		}

		/// <summary>
		/// 写入熔丝位
		/// </summary>
		/// <param name="fuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFuse( byte[] chipFuse, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 写入熔丝位
		/// </summary>
		/// <param name="chipFuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipFuse(TextBox lowFuse, TextBox highFuse, TextBox externFuse, RichTextBox msg)
		{
			return this.CMcuFunc_WriteChipFuse(new byte[] { Convert.ToByte(lowFuse.Text, 16), Convert.ToByte(highFuse.Text, 16), Convert.ToByte(externFuse.Text, 16) }, msg);
		}

		/// <summary>
		/// 写入加密位
		/// </summary>
		/// <param name="fuse"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipLock( byte chipLock, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 写入加密位
		/// </summary>
		/// <param name="lockFuse"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_WriteChipLock(TextBox lockFuse, RichTextBox msg)
		{
			return this.CMcuFunc_WriteChipLock(Convert.ToByte(lockFuse.Text, 16), msg);
		}

		/// <summary>
		/// 读取芯片的ID信息
		/// </summary>
		/// <param name="chipID"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipID(ref byte[] chipID, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取芯片的ID信息
		/// </summary>
		/// <param name="chipID"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipID(RichTextBox msg,Form form=null)
		{
			byte[] tempID = null;
			//---读取ChipID
			int _return = this.CMcuFunc_ReadChipID(ref tempID, msg);
			//---校验读取结果
			if (_return == 0)
			{
				if (CGenFuncEqual.GenFuncEqual(tempID, this.mMcuInfoParam.mChipID) == false)
				{
					if (form != null)
					{
						CMessageBoxPlus.Show(form, "芯片识别字不匹配\r\n读出识别字：" +
													tempID[0].ToString("X2") + ":" + tempID[1].ToString("X2") + ":" + tempID[2].ToString("X2"),
													"消息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					}
					else
					{
						MessageBox.Show("芯片识别字不匹配\r\n读出识别字：" +
													tempID[0].ToString("X2") + ":" + tempID[1].ToString("X2") + ":" + tempID[2].ToString("X2"),
													"消息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					}
				}

			}
			return _return;
		}

		/// <summary>
		/// 读取校准字
		/// </summary>
		/// <param name="chipCalibration"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipCalibration(ref byte[] chipCalibration, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取校准字
		/// </summary>
		/// <param name="chipCalibration"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipCalibration( TextBox oscValue1, TextBox oscValue2, TextBox oscValue3, TextBox oscValue4, RichTextBox msg)
		{
			byte[] chipCalibration = null;
			//---读取校准字
			int _return = this.CMcuFunc_ReadChipCalibration(ref chipCalibration, msg);
			//---校验读取结果
			if ((_return == 0) && (chipCalibration != null))
			{
				for (int i = 0; i < chipCalibration.Length; i++)
				{
					switch (i)
					{
						case 0:
							if (oscValue1.Visible == true)
							{
								if (oscValue1.InvokeRequired)
								{
									oscValue1.BeginInvoke((EventHandler)
											(delegate
											{
												oscValue1.Text = chipCalibration[i].ToString("X2");
											}));
								}
								else
								{
									oscValue1.Text = chipCalibration[i].ToString("X2");
								}
							}
							break;
						case 1:
							if (oscValue2.Visible == true)
							{
								if (oscValue2.InvokeRequired)
								{
									oscValue2.BeginInvoke((EventHandler)
											(delegate
											{
												oscValue2.Text = chipCalibration[i].ToString("X2");
											}));
								}
								else
								{
									oscValue2.Text = chipCalibration[i].ToString("X2");
								}
							}
							break;
						case 2:
							if (oscValue3.Visible == true)
							{
								if (oscValue3.InvokeRequired)
								{
									oscValue3.BeginInvoke((EventHandler)
											(delegate
											{
												oscValue3.Text = chipCalibration[i].ToString("X2");
											}));
								}
								else
								{
									oscValue3.Text = chipCalibration[i].ToString("X2");
								}
							}
							break;
						case 3:
							if (oscValue4.Visible == true)
							{
								if (oscValue4.InvokeRequired)
								{
									oscValue4.BeginInvoke((EventHandler)
											(delegate
											{
												oscValue4.Text = chipCalibration[i].ToString("X2");
											}));
								}
								else
								{
									oscValue4.Text = chipCalibration[i].ToString("X2");
								}
							}
							break;
						default:
							break;
					}
				}
			}
			return _return;
		}

		/// <summary>
		/// 读取ROM信息
		/// </summary>
		/// <param name="chipRom"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipRom(ref byte[] chipRom, RichTextBox msg)
		{
			return -1;
		}

		/// <summary>
		/// 读取ROM信息
		/// </summary>
		/// <param name="chb">Hex编辑器控件</param>
		/// <returns></returns>
		public virtual int CMcuFunc_ReadChipRom(CHexBox chb, RichTextBox msg)
		{
			int _return = -1;
			byte[] tempRom = null;
			_return = this.CMcuFunc_ReadChipRom(ref tempRom,msg);
			if ((_return == 0) && (tempRom != null))
			{
				if (chb.InvokeRequired)
				{
					chb.BeginInvoke((EventHandler)
										(delegate
										{
											chb.AddData(tempRom);
										}));
				}
				else
				{
					chb.AddData(tempRom);
				}
			}
			return _return;
		}

		/// <summary>
		/// 编程时钟设置
		/// </summary>
		/// <param name="chipClock"></param>
		/// <returns></returns>
		public virtual int CMcuFunc_SetProg(byte chipClock, RichTextBox msg)
		{
			return -1;
		}

		#endregion

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion
	}
}