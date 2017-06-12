/*
Copyright Volpin Props 2016

Note: must use 'jQuery', rather than '$' alias, because Wordpress/Theme.
*/

// Define initial value for measurement units toggle
// False = inches, True = mm 
var _buttonState = false;

// State of the print view button
// Disabled by default, so we ensure we have values before printing
var _printViewEnabled = false;

// Number of decimal places we'll use for display
var _decimalPlacesMm = 1;
var _decimalPlacesInches = 3;

// Defaults
var _defaultPadding = 1;
var _defaultPlywood = .375;
var _defaultFrame = 1.5;
var _defaultPaddingMm = 25.4;
var _defaultPlywoodMm = 9.5;
var _defaultFrameMm = 38.0;

// Text to display for inches/millimeters
var _inches = "inches";
var _inchesAbbrev = "in";
var _millimeters = "millimeters";
var _millimetersAbbrev = "mm";

// Add String.format functionality
if (!String.format) {
  String.format = function(format) {
    var args = Array.prototype.slice.call(arguments, 1);
    return format.replace(/{(\d+)}/g, function(match, number) { 
      return typeof args[number] !== 'undefined'
        ? args[number] 
        : match
      ;
    });
  };
}

// Opens a new page with much simplier styling, for printing.
function DoPrint()
{
	if (!_printViewEnabled)
	{
		return;
	}
	
	var printWindow = window.open();
	if (!printWindow) {
		alert('Please enable pop-ups in order to print the unstyled version of this page.');
		return;
	}
	
	var units = (_buttonState) ? _millimetersAbbrev : _inchesAbbrev;
	
	// Default these, if the value isn't entered (or if value is invalid)
	var iPadThick = Number(jQuery('#padding-thickness').val());
	var iPlyThick = Number(jQuery('#plywood-thickness').val());
	var iBoardThick = Number(jQuery('#frame-thickness').val());
	iPadThick = (iPadThick > 0) ? iPadThick : _buttonState ? _defaultPaddingMm : _defaultPadding;
	iPlyThick = (iPlyThick > 0) ? iPlyThick : _buttonState ? _defaultPlywoodMm : _defaultPlywood;
	iBoardThick = (iBoardThick > 0) ? iBoardThick : _buttonState ? _defaultFrameMm : _defaultFrame;
	
	// Update values in the print section
	jQuery("#printItemLength").html(String.format('{0} {1}', jQuery('#item-length').val(), units));
	jQuery("#printItemWidth").html(String.format('{0} {1}', jQuery('#item-width').val(), units));
	jQuery("#printItemHeight").html(String.format('{0} {1}', jQuery('#item-height').val(), units));
	jQuery("#printPlywoodThickness").html(String.format('{0} {1}', iPlyThick, units));
	jQuery("#printFrameThickness").html(String.format('{0} {1}', iBoardThick, units));
	jQuery("#printPaddingThickness").html(String.format('{0} {1}', iPadThick, units));
	jQuery("#printFrameBy").html(String.format('{0} {1}', jQuery('#BYLen').text(), units));
	jQuery("#printFrameBz").html(String.format('{0} {1}', jQuery('#BZLen').text(), units));
	jQuery("#printFrameCy").html(String.format('{0} {1}', jQuery('#CYLen').text(), units));
	jQuery("#printFrameCz").html(String.format('{0} {1}', jQuery('#CZLen').text(), units));
	jQuery("#printPlywoodA").html(String.format('{0} {1} &times; {2} {1}', jQuery('#ALen').text(), units, jQuery('#AWid').text()));
	jQuery("#printPlywoodB").html(String.format('{0} {1} &times; {2} {1}', jQuery('#BLen').text(), units, jQuery('#BWid').text()));
	jQuery("#printPlywoodC").html(String.format('{0} {1} &times; {2} {1}', jQuery('#CLen').text(), units, jQuery('#CWid').text()));

	// Construct HTML for print view page
	var printWindowData =	'<!DOC'+'TYPE ht'+'ml>' +
	 '<ht'+'ml><he'+'ad><title>Volpin Props - Shipping Crate Calculator</title>' +
	 '<scr'+'ipt type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></scr'+'ipt>' +
	 '</he'+'ad><bo'+'dy>' +
	 '<scr'+'ipt type="text/javascript">' + 
	 '</scr'+'ipt>' +
	 jQuery('#printView').html() +
	'</bo'+'dy></ht'+'ml>';
	
	printWindow.document.write(printWindowData);
	
	// This works for simple HTML, but not for the full page we are rendering.  Disabling for now.
	//printWindow.print();
	//printWindow.close();
}

// Update text on the page to reflect some variable options
function UpdatePageText()
{
	// If the toggle is false, set to inches
	var labelUnits = (_buttonState) ? _millimetersAbbrev : _inches;
	var tipUnits = (_buttonState) ? _millimeters : _inches;
		
	// Clear out previous calculated value
	jQuery('.calculated-value').html("- -");
	
	// Update input/output unit labels
	jQuery('.output-units').html(labelUnits);
	jQuery('.input-units').html(labelUnits);
	
	// Update tooltips
	jQuery('#item-length-tip').attr("tip", "Length, in " + tipUnits + ", of the item you will be placing in the crate.");
	jQuery('#item-width-tip').attr("tip", "Width, in " + tipUnits + ", of the item you will be placing in the crate.");
	jQuery('#item-height-tip').attr("tip", "Height, in " + tipUnits + ", of the item you will be placing in the crate.");
	jQuery('#plywood-thickness-tip').attr("tip", (_buttonState) ? '9.5mm is recommended' : '3/8" is recommended');
	jQuery('#frame-thickness-tip').attr("tip", (_buttonState) ? '38mm is recommended' : '1.5" is recommended');
}

// Updates UI components for switching between measurement units
function SwitchMeasurementUnits(){
	var placeholderMm = '0.0';
	var placeholderInches = '0.000';
		
	// Disable print view when switching between units
	EnablePrintView(false);
		
	// Flip the toggle
	_buttonState = !_buttonState;
	
	// Null out all current values when toggling between units
	// We don't wish to convert them
	jQuery('#item-length').val(null);
	jQuery('#item-width').val(null);
	jQuery('#item-height').val(null);
	jQuery('#item-length').attr("placeholder", _buttonState ? placeholderMm : placeholderInches);
	jQuery('#item-width').attr("placeholder", _buttonState ? placeholderMm : placeholderInches);
	jQuery('#item-height').attr("placeholder", _buttonState ? placeholderMm : placeholderInches);
	jQuery('#plywood-thickness').val(null);
	jQuery('#frame-thickness').val(null);
	jQuery('#padding-thickness').val(null);
	jQuery('#plywood-thickness').attr("placeholder", _buttonState ? "9.5" : "0.375");
	jQuery('#frame-thickness').attr("placeholder", _buttonState ? "38.0" : "1.500");
	jQuery('#padding-thickness').attr("placeholder", _buttonState ? "25.4" : "1.000");

	UpdatePageText();
}

// Normalize numbers, and ensure we have no negatives
function NormalizeMeasurements(element)
{
	// Take absolute value first, then parse, otherwise the decimal places are stripped
	element.value = parseFloat(Math.abs(element.value)).toFixed((_buttonState) ? _decimalPlacesMm : _decimalPlacesInches);
	
	// Disable print view, since values have changed
	EnablePrintView(false);
}

// Main calculation
function CalculateCrate(){
	var iLenOfObj = Number(jQuery('#item-length').val());
	var iHighOfObj = Number(jQuery('#item-height').val());
	var iWidOfObj = Number(jQuery('#item-width').val());
	var iPadThick = Number(jQuery('#padding-thickness').val());
	var iPlyThick = Number(jQuery('#plywood-thickness').val());
	var iBoardThick = Number(jQuery('#frame-thickness').val());

	// Default these, if the value isn't entered (or if value is invalid)
	iPadThick = iPadThick > 0 ? iPadThick : _defaultPadding;
	iPlyThick = iPlyThick > 0 ? iPlyThick : _defaultPlywood;
	iBoardThick = iBoardThick > 0 ? iBoardThick : _defaultFrame;

	// Don't calculate if any of these are invalid
	if (iLenOfObj <= 0)
	{
		jQuery('#error').text("Item Length must be greater than 0");
		jQuery('#item-length').focus();
		return;
	}
	if (iWidOfObj <= 0)
	{
		jQuery('#error').text("Item Width must be greater than 0");
		jQuery('#item-width').focus();
		return;
	}
	if (iHighOfObj <= 0)
	{
		jQuery('#error').text("Item Height must be greater than 0");
		jQuery('#item-height').focus();
		return;
	}

	// Ensure error div is empty, if all checked conditions are clear
	jQuery('#error').text('\xa0');

	// Do the math for all calculated pieces
	
	// Plywood piece A - top and bottom of crate
	// - Dimensions are equal to length/width of item, plus padding, plus plywood, plus frame
	var iALen = iLenOfObj+(2*iPlyThick)+(2*iBoardThick)+(2*iPadThick);
	var iAWid = iWidOfObj+(2*iPlyThick)+(2*iBoardThick)+(2*iPadThick);

	// Plywood piece B - ends of the crate
	// - Length is equal to width of item, plus padding, plus plywood, plus frame (it covers C)
	// - Width (height) is equal to height of item, plus padding
	var iBLen = iWidOfObj+(2*iPlyThick)+(2*iBoardThick)+(2*iPadThick);
	var iBWid = iHighOfObj+(2*iPadThick);

	// Plywood Piece C - sides of the crate
	// - Dimensions are equal to length/height of item, plus padding
	var iCLen = iLenOfObj+(2*iPadThick);
	var iCWid = iHighOfObj+(2*iPadThick);

	// Smaller Frame (end frames)
	var iBYLen = iBLen; // Yellow - Equal to length of plywood piece B
	var iBZLen = iBWid-(2*iBoardThick); // Red - Equal to width of plywood piece B - (2 x the frame board thickness)

	// Larger Frame (side frames)
	var iCYLen = iCLen; // Green - Equal to length of plywood piece C
	var iCZLen = iBZLen; // Red - Equal to iBZLen

	// Determine the maximum number of decimal places to show
	var decimalPlaces = (_buttonState) ? _decimalPlacesMm : _decimalPlacesInches;

	// Update all of the display values, stripping trailing 0s, as necessary
	jQuery('#BYLen').text(StripExtraDigits(iBYLen.toFixed(decimalPlaces)));
	jQuery('#BZLen').text(StripExtraDigits(iBZLen.toFixed(decimalPlaces)));
	jQuery('#CYLen').text(StripExtraDigits(iCYLen.toFixed(decimalPlaces)));
	jQuery('#CZLen').text(StripExtraDigits(iCZLen.toFixed(decimalPlaces)));
	jQuery('#ALen').text(StripExtraDigits(iALen.toFixed(decimalPlaces)));
	jQuery('#AWid').text(StripExtraDigits(iAWid.toFixed(decimalPlaces)));
	jQuery('#BLen').text(StripExtraDigits(iBLen.toFixed(decimalPlaces)));
	jQuery('#BWid').text(StripExtraDigits(iBWid.toFixed(decimalPlaces)));
	jQuery('#CLen').text(StripExtraDigits(iCLen.toFixed(decimalPlaces)));
	jQuery('#CWid').text(StripExtraDigits(iCWid.toFixed(decimalPlaces)));
	
	// Get the width of the widest calculated value
	var width = Math.max.apply(Math, jQuery('.calculated-value').map(function(){ return jQuery(this).width(); }).get()).toFixed(2);
	
	// Apply the widest width to all calculated value spans, so they are the same
	jQuery('.calculated-value').css('width', width);
	
	// Enable print view button
	EnablePrintView(true);
}

// Changes the print view button style to enable/disable
function EnablePrintView (enable)
{
	if (!enable)
	{
		_printViewEnabled = false;
		// prevent adding this twice
		jQuery("#printViewButton").removeClass("submitButtonDisabled");
		jQuery("#printViewButton").addClass("submitButtonDisabled");
		return;
	}
	
	_printViewEnabled = true;
	jQuery("#printViewButton").removeClass("submitButtonDisabled");
}

// Removes trailing 0 from the calculated numbers
// They aren't necessary, and it preserves space
// e.g. 3.750 will be 3.75, and 12.000 will be 12
function StripExtraDigits(digits)
{
	var strDigits = digits.toString();
	
	// Remove all trailing 0
	while (strDigits.endsWith('0'))
	{	
		strDigits = strDigits.toString().slice(0,-1);
	}
	
	// If all digits after the decimal were removed, also remove the decimal
	if (strDigits.endsWith('.'))
	{
		strDigits = strDigits.toString().slice(0,-1);
	}
	
	return strDigits;
}

jQuery(document).ready(function(){
	// Change the look of the label, when its corresponding field has focus
	jQuery("div :input").focus(function() {
		jQuery("label[for='" + this.id + "']").addClass("label-focus");
	}).blur(function() {
		jQuery("label").removeClass("label-focus");
	});

	// Set focus to the first input field, when the page loads
	jQuery("#item-length").focus();

	// Make entire div clickable, to enter the text field
	jQuery('.input-column').click(function() {
		jQuery(this).find('input[type="text"]').focus();
	});

	// Tooltips
	jQuery("span.question").hover(function() {
		jQuery(this).append('<div class="tooltip"><p>' + jQuery(this).attr("tip") + '</p></div>');
	}, function () {
		jQuery("div.tooltip").remove();
	});
});
