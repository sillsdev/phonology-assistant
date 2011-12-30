using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// Class for handling phones whose features cannot be accuratly deduced by simply taking
	/// the result of performing a bitwise OR on the features of each codepoint making up the
	/// phone. Therefore, phones in this list have their features overridden.
	/// ----------------------------------------------------------------------------------------
	public class FeatureOverrides : List<FeatureOverride>
	{
		private const string kCurrVersion = "3.3.3";
		public const string kFileName = "FeatureOverrides.xml";

		private readonly PaProject _project;

		/// ------------------------------------------------------------------------------------
		public FeatureOverrides(PaProject project)
		{
			_project = project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default and project-specific list of overriding phone features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FeatureOverrides Load(PaProject project)
		{
			var overrides = new FeatureOverrides(project);

			var filename = GetFileForProject(project.ProjectPathFilePrefix);
			if (!File.Exists(filename))
				return overrides;

			var root = XElement.Load(filename);
			if (root.Nodes().Count() == 0)
				return overrides;

			AddDescriptiveFeatureOverrides(root, ref overrides);
			AddDistinctiveFeatureOverrides(root, project, ref overrides);
			return overrides;
		}

		/// ------------------------------------------------------------------------------------
		private static void AddDescriptiveFeatureOverrides(XElement root, ref FeatureOverrides overrides)
		{
			var featureTypeElement = root.Elements("featureType")
				.FirstOrDefault(e => (string)e.Attribute("class") == "descriptive");

			if (featureTypeElement == null)
				return;

			// Get only the overriding features if they can be found in the descriptive feature cache.
			var list = from element in featureTypeElement.Elements("featureOverride")
					   let phone = (string)element.Attribute("segment")
					   let fnames = element.Elements("feature").Select(e => e.Value).Where(fname => App.AFeatureCache.Keys.Any(n => n == fname))
					   where !string.IsNullOrEmpty(phone) && fnames != null && fnames.Count() > 0
					   select new FeatureOverride { Phone = phone, AFeatureNames = fnames.ToArray() };

			overrides.AddRange(list);
		}
		
		/// ------------------------------------------------------------------------------------
		private static void AddDistinctiveFeatureOverrides(XElement root, PaProject project,
			ref FeatureOverrides overrides)
		{
			var featureTypeElement = root.Elements("featureType").FirstOrDefault(e =>
				(string)e.Attribute("class") == "distinctive" && (string)e.Attribute("set") == project.DistinctiveFeatureSet);

			if (featureTypeElement == null)
				return;
			
			foreach (var element in featureTypeElement.Elements("featureOverride"))
			{
				var phone = element.Attribute("segment").Value;
				
				// Get only the overriding feature names if they
				// can be found in the distinctive feature cache.
				var fnames = element.Elements("feature").Select(e => e.Value)
					.Where(fname => project.BFeatureCache.Keys.Any(n => n == fname)).ToArray();

				if (string.IsNullOrEmpty(phone) || fnames.Length == 0)
					continue;

				var foverride = overrides.GetOverridesForPhone(phone);
				if (foverride == null)
				{
					foverride = new FeatureOverride { Phone = phone };
					overrides.Add(foverride);
				}

				foverride.BFeatureNames = fnames;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFileForProject(string projectPathPrefix)
		{
			return projectPathPrefix + kFileName;
		}

		/// ------------------------------------------------------------------------------------
		public FeatureOverride GetOverridesForPhone(string phone)
		{
			return this.FirstOrDefault(fo => fo.Phone == phone);
		}

		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable<PhoneInfo> phonesWithOverrides)
		{
			RebuildOverridesFromPhones(phonesWithOverrides);
			var root = BuildXmlForOverrides(this, _project.DistinctiveFeatureSet);
			MergeDistinctiveOverridesFromOtherSets(ref root);
			root.Save(GetFileForProject(_project.ProjectPathFilePrefix));
		}

		/// ------------------------------------------------------------------------------------
		private void RebuildOverridesFromPhones(IEnumerable<PhoneInfo> phonesWithOverrides)
		{
			Clear();

			foreach (var phoneInfo in phonesWithOverrides)
			{
				var foverride = new FeatureOverride { Phone = phoneInfo.Phone };
				
				if (phoneInfo.HasAFeatureOverrides)
					foverride.AFeatureNames = phoneInfo.AFeatureNames;

				if (phoneInfo.HasBFeatureOverrides)
					foverride.BFeatureNames = phoneInfo.BFeatureNames;

				Add(foverride);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static XElement BuildXmlForOverrides(IEnumerable<FeatureOverride> overrideList,
			string distinctiveFeatureSetName)
		{
			var root = new XElement("featureOverrides", new XAttribute("version", kCurrVersion));
			
			var descriptiveElement = new XElement("featureType",
				new XAttribute("class", "descriptive"));
			
			var distinctiveElement = new XElement("featureType",
				new XAttribute("class", "distinctive"),
				new XAttribute("set", distinctiveFeatureSetName));

			foreach (var foverride in overrideList)
			{
				AddFeaturesForSegmentElement(descriptiveElement, foverride.Phone, foverride.AFeatureNames);
				AddFeaturesForSegmentElement(distinctiveElement, foverride.Phone, foverride.BFeatureNames);
			}

			if (descriptiveElement.HasElements)
				root.Add(descriptiveElement);
			
			if (distinctiveElement.HasElements)
				root.Add(distinctiveElement);

			return root;
		}

		/// ------------------------------------------------------------------------------------
		private static void AddFeaturesForSegmentElement(XElement featureTypeElement,
			string phone, IEnumerable<string> featureNames)
		{
			var segmentElement = new XElement("featureOverride", new XAttribute("segment", phone));
			foreach (var fname in featureNames)
				segmentElement.Add(new XElement("feature", fname));

			if (segmentElement.HasElements)
				featureTypeElement.Add(segmentElement);
		}

		/// ------------------------------------------------------------------------------------
		private void MergeDistinctiveOverridesFromOtherSets(ref XElement newRoot)
		{
			var filename = GetFileForProject(_project.ProjectPathFilePrefix);
			if (!File.Exists(filename))
				return;

			var oldRoot = XElement.Load(filename);

			var distictiveOverrideElementsForOtherSets = oldRoot.Elements("featureType").Where(e =>
				(string)e.Attribute("class") == "distinctive" && (string)e.Attribute("set") != _project.DistinctiveFeatureSet);

			if (distictiveOverrideElementsForOtherSets.Count() > 0)
				newRoot.Add(distictiveOverrideElementsForOtherSets);
		}
	}

	#region FeatureOverride class
	/// ----------------------------------------------------------------------------------------
	public class FeatureOverride
	{
		public string Phone { get; set; }
		public IEnumerable<string> AFeatureNames { get; set; }
		public IEnumerable<string> BFeatureNames { get; set; }

		/// ------------------------------------------------------------------------------------
		public FeatureOverride()
		{
			AFeatureNames = new List<string>(0);
			BFeatureNames = new List<string>(0);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Phone;
		}
	}

	#endregion
}
