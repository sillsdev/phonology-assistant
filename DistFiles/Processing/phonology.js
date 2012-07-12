// phonology.js 2012-05-24
// Interactive behavior for XHTML files exported from Phonology Assistant.
// http://phonologyassistant.sil.org/

// Requires jQuery 1.7 or later: .on, .Callbacks. http://jquery.com/
// Requires jQuery UI widget base class. http://jqueryui.com
// Requires phonquery.js for:
// * consonant or vowel charts <table class="CV chart ...">
//   with tables of descriptive or distincitve features <table class="... features">
// * values charts <table class="... values chart">

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

// Execute when the DOM is fully loaded.
// The following is related to $(document).ready(...) but even better because:
// * It avoids an explicit global reference to document.
// * It ensures that $(...) is a shortcut for jQuery(...) within the function.

/*global jQuery: false, phonQuery: false */ // for JSLint or JSHint

jQuery(function ($) {
	"use strict"; // Firefox 4 or later

	// http://jslint.com defines a professional subset of JavaScript.
	/*jslint browser: true */ // for example, document, setTimeout
	/*jslint devel: false */ // console, alert
	/*jslint nomen: true */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jslint indent: 4 */ // consider a tab equivalent to 4 spaces

	// http://jshint.com is a tool to detect errors and potential problems in JavaScript code.
	/*jshint browser: true */ // for example, document, setTimeout
	/*jshint devel: false */ // console, alert
	/*jshint trailing: true */ // warn about trailing white space
	/*jshint nomen: false */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jshint onevar: true */ // only one var statement per function
	/*jshint white: false */ // do not warn about indentation

	// Utility functions -------------------------------------------------------

	var slice = Array.prototype.slice,

		// Return a new function that, when called, itself calls f
		// * in the context of thisArg
		// * with a given sequence of arguments
		//   preceding any provided when the new function was called
		// See pages 137-139 in JavaScript Patterns.
		// Instead of extending Function.prototype, define bind as a variable:
		// either the ECMAScript 5 function or a substitute.
		bind = Function.prototype.bind || function (thisArg) {
			var f = this,
				args = slice.call(arguments, 1);

			return function () {
				return f.apply(thisArg, args.concat(slice.call(arguments)));
			};
		},

		// ECMAScript 5 function or an alternate.
		forEach = Array.forEach || function (collection, callback, context) {
			var i, length;

			for (i = 0, length = collection.length; i < length; i += 1) {
				callback.call(context, collection[i], i, collection);
			}
		},

		// Append a (shallow) copy of items in arrayFrom to arrayTo.
		// That is, at the end of arrayTo, push each item in arrayFrom.
		append = function (arrayTo, arrayFrom) {
			var i, length;

			for (i = 0, length = arrayFrom.length; i < length; i += 1) {
				arrayTo.push(arrayFrom[i]);
			}
		},

		// Return whether the browser version is IE7, because of its limitations in CSS:
		// * lack of :before and content for transcriptions
		// * limited support for collapsing rowgroups and colgroups for distribution charts
		browser = function (browserClass) {
			// The link element has a class attribute and is in a conditional comment.
			return $('html head link[rel="stylesheet"]').filter("." + browserClass).length !== 0;
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

		// From the class attribute of an element, return a space-separated value.
		splitClass = function ($element, i) {
			var classes = ($element.attr("class") || "").split(" ");

			return arguments.length === 1 ? classes : (i < classes.length ? classes[i] : "");
		},

		// Store data for this element which represents a segment.
		// For example, interactive cells in a CV chart: $tableCV.find('td.Phonetic').each(dataSegment);
		dataSegment = function () {
			var $element = $(this),
				$uls = $element.children('ul.features'),
				featureQuery = phonQuery.featureQuery,
				data = $element.data(); // The first call to the data method
				// automatically converts any HTML5 data attributes of the element
				// to properties of the data object:
				// http://api.jquery.com/data/#data-html5
				// Important: A call to the data function $.data(element) does not convert attributes.

			data.symbols = $element.children('span').text();
			data.description = $element.attr("title");
			forEach(featureQuery.featureClasses, function (featureClass) {
				var features = data[featureClass],
					$ul;

				if (features && $.isArray(features)) {
					// If jQuery parsed an HTML5 data attribute for the feature class as an array,
					// then replace the data property with the corresponding features object.
					// <... data-descriptive='["consonant", "voiceless", "bilabial", "plosive"]' ...>
					// <... data-distinctive='["-syll", "+cons", "-son", "-approx", ...]' ...>
					// Important: As in JSON, double quotes must enclose the strings.
					features = data[featureClass] = featureQuery(featureClass, features);
				} else {
					$ul = $uls.filter('.' + featureClass);
					if ($ul.length === 1) {
						// If this element has a features list for the feature class,
						// then set the data property to the corresponding features object.
						// <ul class="descriptive features"><li>consonant</li>...</ul>
						// <ul class="distinctive features"><li>-syll</li>...</ul>
						// Tip: Requires display: none in CSS, but is more usable outside a browser.
						// For example, much easier for XSLT to input and output lists of features.
						features = data[featureClass] = featureQuery(featureClass);
						$ul.children().each(function () {
							features.add(textFromListItem(this));
						});
					}
				}
			});
		};

	// Section of a phonology report ===========================================
	//
	// Each section consists of a heading (which is the first child),
	// one or more tables, optional paragraphs, and optional child sections.
	// HTML5: section element
	// HTML4: div element whose class attribute contains section

	$('div.section').children(':header:first-child').on("click.section", function () {
		$(this).parent().toggleClass("collapsed");
	}).addClass("interactive");

	// Data or Search view =====================================================
	//
	// Sort by columns.
	// Collapse/expand groups.

	$.widget("phonology.tableList", {
		_create: function () {
			var $element = this.element,
				$ulTranscriptions = $element.find('td > ul.transcription'),
				// Can sort by any column which contains data,
				// except when there is one minimal pair per group
				// because it might contain both subheading and data rows.
				sortable = $element.has('thead th.sorting_field').length !== 0 &&
					$element.has('tbody.group th.pair').length === 0,
				eventNamespace = "." + this.widgetName,
				self = this;

			if ($ulTranscriptions.length) {
				this._createTranscriptions($ulTranscriptions);
			}

			if (sortable) {
				this._createSorting();
			}

			// If there are sortable columns or collapsible groups, or both,
			// attach event handler for the table, and then add interactive class.
			if (sortable || $element.has('tbody.group') !== 0) {
				$element.on("click" + eventNamespace, function (event) {
					self._click(event);
				}).addClass("interactive");
			}
		},

		// Insert non-semantic markup and content which is intentionally omitted from XHTML files.
		_createTranscriptions: function ($ulTranscriptions) {
			// If a data cell contains a list of transcriptions,
			// wrap all children in a div for the phonology.css file,
			// because the data cell itself cannot have relative position.
			$ulTranscriptions.parent().wrapInner('<div class="transcription"></div>');

			if (browser("IE7")) {
				// Because IE 7 does not support the :before pseudo-element,
				// between del and ins elements, insert black right-pointing small triangle.
				$ulTranscriptions.find('li.change del + ins').before(' \u25B8 ');
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

			this.$details = $element.siblings('table.details').find('tr.sorting_field td');
			this.$thsSortable = $element.find('thead th').not('.group').each(function () {
				var $th = $(this),
					$span = $th.find('span'),
					fieldName = $span.length ? $span.text() : $th.text(),
					classes = ($th.attr("class") || "").split(" "),
					data = {
						// The HTML and CSS class for a field consists of alphanumeric characters only.
						// For example, omit space and period.
						// \W is the opposite of \w, which is equivalent to [0-9A-Z_a-z].
						// Therefore, [\W_] is equivalent to [^0-9A-Za-z],
						// but JSLint does not consider it an insecure use of [^...].
						sortingSelector: '.' + fieldName.replace(/[\W_]/g, ""),

						// Is this the field by which the list is grouped?
						groupField: $.inArray("sorting_field", classes) !== -1 &&
							$th.siblings('th.group').length !== 0,

						// Is this the Phonetic field in Search view?
						searchField: $th.attr("colspan") === "3"
					},
					$ins = $th.find('ins'),
					hasSortingOptions = $.inArray("sorting_options", classes) !== -1;

				if (data.searchField) {
					data.sortingSelector += '.item';
				}
				if ($ins.length) {
					self._sortingIndicator(data, $ins.text()); // initialize descending and sortingOption
					$th.contents().filter(function (i) {
						return this.nodeType === 3 && i !== 0; // text node not at beginning
					}).remove(); // remove any white space between <span>...</span> and <ins>...</ins>
				} else {
					// The first time you sort by any column other than the primary sort field,
					// assume that the direction is ascending and insert the visual indicator.
					data.descending = false;
					if (hasSortingOptions) {
						data.sortingOption = self.sortingOptions[0]; // assume as the default
					}
					if ($span.length === 0) {
						$th.wrapInner('<span></span>');
					}
					$('<ins></ins>').text(self._sortingIndicator(data)).appendTo($th);
				}
				if ($th.has('div.indicator').length === 0) {
					$th.wrapInner('<div class="indicator"></div>');
				}
				if (hasSortingOptions) {
					data.sortingSelector += (' > ul.sorting_order > li');

					// TO DO: encapsulate
					data.hasIndex = $element.find(data.sortingSelector).each(function () {
						var $li = $(this);

						$li.data("sortingIndex", parseInt($li.text(), 10) - 1); // TO DO: descending?
					}).length !== 0;

					// To make IE 7 display the drop-down menu on the next line,
					// wrap the contents in an explicit block element.
					// Must have inserted <span>...</span><ins>...</ins> before this!
					if (browser("IE7")) {
						$th.wrapInner('<div></div>');
					}
					$th.append(self._$ulSortingOptions())
						// Bind event handlers for sorting options.
						.on("mouseenter" + eventNamespace, function (event) {
							self._mouseenterSortingOptions(event);
						})
						.on("mouseleave" + eventNamespace, function (event) {
							self._mouseleaveSortingOptions(event);
						});
				}

				$th.data("sorting", data);
				//console.dir(data);
			}).addClass("sortable");
		},

		// event handlers ------------------------------------------------------

		// When the mouse pointer moves over a Phonetic heading cell,
		// a drop-down menu of sorting options appears.
		_mouseenterSortingOptions: function (event) {
			this._addSortingOptionClasses($(event.currentTarget));
		},

		// When the mouse pointer moves out of the heading cell,
		// sorting options disappear.
		_mouseleaveSortingOptions: function (event) {
			this._removeSortingOptionClasses($(event.currentTarget));
		},

		// Click in a sortable or collapsible list.
		_click: function (event) {
			var $target = $(event.target),
				$cell = $target.closest('th, td'),
				classes;

			if ($cell.is('th')) {
				classes = ($cell.attr("class") || "").split(" ");
				if ($.inArray("group", classes) !== -1) {
					// To collapse/expand all groups, click at the upper left.
					this.element.toggleClass("collapsed"); // table
				} else if ($.inArray("count", classes) !== -1) {
					// To collapse/expand a group, click at the left of its heading row.
					$cell.parent().parent().toggleClass("collapsed"); // tbody
				} else if ($.inArray("sortable", classes) !== -1) {
					// To sort by a column, click its heading cell or a phonetic option.
					this._clickSortableHeading($target);
				}
			}
		},

		// private implementation for event handlers ---------------------------

		_addSortingOptionClasses: function ($th) {
			var sortingOption = $th.data("sorting").sortingOption;

			$th.addClass("active").find('li').filter(function () {
				return $.data(this, "sorting").sortingOption === sortingOption;
			}).addClass("initial");
		},

		_removeSortingOptionClasses: function ($th) {
			$th.removeClass("active").find('li.initial').removeClass("initial");
		},

		// To sort by a column, click its heading cell or a sort option.
		_clickSortableHeading: function ($target) {
			var $th = $target.closest('th'),
				data = $th.data("sorting"),
				classes = ($th.attr("class") || "").split(" "),
				sortingField = $.inArray("sorting_field", classes) !== -1, // click the active column heading
				reverse = sortingField,
				sortingOptionSelected;

			// Phonetic sort option.
			if ($target.is('li')) {
				sortingOptionSelected = $target.data("sorting").sortingOption;
				if (sortingField) {
					// Only if this is the active sorting field:
					// * Reverse the direction if the sort option remains the same.
					// * Keep the direction the same if the sorting option changes.
					reverse = (data.sortingOption === sortingOptionSelected);
				}
				data.sortingOption = sortingOptionSelected;
			}
			// The following condition is independent of the previous condition,
			// because you might click the heading instead of the list.
			if ($.inArray("sorting_options", classes) !== -1) {
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

			if (!sortingField) {
				this.$thsSortable.removeClass("sorting_field");
				$th.addClass("sorting_field");
			}
			$th.find('ins').text(this._sortingIndicator(data));

			if (this.$details.length) {
				this._details($th);
			}
		},

		// private implementation for user interface ---------------------------

		sortingOptions: ["place_or_backness", "manner_or_height"],
		sortingOption_label: {
			"place_or_backness": "place or backness",
			"manner_or_height": "manner or height"
		},

		// Return a list for a drop-down menu of sorting options.
		_$ulSortingOptions: function () {
			var $ul = $('<ul></ul>'),
				sortingOptions = this.sortingOptions,
				sortingOption_label = this.sortingOption_label,
				length = sortingOptions.length,
				i,
				sortingOption;

			for (i = 0; i < length; i += 1) {
				sortingOption = sortingOptions[i];
				$('<li></li>').text(sortingOption_label[sortingOption])
					.data("sorting", {sortingOption: sortingOption})
					.appendTo($ul);
			}
			return $ul;
		},

		sortingIndicators: {
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

		_sortingIndicator: function (data, text) {
			var sortingOption,
				descending,
				value;

			if (arguments.length === 1) {
				// Return a character for the ins element in the column heading.
				sortingOption = data.sortingOption;
				descending = data.descending;
				$.each(this.sortingIndicators, function (key, value) {
					if (value.sortingOption === sortingOption && value.descending === descending) {
						text = key;
						return false; // break;
					}
				});
				return text;
			}

			// Initialize data for the column heading of the sort field
			// from a character in the ins element exported by Phonology Assistant.
			value = this.sortingIndicators[text];
			if (value) {
				data.descending = value.descending;
				if (value.sortingOption) {
					data.sortingOption = value.sortingOption;
				}
			}
		},

		// Update a row in the table of details.
		_details: function ($th) {
			var text = $th.find('span').text(), // name of sorting field
				data = $th.data("sorting"),
				label = this.sortingOption_label[data.sortingOption];

			if (typeof label === "string") {
				text += (", " + label);
			}
			if (data.descending) {
				text += ", descending";
			}
			this.$details.text(text);
		},

		// private implementation for sorting ----------------------------------

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
			/*jslint plusplus: true */ // allow ++ and --
			/*jshint plusplus: false */ // allow ++ and --
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
		}
	});

	// mediator ================================================================

	// Common ancestor of widgets which use callbacks to publish or subscribe, or both.
	// Mediator provides loose coupling between charts, tables, diagrams, forms.
	// See pages 273-282 in Design Patterns, 167-171 in JavaScript Patterns.
	$.widget("phonology.mediator", {
		_create: function () {
			var callbacksObject = {},
				self = this;

			// Proxy functions control access to the corresponding objects.
			// See pages 207-217 in Design Patterns, 159-167 in JavaScript Patterns.
			// The jQuery UI widget factory makes a deep copy of the options object
			// and its object values, but a shallow copy of its function values.
			this.callbacksProxy = function (channel) {
				return self._callbacks(callbacksObject, channel);
			};
		},

		// private implementation ----------------------------------------------

		// Return the callbacks object for a channel, also known as a subject.
		// For information about publish-subscribe, also known as observer, see:
		// pages 293-303 in Design Patterns
		// pages 171-178 in JavaScript Patterns
		// http://api.jquery.com/jQuery.Callbacks/#pubsub
		_callbacks: function (callbacksObject, channel) {
			var callbacks = callbacksObject[channel];

			if (!callbacks) {
				callbacks = callbacksObject[channel] = $.Callbacks();
			}
			return callbacks;
		}
	});

	// mediatee ----------------------------------------------------------------

	// Base class for widgets which use callbacks to publish or subscribe, or both.
	// For loose coupling, create mediator and mediatee widgets independently.
	// Make sure to create a mediator widget before its mediatee descendants.
	$.widget("phonology.mediatee", {
		// Return the callbacks object:
		// * for a channel, also known as a subject
		// * provided by which ancestor mediators:
		//   * "closest" (the default)
		//   * "farthest"
		//   * "all"
		_callbacksProxy: function (channel, which) {
			var $mediators = this.element.parents('.mediator'),
				length = $mediators.length,
				first,
				last,
				callbacksArray,
				i;

			if (length > 0) {
				if (which === "closest" || typeof which === "undefined") {
					first = 0;
					last = 1;
				} else if (which === "farthest") {
					first = length - 1;
					last = length;
				} else if (which === "all") {
					first = 0;
					last = length;
				} else {
					length = 0;
				}
			}
			if (length > 0) {
				callbacksArray = $mediators.map(function (index) {
					var mediator;

					if (first <= index && index < last) {
						mediator = $.data(this, "mediator");
						return mediator && mediator.callbacksProxy(channel);
					}
				});
				length = callbacksArray.length;
			}
			return length === 1 ? callbacksArray[0] :
					// Return a composite callbacks object if "all" is more than one.
					// Important: Its only methods are add and fire.
					{
						add: function () {
							for (i = 0; i < length; i += 1) {
								callbacksArray[i].add.apply(null, arguments);
							}
							return this; // chaining pattern
						},
						fire: function () {
							for (i = 0; i < length; i += 1) {
								callbacksArray[i].fire.apply(null, arguments);
							}
							return this; // chaining pattern
						}
					};
		}
	});

	// counteractivepart =======================================================

	// Base class for widget which is a counterpart in interactive behavior to others:
	// if this widget is active (enabled), they are inactive (disabled), and vice versa.
	$.widget("phonology.counteractivepart", $.phonology.mediatee, {
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
		// * channels to publish when the mouse pointer enters or leaves this
		// * channels to subscribe when the mouse pointer enters or leaves a counterpart
		// * event handlers for descendants to call only when this element is active (enabled)
		_counteractivepart: function (publishObject, subscribeObject, eventMap, $descendants) {
			var eventNamespace = "." + this.widgetName, // of derived class
				self = this,
				subscribe = function (publisher, disable) {
					if (publisher !== self) {
						self.option("disabled", disable);
					}
				},
				publishArray = [],
				length = 0,
				publish = function (disable) {
					var i;

					for (i = 0; i < length; i += 1) {
						publishArray[i](self, disable);
					}
				};

			$.each(subscribeObject, function (channel, which) {
				self._callbacksProxy(channel, which).add(subscribe);
			});
			$.each(publishObject, function (channel, which) {
				publishArray.push(self._callbacksProxy(channel, which).fire);
				length += 1;
			});

			// Bind event handlers to widget.
			// Encapsulate the if (!self.options.disabled) statement,
			// so event handlers can assume the widget is active (enabled).
			this.element
				// Trigger events instead of call methods directly
				// to allow multiple derived classes (for example, tableFeatures, tableCV).
				.on("mouseenter" + eventNamespace, function () {
					// When the mouse pointer moves into a widget,
					// unless it is disabled, disable counteractivepart widgets.
					if (!self.options.disabled) {
						publish(true);
					}
				})
				.on("mouseleave" + eventNamespace, function () {
					// When the mouse pointer moves out of a widget,
					// unless it is disabled or it is still active,
					// enable counteractivepart widgets.
					// Assumes a _counteractive method is defined in derived widget.
					if (!self.options.disabled && !self._counteractive()) {
						publish(false);
					}
				})

				// For information about how the following base event handler
				// is a template method, see pages 325-330 in Design Patterns.
				.on("click" + eventNamespace, function (event) {
					// When there is a click in a widget,
					// unless it is disabled, call _click method in derived widget.
					if (!self.options.disabled) {
						self._click(event);
					}
				});

			// Bind event handlers to interactive descendants of widget.
			if (eventMap && $descendants) {
				$.each(eventMap, function (eventType, eventFunction) {
					$descendants.on(eventType + eventNamespace, function (event) {
						if (!self.options.disabled) {
							eventFunction.call(self, event);
						}
					});
				});
			}

			this.enable();
		}
	});

	// Consonant Chart or Vowel Chart view =====================================
	//
	// Display segments which have sets of features or changes.
	// Display features for segments or pairs of segmentss.

	// =========================================================================

	// Tables of descriptive or distinctive features at the right of CV charts.
	$.widget("phonology.tableFeatures", $.phonology.counteractivepart, {
		_create: function () {
			var $element = this.element,
				$thFeatures = this.$thFeatures = $element.find('thead th'),
				featureClass = this.featureClass = splitClass($element, 0),
				distinctive = this.distinctive = featureClass === "distinctive",
				selectorFeatures = this.selectorFeatures = distinctive ? 'td:not(.name)' : 'td',
				featureQuery = this.featureQuery = phonQuery.featureQuery,
				clickFeature = this._clickFeature,
				$tdsFeatures,
				self = this;

			this.selectorNames = 'td.name'; // for _initData, _createDistinctive, and so on
			this.featuresSelected = featureQuery(featureClass);
			this.featuresMatched = featureQuery(featureClass); // selected or active

			// If no segment is selected in a related CV chart,
			// use false as the falsy value instead of undefined or null.
			this.featuresSelectedSegment = false;

			// Initialize data for main heading cell.
			$thFeatures.data(this.widgetName, {click: this._clickMainHeading});

			// Initialize data for interactive data cells.
			// Must follow featuresSelected and precede _createDistinctive.
			$tdsFeatures = this.$tdsFeatures = $element.find(selectorFeatures)
				.filter(function () {
					// Important: initializes data as a side-effect if function returns true.
					return self._initData(this, clickFeature);
				});

			if (distinctive) {
				this._createDistinctive();
			}

			this.visitorMatched = bind.call(this._visitorMatched, this); // for publishFeatures
			this.publishFeatures = this._callbacksProxy("features").fire;
			this._callbacksProxy("segment").add(bind.call(this._subscribeSegment, this));

			// This features table is a counterpart in interactive behavior
			// to related features tables and CV charts.
			// When it is active (enabled), they are inactive (disabled), and vice versa.
			this._counteractivepart({"features_active": "farthest"},
				{"features_active": "farthest", "segment_active": "farthest"},
				// Bind event handlers to interactive descendants:
				{
					"mouseenter": this._mouseFeature,
					"mouseleave": this._mouseFeature
				},
				$tdsFeatures.addClass("interactive"));
		},

		_createDistinctive: function () {
			var $element = this.element,
				$ulsDistinctive = $element.nextUntil('table.features', 'ul.distinctive'),
				hasChanges = this.hasChanges = $element.hasClass("changes"),
				namesArray = this.namesArray = $.map($element.find(this.selectorNames), textFromCell),
				namesObject = this.namesObject = {},
				i,
				length;

			for (i = 0, length = namesArray.length; i < length; i += 1) {
				namesObject[namesArray[i]] = true;
			}
			this._createContradictions($ulsDistinctive.filter('.contradictions'));
			this._createDependencies($ulsDistinctive.filter('.dependencies'));
			// _createDependencies must precede _createChanges
			this.featuresRedundant = this.featureQuery(this.featureClass);
			if (hasChanges) {
				this._createChanges(); // add a colgroup of changes
				this.visitorChanged = bind.call(this._visitorChanged, this); // for publishFeatures
			}
		},

		// Add a colgroup of changes at the right of a table of distinctive features.
		_createChanges: function () {
			var $element = this.element,
				widgetName = this.widgetName,
				eventNamespace = "." + widgetName,
				featureClass = this.featureClass,
				featureQuery = this.featureQuery,
				$thead = $element.children('thead'),
				$th = $thead.find('th'),
				$thChanges = this.$thChanges = $th.clone(),
				$tfoot = this.$tfoot = $('<tfoot><tr><td><div><span></span></div></td></tr></tfoot>'),
				selectorNames = this.selectorNames,
				feature_td0 = this.feature_td0,
				col0 = !($.isEmptyObject(feature_td0)),
				colspan = col0 ? 3 : 2,
				td0 = col0 ? document.createElement('td') : null,
				selectorFeatures = this.selectorFeatures,
				clickChange = this._clickChange,
				tdsChanges = [],
				tdsChanges0 = [],
				self = this;

			this.changesSelected = featureQuery(featureClass);
			this.changesSelectedOrDependent = featureQuery(featureClass);
			this.featuresVacuous = featureQuery(featureClass);
			this.dependentFeaturesIf = {};
			this.dependentChangesIf = {};
			this.dependentChangesThen = {};

			$thead.before('<colgroup><col /><col />' + (col0 ? '<col />' : '') + '</colgroup>');
			$thChanges.data(widgetName, {click: this._clickChangesHeading})
				.attr("colspan", colspan)
				.find('span').text("changes").wrap('<div></div>');
			$th.after($thChanges);
			$element.find('tbody tr').each(function () {
				var $tr = $(this),
					$tds = $tr.children(selectorFeatures).clone(),
					feature,
					td,
					key;

				$tr.append($tds);
				$tds.each(function () {
					// Important: initializes data as a side-effect if function returns true.
					if (self._initData(this, clickChange)) {
						tdsChanges.push(this);
					}
				});
				if (col0) {
					td = td0.cloneNode(false);
					$tr.append(td);
					feature = "0" + $tr.children(selectorNames).text();
					key = feature_td0[feature]; // "if", "then", or undefined
					if (key) {
						feature_td0[feature] = td;
						td.appendChild(document.createTextNode("0"));
						tdsChanges0.push(td);
						if (key === "if") {
							// Important: cloned cells for changes must already be in the row.
							// Important: initializes data as a side-effect if function returns true.
							if (self._initData(td, clickChange)) {
								tdsChanges.push(td);
							}
						}
					}
				}
			});
			this.$spanChanges = $tfoot.find('td').attr("colspan", 3 + colspan).find('span');
			$element.append($tfoot);
			this.$tdsChanges0 = $(tdsChanges0);
			this.$tdsChanges = $(tdsChanges).on("mouseleave" + eventNamespace, function (event) {
				self._mouseleaveChange(event);
			}).addClass("change");

			this.objectSegmentChanges = {};
			this._callbacksProxy("segment_changes").add(bind.call(this._subscribeSegmentChanges, this));
		},

		// protected interface -------------------------------------------------

		// When the mouse pointer moves out of this table,
		// the counteractivepart base class needs to know whether a feature is still selected.
		_counteractive: function () {
			return this.featuresSelected.has();
		},

		// event handlers ------------------------------------------------------

		// Click in a table of features.
		_click: function (event) {
			var $cell = $(event.target).closest('th, td'),
				data = $cell.data(this.widgetName),
				distinctive;

			// If the target is (in) an interactive feature cell.
			if (data) {
				data.click.call(this, $cell);
				distinctive = this.distinctive;
				if (distinctive) {
					this._feedbackDistinctiveInit();
				}
				this._publishFeatures();
				if (distinctive) {
					this._feedbackDistinctive();
				}
			}
		},

		// Unless an interactive feature cell is already selected:
		// * When the mouse pointer moves in,
		//   match segments which have all selected and active features.
		// * When the mouse pointer moves out of a feature cell,
		//   match segments which have all selected features.
		_mouseFeature: function (event) {
			var enter = event.type === "mouseenter",
				td = event.currentTarget,
				$td = $(td);

			if (!enter) {
				$td.removeClass("click");
			}
			if (!($td.hasClass("selected"))) {
				this.featuresMatched.toggle($.data(td, this.widgetName).feature, enter);
				this._publishFeatures();
			}
		},

		// When the mouse pointer moves out of a change cell,
		// remove class which indicates whether it was clicked.
		_mouseleaveChange: function (event) {
			$(event.currentTarget).removeClass("click");
		},

		// private implementation for event handlers ---------------------------

		// Click to select or clear a feature.
		_clickFeature: function ($td) {
			var $element = this.element,
				widgetName = this.widgetName,
				feature = $td.data(this.widgetName).feature,
				featuresSelected = this.featuresSelected,
				featuresMatched = this.featuresMatched,
				anySelected;

			// If contour values are unavailable, before this feature becomes selected,
			// if the opposite value was selected, clear it.
			if (!this.contour && !($td.hasClass("selected"))) {
				$td.siblings('.selected:not(.change)').each(function () {
					var feature = $(this).removeClass("selected").data(widgetName).feature;

					featuresSelected.remove(feature);
					featuresMatched.remove(feature);
				});
			}
			$td.addClass("click").toggleClass("selected");
			anySelected = featuresSelected.toggle(feature).has();
			// Even if you click to clear a feature, it remains active
			// until the mouse moves out of the cell.
			featuresMatched.add(feature);
			this.$thFeatures.toggleClass("selected", anySelected);
			if (this.hasChanges) {
				// While distinctive feature values are selected, changes are active.
				$element.toggleClass("changeable", anySelected);
				// When the last selected value is cleared, clear any selected changes.
				if (!anySelected) {
					this._clickChangesHeading();
				}
				this._dependentChanges();
			}
		},

		// Click to select or clear a change.
		_clickChange: function ($td) {
			var changesSelected = this.changesSelected,
				widgetName = this.widgetName,
				anySelected;

			// While distinctive feature values are selected at the left of the table,
			// changes at the right are active (except changes implied by dependencies).
			if (this.featuresSelected.has() &&
					!($td.hasClass("then")) && $td.siblings('.then').length === 0) {
				// If contour values are unavailable, before this change becomes selected,
				// if the opposite value was selected, clear it.
				if (!this.contour && !($td.hasClass("selected"))) {
					$td.siblings('.selected.change').each(function () {
						var feature = $(this).removeClass("selected").data(widgetName).feature;

						changesSelected.remove(feature);
					});
				}

				$td.addClass("click").toggleClass("selected");
				anySelected = changesSelected.toggle($td.data(widgetName).feature).has();
				this.$thChanges.toggleClass("changed", anySelected);
				this.$tfoot.toggleClass("changed", anySelected);
				this._dependentChanges();
			}
		},

		// To clear all selected changes in the table, click at the right of its heading.
		_clickChangesHeading: function () {
			this.$tdsChanges.removeClass("selected");
			this.$thChanges.removeClass("changed");
			this.$tfoot.removeClass("changed");
			this.changesSelected.remove();
			this.changesSelectedOrDependent.remove();
			this._emptyObject(this.dependentFeaturesIf);
			this._emptyObject(this.dependentChangesIf);
			this._emptyObject(this.dependentChangesThen);
		},

		// To clear all selected features and changes in the table, click at the left of its heading.
		_clickMainHeading: function () {
			this.$tdsFeatures.removeClass("selected");
			this.$thFeatures.removeClass("selected");
			this.featuresSelected.remove();
			this.featuresMatched.remove();
			if (this.hasChanges) {
				this.element.removeClass("changeable");
				this._clickChangesHeading();
			}
		},

		// publish features ----------------------------------------------------

		// In related CV charts, highlight segments:
		// * which have all selected and active features
		// * which have the selected changes, if any
		// * for which no corresponding segments in the language have the selected changes
		_publishFeatures: function () {
			this.publishFeatures(this.featuresMatched.has() && this.visitorMatched,
				this.hasChanges && this.changesSelected.has() && this.visitorChanged);
		},

		// A visitor represents an operation to be performed on the elements
		// of an object structure (in this case, segments in CV charts).
		// See pages 331-344 in Design Patterns.
		// The actual visitor functions bind the following private implementations to this.

		// Return whether features of a segment match selected and active features.
		// Determine whether any selected features are redundant.
		_visitorMatched: function (segment) {
			// Accumulate state for feedback about redundant features.
			if (this.distinctive && this.featuresSelected.has()) {
				this._segmentRedundant(segment);
			}

			return $.data(segment, this.featureClass).has(this.featuresMatched);
		},

		// Return a key for a segment from the values of its distinctive features,
		// with changes (representing a segment which it changes to).
		_visitorChanged: function (segment) {
			return this.objectSegmentChanges[this._keyFeaturesChanges(segment, this.changesSelectedOrDependent)];
		},

		// dependencies --------------------------------------------------------

		// Map a list of pairs of lists of features to an array of objects,
		// each of which contains if and then properties whose values are features objects.
		// {"if": {"son": "-", "cont": "+"}, then: {"delayed": "+"}, "title": "-son +cont > +delayed"}.
		// {"if": {"son": "+"}, "then": {"delayed": "0"}, "title": "+son > 0delayed"}
		_createDependencies: function ($ulDependencies) {
			var featureClass = this.featureClass,
				featureQuery = this.featureQuery,
				namesObject = this.namesObject,
				namesArray = this.namesArray,
				dependencies = this.dependencies = [];

			$ulDependencies.children().each(function () {
				var $li = $(this),
					dependencyIf = featureQuery(featureClass),
					dependencyThen = featureQuery(featureClass);

				$li.children('ul.if').children().each(function () {
					dependencyIf.add(textFromListItem(this));
				});
				$li.children('ul.then').children().each(function () {
					dependencyThen.add(textFromListItem(this));
				});
				if (dependencyIf.relevantFor(namesObject) && dependencyThen.relevantFor(namesObject)) {
					dependencies.push({
						"if": dependencyIf, // if is a reserved word in JavaScript
						then: dependencyThen,
						title: "dependency: " + dependencyIf.join(" ", namesArray) + " > " +
							dependencyThen.join(" ", namesArray)
					});
				}
			});

			this.feature_td0 = {};
			this._createUnspecifieds(dependencies, "then");
			this._createUnspecifieds(dependencies, "if"); // if must follow then
		},

		// Cache dependency features which have unspecified "0" values.
		// Must precede _createChanges, which replaces the key values
		// with table data elements under changes.
		_createUnspecifieds: function (dependencies, which) {
			var namesArray = this.namesArray,
				feature_td0 = this.feature_td0;

			$.each(dependencies, function () {
				this[which].each(function (feature, name, value) {
					if (value === "0" && $.inArray(name, namesArray) !== -1) {
						feature_td0[feature] = which;
					}
				});
			});
		},

		// For any click to select or clear a feature or a change
		// while any changes are selected, evaluate dependent changes.
		_dependentChanges: function () {
			var featuresSelected = this.featuresSelected,
				changesSelected = this.changesSelected,
				changesSelectedOrDependent = this.changesSelectedOrDependent,
				dependentFeaturesIf = this.dependentFeaturesIf,
				dependentChangesIf = this.dependentChangesIf,
				dependentChangesThen = this.dependentChangesThen;

			changesSelectedOrDependent.remove().add(changesSelected);
			this._emptyObject(dependentFeaturesIf);
			this._emptyObject(dependentChangesIf);
			this._emptyObject(dependentChangesThen);
			$.each(this.dependencies, function () {
				var dependency = this,
					dependencyIf = dependency["if"], // if is a reserved word in JavaScript
					dependencyThen = dependency.then,
					dependencyTitle = dependency.title,
					boolIf = false,
					boolIfChanges = false;

				dependencyIf.each(function (feature) {
					if (changesSelected.has(feature)) {
						boolIf = boolIfChanges = true;
					} else {
						boolIf = featuresSelected.has(feature);
					}
					return boolIf; // break if false
				});

				if (boolIf && boolIfChanges) {
					dependencyIf.each(function (feature) {
						if (changesSelected.has(feature)) {
							dependentChangesIf[feature] = dependencyTitle;
						} else {
							dependentFeaturesIf[feature] = dependencyTitle;
						}
					});
					dependencyThen.each(function (feature) {
						dependentChangesThen[feature] = dependencyTitle;
					});
					changesSelectedOrDependent.add(dependencyThen);
					// Important: _feedbackChanges must remove selected changes implied by dependencies.
				}
			});
		},

		// Display the selected features and changes as a rule
		// which people can copy from the page.
		_displayChanges: function () {
			var featuresSelected = this.featuresSelected,
				namesArray = this.namesArray;

			this.$spanChanges.text(featuresSelected.has() &&
				this.changesSelected.has() ?
						"[" + featuresSelected.join(", ", namesArray) + "] \u2192 [" +
						this.changesSelectedOrDependent.join(", ", namesArray) + "]" : "");
		},

		// callback handlers ---------------------------------------------------

		// Initialize object which refers by key to each segment in related charts.
		_subscribeSegmentChanges: function (td) {
			var key = this.keyFeatures(td),
				value = this.objectSegmentChanges[key];

			if (value) {
				value.push(td); // append to existing list
			} else {
				this.objectSegmentChanges[key] = [td];
			}
		},

		_subscribeSegment: function (tdActive, tdSelected) {
			var featureClass = this.featureClass;

			this._toggleClass(tdActive && $.data(tdActive, featureClass),
				tdSelected && tdSelected !== tdActive && $.data(tdSelected, featureClass));
		},

		// Toggle classes for interactive feature cells.
		_toggleClass: function (featuresActiveSegment, featuresSelectedSegment) {
			var compareSelected = featuresSelectedSegment &&
					featuresSelectedSegment !== featuresActiveSegment,
				widgetName = this.widgetName;

			this.$tdsFeatures.each(function () {
				var matched = false,
					active = false,
					selected = false,
					feature,
					isActive,
					isSelected;

				if (featuresActiveSegment) {
					feature = $.data(this, widgetName).feature;
					isActive = featuresActiveSegment.has(feature);
					if (compareSelected) {
						isSelected = featuresSelectedSegment.has(feature);
						matched = isActive && isSelected;
						if (!matched) {
							active = isActive;
							selected = isSelected;
						}
					} else {
						matched = isActive;
					}
				}
				$(this).toggleClass("matched", matched)
					.toggleClass("active", active)
					.toggleClass("selected", selected);
			});
		},

		// feedback ------------------------------------------------------------

		// When you click in the table of distinctive features,
		// it might highlight values to give feedback.
		// If you point at such a highlighted value,
		// a brief explanation appears in a screen tip.
		_feedbackDistinctive: function () {
			var dependentFeaturesIf = this.dependentFeaturesIf,
				feedbackContradictions = this._feedbackContradictions(),
				feedbackRedundant = this._feedbackRedundant(),
				widgetName = this.widgetName;

			// Update feedback classes of interactive cells for features.
			this.$tdsFeatures.each(function () {
				var $td = $(this),
					selected = $td.hasClass("selected"),
					feature = $td.data(widgetName).feature,
					titleIf = dependentFeaturesIf && dependentFeaturesIf[feature], // hasChanges
					titleContradiction = feedbackContradictions[feature],
					titleRedundant = feedbackRedundant[feature];

				$td.attr("title", titleIf ||
						(selected && (titleContradiction || titleRedundant)) ||
						null)
					.toggleClass("if", !!titleIf)
					.toggleClass("contradiction", selected && !!titleContradiction)
					.toggleClass("redundancy", selected && !!titleRedundant);
			});

			if (this.hasChanges) {
				this._feedbackChanges();
			}
		},

		_feedbackChanges: function () {
			var dependentChangesIf = this.dependentChangesIf,
				dependentChangesThen = this.dependentChangesThen,
				changesSelected = this.changesSelected,
				feedbackContradictions = this._feedbackContradictions(),
				feedbackRedundant = this._feedbackRedundant(),
				widgetName = this.widgetName;

			// Update feedback classes of interactive cells for changes.
			this.$tdsChanges.each(function () {
				var $td = $(this),
					selected = $td.hasClass("selected"),
					feature = $td.data(widgetName).feature,
					titleIf = dependentChangesIf[feature],
					titleThen = dependentChangesThen[feature],
					titleContradiction = feedbackContradictions[feature],
					titleRedundant = feedbackRedundant[feature];

				// Important: remove selected changes implied by dependencies.
				if (selected && !!titleThen) {
					selected = false;
					changesSelected.remove(feature);
					$td.removeClass("selected");
				}

				$td.attr("title", titleIf ||
						titleThen ||
						(selected && (titleContradiction || titleRedundant)) ||
						null)
					.toggleClass("if", !!titleIf)
					.toggleClass("then", !!titleThen)
					.toggleClass("contradiction", selected && !!titleContradiction)
					.toggleClass("redundancy", selected && !!titleRedundant);
			});

			// Update feedback class of non-interactive cells
			// for dependent unspecified (zero) values.
			$.each(this.feature_td0, function (feature, td0) {
				var titleThen = dependentChangesThen[feature] || null;

				$(td0).attr("title", titleThen).toggleClass("then", !!titleThen);
			});

			this._displayChanges();
		},

		// feedback contradictions ---------------------------------------------

		// Map a list of lists of distinctive features to an array of objects,
		// each of which contains a contradictory set of feature values.
		// For example, [-son, +approx].
		_createContradictions: function ($ulContradictions) {
			var featureClass = this.featureClass,
				featureQuery = this.featureQuery,
				namesObject = this.namesObject,
				namesArray = this.namesArray,
				contradictions = this.contradictions = [];

			$ulContradictions.children().each(function () {
				var features = featureQuery(featureClass);

				$(this).children().children().each(function () {
					features.add(textFromListItem(this));
				});
				if (features.relevantFor(namesObject)) {
					contradictions.push({
						features: features,
						title: "contradiction: " + features.join(" ", namesArray)
					});
				}
			});
		},

		// Return an object whose keys are features and values are titles
		// which describe the corresponding contradictions, if any.
		_feedbackContradictions: function () {
			var featuresSelected = this.featuresSelected,
				hasChanges = this.hasChanges,
				changesSelectedOrDependent = this.changesSelectedOrDependent,
				feedbackContradictions = {};

			if (featuresSelected.length) {
				$.each(this.contradictions, function () {
					var features = this.features,
						title;

					if (features.every(function (feature) {
							return featuresSelected.has(feature) ||
								(hasChanges && changesSelectedOrDependent.has(feature));
						})) {
						// The contradictory set of features are a subset of
						// the selected features, selected changes, and dependent changes?
						title = this.title;
						features.each(function (feature) {
							feedbackContradictions[feature] = title;
						});
					}
				});
			}
			return feedbackContradictions;
		},

		// feedback redundant --------------------------------------------------

		// When there is a click in a table of features, initialize before publishing.
		// Array indexes for vacuous or redundant features correspond to selected features.
		_feedbackDistinctiveInit: function () {
			var featuresRedundant = this.featuresRedundant,
				featuresSelected = this.featuresSelected;

			featuresRedundant.remove();
			if (featuresSelected.length() > 1) {
				featuresRedundant.add(featuresSelected);
			}

			if (this.hasChanges) {
				this._initRedundantChanges();
			}
		},

		// When there is a click in a table of features, initialize before publishing.
		// Array indexes for vacuous or redundant features correspond to selected features.
		_initRedundantChanges: function () {
			var featuresVacuous = this.featuresVacuous,
				featuresSelected = this.featuresSelected,
				changesSelectedOrDependent = this.changesSelectedOrDependent;

			// If a selected feature and a selected change have opposite values,
			// it is usually considered redundant by the principle of vacuous application.
			featuresVacuous.remove();
			featuresSelected.each(function (feature, name, char) {
				if (changesSelectedOrDependent.charInternalOpposite(name) === char) {
					featuresVacuous.add(feature);
				}
			});
		},

		// Accumulate state for feedback about redundant features during publishing.
		_segmentRedundant: function (segment) {
			var features = $.data(segment, this.featureClass),
				featuresVacuous = this.featuresVacuous,
				featuresRedundant = this.featuresRedundant,
				self = this;

			// A selected feature value is redundant
			// if every segment which has all the other selected values also has it.
			featuresRedundant.each(function (feature) {
				if (self._hasSelectedFeaturesIgnoring1(features, feature) &&
						!features.has(feature)) {
					featuresRedundant.remove(feature);
				}
			});

			if (this.hasChanges) {
				// If a selected feature and a selected change have opposite values,
				// it is not considered redundant if the feature is unspecified for some segments.
				featuresVacuous.each(function (feature, name) {
					if (self._hasSelectedFeaturesIgnoring1(features, feature) &&
							!features.definedFor(name)) {
						featuresVacuous.remove(feature);
					}
				});
			}
		},

		// Ignoring one selected feature identified by its index in the subset,
		// return whether all the others are a subset of the features (of a segment).
		_hasSelectedFeaturesIgnoring1: function (features, featureIgnoring) {
			return this.featuresSelected.every(function (feature) {
				return feature === featureIgnoring || features.has(feature);
			});
		},

		// Return an object whose keys are features and values are titles
		// which describe the corresponding redundancy, if any.
		_feedbackRedundant: function () {
			var hasChanges = this.hasChanges,
				featuresVacuous = this.featuresVacuous,
				featuresRedundant = this.featuresRedundant,
				feedbackRedundant = {};

			this.featuresSelected.each(function (feature) {
				if (hasChanges && featuresVacuous.has(feature)) {
					feedbackRedundant[feature] = "considered redundant by the principle of vacuous application";
				} else if (featuresRedundant.has(feature)) {
					feedbackRedundant[feature] = "redundant because every segment which has all the other selected values also has this value";
				}
			});
			return feedbackRedundant;
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

		// Initialize the data object for an interactive data cell.
		// Return false to filter out non-interactive empty cells.
		_initData: function (td, click) {
			var $td = $(td),
				text = $td.text(),
				value,
				feature;

			if (this.distinctive) {
				value = this.value_td_li[text];
				if (value) {
					// Important: cloned cells for changes must already be in the row.
					feature = this.featuresSelected.s_feature($td.siblings(this.selectorNames).text(), value);
				} else {
					return false;
				}
			} else {
				feature = text;
			}
			$td.data(this.widgetName, {feature: feature, click: click});
			return true;
		},

		// Delete properties of object.
		_emptyObject: function (object) {
			$.each(object, function (key) {
				delete object[key];
			});
			return object;
		},

		// Return a key for a segment from the values of its distinctive features,
		// optionally with changes (representing a segment which it changes to).
		_keyFeaturesChanges: function (segment, changes) {
			return $.data(segment, this.featureClass).key(this.namesArray, changes);
		},

		// public interface ----------------------------------------------------

		// Return a key for a segment from the values of its distinctive features.
		keyFeatures: function (segment) {
			return this._keyFeaturesChanges(segment);
		}
	});

	// =========================================================================

	// CV chart is a table of consonant or vowel segments.
	$.widget("phonology.tableCV", $.phonology.counteractivepart, {
		_create: function () {
			var $element = this.element,
				$tdsSegment = this.$tdsSegment = $element.find('td.Phonetic'),
				nDistinctive = 0,
				callbacksSegment = this._callbacksProxy("segment", "all"),
				publishSegmentChanges = this._callbacksProxy("segment_changes", "all").fire;

			// If no segment is selected in this or any related chart,
			// use false as the falsy value instead of undefined or null.
			this.tdSelected = false;

			// Display the total number of segments in the upper left heading cell.
			// Also display the number matched when there are active or selected features.
			this.$spanMatched = $element.find('thead th:first-child').addClass("value")
				.append($('<div><div><span></span><span></span></div></div>'))
				.find('span').eq(1).text($tdsSegment.length)
				.end().eq(0);

			$tdsSegment.each(function () {
				dataSegment.call(this); // store features of segments as data
				publishSegmentChanges(this); // must follow the preceding
			});

			// If all segments have distinctive features, then add callback
			// to indicate which segments are similar to the active segment.
			$tdsSegment.each(function () { // must follow the preceding
				if ($.data(this, "distinctive")) {
					nDistinctive += 1;
				}
			});
			if ($tdsSegment.length === nDistinctive) {
				callbacksSegment.add(bind.call(this._subscribeSegment, this));
				//$tdsSegment.each(function (i) {
				//	var dataA = $.data(this),
				//		distinctiveA = dataA.distinctive,
				//		symbolsA = dataA.symbols;

				//	$tdsSegment.slice(i + 1).each(function () {
				//		var dataB = $.data(this),
				//			similarity = distinctiveA.similarity(dataB.distinctive);

				//		if (similarity !== "least") {
				//			console.log(symbolsA, dataB.symbols, similarity);
				//		}
				//	});
				//});
			}

			this.publishSegment = callbacksSegment.fire;
			this.publishSegmentSelected = this._callbacksProxy("segment_selected", "farthest")
				.add(bind.call(this._subscribeSegmentSelected, this)).fire;
			this._callbacksProxy("features", "all").add(bind.call(this._subscribeFeatures, this));

			// This CV chart is a counterpart in interactive behavior to related features tables.
			// When it is active (enabled), they are inactive (disabled), and vice versa.
			this._counteractivepart({"segment_active": "farthest"}, {"features_active": "farthest"},
				{
					"mouseenter": this._mouseenterSegment,
					"mouseleave": this._mouseleaveSegment
				},
				$tdsSegment); // attach event handlers to these interactive descendants
		},

		// protected interface -------------------------------------------------

		// When the mouse pointer moves out of this chart,
		// the counteractivepart base class needs to know whether a segment is still selected.
		_counteractive: function () {
			return !!this.tdSelected;
		},

		// event handlers ------------------------------------------------------

		// Click in a CV chart.
		// If you click a segment which is not already selected, select it.
		// Any other click in a related chart clears the selection.
		// Use false as the falsy value instead of undefined or null.
		_click: function (event) {
			var $td = $(event.target).closest('th, td').filter('td.Phonetic'),
				td = $td.get(0) || false,
				tdSelected = this.tdSelected;

			if (tdSelected) {
				$(tdSelected).removeClass("selected"); // even if it is in a related chart
			}
			tdSelected = this.tdSelected = tdSelected !== td && td;
			if (tdSelected) {
				$td.addClass("selected");
			}
			this.publishSegmentSelected(tdSelected); // must precede the following

			// If you click the selected segment to clear it,
			// it remains active until the mouse pointer moves out of the cell.
			this.publishSegment(td);
		},

		// When the mouse pointer moves into a segment cell,
		// highlight the features of the active segment and optional selected segment.
		_mouseenterSegment: function (event) {
			this.publishSegment($(event.target).closest('td').get(0), this.tdSelected);
		},

		// When the mouse point moves out of a segment cell,
		// highlight the features of the optional selected segment.
		_mouseleaveSegment: function () {
			this.publishSegment(this.tdSelected);
		},

		// callback handlers ---------------------------------------------------

		// Callback for related charts to know when the segment changes.
		_subscribeSegment: function (tdActive) {
			var tdSelected = this.tdSelected;

			this._toggleSimilar(tdActive, tdActive &&
				(tdActive === tdSelected || !tdSelected) &&
				$.data(tdActive, "distinctive"));
		},

		// Callback for related charts to know when the selection changes.
		_subscribeSegmentSelected: function (tdSelected) {
			if (this.tdSelected !== tdSelected) {
				// Either no segment selected
				// or a segment is selected in a related chart.
				this.tdSelected = tdSelected;
				this.publishSegment(false);
			}
		},

		// Callback for active, selected, or changed features.
		_subscribeFeatures: function (visitorMatched, visitorChanged) {
			var segmentsMatched = visitorChanged && {};

			this._toggleMatched(visitorMatched, segmentsMatched);
			if (visitorChanged || this.toggleChangedPrev) {
				this._toggleChanged(visitorChanged, segmentsMatched);
				this.toggleChangedPrev = !!visitorChanged;
			}
		},

		// private implementation for callback handlers ------------------------

		similarity_classes: {
			"more": "more-similar",
			"less": "less-similar"
		},

		// Toggle classes to indicate which segments are similar to the active segment.
		_toggleSimilar: function (tdActive, featuresActive) {
			var similarity_classes = this.similarity_classes;

			this.$tdsSegment.each(function () {
				var $td = $(this),
					similarity = featuresActive && tdActive !== this &&
						$.data(this, "distinctive").similarity(featuresActive);

				$.each(similarity_classes, function (key, value) {
					$td.toggleClass(value, key === similarity);
				});
			});
		},

		// Highlight segments which match all active or selected features.
		_toggleMatched: function (visitorMatched, segmentsMatched) {
			var nMatched = 0;

			this.$tdsSegment.each(function () {
				var isMatched = visitorMatched && visitorMatched(this);

				$(this).toggleClass("matched", isMatched);
				if (isMatched) {
					nMatched += 1;
					if (segmentsMatched) {
						segmentsMatched[$.data(this, "symbols")] = this;
					}
				}
			});
			this.$spanMatched.text(visitorMatched ? nMatched + " / " : "");
		},

		// Highlight and display segments which correspond to changed features.
		_toggleChanged: function (visitorChanged, segmentsMatched) {
			var segmentsChangedFrom = {},
				segmentsChangedTo = {},
				self = this;

			if (visitorChanged) {
				$.each(segmentsMatched, function (symbols, segment) {
					var segments = visitorChanged(segment);

					// Given a matched segment and any segments changed from its features:
					// * Cache any segments changed from it.
					// * Cache it for any segments changed to it.
					if (segments) {
						// For the active segment, cache any segments (possibly including itself)
						// which are changed from it.
						append(self._arrayChanged(segmentsChangedFrom, symbols), segments);

						// Cache the active segment for any segments (possible including itself)
						// which are changed to it.
						$.each(segments, function () {
							self._arrayChanged(segmentsChangedTo, $.data(this, "symbols")).push(segment);
						});
					}
				});
			}

			this.$tdsSegment.each(function () {
				var $td = $(this),
					$div = $td.children('div.changes'),
					$ul = $div.children('ul'),
					symbols = $.data(this, "symbols"),
					segments = visitorChanged && segmentsChangedTo[symbols],
					isChanged = !!segments;

				// If you point at a segment, segments which change to it appear at its left.
				if (isChanged) {
					if ($div.length === 0) {
						$div = $('<div class="changes"></div>').prependTo(this);
					}
					if ($ul.length === 0) {
						$ul = $('<ul></ul>').appendTo($div);
					} else {
						$ul.children().remove();
					}
					$.each(segments, function () {
						$('<li></li>').attr("title", $(this).attr("title"))
							.text($.data(this, "symbols")).appendTo($ul);
					});
					$('<li></li>').text("\u2192").appendTo($ul); // rightwards arrow
				} else {
					$ul.remove();
				}

				// Highlight segments which are affected by changed features.
				if (visitorChanged) {
					$td.toggleClass("changed", isChanged)
						.toggleClass("alone", isChanged &&
							segments.length === 1 && segments[0] === this)
						.toggleClass("unchanged", !!segmentsMatched[symbols] &&
							!segmentsChangedFrom[symbols]);
				} else {
					$td.removeClass("changed alone unchanged");
				}
			});
		},

		_arrayChanged: function (object, key) {
			var value = object[key];

			if (!value) {
				value = object[key] = [];
			}
			return value;
		}
	});

	// diagram for articulatory phonetics ======================================

	$.widget("phonology.diagram", $.phonology.mediatee, {
		_create: function () {
			var $element = this.element;

			// A face diagram is also known as a sagittal section of the vocal tract.
			this.$switches = this._$switchesFeatures($element.find('.sagittal'));
			this._createSegments($element);
			this._callbacksProxy("segment").add(bind.call(this._subscribeSegment, this));
		},

		// A diagram can identify a segment and an optional compared segment. See displayData.
		_createSegments: function ($element) {
			var $segments = $element.children('p.segment'),
				$segmentA = this.$segmentA = $segments.not('.compared'),
				$segmentB = $segments.filter('.compared');

			this.$symbolsA = $segmentA.find('.Phonetic');
			this.$descriptionA = $segmentA.find('.description');
			this.$symbolsB = $segmentB.find('.Phonetic');
			this.$descriptionB = $segmentB.find('.description');
		},

		featureClass: "descriptive",

		// Return the conditional graphical elements of a diagram. See displayFeatures.
		// In JavaScript, a switch statement consists of cases.
		// In the markup for a diagram, svg.switch elements contain g elements.
		// <svg class="switch">
		// <text><tspan>palatal</tspan></text>
		// <g><path class="less" d="M ..."><desc>tongue blade lower</desc></path></g>
		// <g class="break"><text><tspan>approximant</tspan></text><path ...></g>
		// <g><text><tspan>affricate</tspan></text><text><tspan>fricative</tspan></text><path ...></g>
		// <g>...<text class="contour"><tspan>affricate></tspan></text>...<path ...></g>
		// </svg>
		_$switchesFeatures: function ($element) {
			var featureQueryClass = bind.call(phonQuery.featureQuery, null, this.featureClass),
				self = this;

			// Use attribute instead of class selector because, at least through jQuery 1.7,
			// neither the class selector nor the hasClass method work for SVG elements.
			return $element.find('svg[class~="switch"]').each(function () {
				$.data(this, "featuresArray", self._featuresArray(this, featureQueryClass));
				$.data(this, "$cases", $(this).children('g').each(function () {
					$.data(this, "featuresArray", self._featuresArray(this, featureQueryClass));
					// Similar to JavaScript, a case element "falls through" to the following
					// unless its class attribute indicates a break.
					// <g><path class="less" d="M ..."><desc>tongue blade lower</desc></path></g>
					// <g class="break"><text><tspan>approximant</tspan></text><path ...></g>
					// Important: displayFeatures will replace the class attribute.
					$.data(this, "fallThrough", !($(this).is('[class~="break"]')));
				}));
			});
		},

		// Return an array of features objects for a switch or case element. See _featuresMatched.
		_featuresArray: function (element, featureQueryClass) {
			var featuresElement = this._featuresElement, // method does not depend on this
				featuresArray = [];

			// Each text element corresponds to an alternative.
			// <g><text><tspan>affricate</tspan></text><text><tspan>fricative</tspan></text><path ...></g>
			$(element).children('text').each(function () {
				featuresArray.push(featuresElement(this, featureQueryClass));
			});

			// If there are no text elements, then any segment matches an "empty" features object.
			// <g><path class="less" d="M ..."><desc>tongue blade lower</desc></path></g>
			if (featuresArray.length === 0) {
				featuresArray.push(featuresElement(null, featureQueryClass));
			}

			return featuresArray;
		},

		// Return a features object for a child text element of a switch or case element.
		_featuresElement: function (element, featureQueryClass) {
			var $element = $(element),
				diagramClass = $element.attr("class"),
				features = featureQueryClass();

			// Each tspan element corresponds to a feature.
			// <text><tspan>lateral</tspan><tspan>approximant</tspan></text>
			$element.children('tspan').each(function () {
				features.add($(this).text());
			});

			// If the text element has a class attribute, then when its features object is matched,
			// add its class to the class attribute of its parent case element. See _caseClasses.
			// This reduces repetition of graphical elements. For example, an affricate has the same path
			// as other stops, but the contour class indicates a visual distinction: dashed line.
			// <g>...<text class="contour"><tspan>affricate></tspan></text>...<path ...></g>
			if (diagramClass) {
				features.diagramClass = diagramClass; // see _diagramClass
			}

			return features;
		},

		// private implementation ----------------------------------------------

		// Return as a booly value whether a segment matches a switch or case element:
		// * the first object in the array for which the segment has its features;
		// * otherwise undefined.
		_featuresMatched: function (featuresSegment, featuresArray) {
			var i, length;

			for (i = 0, length = featuresArray.length; i < length; i += 1) {
				if (featuresSegment.has(featuresArray[i])) {
					return featuresArray[i];
				}
			}
		},

		// Return the required and optional classes for a case element.
		_caseClasses: function (caseA, caseB, comparing) {
			var requiredClass, optionalClass, optionalClassA, optionalClassB;

			if (caseA) {
				if (caseB) {
					requiredClass = "matched";
					optionalClassA = this._diagramClass(caseA);
					optionalClassB = this._diagramClass(caseB);
					if (optionalClassA === optionalClassB) { // only if both have the same class
						optionalClass = optionalClassA; // for example, prenasalized
					}
				} else {
					requiredClass = comparing ? "comparedA" : "matched";
					optionalClass = this._diagramClass(caseA);
				}
			} else if (caseB) {
				requiredClass = "comparedB";
				optionalClass = this._diagramClass(caseB);
			} else {
				return null; // attr("class", null) means removeAttr("class")
			}

			if (optionalClass && typeof optionalClass === "string") {
				return requiredClass + " " + optionalClass;
			}

			return requiredClass;
		},

		_diagramClass: function (features) {
			if (features && typeof features === "object") {
				return features.diagramClass;
			}
		},

		// Replace the text of the element with the property of the data object;
		// otherwise the empty string.
		_text: function ($element, data, key) {
			$element.text((data && data[key]) || "");
		},

		// callback handlers ---------------------------------------------------

		// For loose coupling between a diagram and one or more widgets that publish segments:
		// Display the diagram for the active segment, selected versus active segments, or neither.
		_subscribeSegment: function (segmentActive, segmentSelected) {
			if (segmentSelected && segmentSelected !== segmentActive && segmentActive) {
				this.displaySegment(segmentSelected, segmentActive);
			} else {
				this.displaySegment(segmentActive);
			}
		},

		// public interface ----------------------------------------------------

		// Display the diagram for segmentA, with optional compared segmentB, or neither.
		// A segment is a DOM element (for example, a table cell) that has associated data.
		displaySegment: function (segmentA, segmentB) {
			this.displayData(segmentA && $.data(segmentA),
				segmentB && segmentB !== segmentA && segmentA && $.data(segmentB));
		},

		// Display the diagram for dataA, with optional compared dataB, or neither.
		displayData: function (dataA, dataB) {
			var comparing = !!dataB && dataB !== dataA && !!dataA, // boolean for toggleClass
				featureClass = this.featureClass;

			this.displayFeatures(dataA && dataA[featureClass], comparing && dataB[featureClass]);
			this.$segmentA.toggleClass("comparedA", comparing);
			this._text(this.$symbolsA, dataA, "symbols");
			this._text(this.$descriptionA, dataA, "description");
			this._text(this.$symbolsB, dataB, "symbols");
			this._text(this.$descriptionB, dataB, "description");
		},

		// Display the diagram for featuresA, with optional compared featuresB, or neither.
		displayFeatures: function (featuresA, featuresB) {
			var comparing = featuresB && featuresB !== featuresA && featuresA,
				self = this;

			// Update the class attribute for each case of each switch.
			this.$switches.each(function () {
				var featuresArray = featuresA && $.data(this, "featuresArray"),
					switchA = featuresA && self._featuresMatched(featuresA, featuresArray),
					switchB = comparing && self._featuresMatched(featuresB, featuresArray);

				// In parallel for featuresA and featuresB:
				// * If all cases have a break, display at most one.
				// * If any cases fall through, possibly display more than one.
				$.data(this, "$cases").each(function () {
					var reached = switchA || switchB,
						fallThrough = reached && $.data(this, "fallThrough"),
						featuresArray = reached && $.data(this, "featuresArray"),
						caseA = switchA && self._featuresMatched(featuresA, featuresArray),
						caseB = switchB && self._featuresMatched(featuresB, featuresArray);

					// Replace or remove the class attribute because, at least through jQuery 1.7,
					// none of the following work for SVG elements: addClass, removeClass, toggleClass.
					$(this).attr("class", self._caseClasses(caseA, caseB, comparing));

					if (caseA) { // if reached this case for featuresA,
						switchA = fallThrough; // then unless break, fall though to the next
					}
					if (caseB) { // if reached this case for featuresB,
						switchB = fallThrough; // then unless break, fall though to the next
					}
				});
			});
		}
	});

	// charts of distinctive feature values ====================================

	// Several charts can optionally follow a CV chart and feature tables.
	// * distinctive-segment: features in rows and segments in columns
	//   consonants by place of articulation
	//   vowels by backness
	// * segment-distinctive: segments in rows and features in columns
	//   consonants by manner of articulation
	//   vowels by height
	//   transposed orientation when there are too many segments for distinctive-segment
	//
	// * descriptive-distinctive
	// * distinctive-descriptive
	// * height-backness
	// * dorsal-backness

	$.widget("phonology.rowgroupsValues", $.phonology.mediatee, {
		_create: function () {
			var $element = this.element, // thead or tfoot
				rowgroupClasses = splitClass($element),
				tableClasses = splitClass($element.parent()),
				dual = $.inArray("dual", tableClasses) !== -1,
				distinctiveCols = (dual ? rowgroupClasses[1] : tableClasses[0].split("-")[1]) === "distinctive",
				$tbodys = $element.siblings('tbody' + (dual ? '.' + rowgroupClasses[0] : '')),
				$rowgroupsDistinctive = this.$rowgroupsDistinctive = distinctiveCols ? $element : $tbodys,
				$rowgroupsCounterpart = this.$rowgroupsCounterpart = distinctiveCols ? $tbodys : $element,
				$thsCounterpart = this.$thsCounterpart = $rowgroupsCounterpart.find('th[scope]').not('[scope="rowgroup"]'),
				valueQuery = phonQuery.valueQuery,
				value = this.value = valueQuery("+", "+-0"),
				regexpUnspecified = /[\s\u00B7\u2022]/, // explicitly unspecified: white space, middle dot, bullet
				addClassDistinctive = this.addClassDistinctive = [],
				tdsCounterpartRows = [],
				selectorData = 'td:not(.Phonetic)',
				nDescriptive = 0,
				eventNamespace = "." + this.widgetName,
				self = this;

			// Interactive for 141 segments of !Xu language (ISO-639-3 code nmn) in UPSID
			// or in the Hayes spreadsheet, but not larger multilingual projects.
			if ($thsCounterpart.length >= 150) {
				return;
			}

			this.addClassCounterpartData = [];
			this.addClassCounterpartHeading = [];

			// Attach event handlers for distinctive feature heading cells.
			if (distinctiveCols) {
				$rowgroupsCounterpart.children('tr').each(function () {
					tdsCounterpartRows.push($(this).children(selectorData).get());
				});
			}
			this.$thsDistinctive = $rowgroupsDistinctive.find('th[scope]').not('[scope="colgroup"]').each(function (i) {
				var $th = $(this),
					$tds = distinctiveCols ? $($.map(tdsCounterpartRows, function (tds) {
						return tds[i];
					})) : $th.siblings(selectorData);

				// Initialize values of data cells.
				$tds.each(function () {
					var text = $(this).text();

					// Unspecified is explicit for data cells in values charts.
					$.data(this, "values", valueQuery(text ? text.replace(regexpUnspecified, "0") : "0"));
				});
				$.data(this, "$tds", $tds);

				addClassDistinctive[i] = [];

				if (!distinctiveCols && $th.parent().hasClass("redundant")) {
					$th.addClass("redundant"); // for JavaScript and CSS
				} else if (!($th.hasClass("redundant"))) {
					$th.addClass("interactive");
				}
			}).on("mouseenter" + eventNamespace, function (event) {
				self._mouseenterDistinctive(event);
			}).on("mouseleave" + eventNamespace, function (event) {
				self._mouseleaveDistinctive(event);
			});

			// TO DO: If there is a mediator for segment publishers and subscribers:
			$thsCounterpart.each(dataSegment); // store features of segments as data
			$thsCounterpart.each(function () { // must follow the preceding
				if ($.data(this, "descriptive")) {
					nDescriptive += 1;
				}
			});
			// If all segments have descriptive features, then add callback
			// to display the active segment in a diagram for articulatory phonetics.
			if ($thsCounterpart.length === nDescriptive) {
				this._publishSegment = this._callbacksProxy("segment", "all").fire;
			} else {
				this._publishSegment = $.noop; // empty function
			}

			// Attach event handlers for segment or descriptive feature heading cells.
			$thsCounterpart.on("mouseenter" + eventNamespace, function (event) {
				self._mouseenterCounterpart(event);
			}).on("mouseleave" + eventNamespace, function (event) {
				self._mouseleaveCounterpart(event);
			}).addClass("interactive");

			// To select which value to highlight when a feature is active,
			// click a small box in a corner of the chart.
			$element.find('th:not([scope])').addClass("value")
				.append('<div><div class="value"></div></div>')
				.find('div.value').on("click" + eventNamespace, function (event) {
					self._clickValue(event);
				}).attr("title", value.title()).text(value.charExternal());

			$rowgroupsDistinctive.addClass("interactive");
			$rowgroupsCounterpart.addClass("interactive");
			setTimeout(function () {
				self._createIndistinguishableHeadings(tableClasses[0].indexOf("segment") !== -1);
			}, 50);
		},

		// For each counterpart heading cell, cache the sibling cells
		// whose distinctive feature values are indistinguishable from its values.
		// * For descriptive feature heading cells in smaller charts possibly edited by hand,
		//   this function must determine which cells are indistinguishable.
		// * For segment heading cells in possibly larger charts exported by Phonology Assistant,
		//   this function assumes that the cells already have classes in the XHTML file,
		//   but it must determine which of these cells are related to each other.
		_createIndistinguishableHeadings: function (segment) {
			var $thsCounterpart = this.$thsCounterpart,
				$thsIndistinguishable = segment && $thsCounterpart.filter('.indistinguishable'),
				$thsThatFromThis = segment && $thsIndistinguishable.filter('.that_from_this'),
				$thsThisFromThat = segment && $thsIndistinguishable.filter('.this_from_that'),
				$thsDistinctive = this.$thsDistinctive,
				self = this;

			$thsCounterpart.each(function (index) {
				var $ths = segment && $thsThatFromThis.index(this) === -1 ? $() :
						$thsCounterpart.filter(function (i) {
							var distinguishable;

							if (i === index) {
								return false;
							}
							if (segment && $thsThisFromThat.index(this) === -1) {
								return false;
							}
							$thsDistinctive.each(function () {
								var $tds = $.data(this, "$tds");

								distinguishable = $.data($tds.get(i), "values")
									.distinguishableFrom($.data($tds.get(index), "values"));
								return !distinguishable; // break if distinguishable
							});
							return !distinguishable;
						});

				$.data(this, "$thsIndistinguishable", $ths);

				if ($ths.length && !segment) {
					self._createIndistinguishableHeading(this, "indistinguishable that_from_this");
					$ths.each(function () {
						self._createIndistinguishableHeading(this, "indistinguishable this_from_that");
					});
				}
			});
		},

		_createIndistinguishableHeading: function (th, className) {
			var $th = $(th).addClass(className);

			if ($th.has("span").length === 0) {
				$th.wrapInner("<span></span>"); // for CSS
			}
		},

		// event handlers ------------------------------------------------------

		// When the mouse pointer moves into a counterpart heading cell,
		// distinctive heading cells become inactive.
		_mouseenterCounterpart: function (event) {
			this.$rowgroupsDistinctive.removeClass("interactive");
			this._addClassCounterpart(event.currentTarget);
			this._publishSegment(event.currentTarget);
		},

		// When the mouse pointer moves out of a counterpart heading cell,
		// distinctive heading cells become interactive.
		_mouseleaveCounterpart: function (event) {
			this.$rowgroupsDistinctive.addClass("interactive");
			this._removeClassCounterpart(event.currentTarget);
			this._publishSegment(false);
		},

		// When the mouse pointer moves into a distinctive heading cell,
		// counterpart heading cells become inactive.
		_mouseenterDistinctive: function (event) {
			var th = event.currentTarget;

			this.$rowgroupsCounterpart.removeClass("interactive");
			if (!($(th).hasClass("redundant"))) {
				this._addClassDistinctive(th);
			}
		},

		// When the mouse pointer moves out of a distinctive heading cell,
		// counterpart heading cells become interactive.
		_mouseleaveDistinctive: function (event) {
			var th = event.currentTarget;

			this.$rowgroupsCounterpart.addClass("interactive");
			if (!($(th).hasClass("redundant"))) {
				this._removeClassDistinctive(th);
			}
		},

		// To specify which value to highlight, click a small box in a corner of the chart.
		// Each click selects the next value: plus, minus, unspecified, plus, and so on.
		_clickValue: function (event) {
			var value = this.value = this.value.next();

			$(event.currentTarget).attr("title", value.title()).text(value.charExternal());
		},

		// private implementation for event handlers ---------------------------

		// Add class to other data cells and counterpart heading cells.
		_addClassCounterpart: function (th) {
			var addClassDistinctive = this.addClassDistinctive,
				index = this.$thsCounterpart.index(th);

			// Mark all other data cells whose values differ from its values.
			this.$thsDistinctive.each(function (i) {
				var addClassCounterpart = addClassDistinctive[i],
					$tds = $.data(this, "$tds"),
					values = $.data($tds.get(index), "values");

				$tds.each(function (j) {
					if (j !== index && $.data(this, "values").distinguishableFrom(values)) {
						addClassCounterpart[j] = true;
						$(this).addClass("mark");
					}
				});
			});

			// Add class to other counterpart heading cells which are indistinguishable.
			$.data(th, "$thsIndistinguishable").addClass("active");
		},

		// Remove class from other data cells and counterpart heading cells.
		_removeClassCounterpart: function (th) {
			var addClassDistinctive = this.addClassDistinctive,
				self = this;

			this.$thsDistinctive.each(function (i) {
				self._removeClass("mark", $.data(this, "$tds"), addClassDistinctive[i]);
			});
			$.data(th, "$thsIndistinguishable").removeClass("active");
		},

		// Add class to corresponding data cells and counterpart heading cells.
		_addClassDistinctive: function (th) {
			var value = this.value,
				addClassCounterpartData = this.addClassCounterpartData,
				addClassCounterpartHeading = this.addClassCounterpartHeading,
				$thsCounterpart = this.$thsCounterpart;

			$.data(th, "$tds").each(function (i) {
				var values = $.data(this, "values"),
					thClass;

				if (values.has(value)) {
					// Mark corresponding data cells which have the specified value.
					// and their counterpart heading cells.
					addClassCounterpartData[i] = true;
					$(this).addClass(thClass = "mark");
				} else if (values.has("0")) {
					// Add class to any other counterpart heading cells
					// for which the value is unspecified.
					thClass = "unspecified";
				}
				if (thClass) {
					addClassCounterpartHeading[i] = true;
					$thsCounterpart.eq(i).addClass(thClass);
				}
			});
		},

		// Remove class from corresponding data cells and counterpart heading cells.
		_removeClassDistinctive: function (th) {
			this._removeClass("mark", $.data(th, "$tds"), this.addClassCounterpartData);
			this._removeClass("mark unspecified", this.$thsCounterpart, this.addClassCounterpartHeading);
		},

		// Remove class from an element if the corresponding array value is true.
		// Set true values to false.
		_removeClass: function (className, $elements, array) {
			$elements.each(function (i) {
				if (array[i]) {
					array[i] = false;
					$(this).removeClass(className);
				}
			});
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
					.on("click." + this.widgetName, function (event) {
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

	// distribution ============================================================
	//
	// A distribution chart displays numbers of occurrences of segments in environments.
	// Each data cell displays the result of a search pattern. Example: [C]/_[V]
	// Unless a chart has been transposed:
	// * A row represents a search item which matches segments in data records (tokens).
	//   Example: [C] matches consonants.
	// * A column represents a search environment in which segments (might) occur.
	//   Example: _[V] means preceding a vowel (also known as followed by a vowel).
	//
	// To exchange the background colors which distinguish zero from non-zero data cells,
	// click the lower-right corner of the upper-left heading cell.
	//
	// In a generalized chart, click to collapse or expand individual rows or columns.
	// * General row group: click the heading cell in the general row.
	//   Example: If a general search item is [C],
	//            individual items for a particular project might be p, b, m, and so on.
	// * General column group: click the heading cell in the general column.
	//   Example: If a general search environment is _[V],
	//            individual environments for a particular project might be _i, _u, _a.
	// * All general groups: click the upper-left heading cell (not at its lower-right corner).
	// If for a particular project a general group has no individual (row or column) children,
	// then the group is not collapsible.
	//
	// A widget creator can provide classUncollapsible in the options to indicate
	// uncollapsible rows or columns in (potentially) collapsible row or column groups.
	// * The value is "general" for distribution charts from Phonology Assistant.
	// * The value is "uncollapsible" by default if the option is undefined.
	// Ponder the distinction in meaning between uncollapsible and not collapsible:
	// * If a group does not contain an uncollapsible child, it is not collapsible.
	// * If a table does not contain any collapsible groups, it is not collapsible.
	// The widget provides additional flexibility not used for Phonology Assistant:
	// * An uncollapsible child can occur in any position (not necessarily first).
	// * A group can have more than one uncollapsible child.

	$.widget("phonology.distribution", {
		_create: function () {
			var $thUpperLeft = this.element.find('thead th:first-child'),
				widgetName = this.widgetName,
				eventNamespace = "." + widgetName;

			this._createZero($thUpperLeft, eventNamespace);
			if (!browser("IE7")) {
				this._createCollapsible($thUpperLeft, widgetName, eventNamespace);
			}
		},

		// If any data cells contain zero:
		// * Insert divs at the lower-right corner of the upper-left heading cell.
		// * Attach event handler: click.
		_createZero: function ($thUpperLeft, eventNamespace) {
			if (this.element.has('tbody td.zero').length) {
				$thUpperLeft.addClass("value")
					.append('<div><div class="value">0</div></div>')
					.find('div.value').on("click" + eventNamespace, bind.call(this._clickZero, this));
			}
		},

		// If there are any general row or column groups which are collapsible:
		// * Add classes for CSS: collapsible, uncollapsible, interactive.
		// * Store data: jQuery object to toggle collapsed classes.
		// * Attach event handler: click.
		_createCollapsible: function ($thUpperLeft, widgetName, eventNamespace) {
			var $element = this.element,
				selector = "." + (this.options.classUncollapsible || "uncollapsible"),
				hasCollapsibleRowgroup = this._createCollapsibleRowgroups(selector, widgetName),
				hasCollapsibleColgroup = this._createCollapsibleColgroups(selector, widgetName),
				self = this;

			if (hasCollapsibleRowgroup || hasCollapsibleColgroup) {
				$thUpperLeft.addClass("interactive").data(widgetName, {
					"rowgroup": hasCollapsibleRowgroup,
					"colgroup": hasCollapsibleColgroup,
					"$collapsible": $element
				});

				// Event handler: click an interactive heading cell.
				$element.addClass("collapsible")
					.on("click" + eventNamespace, 'th.interactive', function () {
						self._clickCollapsible(this); // delegate to th.interactive
					});
			}
		},

		// General row groups: <tbody><tr class="general"><th class="Phonetic" ...
		_createCollapsibleRowgroups: function (selectorUncollapsible, widgetName) {
			var hasCollapsibleRowgroup = false;

			this.element.children('tbody').each(function () {
				var $tbody = $(this),
					$trs = $tbody.children(),
					$trsUncollapsible = $trs.filter(selectorUncollapsible).has('th'),
					lengthUncollapsible = $trsUncollapsible.length;

				// If a general row group has at least one individual row,
				// the heading cell in the general row is interactive.
				if (lengthUncollapsible !== 0 && $trs.length > lengthUncollapsible) {
					hasCollapsibleRowgroup = true;
					$tbody.addClass("collapsible");
					$trsUncollapsible.addClass("uncollapsible")
						.children('th').addClass("interactive").data(widgetName, {
							"rowgroup": true,
							"colgroup": false,
							"$collapsible": $tbody
						});
				}
			});

			return hasCollapsibleRowgroup;
		},

		// General column groups: <colgroup><col class="general" />...</colgroup>
		_createCollapsibleColgroups: function (selectorUncollapsible, widgetName) {
			var $element = this.element,
				$colgroups = $element.children('colgroup'),
				$ths = $element.find('thead th'),
				widthColgroups = 0,
				iStart = 0,
				hasCollapsibleColgroup = false,
				isIE8orEarlier;

			if ($colgroups.children().length === $ths.length) {
				isIE8orEarlier = browser("IE8");
				$colgroups.each(function () {
					var $colgroup = $(this),
						$cols = $colgroup.children(),
						length = $cols.length,
						$colsUncollapsible = $cols.filter(selectorUncollapsible),
						lengthUncollapsible = $colsUncollapsible.length;

					// If a general column group has at least one individual column,
					// the heading cell in the general column is interactive.
					if (lengthUncollapsible !== 0 && length > lengthUncollapsible) {
						hasCollapsibleColgroup = true;
						$colgroup.addClass("collapsible");
						$colsUncollapsible.addClass("uncollapsible").each(function () {
							// Add class and store data: the heading cell for the column.
							$ths.eq(iStart + $cols.index(this)).addClass("interactive")
								.data(widgetName, {
									"rowgroup": false,
									"colgroup": true,
									// IE8 or earlier: Must add the class to col elements
									// because adding the class to the colgroup element
									// does not cause the CSS rule to take effect.
									"$collapsible": isIE8orEarlier ?
											$cols.not($colsUncollapsible) :
											$colgroup
								});
						});
					}
					widthColgroups += this.offsetWidth;
					iStart += length;
				});
			}

			if (hasCollapsibleColgroup) {
				this.classColgroup = "collapsed_by_visibility";
				this.detectColgroup = {
					"widthColgroupsOriginal": widthColgroups,
					"widthTableOriginal": $element.get(0).offsetWidth,
					"$colgroups": $colgroups,
					"$ths": $ths
				}; // see _clickCollapsible, _detectColgroup, _updateColgroups
			}

			return hasCollapsibleColgroup;
		},

		// event handlers ------------------------------------------------------

		// Click the div at the lower-right corner of the upper-left heading cell.
		_clickZero: function (event) {
			this.element.toggleClass("nonzero");
			event.stopPropagation(); // do not call _clickCollapsible
		},

		// Click an interactive heading cell in a collapsible chart.
		_clickCollapsible: function (th) {
			var data = $.data(th, this.widgetName),
				classes = [],
				detectColgroup;

			if (data.rowgroup) {
				classes.push("collapsed");
			}
			if (data.colgroup) {
				classes.push(this.classColgroup);
				detectColgroup = this.detectColgroup;
			}
			data.$collapsible.toggleClass(classes.join(" "));

			// For information about lazy loading,
			// see pages 154-155 in High Performance JavaScript.
			if (detectColgroup) {
				this._detectColgroup(detectColgroup, data);
				delete this.detectColgroup;
			}
		},

		// private implementation for feature detection ------------------------

		// After the first time to collapse at least one column group,
		// detect whether the browser implements visibility: collapse rules in CSS.
		_detectColgroup: function (detectColgroup, data) {
			var widthColgroupsOriginal = detectColgroup.widthColgroupsOriginal,
				widthColgroups = 0,
				update;

			if (widthColgroupsOriginal !== 0) {
				detectColgroup.$colgroups.each(function () {
					widthColgroups += this.offsetWidth;
				});
				// Sum of widths of column groups:
				// * It changes in Opera 10.5 or later.
				// * It remains the same in Firefox 9. That is,
				//   cells collapse (see below) but colgroup borders do not.
				//   http://www.w3.org/TR/CSS21/tables.html#dynamic-effects
				//   "The suppression of the row or column, however,
				//   does not otherwise affect the layout of the table."
				//   Does the standard allow browsers not to update borders?!
				update = widthColgroups === widthColgroupsOriginal;
			} else {
				// If widths of column groups are 0, test width of the table instead:
				// * It changes in IE8 or later.
				//   By the way, it also changes in Firefox 9, Opera 10.5 or later.
				// * It remains the same in Chrome 16, Safari 5.1.2.
				update = this.element.get(0).offsetWidth === detectColgroup.widthTableOriginal;
			}

			if (update) {
				data.$collapsible.removeClass(this.classColgroup);
				this._updateColgroups(detectColgroup); // changes the $collapsible property!
				data.$collapsible.addClass(this.classColgroup = "collapsed_by_display");
			}
		},

		// Update column groups to collapse by display: none rules in CSS.
		_updateColgroups: function (detectColgroup) {
			var $ths = detectColgroup.$ths,
				$cellsArray = [],
				widgetName = this.widgetName,
				iStart = 0,
				self = this;

			this.element.find('tbody tr').each(function () {
				$cellsArray.push($(this).children());
			});
			detectColgroup.$colgroups.each(function () {
				var $colgroup = $(this),
					$cols = $colgroup.children(),
					iEnd = iStart + $cols.length;

				if ($colgroup.hasClass("collapsible")) {
					self._updateColgroup($cols, iStart, iEnd, $ths, $cellsArray, widgetName);
				}
				iStart = iEnd;
			});
		},

		// Update heading and data cells in collapsible column group.
		_updateColgroup: function ($cols, iStart, iEnd, $ths, $cellsArray, widgetName) {
			var updateCells = this._updateCells, // method does not depend on this
				colsCollapsible = [],
				indexesUncollapsible = [], // sparse array, initialized as side effect
				$colsUncollapsible = $cols.filter('.uncollapsible').each(function () {
					indexesUncollapsible[$cols.index(this)] = true;
				}),
				length = $cellsArray.length,
				i;

			// Add collapsible class to cells in the column group,
			// except one or more in each row which are uncollapsible.
			updateCells($ths.slice(iStart, iEnd), indexesUncollapsible, colsCollapsible);
			for (i = 0; i < length; i += 1) {
				updateCells($cellsArray[i].slice(iStart, iEnd), indexesUncollapsible, colsCollapsible);
			}

			// Update data for interactive heading cells, which correspond to uncollapsible columns.
			$colsUncollapsible.each(function () {
				var data = $ths.eq(iStart + $cols.index(this)).data(widgetName);

				data.$collapsible = data.$collapsible.add(colsCollapsible);
			});
		},

		// Except for uncollapsible cells, add class and append to list.
		_updateCells: function ($cells, indexesUncollapsible, colsCollapsible) {
			$cells.each(function (i) {
				if (!indexesUncollapsible[i]) {
					$(this).addClass("collapsible");
					colsCollapsible.push(this); // cell DOM element
				}
			});
		}
	});

	// create widgets ==========================================================

	$('.quadrilaterals').quadrilaterals();
	setTimeout(function () {
		var $mediator = $('div.mediator');

		$('table.distribution').distribution({"classUncollapsible": "general"});
		$('table.list').tableList();
		$mediator.mediator(); // create mediator before any of the following:
		$mediator.find('div.diagram').diagram();
		$mediator.find('table.features').tableFeatures(); // create tableFeatures before tableCV
		$mediator.find('table.CV').tableCV();
		$('table.values > thead').siblings('table.dual > tfoot').andSelf().rowgroupsValues();
	}, 50);
});