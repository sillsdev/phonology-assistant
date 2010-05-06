// phonology.js 2010-04-29
// Interactive behavior for XHTML files exported from Phonology Assistant.
// http://www.sil.org/computing/pa/index.htm

// Requires jQuery 1.4 or later. http://jquery.com/

// JSLint defines a professional subset of JavaScript. http://jslint.com
/*jslint white: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global jQuery */

"use strict";


// Execute when the DOM is fully loaded.
// The following is related to $(document).ready(...) but even better because:
// * It avoids an explicit global reference to document.
// * It ensures that $(...) is a shortcut for jQuery(...) within the function.

jQuery(function ($) {


	// Phonology report ========================================================
	
	// Each division consists of a heading (which is the first child),
	// one or more tables, optional paragraphs, and optional child divisions.

	// To expand/collapse a division, click its heading.
	function clickHeadingReport() {
		$(this).parent().toggleClass('collapsed');
	}
		
	$('div.report').find(':first-child').filter(':header').each(function () {
		// Although .each() iteration is unnecessary here, follow the pattern.
		$(this).addClass('interactive').click(clickHeadingReport);
	});


	// Data Corpus or Search view ==============================================

	// To compare sort keys, undo changes to original field values for XHTML table cells.
	function sortableValue(sortKey) {
		if (sortKey.length === 1) {
			// A single non-breaking space replaced an empty field.
			if (sortKey.charCodeAt(0) === 160) {
				sortKey = '';
			}
		}
		else if (sortKey.length) {
			// A non-breaking space at the beginning or end replaced a space.
			if (sortKey.charCodeAt(0) === 160) {
				sortKey = ' ' + sortKey.slice(1);
			}
			if (sortKey.charCodeAt(sortKey.length - 1) === 160) {
				sortKey = sortKey.slice(0, -1) + ' ';
			}
		}
		return sortKey;
	}

	// Within one or more parent elements, sort children according to the selectors.
	function sortChildren($parents, childrenSelector, sortKeySelector, descending) {
		$parents.each(function () {
			var $parent = $(this),
				children = $parent.find(childrenSelector).get(); // Array of DOM objects.
				
			children.sort(function (childA, childB) {
				var sortKeyA = sortableValue($(childA).find(sortKeySelector).text()),
					sortKeyB = sortableValue($(childB).find(sortKeySelector).text());

				return descending ?
					sortKeyB.localeCompare(sortKeyA) :
					sortKeyA.localeCompare(sortKeyB);
			});
			$parent.append(children); // Replace DOM objects in sorted order.
		});
	}

	// Sort according to data from a column heading.
	function sortList($table, data) {
		var $parents,
			childrenSelector,
			sortKeySelector = '.' + data.classOfField;

		if (data.sortOption) {
			// Phonetic or (someday) Phonemic field.
			sortKeySelector += (' ul.sortOrder li.' + data.sortOption);
		}
		if (data.groupField) {
			// The field by which the list is grouped.
			if (data.searchField) {
				// For Phonetic or (someday) Phonemic in Search view,
				// must sort rows because they might not contain identical values.
				sortChildren($table.find('tbody'), 'tr:not(.heading',
					sortKeySelector, data.descending);
			}
			// Sort groups by the heading row.
			$parents = $table;
			childrenSelector = 'tbody';
			sortKeySelector = 'tr.heading ' + sortKeySelector;
		} else {
			// Sort data (that is, non-heading) rows.
			// Differences in behavior:
			// * If the list is grouped, the Phonology Assistant program regroups,
			//   but an interactive Web page resorts within the same groups instead.
			// * If the list has minimal groups, the program or a Web page resorts,
			//   but if one minimal pair per group, a Web page has no sortable columns.
			$parents = $table.find('tbody');
			childrenSelector = 'tr:not(.heading)';
		}
		sortChildren($parents, childrenSelector, sortKeySelector, data.descending);
	}

	// To sort by a column, click its heading cell (or click a phonetic sort option).
	function clickColumnHeadingList($target) {
		var $th = $target.closest('th'),
			data = $th.data('phonology'),
			$table = $th.closest('table'),
			reverse = $th.hasClass('sortField'), // Click the active column heading.
			sortOptionSelected,
			$ins,
			$details,
			details;

		// Phonetic sort option.
		if ($target.is('li')) {
			sortOptionSelected = $target.data('phonology').sortOption;
			reverse = (sortOptionSelected === data.sortOption);
			data.sortOption = sortOptionSelected;
		}
		// The following condition is independent of the previous condition,
		// because you might click the heading instead of the list.
		if ($th.hasClass('sortOptions')) {
			$th.find('ul').remove();
		}
		
		if (data.descending === undefined) {
			// The first time you sort by any column other than the primary sort field,
			// assume that the direction is ascending and insert the visual indicator.
			data.descending = false;
			if (!($th.find('span').length)) {
				$th.wrapInner('<span></span>');
			}
			$th.append(' <ins>▴</ins>'); // up-pointing
		}
		
		if (reverse) {
			data.descending = !data.descending;
			$ins = $th.find('ins');
			if (data.descending) {
				$ins.text('▾'); // down-pointing
			} else {
				$ins.text('▴'); // up-pointing
			}
		} else {
			$table.find('thead th.sortField').removeClass('sortField');
			$th.addClass('sortField');
		}
		
		sortList($table, data);

		// Update the table of details.		
		$details = $table.parent().find('table.details tr.sortField td');
		if ($details.length) {
			details = $th.find('span').text(); // Name of sort field.
			// In XHTML files exported from Phonology Assistant,
			// the sort option is wrapped in a span with a class,
			// but it is unnecessary after initialization (see below).
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
				// To expand/collapse all groups, click at the upper left.
				$(this).toggleClass('collapsed'); // this refers to the table.
			} else if ($th.hasClass('count') || $th.hasClass('pair')) {
				// To expand/collapse a group,
				// click the cell at the left of its heading row.
				$th.closest('tbody').toggleClass('collapsed');
			} else if ($th.hasClass('sortable')) {
				// To sort by a column,
				// click its heading cell (or click a phonetic sort option).
				clickColumnHeadingList($target);
			}
		}
	}

	// When the mouse pointer moves over a Phonetic or (someday) Phonemic
	// heading cell, phonetic sort options appear.
	function mouseoverSortOptionList() {
		var $th = $(this),
			sortOption = $th.data('phonology').sortOption,
			$ul = $('<ul></ul>'),
			$li;
			
		$li = $('<li>place of articulation</li>')
			.data('phonology', {sortOption: 'placeOfArticulation'});
		if (sortOption === 'placeOfArticulation') {
			$li.addClass('initial');
		}
		$ul.append($li);
		$li = $('<li>manner of articulation</li>')
			.data('phonology', {sortOption: 'mannerOfArticulation'});
		if (sortOption === 'mannerOfArticulation') {
			$li.addClass('initial');
		}
		$ul.append($li);
		$th.append($ul);
	}

	// When the mouse pointer moves out of the heading cell,
	// phonetic sort options disappear.
	function mouseoutSortOptionList() {
		$(this).find('ul').remove();
	}

	// Initialization ----------------------------------------------------------

	// Initialize sorting data for column headings of lists.
	function initializeDataForHeadingList() {
		var $th = $(this),
			$span = $th.find('span'),
			nameOfField = $span.length ? $span.text() : $th.text(),
			$ins = $th.find('ins'),
			ins,
			$sortOption,
			data = {};

		// The HTML/CSS class for a field consists of alphanumeric characters only.
		// \W is the opposite of \w, which is equivalent to [0-9A-Z_a-z].
		// Therefore, [\W_] is equivalent to [^0-9A-Za-z],
		// but JSLint does not consider it an insecure use of [^...].
		data.classOfField = nameOfField.replace(/[\W_]/g, '');
		
		if ($ins.length) {
			// In XHTML files exported from Phonology Assistant,
			// an ins element indicates the direction for the primary sort field.
			ins = $ins.text();
			if (ins === '▾') { // down-pointing
				data.descending = true;
			} else if (ins === '▴') { // up-pointing
				data.descending = false;
			}
		}
		if ($th.hasClass('sortOptions')) {
			// In XHTML files exported from Phonology Assistant,
			// the sort option is wrapped in a span with a class,
			$sortOption = $th.closest('table').parent()
				.find('table.details tr.sortField td span[class]');
			if ($sortOption.hasClass('placeOfArticulation')) {
				data.sortOption = 'placeOfArticulation';
			} else if ($sortOption.hasClass('mannerOfArticulation')) {
				data.sortOption = 'mannerOfArticulation';
			} else {
				data.sortOption = null;
			}
		}
		if ($th.hasClass('sortField') && $th.parent().has('th.group').length) {
			// The field by which the list is grouped.
			data.groupField = true;
		}
		if ($th.attr('class') === '3') {
			// Phonetic or (someday) Phonemic in Search view,
			data.searchField = true;
		}
		$th.data('phonology', data);
	}

	$('table.list').each(function () {
		var $table = $(this);
		
		$table.addClass('interactive').click(clickList)
			.find('thead th.sortOptions')
				.hover(mouseoverSortOptionList, mouseoutSortOptionList);
		
		if (!($table.has('tbody.group th.pair').length)) {
			// Except when there is one minimal pair per group,
			// you can sort any column which contains data.
			$table.find('thead th:not(.group)').addClass('sortable')
				.each(initializeDataForHeadingList);
		}
		
		$table.find('td > ul.transcription')
			// Equivalent to the :before pseudo-element in CSS,
			// which Internet Explorer 7 does not support.
			// U+25B8 black right-pointing small triangle.
			.find('li.change del + ins').before(' ▸ ')
			// The phonology.css file assumes all siblings are in a div.
			// because the data cell itself cannot have relative position.
			.end().parent().wrapInner('<div class="transcription"></div>');
	});


	// Consonant Chart or Vowel Chart view =====================================

	// Utility functions -------------------------------------------------------

	// Convert list item (DOM) to string.
	function textFromListItem(li) {
		// Internet Explorer 7 returns a trailing space in text of li elements.
		return $.trim($(li).text());
	}

	// Processing functions ----------------------------------------------------
	
	// Return whether a particular unit matches a set set of features.
	function matchUnitCV($tdPhonetic, allFeaturesToMatch) {
		var allFeaturesOfUnit = $tdPhonetic.data('phonology').features,
			classOfFeatures,
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
	// and the optional active feature in a set of related feature tables.
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
				if (matchUnitCV($tdPhonetic, allFeaturesToMatch)) {
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

	// Match a particular feature according to the active (unit) data cell
	// and the optional selected (unit) data cell in a consonant or vowel chart.
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

	// Match all feature tables according to the active (unit) data cell
	// and the optional selected (unit) data cell in a consonant or vowel chart.
	function matchFeaturesCV($tdActive, $tdSelected) {
		$tdActive.closest('table').parent().find('table.features').each(function () {
			var $table = $(this),
				classOfFeatures = $table.data('phonology').classOfFeatures,
				featuresActive = $tdActive.data('phonology').features[classOfFeatures],
				featuresSelected;
			
			if ($tdSelected && $tdSelected.length && 
					$tdSelected.get(0) !== $tdActive.get(0)) {
				featuresSelected = $tdSelected.data('phonology').features[classOfFeatures];
			}

			$table.find('tbody tr').each(function () {
				var $tr = $(this),
					$td;
				
				if ($tr.hasClass('bivalent')) {
					$td = $tr.find('td.plus');
					if ($td.length) {
						matchFeatureCV($td, featuresActive, featuresSelected);
					}
					$td = $tr.find('td.minus');
					if ($td.length) {
						matchFeatureCV($td, featuresActive, featuresSelected);
					}
				} else {
					matchFeatureCV($tr.find('td.name'), featuresActive, featuresSelected);
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

	// Move the mouse pointer into a feature table:
	function mouseoverFeaturesCV() {
		// Related CV charts become (visually) inactive.
		$(this).parent().find('table.CV').removeClass('interactive')
			// The selected unit (if any) is cleared.
			.find('td.selected').removeClass('selected');
	}

	// Move the mouse pointer out of a feature table:
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

		if ($target.is('td')) {
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
		var $this = $(this),
			$container = $this.parent();

		// Chart becomes active and units become unmatched.
		$this.addClass('interactive')
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

		// If you click in a data cell.
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
					} else {
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
				unitEnvironments = $tdPhonetic.data('phonology').environments;

			if ($.inArray(environment, unitEnvironments) < 0) {
				$tdPhonetic.addClass('zero');
			}
		});
	}

	function clickEnvironments(event) {
		matchUnitsForSelectedEnvironmentCV($(event.target).closest('li'));
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
				.end().find('li:first-child').addClass('selected')
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
		// followed by U+25BE black down-pointing small triangle.
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
		// Instead of U+002D hyphen-minus, the text might be U+2013 en dash.
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
	});


	// Distribution Chart view =================================================

	// Insert the following symbols in empty cells corresponding to group headings.
	// To collapse a column group, click U+25C2 black left-pointing small triangle.
	// To expand a column group, click U+25B8 black right-pointing small triangle.
	// To collapse a row group, click U+25B4 black up-pointing small triangle.
	// To expand a row group, click U+25BE black down-pointing small triangle.

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
			.addClass('interactive') // Used by CSS, not JavaScript.
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
					.html('◂'); // left-pointing
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

			$th.addClass('individual'); // TO DO: CSS
			if (rowspan !== 1) {
				$tr.find('th.Phonetic').data('phonology', {rowspan: rowspan});
				$th.addClass('interactive').html('▴'); // up-pointing
			}
		});
	}

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
				var $thPhonetic = $(this),
					$tbody;

				if (collapsed) {
					rowspan = 1;
				} else {
					$tbody = $thPhonetic.closest('tbody');
					if ($tbody.hasClass('collapsed')) {
						rowspan = 1;
					} else {
						rowspan = $thPhonetic.data('phonology').rowspan;
					}
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
			$thCollapsible.html(heading);

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
			$tbody = $tr.parent();
			$tbody.toggleClass('collapsed');

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
			$thCollapsible.html(heading);
		}
	}

	function clickChartDistribution(event) {
		var $target = $(event.target),
			$th;

		if ($target.is('span.zero')) {
			$(this).toggleClass('nonZero')
				.parent().find("ul.environments + table.CV").toggleClass('nonZero');
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

	// TO DO: Modularize the dependency between distribution chart
	// and related consonant or vowel chart with tabbed environments.	
	function clickZeroCV()
	{
		$(this).closest('table').toggleClass('nonZero')
			.parent().find('table.distribution').toggleClass('nonZero');
	}

	// Initialization ----------------------------------------------------------
	
	$('table.distribution').each(function () {
		var $table = $(this);
		$table.addClass('interactive');
		// To reverse the background color of zero/non-zero cells,
		// click at the lower-right of the upper-left cell.
		if ($table.find('td.zero').length) {
			$table.find('thead tr:first-child th:first-child')
				.append('<div><span class="zero">0</span></div>');
			$table.parent().find("ul.environments + table.CV thead tr:first-child th:first-child")
				.append('<div><span class="zero">0</span></div>')
				.find('span.zero').click(clickZeroCV);
		}
		// To reduce or restore a heading, click the cell.
		$table.find('th.Phonetic')
			.wrapInner('<span></span>') // Wrap heading text in span,
			.append('<abbr>…</abbr>'); // Insert U+2026 horizontal ellipsis in abbr.
		// To collapse or expand a general row or column,
		// click the individual cell corresponding to the row or column group heading.
		if ($table.find('tr.general').length) {
			initializeGeneralizedDistribution($table);
		}
		$table.click(clickChartDistribution);
	});

	
});
