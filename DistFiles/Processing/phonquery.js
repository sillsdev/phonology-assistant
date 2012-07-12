// phonquery.js 2012-05-24
// Phonology queries for XHTML files exported from Phonology Assistant which contain:
// * consonant or vowel charts <table class="CV chart ...">
//   * with tables of descriptive or distinctive features <table class="... features">
//   * with diagrams for articulatory phonetics <div class="diagram">
// * values charts <table class="... values chart">
//
// http://phonologyassistant.sil.org/

// You can create queryable objects for:
// * features: a set of phonetic or phonological features
// * value or values: one or more values of a distinctive feature

// An immediate function hides the implementation. See pages 69-73 in JavaScript Patterns.
(function (root) {
	"use strict"; // Firefox 4 or later

	// http://jslint.com defines a professional subset of JavaScript.
	/*jslint devel: false */ // warn about console, alert
	/*jslint nomen: true */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jslint indent: 4 */ // consider a tab equivalent to 4 spaces
	/*jslint forin: true */ // do not warn about unfiltered for (... in ...) loops
	// As in jQuery, assume that neither application nor library code has changed the prototype of Object.

	// http://jshint.com is a tool to detect errors and potential problems in JavaScript code.
	/*jshint devel: false */ // warn about console, alert
	/*jshint trailing: true */ // warn about trailing white space
	/*jshint nomen: false */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jshint onevar: true */ // only one var statement per function
	/*jshint white: false */ // do not warn about indentation
	/*jshint forin: false */ // do not warn about unfiltered for (... in ...) loops
	// As in jQuery, assume that neither application nor library code has changed the prototype of Object.

	var forEach = Array.prototype.forEach, // ECMAScript 5
		slice = Array.prototype.slice,
		hasOwn = Object.prototype.hasOwnProperty,
		toString = Object.prototype.toString,

		arg_typeOf = {
			"boolean": "boolean",
			"number": "number",
			"string": "string",
			"undefined": "undefined"
		},

		toString_typeOf = {
			"[object Array]": "array",
			"[object Error]": "error",
			"[object Date]": "date",
			"[object Function]": "function",
			"[object RegExp]": "regexp"
		},

		// Return one of the following types:
		// array boolean date error function null number object regexp string undefined
		// See YUI.Lang.type or jQuery.type (which does not distinguish error from object)
		// or pages 16, 61, 103-104 in JavaScript: The Good Parts.
		typeOf = function (arg) {
			return arg_typeOf[typeof arg] ||
				toString_typeOf[toString.call(arg)] ||
				(arg ? "object" : "null");
		},

		// For iteration methods each, every, some:
		// * Make no assumption about the order of the items in an object.
		// * Provide context if the callback function refers to this.
		// * Callback arguments are like YUI 3 and forEach in ECMAScript 5:
		//   array: callback(value, index, array)
		//   object: callback(value, key, object)

		// Call a function for each item in a collection.
		// Unlike jQuery, callback cannot return false to stop the iteration.
		each = function (collection, callback, context) {
			var i, length, key;

			switch (typeOf(collection)) {
			case "array":
				if (forEach) {
					forEach.call(collection, callback, context);
				} else {
					for (i = 0, length = collection.length; i < length; i += 1) {
						callback.call(context, collection[i], i, collection);
					}
				}
				break;

			case "object":
				for (key in collection) {
					callback.call(context, collection[key], key, collection);
				}
				break;
			}
			return collection; // chainable function
		},

		// Return whether all of the items in a collection pass a test.
		// That is, whether callback returns a truthy value for every item.
		// Like the && operator, iteration stops if callback returns a falsy value.
		// Unlike the && operator, the result is always boolean.
		every = function (collection, callback, context) {
			var i, length, key;

			switch (typeOf(collection)) {
			case "array":
				for (i = 0, length = collection.length; i < length; i += 1) {
					if (!callback.call(context, collection[i], i, collection)) {
						return false;
					}
				}
				break;

			case "object":
				for (key in collection) {
					if (!callback.call(context, collection[key], key, collection)) {
						return false;
					}
				}
				break;
			}
			return true;
		},

		// Return whether at least one item in a collection passes a test.
		// That is, whether callback returns a truthy value for some item.
		// Like the || operator, iteration stops if callback returns a truthy value.
		// Unlike the || operator, the result is always boolean.
		some = function (collection, callback, context) {
			var i, length, key;

			switch (typeOf(collection)) {
			case "array":
				for (i = 0, length = collection.length; i < length; i += 1) {
					if (callback.call(context, collection[i], i, collection)) {
						return true;
					}
				}
				break;

			case "object":
				for (key in collection) {
					if (callback.call(context, collection[key], key, collection)) {
						return true;
					}
				}
				break;
			}
			return false;
		},

		// phonQuery -----------------------------------------------------------

		// You can access the virtual constructor functions using:

		// * Module Pattern (see pages 97-101 in JavaScript Patterns)
		//   features = phonQuery.featureQuery(...);
		//   value = phonQuery.valueQuery(...);
		//
		//   Because phonQuery is a function instead of an object,
		//   jQuery UI does not make a deep copy when it is an option of a widget.

		// * Sandbox Pattern (see pages 101-105 in JavaScript Patterns)
		//   The sandbox is a callback function which receives
		//   one or more virtual constructor functions as arguments:
		//   r = phonQuery(["valueQuery", "featureQuery"], function (vQ, fQ) { ... });
		//   r = phonQuery("featureQuery", "valueQuery", function (fQ, vQ) { ... });
		//   r = phonQuery("featureQuery", function (fQ) { ... });
		//
		//   You might use a sandbox only to create objects if you:
		//   * store them as data associated with DOM elements
		//     for example, in jQuery: $.data(td, "values", valueQuery($(td).text()));
		//   * store them as properties of this in jQuery UI widgets
		//   * return them from the callback function

		phonQuery = function (arg0) {
			var args = slice.call(arguments), // convert to an array
				callback = args.pop(), // the callback is the last argument
				length = args.length, // args which precede the callback
				callbackArgs = [];

			if (typeof callback === "function" && length !== 0) {
				each(length === 1 && typeOf(arg0) === "array" ? arg0 : args, function (arg) {
					callbackArgs.push(typeof arg === "string" &&
						// Call hasOwn in case application code has added methods
						// to Function.prototype (for example, a bind function
						// for browsers which do not support ECMAScript 5).
						hasOwn.call(phonQuery, arg) ? phonQuery[arg] : null);
				});
				return callback.apply(null, callbackArgs);
			}
		};

	// values of distinctive features ==========================================

	// An object represents one or more values of a distinctive feature.
	// * As in jQuery, you can call some methods (for example, add) in a chain.
	//   See pages 110-111 in JavaScript Patterns.
	// * As in jQuery, chaining methods (might) return a new (flyweight) object.
	//
	// Examples of concise simplicity and expressive power of value queries:
	//
	// * Click a small box in a corner of a values chart to select
	//   the next value to highlight when a distinctive feature is active:
	//   var value = this.value = this.value.next();
	//   $div.attr("title", value.title()).text(value.charExternal());
	//
	// * Assuming that cells in a values chart have a values object as data,
	//   mark the cells in a row or column which have the selected value:
	//   var value = this.value;
	//   $tds.filter(function () {
	//        return $.data(this, "values").has(value);
	//   }).addClass("mark");

	// An immediate function returns a virtual constructor function.
	phonQuery.valueQuery = (function () {
		var valueOrder = ["+", "-", "0"],
			lengthOrder = valueOrder.length,
			valueDefinitions = {
				"+": {
					description: "plus",
					index: 1,
					chars: ["+", "\u00B1"] // plus sign, plus-minus sign
				},
				"-": {
					description: "minus",
					index: 2,
					chars: ["\u2013", "-", "\u00B1"] // en dash, hyphen-minus, plus-minus sign
				},
				"0": {
					description: "unspecified",
					index: 4,
					chars: ["0"] // digit zero
				}
			},

			// Strategies define a family of algorithms, encapsulate each one,
			// and make them interchangeable. See pages 315-323 in Design Patterns.

			Value = function () {},
			prototypeValue = Value.prototype, // see common methods

			// Private constructor for one value of a distinctive feature.
			// Example: new Value1("+", "+-") // plus value in the bivalent required sequence
			Value1 = function (char, valueSequence) {
				var valueDefinition = valueDefinitions[char],
					index = this.index = valueDefinition.index % 4; // for deletable, and so on

				this.digit = String(index); // for key
				this.char = char; // for charInternal
				this.description = valueDefinition.description; // for title
				this.valueSequence = valueSequence; // for add, remove
			},
			prototypeValue1 = Value1.prototype = new Value(), // see value1 methods

			// Private constructor for a set of values of a distinctive feature.
			// Example: new Values({"+": true, "-": true}) // set contains plus, minus
			Values = function (values) {
				var descriptions = [];

				each(valueOrder, function (char) {
					if (values[char]) {
						descriptions.push(valueDefinitions[char].description);
					}
				});
				this.description = descriptions.join(" "); // for title
				this.values = values; // for each, has, and so on
			},
			prototypeValues = Values.prototype = new Value(), // see values methods

			// Flyweights are shared objects that can be used in multiple contexts.
			// See pages 195-206 in Design Patterns.

			// A distinctive feature can have one of the following value sequences.
			// Example of when the sequence matters: Click to change to the next value
			// in the Distinctive Features dialog box in Phonology Assistant.
			value1Flyweights = each({
				"+0": {}, // univalent
				"+-": {}, // bivalent required
				"+-0": {} // bivalent optional
			}, function (objectSequence, valueSequence) {
				var length = valueSequence.length,
					i,
					char;

				// The sequence object contains value objects corresponding to the chars.
				for (i = 0; i < length; i += 1) {
					char = valueSequence.charAt(i);
					objectSequence[char] = new Value1(char, valueSequence);
				}
				// Each object points to its successor (the next object in the sequence).
				for (i = 0; i < length; i += 1) {
					objectSequence[valueSequence.charAt(i)].successor =
						objectSequence[valueSequence.charAt((i + 1) % length)];
				}
			}),

			// digit zero, plus sign, hyphen-minus, plus-minus sign:
			charForIndexMod4 = ["0", "+", "-", "\u00B1"],

			// The index of each object in the array is the sum of
			// the index property in the value definitions for "+", "-", "0".
			valuesFlyweights = each([
				// unspecified is implicit for segments in consonant or vowel charts:
				new Values({}),
				new Values({"+": true}),
				new Values({"-": true}),
				new Values({"+": true, "-": true}),
				// unspecified is explicit for data cells in values charts:
				new Values({"0": true}),
				new Values({"+": true, "0": true}),
				new Values({"-": true, "0": true}),
				new Values({"+": true, "-": true, "0": true})
			], function (valuesFlyweight, index) {
				valuesFlyweight.index = index; // for add, remove, and so on
				valuesFlyweight.digit = String(index % 4); // for key
				valuesFlyweight.char = charForIndexMod4[index % 4]; // for charInternal
			}),

			// valueQuery ------------------------------------------------------

			// The valueQuery virtual constructor returns a value object.
			// Also known as a factory method. See pages 107-116 in Design Patterns.
			//
			// * Examples of Value1 objects:
			//   * the (initial) value to highlight when a feature is active in a values chart:
			//     this.value = valueQuery("+", "+-0");
			//   * property of a features object for a segment in an XHTML file
			//     which simulates the Distinctive Features dialog box in Phonology Assistant:
			//     this.features["son"] = valueQuery("-", "+-"); // "-son"
			//
			//   Return undefined for unknown text or valueSequence.
			//
			// * Examples of Values objects:
			//   * property of a features object for a segment in a consonant or vowel chart or
			//     for the selected features in a table of features at the right of a CV chart:
			//     this.features["son"] = valueQuery("-"); // "-son"
			//     A character corresponds to a value in a features object.
			//   * data for a cell in a values chart, omitting some details:
			//     $.data(td, "values", valueQuery($(td).text() ...));
			//     A string might correspond to values in a cell in a values chart:
			//     * for all segments which have a descriptive feature (for example, approximant)
			//     * which represents a contour value (for diphthong, prenasalized, affricate)
			//
			//   Return implicitly unspecified object for unknown text, including empty string.

			valueQuery = function (text, valueSequence) {
				var index, char, valueDefinition, chars, i, length;

				if (typeof valueSequence === "string") {
					if (hasOwn.call(value1Flyweights, valueSequence)) {
						return value1Flyweights[valueSequence][text];
					}
				} else {
					index = 0; // implicitly unspecified
					for (char in valueDefinitions) {
						valueDefinition = valueDefinitions[char];
						chars = valueDefinition.chars;
						for (i = 0, length = chars.length; i < length; i += 1) {
							if (text.indexOf(chars[i]) !== -1) {
								index += valueDefinition.index;
								break;
							}
						}
					}
					return valuesFlyweights[index];
				}
			};

		// Replace Value with the correct constructor.
		// See page 127 in JavaScript Patterns.
		prototypeValue1.constructor = Value1;
		prototypeValues.constructor = Values;

		// You can add custom methods (plugins) to valueQuery:
		// * valueQuery.prototype.plugin = function (...) {...}; // common plugins
		// * valueQuery.value1.prototype.plugin = function (...) {...};
		// * valueQuery.values.prototype.plugin = function (...) {...};
		valueQuery.prototype = prototypeValue;
		valueQuery.value1 = {"prototype": prototypeValue1};
		valueQuery.values = {"prototype": prototypeValues};

		// common methods ------------------------------------------------------

		// Return whether this can be represented as an undefined property,
		// especially in a features object for a table of (distinctive) features.
		prototypeValue.deletable = function () {
			return this.index === 0; // implicitly unspecified
		};

		// Return whether this is distinguishable from the other value.
		prototypeValue.distinguishableFrom = function (value) {
			return (this.has("+") && !value.has("+")) || (this.has("-") && !value.has("-"));
		};

		// Return the digit character which represents the value in a key.
		prototypeValue.key = function () {
			return this.digit;
		};

		// Return the character for the value:
		// * in JavaScript code
		// * in HTML markup which is not displayed
		//   For example, features of a segment in a consonant or vowel chart.
		prototypeValue.charInternal = function () {
			return this.char;
		};

		// Return the opposite character if only one value is specified.
		prototypeValue.charInternalOpposite = function () {
			switch (this.index) {
			case 1: // plus
				return "-"; // hyphen-minus
			case 2: // minus
				return "+"; // plus sign
			}
		};

		// Return a title for the value to display in a screen tip.
		prototypeValue.title = function () {
			return this.description;
		};

		// Return the character for the value in HTML markup which is displayed.
		prototypeValue.charExternal = function () {
			return this.s_charExternal(this.char);
		};

		// common static methods -----------------------------------------------

		// Return the character in HTML markup which is displayed.
		prototypeValue.s_charExternal = function (char) {
			return char === "-" ? "\u2013" : char; // display minus as en dash
		};

		// Return the character for the value argument string or object.
		prototypeValue.s_charOrValue = function (arg) {
			return typeof arg === "string" ? arg : arg.charInternal();
		};

		// value1 methods ------------------------------------------------------

		// Add to the value usually means replace the value.
		prototypeValue1.add = function (char) {
			return valueQuery(char, this.valueSequence) || this; // chaining method
		};

		// Remove the value usually means replace with unspecified.
		prototypeValue1.remove = function (char) {
			return arguments.length === 0 || (char !== "0" && this.char === char) ?
					valueQuery("0", this.valueSequence) :
					this; // chaining method
		};

		// For iteration methods each, every, some:
		// * Provide context if the callback function refers to this.
		// * Method arguments are like YUI 3
		//   but callback arguments are different: callback(char).

		// Call a function for the value.
		prototypeValue1.each = function (callback, context) {
			callback.call(context, this.char);
			return this; // chaining method
		};

		// Return whether the value passes a test.
		prototypeValue1.every = prototypeValue1.some = function (callback, context) {
			return !!callback.call(context, this.char); // boolean
		};

		// Return the number of values.
		prototypeValue1.length = function () {
			return 1;
		};

		// Return whether this has a (certain) value.
		prototypeValue1.has = function (arg) {
			return arguments.length === 0 || this.is(arg);
		};

		// Return whether this is a certain value.
		prototypeValue1.is = function (arg) {
			return this.char === this.s_charOrValue(arg);
		};

		// value1-only methods -------------------------------------------------

		// Return the next value in the sequence ("+0", "+-", or "+-0").
		prototypeValue1.next = function () {
			return this.successor;
		};

		// values methods ------------------------------------------------------

		// Add to the values.
		prototypeValues.add = function (char) {
			var valueDefinition;

			if (!this.values[char]) { // not has
				valueDefinition = valueDefinitions[char];
				if (valueDefinition) {
					return valuesFlyweights[this.index + valueDefinition.index];
				}
			}
			return this; // chaining method
		};

		// Remove from the values.
		prototypeValues.remove = function (char) {
			if (arguments.length === 0) { // remove all values
				return valuesFlyweights[0]; // implicitly unspecified
			}
			if (this.values[char]) { // has
				return valuesFlyweights[this.index - valueDefinitions[char].index];
			}
			return this; // chaining method
		};

		// For iteration methods each, every, some:
		// * Provide context if the callback function refers to this.
		// * Method arguments are (almost) like YUI 3 (each has an extra argument)
		//   but callback arguments are different: callback(char).

		// Call a function for each value.
		// Provide inOrder if the calling code assumes a predictable order of values.
		// Unlike jQuery, callback cannot return false to stop the iteration.
		prototypeValues.each = function (callback, context, inOrder) {
			var values = this.values,
				i,
				char;

			if (inOrder) {
				for (i = 0; i < lengthOrder; i += 1) {
					char = valueOrder[i];
					if (values[char]) {
						callback.call(context, char);
					}
				}
			} else {
				for (char in values) {
					callback.call(context, char);
				}
			}
			return this; // chaining method
		};

		// Return whether all of the values pass a test.
		// That is, whether callback returns a truthy value for every value.
		// Like the && operator, iteration stops if callback returns a falsy value.
		// Unlike the && operator, the result is always boolean.
		prototypeValues.every = function (callback, context) {
			var values = this.values,
				char;

			for (char in values) {
				if (!callback.call(context, char)) {
					return false;
				}
			}
			return true;
		};

		// Return whether at least one value passes a test.
		// That is, whether callback returns a truthy value for some value.
		// Like the || operator, iteration stops if callback returns a truthy value.
		// Unlike the || operator, the result is always boolean.
		prototypeValues.some = function (callback, context) {
			var values = this.values,
				char;

			for (char in values) {
				if (callback.call(context, char)) {
					return true;
				}
			}
			return false;
		};

		// Return the number of values.
		// Tip: Use values.has() instead of values.length() !== 0.
		prototypeValues.length = function () {
			var length = 0;

			this.each(function () {
				length += 1;
			});
			return length;
		};

		// Return whether this has a (certain) value.
		// Tip: Use values.has() instead of values.length() !== 0.
		prototypeValues.has = function (arg) {
			return arguments.length === 0 ?
					this.index !== 0 : // not implicitly unspecified
					!!this.values[this.s_charOrValue(arg)]; // return boolean
		};

		// Return whether this is a certain value.
		prototypeValues.is = function (arg) {
			return this === valueQuery(this.s_charOrValue(arg));
		};

		// ---------------------------------------------------------------------

		return valueQuery;
	}());

	// features ================================================================

	// An object represents a set of phonetic or phonological features.
	// * As in jQuery, you can call some methods (for example, add) in a chain.
	//   See pages 110-111 in JavaScript Patterns.
	// * However, chaining methods affect and return the original object.
	//
	// Examples of concise simplicity and expressive power of feature queries:
	//
	// * To replace the contents of featuresTo with a copy of featuresFrom:
	//   featuresTo.remove().add(featuresFrom); // chaining method
	//
	// * Assuming that cells in a features table have a feature string as data,
	//   if any features are still selected after you click a cell, then ...
	//   if (featuresSelected.toggle($.data(td, "feature")).has()) { ... }
	//
	// * Assuming that cells in a consonant chart have features objects as data,
	//   match the cells which have the descriptive feature fricative,
	//   or the equivalent distinctive features non-sonorant and continuant:
	//   * featureClass = "descriptive";
	//     featuresSelected = featureQuery(featureClass, "fricative");
	//   * featureClass = "distinctive";
	//     featuresSelected = featureQuery(featureClass, ["-son", "+cont"]);
	//   $tds.filter(function () {
	//        return $.data(this, featureClass).has(featuresSelected);
	//   }).addClass("matched");

	// An immediate function returns a virtual constructor function.
	phonQuery.featureQuery = (function (valueQuery) { // featureQuery uses valueQuery
		var keyUnspecified = valueQuery("0").key(),

			// Append a (shallow) copy of items in arrayFrom to arrayTo.
			// That is, at the end of arrayTo, push each item in arrayFrom.
			append = function (arrayTo, arrayFrom) {
				each(arrayFrom, function (item) {
					arrayTo.push(item);
				});
			},

			// Strategies define a family of algorithms, encapsulate each one,
			// and make them interchangeable. See pages 315-323 in Design Patterns.

			Features = function () {},
			prototypeFeatures = Features.prototype, // see common methods

			DescriptiveFeatures = function () {
				this._init();
			},
			prototypeDescriptive = DescriptiveFeatures.prototype = new Features(),

			DistinctiveFeatures = function (valueSequences) {
				this.valueSequences = valueSequences; // object or undefined
				this._init();
			},
			prototypeDistinctive = DistinctiveFeatures.prototype = new Features(),

			ConstructorFeatures = {
				"descriptive": DescriptiveFeatures,
				"distinctive": DistinctiveFeatures
			},

			// featureQuery ----------------------------------------------------

			// The featureQuery virtual constructor returns a features object.
			// Also known as a factory method. See pages 107-116 in Design Patterns.
			//
			// If the first argument is:
			//
			// * features class string: create that type of object
			//   features_p = featureQuery("descriptive",
			//       "consonant", "voiceless", "bilabial", "plosive");
			//   features_syllabic_vowel = featureQuery("distinctive",
			//       ["+syll", "-cons", "+son", "+approx", ...]);
			//
			//   If the second argument is an object, distinctive feature properties
			//   are Value1 (instead of Values) objects with the corresponding sequences:
			//   features = featureQuery("distinctive",
			//       {... "coronal": "+-", "ant": "+-0", "distr": "+-0", "strid": "+-0", ...},
			//       ["+syll", "-cons", "+son", "+approx", ... "-coronal", "+dorsal", ...]);
			//
			// * features object: create a copy of it (like a copy constructor)
			//   featuresTo = featureQuery(featuresFrom);
			//
			// Additional string or array arguments add features to the object.

			featureQuery = function (arg0, arg1) {
				var Constructor, valueSequences, i, object, length, arg, type;

				if (typeof arg0 === "string") {
					Constructor = ConstructorFeatures[arg0];
					if (typeOf(arg1) === "object") {
						valueSequences = arg1;
						i = 2;
					} else {
						i = 1;
					}
				} else if (arg0 instanceof Features && arg0.constructor !== Features) {
					Constructor = arg0.constructor;
					valueSequences = arg0.valueSequences;
					i = 0;
				}
				if (typeof Constructor === "function") {
					object = new Constructor(valueSequences);
					for (length = arguments.length; i < length; i += 1) {
						arg = arguments[i];
						type = typeOf(arg);
						if (i === 0 || type === "string" || type === "array") {
							object.add(arg);
						}
					}
					return object;
				}
			},
			featureClasses = featureQuery.featureClasses = [];

		// You can add custom methods (plugins) to featureQuery:
		// * featureQuery.prototype.plugin = function (...) {...}; // common plugins
		// * featureQuery.descriptive.prototype.plugin = function (...) {...};
		// * featureQuery.distinctive.prototype.plugin = function (...) {...};
		featureQuery.prototype = prototypeFeatures;
		each(ConstructorFeatures, function (constructor, featureClass) {
			var prototype = constructor.prototype;

			featureClasses.push(featureClass); // for introspection
			prototype.featureClass = featureClass; // for introspection
			prototype.constructor = constructor; // see page 127 in JavaScript Patterns
			featureQuery[featureClass] = {"prototype": prototype};
		});

		// common methods ------------------------------------------------------

		// Private initialization for constructors.
		prototypeFeatures._init = function () {
			this.features = {};
		};

		// Call a method of this for each feature in an array or object.
		// * default: callback(feature)
		// * if everyNameChar when arg is a distinctive features object: callback(name, char)
		// If callback returns false, then stop the iteration and return false;
		// otherwise, return true after the iteration ends.
		prototypeFeatures._iterateBoolean = function (arg, callback, everyNameChar) {
			switch (typeOf(arg)) {
			case "array":
				return every(arg, function (feature) {
					return callback.call(this, feature) !== false;
				}, this); // must provide this

			case "object":
				return everyNameChar && arg instanceof DistinctiveFeatures ?
						// If callback does not need a feature string, avoid concatenation.
						arg.everyNameChar(function (name, char) {
							return callback.call(this, name, char) !== false;
						}, this) : // must provide this
						arg.every(function (feature) {
							return callback.call(this, feature) !== false;
						}, this); // must provide this
			}
		};

		// A Template Method lets subclasses redefine certain steps
		// of an algorithm without changing the algorithm's structure.
		// See pages 325-330 in Design Patterns.

		// Add to the features.
		prototypeFeatures.add = function (arg) {
			if (typeof arg === "string") {
				this._add(arg);
			} else { // arg is an array or object
				this._iterateBoolean(arg, this._add, true);
			}
			return this; // chaining method
		};

		// Remove from the features.
		prototypeFeatures.remove = function (arg) {
			var features, feature;

			if (arguments.length === 0) { // remove all features
				features = this.features;
				for (feature in features) {
					delete features[feature];
				}
			} else if (typeof arg === "string") {
				this._remove(arg);
			} else { // arg is an array or object
				this._iterateBoolean(arg, this._remove, true);
			}
			return this; // chaining method
		};

		// Add to or remove from the features.
		prototypeFeatures.toggle = function (arg, bool) {
			if (typeof bool === "boolean") { // arg is a string, array, or object
				this[bool ? "add" : "remove"](arg);
			} else if (typeof arg === "string") {
				this[this._has(arg) ? "_remove" : "_add"](arg);
			} else { // arg is an array or object
				this._iterateBoolean(arg, this.toggle, false);
			}
			return this; // chaining method
		};

		// For iteration methods each, every, some:
		// * Make no assumption about the order of the features.
		// * Provide context if the callback function refers to this.
		// * Method arguments are like YUI 3
		//   but callback arguments are different:
		//   * descriptive: callback(feature)
		//   * distinctive: callback(feature, name, char)

		// Call a function for each feature.
		// Unlike jQuery, callback cannot return false to stop the iteration.
		// Tip: For distinctive features, see eachNameChar.
		prototypeFeatures.each = function (callback, context) {
			var features = this.features,
				feature;

			for (feature in features) {
				this._iterate(callback, context, feature, "each");
			}
			return this; // chaining method
		};

		// Return whether all of the features pass a test.
		// That is, whether callback returns a truthy value for every feature.
		// Like the && operator, iteration stops if callback returns a falsy value.
		// Unlike the && operator, the result is always boolean.
		// Tip: For distinctive features, see everyNameChar.
		prototypeFeatures.every = function (callback, context) {
			var features = this.features,
				feature;

			for (feature in features) {
				if (!this._iterate(callback, context, feature, "every")) {
					return false;
				}
			}
			return true;
		};

		// Return whether at least one feature passes a test.
		// That is, whether callback returns a truthy value for some feature.
		// Like the || operator, iteration stops if callback returns a truthy value.
		// Unlike the || operator, the result is always boolean.
		// Tip: For distinctive features, see someNameChar.
		prototypeFeatures.some = function (callback, context) {
			var features = this.features,
				feature;

			for (feature in features) {
				if (this._iterate(callback, context, feature, "some")) {
					return true;
				}
			}
			return false;
		};

		// Return the number of features.
		// Tip: Use features.has() instead of features.length() !== 0.
		prototypeFeatures.length = function () {
			var length = 0;

			this.each(function () {
				length += 1;
			});
			return length;
		};

		// Return whether this has (certain) features.
		prototypeFeatures.has = function (arg) {
			var features, feature;

			if (arguments.length === 0) {
				// Tip: Use features.has() instead of features.length() !== 0.
				features = this.features;
				for (feature in features) {
					if (typeof feature === "string") { // placate JSLint
						return true; // has at least one feature
					}
				}
				return false; // has no features
			}
			if (typeof arg === "string") {
				return this._has(arg);
			}
			// arg is an array or object
			// Are the features of arg a subset of the features of this?
			// That is, return whether this has all the features of arg.
			return this._iterateBoolean(arg, this._has, true);
		};

		// Return an array of the features in order determined by the names array.
		prototypeFeatures.array = function (names) {
			var array = [];

			each(names, function (name) {
				this._array(array, name);
			}, this); // must provide this
			return array;
		};

		// Return a string of the features:
		// * separated by separator
		// * concatenated in order determined by the names array
		prototypeFeatures.join = function (separator, names) {
			return this.array(names).join(separator);
		};

		// Return a key string which represents the values of features
		// concatenated in order determined by the names array.
		// The values of an optional features object of changes
		// have priority over the values of this.
		// If you compute the key string with and without changes
		// for each segment in a phonetic inventory,
		// you can determine which segments change to and from
		// other segments without changing any of their features.
		prototypeFeatures.key = function (names, changes) {
			var array = [];

			each(names, function (name) {
				array.push(this.keyFor(name, changes));
			}, this); // must provide this
			return array.join("");
		};

		// descriptive methods -------------------------------------------------

		// Phonology Assistant has about 100 descriptive features.
		// For more information, see The Sounds of the World's Languages
		// or chapter 1 in Introductory Phonology.
		// * A specified feature is a property whose value is true.
		// * An unspecified feature is an undefined property.
		//
		// A typical segment has four to six features. For example, p has four:
		// {"consonant": true, "voiceless": true, "bilabial": true, "plosive": true}

		prototypeDescriptive._add = function (feature) {
			this.features[feature] = true;
		};

		prototypeDescriptive._remove = function (feature) {
			delete this.features[feature];
		};

		prototypeDescriptive._iterate = function (callback, context, feature) {
			return callback.call(context, feature);
		};

		prototypeDescriptive._has = function (feature) {
			return !!this.features[feature]; // boolean
		};

		prototypeDescriptive._array = function (array, feature) {
			if (this.features[feature]) { // has
				array.push(feature);
			}
		};

		// Return the digit character which represents the value of a feature.
		prototypeDescriptive.keyFor = function (feature, changes) {
			return (changes && changes.has(feature)) || this.features[feature] ? "1" : "0";
		};

		// Get or set the value of a feature.
		prototypeDescriptive.value = function (feature, bool) {
			if (arguments.length === 1) {
				return this.features[feature]; // get returns true or undefined
			}
			if (typeof bool === "boolean") {
				this.toggle(feature, bool); // set
			}
			return this; // chaining method for set
		};

		// descriptive-only methods --------------------------------------------

		// Return a description string for the features according to the optional format.
		prototypeDescriptive.description = function (argFormat) {
			var categories = this._descriptionFormat("categories", argFormat),
				description = [],
				pattern;

			if (some(this._descriptionFormat("patterns", argFormat), function (value, key) {
					if (this.has(key)) {
						pattern = value; // pattern for consonant, monophtong, diphthong
						return true;
					}
				}, this)) {
				each(pattern, function (category) {
					var prev = false;

					each(categories[category] || [], function (feature) {
						var push = true;

						if (this.has(feature)) {
							some(this._descriptionFormat("featureChanges", argFormat)[feature] ||
								this._descriptionFormat("categoryChanges", argFormat)[category] ||
								[], function (change) {
									var when = change.when,
										matched = when ?
												some(when, function (condition) {
													return typeof condition === "function" ?
															condition.call(this, argFormat) :
															this.has(condition);
												}, this) :
												true;

									if (matched) {
										if (change.prepend) {
											// One or more elements precede the feature or its replacement.
											append(description, change.prepend);
										}
										if (change.replace) {
											// Zero or more elements replace the feature.
											append(description, change.replace);
											push = false;
										}
										return true;
									}
								}, this);
							if (push) {
								// Append the feature unless it was replaced above.
								description.push(prev && category === "place" ?
										description.pop() + "-" + feature :
										feature);
							}
							prev = true;
						}
					}, this);
				}, this);
			}
			return description.join(" ");
		};

		// For the description method or a when condition function of a change, return a property of:
		// * formatting object which the caller provides
		// * the default formatting object
		prototypeDescriptive._descriptionFormat = function (key, argFormat) {
			return (argFormat && hasOwn.call(argFormat, key) ? argFormat : this.descriptionFormat)[key];
		};

		// distinctive methods -------------------------------------------------

		// Phonology Assistant has about 30 distinctive features.
		// For more information, see chapter 4 in Introductory Phonology.
		// A feature string consists of a name preceded by a value character.
		// * The ordinary specified values are "+" plus sign or "-" hyphen-minus.
		//   A features table or values chart displays "-" as "\u2013" en dash.
		// * A simultaneous plus and minus value can be "\u00B1" plus-minus sign.
		//   The value method returns it as one character, but the each and array methods,
		//   process it as a set of two values. For a prenasalized contour value:
		//   var features = featureQuery("distinctive", "\u00B1nas");
		//   * features.value("nas") returns "\u00B1"
		//   * features.array(["nas"]) returns ["+nas", "-nas"]
		// * An explicitly unspecified feature (in a dependency) is "0" digit zero.
		// * An implicitly unspecified feature (of a segment) is an undefined property.
		//
		// For a syllabic vowel, features.array(["syll", "cons", "son", "approx", ...])
		// returns ["+syll", "-cons", "+son", "+approx", ...]

		prototypeDistinctive._add = function (arg, char) {
			this._callNameChar(arg, char, this._addNameChar);
		};

		prototypeDistinctive._remove = function (arg, char) {
			this._callNameChar(arg, char, this._removeNameChar);
		};

		prototypeDistinctive._iterate = function (callback, context, name, method) {
			// Assume that the feature name is defined.
			return this.features[name][method](function (char) {
				return callback.call(context, this.s_feature(name, char), name, char);
			}, this); // must provide this
		};

		prototypeDistinctive._has = function (arg, char) {
			return this._callNameChar(arg, char, this._hasNameChar);
		};

		prototypeDistinctive._array = function (array, name) {
			var value = this.features[name];

			if (value) {
				value.each(function (char) {
					array.push(this.s_feature(name, value.s_charExternal(char)));
				}, this, true); // must provide this, iterate in predictable order
			}
		};

		// Return the digit character which represents the value of a feature.
		prototypeDistinctive.keyFor = function (name, changes) {
			var value;

			if (changes && changes.definedFor(name)) {
				return changes.keyFor(name);
			}
			value = this.features[name];
			return value ? value.key() : keyUnspecified;
		};

		// Get or set the (internal character) value of a feature.
		prototypeDistinctive.value = function (name, char) {
			var value;

			if (arguments.length === 1) {
				value = this.features[name];
				return value && value.charInternal(); // get returns undefined or a character
			}
			return this._change(name, this._valueQuery(name, char)); // chaining method for set
		};

		// distinctive-only methods --------------------------------------------

		// Given either a feature string or a name and a character string as arguments,
		// call a method of this with a name and a character string as arguments.
		prototypeDistinctive._callNameChar = function (arg, char, callbackNameChar) {
			var name;

			if (typeof char === "string") {
				name = arg;
			} else {
				name = this.s_name(arg);
				char = this.s_char(arg);
			}
			return callbackNameChar.call(this, name, char);
		};

		prototypeDistinctive._addNameChar = function (name, char) {
			var value = this.features[name];

			this._change(name, value ? value.add(char) : this._valueQuery(name, char));
		};

		prototypeDistinctive._removeNameChar = function (name, char) {
			var value = this.features[name];

			if (value) {
				this._change(name, value.remove(char));
			}
		};

		prototypeDistinctive._hasNameChar = function (name, char) {
			var value = this.features[name];

			return !!value && value.has(char); // return boolean
		};

		// Return the value (object) for a feature name and character:
		// * as a Value1 object if this has a value sequence for the name
		// * as a Values object otherwise
		prototypeDistinctive._valueQuery = function (name, char) {
			var valueSequences = this.valueSequences;

			return valueQuery(char, valueSequences && valueSequences[name]);
		};

		// Return whether the feature name is defined.
		prototypeDistinctive.definedFor = function (name) {
			return !!this.features[name]; // return boolean
		};

		// Set the next value in the sequence for a feature.
		// Assume that there is a value sequence for the name.
		prototypeDistinctive.nextFor = function (name) {
			var value = this.features[name] || this._valueQuery(name, "0");

			return this._change(name, value.next()); // chaining method
		};

		// Return the opposite character if only one value is specified.
		prototypeDistinctive.charInternalOpposite = function (name) {
			var value = this.features[name];

			return value && value.charInternalOpposite(); // return undefined or a character
		};

		// Change feature values as in the right side of a rule.
		// Tip: If a feature can have plus and minus (contour) values, use add or remove.
		// Tip: If you compute the key string with and without changes
		// for each segment in a phonetic inventory,
		// you can determine which segments change to and from
		// other segments without changing any of their features.
		prototypeDistinctive.change = function (arg) {
			var name;

			if (typeof arg === "string") {
				name = this.s_name(arg);
				this._change(name, this._valueQuery(name, this.s_char(arg)));
			} else { // arg is an array or object
				this._iterateBoolean(arg, this.change, false);
			}
			return this; // chaining method
		};

		// Make sure than an implicitly unspecified feature is an undefined property.
		prototypeDistinctive._change = function (name, value) {
			var features = this.features;

			if (value.deletable()) {
				delete features[name];
			} else {
				features[name] = value;
			}
			return this; // chaining method
		};

		// Return whether this matches certain features.
		// For an explicitly unspecified feature of arg, matches is true if in this either:
		// * The feature is implicitly unspecified (that is, an undefined property).
		// * The feature is explicitly unspecified (and specified neither plus nor minus).
		// Tip: To test for contradictions in features of segments, use matches instead of has.
		prototypeDistinctive.matches = function (arg) {
			if (typeof arg === "string") {
				return this._matches(arg);
			}
			// arg is an array or object
			return this._iterateBoolean(arg, this._matches, true);
		};

		prototypeDistinctive._matches = function (arg, char) {
			return this._callNameChar(arg, char, this._matchesNameChar);
		};

		prototypeDistinctive._matchesNameChar = function (name, char) {
			var value = this.features[name];

			return char === "0" ? // return boolean
					!value || value.is(char) : // is undefined or is digit zero
					!!value && value.has(char); // same as has
		};

		// Return whether the feature names exist as keys in the names object.
		// * Is a contradiction relevant for a table of features?
		// * Is a dependency relevant for a table of changes?
		prototypeDistinctive.relevantFor = function (namesObject) {
			return this.everyNameChar(function (name) {
				return namesObject[name];
			});
		};

		// For iteration methods eachNameChar, everyNameChar, someNameChar:
		// * Make no assumption about the order of the features.
		// * Provide context if the callback function refers to this.
		// * Method arguments are like YUI 3
		//   but callback arguments are different: callback(name, char)
		// * If callback does not need a feature string,
		//   these methods avoid concatenation (contrast with each, every, some).

		// Call a function for each feature.
		// Unlike jQuery, callback cannot return false to stop the iteration.
		prototypeDistinctive.eachNameChar = function (callback, context) {
			var features = this.features,
				name;

			for (name in features) {
				this._iterateNameChar(callback, context, name, "each");
			}
			return this; // chaining method
		};

		// Return whether all of the features pass a test.
		// That is, whether callback returns a truthy value for every feature.
		// Like the && operator, iteration stops if callback returns a falsy value.
		// Unlike the && operator, the result is always boolean.
		prototypeDistinctive.everyNameChar = function (callback, context) {
			var features = this.features,
				name;

			for (name in features) {
				if (!this._iterateNameChar(callback, context, name, "every")) {
					return false;
				}
			}
			return true;
		};

		// Return whether at least one feature passes a test.
		// That is, whether callback returns a truthy value for some feature.
		// Like the || operator, iteration stops if callback returns a truthy value.
		// Unlike the || operator, the result is always boolean.
		prototypeDistinctive.someNameChar = function (callback, context) {
			var features = this.features,
				name;

			for (name in features) {
				if (this._iterateNameChar(callback, context, name, "some")) {
					return true;
				}
			}
			return false;
		};

		prototypeDistinctive._iterateNameChar = function (callback, context, name, method) {
			// Assume that the feature name is defined.
			return this.features[name][method](function (char) {
				return callback.call(context, name, char);
			});
		};

		// Return the degree of similarity between this and the features argument
		// according to the optional metric.
		prototypeDistinctive.similarity = function (features, argMetric) {
			var value = 0, // of differences (might be a fraction)
				articulators = 0; // which differ (integer)

			each(argMetric || this.similarityMetric, function (featureMetric, name) {
				if (this.value(name) !== features.value(name) &&
						featureMetric.condition(this, features)) {
					value += featureMetric.value(this, features);
					if (featureMetric.articulator) {
						articulators += 1;
					}
				}
			}, this);
			return value <= 1 ? "more" :
					value <= 2 && articulators <= 1 ? "less" :
							"least";
		};

		// distinctive-only static methods -------------------------------------

		// A feature string consists of a name preceded by a value character.

		prototypeDistinctive.s_feature = function (name, char) {
			return char + name;
		};

		prototypeDistinctive.s_name = function (feature) {
			return feature.slice(1);
		};

		prototypeDistinctive.s_char = function (feature) {
			return feature.charAt(0);
		};

		return featureQuery;
	}(phonQuery.valueQuery));

	// Assign phonQuery as a property of the root object:
	// * window in a browser
	// * global on a server
	root.phonQuery = phonQuery;
}(this));

// descriptive plugins =========================================================

/*global phonQuery: false */ // for JSLint or JSHint

// An immediate function hides assignment of a default format property
// for the description method of descriptive features objects.
(function (prototypeDescriptive) {
	"use strict"; // Firefox 4 or later

	// http://jslint.com defines a professional subset of JavaScript.
	/*jslint nomen: true */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jslint indent: 4 */ // consider a tab equivalent to 4 spaces

	// http://jshint.com is a tool to detect errors and potential problems in JavaScript code.
	/*jshint trailing: true */ // warn about trailing white space
	/*jshint nomen: false */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jshint white: false */ // do not warn about indentation

	prototypeDescriptive.descriptionFormat = {
		patterns: {
			"consonant": [
				"syllabicity",
				"length",
				"nasalization",
				"onset",
				"click_modifier",
				"secondary_articulation",
				"voicing",
				"laryngeal_setting",
				"coronal_articulator",
				"backness_modifier",
				"rounding",
				"place",
				"manner_modifier",
				"manner",
				"release",
				"tone"
			],
			"monophthong": [
				"syllabicity",
				"length",
				"nasalization",
				"rhoticity",
				"secondary_articulation",
				"voicing",
				"dual_modifier",
				"height_modifier",
				"height",
				"backness_modifier",
				"backness",
				"rounding_modifier",
				"rounding",
				"manner",
				"type",
				"stricture",
				"tongue_root",
				"tone"
			]
		},

		// Phonology Assistant has about 100 descriptive features.
		// For more information, see The Sounds of the World's Languages
		// or chapter 1 in Introductory Phonology.
		categories: {
			"type": [
				"consonant",
				"vowel"
			],
			"sequence": [
				"monophthong",
				"diphthong"
			],
			"voicing": [
				"voiceless",
				"voiced",
				"breathy voiced",
				"slack voiced",
				"stiff voiced",
				"creaky voiced"
			],
			"laryngeal_setting": [
				"aspirated",
				"preaspirated",
				"glottalized",
				"preglottalized",
				"ejective"
			],
			"nasalization": [
				"nasalized",
				"prenasalized"
			],
			"place": [
				"bilabial",
				"labiodental",
				"linguolabial",
				"dental",
				"dental/alveolar",
				"alveolar",
				"postalveolar",
				"retroflex",
				"alveolo-palatal",
				"palatal",
				"velar",
				"uvular",
				"pharyngeal",
				"epiglottal",
				"glottal"
			],
			"manner": [
				"implosive",
				"plosive",
				"click",
				"affricate",
				"nasal",
				"trill",
				"tap",
				"flap",
				"fricative",
				"approximant"
			],
			"height": [
				"close",
				"near-close",
				"close-mid",
				"mid",
				"open-mid",
				"near-open",
				"open"
			],
			"backness": [
				"front",
				"near-front",
				"central",
				"near-back",
				"back"
			],
			"rounding": [
				"unrounded",
				"rounded"
			],
			"height_modifier": [
				"raised",
				"lowered"
			],
			"backness_modifier": [
				"advanced",
				"centralized",
				"retracted"
			],
			"dual_modifier": [
				"mid-centralized"
			],
			"rounding_modifier": [
				"more rounded",
				"less rounded",
				"lip-compressed"
			],
			"tongue_root": [
				"advanced tongue root",
				"retracted tongue root"
			],
			"rhoticity": [
				"rhotic"
			],
			"coronal_articulator": [
				"apical",
				"laminal"
			],
			"manner_modifier": [
				"sibilant",
				"lateral",
				"affricated"
			],
			"closure": [
				"double closure"
			],
			"secondary_articulation": [
				"labialized",
				"palatalized",
				"velarized",
				"pharyngealized"
			],
			"click_modifier": [
				"velar-fricated"
			],
			"onset": [
				"prestopped"
			],
			"release": [
				"lateral release",
				"nasal release",
				"no audible release"
			],
			"syllabicity": [
				"syllabic",
				"non-syllabic"
			],
			"length": [
				"long",
				"half-long",
				"extra-short"
			],
			"tone": [
				"extra high tone",
				"high rising tone",
				"high tone",
				"high falling tone",
				"falling tone",
				"falling-rising tone",
				"mid tone",
				"rising tone",
				"rising-falling tone",
				"low rising tone",
				"low tone",
				"low falling tone",
				"extra low tone",
				"undetermined tone"
			]
		},

		featureChanges: {
			"voiceless": [{
				when: [
					["glottal", "plosive"],
					["epiglottal", "plosive"]
				],
				replace: []
			}],
			"voiced": [{
				when: [
					["vowel"]
				],
				replace: []
			}],
			"bilabial": [{
				when: [
					function (argFormat) {
						var places = this._descriptionFormat("categories", argFormat).place,
							n = 0,
							i,
							length;

						if (this.has("click")) {
							return false;
						}
						for (i = 0, length = places.length; i < length; i += 1) {
							if (this.has(places[i])) {
								n += 1;
								if (n >= 2) {
									return true;
								}
							}
						}
						return false;
					}
				],
				replace: ["labial"]
			}],
			"alveolar": [{
				when: [
					["click"]
				],
				replace: ["(post)alveolar"]
			}],
			"postalveolar": [{
				when: [
					["click"]
				],
				replace: ["palatoalveolar"]
			}],
			"velar": [{
				when: [
					["click"]
				],
				replace: []
			}],
			"rounded": [{
				when: [
					["consonant"]
				],
				replace: ["labial"]
			}, {
				when: [
					["more rounded"],
					["less rounded"]
				],
				replace: []
			}],
			"advanced": [{
				when: [
					["velar"]
				],
				replace: ["fronted"]
			}],
			"retracted": [{
				when: [
					["velar"]
				],
				replace: ["back"]
			}]
		},

		categoryChanges: {
			"release": [{
				prepend: ["with"]
			}],
			"tongue_root": [{
				prepend: ["with"]
			}],
			"tone": [{
				prepend: ["with"]
			}]
		}
	};
}(phonQuery.featureQuery.descriptive.prototype));

// distinctive plugins =========================================================

/*global phonQuery: false */ // for JSLint or JSHint

// An immediate function hides assignment of:
// * default similarityMetric property
//   for the similarity method of distinctive features objects
// * default value sequences as a static property of the distinctive features module
//   so that it is available as an optional argument for virtual constructors:
//   * featuresQuery("distinctive", phonQuery.featureQuery.distinctive.valueSequences)
//   * valueQuery(text, phonQuery.featureQuery.distinctive.valueSequences)
(function (distinctive) {
	"use strict"; // Firefox 4 or later

	// http://jslint.com defines a professional subset of JavaScript.
	/*jslint nomen: true */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jslint indent: 4 */ // consider a tab equivalent to 4 spaces

	// http://jshint.com is a tool to detect errors and potential problems in JavaScript code.
	/*jshint trailing: true */ // warn about trailing white space
	/*jshint nomen: false */ // do not warn about preceding underscore for private methods in jQuery UI
	/*jshint white: false */ // do not warn about indentation

	var slice = Array.prototype.slice,
		hasOwn = Object.prototype.hasOwnProperty,

		// Return a new function that, when called, itself calls f
		// * with context bound to this
		// * with a given sequence of arguments
		//   preceding any provided when the new function was called
		// See pages 137-139 in JavaScript Patterns.
		// Instead of extending Function.prototype, define bind as a variable:
		// either the ECMAScript 5 function or a substitute.
		bind = Function.prototype.bind || function (context) {
			var f = this,
				args = slice.call(arguments, 1);

			return function () {
				return f.apply(context, args.concat(slice.call(arguments)));
			};
		},

		// See page 22 in JavaScript: The Good Parts or pages 131-133 in JavaScript Patterns.
		create = function (prototype, properties) {
			/*jslint forin: true */ // do not warn about unfiltered for (... in ...) loops
			var F = function () {},
				object,
				key;

			F.prototype = prototype;
			object = new F();
			if (properties) {
				for (key in properties) {
					if (hasOwn.call(properties, key)) {
						object[key] = properties[key]; // shallow copy
					}
				}
			}
			return object;
		},

		// Return whether featuresA and featuresB both have features.
		when = function (features, featuresA, featuresB) {
			return featuresA.has(features) && featuresB.has(features);
		},

		// Return whether featuresA and featuresB do not both have features.
		unless = function (features, featuresA, featuresB) {
			return !when(features, featuresA, featuresB);
		},

		ordinary = {
			articulator: false,
			value: function () { return 1; },
			condition: function () { return true; }
		},

		articulator = create(ordinary, {
			articulator: true
		}),

		// Do not count differences in nas or voice if both segments are nonconsonantal dorsal,
		// to limit their differences to height, backness, or place of articulation.
		unlessNonconsonantalDorsal = create(ordinary, {
			condition: bind.call(unless, null, ["-cons", "+dorsal"])
		}),

		// Count dorsal place differences as half if either segment is consonantal,
		// to balance differences in cont and voice which are common for consonantal segments
		// but rare for nonconsonantal segments.
		whenDorsal = create(ordinary, {
			value: function (featuresA, featuresB) {
				return when(["-cons"], featuresA, featuresB) ? 1 : 0.5;
			},
			condition: bind.call(when, null, ["+dorsal"])
		});

	distinctive.prototype.similarityMetric = {
		// major
		"cons": ordinary,
		// manner
		"cont": ordinary,
		"nas": unlessNonconsonantalDorsal,
		// labial place
		"labial": articulator,
		// coronal place
		// Do not count a coronal difference if both segments are nasals [+son, –approx].
		"coronal": create(articulator, {
			condition: bind.call(unless, null, ["+son", "-approx"])
		}),
		// Count a strid difference as double when both segments are coronal.
		"strid": create(ordinary, {
			value: function () { return 2; },
			condition: bind.call(when, null, ["+coronal"])
		}),
		// dorsal place
		"dorsal": articulator,
		"high": whenDorsal,
		"low": whenDorsal,
		"front": whenDorsal,
		"back": whenDorsal,
		// radical place
		"radical": articulator,
		// laryngeal
		"voice": unlessNonconsonantalDorsal
		// length
	};

	// Phonology Assistant has about 30 distinctive features.
	// For more information, see chapter 4 in Introductory Phonology.
	distinctive.valueSequences = {
		// major
		"syll": "+-",
		"cons": "+-",
		"son": "+-",
		"approx": "+-",
		// manner
		"cont": "+-",
		"delayed": "+-0",
		"trill": "+-",
		"tap": "+-",
		"lat": "+-",
		"nas": "+-",
		// labial place
		"labial": "+-",
		"rnd": "+-",
		"labiodental": "+-",
		// coronal place
		"coronal": "+-",
		"ant": "+-0",
		"distr": "+-0",
		"strid": "+-0",
		// dorsal place
		"dorsal": "+-",
		"high": "+-0",
		"low": "+-0",
		"tense": "+-0",
		"front": "+-0",
		"back": "+-0",
		// radical place
		"radical": "+-",
		"ATR": "+-0",
		// laryngeal
		"voice": "+-",
		"spread": "+-",
		"constr": "+-",
		"implosive": "+-",
		// length
		"long": "+-"
	};
}(phonQuery.featureQuery.distinctive));
