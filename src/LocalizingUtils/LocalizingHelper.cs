using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.ComponentModel.Design;
using System.Net;

namespace SIL.Localize.LocalizingUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class LocalizingHelper
	{
		private const string kAssemblyInfo =
			"using System.Reflection; [assembly: AssemblyCulture(\"{0}\")]";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compiles a collection of .resource files into a satellite resource DLL.
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="dllName">Name of the DLL (without extension) that was localized
		/// (e.g. Framework)</param>
		/// <param name="resNames">Array of localized resource files in .resource format. The
		/// name should include the full namespace and culture ID. For example:
		///		SIL.FieldWorks.Common.Framework.FrameworkStrings.fr.resources
		///		SIL.FieldWorks.Common.Framework.FwMainWnd.fr.resources
		///		etc.</param>
		///	<param name="exePath">Path of the executable program being localized.</param>
		/// <returns>true if compilation successful. Otherwise, false.</returns>
		/// ------------------------------------------------------------------------------------
		public static bool CompileLocalizedAssembly(string cultureId, string dllName,
			string[] resNames, string exePath)
		{
			CompilerError[] errors;
			return CompileLocalizedAssembly(cultureId, dllName, resNames, exePath, out errors);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compiles a collection of .resource files into a satellite resource DLL.
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="dllName">Name of the DLL (without extension) that was localized
		/// (e.g. Framework)</param>
		/// <param name="resNames">Array of localized resource files in .resources format. The
		/// name should include the full namespace and culture ID. For example:
		///		SIL.FieldWorks.Common.Framework.FrameworkStrings.fr.resources
		///		SIL.FieldWorks.Common.Framework.FwMainWnd.fr.resources
		///		etc.</param>
		///	<param name="exePath">Path of the executable program being localized.</param>
		/// <param name="errors"></param>
		/// <returns>true if compilation successful. Otherwise, false.</returns>
		/// ------------------------------------------------------------------------------------
		public static bool CompileLocalizedAssembly(string cultureId, string dllName,
			string[] resNames, string exePath, out CompilerError[] errors)
		{
			errors = null;
			string outPath = Path.Combine(exePath, cultureId);

			// Create the output directory if it does not exist. How will
			// this work if user doesn't have admin. rights to that folder?
			if (!Directory.Exists(outPath))
				Directory.CreateDirectory(outPath);

			CompilerParameters cp = new CompilerParameters();
			cp.OutputAssembly = Path.Combine(outPath, dllName + ".resources.dll");
			cp.WarningLevel = 3;
			cp.TreatWarningsAsErrors = false;
			cp.CompilerOptions = "/optimize";

			foreach (string res in resNames)
				cp.EmbeddedResources.Add(res);

			// The only code necessary is assembly info. to specify the assembly's culture.
			string[] code = new string[] { string.Format(kAssemblyInfo, cultureId) };

			// Invoke compilation.
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CompilerResults cr = provider.CompileAssemblyFromSource(cp, code);

			if (cr.Errors.Count == 0)
				return true;

			List<CompilerError> errs = new List<CompilerError>();
			foreach (CompilerError err in cr.Errors)
				errs.Add(err);

			errors = errs.ToArray();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int AssemblyResourceInfoComparer(AssemblyResourceInfo x, AssemblyResourceInfo y)
		{
			return string.Compare(x.AssemblyName, y.AssemblyName);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int ResourceInfoComparer(ResourceInfo x, ResourceInfo y)
		{
			return string.Compare(x.ResourceName, y.ResourceName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int ResourceEntryComparer(ResourceEntry x, ResourceEntry y)
		{
			return string.Compare(x.StringId, y.StringId);
		}
	}
}
