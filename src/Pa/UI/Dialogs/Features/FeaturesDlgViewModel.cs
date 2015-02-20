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
using System.Linq;
using Localization;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public class FeaturesDlgViewModel
	{
		public PaProject Project { get; private set; }
		
		private readonly PhoneInfo[] _phones;
		private readonly App.FeatureType _featureType;

		/// ------------------------------------------------------------------------------------
		public FeaturesDlgViewModel(PaProject project, App.FeatureType featureType)
		{
			Project = project;
			_featureType = featureType;

			_phones = (from p in project.PhoneCache.Values
					   where p is PhoneInfo
					   orderby p.POAKey
					   select p.Clone() as PhoneInfo).ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public int PhoneCount
		{
			get { return _phones.Length; }
		}

		/// ------------------------------------------------------------------------------------
		public string GetPhone(int index)
		{
			return (index < 0 || index >= _phones.Length ? null : _phones[index].Phone);
		}

		/// ------------------------------------------------------------------------------------
		public int GetPhoneCount(int index)
		{
			return (index < 0 || index >= _phones.Length ? 0 :
				_phones[index].TotalCount + _phones[index].CountAsPrimaryUncertainty);
		}

		/// ------------------------------------------------------------------------------------
		public string GetPhoneDescription(int index)
		{
			return (index < 0 || index >= _phones.Length || string.IsNullOrEmpty(_phones[index].Description) ?
				LocalizationManager.GetString("DialogBoxes.FeaturesDlgBase.NoPhoneDescriptionAvailableMsg", "(no phone description available)") :
				_phones[index].Description);
		}

		/// ------------------------------------------------------------------------------------
		public PhoneInfo GetPhoneInfo(int index)
		{
			return (index < 0 || index >= _phones.Length ? null : _phones[index]);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetListOfDefaultFeaturesForPhone(int index)
		{
			return (_featureType == App.FeatureType.Articulatory ?
				App.AFeatureCache.GetFeatureList(_phones[index].DefaultAMask) :
				App.BFeatureCache.GetFeatureList(_phones[index].DefaultBMask));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the user has changed any of the features
		/// for the phones in the phone inventory list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetDidAnyPhoneFeaturesChange()
		{
			return (from phone in _phones
					let origPhone = Project.PhoneCache[phone.Phone]
					where origPhone.AMask != phone.AMask || origPhone.BMask != phone.BMask
					select phone).Any();
		}

		/// ------------------------------------------------------------------------------------
		public void SaveChanges()
		{
			Project.UpdateFeatureOverrides(_phones.Where(p => p.HasAFeatureOverrides || p.HasBFeatureOverrides));
			App.Project.ReloadDataSources();
		}
	}
}
