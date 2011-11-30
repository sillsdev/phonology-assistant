using System.Collections.Generic;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class ValidatorBase
	{
		protected readonly PaProject _project;

		public List<SearchQueryValidationError> Errors { get; private set; }
		
		/// ------------------------------------------------------------------------------------
		public ValidatorBase(PaProject project)
		{
			_project = project;
			Errors = new List<SearchQueryValidationError>();
		}

		/// ------------------------------------------------------------------------------------
		public bool HasErrors
		{
			get { return Errors.Count > 0; }
		}
	}
}
