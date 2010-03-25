// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FilterHelper.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilUtils;

namespace SIL.Pa.Filters
{
	#region FilterHelper static class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	public static class FilterHelper
	{
		public const string kFiltersFilePrefix = "Filters.xml";

		public static Filter CurrentFilter { get; private set; }
		public static List<Filter> Filters { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static FilterHelper()
		{
			Filters = new List<Filter>();
		}

		#region Loading and saving filters
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of filters for the currently loaded project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadFilters()
		{
			LoadFilters(App.Project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of filters for the specified project. If the project is
		/// null, then an empty list is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadFilters(PaProject project)
		{
			string filename = (project != null ?
				project.ProjectPathFilePrefix + kFiltersFilePrefix : null);

			List<Filter> filtersList = null;

			if (filename != null && File.Exists(filename))
				filtersList = XmlSerializationHelper.DeserializeFromFile<List<Filter>>(filename, "filters");

			if (filtersList != null)
				filtersList.Sort((x, y) => x.Name.CompareTo(y.Name));

			Filters = (filtersList ?? new List<Filter>());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void SaveFilters()
		{
			SaveFilters(App.Project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void SaveFilters(PaProject project)
		{
			if (project != null)
			{
				string filename = project.ProjectPathFilePrefix + kFiltersFilePrefix;
				XmlSerializationHelper.SerializeToFile(filename, Filters, "filters");
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the current filter to the one having the specified name. If apply is true,
		/// then that filter is immediately applies. If setting the current filter succeeds,
		/// then true is returned. Otherwise false is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SetCurrentFilter(string filterName, bool apply)
		{
			var filter = GetFilter(filterName);
			if (filter == null)
				return false;

			CurrentFilter = filter;
			if (apply)
				ApplyFilter(filter);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Filter GetFilter(string filterName)
		{
			return Filters.FirstOrDefault(x => x.Name == filterName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void TurnOffCurrentFilter()
		{
			ApplyFilter(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool EntryMatchesCurrentFilter(WordCacheEntry entry)
		{
			return (CurrentFilter == null ? true : CurrentFilter.Matches(entry));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ApplyFilter(Filter filter)
		{
			ApplyFilter(filter, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ApplyFilter(Filter filter, bool forceReapplication)
		{
			if (App.RecordCache == null || (CurrentFilter == filter && !forceReapplication))
				return;

			CurrentFilter = filter;

			App.InitializeProgressBar(string.Empty, App.RecordCache.Count);
			App.RecordCache.BuildWordCache(App.ProgressBar);
			App.UninitializeProgressBar();

			App.MsgMediator.SendMessage("DataSourcesModified", App.Project.ProjectFileName);
			App.MsgMediator.SendMessage("FilterChanged", filter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchEngine CheckSearchQuery(SearchQuery query, bool showErrMsg)
		{
			query.ErrorMessages.Clear();
			SearchQuery modifiedQuery;
			if (!App.ConvertClassesToPatterns(query, out modifiedQuery, showErrMsg))
				return null;

			if (App.Project != null)
				SearchEngine.IgnoreUndefinedCharacters = App.Project.IgnoreUndefinedCharsInSearches;

			SearchEngine.ConvertPatternWithExperimentalTrans =
				App.SettingsHandler.GetBoolSettingsValue("searchengine",
				"convertpatternswithexperimentaltrans", false);

			SearchEngine engine = new SearchEngine(modifiedQuery, App.PhoneCache);

			string[] errors = modifiedQuery.ErrorMessages.ToArray();
			string msg = ReflectionHelper.GetStrResult(typeof(App),
				"CombineErrorMessages", errors);

			if (!string.IsNullOrEmpty(msg))
			{
				if (showErrMsg)
					Utils.MsgBox(msg);

				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				return null;
			}

			if (!ReflectionHelper.GetBoolResult(typeof(App),
				"VerifyMiscPatternConditions", new object[] { engine, showErrMsg }))
			{
				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				return null;
			}

			return engine;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PostCacheBuildingFinalize()
		{
			CleanUpExpressionSearchEngines(CurrentFilter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method is used after a filter is applied. It goes through the expressions
		/// and dereferences (so they'll get garbage collected) all the search engines
		/// created by expressions that are based on other filters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CleanUpExpressionSearchEngines(Filter filter)
		{
			if (filter == null)
				return;

			foreach (FilterExpression expression in filter.Expressions)
			{
				if (expression.ExpressionType == Filter.ExpressionType.PhoneticSrchPtrn)
				{
					CleanUpExpressionSearchEngines(GetFilter(expression.Pattern));
					expression.SearchEngine = null;
				}
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void HandleFilterStatusStripLabelPaint(object sender, PaintEventArgs e)
		{
			var lbl = sender as ToolStripStatusLabel;

			if (lbl == null || !lbl.Visible || CurrentFilter == null)
				return;

			Rectangle rc = lbl.ContentRectangle;

			// Fill in shaded background
			using (LinearGradientBrush br = new LinearGradientBrush(rc,
				Color.Gold, Color.Khaki, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw side borders
			using (Pen pen = new Pen(Color.Goldenrod))
			{
				e.Graphics.DrawLine(pen, 0, 0, 0, rc.Height);
				e.Graphics.DrawLine(pen, rc.Width - 1, 0, rc.Width - 1, rc.Height);
			}

			// Draw little filter image
			Image img = Properties.Resources.kimidFilterSmall;
			rc = lbl.ContentRectangle;
			Rectangle rcImage = new Rectangle(0, 0, img.Width, img.Height);
			rcImage.X = 3;
			rcImage.Y = (int)(Math.Ceiling(((decimal)rc.Height - rcImage.Height) / 2));
			e.Graphics.DrawImageUnscaledAndClipped(img, rcImage);

			// Draw text
			rc.X = rcImage.Width + 4;
			rc.Width -= rc.X;
			const TextFormatFlags flags = TextFormatFlags.EndEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;

			TextRenderer.DrawText(e.Graphics, CurrentFilter.Name, lbl.Font, rc, Color.Black, flags);
		}
	}

	#endregion
}
