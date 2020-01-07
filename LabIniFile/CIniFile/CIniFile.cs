using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Harry.LabTools.LabIniFile
{
	public partial class CIniFile
	{
		#region 变量定义

		/// <summary>
		/// 文件路劲
		/// </summary>
		private string defaultFilePath=string.Empty;

		#endregion

		#region 属性定义

		/// <summary>
		/// 文件路径
		/// </summary>
		public virtual string mFilePath
		{
			get
			{
				return this.defaultFilePath;
			}
		}

		/// <summary>
		/// 文件路径是否存在
		/// </summary>
		public virtual bool mPathExists
		{
			get
			{
				return (!(string.IsNullOrEmpty(this.defaultFilePath)) && (File.Exists(this.defaultFilePath)));
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 无参数构造函数
		/// </summary>
		public CIniFile()
		{
		
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="path"></param>
		public CIniFile(string path)
		{
			this.defaultFilePath = path;
		}

		#endregion

		#region 析构函数

		~CIniFile()
		{
			
		}

		#endregion

		#region 公共函数

		
		/// <summary>
		/// 清除某个Section
		/// </summary>
		/// <param name="Section"></param>
		public void EraseSection(string section)
		{
			// 
			if (!WritePrivateProfileString(section, null, null, this._fileName))
			{
				throw (new ApplicationException("无法清除Ini文件中的Section"));
			}
		}

		/// <summary>
		/// 删除某个Section下的键
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Ident"></param>
		public void DeleteKey(string section, string ident)
		{
			WritePrivateProfileString(section, ident, null, this._fileName);
		}

		/// <summary>
		/// 在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile 
		/// Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
		/// 执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
		/// </summary>
		public void UpdateFile()
		{
			WritePrivateProfileString(null, null, null, this._fileName);
		}

		/// <summary>
		/// 检查某个Section下的某个键值是否存在 
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Ident"></param>
		/// <returns></returns>
		public bool ValueExists(string section, string ident)
		{
			// 
			StringCollection Idents = new StringCollection();
			ReadSection(section, ref Idents);
			return Idents.IndexOf(ident) > -1;
		}
		#endregion

		#region 保护函数

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion

		#region API函数

		/// <summary>
		/// 声明读写INI文件的API函数 
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[DllImport("kernel32")]
		private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <param name="def"></param>
		/// <param name="retVal"></param>
		/// <param name="size"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
		#endregion

	}
}
