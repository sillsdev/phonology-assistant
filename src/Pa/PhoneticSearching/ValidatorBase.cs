using System.Collections.Generic;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class ValidatorBase
	{
		protected readonly PaProject _project;

		public Dictionary<string, string> Errors { get; private set; }
		
		/// ------------------------------------------------------------------------------------
		public ValidatorBase(PaProject project)
		{
			_project = project;
			Errors = new Dictionary<string, string>();
		}

		/// ------------------------------------------------------------------------------------
		public bool HasErrors
		{
			get { return Errors.Count > 0; }
		}
	}
}
