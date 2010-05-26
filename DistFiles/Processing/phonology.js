// phonology.js 2010-05-26
// Interactive behavior for XHTML files exported from Phonology Assistant.
// http://www.sil.org/computing/pa/index.htm

// Requires jQuery 1.4 or later. http://jquery.com/

// JSLint defines a professional subset of JavaScript. http://jslint.com
/*jslint white: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global jQuery */

"use strict";


// Unicode values of characters mentioned in the JavaScript code:
// U+002D hyphen-minus
// U+00A0 no-break space
// U+2013 en dash
// U+2026 horizontal ellipsis
// U+25B2 black up-pointing triangle
// U+25B4 black up-pointing small triangle
// U+25B6 black right-pointing triangle
// U+25B8 black right-pointing small triangle
// U+25BC black down-pointing triangle
// U+25BE black down-pointing small triangle
// U+25C0 black left-pointing triangle
// U+25C2 black left-pointing small triangle


// Execute when the DOM is fully loaded.
// The following is related to $(document).ready(...) but even better because:
// * It avoids an explicit global reference to document.
// * It ensures that $(...) is a shortcut for jQuery(...) within the function.

jQuery(function ($) {
	
	// Initialization associates an object with some DOM elements to avoid:
	// * repeating data conversions in event processing functions
	//   (for example, features in CV charts and related feature tables)
	// * storing state information in class attributes,
	//   unless it directly affects the visual appearance (for example, the sort field)
	//
	// $(X).data('phonology', {Y: Z, ...}); // Store key Y and value Z with element X.
	// $(X).data('phonology').Y; // Return value Z for key Y from element X.
	
	
	// Phonology report ========================================================
	//	
	// Each division consists of a heading (which is the first child),
	// one or more tables, optional paragraphs, and optional child divisions.

	// To collapse/expand a division, click its heading.
	function clickHeadingReport() {
		$(this).parent().toggleClass('collapsed');
	}
		
	$('div.report').find(':first-child').filter(':header').each(function () {
		// Although .each() iteration is not (yet) necessary here, follow the pattern.
		$(this).addClass('interactive').click(clickHeadingReport);
	});


	// Data Corpus or Search view ==============================================
	//
	// Sort by columns.
	// Collapse/expand groups.

	// Return a character for the ins element in the heading of the sort field.
	function indicatorFromSortingData(data) {
		var sortOption = data.sortOption,
			descending = data.descending,
			indicator;
		
		if (sortOption) {
			// Phonetic or (someday) Phonemic field has regular-size triangle.
			if (sortOption === 'mannerOfArticulation') {
				// Down-pointing or up-pointing means manner of articulation.
				indicator = (descending ? '▼' : '▲');
			} else if (sortOption === 'placeOfArticulation') {
				// Right-pointing or left-pointing means place of articulation.
				indicator = (descending ? '▶' : '◀');
			}
		}
		else {
			// Any other field has black down-pointing or up-pointing small triangle.
			indicator = (descending ? '▾' : '▴');
		}
		return indicator;
	}
	
	// Initialize data for the column heading of the sort field
	// from a character in the ins element exported by Phonology Assistant.
	function initializeDataFromSortingIndicator(data, indicator) {
		// Down-pointing or right-pointing means descending.
		data.descending = (indicator === '▾' || indicator === '▼' || indicator === '▶');
		// Regular-size triangle distinguishes sort options from generic fields.
		if (indicator === '▼' || indicator === '▲') {
			// Down-pointing or up-pointing means manner of articulation.
			data.sortOption = 'mannerOfArticulation'; 
		} else if (indicator === '▶' || indicator === '◀') {
			// Right-pointing or left-pointing means place of articulation.
			data.sortOption = 'placeOfArticulation';
		}
	}
	
	// To compare field values, change some occurrences of no-break space.
	function sortableValue(sortKey) {
		var length = sortKey.length;
		if (length) {
			if (sortKey.charCodeAt(0) === 160) { // xA0 = 160
				if (length === 1) {
					// A single no-break space replaced an empty field.
					sortKey = '';
				} else {
					// A no-break space at the beginning replaced a space.
					sortKey = ' ' + sortKey.slice(1); // length is unchanged
				}
			}
			if (length > 1 && sortKey.charCodeAt(length - 1) === 160) {
				// A no-break space at end replaced a space.
				sortKey = sortKey.slice(0, -1) + ' ';
			}
		}
		return sortKey;
	}

	// Within one or more parent elements, sort children according to arguments.
	function sortChildren($parents, childrenSelector,
			sortFieldSelector, sortOrderSelector, descending) {
		$parents.each(function () {
			var $parent = $(this),
				children = $parent.find(childrenSelector).get(); // Array of DOM objects.
				
			children.sort(function (childA, childB) {
				var $cellA = $(childA).find(sortFieldSelector),
					$cellB = $(childB).find(sortFieldSelector),
					sortKeyA,
					sortKeyB;
				
				if (sortOrderSelector) {
					sortKeyA = $cellA.find(sortOrderSelector).text();
					sortKeyB = $cellB.find(sortOrderSelector).text();
				} else {
					sortKeyA = sortableValue($cellA.text());
					sortKeyB = sortableValue($cellB.text());
				}

				return descending ?
					sortKeyB.localeCompare(sortKeyA) :
					sortKeyA.localeCompare(sortKeyB);
			});
			$parent.append(children); // Replace DOM objects in sorted order.
		});
	}

	// Sort according to data from a column heading.
	// Differences between views in the Phonology Assistant program
	// and interactive tables in XHTML files exported from Phonology Assistant:
	// * If the list is grouped, a view regroups according to the column,
	//   but a table resorts within the same groups.
	// * If the list has minimal groups, both resort within the same groups,
	//   but if one minimal pair per group, a table has no sortable columns.
	function sortList($table, data) {
		var descending = data.descending,
			sortFieldSelector = '.' + data.classOfField,
			sortOrderSelector;

		if (data.searchField) {
			sortFieldSelector += '.item';
		}
		if (data.sortOption) {
			// Phonetic or (someday) Phonemic field.
			sortOrderSelector = 'ul.sortOrder li.' + data.sortOption;
		}
		if (data.groupField) {
			// The field in the heading row of groups.
			sortChildren($table, 'tbody',
				'tr.heading ' + sortFieldSelector, sortOrderSelector, descending);
		}
		if (data.searchField || !data.groupField) {
			// For Phonetic or (someday) Phonemic in Search view, sort rows
			// even if grouped, because they might not contain identical values.
			sortChildren($table.find('tbody'), 'tr:not(.heading)',
				sortFieldSelector, sortOrderSelector, descending);
		}
	}

	// To sort by a column, click its heading cell or a sort option.
	function clickColumnHeadingList($target) {
		var $th = $target.closest('th'),
			data = $th.data('phonology'),
			$table = $th.closest('table'),
			reverse = $th.hasClass('sortField'), // Click the active column heading.
			sortOptionSelected,
			$details,
			details;

		// Phonetic sort option.
		if ($target.is('li')) {
			sortOptionSelected = $target.data('phonology').sortOption;
			if (reverse) {
				// Only if this is the active sort field:
				// * Reverse the direction if the sort option remains the same.
				// * Keep the direction the same if the sort option changes.
				reverse = (sortOptionSelected === data.sortOption);
			}
			data.sortOption = sortOptionSelected;
		}
		// The following condition is independent of the previous condition,
		// because you might click the heading instead of the list.
		if ($th.hasClass('sortOptions')) {
			$th.removeClass('active').find('li.initial').removeClass('initial');
		}

		// Important: Must resort in the opposite direction;
		// cannot just call the reverse() member function,
		// because it does not preserve stable order of records
		// with identical values of the sort field.
		if (reverse) {
			data.descending = !data.descending;
		}
		
		sortList($table, data);

		if (!($th.hasClass('sortField'))) {
			$table.find('thead th.sortField').removeClass('sortField');
			$th.addClass('sortField');
		}
		$th.find('ins').text(indicatorFromSortingData(data));

		// Update the table of details.		
		$details = $table.parent().find('table.details tr.sortField td');
		if ($details.length) {
			details = $th.find('span').text(); // Name of sort field.
			if (data.sortOption === 'placeOfArticulation') {
				details += ', place of articulation';
			} else if (data.sortOption === 'mannerOfArticulation') {
				details += ', manner of articulation';
			}
			if (data.descending) {
				details += ', descending';
			}
			$details.text(details);
		}
	}
	
	// Click a list.
	function clickList(event) {
		var $target = $(event.target),
			$th = $target.closest('th');

		if ($th.length) {
			if ($th.hasClass('group')) {
				// To collapse/expand all groups, click at the upper left.
				$(this).toggleClass('collapsed'); // this refers to the table.
			} else if ($th.hasClass('count') || $th.hasClass('pair')) {
				// To collapse/expand a group, click at the left of its heading row.
				$th.closest('tbody').toggleClass('collapsed');
			} else if ($th.hasClass('sortable')) {
				// To sort by a column, click its heading cell or a phonetic option.
				clickColumnHeadingList($target);
			}
		}
	}

	// When the mouse pointer moves over a Phonetic or (someday) Phonemic heading cell,
	// it displays a drop-down menu of sort options.
	function mouseoverSortOptionList() {
		var $th = $(this),
			sortOption = $th.data('phonology').sortOption;

		$th.addClass('active').find('li').each(function () {
			var $li = $(this);
			if ($li.data('phonology').sortOption === sortOption) {
				$li.addClass('initial');
			}
		});
	}

	// When the mouse pointer moves out of the heading cell, sort options disappear.
	function mouseoutSortOptionList() {
		$(this).removeClass('active').find('li.initial').removeClass('initial');
	}

	// Initialization ----------------------------------------------------------

	// Return an item for a drop-down menu of sort options.
	function sortOptionItem(sortOptionText, sortOption) {
		return $('<li>' + sortOptionText + '</li>').data('phonology', {sortOption: sortOption});
	}

	// Initialize sorting data for column headings of lists.
	function initializeDataForHeadingList() {
		var $th = $(this),
			$span = $th.find('span'),
			nameOfField = $span.length ? $span.text() : $th.text(),
			$ins = $th.find('ins'),
			hasSortOptions = $th.hasClass('sortOptions'),
			data = {};

		// The following key is not class, because that is a JavaScript reserved word.
		// The HTML/CSS class for a field consists of alphanumeric characters only.
		// \W is the opposite of \w, which is equivalent to [0-9A-Z_a-z].
		// Therefore, [\W_] is equivalent to [^0-9A-Za-z],
		// but JSLint does not consider it an insecure use of [^...].
		data.classOfField = nameOfField.replace(/[\W_]/g, '');
		
		if ($ins.length) {
			initializeDataFromSortingIndicator(data, $ins.text());
		} else {
			// The first time you sort by any column other than the primary sort field,
			// assume that the direction is ascending and insert the visual indicator.
			data.descending = false;
			if (hasSortOptions) {
				data.sortOption = 'placeOfArticulation'; // Assume as the default.
			}
			if (!($span.length)) {
				$th.wrapInner('<span></span>');
			}
			$th.append(' <ins></ins>').find('ins').text(indicatorFromSortingData(data));
		}
		if (hasSortOptions) {
			// To make Internet Explorer 7 display the drop-down menu on the next line,
			// wrap the contents in an explicit block element.
			// Must have inserted <span>...</span><ins>...</ins> before this!
			$th.wrapInner('<div></div>');
			$('<ul></ul>')
				.append(sortOptionItem('place of articulation', 'placeOfArticulation'))
				.append(sortOptionItem('manner of articulation', 'mannerOfArticulation'))
				.appendTo($th);
		}
		if ($th.hasClass('sortField') && $th.parent().has('th.group').length) {
			// The field by which the list is grouped.
			data.groupField = true;
		}
		if ($th.attr('colspan') === 3) {
			// Phonetic or (someday) Phonemic in Search view.
			data.searchField = true;
		}
		$th.data('phonology', data);
	}

	$('table.list').each(function () {
		var $table = $(this).addClass('interactive').click(clickList),
			$ulTranscriptions = $table.find('td > ul.transcription'),
			$ins = $ulTranscriptions.find('li.change del + ins'),
			browser;
		
		$table.find('thead th.sortOptions')
			.hover(mouseoverSortOptionList, mouseoutSortOptionList);
		
		if (!($table.has('tbody.group th.pair').length)) {
			// You can sort any column which contains data,
			// except when there is one minimal pair per group
			// because it might contain both subheading and data rows.
			$table.find('thead th:not(.group)').addClass('sortable')
				.each(initializeDataForHeadingList);
		}
		
		// The phonology.css file assumes all siblings are in a div.
		// because the data cell itself cannot have relative position.
		$ulTranscriptions.parent().wrapInner('<div class="transcription"></div>');
		if ($ins.length) {
			// For Internet Explorer 7, which does not support the :before pseudo-element,
			// insert the content to separate del and ins elements.
			// For lack of a "proper feature detection" method for this problem,
			// here is the deprecated "browser sniffing" method.
			browser = $.browser;
			if (browser && browser.msie && browser.version === '7.0') {
				$ins.before(' ▸ '); // right-pointing triangle
			}
		}
	});


	// Consonant Chart or Vowel Chart view =====================================
	//
	// Display units which have sets of features.
	// Display features for units or pairs of units.

	// Return the text of feature or environment in a list item (DOM).
	function textFromListItem(li) {
		// Internet Explorer 7 returns a trailing space in text of li elements.
		return $.trim($(li).text());
	}

	// Processing functions ----------------------------------------------------
	
	// Return whether a particular unit matches a set of features.
	function matchUnitCV(allFeaturesOfUnit, allFeaturesToMatch) {
		var classOfFeatures,
			classFeaturesToMatch,
			classFeaturesOfUnit,
			i;

		// For information about enumeration, see Reflection and Enumeration
		// in chapter 3 of JavaScript: The Good Parts.
		for (classOfFeatures in allFeaturesToMatch) {
			if (allFeaturesToMatch.hasOwnProperty(classOfFeatures)) {
				classFeaturesToMatch = allFeaturesToMatch[classOfFeatures];
				if (typeof classFeaturesToMatch !== 'function') {
					classFeaturesOfUnit = allFeaturesOfUnit[classOfFeatures];
					if (!classFeaturesOfUnit) {
						return false;
					}

					for (i = 0; i < classFeaturesToMatch.length; i += 1) {
						if ($.inArray(classFeaturesToMatch[i], classFeaturesOfUnit) < 0) {
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Match all units in one or more CV charts according to selected features
	// and the optional active feature in the related feature tables.
	function matchUnitsForFeaturesCV($container, $tdActive) {
		var $tableCV = $container.find('table.CV'),
			$tdFeaturesToMatch = $container.find('table.features td.selected'),
			allFeaturesToMatch = {};

		if ($tdActive) {
			$tdFeaturesToMatch = $tdFeaturesToMatch.add($tdActive);
		}
		if ($tdFeaturesToMatch.length) {
			$tdFeaturesToMatch.each(function () {
				var $td = $(this),
					classOfFeatures = $td.closest('table.features')
						.data('phonology').classOfFeatures,
					feature = $td.data('phonology').feature;

				// See hasOwnProperty and Object
				// in Appendix A of JavaScript: The Good Parts.
				if (allFeaturesToMatch.hasOwnProperty(classOfFeatures)) {
					allFeaturesToMatch[classOfFeatures].push(feature);
				} else {
					allFeaturesToMatch[classOfFeatures] = [feature];
				}
			});
			$tableCV.find('td.Phonetic').each(function () {
				var $tdPhonetic = $(this);
				if (matchUnitCV($tdPhonetic.data('phonology').features,
						allFeaturesToMatch)) {
					$tdPhonetic.addClass('matched');
				} else {
					$tdPhonetic.removeClass('matched');
				}
			});
		} else {
			// If the mouse moves out of a feature and no other features selected.
			$tableCV.find('.matched').removeClass('matched');
		}
	}

	function matchUnitsForSelectedFeaturesCV($td) {
		matchUnitsForFeaturesCV($td.closest('table.features').parent());
	}

	function matchUnitsForSelectedAndActiveFeaturesCV($td) {
		matchUnitsForFeaturesCV($td.closest('table.features').parent(), $td);
	}

	// Match a particular feature according to the active unit
	// and the optional selected unit in a consonant or vowel chart.
	function matchFeatureCV($td, featuresActive, featuresSelected) {
		var feature = $td.data('phonology').feature,
			indexActive = $.inArray(feature, featuresActive),
			indexSelected;

		if (featuresSelected) {
			indexSelected = $.inArray(feature, featuresSelected);
			if (indexActive >= 0 && indexSelected >= 0) {
				$td.addClass('matched');
			}
			else if (indexActive >= 0) {
				$td.addClass('active');
			}
			else if (indexSelected >= 0) {
				$td.addClass('selected');
			}
		} else if (indexActive >= 0) {
			$td.addClass('matched');
		}
	}

	// Match the related feature tables according to the active unit
	// and the optional selected unit in a consonant or vowel chart.
	function matchFeaturesCV($tdActive, $tdSelected) {
		var allFeaturesActive = $tdActive.data('phonology').features,
			allFeaturesSelected;
		
		if ($tdSelected && $tdSelected.length && $tdSelected.get(0) !== $tdActive.get(0)) {
			allFeaturesSelected = $tdSelected.data('phonology').features;
		}
		
		$tdActive.closest('table').parent().find('table.features').each(function () {
			var $table = $(this),
				classOfFeatures = $table.data('phonology').classOfFeatures,
				classFeaturesActive = allFeaturesActive[classOfFeatures],
				classFeaturesSelected;
			
			if (allFeaturesSelected) {
				classFeaturesSelected = allFeaturesSelected[classOfFeatures];
			}

			$table.find('tbody tr').each(function () {
				var $tr = $(this),
					$td;
				
				if ($tr.hasClass('bivalent')) {
					$td = $tr.find('td.plus');
					if ($td.length) {
						matchFeatureCV($td, classFeaturesActive, classFeaturesSelected);
					}
					$td = $tr.find('td.minus');
					if ($td.length) {
						matchFeatureCV($td, classFeaturesActive, classFeaturesSelected);
					}
				} else {
					matchFeatureCV($tr.find('td.name'), classFeaturesActive, classFeaturesSelected);
				}
			});
		});
	}

	// Clear the classes related to matching in all feature tables.
	function unmatchFeaturesCV($td) {
		$td.closest('table').parent().find('table.features')
			.find('.matched').removeClass('matched')
			.end().find('.active').removeClass('active')
			.end().find('.selected').removeClass('selected');
	}

	// Event functions for table cells -----------------------------------------

	// Move the mouse pointer into a non-empty data cell.
	function mouseoverUnitCV() {
		var $tdActive = $(this),
			$tdSelected = $tdActive.hasClass('selected') ?
				$tdActive :
				$tdActive.closest('table').find('td.selected');
		
		matchFeaturesCV($tdActive, $tdSelected);
	}

	// Move the mouse pointer out of a non-empty data cell.
	function mouseoutUnitCV() {
		unmatchFeaturesCV($(this));
	}

	// Move the mouse pointer into an interactive data cell.
	function mouseoverFeatureCV() {
		var $td = $(this);
		if (!($td.hasClass('selected')) && 
				!($td.hasClass('inactive'))) { // binary features only
			matchUnitsForSelectedAndActiveFeaturesCV($td);
		}
	}

	// Move the mouse pointer out of an interactive data cell.
	function mouseoutFeatureCV() {
		var $td = $(this);
		$td.removeClass('inactive'); // any features
		if (!($td.hasClass('selected'))) {
			matchUnitsForSelectedFeaturesCV($td);
		}
	}

	// Click an interactive data cell.
	function clickFeatureCV($td) {
		// If you click to clear a selected feature, make it inactive
		// until you move the mouse out or click to select it again.
		if ($td.hasClass('selected')) {
			$td.addClass('inactive'); // any features
		}
		else {
			$td.removeClass('inactive'); // any features
			// If opposite binary value is selected, clear it.
			// Does not apply to bivalent hierarchical values.
			if ($td.closest('table').hasClass('binary')) {
				$td.parent().find('td.' + ($td.hasClass('minus') ? 'plus' : 'minus'))
					.removeClass('selected');
			}
		}
			
		$td.toggleClass('selected');
		matchUnitsForSelectedFeaturesCV($td);
	}

	// Event functions for tables ----------------------------------------------

	// Move the mouse pointer into a feature table.
	function mouseoverFeaturesCV() {
		// Related CV charts become (visually) inactive.
		$(this).parent().find('table.CV').removeClass('interactive')
			// The selected unit (if any) is cleared.
			.find('td.selected').removeClass('selected');
	}

	// Move the mouse pointer out of a feature table.
	function mouseoutFeaturesCV() {
		var $container = $(this).parent();
		// If no features in any related tables are selected,
		if (!($container.find('table.features td.selected').length)) {
			// related CV charts become (visually) active.
			$container.find('table.CV').addClass('interactive');
		}
	}

	// Click a feature table.
	function clickFeaturesCV(event) {
		var $target = $(event.target);

		if ($target.is('tr.univalent td.name div div')) {
			// Toggle text up-pointing ? down-pointing : up-pointing.
			$target.text($target.text() === '▴' ? '▾' : '▴')
				// Collapse/expand redundant bivalent rows
				// between this row and the next univalent row.
				.closest('tr.univalent').nextUntil('tr.univalent')
					.filter('tr.redundant').toggleClass('collapsed');
		} else if ($target.is('tr.univalent td.name > div')) {
			// Click an inserted div that wraps the contents of td.
			clickFeatureCV($target.closest('td'));
		} else if ($target.is('td')) {
			if ($target.hasClass('plus') || $target.hasClass('minus') ||
					($target.hasClass('name') && 
						!($target.parent().hasClass('bivalent')))) {
				clickFeatureCV($target);
			}
		} else if ($target.closest('th').length) {
			$(this).toggleClass('collapsed');
		}
	}

	// Move the mouse pointer into a consonant or vowel chart.
	function mouseoverChartCV() {
		var $table = $(this),
			$container = $table.parent();

		// Chart becomes active and units become unmatched.
		$table.addClass('interactive')
			.find('td.matched').removeClass('matched');

		// Feature tables become inactive and selected features become cleared.
		$container.find('table.features').removeClass('interactive')
			.find('td.selected').removeClass('selected');
		$container.find('table.binary')
			.find('td.inactive').removeClass('inactive');
	}

	// Move the mouse pointer out of a consonant or vowel chart.
	function mouseoutChartCV() {
		$(this).parent().find('table.features').addClass('interactive');
	}

	// Click a consonant or vowel chart.
	function clickChartCV(event) {
		var $tdActive = $(event.target).closest('td'),
			tdPhonetic,
			$tdSelected;

		// If you click a data cell.
		if ($tdActive.length) {
			if ($tdActive.hasClass('selected')) {
				$tdActive.removeClass('selected');
			}
			else {
				tdPhonetic = $tdActive.hasClass('Phonetic');
				$tdSelected = $(this).find('td.selected');
				if ($tdSelected.length) {
					$tdSelected.removeClass('selected');
					unmatchFeaturesCV($tdActive);
					if (tdPhonetic) {
						matchFeaturesCV($tdActive);
					}
				}
				if (tdPhonetic) {
					$tdActive.addClass('selected');
				}
			}
		}
	}

	// Environment tabs ----------------------------------------------------------

	function matchUnitsForSelectedEnvironmentCV($li) {
		var $ulEnvironments = $li.parent(),
			$tableCV = $ulEnvironments.parent().find('table.CV'),
			environment = $li.data('phonology').environment;

		$ulEnvironments.find('li').removeClass('selected');
		$li.addClass('selected');
		$tableCV.find('td.zero').removeClass('zero');
		$tableCV.find('td.Phonetic').each(function () {
			var $tdPhonetic = $(this),
				environmentsOfUnit = $tdPhonetic.data('phonology').environments;

			if ($.inArray(environment, environmentsOfUnit) < 0) {
				$tdPhonetic.addClass('zero');
			}
		});
	}

	function clickEnvironments(event) {
		var $li = $(event.target).closest('li');
		if (!($li.hasClass('selected'))) {
			matchUnitsForSelectedEnvironmentCV($li);
		}
	}

	// Feature charts ----------------------------------------------------------

	// Move the mouse pointer into a Phonetic heading cell in a feature chart.
	function mouseoverFeatureChartCV() {
		var $th = $(this),
			// Zero-based index, not including heading column at the left.
			index = $th.parent().children('th.Phonetic').index(this);
		
		$th.closest('table').find('tbody tr').each(function () {
			var $tr = $(this),
				$tds = $tr.find('td'),
				text;
			
			// If the row contains feature values.
			// That is, not the repeated heading row in the body
			// which separates binary from hierarchical features.
			if ($tds.length) {
				text = $tds.eq(index).text(); // Value of active unit.
				// Match the units whose values are different.
				// Must compare the entire value: +, -, +-, or -+.
				$tds.filter(function (i) {
					return $(this).text() !== text;
				}).addClass('matched');
			}
		});
	}

	// Move the mouse pointer out of a Phonetic heading cell in a feature chart.
	function mouseoutFeatureChartCV() {
		$(this).closest('table').find('.matched').removeClass('matched');
	}

	// Initialization ----------------------------------------------------------

	// Initialize the features as data of each non-empty data cell in CV charts.
	function initializeDataForUnitCV() {
		var $tdPhonetic = $(this),
			features = {},
			$environments = $tdPhonetic.find('ul.environments'),
			data = {};

		$tdPhonetic.find('ul.features').each(function () {
			var $ul = $(this),
				classOfFeatures = $ul.attr('class').split(' ')[0];

			features[classOfFeatures] = $.map($ul.find('li'), textFromListItem);
		});
		data.features = features;

		if ($environments.length) {
			data.environments = $.map($environments.find('li'), textFromListItem);
		}
		
		$tdPhonetic.data('phonology', data);
	}
	
	// Initialize the environment as data in tabs preceding CV charts.
	function initializeDataForEnvironmentCV() {
		$(this).data('phonology', {environment: textFromListItem(this)});
	}
	
	// Initialize consonant or vowel charts.
	$('table.CV').each(function () {
		var $table = $(this),
			$tdPhonetics = $table.find('td.Phonetic'), // non-empty cells
			$ulEnvironments = $table.prev('ul.environments');

		if ($table.parent().find('table.features').length) {
			$table.addClass('interactive')
				.hover(mouseoverChartCV, mouseoutChartCV)
				.click(clickChartCV);
			$tdPhonetics.hover(mouseoverUnitCV, mouseoutUnitCV);
		}
		
		$tdPhonetics.each(initializeDataForUnitCV);
		
		if ($ulEnvironments.length) {
			$ulEnvironments.addClass('interactive').click(clickEnvironments)
				.find('li').each(initializeDataForEnvironmentCV)
				.end().find('li:first-child')
					.each(function () {
						matchUnitsForSelectedEnvironmentCV($(this));
					});
		}
	});

	// Initialize heading abbreviation in feature tables.
	function initializeHeadingForFeaturesCV() {
		var $th = $(this),
			$span = $th.find('span'), // The text is already wrapped in a span.
			text;

		// Title attribute consists of the original text.
		// Abbreviation consists of the first character,
		// followed by down-pointing triangle.
		if ($span.length) {
			text = $span.text();
			if (text) {
				$th.append('<abbr title="' + text + '">' + text.charAt(0) + '▾</abbr>');
			}
		}
	}

	// Initialize the feature as data of each interactive data cell in feature tables.
	function initializeDataForFeatureCV() {
		var $td = $(this),
			value,
			feature;

		// Do not assume that value is the text in the cell.
		// Instead of hyphen-minus, the text might be en dash.
		if ($td.hasClass('plus')) {
			value = '+';
		} else if ($td.hasClass('minus')) {
			value = '-';
		}
		if (value) {
			feature = value + $td.parent().find('td.name').text();
		} else {
			feature = $td.text();
		}
		$td.data('phonology', {feature: feature});
	}
	
	// Initialize feature tables separately from CV charts to prevent repetition
	// in case a feature table relates to a pair of consonant and vowel charts.
	$('table.features').each(function () {
		var $table = $(this);
		$table.addClass('interactive')
				.click(clickFeaturesCV)
				.hover(mouseoverFeaturesCV, mouseoutFeaturesCV)
				// Initialize the distinctive class as data of each feature table.
				.data('phonology', {classOfFeatures: $table.attr('class').split(' ')[0]})
			.find('th').each(initializeHeadingForFeaturesCV)
			.end().find('td.plus, td.minus, td.name').filter(':not(tr.bivalent td.name)')
				.hover(mouseoverFeatureCV, mouseoutFeatureCV)
				.each(initializeDataForFeatureCV);
		
		if ($table.hasClass('hierarchical')) {
			$table.find('tr.univalent').each(function () {
				var $trUnivalent = $(this);
				if ($trUnivalent.nextUntil('tr.univalent').filter('tr.redundant').length) {
					// Insert a div to be able to collapse/expand redundant bivalent rows
					// between this row and the next univalent row.
					$trUnivalent.find('td.name').append('<div>▴</div>').wrapInner('<div></div>');
				}
			});
		}
	});

	// Initialize feature charts following a CV chart and its feature tables.
	$('table.feature').each(function () {
		// Although .each() iteration is not (yet) necessary here, follow the pattern.
		$(this).addClass('interactive')
			.find('th.Phonetic').hover(mouseoverFeatureChartCV, mouseoutFeatureChartCV);
	});

	// Distribution Chart view =================================================
	//
	// In a generalized chart, collapse/expand individual rows or columns.
	// * To collapse a column group, click left-pointing triangle.
	// * To expand a column group, click right-pointing triangle.
	// * To collapse a row group, click up-pointing triangle.
	// * To expand a row group, click down-pointing triangle.
	// In any distribution chart, reduce/restore phonetic heading cells.
	// * To reduce a search element, click it. An ellipsis appears.
	// * To restore a search element, click it. The text appears again.

	// Temporarily collapse/expand all individual rows and columns in generalized charts;
	// otherwise reduce/restore all phonetic headings.
	function clickUpperLeftDistribution($thUpperLeft) {
		var $table = $thUpperLeft.closest('table'),
			collapsed,
			data,
			colspan,
			rowspan,
			$colgroups = $table.find('colgroup');

		if ($table.hasClass('generalized')) {
			// 1. Toggle collapsed class of table.
			$table.toggleClass('collapsed');
			collapsed = $table.hasClass('collapsed');

			// 2a. Adjust colspan and rowspan of upper-left cell.
			if (collapsed) {
				colspan = 1;
				rowspan = 1;
			} else {
				data = $thUpperLeft.data('phonology');
				colspan = data.colspan;
				rowspan = data.rowspan;
			}
			$thUpperLeft.attr('colspan', colspan);
			$thUpperLeft.attr('rowspan', rowspan);
			
			// 2b. Adjust colspan of general column headings, if any.
			$table.find('thead tr.general th.Phonetic').each(function () {
				var $thPhonetic = $(this),
					data;

				if (collapsed) {
					colspan = 1;
				} else {
					data = $thPhonetic.data('phonology');
					if ($colgroups.eq(data.colgroup).hasClass('collapsed')) {
						colspan = 1;
					} else {
						colspan = data.colspan;
					}
				}
				$thPhonetic.attr('colspan', colspan);
			});
			
			// 2c. Adjust rowspan of general row headings, if any.
			$table.find('tbody tr.general th.Phonetic').each(function () {
				var $thPhonetic = $(this);

				if (collapsed) {
					rowspan = 1;
				} else if ($thPhonetic.closest('tbody').hasClass('collapsed')) {
					rowspan = 1;
				} else {
					rowspan = $thPhonetic.data('phonology').rowspan;
				}
				$thPhonetic.attr('rowspan', rowspan);
			});
		}
		else {
			// Temporarily reduce all headings.
			$table.toggleClass('reduced');
		}
	}

	// Collapse/expand all the individual columns in a general column group.
	function clickCollapsibleColumnHeadingDistribution($thCollapsible) {
		var $table,
			data,
			colgroup,
			$colgroup,
			$thColgroup,
			colspan,
			heading,
			start,
			end;

		if ($thCollapsible.hasClass('interactive')) {
			$table = $thCollapsible.closest('table');
			data = $thCollapsible.data('phonology');

			// 1. Toggle collapsed class of colgroup.
			colgroup = data.colgroup;
			$colgroup = $table.find('colgroup').eq(colgroup);
			$colgroup.toggleClass('collapsed');

			// 2. Adjust colspan and heading.
			$thColgroup = $table.find('thead tr.general th').eq(colgroup);
			if ($colgroup.hasClass('collapsed')) {
				colspan = 1;
				heading = '▸'; // right-pointing
			} else {
				colspan = $thColgroup.data('phonology').colspan;
				heading = '◂'; // left-pointing
			}
			$thColgroup.attr('colspan', colspan);
			$thCollapsible.text(heading);

			// 3. Toggle collapsed class for cells in individual columns.
			start = data.start + 1; // The first column is general.
			end = data.end; // Up to, but not including, end.
			$table.find('thead tr.individual')
				.children('th').slice(start, end).toggleClass('collapsed');
			$table.find('tbody tr').each(function () {
				$(this).children('td').slice(start, end).toggleClass('collapsed');
			});
		}
	}

	// Collapse/expand all the individual rows in a general row group.
	function clickCollapsibleRowHeadingDistribution($thCollapsible) {
		var $tr,
			$tbody,
			$thPhonetic,
			rowspan,
			heading;
	
		if ($thCollapsible.hasClass('interactive')) {
			$tr = $thCollapsible.parent();

			// 1. Toggle collapsed class of tbody.
			$tbody = $tr.parent().toggleClass('collapsed');

			// 2. Adjust rowspan and heading.
			$thPhonetic = $tr.find('th.Phonetic');
			if ($tbody.hasClass('collapsed')) {
				rowspan = 1;
				heading = '▾'; // down-pointing
			} else {
				rowspan = $thPhonetic.data('phonology').rowspan;
				heading = '▴'; // up-pointing
			}
			$thPhonetic.attr('rowspan', rowspan);
			$thCollapsible.text(heading);
		}
	}

	function clickChartDistribution(event) {
		var $target = $(event.target),
			$th;

		if ($target.is('div.zero')) {
			// To reverse the background color of zero/non-zero cells,
			// click at the lower-right of the upper-left cell.
			$(this).toggleClass('nonZero')
				.parent().find('ul.environments + table.CV').toggleClass('nonZero');
		}
		else {	
			$th = $target.closest('th');
			if ($th.length) {
				if ($th.hasClass('Phonetic')) {
					$th.toggleClass('reduced');
				}
				else if ($th.parent().parent().is('tbody')) {
					clickCollapsibleRowHeadingDistribution($th);
				}
				else if ($th.is('thead tr:first-child th:first-child')) {
					clickUpperLeftDistribution($th);
				}
				else {
					clickCollapsibleColumnHeadingDistribution($th);
				}
			}
		}
	}

	// To reverse the background color of zero/non-zero cells,
	// click at the lower-right of the upper-left cell.
	// Because the related consonant or vowel chart with tabbed environments
	// is not interactive, this is separate from the clickChartCV event function.
	function clickZeroCV()
	{
		$(this).closest('table').toggleClass('nonZero')
			.parent().find('table.distribution').toggleClass('nonZero');
	}

	// Initialization ----------------------------------------------------------

	// Add data, class attributes, text to elements.
	function initializeGeneralizedDistribution($table) {
		var $colgroups = $table.find('colgroup'),
			chartHasGeneralColumns = ($table.has('thead tr.general').length !== 0),
			$thCols = $table.find('thead tr.individual th'),
			$thCollapsibles,
			start = 0;

		$table.addClass('generalized');

		// Add data to colgroup elements and class attributes to col elements.
		$colgroups.each(function (colgroup) {
			var $colgroup = $(this),
				$cols = $colgroup.find('col'),
				colspan = $cols.length;

			$colgroup.data('phonology', {colspan: colspan});
			if (colgroup === 0) { // Row headings at the left of the data.
				if (colspan === 2) { // If there are general rows:
					$cols.eq(0).addClass('general');
					$cols.eq(1).addClass('individual');
				}
			} else if (chartHasGeneralColumns) {
				$cols.eq(0).addClass('general'); // First column.
				$cols.slice(1).addClass('individual'); // Any other columns.
			}
		});
		
		// Add class attribute and data to upper-left cell.
		$table.find('thead tr:first-child th:first-child')
			.data('phonology', {
				colspan: $table.find('tbody:first tr:first-child th').length,
				rowspan: $table.find('thead tr').length
			});

		// Add class attribute and data to general column group headings, if any.
		$table.find('thead tr.general th.Phonetic').each(function (i) {
			var $thColgroup = $(this),
				colgroup = i + 1, // Adjust index to account for the upper-left cell.
				colspan = $colgroups.eq(colgroup).find('col').length,
				end = start + colspan;

			$thColgroup.data('phonology', {colgroup: colgroup, colspan: colspan});

			// Column headings for the group in the individual heading row.
			// Empty heading cell corresponding to the column group.
			if (colspan > 1) { // If there are any individual columns:
				$thCols.eq(start)
					.addClass('interactive')  // Used by CSS, not JavaScript.
					.data('phonology', {colgroup: colgroup, start: start, end: end})
					.text('◂'); // left-pointing
			}

			// Add class to individual heading cells.
			$thCols.slice(start + 1, end).addClass('individual');

			start += colspan;
		});

		if (chartHasGeneralColumns) {
			// Add class to individual data cells.		
			$thCollapsibles = $table.find('thead tr.individual th.interactive');
			$table.find('tbody tr').each(function () {
				var $tds = $(this).find('td');
				$thCollapsibles.each(function () {
					var data = $(this).data('phonology');
					$tds.slice(data.start + 1, data.end).addClass('individual');
				});
			});
		}

		// General row groups, if any.
		// Empty heading cells corresponding to row groups.
		$table.find('tbody tr.general').each(function () {
			var $tr = $(this),
				rowspan = $tr.parent().find('tr').length,
				$th = $tr.find('th:not(.Phonetic)');

			$th.addClass('individual');
			if (rowspan !== 1) {
				$th.addClass('interactive') // Used by CSS, not JavaScript.
					.text('▴'); // up-pointing
			}
			$tr.find('th.Phonetic').data('phonology', {rowspan: rowspan});
		});
	}
	
	$('table.distribution').each(function () {
		var $table = $(this).addClass('interactive').click(clickChartDistribution),
			$thUpperLeft = $table.find('thead tr:first-child th:first-child'),
			browser;

		// Click at the upper-left to collapse/expand, otherwise to reduce/restore.
		$thUpperLeft.addClass('interactive'); // Used by CSS, not JavaScript.

		// To reverse the background color of zero/non-zero cells,
		// click at the lower-right of the upper-left cell.
		if ($table.has('td.zero').length) {
			// In Internet Explorer 7, if the outer div lacks static content,
			// it has relative position for the absolute position of the inner div.
			// Therefore, include a no-break space, which the inner div covers
			// because the cell is aligned right.
			$thUpperLeft.append('<div>&#xA0;<div class="zero">0</div></div>');
			$table.parent().find("ul.environments + table.CV thead tr:first-child th:first-child")
				.append('<div>&#xA0;<div class="zero">0</div></div>')
				.find('.zero').click(clickZeroCV);
		}

		// To reduce or restore a heading, click the cell.
		$table.find('th.Phonetic')
			.wrapInner('<span></span>') // Wrap heading text in span,
			.append('<abbr>…</abbr>'); // Insert U+2026 horizontal ellipsis in abbr.

		// To collapse or expand a general row or column,
		// click the individual cell corresponding to the row or column group heading.
		if ($table.has('tr.general').length) {
			// Display of interactive colgroups is unreliable in Internet Explorer 7.
			// For lack of a "proper feature detection" method for this problem,
			// here is the deprecated "browser sniffing" method.
			browser = $.browser;
			if (!browser || !browser.msie || browser.version !== '7.0') {
				initializeGeneralizedDistribution($table);
			}
		}
	});

	
});
