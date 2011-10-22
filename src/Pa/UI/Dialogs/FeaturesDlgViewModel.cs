using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public class FeaturesDlgViewModel
	{
		public PaProject Project { get; private set; }
		
		private readonly PhoneInfo[] _phones;

		/// ------------------------------------------------------------------------------------
		public FeaturesDlgViewModel(PaProject project)
		{
			Project = project;

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
				App.GetString("DialogBoxes.FeaturesDlg.CommonStrings.NoPhoneDescriptionAvailableText", "(no phone description available)") :
				_phones[index].Description);
		}

		/// ------------------------------------------------------------------------------------
		public PhoneInfo GetPhoneInfo(int index)
		{
			return (index < 0 || index >= _phones.Length ? null : _phones[index]);
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
