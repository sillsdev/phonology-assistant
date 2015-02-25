// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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
