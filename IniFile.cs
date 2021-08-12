using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ShowTargetHealth
{
	class IniFile
	{
		string Path;
		string EXE = Assembly.GetExecutingAssembly().GetName().Name;

		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

		public IniFile(string IniPath = null)
		{
			Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
		}

		public string Read(string Key, string Section = null)
		{
			var RetVal = new StringBuilder(255);
			GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
			return RetVal.ToString();
		}

		public bool KeyExists(string Key, string Section = null)
		{
			return Read(Key, Section).Length > 0;
		}

		public bool Exists()
		{
			return File.Exists(Path);
		}
	}
}
