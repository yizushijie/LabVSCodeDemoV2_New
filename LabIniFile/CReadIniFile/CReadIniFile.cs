using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Harry.LabTools.LabIniFile
{
	public partial class CIniFile
	{
		#region 公共函数

		/// <summary>
		/// 读取INI文件中指定的"小结"中的"键"值
		/// </summary>
		/// <param name="Section">小结名</param>
		/// <param name="Ident">键名</param>
		/// <param name="Default">默认值</param>
		/// <returns>读取的字符串</returns>
		public string CIniFileReadString(string section, string ident, string defaultValue)
		{
			byte[] Buffer = new byte[65535];
			//---读取数据
			int bufLen = GetPrivateProfileString(section, ident, defaultValue, Buffer, Buffer.GetUpperBound(0), this.defaultFilePath);
			//---必须设定0（系统默认的代码页）的编码方式，否则无法支持中文 
			string s = Encoding.GetEncoding(0).GetString(Buffer);
			s = s.Substring(0, bufLen);
			return s.Trim();
		}

		/// <summary>
		/// 从给定的"小结"中的"键"中读取整数 
		/// </summary>
		/// <param name="Section">小结</param>
		/// <param name="Ident">键</param>
		/// <param name="Default">默认值</param>
		/// <returns>读取的整数值</returns>
		public int CIniFileReadInt(string section, string ident, int defaultValue)
		{
			string intStr = CIniFileReadString(section, ident, Convert.ToString(defaultValue));
			try
			{
				return Convert.ToInt32(intStr);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return defaultValue;
			}
		}

		/// <summary>
		/// 从给定的"小结"中的"键"中读取布尔值
		/// </summary>
		/// <param name="Section">小结</param>
		/// <param name="Ident">键</param>
		/// <param name="Default">默认值</param>
		/// <returns>读取的布尔值</returns>
		public bool CIniFileReadBool(string section, string ident, bool defaultValue)
		{
			try
			{
				return Convert.ToBoolean(CIniFileReadString(section, ident, Convert.ToString(defaultValue)));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return defaultValue;
			}
		}

		/// <summary>
		/// 从Ini文件中，将指定的Section名称中的所有Ident添加到列表中 
		/// </summary>
		/// <param name="section">小结</param>
		/// <param name="idents">ref 返回"键"字符串列表</param>
		public void CIniFileReadSection(string section, ref StringCollection idents)
		{
			byte[] buffer = new byte[16384];
			if (idents != null)
			{
				idents.Clear();
			}
			else
			{
				idents = new StringCollection();
			}
			//---读取数据
			int bufLen = GetPrivateProfileString(section, null, null, buffer, buffer.GetUpperBound(0),this.defaultFilePath);
			//---对Section进行解析 
			BytesToString(buffer, bufLen, ref idents);
		}

		#endregion

		#region 保护函数

		#endregion

		#region 私有函数

		/// <summary>
		/// 从字节数组中获取字符串
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="bufLen">缓冲区长度</param>
		/// <param name="strings">字符串数组</param>
		private void BytesToString(byte[] buffer, int bufLen, ref StringCollection strings)
		{
			if (strings != null)
			{
				strings.Clear();
			}
			else
			{
				strings = new StringCollection();
			}				
			if (bufLen != 0)
			{
				int start = 0;
				for (int i = 0; i < bufLen; i++)
				{
					if ((buffer[i] == 0) && ((i - start) > 0))
					{
						string s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
						strings.Add(s);
						start = i + 1;
					}
				}
			}
		}

		#endregion

	}
}
