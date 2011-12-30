// phonology.js 2011-10-18
// Interactive behavior for XHTML files exported from Phonology Assistant.
// http://www.sil.org/computing/pa/index.htm

// Requires jQuery 1.4 or later. http://jquery.com/
// Requires jQuery UI widget base class. http://jqueryui.com

// JSLint defines a professional subset of JavaScript. http://jslint.com
/*global jQuery: false */

// Here is the convention for quote marks enclosing Javascript strings in this file:
// single quotes: all selectors, because they sometimes include quoted attribute values
// double quotes: everywhere else, for example, hasClass("...")

// Unicode values of characters which are mentioned in comments:
// U+002B plus sign
// U+002D hyphen-minus
// U+0030 digit zero
// U+00A0 no-break space (decimal code is 160)
// U+00B1 plus-minus sign
// U+00B7 middle dot
// U+2013 en dash
// U+2022 bullet
// U+2026 horizontal ellipsis
// U+2192 rightwards arrow
// U+25B2 black up-pointing triangle
// U+25B4 black up-pointing small triangle
// U+25B6 black right-pointing triangle
// U+25B8 black right-pointing small triangle
// U+25BC black down-pointing triangle
// U+25BE black down-pointing small triangle
// U+25C0 black left-pointing triangle

// In earlier versions of XHTML files, a cell containing one no-break space represented an empty field.
// In current versions of XHTML files, an empty cell represents an empty field, however:
// * If an empty cell has an explicit start and end tag, the text is always empty string.
// * If an empty cell has a self-closing tag, which is what Phonology Assistant exports:
//   * If the XHTML file is served as XML or has .xhtml extension, the text is empty string.
//   * If the XHTML file is served as HTML or has .html extension:
//     * If the file has no line breaks, the text is empty string.
//     * If the file has line breaks:
//       * In modern browsers which follow the standard, including IE9, the text is the line break.
//       * In IE6, IE7, IE8, the text is empty string.

// For more information, see page 137-139 of JavaScript Patterns.
// The following function is defined in ECMAScript 5
// and implemented in Firefox 4.
if (typeof Function.prototype.bind === "undefined") {
	Function.prototype.bind = function (thisArg) {
		"use strict"; // Firefox 4
		var fn = this,
			slice = Array.prototype.slice,
			args = slice.call(arguments, 1);

		return function () {
			return fn.apply(thisArg, args.concat(slice.call(arguments)));
		};
	};
}

// Execute when the DOM is fully loaded.
// The following is related to $(document).ready(...) but even better because:
// * It avoids an explicit global reference to document.
// * It ensures that $(...) is a shortcut for jQuery(...) within the function.

jQuery(function ($) {
	"use strict"; // Firefox 4
	/*jslint browser: true */ // setTimeout
	/*jslint nomen: true */ // jQuery UI: preceding underscore for private methods
	/*jslint devel: false */ // console

	// Utility methods ---------------------------------------------------------

	var browserIE7 = function () {
			// Return whether the browser version is IE7, because of its limitations in CSS:
			// * lack of :before and content for transcriptions
			// * limited support for collapsing rowgroups and colgroups for distribution charts
			// The link element has a class attribute and is in a conditional comment.
			return !!$('html head link.IE7[rel="stylesheet"]').length;
		},

		// Return the text of a list item (DOM).
		textFromListItem = function (li) {
			// IE 7 returns a trailing space in text of li elements.
			return $.trim($(li).text());
		},

		// Return the text of a table cell (DOM).
		textFromCell = function (cell) {
			return $(cell).text();
		},

		// Return whether all values in the subset (array) are in any of the sets (arrays).
		isSubsetOf = function (subset) {
			var length = subset.length,
				lengthArguments = arguments.length,
				i,
				iSet;

			for (i = 0; i < length; i += 1) {
				// Skip the subset argument.
				for (iSet = 1; iSet < lengthArguments; iSet += 1) {
					if ($.inArray(subset[i], arguments[iSet]) !== -1) {
						break;
					}
				}
				if (iSet >= lengthArguments) {
					return false;
				}
			}
			return true;
		},

		// Ignoring one value identified by its index in the subset,
		// return whether all the other values in the subset (array) are in the set (array).
		isSubsetOfIgnoring1 = function (subset, set, index) {
			var length = subset.length,
				i;

			for (i = 0; i < length; i += 1) {
				if (i !== index) {
					if ($.inArray(subset[i], set) === -1) {
						return false;
					}
				}
			}
			return true;
		},

		// Append the items in one or more arrays to the first array argument.
		// Unlike array.push which updates array, but appends entire arrays as items.
		// Unlike array.concat which appends items of arrays, but returns a new array.
		append = function (array0) {
			// Skip the array0 argument.
			$.each(Array.prototype.slice.call(arguments, 1), function () {
				var array = this,
					length = array.length,
					i;

				for (i = 0; i < length; i += 1) {
					array0.push(array[i]);
				}
			});
			return array0;
		},

		// Delete properties of object.
		emptyObject = function (object) {
			$.each(object, function (key) {
				delete object[key];
			});
			return object;
		};

	// Section of a phonology report ===========================================
	//	
	// Each section consists of a heading (which is the first child),
	// one or more tables, optional paragraphs, and optional child sections.
	// HTML5: section element
	// HTML4: div element whose class attribute contains section

	$.widget("phonology.section", {
		_create: function () {
			var eventNamespace = "." + this.widgetName,
				self = this;

			// Bind event handler.
			this.element.children(':first-child:header')
				.bind("click" + eventNamespace, function (event) {
					self._click(event);
				})
				.addClass("interactive");
		},

		// To collapse/expand a division, click its heading.
		_click: function () {
			this.element.toggleClass("collapsed");
		}
	});

	// Data or Search view =====================================================
	//
	// Sort by columns.
	// Collapse/expand groups.

	$.widget("phonology.tableList", {
		_create: function () {
			var $element = this.element,
				$ulTranscriptions = $element.find('td > ul.transcription'),
				eventNamespace = "." + this.widgetName,
				self = this;

			if ($ulTranscriptions.length) {
				this._createTranscriptions($ulTranscriptions);
			}

			// You can sort any column which contains data,
			// except when there is one minimal pair per group
			// because it might contain both subheading and data rows.
			if (!$element.has('tbody.group th.pair').length) {
				this._createSorting();
			}

			// Bind event handler for the table.
			// It has sortable columns or groups, or both.
			$element
				.bind("click" + eventNamespace, function (event) {
					self._click(event);
				})
				.addClass("interactive");
		},

		// Insert non-semantic markup and content which is intentionally omitted from XHTML files.
		_createTranscriptions: function ($ulTranscriptions) {
			// If a data cell contains a list of transcriptions,
			// wrap all children in a div for the phonology.css file,
			// because the data cell itself cannot have relative position.
			$ulTranscriptions.parent().wrapInner('<div class="transcription"></div>');

			if (browserIE7()) {
				// Because IE 7 does not support the :before pseudo-element,
				// insert content to separate del and ins elements.
				$ulTranscriptions.find('li.change del + ins').before(' \u25B8 '); // black right-pointing small triangle
			}
		},

		// Initialize sorting data for column heading cells to avoid:
		// * repeating data conversions in event processing functions
		//   (for example, sorting index, especially for the Phonetic field)
		// * storing state information in class attributes,
		//   unless it directly affects the visual appearance (for example, the sort field)
		//
		// $(td).data("sortingIndex", index) // set number value
		// $(td).data("sortingIndex"); // get number value
		// $(th).data("sorting", {sortingSelector: '.Gloss', ...}) // set object
		// $(th).data("sorting").sortingSelector // get object property
		_createSorting: function () {
			var $element = this.element,
				eventNamespace = "." + this.widgetName,
				self = this;

			this.$sortingDetails = $element.parent().find('table.details tr.sorting_field td');
			this.$thsSortable = $element.find('thead th:not(.group)').each(function () {
				var $th = $(this),
					$span = $th.find('span'),
					fieldName = $span.length ? $span.text() : $th.text(),
					data = {
						// The HTML and CSS class for a field consists of alphanumeric characters only.
						// For example, omit space and period.
						// \W is the opposite of \w, which is equivalent to [0-9A-Z_a-z].
						// Therefore, [\W_] is equivalent to [^0-9A-Za-z],
						// but JSLint does not consider it an insecure use of [^...].
						sortingSelector: '.' + fieldName.replace(/[\W_]/g, ""),

						// Is this the field by which the list is grouped?
						groupField: $th.hasClass("sorting_field") && !!$th.siblings('th.group').length,

						// Is this the Phonetic or (someday) Phonemic field in Search view?
						searchField: $th.attr("colspan") === "3"
					},
					$ins = $th.find('ins'),
					hasSortingOptions = $th.hasClass("sorting_options");

				if (data.searchField) {
					data.sortingSelector += '.item';
				}
				if ($ins.length) {
					self._indicator(data, $ins.text()); // initialize descending and sortingOption
				} else {
					// The first time you sort by any column other than the primary sort field,
					// assume that the direction is ascending and insert the visual indicator.
					data.descending = false;
					if (hasSortingOptions) {
						data.sortingOption = self.sortingOptions[0]; // assume as the default
					}
					if (!$span.length) {
						$th.wrapInner('<span></span>');
					}
					$th.append(' <ins></ins>').find('ins').text(self._indicator(data));
				}
				if (hasSortingOptions) {
					data.sortingSelector += (' > ul.sorting_order > li');

					// TO DO: encapsulate
					data.hasIndex = !!$element.find(data.sortingSelector).each(function () {
						var $li = $(this);

						$li.data("sortingIndex", parseInt($li.text(), 10) - 1); // TO DO: descending?
					}).length;

					// To make IE 7 display the drop-down menu on the next line,
					// wrap the contents in an explicit block element.
					// Must have inserted <span>...</span><ins>...</ins> before this!
					$th.wrapInner('<div></div>').append(self._$ulSortOptions())
						// Bind event handlers for sorting options.
						.bind("mouseenter" + eventNamespace, function (event) {
							self._mouseenterSortOptions(event);
						})
						.bind("mouseleave" + eventNamespace, function (event) {
							self._mouseleaveSortOptions(event);
						});
				}

				$th.data("sorting", data);
				//console.dir(data);
			}).addClass("sortable");
		},

		// event handlers ------------------------------------------------------

		// Click in a list.
		_click: function (event) {
			var $target = $(event.target),
				$cell = $target.closest('th, td');

			if ($cell.is('th')) {
				if ($cell.hasClass("group")) {
					// To collapse/expand all groups, click at the upper left.
					this.element.toggleClass("collapsed");
				} else if ($cell.hasClass("count")) {
					// To collapse/expand a group, click at the left of its heading row.
					$cell.closest('tbody').toggleClass("collapsed");
				} else if ($cell.hasClass("sortable")) {
					// To sort by a column, click its heading cell or a phonetic option.
					this._clickSortableHeading($target);
				}
			}
		},

		// When the mouse pointer moves over a Phonetic or (someday) Phonemic heading cell,
		// a drop-down menu of sorting options appears.
		_mouseenterSortOptions: function (event) {
			this._addSortingOptionClasses($(event.currentTarget));
		},

		// When the mouse pointer moves out of the heading cell,
		// sorting options disappear.
		_mouseleaveSortOptions: function (event) {
			this._removeSortingOptionClasses($(event.currentTarget));
		},

		// private implementation ----------------------------------------------

		_addSortingOptionClasses: function ($th) {
			var sortingOption = $th.data("sorting").sortingOption;

			$th.addClass("active").find('li').filter(function () {
				return $(this).data("sorting").sortingOption === sortingOption;
			}).addClass("initial");
		},

		_removeSortingOptionClasses: function ($th) {
			$th.removeClass("active").find('li').removeClass("initial");
		},

		// Return a list for a drop-down menu of sorting options.
		_$ulSortOptions: function () {
			var $ul = $('<ul></ul>'),
				sortingOption_label = this.sortingOption_label,
				sortingOptions = this.sortingOptions,
				length = sortingOptions.length,
				i,
				sortingOption;

			for (i = 0; i < length; i += 1) {
				sortingOption = sortingOptions[i];
				$ul.append($('<li>' + sortingOption_label[sortingOption] + '</li>').data("sorting", {sortingOption: sortingOption}));
			}
			return $ul;
		},

		sortingOptions: ["place_or_backness", "manner_or_height"],
		sortingOption_label: {
			"place_or_backness": "place or backness",
			"manner_or_height": "manner or height"
		},

		sortIndicators: {
			"\u25B2": { // black up-pointing triangle
				sortingOption: "manner_or_height",
				descending: false
			},
			"\u25BC": { // black down-pointing triangle
				sortingOption: "manner_or_height",
				descending: true
			},
			"\u25C0": { // black left-pointing triangle
				sortingOption: "place_or_backness",
				descending: false
			},
			"\u25B6": { // black right-pointing triangle
				sortingOption: "place_or_backness",
				descending: true
			},
			"\u25B4": { // black up-pointing small triangle
				descending: false
			},
			"\u25BE": { // black down-pointing small triangle
				descending: true
			}
		},

		_indicator: function (data, sortIndicator) {
			var object;

			if (typeof sortIndicator === "string") {
				// Initialize data for the column heading of the sort field
				// from a character in the ins element exported by Phonology Assistant.
				object = this.sortIndicators[sortIndicator];
				if (object) {
					data.descending = object.descending;
					if (object.sortingOption) {
						data.sortingOption = object.sortingOption;
					}
				}
			} else {
				// Return a character for the ins element in the heading of the sort field.
				sortIndicator = undefined;
				$.each(this.sortIndicators, function (key, value) {
					if (value.sortingOption === data.sortingOption && value.descending === data.descending) {
						sortIndicator = key;
						return false;
					}
				});
				return sortIndicator;
			}
		},

		// To compare field values, change some occurrences of white space.
		emptyValues: ["\n", "\r", "\r\n", "\u00A0"], // newline, return, return-newline, no-break space
		_sortableValue: function (value) {
			var length = value.length;

			if ($.inArray(value, this.emptyValues) !== -1) {
				return "";
			}
			if (length > 1) {
				if (value.charCodeAt(0) === 160) {
					// A no-break space at the beginning replaced a space.
					value = " " + value.slice(1); // length is unchanged
				}
				if (value.charCodeAt(length - 1) === 160) {
					// A no-break space at end replaced a space.
					value = value.slice(0, -1) + " ";
				}
			}
			return value;
		},

		_getIndex: function (elements, sortingSelector, method) {
			var length = elements.length,
				i,
				$child;

			//console.log("tableList._getIndex", elements.length, sortingSelector, method);
			for (i = 0; i < length; i += 1) {
				$child = $(elements[i]);
				$child.data("sortingIndex", $child[method](sortingSelector).data("sortingIndex"));
			}
		},

		_setIndex: function (elements, sortingSelector, method, descending) {
			/*jslint plusplus: true */
			var length = elements.length,
				index = descending ? length : 0,
				i;

			//console.log("tableList._setIndex", elements.length, sortingSelector, descending);
			for (i = 0; i < length; i += 1) {
				$(elements[i])[method](sortingSelector).data("sortingIndex", descending ? --index : index++);
			}
		},

		// Within one or more parent elements, sort children according to arguments.
		_sortChildren: function ($parents, childrenSelector, sortingSelector,
				sortingByGrandchildren, hasIndex, descending) {
			var method = sortingByGrandchildren ? "children" : "find",
				self = this;

			//console.log("tableList._sort", $parents.get(0).nodeName, childrenSelector, sortingSelector, sortingByGrandchildren, hasIndex, descending);
			$parents.each(function () {
				var $parent = $(this),
					children = $parent.children(childrenSelector).get(); // array of DOM objects

				if (hasIndex) {
					self._getIndex(children, sortingSelector, method);
				}
				children.sort(function (childA, childB) {
					var valueA,
						valueB;

					if (hasIndex) {
						valueA = $(childA).data("sortingIndex");
						valueB = $(childB).data("sortingIndex");
						return descending ? valueB - valueA : valueA - valueB;
					}

					valueA = self._sortableValue($(childA)[method](sortingSelector).text());
					valueB = self._sortableValue($(childB)[method](sortingSelector).text());
					return descending ? valueB.localeCompare(valueA) : valueA.localeCompare(valueB);
				});
				if (!hasIndex) {
					self._setIndex(children, sortingSelector, method, descending);
				}
				$parent.append(children); // replace DOM objects in sorted order
			});
		},

		// Sort according to data from a column heading.
		// Differences between views in the Phonology Assistant program
		// and interactive tables in XHTML files exported from Phonology Assistant:
		// * If the list is grouped, a view regroups according to the column,
		//   but a table resorts within the same groups.
		// * If the list has minimal groups, both resort within the same groups,
		//   but if one minimal pair per group, a table has no sortable columns.
		_sort: function (data) {
			var $element = this.element, // table
				sortingSelector = data.sortingSelector,
				hasIndex = data.hasIndex,
				descending = data.descending;

			if (data.sortingOption) {
				sortingSelector += ('.' + data.sortingOption);
			}
			if (data.groupField) {
				// The field in the heading row of groups.
				this._sortChildren($element, 'tbody', 'tr.heading ' + sortingSelector,
					false, hasIndex, descending);
			}
			if (data.searchField || !data.groupField) {
				// For Phonetic or (someday) Phonemic in Search view, sort rows
				// even if grouped, because they might not contain identical values.
				this._sortChildren($element.find('tbody'), 'tr:not(.heading)', sortingSelector,
					!data.sortingOption, hasIndex, descending);
			}
			data.hasIndex = true;
		},

		// To sort by a column, click its heading cell or a sort option.
		_clickSortableHeading: function ($target) {
			var $th = $target.closest('th'),
				data = $th.data("sorting"),
				sortField = $th.hasClass("sorting_field"), // click the active column heading
				reverse = sortField,
				sortingOptionSelected;

			// Phonetic sort option.
			if ($target.is('li')) {
				sortingOptionSelected = $target.data("sorting").sortingOption;
				if (reverse) {
					// Only if this is the active sort field:
					// * Reverse the direction if the sort option remains the same.
					// * Keep the direction the same if the sort option changes.
					reverse = (data.sortingOption === sortingOptionSelected);
				}
				data.sortingOption = sortingOptionSelected;
			}
			// The following condition is independent of the previous condition,
			// because you might click the heading instead of the list.
			if ($th.hasClass("sorting_options")) {
				this._removeSortingOptionClasses($th);
			}

			// Important: Must resort in the opposite direction;
			// cannot just call the reverse() method,
			// because it does not preserve stable order of records
			// with identical values of the sort field.
			if (reverse) {
				data.descending = !data.descending;
			}

			this._sort(data);

			if (!sortField) {
				this.$thsSortable.removeClass("sorting_field");
				$th.addClass("sorting_field");
			}
			$th.find('ins').text(this._indicator(data));

			if (this.$sortingDetails.length) {
				this._updateDetails($th);
			}
		},

		// Update a row in the table of details.		
		_updateDetails: function ($th) {
			var details = $th.find('span').text(), // name of sort field
				data = $th.data("sorting"),
				label = this.sortingOption_label[data.sortingOption];

			if (typeof label === "string") {
				details += (", " + label);
			}
			if (data.descending) {
				details += ", descending";
			}
			this.$sortingDetails.text(details);
		}
	});

	// Consonant Chart or Vowel Chart view =====================================
	//
	// Display units which have sets of features.
	// Display features for units or pairs of units.

	// =========================================================================

	// Division which contains CV charts and tables of features.
	$.widget("phonology.divFeaturesCV", {
		_create: function () {
			var $element = this.element;

			this.tableSelected = undefined;
			this.tdSelected = undefined;
			this.$tablesFeaturesShared = $element.has('div.CV2').children('table.features');

			this.$tableFeaturesDistinctive = $element.children('table.features.distinctive');
			this.key_segments = {};

			$element.find('table.features').tableFeatures();
		},

		// private implementation ----------------------------------------------

		_matchFeaturesShared: function (dataActive, dataSelected) {
			this.$tablesFeaturesShared.tableFeatures("toggleClassFeatures", dataActive, dataSelected);
		},

		// public interface ----------------------------------------------------

		// The mouse pointer moves into or out of a segment in a CV chart.
		mediateSegmentActive: function (tableCV, td) {
			var tableSelected = this.tableSelected,
				dataSelected = tableSelected && $.data(this.tdSelected),
				dataActive = td && $.data(td);

			if (td) {
				this._matchFeaturesShared(dataActive, dataSelected);
				tableCV.matchFeatures(dataActive, dataSelected);
				if (tableSelected && tableSelected !== tableCV) {
					tableSelected.matchFeatures(dataActive, dataSelected);
				}
			} else {
				this._matchFeaturesShared(dataSelected);
				if (tableSelected && tableSelected !== tableCV) {
					tableCV.matchFeatures();
					tableSelected.matchFeatures(dataSelected);
				} else {
					tableCV.matchFeatures(dataSelected);
				}
			}
		},

		// Select or clear a segment in a CV chart.
		mediateSegmentSelected: function (tableCV, td) {
			var tableSelectedPrev = this.tableSelected,
				dataSelected = td && $.data(td);

			this._matchFeaturesShared(dataSelected);
			tableCV.matchFeatures(dataSelected);
			if (tableSelectedPrev && tableSelectedPrev !== tableCV) {
				tableSelectedPrev.matchFeatures();
				tableSelectedPrev.clearSegmentSelected();
			}
			this.tableSelected = td && tableCV;
			this.tdSelected = td;
		},

		hasSegmentSelected: function () {
			return !!this.tdSelected;
		},

		getSegments: function (key) {
			return this.key_segments[key];
		},

		pushSegment: function (segment) {
			var features = $.data(segment, "distinctive"),
				key = $.isArray(features) && this.$tableFeaturesDistinctive.tableFeatures("keyFeaturesChanges", features),
				key_segments = this.key_segments,
				value = key && key_segments[key];

			if (value) {
				value.push(segment);
			} else {
				key_segments[key] = [segment];
			}
		}
	});

	// =========================================================================

	// Base class for widgets which are in a counteractive relationship.
	// This widget is active (enabled), they are inactive (disabled), and vice versa.
	$.widget("phonology.counteractive", {
		// Override method in $.ui.widget base class called by enable and disable methods.
		_setOption: function (key, value) {
			if (key === "disabled") {
				// Instead of the default ui-state-disabled class when disabled,
				// a derived widget has interactive class when it is enabled.
				this.options[key] = value;
				this.element.toggleClass("interactive", !value);
				return this;
			}
			// For other keys, call the default implementation.
			return $.Widget.prototype._setOption.call(this, key, value);
		},

		// protected interface -------------------------------------------------

		// Derived widget must call this function at the end of its _create method.
		_initCounteractive: function ($widgetsCounteractive, eventMap, $descendants) {
			var eventNamespace = "." + this.widgetName, // of derived class
				self = this;

			this.$widgetsCounteractive = $widgetsCounteractive;

			// Bind event handlers to widget.
			this.element
				// Triggered events call methods in $.ui.widget base class.
				.bind("disable" + eventNamespace, function () {
					self.disable();
				})
				.bind("enable" + eventNamespace, function () {
					self.enable();
				})

				// For information about how the following base event handlers
				// are template methods, see pages 325-330 in Design Patterns.
				// Because they encapsulate the if (!self.options.disabled) statement,
				// derived event handlers can assume the widget is active (enabled).

				// Trigger events instead of call methods directly
				// to allow multiple derived classes (for example, tableFeatures, tableCV).
				.bind("mouseenter" + eventNamespace, function () {
					// When the mouse pointer moves into a widget,
					// unless it is disabled, disable counteractive widgets.
					if (!self.options.disabled) {
						self.$widgetsCounteractive.trigger("disable");
					}
				})
				.bind("mouseleave" + eventNamespace, function () {
					// When the mouse pointer moves out of a widget,
					// unless it is disabled or it is still active,
					// enable counteractive widgets.
					// Assumes a _stillActive method is defined in derived widget.
					if (!self.options.disabled && !self._stillActive()) {
						self.$widgetsCounteractive.trigger("enable");
					}
				})

				.bind("click" + eventNamespace, function (event) {
					// When there is a click in a widget,
					// unless it is disabled, call _click method in derived widget.
					if (!self.options.disabled) {
						self._click(event);
					}
				});

			// Bind event handlers to interactive descendants of widget.
			if (eventMap && $descendants) {
				$.each(eventMap, function (eventType, eventFunction) {
					$descendants.bind(eventType + eventNamespace, function (event) {
						if (!self.options.disabled) {
							eventFunction.call(self, event);
						}
					});
				});
			}

			this.enable();
		}
	});

	// =========================================================================

	// Tables of descriptive or distinctive features at the right of CV charts.
	$.widget("phonology.tableFeatures", $.phonology.counteractive, {
		_create: function () {
			var $element = this.element,
				$divFeaturesCV = $element.parentsUntil('body', 'div.CV_features').last(),
				$tablesCV = this.$tablesCV = $element.closest('div.CV_features').find('table.CV'),
				$ulsDistinctive = $element.nextUntil('table.features', 'ul.distinctive'),
				className = this.className = $element.attr('class').split(' ')[0],
				distinctive = this.distinctive = className === "distinctive",
				selectorNames = this.selectorNames = distinctive ? 'td.name' : 'td',
				selectorFeatures = this.selectorFeatures = distinctive ? 'td:not(.name)' : 'td',
				clickFeature = this._clickFeature,
				self = this,
				$tdsFeatures = this.$tdsFeatures = $element.find(selectorFeatures)
					.filter(function () {
						// Important: initializes data as a side-effect if function returns true.
						return self._initData($(this), clickFeature);
					});

			this.$thFeatures = $element.children('thead').find('th');
			this.divFeaturesCV = $divFeaturesCV.data("divFeaturesCV");
			this.featureNames = $.map($element.find(selectorNames), textFromCell);
			this.selectedFeatures = [];
			this.potentiallyRedundantVacuous = [];
			this.potentiallyRedundant = [];
			this.hasChanges = false;

			// Initialize data for main heading cell.
			this._initHeading($element.find('th'), this._clickMainHeading);

			if (distinctive) {
				this._createContradictions($ulsDistinctive.filter('ul.contradictions'));
				// _createDependencies must follow featureNames
				this._createDependencies($ulsDistinctive.filter('ul.dependencies'));
				// _createDependencies must precede _createChanges
				if ($element.hasClass("changes")) {
					this.hasChanges = true; // table has a colgroup of changes
					this._createChanges();
				}
			}
			this.callbackFeatures = this.selectedChanges ?
					this._callbackFeaturesChanges.bind(this) :
					this._callbackFeatures.bind(this);

			// This table is counteractive with related CV charts and their tables of features.
			// When it is active (enabled), they are inactive (disabled), and vice versa.
			this._initCounteractive($divFeaturesCV.find('table.features').not(this.element).add($tablesCV),
				// Bind event handlers to interactive descendants:
				{
					"mouseenter": this._mouseenterFeature,
					"mouseleave": this._mouseleaveFeature
				},
				$tdsFeatures.addClass("interactive"));
		},

		// Map a list of lists of features to an array of arrays of features,
		// each of which contains a contradictory set of feature values.
		// For example, [-son, +approx].
		_createContradictions: function ($ulContradictions) {
			var contradictions = this.contradictions = [];

			$ulContradictions.children('li').each(function () {
				var contradiction = $.map($(this).children('ul').children('li'), textFromListItem);

				contradictions.push({
					features: contradiction,
					title: ["contradiction:"].concat(contradiction).join(" ")
				});
			});
		},

		// Map a list of pairs of lists of features to an array of objects,
		// each of which contains if and then properties whose values are arrays of features.
		// For example, {if: [-son, +cont], then: [+delayed]}.
		_createDependencies: function ($ulDependencies) {
			var dependencies = this.dependencies = [];

			$ulDependencies.children('li').each(function () {
				var $li = $(this),
					dependencyIf = $.map($li.children('ul.if').children('li'), textFromListItem),
					dependencyThen = $.map($li.children('ul.then').children('li'), textFromListItem);

				dependencies.push({
					// "if" must be in quote marks because is is a reserved word in JavaScript
					"if": dependencyIf,
					then: dependencyThen,
					title: ["dependency:"].concat(dependencyIf, [">"], dependencyThen).join(" ")
				});
			});

			this.featureName_td0 = {};
			this._createUnspecifieds(dependencies, "then");
			this._createUnspecifieds(dependencies, "if"); // if must follow then
		},

		_createUnspecifieds: function (dependencies, key) {
			var featureNames = this.featureNames,
				featureName_td0 = this.featureName_td0;

			$.each(dependencies, function (i, dependency) {
				$.each(dependency[key], function (j, feature) {
					var featureName = feature.slice(1);

					if (feature.charAt(0) === '0' && $.inArray(featureName, featureNames) !== -1) {
						featureName_td0[featureName] = key;
					}
				});
			});
		},

		// Add a colgroup of changes at the right of a distinctive features table.
		_createChanges: function () {
			var $element = this.element,
				eventNamespace = "." + this.widgetName,
				$thead = $element.children('thead'),
				$th = $thead.find('th'),
				$thChanges = this.$thChanges = $th.clone(),
				selectorNames = this.selectorNames,
				featureName_td0 = this.featureName_td0,
				col0 = !$.isEmptyObject(featureName_td0),
				colspan = col0 ? 3 : 2,
				td0 = col0 ? document.createElement('td') : null,
				selectorFeatures = this.selectorFeatures,
				clickChange = this._clickChange,
				tdsChanges = [],
				tdsChanges0 = [],
				$tfoot = this.$tfoot = $('<tfoot><tr><td><div><span></span></div></td></tr></tfoot>'),
				self = this;

			$thead.before('<colgroup><col /><col />' + (col0 ? '<col />' : '') + '</colgroup>');
			this._initHeading($thChanges, this._clickChangesHeading);
			$thChanges.attr("colspan", colspan).find('span').text("changes").wrap('<div></div>');
			$th.after($thChanges);
			$element.children('tbody').children('tr').each(function () {
				var $tr = $(this),
					$tds = $tr.children(selectorFeatures).clone(),
					featureName,
					td,
					key;

				$tr.append($tds);
				$tds.each(function () {
					// Important: initializes data as a side-effect if function returns true.
					if (self._initData($(this), clickChange)) {
						tdsChanges.push(this);
					}
				});
				if (col0) {
					td = td0.cloneNode(false);
					$tr.append(td);
					featureName = $tr.children(selectorNames).text();
					key = featureName_td0[featureName];
					if (key) {
						td.appendChild(document.createTextNode("0"));
						tdsChanges0.push(td);
						if (key === "if") {
							// Important: cloned cells for changes must already be in the row.
							// Important: initializes data as a side-effect if function returns true.
							if (self._initData($(td), clickChange)) {
								tdsChanges.push(td);
							}
						}
						featureName_td0[featureName] = td;
					}
				}
			});
			this.$tdsChanges = $(tdsChanges).bind("mouseleave" + eventNamespace, function (event) {
				self._mouseleaveChange(event);
			}).addClass("change");
			this.$tdsChanges0 = $(tdsChanges0);
			this.selectedChanges = [];
			this.dependentFeaturesIf = {};
			this.dependentChangesIf = {};
			this.dependentChangesThen = {};
			this.objectChanges = {};
			this.segmentsMatched = [];
			this.segmentsChangedFrom = {};
			this.segmentsChangedTo = {};
			this.callbackChanged = this._callbackChanged.bind(this);
			this.callbackChangedAlone = this._callbackChangedAlone.bind(this);
			this.callbackUnchanged = this._callbackUnchanged.bind(this);
			this.$spanChanges = $tfoot.find('td').attr("colspan", 3 + colspan).find('span');
			$element.append($tfoot);
		},

		// protected interface -------------------------------------------------

		// When the mouse pointer moves out of this table, 
		// the counteractive base class needs to know whether a feature is still selected.
		_stillActive: function () {
			return !!this.selectedFeatures.length;
		},

		// event handlers ------------------------------------------------------

		// Click in a table of features.
		_click: function (event) {
			var $cell = $(event.target).closest('th, td'),
				data = this._data($cell);

			// If the target is (in) an interactive feature cell.
			if (data) {
				data.clickFunction.call(this, $cell);
				this._iterateSegmentsCV(true);
			}
		},

		// When the mouse pointer moves into an interactive feature cell,
		// match segments which have all selected and active features.
		_mouseenterFeature: function (event) {
			var $td = $(event.target);

			if (!$td.hasClass("selected")) {
				// If the cell is not already selected,
				// highlight segments which have active and selected features.
				this._iterateSegmentsCV(false, this._feature($td));
			}
		},

		// When the mouse pointer moves out of a feature cell,
		// match segments which have all selected features.
		_mouseleaveFeature: function (event) {
			var $td = $(event.target);

			$td.removeClass("click");
			if (!$td.hasClass("selected")) {
				// If the cell is not already selected,
				// highlight segments which have active and selected features.
				this._iterateSegmentsCV(false);
			}
		},

		// When the mouse pointer moves out of a change cell,
		// remove class which indicates whether it was clicked.
		_mouseleaveChange: function (event) {
			$(event.target).removeClass("click");
		},

		// private implementation called by event handlers ---------------------

		// Click to select or clear a feature.
		_clickFeature: function ($td) {
			var $element = this.element,
				selected;

			$td.addClass("click");
			$td.toggleClass("selected");
			this._toggleFeature(this._feature($td));
			selected = !!this.selectedFeatures.length;
			this.$thFeatures.toggleClass("selected", selected);
			if (this.hasChanges) {
				if (selected) {
					// While distinctive feature values are selected, changes are active.
					$element.addClass("changeable");
				} else {
					// When the last selected value is cleared, clear any selected changes.
					this._clickChangesHeading();
					$element.removeClass("changeable");
				}
				this._dependentChanges();
				this._displayChanges();
			}
		},

		// Click to select or clear a change.
		_clickChange: function ($td) {
			var $tdCounterpart; // adjacent cell which contains opposite value

			// While distinctive feature values are selected at the left of the table,
			// changes at the right are active.
			if (this.selectedFeatures.length && !$td.hasClass("then") && !$td.siblings('.then').length) {
				// Before this change becomes selected,
				// if the opposite value was selected, clear it.
				if (!$td.hasClass("selected")) {
					$tdCounterpart = $td.siblings('.change.selected');
					if ($tdCounterpart.length) {
						$tdCounterpart.removeClass("selected");
						this._toggleChange(this._feature($tdCounterpart));
					}
				}

				$td.addClass("click");
				$td.toggleClass("selected");
				this._toggleChange(this._feature($td));
				this.$thChanges.toggleClass("changed", !!this.selectedChanges.length);
				this.$tfoot.toggleClass("changed", !!this.selectedChanges.length);
				this._dependentChanges();
				this._displayChanges();
			}
		},

		// To clear all selected changes in the table, click at the right of its heading.
		_clickChangesHeading: function () {
			this.$tdsChanges.removeClass("selected");
			this.$thChanges.removeClass("changed");
			this.$tfoot.removeClass("changed");
			this._clearChanges();
			this._displayChanges();
		},

		// To clear all selected features and changes in the table, click at the left of its heading.
		_clickMainHeading: function () {
			this.$tdsFeatures.removeClass("selected");
			this.$thFeatures.removeClass("selected");
			this._clearFeatures();
			if (this.hasChanges) {
				this.element.removeClass("changeable");
				this._clickChangesHeading();
			}
		},

		// For any click to select or clear a feature or a change
		// while any changes are selected, evaluate dependent changes.
		_dependentChanges: function () {
			var selectedFeatures = this.selectedFeatures,
				selectedChanges = this.selectedChanges,
				dependentFeaturesIf = this.dependentFeaturesIf,
				dependentChangesIf = this.dependentChangesIf,
				dependentChangesThen = this.dependentChangesThen,
				dependentChangesThenArray = [],
				self = this;

			emptyObject(dependentFeaturesIf);
			emptyObject(dependentChangesIf);
			emptyObject(dependentChangesThen);
			$.each(this.dependencies, function () {
				var dependency = this,
					dependencyIf = dependency["if"],
					dependencyTitle = dependency.title,
					nIfChanges = 0,
					nIfFeatures = 0;

				$.each(dependencyIf, function (i, feature) {
					if ($.inArray(feature, selectedChanges) !== -1) {
						nIfChanges += 1;
					} else if ($.inArray(feature, selectedFeatures) !== -1) {
						nIfFeatures += 1;
					} else {
						return false; // break
					}
				});
				if (nIfChanges && (nIfChanges + nIfFeatures === dependencyIf.length)) {
					$.each(dependencyIf, function (i, feature) {
						if ($.inArray(feature, selectedChanges) !== -1) {
							dependentChangesIf[feature] = dependencyTitle;
						} else {
							dependentFeaturesIf[feature] = dependencyTitle;
						}
					});
					$.each(dependency.then, function (i, feature) {
						dependentChangesThen[feature] = dependencyTitle;
						dependentChangesThenArray.push(feature);
					});
					// Important: _giveFeedback must remove selected changes implied by dependencies.
				}
			});
			this._objectFeatures(this.objectChanges, this.selectedChanges, dependentChangesThenArray);
		},

		// Display the selected features and changes as a rule which people can copy from the page.
		_displayChanges: function () {
			var selectedFeatures = this.selectedFeatures,
				objectChanges = this.objectChanges;

			this.$spanChanges.text(selectedFeatures.length && !$.isEmptyObject(objectChanges) ?
					"[" + selectedFeatures.join(", ") + "] \u2192 [" + $.map(this.featureNames, function (featureName) {
						var value = objectChanges[featureName];

						return value ? value + featureName : null;
					}).join(", ") + "]" : "");
		},

		// In related CV charts, highlight segments:
		// * which have all selected and active features
		// * which have the selected changes, if any
		// * for which no corresponding segments in the language have the selected changes
		_iterateSegmentsCV: function (click, feature) {
			if (this.hasChanges) {
				this.segmentsMatched.length = 0;
				emptyObject(this.segmentsChangedFrom);
				emptyObject(this.segmentsChangedTo);
				if (click) {
					this._potentiallyRedundant(true);
				}
			}
			if (feature || this.selectedFeatures.length) {
				this.activeFeature = feature;
				this.$tablesCV.tableCV("toggleClassSegments", "matched", this.callbackFeatures, this.className);
			} else {
				this.$tablesCV.tableCV("toggleClassSegments", "matched");
			}
			if (this.hasChanges) {
				if (this.selectedChanges.length) {
					this.$tablesCV.tableCV("toggleClassSegments", "changed", this.callbackChanged);
					this.$tablesCV.tableCV("toggleClassSegments", "alone", this.callbackChangedAlone);
					this.$tablesCV.tableCV("toggleClassSegments", "unchanged", this.callbackUnchanged);
					this.$tablesCV.tableCV("toggleChangesSegments", this.segmentsChangedTo);
				} else {
					this.$tablesCV.tableCV("toggleClassSegments", "changed alone unchanged");
					this.$tablesCV.tableCV("toggleChangesSegments");
				}
				if (click) {
					this._giveFeedback();
				}
			}
		},

		// callbacks which this._iterateSegmentsCV provides to tableCV.toggleClassSegments

		// Return whether features (of a segment) match the selected and active features.
		_callbackFeatures: function (segment, features) {
			var selectedFeatures = this.selectedFeatures,
				activeFeature = this.activeFeature,
				potentiallyRedundantVacuous = this.potentiallyRedundantVacuous,
				lengthVacuous = potentiallyRedundantVacuous.length,
				potentiallyRedundant = this.potentiallyRedundant,
				length = potentiallyRedundant.length,
				i;

			for (i = 0; i < lengthVacuous; i += 1) {
				if (potentiallyRedundantVacuous[i] && isSubsetOfIgnoring1(selectedFeatures, features, i)) {
					if ($.inArray(selectedFeatures[i], features) === -1 && $.inArray(potentiallyRedundantVacuous[i], features) === -1) {
						potentiallyRedundantVacuous[i] = undefined;
					}
				}
			}
			for (i = 0; i < length; i += 1) {
				if (potentiallyRedundant[i] && isSubsetOfIgnoring1(selectedFeatures, features, i)) {
					potentiallyRedundant[i] = $.inArray(selectedFeatures[i], features) !== -1;
				}
			}

			if (activeFeature && $.inArray(activeFeature, features) === -1) {
				return false;
			}
			return isSubsetOf(selectedFeatures, features);
		},

		// Return whether features (of a segment) match the active and selected features and
		// if there are selected changes, determine the change-from and change-to segments.
		_callbackFeaturesChanges: function (segment, features) {
			var segments,
				literal,
				value,
				segmentsChangedFrom,
				segmentsChangedTo;

			if (this._callbackFeatures(segment, features)) {
				this.segmentsMatched.push(segment);
				if (this.selectedChanges.length) {
					segments = this.divFeaturesCV.getSegments(this.keyFeaturesChanges(features, this.objectChanges));
					if (segments) {
						segmentsChangedFrom = this.segmentsChangedFrom;
						literal = $.data(segment, "literal");
						value = segmentsChangedFrom[literal];
						if (!value) {
							value = segmentsChangedFrom[literal] = [];
						}
						append(value, segments);

						segmentsChangedTo = this.segmentsChangedTo;
						$.each(segments, function (i, segmentChangedTo) {
							var literalChangedTo = $.data(segmentChangedTo, "literal");

							value = segmentsChangedTo[literalChangedTo];
							if (!value) {
								value = segmentsChangedTo[literalChangedTo] = [];
							}
							value.push(segment);
						});
					}
				}
				return true;
			}
			return false;
		},

		_callbackChanged: function (segment) {
			return !!this.segmentsChangedTo[$.data(segment, "literal")];
		},

		_callbackChangedAlone: function (segment) {
			var value = this.segmentsChangedTo[$.data(segment, "literal")];

			return !!value && value.length === 1 && value[0] === segment;
		},

		_callbackUnchanged: function (segment) {
			return $.inArray(segment, this.segmentsMatched) !== -1 && !this.segmentsChangedFrom[$.data(segment, "literal")];
		},

		// data methods --------------------------------------------------------

		_toggleFeature: function (feature) {
			var selectedFeatures = this.selectedFeatures,
				index = $.inArray(feature, selectedFeatures);

			if (index !== -1) {
				selectedFeatures.splice(index, 1); // remove
			} else {
				selectedFeatures.push(feature); // add at the end
			}
		},

		_clearFeatures: function () {
			this.selectedFeatures.length = 0;
		},

		_toggleChange: function (feature) {
			var selectedChanges = this.selectedChanges,
				index = $.inArray(feature, selectedChanges);

			if (index !== -1) {
				selectedChanges.splice(index, 1); // remove
			} else {
				selectedChanges.push(feature); // add at the end
			}
		},

		_clearChanges: function () {
			this.selectedChanges.length = 0;
			emptyObject(this.dependentFeaturesIf);
			emptyObject(this.dependentChangesIf);
			emptyObject(this.dependentChangesThen);
			emptyObject(this.objectChanges);
		},

		// private implementation of feedback ----------------------------------

		_potentiallyRedundant: function () {
			var potentiallyRedundantVacuous = this.potentiallyRedundantVacuous,
				value_opposite = this.value_opposite,
				objectChanges = this.objectChanges,
				potentiallyRedundant = this.potentiallyRedundant,
				selectedFeatures = this.selectedFeatures,
				length = selectedFeatures.length,
				i;

			potentiallyRedundantVacuous.length = 0;
			$.each(selectedFeatures, function (i, feature) {
				var featureValue = feature.charAt(0),
					featureValueOpposite = value_opposite[featureValue],
					featureName = feature.slice(1),
					changeValue = objectChanges[featureName];

				potentiallyRedundantVacuous.push(changeValue && changeValue === featureValueOpposite ? featureValueOpposite + featureName : undefined);
			});

			potentiallyRedundant.length = 0;
			if (length > 1) {
				for (i = 0; i < length; i += 1) {
					potentiallyRedundant.push(true);
				}
			}
		},

		_giveFeedback: function () {
			var selectedFeatures = this.selectedFeatures,
				selectedChanges = this.selectedChanges,
				dependentFeaturesIf = this.dependentFeaturesIf,
				dependentChangesIf = this.dependentChangesIf,
				dependentChangesThen = this.dependentChangesThen,
				activeContradictions = {},
				potentiallyRedundantVacuous = this.potentiallyRedundantVacuous,
				potentiallyRedundant = this.potentiallyRedundant,
				activeRedundancies = {},
				self = this;

			if (selectedFeatures.length) {
				$.each(this.contradictions, function () {
					var features = this.features,
						title = this.title;

					if (isSubsetOf(features, selectedFeatures)) { // , selectedChanges, this.dependentChangesThen
						$.each(features, function (i, feature) {
							activeContradictions[feature] = title;
						});
					}
				});
			}

			// 
			$.each(selectedFeatures, function (i, feature) {
				if (potentiallyRedundantVacuous[i]) {
					activeRedundancies[feature] = "considered redundant by the principle of vacuous application";
				} else if (potentiallyRedundant[i]) {
					activeRedundancies[feature] = "redundant because every segment which has all the other selected values also has this value";
				}
			});

			// Update feedback classes of interactive cells for features.
			this.$tdsFeatures.each(function () {
				var $td = $(this),
					selected = $td.hasClass("selected"),
					feature = self._feature($td),
					titleIf = dependentFeaturesIf[feature],
					titleContradiction = activeContradictions[feature],
					titleRedundancy = activeRedundancies[feature],
					title;

				$td.toggleClass("if", typeof titleIf === "string");
				$td.toggleClass("contradiction", typeof titleContradiction === "string" && selected);
				$td.toggleClass("redundancy", typeof titleRedundancy === "string" && selected);

				if (typeof titleIf === "string") {
					title = titleIf;
				} else if (typeof titleContradiction === "string" && selected) {
					title = titleContradiction;
				} else if (typeof titleRedundancy === "string" && selected) {
					title = titleRedundancy;
				}
				if (title) {
					$td.attr("title", title);
				} else {
					$td.removeAttr("title");
				}
			});

			// Update feedback classes of interactive cells for changes.
			this.$tdsChanges.each(function () {
				var $td = $(this),
					selected = $td.hasClass("selected"),
					feature = self._feature($td),
					titleIf = dependentChangesIf[feature],
					titleThen = dependentChangesThen[feature],
					titleContradiction = activeContradictions[feature],
					titleRedundancy = activeRedundancies[feature],
					title;

				// Important: remove selected changes implied by dependencies.
				if (selected && typeof titleThen === "string") {
					selected = false;
					selectedChanges.splice($.inArray(feature, selectedChanges), 1); // remove
					$td.removeClass("selected");
				}

				$td.toggleClass("if", typeof titleIf === "string");
				$td.toggleClass("then", typeof titleThen === "string");
				$td.toggleClass("contradiction", typeof titleContradiction === "string" && selected);
				$td.toggleClass("redundancy", typeof titleRedundancy === "string" && selected);

				if (typeof titleIf === "string") {
					title = titleIf;
				} else if (typeof titleThen === "string") {
					title = titleThen;
				} else if (typeof titleContradiction === "string" && selected) {
					title = titleContradiction;
				} else if (typeof titleRedundancy === "string" && selected) {
					title = titleRedundancy;
				}
				if (title) {
					$td.attr("title", title);
				} else {
					$td.removeAttr("title");
				}
			});

			// Update feedback class of non-interactive cells
			// for dependent unspecified (zero) values.
			$.each(this.featureName_td0, function (featureName, td0) {
				var $td = $(td0),
					title = dependentChangesThen["0" + featureName];

				$td.toggleClass("then", typeof title === "string");

				if (title) {
					$td.attr("title", title);
				} else {
					$td.removeAttr("title");
				}
			});
		},

		// private implementation ----------------------------------------------

		// For values of distinctive features, convert from symbols in table cells
		// to corresponding symbols in list items of segments in CV charts.
		value_td_li: {
			"+": "+", // plus sign: plus sign
			"-": "-", // hyphen-minus: hyphen-minus
			"\u2013": "-", // en dash: hyphen-minus
			"0": "0" // digit zero: digit zero
		},
		value_opposite: {
			"+": "-", // plus sign: hyphen-minus
			"-": "+" // hyphen-minus: plus sign
		},

		// Initialize the data object for an interactive data cell.
		// Return false to filter out non-interactive empty cells.
		_initData: function ($td, clickFunction) {
			var text = $td.text(),
				value,
				feature;

			if (this.distinctive) {
				value = this.value_td_li[text];
				if (value) {
					// Important: cloned cells for changes must already be in the row.
					feature = value + $td.siblings(this.selectorNames).text();
				} else {
					return false;
				}
			} else {
				feature = text;
			}
			$td.data("cellFeatures", {feature: feature, clickFunction: clickFunction});
			return true;
		},

		// Initialize the data object for an interactive heading cell.
		_initHeading: function ($th, clickFunction) {
			$th.data("cellFeatures", {clickFunction: clickFunction}); // no feature
		},

		// Return the data object for an interactive cell.
		_data: function ($cell) {
			return $cell.data("cellFeatures");
		},

		// Return the feature for an interactive data cell.
		_feature: function ($td) {
			return this._data($td).feature;
		},

		// If the cell is interactive, return its click function.
		_clickFunction: function ($cell) {
			var data = this._data($cell);

			if (data) {
				return data.clickFunction;
			}
		},

		// Return an object whose properties have been set
		// according to one or more arrays of binary features.
		_objectFeatures: function (object) {
			emptyObject(object);

			// Skip the first argument
			$.each(Array.prototype.slice.call(arguments, 1), function () {
				var features = this,
					length = features.length,
					i,
					feature;

				for (i = 0; i < length; i += 1) {
					feature = features[i];
					object[feature.slice(1)] = feature.charAt(0);
				}
			});
			return object;
		},

		// public interface ----------------------------------------------------

		// Return a key for distinctive features of a segment.
		keyFeaturesChanges: function (features, objectChanges) {
			var object = this._objectFeatures({}, features);

			return $.map(this.featureNames, function (featureName) {
				return (objectChanges && objectChanges[featureName]) || object[featureName] || "0";
			}).join(" ");
		},

		// Match the features of the active segment and optional selected segment.
		toggleClassFeatures: function (dataActive, dataSelected) {
			var $tdsFeatures = this.$tdsFeatures,
				className = this.className,
				featuresActive = dataActive && dataActive[className],
				featuresSelected = dataSelected && dataSelected[className],
				self = this;

			if (featuresActive) {
				$tdsFeatures.each(function () {
					var $td = $(this),
						feature = self._feature($td),
						isActive = $.inArray(feature, featuresActive) !== -1,
						isSelected;

					if (featuresSelected) {
						isSelected = $.inArray(feature, featuresSelected) !== -1;
						if (isActive && isSelected) {
							$td.addClass("matched").removeClass("active selected");
						} else if (isActive) {
							$td.addClass("active").removeClass("matched selected");
						} else if (isSelected) {
							$td.addClass("selected").removeClass("matched active");
						} else {
							$td.removeClass("matched active selected");
						}
					} else {
						$td.toggleClass("matched", isActive).removeClass("active selected");
					}
				});
			} else {
				$tdsFeatures.removeClass("matched active selected");
			}
		}
	});

	// =========================================================================

	// CV chart is a table of consonant or vowel segments.
	$.widget("phonology.tableCV", $.phonology.counteractive, {
		_create: function () {
			var $element = this.element,
				$divFeaturesCV = $element.parentsUntil('body', 'div.CV_features').last(),
				divFeaturesCV = $divFeaturesCV.data("divFeaturesCV"),
				$tdsSegment;

			if (divFeaturesCV) {
				$tdsSegment = this.$tdsSegment = $element.find('td.Phonetic');
				this.divFeaturesCV = divFeaturesCV; // mediator widget
				this.$tablesFeatures = $element.siblings('table.features'); // own tables of featurs
				this.tdSelected = undefined; // DOM element selected in this chart, if any

				// Store features arrays as data for interactive table cells.
				$tdsSegment.each(function () {
					var td = this,
						$td = $(td);

					$td.children('ul.features').each(function () {
						var $ul = $(this);

						$td.data($ul.attr("class").split(" ")[0], $.map($ul.children('li'), textFromListItem));
					});
					$td.data("literal", $td.find('span').text());
					divFeaturesCV.pushSegment(td); // requires the data for distinctive features
				});

				// This CV chart is counteractive with directly or indirectly related tables of features.
				// When it is active (enabled), they are inactive (disabled), and vice versa.
				this._initCounteractive(divFeaturesCV.widget().find('table.features'),
					// Bind event handlers to interactive descendants:
					{
						"mouseenter": this._mouseenterSegment,
						"mouseleave": this._mouseleaveSegment
					},
					$tdsSegment);
			}
		},

		// protected interface -------------------------------------------------

		// When the mouse pointer moves out of this chart, 
		// the counteractive base class needs to know whether a segment is still selected.
		_stillActive: function () {
			return this.divFeaturesCV.hasSegmentSelected();
		},

		// event handlers ------------------------------------------------------

		// Click in a CV chart.
		// If you click a segment which is not already selected, select it.
		// Any other click in a CV chart clears the selection, if any.
		_click: function (event) {
			var $cell = $(event.target).closest('th, td').filter('td.Phonetic:not(.selected)'),
				td = $cell.length ? $cell.get(0) : undefined,
				tdSelectedPrev = this.tdSelected;

			this.divFeaturesCV.mediateSegmentSelected(this, td);
			if (tdSelectedPrev) {
				$(tdSelectedPrev).removeClass("selected");
			}
			if (td) {
				$cell.addClass("selected");
			}
			this.tdSelected = td;
		},

		// When the mouse pointer moves into a segment cell,
		// highlight the features of the active segment and optional selected segment.
		_mouseenterSegment: function (event) {
			this.divFeaturesCV.mediateSegmentActive(this, $(event.target).closest('td').get(0));
		},

		// When the mouse point moves out of a segment cell,
		// highlight the features of the optional selected segment.
		_mouseleaveSegment: function () {
			this.divFeaturesCV.mediateSegmentActive(this);
		},

		// public interface ----------------------------------------------------

		// If the active or selected segment is or was in this chart,
		// then the mediator widget calls this function.
		matchFeatures: function (dataActive, dataSelected) {
			this.$tablesFeatures.tableFeatures("toggleClassFeatures", dataActive, dataSelected);
		},

		// If a segment is selected in this chart,
		// when a segment is selected or cleared in a related chart,
		// then the mediator widget calls this function.
		clearSegmentSelected: function () {
			$(this.tdSelected).removeClass("selected");
			this.tdSelected = undefined;
		},

		// For each segment, toggle (add or remove) the class
		// according to the result of the callback function
		// optionally given the data value of the segment for the key.
		// Important: The callback function arguments and return value differ from jQuery.
		// The tableFeatures widgets call this function.
		toggleClassSegments: function (className, callback, key) {
			var $tdsSegment = this.$tdsSegment;

			if (typeof callback === "function") {
				$tdsSegment.each(function () {
					var $td = $(this),
						toggle = callback(this, typeof key === "string" ? $td.data(key) : undefined);

					if (typeof toggle === "boolean") {
						$td.toggleClass(className, toggle);
					}
				});
			} else {
				$tdsSegment.removeClass(className);
			}
		},

		// 
		toggleChangesSegments: function (segmentsChanged) {
			this.$tdsSegment.each(function () {
				var $td = $(this),
					literal = $td.data("literal"),
					value = segmentsChanged && segmentsChanged[literal],
					$div = $td.children('div.changes'),
					$ul;

				if (value) {
					if ($div.length === 0) {
						//$div = $td.children('span').wrap('<div class="changes"></div>').parent();
						$div = $td.children('span').before('<div class="changes"></div>').prev();
					}
					$ul = $div.children('ul');
					if ($ul.length) {
						$ul.children().remove();
					} else {
						$ul = $('<ul></ul>').appendTo($div);
					}
					$.each(value, function () {
						var $td = $(this);

						$('<li></li>').attr("title", $td.attr("title")).text($td.data("literal")).appendTo($ul);
					});
					$('<li>\u2192</li>').appendTo($ul); // rightwards arrow
				} else {
					$div.children('ul').remove();
				}
			});
		}
	});

	// Charts of distinctive feature values for segments =======================

	// One or two charts can optionally follow a CV chart and feature tables.
	// Each chart is in a separate section (div).
	// * features-segments: distinctive features in rows and segments in columns
	//   default orientation for typical inventories of consonants and vowels
	// * segments-features: segments in rows and distinctive features in columns
	//   optional transposed orientation for large numbers of segments

	$.widget("phonology.rowgroupsValues", {
		_create: function () {
			var $element = this.element,
				$table = $element.parent(),
				isDistinctive = $element.is('.segment-distinctive thead, .descriptive-distinctive thead, .height-backness tfoot, .analogous thead'),
				selector = "tbody" + ($table.is(".height-backness, .analogous") ? "." + $element.attr("class") : ""),
				$tbodys = $table.children(selector),
				$rowgroupsDistinctive = this.$rowgroupsDistinctive = isDistinctive ? $element : $tbodys,
				$rowgroupsCounterpart = this.$rowgroupsCounterpart = isDistinctive ? $tbodys : $element,
				$thsCounterpart = $rowgroupsCounterpart.find('th[scope]').not('[scope="rowgroup"]'),
				$tdsInCounterpartRows = [],
				selectorValues = 'td:not(.Phonetic)',
				eventNamespace = "." + this.widgetName,
				self = this;

			// Interactive for 141 segments of !Xu language (ISO-639-3 code nmn) in UPSID
			// or in the Hayes spreadsheet, but not larger multilingual projects.
			if ($thsCounterpart.length >= 150) {
				return;
			}

			if (isDistinctive) {
				$rowgroupsCounterpart.children('tr').each(function () {
					$tdsInCounterpartRows.push($(this).children(selectorValues));
				});
			}

			// Bind event handlers for distinctive feature heading cells.
			this.$thsDistinctive = $rowgroupsDistinctive.find('th[scope]').not('[scope="colgroup"]')
				.each(function (i) {
					var $th = $(this),
						$tds = isDistinctive ?
								$($.map($tdsInCounterpartRows, function ($tdsInCounterpartRow) {
									return $tdsInCounterpartRow.get(i);
								})) :
								$th.siblings(selectorValues);

					// Initialize values of feature value data cells.
					$tds.each(function () {
						var $td = $(this);

						$td.data("values", self._valuesInText($td.text()));
					});
					$th.data("$tds", $tds);
					if ($th.parent().hasClass("redundant")) {
						$th.addClass("redundant"); // for JavaScript and CSS
					}
				})
				.bind("mouseenter" + eventNamespace, function (event) {
					self._mouseenterDistinctive(event);
				})
				.bind("mouseleave" + eventNamespace, function (event) {
					self._mouseleaveDistinctive(event);
				})
				.addClass("interactive");

			// Bind event handlers for segment or descriptive feature heading cells.
			this.$thsCounterpart = $thsCounterpart
				.bind("mouseenter" + eventNamespace, function (event) {
					self._mouseenterCounterpart(event);
				}).bind("mouseleave" + eventNamespace, function (event) {
					self._mouseleaveCounterpart(event);
				})
				.addClass("interactive");

			// To select which value to highlight when a feature is active,
			// click at the lower-right of the upper-left cell.
			$element.find('th:not([scope])')
				.addClass("value")
				.append('<div><div class="value"></div></div>')

				.find('div.value')
				.bind("click" + eventNamespace, function (event) {
					self._clickValue(event);
				})
				.trigger("click"); // initialize text of div.value

			$rowgroupsDistinctive.addClass("interactive");
			$rowgroupsCounterpart.addClass("interactive");
			setTimeout(function () {
				self._createIndistinguishables($table.hasClass("distinctive-segment") || $table.hasClass("segment-distinctive"));
			}, 50);
		},

		// For each counterpart heading cell, cache the sibling cells
		// whose distinctive feature values are indistinguishable from its values.
		// * For descriptive feature heading cells in smaller charts possibly edited by hand,
		//   this function must determine which cells are indistinguishable.
		// * For segment heading cells in possibly larger charts exported by Phonology Assistant,
		//   this function assumes that the cells already have classes in the XHTML file,
		//   but it must determine which of these cells are related to each other.
		_createIndistinguishables: function (segment) {
			var $thsCounterpart = this.$thsCounterpart,
				$thsThatFromThis = segment ? $thsCounterpart.filter('.indistinguishable.that_from_this') : undefined,
				$thsThisFromThat = segment ? $thsCounterpart.filter('.indistinguishable.this_from_that') : undefined,
				$thsDistinctive = this.$thsDistinctive,
				self = this;

			$thsCounterpart.each(function (index) {
				var $th,
					$thsIndistinguishable;

				if (segment && $thsThatFromThis.index(this) === -1) {
					return;
				}

				$th = $(this);
				$thsIndistinguishable = $thsCounterpart.filter(function (i) {
					var distinguishable;

					if (i === index) {
						return false;
					} else if (segment && $thsThisFromThat.index(this) === -1) {
						return false;
					}

					$thsDistinctive.each(function () {
						var $tds = $(this).data("$tds");

						distinguishable = self._distinguishable($tds.eq(i).data("values"), $tds.eq(index).data("values"));
						return !distinguishable; // break if distinguishable
					});
					return !distinguishable;
				});
				if ($thsIndistinguishable.length) {
					$th.data("$thsIndistinguishable", $thsIndistinguishable);
					if (!segment) {
						if (!$th.has("span").length) {
							$th.wrapInner("<span></span>"); // for CSS
						}
						$th.addClass("indistinguishable that_from_this");

						$thsIndistinguishable.each(function () {
							var $thIndistinguishable = $(this);

							if (!$thIndistinguishable.has("span").length) {
								$thIndistinguishable.wrapInner("<span></span>"); // for CSS
							}
						}).addClass("indistinguishable this_from_that");
					}
				}
			});
		},

		// event handlers ------------------------------------------------------

		// When the mouse pointer moves into a segment heading cell,
		// mark all data cells whose values differ from its values.
		// Indicate any other segments which are indistinguishable.
		_mouseenterCounterpart: function (event) {
			var th = event.currentTarget,
				$th = $(th),
				$thsIndistinguishable = $th.data("$thsIndistinguishable"),
				$thsCounterpart = this.$thsCounterpart,
				length = $thsCounterpart.length,
				index = $thsCounterpart.index(th),
				tdsClass = [],
				distinguishables = [], // numbers distinguishable feature values for segments
				n = 0, // number of segments which are indistinguishable from the active segment
				i,
				self = this;

			this.$rowgroupsDistinctive.removeClass("interactive");

			// Cache data cells with added classes for _mouseleaveCounterpart.
			for (i = 0; i < length; i += 1) {
				distinguishables[i] = 0;
			}
			this.$thsDistinctive.each(function () {
				var $tds = $(this).data("$tds"),
					valuesActive = $tds.eq(index).data("values");

				$tds.filter(function (i) {
					if (self._distinguishable($(this).data("values"), valuesActive)) {
						tdsClass.push(this);
						distinguishables[i] += 1;
						return true;
					}
				}).addClass("mark");
			});
			this.$tdsClass = $(tdsClass);
			for (i = 0; i < length; i += 1) {
				if (i !== index && distinguishables[i] === 0) {
					n += 1;
				}
			}

			// Cache segment heading cells with added classes for _mouseleaveCounterpart.
			//this.$thsClass = $thsCounterpart.filter(function (i) {
			//	if (distinguishables[i] === 0 && n !== 0) {
			//		return true;
			//	}
			//}).addClass("indistinguishable");
			if ($thsIndistinguishable) {
				$thsIndistinguishable.addClass("active");
			}
		},

		// When the mouse pointer moves out of a segment heading cell,
		// remove classes from data cells and segment heading cells.
		_mouseleaveCounterpart: function (event) {
			var $th = $(event.currentTarget),
				$thsIndistinguishable = $th.data("$thsIndistinguishable");

			this.$rowgroupsDistinctive.addClass("interactive");
			this.$tdsClass.removeClass("mark");
			//this.$thsClass.removeClass("indistinguishable");
			if ($thsIndistinguishable) {
				$thsIndistinguishable.removeClass("active");
			}
		},

		// When the mouse pointer moves into a feature heading cell,
		// mark its data cells which have the specified value
		// and the corresponding segment heading cells.
		// Indicate any other segments for which the value is unspecified.
		_mouseenterDistinctive: function (event) {
			var $thDistinctive = $(event.currentTarget),
				$tds = $thDistinctive.data("$tds"),
				value = this.value,
				mark = [], // array of indexes of marked cells
				m = 0,
				length,
				self = this;

			this.$rowgroupsCounterpart.removeClass("interactive");
			if ($thDistinctive.hasClass("redundant")) {
				return;
			}

			// Cache data cells with added classes for _mouseleaveDistinctive.
			this.$tdsClass = $tds.filter(function (i) {
				if (self._match($(this).data("values"), value)) {
					mark.push(i);
					return true;
				}
			}).addClass("mark");
			length = mark.length;

			// Cache segment heading cells with added classes for _mouseleaveDistinctive.
			this.$thsClass = this.$thsCounterpart.filter(function (i) {
				var $thCounterpart = $(this);

				if (m < length && i === mark[m]) {
					$thCounterpart.addClass("mark");
					m += 1;
					return true;
				} else if (self._match($tds.eq(i).data("values"), "unspecified")) {
					$thCounterpart.addClass("unspecified");
					return true;
				}
			});
		},

		// When the mouse pointer moves out of a feature heading cell,
		// remove classes from data cells and segment heading cells.
		_mouseleaveDistinctive: function (event) {
			var $thDistinctive = $(event.currentTarget);

			this.$rowgroupsCounterpart.addClass("interactive");
			if ($thDistinctive.hasClass("redundant")) {
				return;
			}

			this.$tdsClass.removeClass("mark");
			this.$thsClass.removeClass("mark unspecified");
		},

		// To specify which feature value to highlight, click at the upper left.
		// Each click selects the next value: plus, minus, unspecified, plus, and so on.
		_clickValue: function (event) {
			var values = this.valueKeys,
				value = values[($.inArray(this.value, values) + 1) % values.length];

			this.value = value;
			$(event.currentTarget).attr("title", value).text(this.valueKeys_symbols[value][0]);
		},

		// private implementation ----------------------------------------------

		valueKeys: ["plus", "minus", "unspecified"], // array determines the order
		valueKeys_symbols: {
			"plus": ["+", "\u00B1"], // plus sign, plus-minus sign
			"minus": ["\u2013", "-", "\u00B1"], // en dash, hyphen-minus, plus-minus sign
			"unspecified": ["0", "\u00B7", "\u2022"] // digit zero, middle dot, bullet
		},
		// return references to one set of objects instead of creating many
		i_values: [
			{unspecified: true, minus: false, plus: false},	// 0 -> 4 unspecified if neither plus nor minus
			{unspecified: false, minus: false, plus: true},	// 1 = 0 + 0 + 1
			{unspecified: false, minus: true, plus: false},	// 2 = 0 + 2 + 0
			{unspecified: false, minus: true, plus: true},	// 3 = 0 + 2 + 1
			{unspecified: true, minus: false, plus: false},	// 4 = 4 + 0 + 0
			{unspecified: true, minus: false, plus: true},	// 5 = 4 + 0 + 1
			{unspecified: true, minus: true, plus: false},	// 6 = 4 + 2 + 0
			{unspecified: true, minus: true, plus: true}	// 7 = 4 + 2 + 1
		],

		// Return whether any of the symbols for the value occur in the text.
		_matchValueInText: function (value, text) {
			var symbols = this.valueKeys_symbols[value],
				length = symbols.length,
				i;

			for (i = 0; i < length; i += 1) {
				if (text.indexOf(symbols[i]) !== -1) {
					return true;
				}
			}
			return false;
		},

		// Return an object representing the values which occur in the text.
		_valuesInText: function (text) {
			var i = (this._matchValueInText("unspecified", text) ? 4 : 0) +
				(this._matchValueInText("minus", text) ? 2 : 0) +
				(this._matchValueInText("plus", text) ? 1 : 0);

			return this.i_values[i];
		},

		// Return whether the values of the feature match the specified value.
		_match: function (values, value) {
			return values[value];
		},

		// Return whether the values of a feature for a segment are distinguishable
		// from the values of the feature for the other segment.
		_distinguishable: function (valuesThis, valuesThat) {
			return (valuesThis.plus && !valuesThat.plus) || (valuesThis.minus && !valuesThat.minus);
		}
	});

	// Vowel quadrilaterals ====================================================

	$.widget("phonology.quadrilaterals", {
		_create: function () {
			var $element = this.element,
				$quadrilaterals = $element.find('.quadrilateral'),
				self = this;

			// Add buttons to merge quadrilaterals, and then bind event handler.
			if ($quadrilaterals.length === 2 &&
					$quadrilaterals.filter('.left').length === 1 &&
					$quadrilaterals.filter('.right').length === 1) {
				$quadrilaterals.append('<span class="button" title="merge quadrilaterals"></span>').find('.button')
					.bind("click." + this.widgetName, function (event) {
						self._click(event);
					});
			}
		},

		// To merge or split quadrilateral, click a button.
		_click: function (event) {
			var $button = $(event.currentTarget),
				$quadrilateral = $button.closest('.quadrilateral'),
				merged = $quadrilateral.hasClass("merged"),
				srcMergedFind = merged ? "_merged" : "",
				srcMergedReplace = merged ? "" : "_merged",
				srcExtension = ".png",
				lengthSuffix = srcMergedFind.length + srcExtension.length;

			// Swap images to simulate opacity in older browsers (especially IE 8 or earlier).
			$quadrilateral.find('.data img[src$="' + srcMergedFind + srcExtension + '"]').each(function () {
				var $img = $(this),
					src = $img.attr("src").slice(0, -lengthSuffix) + srcMergedReplace + srcExtension;

				$img.attr("src", src);
			});

			$button.attr("title", merged ? "merge quadrilaterals" : "split quadrilaterals");
			$quadrilateral.toggleClass("merged")
				.closest('.quadrilaterals').toggleClass("merged");
			if ($quadrilateral.hasClass("left")) {
				$quadrilateral.removeClass("left").addClass("right");
			} else if ($quadrilateral.hasClass("right")) {
				$quadrilateral.removeClass("right").addClass("left");
			}
		}
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

	$.widget("phonology.tableDistribution", {
		_create: function () {
			var $element = this.element,
				eventNamespace = "." + this.widgetName,
				self = this;

			// To reverse the background color of zero/non-zero cells,
			// click at the lower-right of the upper-left cell.
			if ($element.has('td.zero').length) {
				$element.find('thead tr:first-child th:first-child').addClass("value").append('<div><div class="value">0</div></div>');
			}

			// To reduce or restore a heading, click the cell.
			$element.find('th.Phonetic')
				.wrapInner('<span></span>') // wrap heading text in span,
				.append('<abbr>\u2026</abbr>'); // insert horizontal ellipsis in abbr

			// To collapse or expand a general row or column,
			// click the individual cell corresponding to the row or column group heading.
			if ($element.has('tr.general').length) {
				// Display of interactive colgroups is unreliable in IE 7.
				if (!browserIE7()) {
					this._initializeGeneralizedDistribution();
				}
			}

			// Bind event handler.
			$element
				.bind("click" + eventNamespace, function (event) {
					self._click(event);
				})
				.addClass("interactive");
		},

		// event handlers ------------------------------------------------------

		_click: function (event) {
			var $target = $(event.target),
				$cell;

			if ($target.is('div.value')) {
				// To reverse the background color of zero/non-zero cells,
				// click at the lower-right of the upper-left cell.
				this.element.toggleClass("nonzero");
			} else {
				$cell = $target.closest('th, td');
				if ($cell.is('th')) {
					if ($cell.hasClass("Phonetic")) {
						$cell.toggleClass("reduced");
					} else if ($cell.hasClass("interactive")) {
						if ($cell.parent().parent().is('tbody')) {
							this._clickCollapsibleRowHeading($cell);
						} else if ($cell.is('thead tr:first-child th:first-child')) {
							this._clickUpperLeft($cell);
						} else {
							this._clickCollapsibleColumnHeading($cell);
						}
					}
				}
			}
		},

		// private implementation ----------------------------------------------

		// Add data, class attributes, text to elements.
		_initializeGeneralizedDistribution: function () {
			var $element = this.element,
				$colgroups = $element.find('colgroup'),
				chartHasGeneralColumns = ($element.has('thead tr.general').length !== 0),
				$thCols = $element.find('thead tr.individual th'),
				$thCollapsibles,
				start = 0;

			$element.addClass('generalized');

			// Add data to colgroup elements and class attributes to col elements.
			$colgroups.each(function (colgroup) {
				var $colgroup = $(this),
					$cols = $colgroup.find('col'),
					colspan = $cols.length;

				$colgroup.data("distribution", {colspan: colspan});
				if (colgroup === 0) { // Row headings at the left of the data.
					if (colspan === 2) { // If there are general rows:
						$cols.eq(0).addClass("general");
						$cols.eq(1).addClass("individual");
					}
				} else if (chartHasGeneralColumns) {
					$cols.eq(0).addClass("general"); // First column.
					$cols.slice(1).addClass("individual"); // Any other columns.
				}
			});

			// Add class attribute and data to upper-left cell.
			$element.find('thead tr:first-child th:first-child')
				.addClass("interactive") // Used by CSS, not JavaScript.			
				.data("distribution", {
					colspan: $element.find('tbody:first tr:first-child th').length,
					rowspan: $element.find('thead tr').length
				});

			// Add class attribute and data to general column group headings, if any.
			$element.find('thead tr.general th.Phonetic').each(function (i) {
				var $thColgroup = $(this),
					colgroup = i + 1, // Adjust index to account for the upper-left cell.
					colspan = $colgroups.eq(colgroup).find('col').length,
					end = start + colspan;

				$thColgroup.data("distribution", {colgroup: colgroup, colspan: colspan});

				// Column headings for the group in the individual heading row.
				// Empty heading cell corresponding to the column group.
				if (colspan > 1) { // If there are any individual columns:
					$thCols.eq(start)
						.addClass("interactive")  // Used by CSS, not JavaScript.
						.data("distribution", {colgroup: colgroup, start: start, end: end});
				}

				// Add class to individual heading cells.
				$thCols.slice(start + 1, end).addClass('individual');

				start += colspan;
			});

			if (chartHasGeneralColumns) {
				// Add class to individual data cells.		
				$thCollapsibles = $element.find('thead tr.individual th.interactive');
				$element.find('tbody tr').each(function () {
					var $tds = $(this).find('td');

					$thCollapsibles.each(function () {
						var data = $(this).data("distribution");
						$tds.slice(data.start + 1, data.end).addClass("individual");
					});
				});
			}

			// General row groups, if any.
			// Empty heading cells corresponding to row groups.
			$element.find('tbody tr.general').each(function () {
				var $tr = $(this),
					rowspan = $tr.parent().find('tr').length,
					$th = $tr.find('th:not(.Phonetic)');

				$th.addClass("individual");
				if (rowspan !== 1) {
					$th.addClass("interactive"); // Used by CSS, not JavaScript.
				}
				$tr.find('th.Phonetic').data("distribution", {rowspan: rowspan});
			});
		},

		// Temporarily collapse/expand all individual rows and columns in generalized charts.
		_clickUpperLeft: function ($thUpperLeft) {
			var $element = this.element,
				collapsed,
				data,
				colspan,
				rowspan,
				$colgroups = $element.find('colgroup');

			// 1. Toggle collapsed class of table.
			$element.toggleClass("collapsed");
			collapsed = $element.hasClass("collapsed");

			// 2a. Adjust colspan and rowspan of upper-left cell.
			if (collapsed) {
				colspan = 1;
				rowspan = 1;
			} else {
				data = $thUpperLeft.data("distribution");
				colspan = data.colspan;
				rowspan = data.rowspan;
			}
			$thUpperLeft.attr("colspan", colspan).attr("rowspan", rowspan);

			// 2b. Adjust colspan of general column headings, if any.
			$element.find('thead tr.general th.Phonetic').each(function () {
				var $thPhonetic = $(this),
					data;

				if (collapsed) {
					colspan = 1;
				} else {
					data = $thPhonetic.data("distribution");
					if ($colgroups.eq(data.colgroup).hasClass("collapsed")) {
						colspan = 1;
					} else {
						colspan = data.colspan;
					}
				}
				$thPhonetic.attr("colspan", colspan);
			});

			// 2c. Adjust rowspan of general row headings, if any.
			$element.find('tbody tr.general th.Phonetic').each(function () {
				var $thPhonetic = $(this);

				if (collapsed) {
					rowspan = 1;
				} else if ($thPhonetic.closest('tbody').hasClass("collapsed")) {
					rowspan = 1;
				} else {
					rowspan = $thPhonetic.data("distribution").rowspan;
				}
				$thPhonetic.attr("rowspan", rowspan);
			});
		},

		// Collapse/expand all the individual columns in a general column group.
		_clickCollapsibleColumnHeading: function ($thCollapsible) {
			var $element = this.element,
				data = $thCollapsible.data("distribution"),
				colgroup = data.colgroup,
				$colgroup = $element.find('colgroup').eq(colgroup),
				$thColgroup = $element.find('thead tr.general th').eq(colgroup),
				start = data.start + 1, // The first column is general.,
				end = data.end; // Up to, but not including, end.;

			// 1. Toggle collapsed class of individual heading cell and colgroup.
			$thCollapsible.toggleClass("collapsed");
			$colgroup.toggleClass("collapsed");

			// 2. Adjust colspan of general heading cell.
			$thColgroup.attr("colspan", $colgroup.hasClass("collapsed") ? 1 : $thColgroup.data("distribution").colspan);

			// 3. Toggle collapsed class for cells in individual columns.
			$element.find('thead tr.individual')
				.children('th').slice(start, end).toggleClass("collapsed");
			$element.find('tbody tr').each(function () {
				$(this).children('td').slice(start, end).toggleClass("collapsed");
			});
		},

		// Collapse/expand all the individual rows in a general row group.
		_clickCollapsibleRowHeading: function ($thCollapsible) {
			var $tr = $thCollapsible.parent(),
				$thPhonetic = $tr.find('th.Phonetic'),
				$tbody = $tr.parent();

			// 1. Toggle collapsed class of tbody.
			$tbody.toggleClass("collapsed");

			// 2. Adjust rowspan of general row.
			$thPhonetic.attr("rowspan", $tbody.hasClass("collapsed") ? 1 : $thPhonetic.data("distribution").rowspan);
		}
	});

	$('div.section').section();
	$('.quadrilaterals').quadrilaterals();
	setTimeout(function () {
		$('table.distribution').tableDistribution();
		$('table.list').not('.contradictions, .dependencies').tableList();
		$('div.CV_features').not('div.CV_features div.CV_features').has('table.features').has('table.CV').divFeaturesCV();
		// tableCV widgets must follow tablesFeatures widgets (which are created by divFeaturesCV widgets)
		$('table.CV').tableCV();
		$('table.values thead, table.height-backness tfoot, table.analogous tfoot').rowgroupsValues();
	}, 50);
});