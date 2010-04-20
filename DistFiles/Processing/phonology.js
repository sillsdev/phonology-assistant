// phonology.js 2010-04-20
// Interactive behavior for XHTML files exported from Phonology Assistant.
// Requires jQuery 1.3 or later.

// Execute when the DOM is fully loaded.

$(document).ready(function () {

	// Data Corpus or Search view ==============================================

	// Remove all non-alphanumeric characters from a field name to get its class.
	function fieldClassFromName(name) {
		return name.replace(/[^A-Za-z0-1]/g, '');
	}

	// To compare sort keys, undo changes to original field values for XHTML table cells.
	function sortFieldValue(sortKey) {
		if (sortKey.length === 1) {
			// A single non-breaking space replaced an empty field.
			if (sortKey.charCodeAt(0) === 160) {
				sortKey = ''; // TO DO: sortKey.length = 0?
			}
		}
		else if (sortKey.length !== 0) {
			// A non-breaking space at the beginning or end replaced a space.
			if (sortKey.charCodeAt(0) === 160) {
				sortKey = ' ' + sortKey.substr(1);
			}
			if (sortKey.charCodeAt(sortKey.length - 1) === 160) {
				sortKey = sortKey.substr(0, sortKey.length - 1) + ' ';
			}
		}
		return sortKey;
	}

	// To sort rows by a column, click its heading cell (or click a phonetic sort option).
	function clickColumnHeadingList(event) {
		var $target = $(event.target);
		var $th = $target.closest('th');
		var sortOptions = $th.hasClass('sortOptions');
		var reverse = $th.hasClass('sortField');
		if ($target.is('li')) {
			var liManner = $target.hasClass('mannerOfArticulation');
			var thManner = $th.hasClass('mannerOfArticulation');
			if (liManner && !thManner) {
				$th.addClass('mannerOfArticulation');
				reverse = false;
			} else if (!liManner && thManner) {
				$th.removeClass('mannerOfArticulation');
				reverse = false;
			}
		}
		if (reverse) {
			$th.toggleClass('descending');
		}
		var mannerOfArticulation = $th.hasClass('mannerOfArticulation');
		$th.find('ul').remove();
		
		var $table = $th.closest('table');
		$table.find('thead th.sortField').removeClass('sortField');
		var fieldName = $th.text();
		var fieldClass = fieldClassFromName(fieldName);
		var sortKeySelector = '.' + fieldClass;
		if (sortOptions) {
			sortKeySelector += (' ul.sortOrder li.' +
				(mannerOfArticulation ? 'mannerOfArticulation' : 'placeOfArticulation'));
		}
		else if ($table.find('td' + sortKeySelector + ' ul.sortOrder li').length !== 0) {
			sortKeySelector += ' ul.sortOrder li';
		}
		var descending = $th.hasClass('descending');
		var $parents;
		var childrenSelector;
		if ($table.find('tbody.group').length === 0) {
			// For an ungrouped list in Data Corpus or Search view,
			// sort all the rows in the table body element.
			$parents = $table.find('tbody');
			childrenSelector = 'tr';
		}
		else if ($table.find('tbody.group th.pair').length !== 0) {
			// For each minimal pair group in Search view, which is a tbody element,
			// sort the data rows, but not the heading row.
			$parents = $table.find('tbody.group');
			childrenSelector = 'tr.data';
		}
		else {
			// For each generic group in Data Corpus or Search view,
			// sort the entire group by the primary sort field in the heading row.
			$parents = $table;
			childrenSelector = 'tbody.group';
			sortKeySelector = 'tr.heading ' + sortKeySelector;
		}
		
		$parents.each(function () {
			var $parent = $(this);
			// TO DO: Find an idiom to reverse or sort, and then replace.
			var children = $parent.find(childrenSelector).remove().get();
			if (reverse) {
				children.reverse();
			} else {
				children.sort(function(childA, childB) {
					var sortKeyA = sortFieldValue($(childA).find(sortKeySelector).text());
					var sortKeyB = sortFieldValue($(childB).find(sortKeySelector).text());
					var result = sortKeyA.localeCompare(sortKeyB);
					return descending ? -result : result;
				});
			}
			$parent.append(children);
		});
		
		$th.addClass('sortField');
		var detailsAboutSortField = fieldName;
		if (sortOptions) {
			detailsAboutSortField += (', ' + (mannerOfArticulation ? 'manner of articulation' : 'place of articulation'));
		}
		if (descending) {
			detailsAboutSortField += ', descending';
		}
		$table.parent().find('table.details tr.primarySortField td').text(detailsAboutSortField);
	}

	// Execute when the DOM is fully loaded.
	function readyList() {
		$('table.list').each(function () {
			var $table = $(this);
			$table.addClass('interactive').click(function (event) {
				var $th = $(event.target).closest('th');
				if ($th.length === 1) {
					if ($th.hasClass('group')) {

						// To expand or collapse all groups, click the cell at the upper left.
						$(this).toggleClass('collapsed'); // this refers to the table.

					} else if ($th.hasClass('count') || $th.hasClass('pair')) {

						// To expand or collapse a group, click the cell at the left of its heading row
						// which contains the number of records or the minimal pair.
						$th.closest('tbody').toggleClass('collapsed');

					} else if ($th.hasClass('sortable')) {

						// To sort by a column, click its heading cell (or click a phonetic sort option).
						clickColumnHeadingList(event);

					}
				}
			});
			var hasGroups = $table.find('thead th.group').length !== 0;
			var hasPairs = hasGroups && $table.find('tbody.group th.pair').length !== 0;
			$table.find('thead th.sortOptions').hover(function () {
				/* When the mouse pointer moves over the heading cell, phonetic sort options appear. */
				$(this).append('<ul><li class="placeOfArticulation">place of articulation</li><li class="mannerOfArticulation">manner of articulation</li></ul>');
			},
			function () {
				/* When the mouse pointer moves out of the heading cell, phonetic sort options disappear. */
				$(this).find('ul').remove();
			});
			var $sortableColumnHeadings;
			if (!hasPairs) {
				if (hasGroups) {
					$sortableColumnHeadings = $table.find('thead th.sortField');
				}
				else {
					$sortableColumnHeadings = $table.find('thead th');
				}
				$sortableColumnHeadings.addClass('sortable');
			}
			$table.find('td > ul.transcription').parent().wrapInner('<div class="transcription"></div>');
		});
	}

	// Consonant Chart or Vowel Chart view =====================================

	// Utility functions -------------------------------------------------------

	// Convert list item to string.
	function textFromListItem(item) {
		// Internet Explorer 7 returns a trailing space in text of li elements.
		return $.trim($(item).text());
	}

	// Convert feature in the table to string.
	function featureFromCell(td) {
		var $td = $(td);
		var text = $td.text();
		if ($td.hasClass('plus')) {
			text = '+' + $td.parent().find('td.name').text();
		} else if ($td.hasClass('minus')) {
			text = '-' + $td.parent().find('td.name').text();
		}
		return text;
	}

	// Processing functions ----------------------------------------------------
	
	function featuresToMatchCV(classOfFeatures, $container, $tdActive) {
		var $features = $container.find('table.' + classOfFeatures + ' td.selected');
		if ($tdActive && $tdActive.closest('table.features').hasClass(classOfFeatures)) {
			$features = $features.add($tdActive);
		}
		return $.map($features, featureFromCell);
	}
	
	function matchUnitForClassFeaturesCV(classOfFeatures, features, $tdPhonetic) {
		var featuresOfUnit = $.map($tdPhonetic.find('ul.' + classOfFeatures + ' li'), textFromListItem);
		var i;
		for (i = 0; i !== features.length; i++) {
			if ($.inArray(features[i], featuresOfUnit) < 0) {
				// The phone does not match a feature.
				return false;
			}
		}
		return true;
	}

	// When the mouse pointer moves over or out of a feature,
	// or if you select or clear a feature, match phones in the chart.
	function matchUnitsForFeaturesCV($container, $tdActive) {
		var featuresArticulatory = featuresToMatchCV('articulatory', $container, $tdActive);
		var featuresBinary = featuresToMatchCV('binary', $container, $tdActive);
		var featuresHierarchical = featuresToMatchCV('hierarchical', $container, $tdActive);
		var $tableCV = $container.find('table.CV');

		if (featuresArticulatory.length === 0 && featuresBinary.length === 0 && featuresHierarchical.length === 0) {
			// If moving the mouse out of a feature and no other features selected.
			$tableCV.find('.matched').removeClass('matched');
		} else {
			// In non-empty cells of the chart, match units that have the features.
			$tableCV.find('td.Phonetic').each(function () {
				var $tdPhonetic = $(this);
				var matched = matchUnitForClassFeaturesCV('articulatory', featuresArticulatory, $tdPhonetic);
				if (matched) {
					matched = matchUnitForClassFeaturesCV('binary', featuresBinary, $tdPhonetic);
				}
				if (matched) {
					matched = matchUnitForClassFeaturesCV('hierarchical', featuresHierarchical, $tdPhonetic);
				}
				if (matched) {
					$tdPhonetic.addClass('matched');
				} else {
					$tdPhonetic.removeClass('matched');
				}
			});
		}
	}

	function matchUnitsForSelectedFeaturesCV($td) {
		matchUnitsForFeaturesCV($td.closest('table.features').parent());
	}

	function matchUnitsForSelectedAndActiveFeaturesCV($td) {
		matchUnitsForFeaturesCV($td.closest('table.features').parent(), $td);
	}

	function matchFeatureForActiveUnitCV($td, indexActive) {
		if (indexActive >= 0) {
			$td.addClass('matched');
		}
		else {
			$td.removeClass('matched');
		}
	}

	function matchFeatureForActiveAndSelectedUnitsCV($td, indexActive, indexSelected) {
		if (indexActive >= 0 && indexSelected >= 0) {
			$td.addClass('matched');
		}
		else if (indexActive >= 0) {
			$td.addClass('active');
		}
		else if (indexSelected >= 0) {
			$td.addClass('selected');
		}
		else {
			$td.removeClass('matched');
		}
	}

	function matchFeatureCV($td, feature, featuresActive, featuresSelected) {
		var indexActive = $.inArray(feature, featuresActive);
		if (featuresSelected) {
			matchFeatureForActiveAndSelectedUnitsCV($td, indexActive, $.inArray(feature, featuresSelected));
		} else {
			matchFeatureForActiveUnitCV($td, indexActive);
		}
	}

	function matchClassFeaturesCV(classOfFeatures, $container, $tdActive, $tdSelected) {
		var selector = 'ul.' + classOfFeatures + ' li';
		var featuresActive = $.map($tdActive.find(selector), textFromListItem);
		var featuresSelected;
		if ($tdSelected && $tdSelected.length === 1)  {
			featuresSelected = $.map($tdSelected.find(selector), textFromListItem);
		}
		$container.find('table.' + classOfFeatures + ' tbody tr').each(function () {
			var $tr = $(this);
			var $td = $tr.find('td.name');
			var featureName = $td.text();
			if ($tr.hasClass('bivalent')) {
				$td = $tr.find('td.plus');
				if ($td.length === 1) {
					matchFeatureCV($td, '+' + featureName, featuresActive, featuresSelected);
				}

				$td = $tr.find('td.minus');
				if ($td.length === 1) {
					matchFeatureCV($td, '-' + featureName, featuresActive, featuresSelected);
				}
			} else {
				matchFeatureCV($td, featureName, featuresActive, featuresSelected);
			}
		});
	}

	// When the mouse pointer moves over a cell in the chart, match features in the tables.
	function matchFeaturesCV($tdActive, $tdSelected) {
		var $container = $tdActive.closest('table').parent();
		matchClassFeaturesCV('articulatory', $container, $tdActive, $tdSelected);
		matchClassFeaturesCV('binary', $container, $tdActive, $tdSelected);
		matchClassFeaturesCV('hierarchical', $container, $tdActive, $tdSelected);
	}

	// When the mouse pointer moves away from a cell in the chart,
	// all features in the tables become unmarked.
	function unmatchFeaturesCV($container) {
		var $tablesOfFeatures = $container.find('table.features');
		$tablesOfFeatures.find('.matched').removeClass('matched')
			.end().find('.active').removeClass('active')
			.end().find('.selected').removeClass('selected');
	}

	// Event functions for tables ----------------------------------------------

	function mouseoverFeaturesCV() {
		$(this).parent().find('table.CV').addClass('inactive')
			.find('td.selected').removeClass('selected');
	}

	function mouseoutFeaturesCV() {
		var $container = $(this).parent();
		if ($container.find('table.features td.selected').length === 0) {
			$container.find('table.inactive').removeClass('inactive');
		}
	}

	function mouseoverChartCV() {
		// Chart becomes active and phones become unmatched.
		var $this = $(this);
		$this.removeClass('inactive');
		$this.find('td.matched').removeClass('matched');

		// Feature tables become inactive and selected features become cleared.
		var $container = $this.parent();
		$container.find('table.features').addClass('inactive')
			.find('td.selected').removeClass('selected');
		$container.find('table.binary')
			.find('td.inactive').removeClass('inactive');
	}

	function mouseoutChartCV() {
		$(this).parent().find('table.features').removeClass('inactive');
	}

	// Event functions for table cells -----------------------------------------

	function clickChartCV(event) {
		var $activeCell = $(event.target).closest('td');
		if ($activeCell.length === 0) {
			return;
		}

		var $table = $activeCell.closest('table');
		if ($activeCell.hasClass('selected')) {
			$activeCell.removeClass('selected');
		}
		else {
			var phoneticCell = $activeCell.hasClass('Phonetic');
			var $selectedCell = $table.find('td.selected');
			if ($selectedCell.length !== 0) {
				$selectedCell.removeClass('selected');
				unmatchFeaturesCV($table.parent());
				if (phoneticCell) {
					matchFeaturesCV($activeCell);
				}
			}
			if (phoneticCell) {
				$activeCell.addClass('selected');
			}
		}
	}

	function mouseoverUnitCV() {
		var $activeCell = $(this);
		var $table = $activeCell.closest('table');
		var $selectedCell = $table.find('td.selected');
		if ($selectedCell.length === 1 && $selectedCell.get(0) !== $activeCell.get(0)) {
			matchFeaturesCV($activeCell, $selectedCell);
		}
		else {
			matchFeaturesCV($activeCell);
		}
	}

	function mouseoutUnitCV() {
		unmatchFeaturesCV($(this).closest('table').parent());
	}

	function mouseoverFeatureCV() {
		var $td = $(this);
		if (!($td.hasClass('selected'))) {
			matchUnitsForSelectedAndActiveFeaturesCV($td);
		}
	}

	function mouseoutFeatureCV() {
		var $td = $(this);
		$td.removeClass('inactive');
		if (!($td.hasClass('selected'))) {
			matchUnitsForSelectedFeaturesCV($td);
		}
	}

	function mouseoverBinaryFeatureValueCV() {
		var $td = $(this);
		if (!($td.hasClass('inactive')) && !($td.hasClass('selected'))) {
			matchUnitsForSelectedAndActiveFeaturesCV($td);
		}
	}

	function mouseoutBinaryFeatureValueCV() {
		var $td = $(this);
		if (!($td.hasClass('selected'))) {
			if ($td.parent().find('td.selected').length === 0) {
				$td.removeClass('inactive');
			}
			matchUnitsForSelectedFeaturesCV($td);
		}
	}

	function clickFeatureCV($td) {
		// If you click to clear a selected feature, make it inactive
		// until you move the mouse out or click to select it again.
		if ($td.hasClass('selected')) {
			$td.addClass('inactive');
		}
		else if ($td.hasClass('inactive')) {
			$td.removeClass('inactive');
		}
			
		$td.toggleClass('selected');
		matchUnitsForSelectedFeaturesCV($td);
	}

	function clickBinaryFeatureValueCV($td) {
		var oppositeValue = $td.hasClass('minus') ? 'plus' : 'minus';
		var $oppositeValue = $td.parent().find('td.' + oppositeValue);
		if ($td.hasClass('selected')) {
			// If this value is selected, clear it.
			$td.removeClass('selected');
			$td.addClass('inactive');
			// The opposite value becomes active again.
			$oppositeValue.removeClass('inactive');
		}
		else {
			// Select this value.
			$td.addClass('selected');
			$td.removeClass('inactive');
			// The opposite value becomes inactive and is cleared if it was selected.
			$oppositeValue.addClass('inactive').removeClass('selected');
		}
		matchUnitsForSelectedFeaturesCV($td);
	}

	function clickFeaturesCV(event) {
		var $table = $(this);
		var $target = $(event.target);
		if ($target.is('td')) {
			if ($target.hasClass('name') && !($target.parent().hasClass('bivalent'))) {
				clickFeatureCV($target);
			}
			else if ($target.hasClass('plus') || $target.hasClass('minus')) {
				if ($table.hasClass('binary')) {
					clickBinaryFeatureValueCV($target);
				} else {
					clickFeatureCV($target);
				}
			}
		} else if ($target.closest('th').length === 1) {
			$table.toggleClass('collapsed');
		}
	}
	
	function headingOfFeatures() {
		var $th = $(this);
		// Assume that the text is wrapped in a span, preceded by an optional br.
		var text = $th.find('span').text();
		// Title attribute is the original text.
		// Abbreviation consists of the first character,
		// followed by U+25BE black down-pointing small triangle.
		$th.append('<abbr title="' + text + '">' + text.charAt(0) + '▾</abbr>');
	}

	// Execute when the DOM is fully loaded ------------------------------------
	
	function readyCV() {
		$('table.CV').each(function () {
			var $tableCV = $(this);
			var $tablesOfFeatures = $tableCV.parent().find('table.features');
			if ($tablesOfFeatures.length !== 0) {
				$tableCV.addClass('interactive')
						.hover(mouseoverChartCV, mouseoutChartCV)
						.click(clickChartCV)
					// Only non-empty cells of the chart:
					.find('td.Phonetic')
						.hover(mouseoverUnitCV, mouseoutUnitCV);
				$tablesOfFeatures.each(function () {
					var $table = $(this);
					$table.addClass('interactive')
							.click(clickFeaturesCV)
							.hover(mouseoverFeaturesCV, mouseoutFeaturesCV)
						.find('th').each(headingOfFeatures);
					if ($table.hasClass('articulatory')) {
						$table.find('td').hover(mouseoverFeatureCV, mouseoutFeatureCV);
					} else if ($table.hasClass('binary')) {
						$table.find('td.plus, td.minus')
							.hover(mouseoverBinaryFeatureValueCV, mouseoutBinaryFeatureValueCV)
						.end().find('tr.univalent td.name')
							.hover(mouseoverFeatureCV, mouseoutFeatureCV);
					} else if ($table.hasClass('hierarchical')) {
						$table.find('td.plus, td.minus, tr.univalent td.name')
							.hover(mouseoverFeatureCV, mouseoutFeatureCV);
					}
				});
			}
		});
	}

	// ===========================================================================

	// List of environments preceding a consonant or vowel distribution chart.

	function matchPhonesForSelectedEnvironmentsCV($environment) {
		var $listOfEnvironments = $environment.parent();
		$listOfEnvironments.find('li').removeClass('selected');
		$environment.addClass('selected');
		var environment = textFromListItem($environment);
		var $chart = $listOfEnvironments.parent().find('table.CV');
		$chart.find('.notInEnvironment').removeClass('notInEnvironment');
		$chart.find('td.Phonetic').each(function () {
			var $phone = $(this);
			var $phoneEnvironments = $phone.find('ul.environments li');
			var phoneEnvironments = $.map($phoneEnvironments, textFromListItem);
			if ($.inArray(environment, phoneEnvironments) < 0) {
				$phone.addClass('notInEnvironment');
			}
		});
	}

	function clickEnvironments(event) {
		var $environment = $(event.target).closest('li');
		matchPhonesForSelectedEnvironmentsCV($environment);
	}

	// Execute when the DOM is fully loaded.
	function readyEnvironments() {
		$('ul.environments > li.Phonetic:first-child').parent().addClass('interactive')
			.click(clickEnvironments);
		// Select the first environment and indicate which phones are not in it.
		$('ul.environments li.Phonetic:first-child').addClass('selected').each(function () {
			matchPhonesForSelectedEnvironmentsCV($(this));
		});
	}

	// Distribution Chart view =================================================

	// Insert the following symbols in empty cells corresponding to group headings.
	// To collapse a column group, click U+25C2 black left-pointing small triangle.
	// To expand a column group, click U+25B8 black right-pointing small triange.
	// To collapse a row group, click U+25B4 black up-pointing small triangle.
	// To expand a row group, click U+25BE black down-pointing small triangle.

	function readyGeneralizedDistribution($table) {
		$table.addClass('generalized');
		
		// Upper-left cell.
		$table.find('thead tr:first th:first-child').addClass('upperLeft');

		// General column groups, if any.
		var $colgroups = $table.find('colgroup');
		var $thCols = $table.find('thead tr.individual th');
		var dataCellsByRow = [];
		$table.find('tbody tr').each(function (i) {
			dataCellsByRow[i] = $(this).find('td');
		});
		var iCol = 0;
		$table.find('thead tr.general th.Phonetic').each(function (i) {
			var j, k;
			i++;
			var colgroupClass = 'colgroup' + i;
			var $thColgroup = $(this);
			$thColgroup.addClass(colgroupClass);
			$colgroups.eq(i).addClass(colgroupClass);

			var colspan = $thColgroup.attr('colspan');		
			colspan = (colspan.length === 0 ? 1 : parseInt(colspan, 10));

			// Column headings for the group in the individual heading row.
			// Empty heading cell corresponding to the column group.
			if (colspan !== 1) {
				$thCols.eq(iCol).addClass(colgroupClass).html('◂'); // left-pointing
			}
			// Individual heading cells.
			for (k = 1; k !== colspan; k++) {
				$thCols.eq(iCol + k).addClass(colgroupClass).addClass('individual');
			}

			// Data cells for the group.
			for (j = 0; j !== dataCellsByRow.length; j++) {
				var $td = dataCellsByRow[j];
				for (k = 0; k !== colspan; k++) {
					$td.eq(iCol + k).addClass(colgroupClass);
					//if (k !== 0)
					//	$td.eq(iCol + k).addClass('individual');
				}
			}

			iCol += colspan;
		});

		// General row groups, if any.
		// Empty heading cells corresponding to row groups.
		$table.find('tbody tr.general th').not('th.Phonetic').each(function () {
			var $th = $(this);
			if ($th.closest('tbody').find('tr').length !== 1) {
				$th.addClass('individual').html('▴'); // up-pointing
			}
		});
	}

	function colgroupChildren($colgroup, n) {
		var $cols = $colgroup.find('col');
		var nInitial = $cols.length;
		var i;
		if (n < nInitial) {
			for (i = n; i !== nInitial; i++) {
				$cols.eq(i).remove();
			}
		}
		else if (n > nInitial) {
			for (i = nInitial; i !== n; i++) {
				$colgroup.append('<col></col>');
			}
		}
	}

	// To temporarily collapse or expand all general rows and columns,
	// click the upper left of the table.
	function clickUpperLeftDistribution($thUpperLeft) {
		var $table = $thUpperLeft.closest('table');
		if ($table.hasClass('generalized')) {
			$table.toggleClass('collapsed');

			// Make the first column group consistent with heading rows and columns.
			var colspan = ($table.hasClass('collapsed') ? 1 : $table.find('tbody:first tr:first th').length);
			var rowspan = ($table.hasClass('collapsed') ? 1 : $table.find('thead tr').length);
			var $colgroups = $table.find('colgroup');
			colgroupChildren($colgroups.eq(0), colspan);
			$thUpperLeft.attr('colspan', colspan);
			$thUpperLeft.attr('rowspan', rowspan);
			
			// Make general column headings, if any, consistent with visible cells.
			$table.find('thead tr.general th.Phonetic').each(function (i) {
				i++;
				var $colgroup = $colgroups.eq(i);
				var colgroupClass = $colgroup.attr('class');
				var colspan = 1;
				if (!($table.hasClass('collapsed'))) {
					colspan += $table.find('thead tr.individual th.individual.' + colgroupClass).not('.collapsed').length;
				}
				colgroupChildren($colgroup, colspan);
				$(this).attr('colspan', colspan);
			});
			
			// Make general row headings, if any, consistent with visible rows.
			$table.find('tbody tr.general th.Phonetic').each(function () {
				var $thPhonetic = $(this);
				var $tbody = $thPhonetic.closest('tbody');
				var rowspan = ($table.hasClass('collapsed') || $tbody.hasClass('collapsed') ? 1 : $tbody.find('tr').length);
				$thPhonetic.attr('rowspan', rowspan);
			});
		}
		else {
			// Temporarily reduce all headings.
			$table.toggleClass('reduced');
		}
	}

	function clickColumnHeadingDistribution($th) {
		if ($th.hasClass('Phonetic')) {
			$th.toggleClass('reduced');
		}
		else if ($th.attr('class').length !== 0) {
			// Toggle the class, and then change the colgroup, colspan, and heading.
			var colgroupClass = $th.attr('class');
			var $table = $th.closest('table');
			$table.find('.individual.' + colgroupClass).toggleClass('collapsed');
			var colspan = $table.find('thead tr.individual th.' + colgroupClass).not('.collapsed').length;

			var $colgroup = $table.find('colgroup.' + colgroupClass);
			colgroupChildren($colgroup, colspan);

			$table.find('thead tr.general th.' + colgroupClass).attr('colspan', colspan);

			$th.html($th.html() === '◂' ? '▸' : '◂'); // right-pointing : left-pointing
		}
	}

	function clickRowHeadingDistribution($th) {
		if ($th.hasClass('Phonetic')) {
			$th.toggleClass('reduced');
		}
		else if ($th.attr('class').length !== 0) {
			// Toggle the class, and then change the rowspan and heading.
			var $tbody = $th.closest('tbody');
			$tbody.toggleClass('collapsed');
			var rowspan = ($tbody.hasClass('collapsed') ? 1 : $tbody.find('tr').length);
			$th.closest('tr').find('th.Phonetic').attr('rowspan', rowspan);
			$th.html($th.html() === '▴' ? '▾' : '▴'); // down-pointing : up-pointing
		}
	}

	function clickChartDistribution(event) {
		var $target = $(event.target);
		if ($target.is('span') && $target.hasClass('zero')) {
			$(this).toggleClass('nonZero');
		}
		else {	
			var $th = $target.closest('th');
			if ($th.length !== 0) {
				if ($th.hasClass('upperLeft')) {
					clickUpperLeftDistribution($th);
				}
				else if ($th.closest('thead').length !== 0) {
					clickColumnHeadingDistribution($th);
				}
				else {
					clickRowHeadingDistribution($th);
				}
			}
		}
	}

	// Execute when the DOM is fully loaded ------------------------------------
	
	function readyDistribution() {
		$('table.distribution').each(function () {
			var $table = $(this);
			$table.addClass('interactive');
			// To reverse the background color of zero/non-zero cells,
			// click at the lower-right of the upper-left cell.
			if ($table.find('td.zero').length !== 0) {
				$table.find('thead tr:first th:first')
					.append('<div><span class="zero">0</span></div>');
			}
			// To reduce or restore a heading, click the cell.
			$table.find('th.Phonetic')
				.wrapInner('<span></span>') // Wrap heading text in span,
				.append('<abbr>…</abbr>'); // Insert U+2026 horizontal ellipsis in abbr.
			// To collapse or expand a general row or column,
			// click the individual cell corresponding to the row or column group heading.
			if ($table.find('tr.general').length !== 0) {
				readyGeneralizedDistribution($table);
			}
			$table.click(clickChartDistribution);
		});
	}

	// Phonology report ========================================================
	
	// Each division consists of a heading (which is the first child),
	// one or more tables, optional paragraphs, and optional child divisions.
	// To expand or collapse a division, click its heading.
	
	$('div.report').find(':first-child').filter(':header').each(function () {
		$(this).addClass('interactive').click(function () {
			$(this).parent().toggleClass('collapsed');
		});
	});
	
	readyList();
	readyCV();
	readyEnvironments();
	readyDistribution();
});
