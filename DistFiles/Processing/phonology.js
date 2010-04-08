// phonology.js 2010-04-06
// Interactive behavior for XHTML files exported from Phonology Assistant.
// Requires jQuery 1.3 or later.

// Execute when the DOM is fully loaded.
$(document).ready(function(){
	readyReport();
	readyList();
	readyEnvironments();
	readyCV();
	readyDistribution();
});

// =============================================================================

// Phonology report.

// Execute when the DOM is fully loaded.
function readyReport()
{
	$("div.report").each(function () {
		$(this).addClass("interactive").find(":header").click(clickHeadingReport);
	});
}

// To temporarily expand or collapse a report division, click its heading.
function clickHeadingReport() {
	$(this).closest("div").toggleClass("collapsed");
}

// =============================================================================

// List in Data Corpus or Search view.

// Execute when the DOM is fully loaded.
function readyList() {
	$("table.list").each(function () {
		var $table = $(this);
		$table.addClass("interactive");
		var hasGroups = $table.find("thead th.group").length !== 0;
		var hasPairs = $table.find("tbody.group th.pair").length !== 0;
		if (hasGroups) {
			$table.find("thead tr th.group")
				.click(clickUpperLeftList)
			.end().find("tbody.group tr.heading th:first-child")
				.click(clickGroupHeadingList);
		}
		$table.find("thead th.sortOptions")
			.hover(mouseoverPhoneticColumnHeadingList, mouseoutPhoneticColumnHeadingList);
		var $sortableColumnHeadings;
		if (!hasPairs) {
			if (hasGroups)
				$sortableColumnHeadings = $table.find("thead th.sortField");
			else
				$sortableColumnHeadings = $table.find("thead th");
			$sortableColumnHeadings.addClass("sortable").click(clickColumnHeadingList);
		}
	});
}

// To temporarily expand or collapse all groups, click the upper left of the table.
function clickUpperLeftList() {
	$(this).closest("table").toggleClass("collapsed");
}

// To expand or collapse a group, click the number of records or minimal pairs cell in its heading row.
function clickGroupHeadingList() {
	$(this).closest("tbody").toggleClass("collapsed");
}

// When the mouse pointer moves over the heading cell, phonetic sort options appear.
function mouseoverPhoneticColumnHeadingList() {
	$(this).append('<ul><li class="placeOfArticulation">place of articulation</li><li class="mannerOfArticulation">manner of articulation</li></ul>');
}

// When the mouse pointer moves out of the heading cell, phonetic sort options disappear.
function mouseoutPhoneticColumnHeadingList() {
	$(this).find("ul").remove();
}

// To sort rows by a column, click its heading cell (or click a phonetic sort option).
function clickColumnHeadingList(event) {
	var $this = $(this);
	var sortOptions = $this.hasClass("sortOptions");
	if (this === event.target) { // Click the heading.
		if ($this.hasClass("sortField"))
			$this.toggleClass("descending");
	}
	else { // Click a phonetic sort option.
		if ($(event.target).hasClass("mannerOfArticulation"))
			$this.addClass("mannerOfArticulation");
		else
			$this.removeClass("mannerOfArticulation");
	}
	var mannerOfArticulation = $this.hasClass("mannerOfArticulation");
	$this.find("ul").remove();
	
	var $table = $this.closest("table");
	$table.find("thead th.sortField").removeClass("sortField");
	var fieldName = $this.text();
	var fieldClass = fieldClassFromName(fieldName);
	var sortKeySelector = "." + fieldClass;
	if (sortOptions) {
		sortKeySelector += (" ul.sortOrder li." +
			(mannerOfArticulation ? "mannerOfArticulation" : "placeOfArticulation"));
	}
	else if ($table.find("td" + sortKeySelector + " ul.sortOrder li").length !== 0) {
		sortKeySelector += " ul.sortOrder li";
	}
	var direction = $this.hasClass("descending") ? -1 : 1;
	var $parents;
	var childrenSelector;
	if ($table.find("tbody.group").length === 0) {
		// For an ungrouped list in Data Corpus or Search view,
		// sort all the rows in the table body element.
		$parents = $table.find("tbody");
		childrenSelector = "tr";
	}
	else if ($table.find("tbody.group th.pair").length !== 0) {
		// For each minimal pair group in Search view, which is a tbody element,
		// sort the data rows, but not the heading row.
		$parents = $table.find("tbody.group");
		childrenSelector = "tr.data";
	}
	else {
		// For each generic group in Data Corpus or Search view,
		// sort the entire group by the primary sort field in the heading row.
		$parents = $table;
		childrenSelector = "tbody.group";
		sortKeySelector = "tr.heading " + sortKeySelector;
	}
	
	$parents.each(function () {
		var $parent = $(this);
		// TO DO: Find an idiom to sort and replace.
		var $children = $parent.find(childrenSelector).remove().get().sort(function(childA,childB){
			var sortKeyA = sortFieldValue($(childA).find(sortKeySelector).text());
			var sortKeyB = sortFieldValue($(childB).find(sortKeySelector).text());
			if (sortKeyA > sortKeyB)
				return direction;
			else if (sortKeyB > sortKeyA)
				return -direction;
			else
				return 0;
		});
		$parent.append($children);
	});
	
	$this.addClass("sortField");
	var detailsAboutSortField = fieldName;
	if (sortOptions)
		detailsAboutSortField += (", " + (mannerOfArticulation ? "manner of articulation" : "place of articulation"));
	if (direction < 0)
		detailsAboutSortField += ", descending";
	$table.parent().find("table.details tr.primarySortField td").text(detailsAboutSortField);
}

// Remove all non-alphanumeric characters from a field name to get its class.
function fieldClassFromName(name) {
	return name.replace(/[^A-Za-z0-1]/g, "");
}

// To compare sort keys, undo changes to original field values for XHTML table cells.
function sortFieldValue(sortKey) {
	if (sortKey.length === 1) {
		// A single non-breaking space replaced an empty field.
		if (sortKey.charCodeAt(0) === 160)
			sortKey = "";
	}
	else if (sortKey.length !== 0) {
		// A non-breaking space at the beginning or end replaced a space.
		if (sortKey.charCodeAt(0) === 160)
			sortKey = " " + sortKey.substr(1);
		if (sortKey.charCodeAt(sortKey.length - 1) === 160)
			sortKeyA = sortKeyA.substr(0,sortKeyA.length - 1) + " ";
	}
	return sortKey;
}

// =============================================================================

// List of environments preceding a consonant or vowel distribution chart.

// Execute when the DOM is fully loaded.
function readyEnvironments() {
	$("ul.environments > li.Phonetic:first-child").parent().addClass("interactive")
		.click(clickEnvironments);
	// Select the first environment and indicate which phones are not in it.
	$("ul.environments li.Phonetic:first-child").addClass("selected").each(function () {
		matchPhonesForSelectedEnvironmentsCV($(this));
	});
}

function clickEnvironments(event) {
	var $environment = $(event.target).closest("li");
	matchPhonesForSelectedEnvironmentsCV($environment);
}

function matchPhonesForSelectedEnvironmentsCV($environment) {
	var $listOfEnvironments = $environment.parent();
	$listOfEnvironments.find("li").removeClass("selected");
	$environment.addClass("selected");
	var environment = textFromListItem($environment);
	var $chart = $listOfEnvironments.parent().find("table.CV");
	$chart.find(".notInEnvironment").removeClass("notInEnvironment");
	// TO DO: Cells which contain a list of multiple phones.
	$chart.find("td.Phonetic").each(function () {
		var $phone = $(this);
		var $phoneEnvironments = $phone.find("ul.environments li");
		var phoneEnvironments = $.map($phoneEnvironments, textFromListItem);
		if ($.inArray(environment, phoneEnvironments) < 0)
			$phone.addClass("notInEnvironment");
	});
}

// =============================================================================

// Tables of phones and features in Consonant Chart or Vowel Chart view.

// Execute when the DOM is fully loaded.
function readyCV() {
	// The following do not require iteration using an .each() function.
	$("table.articulatory").not(".phone").addClass("interactive")
			.click(clickFeaturesCV)
			.hover(mouseoverFeaturesCV, mouseoutFeaturesCV)
		.find("td")
			.hover(mouseoverFeatureCV, mouseoutFeatureCV)
		// To collapse or expand the table, click the heading cell.
		.end().find("th")
			.append("<del>A</del>")
			.contents().filter(function()
			{
				return this.nodeType === 3; // Text node.
			})
				.wrap("<ins></ins>"); // Wrap heading text in ins.
	$("table.binary").not(".phone").addClass("interactive")
			.click(clickFeaturesCV)
			.hover(mouseoverFeaturesCV, mouseoutFeaturesCV)
		.find("td.minus, td.plus, div")
			.hover(mouseoverBinaryFeatureCV, mouseoutBinaryFeatureCV)
		// To collapse or expand the table, click the heading cell.
		.end().find("th")
			.append("<del>±b</del>") // Insert U+00B1 plus-minus sign in del.
			.contents().filter(function()
			{
				return this.nodeType === 3; // Text node.
			})
				.wrap("<ins></ins>"); // Wrap heading text in ins.
	$("table.hierarchical").addClass("interactive")
			.click(clickFeaturesCV)
			.hover(mouseoverFeaturesCV, mouseoutFeaturesCV)
		.find("td.minus, td.plus, div")
			.hover(mouseoverFeatureCV, mouseoutFeatureCV)
		.end().find("th")
			.append("<del>±h</del>") // Insert U+00B1 plus-minus sign in del.
			.contents().filter(function()
			{
				return this.nodeType === 3; // Text node.
			})
				.wrap("<ins></ins>"); // Wrap heading text in ins.
	$("table.CV").addClass("interactive")
				.hover(mouseoverChartCV, mouseoutChartCV)
				.click(clickChartCV)
			// Only non-empty cells of the chart:
			.find("td.Phonetic")
				.hover(mouseoverPhoneCV, mouseoutPhoneCV);
}

// Event functions for tables ------------------------------------------------

function mouseoverFeaturesCV() {
	$(this).parent().find("table.CV").addClass("inactive")
		.find("td.selected").removeClass("selected");
}

function mouseoutFeaturesCV() {
	var $container = $(this).parent();
	if ($container.find("table.features td.selected").length === 0)
		$container.find("table.inactive").removeClass("inactive");
}

function mouseoverChartCV() {
	// Chart becomes active and phones become unmatched.
	var $this = $(this);
	$this.removeClass("inactive");
	$this.find("td.matched").removeClass("matched");

	// Feature tables become inactive and selected features become cleared.
	var $container = $this.parent();
	$container.find("table.features").addClass("inactive")
		.find("td.selected").removeClass("selected");
	$container.find("table.binary")
		.find("td.inactive").removeClass("inactive");
}

function mouseoutChartCV() {
	$(this).parent().find("table.features").removeClass("inactive");
}

// Event functions for table cells -------------------------------------------

function mouseoverFeatureCV() {
	var $feature = $(this);
	if (!($feature.hasClass("selected")))
		matchPhonesForActiveAndSelectedFeaturesCV($feature);
}

function mouseoutFeatureCV() {
	var $feature = $(this);
	$feature.removeClass("inactive");
	if (!($feature.hasClass("selected")))
		matchPhonesForSelectedFeaturesCV($feature);
}

function mouseoverBinaryFeatureCV() {
	var $feature = $(this);
	if (!($feature.hasClass("inactive")) && !($feature.hasClass("selected")))
		matchPhonesForActiveAndSelectedFeaturesCV($feature);
}

function mouseoutBinaryFeatureCV() {
	var $feature = $(this);
	if (!($feature.hasClass("selected"))) {
		if ($feature.parent().find("td.selected").length === 0)
			$feature.removeClass("inactive");
		matchPhonesForSelectedFeaturesCV($feature)
	}
}

function clickFeaturesCV(event) {
	var $target = $(event.target);
	var $table = $(this);
	if ($target.is("div"))
		clickFeatureCV($target);
	else if ($target.closest("th").length === 1)
		$table.toggleClass("collapsed");
	else {
		var $td = $target.closest("td");
		if ($td.length === 1) {
			if ($table.hasClass("articulatory"))
				clickFeatureCV($td);
			else if ($table.hasClass("hierarchical")) {
				if ($td.hasClass("plus") || $td.hasClass("minus"))
					clickFeatureCV($td);
			}
			else if ($td.hasClass("plus") || $td.hasClass("minus")) {
				clickBinaryFeatureCV($td);
			}
		}
	}
}

function clickFeatureCV($feature) {
	// If you click to clear a selected feature, make it inactive
	// until you move the mouse out or click to select it again.
	if ($feature.hasClass("selected"))
		$feature.addClass("inactive");
	else if ($feature.hasClass("inactive"))
		$feature.removeClass("inactive");
		
	$feature.toggleClass("selected");
	matchPhonesForSelectedFeaturesCV($feature);
}

function clickBinaryFeatureCV($feature) {
	var oppositeValue = $feature.hasClass("minus") ? "plus" : "minus";
	var $oppositeValue = $feature.parent().find("td." + oppositeValue);
	if ($feature.hasClass("selected")) {
		// If this value is selected, clear it.
		$feature.removeClass("selected");
		$feature.addClass("inactive");
		// The opposite value becomes active again.
		$oppositeValue.removeClass("inactive");
	}
	else {
		// Select this value.
		$feature.addClass("selected");
		$feature.removeClass("inactive");
		// The opposite value becomes inactive and is cleared if it was selected.
		$oppositeValue.addClass("inactive").removeClass("selected");
	}
	matchPhonesForSelectedFeaturesCV($feature);
}

function clickChartCV(event) {
	var $activeCell = $(event.target).closest("td");
	if ($activeCell.length === 0)
		return;

	var $table = $activeCell.closest("table");
	var $container = $table.parent();
	if ($activeCell.hasClass("selected")) {
		$activeCell.removeClass("selected");
	}
	else {
		var phoneticCell = $activeCell.hasClass("Phonetic");
		var $selectedCell = $table.find("td.selected");
		if ($selectedCell.length !== 0) {
			$selectedCell.removeClass("selected");
			unmatchFeaturesCV($table.parent());
			if (phoneticCell)
				matchFeatures1CV($activeCell);
		}
		if (phoneticCell) {
			$activeCell.addClass("selected");
		}
	}
}

function mouseoverPhoneCV() {
	var $activeCell = $(this);
	var $table = $activeCell.closest("table");
	var $selectedCell = $table.find("td.selected");
	if ($selectedCell.length === 1 && $selectedCell.get(0) !== $activeCell.get(0))
		matchFeatures2CV($activeCell, $selectedCell);
	else
		matchFeatures1CV($activeCell);
}

function mouseoutPhoneCV() {
	unmatchFeaturesCV($(this).closest("table").parent());
}

// Processing functions ------------------------------------------------------

// When the mouse pointer moves over or out of a feature,
// or if you select or clear a feature, match phones in the chart.

function matchPhonesForActiveAndSelectedFeaturesCV($feature) {
	var $table = $feature.closest("table.features");
	var $container = $table.parent();
	var $matchArticulatory = $container.find("table.articulatory td.selected");
	var $matchBinary = $container.find("table.binary td.selected");
	var $matchHierarchical = $container.find("table.hierarchical .selected");

	// Append the active feature to the array.
	if ($table.hasClass("articulatory"))
		$matchArticulatory = $matchArticulatory.add($feature);
	else if ($table.hasClass("binary"))
		$matchBinary = $matchBinary.add($feature);
	else
		$matchHierarchical = $matchHierarchical.add($feature);

	matchPhonesCV($container, $matchArticulatory, $matchBinary, $matchHierarchical);
}

function matchPhonesForSelectedFeaturesCV($feature) {
	var $container = $feature.closest("table.features").parent();
	var $matchArticulatory = $container.find("table.articulatory td.selected");
	var $matchBinary = $container.find("table.binary td.selected");
	var $matchHierarchical = $container.find("table.hierarchical .selected");

	matchPhonesCV($container, $matchArticulatory, $matchBinary, $matchHierarchical);
}

function matchPhonesCV($container, $matchArticulatory, $matchBinary, $matchHierarchical) {
	var $table = $container.find("table.CV");

	// Arrays of features that phones must match.
	var matchArticulatory = $.map($matchArticulatory, textFromArticulatoryFeature);
	var matchBinary = $.map($matchBinary, textFromBinaryFeature);
	var matchHierarchical = $.map($matchHierarchical, textFromHierarchicalFeature);

	// If moving the mouse out of a feature and no other features selected.
	if (matchArticulatory.length === 0 && matchBinary.length === 0 && matchHierarchical.length === 0) {
		$table.find(".matched").removeClass("matched");
		return;
	}
	
	// In non-empty cells of the chart, match phones that have the features.
	// TO DO: List of multiple phones in a cell.
	$table.find("td.Phonetic").each(function () {
		var $phone = $(this);
	
		// Arrays of features for the phone.
		var $divFeatures = $phone.find("div.features");
		var phoneArticulatory = $.map($divFeatures.find("ul.articulatory li"), textFromListItem);
		var phoneBinary = $.map($divFeatures.find("ul.binary li"), textFromListItem);
		var phoneHierarchical = $.map($divFeatures.find("ul.hierarchical li"), textFromListItem);
		
		for (var i = 0; i !== matchArticulatory.length; i++) {
			if ($.inArray(matchArticulatory[i], phoneArticulatory) < 0) {
				// The phone does not match an articulatory feature.
				$phone.removeClass("matched");
				return true;
			}
		}

		for (var i = 0; i !== matchBinary.length; i++) {
			if ($.inArray(matchBinary[i], phoneBinary) < 0) {
				// The phone does not match a binary feature.
				$phone.removeClass("matched");
				return true;
			}
		}

		for (var i = 0; i !== matchHierarchical.length; i++) {
			if ($.inArray(matchHierarchical[i], phoneHierarchical) < 0) {
				// The phone does not match a hierarchical feature.
				$phone.removeClass("matched");
				return true;
			}
		}

		// The phone matches all the features.
		$phone.addClass("matched");
	})
}

// When the mouse pointer moves over a cell in the chart, match features in the tables.
function matchFeatures1CV($activeCell) {
	// Arrays of features for the cell.
	var activeArticulatoryFeatures = $.map($activeCell.find("ul.articulatory li"), textFromListItem);
	var activeBinaryFeatures = $.map($activeCell.find("ul.binary li"), textFromListItem);
	var activeHierarchicalFeatures = $.map($activeCell.find("ul.hierarchical li"), textFromListItem);
	
	var $container = $activeCell.closest("table").parent();
	var $td;
	var feature;
	var activeIndex;

	// For every articulatory feature in the table, match it with the cell.
	$container.find("table.articulatory td").each(function () {
		$td = $(this);
		feature = $td.text();
		activeIndex = $.inArray(feature, activeArticulatoryFeatures);
		matchFeature1CV($td, activeIndex);
	})

	// For every binary feature in the table, match it with the cell.
	$container.find("table.binary tbody tr").each(function () {
		var $tr = $(this);
		var $name = $tr.find(".name");
		var featureName = $name.text();
		
		$td = $tr.find("td.univalent");
		if ($td.length === 1) {
			feature = '+' + featureName;
			activeIndex = $.inArray(feature, activeBinaryFeatures);
			matchFeature1CV($name, activeIndex);
		}

		$td = $tr.find("td.minus");
		if ($td.length === 1) {
			feature = '-' + featureName;
			activeIndex = $.inArray(feature, activeBinaryFeatures);
			matchFeature1CV($td, activeIndex);
		}

		$td = $tr.find("td.plus");
		if ($td.length === 1) {
			feature = '+' + featureName;
			activeIndex = $.inArray(feature, activeBinaryFeatures);
			matchFeature1CV($td, activeIndex);
		}
	})

	// For every hierarchical feature in the table, match it with the cell.
	$container.find("table.hierarchical tbody tr").each(function () {
		var $tr = $(this);
		$td = $tr.children("td.name");
		if ($td.length === 1) {
			var featureName = $td.text();

			$td = $tr.children("td.minus");
			if ($td.length === 1) {
				feature = '-' + featureName;
				activeIndex = $.inArray(feature, activeHierarchicalFeatures);
				matchFeature1CV($td, activeIndex);
			}

			$td = $tr.children("td.plus");
			if ($td.length === 1) {
				feature = '+' + featureName;
				activeIndex = $.inArray(feature, activeHierarchicalFeatures);
				matchFeature1CV($td, activeIndex);
			}
		}
		else {
			var $div = $tr.children("td.univalent").children("div");
			feature = $div.text();
			activeIndex = $.inArray(feature, activeHierarchicalFeatures);
			matchFeature1CV($div, activeIndex);
		}
	})
}

function matchFeature1CV($td, activeIndex) {
	if (activeIndex >= 0)
		$td.addClass("matched");
	else
		$td.removeClass("matched");
}

// When the mouse pointer moves over a cell in the chart,
// if another cell is selected, match common and differing features in the tables.
function matchFeatures2CV($activeCell, $selectedCell) {
	// Arrays of features for the cells.
	var activeArticulatoryFeatures = $.map($activeCell.find("ul.articulatory li"), textFromListItem);
	var activeBinaryFeatures = $.map($activeCell.find("ul.binary li"), textFromListItem);
	var activeHierarchicalFeatures = $.map($activeCell.find("ul.hierarchical li"), textFromListItem);
	var selectedArticulatoryFeatures = $.map($selectedCell.find("ul.articulatory li"), textFromListItem);
	var selectedBinaryFeatures = $.map($selectedCell.find("ul.binary li"), textFromListItem);
	var selectedHierarchicalFeatures = $.map($selectedCell.find("ul.hierarchical li"), textFromListItem);
	
	var $container = $activeCell.closest("table").parent();
	var $td;
	var feature;
	var activeIndex;
	var selectedIndex;

	// For every articulatory feature in the table, match it with the cells.
	$container.find("table.articulatory td").each(function () {
		$td = $(this);
		feature = $td.text();
		activeIndex = $.inArray(feature, activeArticulatoryFeatures);
		selectedIndex = $.inArray(feature, selectedArticulatoryFeatures);
		matchFeature2CV($td, activeIndex, selectedIndex);
	});

	// For every binary feature in the table, match it with the cells.
	$container.find("table.binary tbody tr").each(function () {
		var $tr = $(this);
		var featureName = $tr.find("td.name").text();

		$td = $tr.find("td.minus");
		if ($td.length === 1) {
			feature = '-' + featureName;
			activeIndex = $.inArray(feature, activeBinaryFeatures);
			selectedIndex = $.inArray(feature, selectedBinaryFeatures);
			matchFeature2CV($td, activeIndex, selectedIndex);
		}

		$td = $tr.find("td.plus");
		if ($td.length === 1) {
			feature = '+' + featureName;
			activeIndex = $.inArray(feature, activeBinaryFeatures);
			selectedIndex = $.inArray(feature, selectedBinaryFeatures);
			matchFeature2CV($td, activeIndex, selectedIndex);
		}
	})

	// For every hierarchical feature in the table, match it with the cells.
	$container.find("table.hierarchical tbody tr").each(function () {
		var $tr = $(this);
		$td = $tr.children("td.name");
		if ($td.length === 1) {
			var featureName = $td.text();

			$td = $tr.children("td.minus");
			if ($td.length === 1) {
				feature = '-' + featureName;
				activeIndex = $.inArray(feature, activeHierarchicalFeatures);
				selectedIndex = $.inArray(feature, selectedHierarchicalFeatures);
				matchFeature2CV($td, activeIndex, selectedIndex);
			}

			$td = $tr.children("td.plus");
			if ($td.length === 1) {
				feature = '+' + featureName;
				activeIndex = $.inArray(feature, activeHierarchicalFeatures);
				selectedIndex = $.inArray(feature, selectedHierarchicalFeatures);
				matchFeature2CV($td, activeIndex, selectedIndex);
			}
		}
		else {
			var $div = $tr.children("td.univalent").children("div");
			feature = $div.text();
			activeIndex = $.inArray(feature, activeHierarchicalFeatures);
			selectedIndex = $.inArray(feature, selectedHierarchicalFeatures);
			matchFeature2CV($div, activeIndex, selectedIndex);
		}
	});
}

function matchFeature2CV($td, activeIndex, selectedIndex) {
	if (activeIndex >= 0 && selectedIndex >= 0)
		$td.addClass("matched");
	else if (activeIndex >= 0)
		$td.addClass("active");
	else if (selectedIndex >= 0)
		$td.addClass("selected");
	else
		$td.removeClass("matched");
}

// When the mouse pointer moves away from a cell in the chart,
// all features in the tables become unmarked.
function unmatchFeaturesCV($container) {
	$container.find("table.features .matched").removeClass("matched")
		.end().find("table.features .active").removeClass("active")
		.end().find("table.features .selected").removeClass("selected");
}

// Utility functions ---------------------------------------------------------

// Convert list item to string.
function textFromListItem(item) {
	// Internet Explorer 7 returns a trailing space in text of li elements.
	return $.trim($(item).text());
}

// Convert articulatory feature in the table to string.
function textFromArticulatoryFeature(element) {
	return $(element).text();
}

// Convert binary feature in the table to string consisting of value and name.
function textFromBinaryFeature(element) {
	var $feature = $(element);
	var text = $feature.text();
	if ($feature.hasClass("plus") || $feature.hasClass("minus"))
		text += $feature.parent().find("td.name").text();
	else
		text = '+' + text; // Univalent feature.
	return text;
}

// Convert hierarchical feature in the table to string.
function textFromHierarchicalFeature(element) {
	var $feature = $(element);
	var text = $feature.text();
	if ($feature.hasClass("plus") || $feature.hasClass("minus"))
		text += $feature.parent().find("td.name").text();
	return text;
}

// =============================================================================
// Table of counts in Distribution Chart view.

// Execute when the DOM is fully loaded.
function readyDistribution() {
	$("table.distribution").each(function () {
		var $table = $(this);
		$table.addClass("interactive");
		// To reverse the background color of zero/non-zero cells,
		// click at the lower-right of the upper-left cell.
		if ($table.find("td.zero").length !== 0)
			$table.find("thead tr:first th:first")
				.append('<div><span class="zero">0</span></div>');
		// To reduce or restore a heading, click the cell.
		$table.find("th.Phonetic")
			.wrapInner("<ins></ins>") // Wrap heading text in ins,
			.append("<del>…</del>"); // Insert U+2026 horizontal ellipsis in del.
		// To collapse or expand a general row or column,
		// click the individual cell corresponding to the row or column group heading.
		if ($table.find("tr.general").length !== 0)
			readyGeneralizedDistribution($table);
		$table.click(clickChartDistribution);
	});
}

// Insert the following symbols in empty cells corresponding to group headings.
// To collapse a column group, click U+25C2 black left-pointing small triangle.
// To expand a column group, click U+25B8 black right-pointing small triange.
// To collapse a row group, click U+25B4 black up-pointing small triangle.
// To expand a row group, click U+25BE black down-pointing small triangle.

function readyGeneralizedDistribution($table) {
	$table.addClass("generalized");
	
	// Upper-left cell.
	$table.find("thead tr:first th:first-child").addClass("upperLeft");

	// General column groups, if any.
	var $colgroups = $table.find("colgroup");
	var $thCols = $table.find("thead tr.individual th");
	var dataCellsByRow = new Array();
	$table.find("tbody tr").each(function (i) {
		dataCellsByRow[i] = $(this).find("td");
	});
	var iCol = 0;
	$table.find("thead tr.general th.Phonetic").each(function (i) {
		i++;
		var colgroupClass = "colgroup" + i;
		var $thColgroup = $(this);
		$thColgroup.addClass(colgroupClass);
		$colgroups.eq(i).addClass(colgroupClass);

		var colspan = $thColgroup.attr("colspan");		
		var colspan = (colspan.length === 0 ? 1 : parseInt(colspan));

		// Column headings for the group in the individual heading row.
		// Empty heading cell corresponding to the column group.
		if (colspan !== 1)
			$thCols.eq(iCol).addClass(colgroupClass).html("◂"); // left-pointing
		// Individual heading cells.
		for (var k = 1; k !== colspan; k++)
			$thCols.eq(iCol + k).addClass(colgroupClass).addClass("individual");

		// Data cells for the group.
		for (var j = 0; j !== dataCellsByRow.length; j++) {
			var $td = dataCellsByRow[j];
			for (var k = 0; k !== colspan; k++) {
				$td.eq(iCol + k).addClass(colgroupClass);
				//if (k !== 0)
				//	$td.eq(iCol + k).addClass("individual");
			}
		}

		iCol += colspan;
	});

	// General row groups, if any.
	// Empty heading cells corresponding to row groups.
	$table.find("tbody tr.general th").not("th.Phonetic").each(function () {
		var $th = $(this);
		if ($th.closest("tbody").find("tr").length !== 1)
			$th.addClass("individual").html("▴"); // up-pointing
	});
}

function clickChartDistribution(event) {
	var $target = $(event.target);
	if ($target.closest("span").length !== 0) {
		$(this).toggleClass("nonZero");
	}
	else {	
		var $th = $target.closest("th");
		if ($th.length !== 0) {
			if ($th.hasClass("upperLeft"))
				clickUpperLeftDistribution($th);
			else if ($th.closest("thead").length !== 0)
				clickColumnHeadingDistribution($th);
			else
				clickRowHeadingDistribution($th);
		}
	}
}

// To temporarily collapse or expand all general rows and columns,
// click the upper left of the table.
function clickUpperLeftDistribution($thUpperLeft) {
	var $table = $thUpperLeft.closest("table");
	if ($table.hasClass("generalized")) {
		$table.toggleClass("collapsed");

		// Make the first column group consistent with heading rows and columns.
		var colspan = ($table.hasClass("collapsed") ? 1 : $table.find("tbody:first tr:first th").length);
		var rowspan = ($table.hasClass("collapsed") ? 1 : $table.find("thead tr").length);
		var $colgroups = $table.find("colgroup");
		colgroupChildren($colgroups.eq(0), colspan);
		$thUpperLeft.attr("colspan", colspan);
		$thUpperLeft.attr("rowspan", rowspan);
		
		// Make general column headings, if any, consistent with visible cells.
		$table.find("thead tr.general th.Phonetic").each(function (i) {
			i++;
			var $colgroup = $colgroups.eq(i);
			var colgroupClass = $colgroup.attr("class");
			var colspan = 1;
			if (!($table.hasClass("collapsed")))
				colspan += $table.find("thead tr.individual th.individual." + colgroupClass).not(".collapsed").length;
			colgroupChildren($colgroup, colspan);
			$(this).attr("colspan", colspan);
		});
		
		// Make general row headings, if any, consistent with visible rows.
		$table.find("tbody tr.general th.Phonetic").each(function () {
			var $thPhonetic = $(this);
			var $tbody = $thPhonetic.closest("tbody");
			var rowspan = ($table.hasClass("collapsed") || $tbody.hasClass("collapsed") ? 1 : $tbody.find("tr").length);
			$thPhonetic.attr("rowspan", rowspan);
		});
	}
	else
		// Temporarily reduce all headings.
		$table.toggleClass("reduced");
}

function colgroupChildren($colgroup, n) {
	var $cols = $colgroup.find("col");
	var nInitial = $cols.length;
	if (n < nInitial) {
		for (var i = n; i !== nInitial; i++)
			$cols.eq(i).remove();
	}
	else if (n > nInitial) {
		for (var i = nInitial; i !== n; i++)
			$colgroup.append("<col></col>");
	}
}

function clickColumnHeadingDistribution($th) {
	if ($th.hasClass("Phonetic"))
		$th.toggleClass("reduced");
	else if ($th.attr("class").length !== 0) {
		// Toggle the class, and then change the colgroup, colspan, and heading.
		var colgroupClass = $th.attr("class");
		var $table = $th.closest("table");
		$table.find(".individual." + colgroupClass).toggleClass("collapsed");
		var colspan = $table.find("thead tr.individual th." + colgroupClass).not(".collapsed").length;

		var $colgroup = $table.find("colgroup." + colgroupClass);
		colgroupChildren($colgroup, colspan);

		$table.find("thead tr.general th." + colgroupClass).attr("colspan", colspan);

		$th.html($th.html() === "◂" ? "▸" : "◂"); // right-pointing : left-pointing
	}
}

function clickRowHeadingDistribution($th) {
	if ($th.hasClass("Phonetic"))
		$th.toggleClass("reduced");
	else if ($th.attr("class").length !== 0) {
		// Toggle the class, and then change the rowspan and heading.
		var $tbody = $th.closest("tbody");
		$tbody.toggleClass("collapsed");
		var rowspan = ($tbody.hasClass("collapsed") ? 1 : $tbody.find("tr").length);
		$th.closest("tr").find("th.Phonetic").attr("rowspan", rowspan);
		$th.html($th.html() === "▴" ? "▾" : "▴"); // down-pointing : up-pointing
	}
}