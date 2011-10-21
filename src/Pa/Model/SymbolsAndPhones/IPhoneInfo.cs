using System.Collections.Generic;

namespace SIL.Pa.Model
{
	public interface IPhoneInfo
	{
		IPhoneInfo Clone();

		bool HasAFeatureOverrides { get; }
		bool HasBFeatureOverrides { get; }

		//bool AFeaturesAreOverridden { get; set; }
		//bool BFeaturesAreOverridden { get; set; }

		string Phone { get; set; }
		string Description { get; set; }
		IPASymbolType CharType { get; set; }
		char BaseCharacter { get; set; }
		FeatureMask AMask { get; }
		FeatureMask BMask { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when not found in an uncertain group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int TotalCount { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the hex place of articulation sort key for the phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string POAKey { get; set; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the hex manner of articulation sort key for the phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string MOAKey { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the non primary phone
		/// in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int CountAsNonPrimaryUncertainty { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the primary phone
		/// (i.e. the first in group) in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int CountAsPrimaryUncertainty { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// List of phones found in the same uncertain group(s).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		List<string> SiblingUncertainties { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the symbols (or codepoints) of which the phone consists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPASymbol> GetSymbols();
	}
}
