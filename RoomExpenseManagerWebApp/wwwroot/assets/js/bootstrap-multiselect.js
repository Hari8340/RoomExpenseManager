﻿(function (root, factory) { if (typeof define === 'function' && define.amd && typeof require === 'function' && typeof require.specified === 'function' && require.specified('knockout')) { define(['jquery', 'knockout'], factory); } else { factory(root.jQuery, root.ko); } })(this, function ($, ko) {
    "use strict"; if (typeof ko !== 'undefined' && ko.bindingHandlers && !ko.bindingHandlers.multiselect) {
        ko.bindingHandlers.multiselect = {
            after: ['options', 'value', 'selectedOptions', 'enable', 'disable'], init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var $element = $(element); var config = ko.toJS(valueAccessor()); $element.multiselect(config); if (allBindings.has('options')) {
                    var options = allBindings.get('options'); if (ko.isObservable(options)) {
                        ko.computed({
                            read: function () {
                                options(); setTimeout(function () {
                                    var ms = $element.data('multiselect'); if (ms)
                                        ms.updateOriginalOptions(); $element.multiselect('rebuild');
                                }, 1);
                            }, disposeWhenNodeIsRemoved: element
                        });
                    }
                }
                if (allBindings.has('value')) { var value = allBindings.get('value'); if (ko.isObservable(value)) { ko.computed({ read: function () { value(); setTimeout(function () { $element.multiselect('refresh'); }, 1); }, disposeWhenNodeIsRemoved: element }).extend({ rateLimit: 100, notifyWhenChangesStop: true }); } }
                if (allBindings.has('selectedOptions')) { var selectedOptions = allBindings.get('selectedOptions'); if (ko.isObservable(selectedOptions)) { ko.computed({ read: function () { selectedOptions(); setTimeout(function () { $element.multiselect('refresh'); }, 1); }, disposeWhenNodeIsRemoved: element }).extend({ rateLimit: 100, notifyWhenChangesStop: true }); } }
                var setEnabled = function (enable) {
                    setTimeout(function () {
                        if (enable)
                            $element.multiselect('enable'); else
                            $element.multiselect('disable');
                    });
                }; if (allBindings.has('enable')) { var enable = allBindings.get('enable'); if (ko.isObservable(enable)) { ko.computed({ read: function () { setEnabled(enable()); }, disposeWhenNodeIsRemoved: element }).extend({ rateLimit: 100, notifyWhenChangesStop: true }); } else { setEnabled(enable); } }
                if (allBindings.has('disable')) { var disable = allBindings.get('disable'); if (ko.isObservable(disable)) { ko.computed({ read: function () { setEnabled(!disable()); }, disposeWhenNodeIsRemoved: element }).extend({ rateLimit: 100, notifyWhenChangesStop: true }); } else { setEnabled(!disable); } }
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () { $element.multiselect('destroy'); });
            }, update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { var $element = $(element); var config = ko.toJS(valueAccessor()); $element.multiselect('setOptions', config); $element.multiselect('rebuild'); }
        };
    }
    function forEach(array, callback) { for (var index = 0; index < array.length; ++index) { callback(array[index], index); } }
    function Multiselect(select, options) {
        this.$select = $(select); this.options = this.mergeOptions($.extend({}, options, this.$select.data())); if (this.$select.attr("data-placeholder")) { this.options.nonSelectedText = this.$select.data("placeholder"); }
        this.originalOptions = this.$select.clone()[0].options; this.query = ''; this.searchTimeout = null; this.lastToggledInput = null; this.options.multiple = this.$select.attr('multiple') === "multiple"; this.options.onChange = $.proxy(this.options.onChange, this); this.options.onSelectAll = $.proxy(this.options.onSelectAll, this); this.options.onDeselectAll = $.proxy(this.options.onDeselectAll, this); this.options.onDropdownShow = $.proxy(this.options.onDropdownShow, this); this.options.onDropdownHide = $.proxy(this.options.onDropdownHide, this); this.options.onDropdownShown = $.proxy(this.options.onDropdownShown, this); this.options.onDropdownHidden = $.proxy(this.options.onDropdownHidden, this); this.options.onInitialized = $.proxy(this.options.onInitialized, this); this.options.onFiltering = $.proxy(this.options.onFiltering, this); this.buildContainer(); this.buildButton(); this.buildDropdown(); this.buildReset(); this.buildSelectAll(); this.buildDropdownOptions(); this.buildFilter(); this.updateButtonText(); this.updateSelectAll(true); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
        this.options.wasDisabled = this.$select.prop('disabled'); if (this.options.disableIfEmpty && $('option', this.$select).length <= 0) { this.disable(); }
        this.$select.wrap('<span class="multiselect-native-select" />').after(this.$container); this.options.onInitialized(this.$select, this.$container);
    }
    Multiselect.prototype = {
        defaults: {
            buttonText: function (options, select) {
                if (this.disabledText.length > 0 && (select.prop('disabled') || (options.length == 0 && this.disableIfEmpty))) { return this.disabledText; }
                else if (options.length === 0) { return this.nonSelectedText; }
                else if (this.allSelectedText && options.length === $('option', $(select)).length && $('option', $(select)).length !== 1 && this.multiple) {
                    if (this.selectAllNumber) { return this.allSelectedText + ' (' + options.length + ')'; }
                    else { return this.allSelectedText; }
                }
                else if (this.numberDisplayed != 0 && options.length > this.numberDisplayed) { return options.length + ' ' + this.nSelectedText; }
                else { var selected = ''; var delimiter = this.delimiterText; options.each(function () { var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).text(); selected += label + delimiter; }); return selected.substr(0, selected.length - this.delimiterText.length); }
            }, buttonTitle: function (options, select) {
                if (options.length === 0) { return this.nonSelectedText; }
                else { var selected = ''; var delimiter = this.delimiterText; options.each(function () { var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).text(); selected += label + delimiter; }); return selected.substr(0, selected.length - this.delimiterText.length); }
            }, checkboxName: function (option) { return false; }, optionLabel: function (element) { return $(element).attr('label') || $(element).text(); }, optionClass: function (element) { return $(element).attr('class') || ''; }, onChange: function (option, checked) { }, onDropdownShow: function (event) { }, onDropdownHide: function (event) { }, onDropdownShown: function (event) { }, onDropdownHidden: function (event) { }, onSelectAll: function () { }, onDeselectAll: function () { }, onInitialized: function ($select, $container) { }, onFiltering: function ($filter) { }, enableHTML: false, buttonClass: 'btn btn-default', inheritClass: false, buttonWidth: 'auto', buttonContainer: '<div class="btn-group" />', dropRight: false, dropUp: false, selectedClass: 'active', maxHeight: false, includeSelectAllOption: false, includeSelectAllIfMoreThan: 0, selectAllText: ' Select all', selectAllValue: 'multiselect-all', selectAllName: false, selectAllNumber: true, selectAllJustVisible: true, enableFiltering: false, enableCaseInsensitiveFiltering: false, enableFullValueFiltering: false, enableClickableOptGroups: false, enableCollapsibleOptGroups: false, collapseOptGroupsByDefault: false, filterPlaceholder: 'Search', filterBehavior: 'text', includeFilterClearBtn: true, preventInputChangeEvent: false, nonSelectedText: 'None selected', nSelectedText: 'selected', allSelectedText: 'All selected', numberDisplayed: 3, disableIfEmpty: false, disabledText: '', delimiterText: ', ', includeResetOption: false, includeResetDivider: false, resetText: 'Reset', templates: { button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> <b class="caret"></b></button>', ul: '<ul class="multiselect-container dropdown-menu"></ul>', filter: '<li class="multiselect-item multiselect-filter"><div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form-control multiselect-search" type="text" /></div></li>', filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default multiselect-clear-filter" type="button"><i class="glyphicon glyphicon-remove-circle"></i></button></span>', li: '<li><a tabindex="0"><label></label></a></li>', divider: '<li class="multiselect-item divider"></li>', liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>', resetButton: '<li class="multiselect-reset text-center"><div class="input-group"><a class="btn btn-default btn-block"></a></div></li>' }
        }, constructor: Multiselect, buildContainer: function () { this.$container = $(this.options.buttonContainer); this.$container.on('show.bs.dropdown', this.options.onDropdownShow); this.$container.on('hide.bs.dropdown', this.options.onDropdownHide); this.$container.on('shown.bs.dropdown', this.options.onDropdownShown); this.$container.on('hidden.bs.dropdown', this.options.onDropdownHidden); }, buildButton: function () {
            this.$button = $(this.options.templates.button).addClass(this.options.buttonClass); if (this.$select.attr('class') && this.options.inheritClass) { this.$button.addClass(this.$select.attr('class')); }
            if (this.$select.prop('disabled')) { this.disable(); }
            else { this.enable(); }
            if (this.options.buttonWidth && this.options.buttonWidth !== 'auto') { this.$button.css({ 'width': '100%', 'overflow': 'hidden', 'text-overflow': 'ellipsis' }); this.$container.css({ 'width': this.options.buttonWidth }); }
            var tabindex = this.$select.attr('tabindex'); if (tabindex) { this.$button.attr('tabindex', tabindex); }
            this.$container.prepend(this.$button);
        }, buildDropdown: function () {
            this.$ul = $(this.options.templates.ul); if (this.options.dropRight) { this.$ul.addClass('pull-right'); }
            if (this.options.maxHeight) { this.$ul.css({ 'max-height': this.options.maxHeight + 'px', 'overflow-y': 'auto', 'overflow-x': 'hidden' }); }
            if (this.options.dropUp) { var height = Math.min(this.options.maxHeight, $('option[data-role!="divider"]', this.$select).length * 26 + $('option[data-role="divider"]', this.$select).length * 19 + (this.options.includeSelectAllOption ? 26 : 0) + (this.options.enableFiltering || this.options.enableCaseInsensitiveFiltering ? 44 : 0)); var moveCalc = height + 34; this.$ul.css({ 'max-height': height + 'px', 'overflow-y': 'auto', 'overflow-x': 'hidden', 'margin-top': "-" + moveCalc + 'px' }); }
            this.$container.append(this.$ul);
        }, buildDropdownOptions: function () {
            this.$select.children().each($.proxy(function (index, element) {
                var $element = $(element); var tag = $element.prop('tagName').toLowerCase(); if ($element.prop('value') === this.options.selectAllValue) { return; }
                if (tag === 'optgroup') { this.createOptgroup(element); }
                else if (tag === 'option') {
                    if ($element.data('role') === 'divider') { this.createDivider(); }
                    else { this.createOptionValue(element); }
                }
            }, this)); $(this.$ul).off('change', 'li:not(.multiselect-group) input[type="checkbox"], li:not(.multiselect-group) input[type="radio"]'); $(this.$ul).on('change', 'li:not(.multiselect-group) input[type="checkbox"], li:not(.multiselect-group) input[type="radio"]', $.proxy(function (event) {
                var $target = $(event.target); var checked = $target.prop('checked') || false; var isSelectAllOption = $target.val() === this.options.selectAllValue; if (this.options.selectedClass) {
                    if (checked) { $target.closest('li').addClass(this.options.selectedClass); }
                    else { $target.closest('li').removeClass(this.options.selectedClass); }
                }
                var value = $target.val(); var $option = this.getOptionByValue(value); var $optionsNotThis = $('option', this.$select).not($option); var $checkboxesNotThis = $('input', this.$container).not($target); if (isSelectAllOption) {
                    if (checked) { this.selectAll(this.options.selectAllJustVisible, true); }
                    else { this.deselectAll(this.options.selectAllJustVisible, true); }
                }
                else {
                    if (checked) {
                        $option.prop('selected', true); if (this.options.multiple) { $option.prop('selected', true); }
                        else {
                            if (this.options.selectedClass) { $($checkboxesNotThis).closest('li').removeClass(this.options.selectedClass); }
                            $($checkboxesNotThis).prop('checked', false); $optionsNotThis.prop('selected', false); this.$button.click();
                        }
                        if (this.options.selectedClass === "active") { $optionsNotThis.closest("a").css("outline", ""); }
                    }
                    else { $option.prop('selected', false); }
                    this.options.onChange($option, checked); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
                }
                this.$select.change(); this.updateButtonText(); if (this.options.preventInputChangeEvent) { return false; }
            }, this)); $('li a', this.$ul).on('mousedown', function (e) { if (e.shiftKey) { return false; } }); $(this.$ul).on('touchstart click', 'li a', $.proxy(function (event) {
                event.stopPropagation(); var $target = $(event.target); if (event.shiftKey && this.options.multiple) {
                    if ($target.is("label")) { event.preventDefault(); $target = $target.find("input"); $target.prop("checked", !$target.prop("checked")); }
                    var checked = $target.prop('checked') || false; if (this.lastToggledInput !== null && this.lastToggledInput !== $target) {
                        var from = this.$ul.find("li:visible").index($target.parents("li")); var to = this.$ul.find("li:visible").index(this.lastToggledInput.parents("li")); if (from > to) { var tmp = to; to = from; from = tmp; }
                        ++to; var range = this.$ul.find("li").not(".multiselect-filter-hidden").slice(from, to).find("input"); range.prop('checked', checked); if (this.options.selectedClass) { range.closest('li').toggleClass(this.options.selectedClass, checked); }
                        for (var i = 0, j = range.length; i < j; i++) { var $checkbox = $(range[i]); var $option = this.getOptionByValue($checkbox.val()); $option.prop('selected', checked); }
                    }
                    $target.trigger("change");
                }
                if ($target.is("input") && !$target.closest("li").is(".multiselect-item")) { this.lastToggledInput = $target; }
                $target.blur();
            }, this)); this.$container.off('keydown.multiselect').on('keydown.multiselect', $.proxy(function (event) {
                if ($('input[type="text"]', this.$container).is(':focus')) { return; }
                if (event.keyCode === 9 && this.$container.hasClass('open')) { this.$button.click(); }
                else {
                    var $items = $(this.$container).find("li:not(.divider):not(.disabled) a").filter(":visible"); if (!$items.length) { return; }
                    var index = $items.index($items.filter(':focus')); if (event.keyCode === 38 && index > 0) { index--; }
                    else if (event.keyCode === 40 && index < $items.length - 1) { index++; }
                    else if (!~index) { index = 0; }
                    var $current = $items.eq(index); $current.focus(); if (event.keyCode === 32 || event.keyCode === 13) { var $checkbox = $current.find('input'); $checkbox.prop("checked", !$checkbox.prop("checked")); $checkbox.change(); }
                    event.stopPropagation(); event.preventDefault();
                }
            }, this)); if (this.options.enableClickableOptGroups && this.options.multiple) {
                $("li.multiselect-group input", this.$ul).on("change", $.proxy(function (event) {
                    event.stopPropagation(); var $target = $(event.target); var checked = $target.prop('checked') || false; var $li = $(event.target).closest('li'); var $group = $li.nextUntil("li.multiselect-group").not('.multiselect-filter-hidden').not('.disabled'); var $inputs = $group.find("input"); var values = []; var $options = []; if (this.options.selectedClass) {
                        if (checked) { $li.addClass(this.options.selectedClass); }
                        else { $li.removeClass(this.options.selectedClass); }
                    }
                    $.each($inputs, $.proxy(function (index, input) {
                        var value = $(input).val(); var $option = this.getOptionByValue(value); if (checked) { $(input).prop('checked', true); $(input).closest('li').addClass(this.options.selectedClass); $option.prop('selected', true); }
                        else { $(input).prop('checked', false); $(input).closest('li').removeClass(this.options.selectedClass); $option.prop('selected', false); }
                        $options.push(this.getOptionByValue(value));
                    }, this))
                    this.options.onChange($options, checked); this.$select.change(); this.updateButtonText(); this.updateSelectAll();
                }, this));
            }
            if (this.options.enableCollapsibleOptGroups && this.options.multiple) {
                $("li.multiselect-group .caret-container", this.$ul).on("click", $.proxy(function (event) {
                    var $li = $(event.target).closest('li'); var $inputs = $li.nextUntil("li.multiselect-group").not('.multiselect-filter-hidden'); var visible = true; $inputs.each(function () { visible = visible && !$(this).hasClass('multiselect-collapsible-hidden'); }); if (visible) { $inputs.hide().addClass('multiselect-collapsible-hidden'); }
                    else { $inputs.show().removeClass('multiselect-collapsible-hidden'); }
                }, this)); $("li.multiselect-all", this.$ul).css('background', '#f3f3f3').css('border-bottom', '1px solid #eaeaea'); $("li.multiselect-all > a > label.checkbox", this.$ul).css('padding', '3px 20px 3px 35px'); $("li.multiselect-group > a > input", this.$ul).css('margin', '4px 0px 5px -20px');
            }
        }, createOptionValue: function (element) {
            var $element = $(element); if ($element.is(':selected')) { $element.prop('selected', true); }
            var label = this.options.optionLabel(element); var classes = this.options.optionClass(element); var value = $element.val(); var inputType = this.options.multiple ? "checkbox" : "radio"; var $li = $(this.options.templates.li); var $label = $('label', $li); $label.addClass(inputType); $label.attr("title", label); $li.addClass(classes); if (this.options.collapseOptGroupsByDefault && $(element).parent().prop("tagName").toLowerCase() === "optgroup") { $li.addClass("multiselect-collapsible-hidden"); $li.hide(); }
            if (this.options.enableHTML) { $label.html(" " + label); }
            else { $label.text(" " + label); }
            var $checkbox = $('<input/>').attr('type', inputType); var name = this.options.checkboxName($element); if (name) { $checkbox.attr('name', name); }
            $label.prepend($checkbox); var selected = $element.prop('selected') || false; $checkbox.val(value); if (value === this.options.selectAllValue) { $li.addClass("multiselect-item multiselect-all"); $checkbox.parent().parent().addClass('multiselect-all'); }
            $label.attr('title', $element.attr('title')); this.$ul.append($li); if ($element.is(':disabled')) { $checkbox.attr('disabled', 'disabled').prop('disabled', true).closest('a').attr("tabindex", "-1").closest('li').addClass('disabled'); }
            $checkbox.prop('checked', selected); if (selected && this.options.selectedClass) { $checkbox.closest('li').addClass(this.options.selectedClass); }
        }, createDivider: function (element) { var $divider = $(this.options.templates.divider); this.$ul.append($divider); }, createOptgroup: function (group) {
            var label = $(group).attr("label"); var value = $(group).attr("value"); var $li = $('<li class="multiselect-item multiselect-group"><a href="javascript:void(0);"><label><b></b></label></a></li>'); var classes = this.options.optionClass(group); $li.addClass(classes); if (this.options.enableHTML) { $('label b', $li).html(" " + label); }
            else { $('label b', $li).text(" " + label); }
            if (this.options.enableCollapsibleOptGroups && this.options.multiple) { $('a', $li).append('<span class="caret-container"><b class="caret"></b></span>'); }
            if (this.options.enableClickableOptGroups && this.options.multiple) { $('a label', $li).prepend('<input type="checkbox" value="' + value + '"/>'); }
            if ($(group).is(':disabled')) { $li.addClass('disabled'); }
            this.$ul.append($li); $("option", group).each($.proxy(function ($, group) { this.createOptionValue(group); }, this))
        }, buildReset: function () {
            if (this.options.includeResetOption) {
                if (this.options.includeResetDivider) { this.$ul.prepend($(this.options.templates.divider)); }
                var $resetButton = $(this.options.templates.resetButton); if (this.options.enableHTML) { $('a', $resetButton).html(this.options.resetText); }
                else { $('a', $resetButton).text(this.options.resetText); }
                $('a', $resetButton).click($.proxy(function () { this.clearSelection(); }, this)); this.$ul.prepend($resetButton);
            }
        }, buildSelectAll: function () {
            if (typeof this.options.selectAllValue === 'number') { this.options.selectAllValue = this.options.selectAllValue.toString(); }
            var alreadyHasSelectAll = this.hasSelectAll(); if (!alreadyHasSelectAll && this.options.includeSelectAllOption && this.options.multiple && $('option', this.$select).length > this.options.includeSelectAllIfMoreThan) {
                if (this.options.includeSelectAllDivider) { this.$ul.prepend($(this.options.templates.divider)); }
                var $li = $(this.options.templates.li); $('label', $li).addClass("checkbox"); if (this.options.enableHTML) { $('label', $li).html(" " + this.options.selectAllText); }
                else { $('label', $li).text(" " + this.options.selectAllText); }
                if (this.options.selectAllName) { $('label', $li).prepend('<input type="checkbox" name="' + this.options.selectAllName + '" />'); }
                else { $('label', $li).prepend('<input type="checkbox" />'); }
                var $checkbox = $('input', $li); $checkbox.val(this.options.selectAllValue); $li.addClass("multiselect-item multiselect-all"); $checkbox.parent().parent().addClass('multiselect-all'); this.$ul.prepend($li); $checkbox.prop('checked', false);
            }
        }, buildFilter: function () {
            if (this.options.enableFiltering || this.options.enableCaseInsensitiveFiltering) {
                var enableFilterLength = Math.max(this.options.enableFiltering, this.options.enableCaseInsensitiveFiltering); if (this.$select.find('option').length >= enableFilterLength) {
                    this.$filter = $(this.options.templates.filter); $('input', this.$filter).attr('placeholder', this.options.filterPlaceholder); if (this.options.includeFilterClearBtn) { var clearBtn = $(this.options.templates.filterClearBtn); clearBtn.on('click', $.proxy(function (event) { clearTimeout(this.searchTimeout); this.query = ''; this.$filter.find('.multiselect-search').val(''); $('li', this.$ul).show().removeClass('multiselect-filter-hidden'); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); } }, this)); this.$filter.find('.input-group').append(clearBtn); }
                    this.$ul.prepend(this.$filter); this.$filter.val(this.query).on('click', function (event) { event.stopPropagation(); }).on('input keydown', $.proxy(function (event) {
                        if (event.which === 13) { event.preventDefault(); }
                        clearTimeout(this.searchTimeout); this.searchTimeout = this.asyncFunction($.proxy(function () {
                            if (this.query !== event.target.value) {
                                this.query = event.target.value; var currentGroup, currentGroupVisible; $.each($('li', this.$ul), $.proxy(function (index, element) {
                                    var value = $('input', element).length > 0 ? $('input', element).val() : ""; var text = $('label', element).text(); var filterCandidate = ''; if ((this.options.filterBehavior === 'text')) { filterCandidate = text; }
                                    else if ((this.options.filterBehavior === 'value')) { filterCandidate = value; }
                                    else if (this.options.filterBehavior === 'both') { filterCandidate = text + '\n' + value; }
                                    if (value !== this.options.selectAllValue && text) {
                                        var showElement = false; if (this.options.enableCaseInsensitiveFiltering) { filterCandidate = filterCandidate.toLowerCase(); this.query = this.query.toLowerCase(); }
                                        if (this.options.enableFullValueFiltering && this.options.filterBehavior !== 'both') { var valueToMatch = filterCandidate.trim().substring(0, this.query.length); if (this.query.indexOf(valueToMatch) > -1) { showElement = true; } }
                                        else if (filterCandidate.indexOf(this.query) > -1) { showElement = true; }
                                        if (!showElement) { $(element).css('display', 'none'); $(element).addClass('multiselect-filter-hidden'); }
                                        if (showElement) { $(element).css('display', 'block'); $(element).removeClass('multiselect-filter-hidden'); }
                                        if ($(element).hasClass('multiselect-group')) { currentGroup = element; currentGroupVisible = showElement; }
                                        else {
                                            if (showElement) { $(currentGroup).show().removeClass('multiselect-filter-hidden'); }
                                            if (!showElement && currentGroupVisible) { $(element).show().removeClass('multiselect-filter-hidden'); }
                                        }
                                    }
                                }, this));
                            }
                            this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
                            this.options.onFiltering(event.target);
                        }, this), 300, this);
                    }, this));
                }
            }
        }, destroy: function () { this.$container.remove(); this.$select.show(); this.$select.prop('disabled', this.options.wasDisabled); this.$select.data('multiselect', null); }, refresh: function () {
            var inputs = {}; $('li input', this.$ul).each(function () { inputs[$(this).val()] = $(this); }); $('option', this.$select).each($.proxy(function (index, element) {
                var $elem = $(element); var $input = inputs[$(element).val()]; if ($elem.is(':selected')) { $input.prop('checked', true); if (this.options.selectedClass) { $input.closest('li').addClass(this.options.selectedClass); } }
                else { $input.prop('checked', false); if (this.options.selectedClass) { $input.closest('li').removeClass(this.options.selectedClass); } }
                if ($elem.is(":disabled")) { $input.attr('disabled', 'disabled').prop('disabled', true).closest('li').addClass('disabled'); }
                else { $input.prop('disabled', false).closest('li').removeClass('disabled'); }
            }, this)); this.updateButtonText(); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
        }, select: function (selectValues, triggerOnChange) {
            if (!$.isArray(selectValues)) { selectValues = [selectValues]; }
            for (var i = 0; i < selectValues.length; i++) {
                var value = selectValues[i]; if (value === null || value === undefined) { continue; }
                var $option = this.getOptionByValue(value); var $checkbox = this.getInputByValue(value); if ($option === undefined || $checkbox === undefined) { continue; }
                if (!this.options.multiple) { this.deselectAll(false); }
                if (this.options.selectedClass) { $checkbox.closest('li').addClass(this.options.selectedClass); }
                $checkbox.prop('checked', true); $option.prop('selected', true); if (triggerOnChange) { this.options.onChange($option, true); }
            }
            this.updateButtonText(); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
        }, clearSelection: function () { this.deselectAll(false); this.updateButtonText(); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); } }, deselect: function (deselectValues, triggerOnChange) {
            if (!$.isArray(deselectValues)) { deselectValues = [deselectValues]; }
            for (var i = 0; i < deselectValues.length; i++) {
                var value = deselectValues[i]; if (value === null || value === undefined) { continue; }
                var $option = this.getOptionByValue(value); var $checkbox = this.getInputByValue(value); if ($option === undefined || $checkbox === undefined) { continue; }
                if (this.options.selectedClass) { $checkbox.closest('li').removeClass(this.options.selectedClass); }
                $checkbox.prop('checked', false); $option.prop('selected', false); if (triggerOnChange) { this.options.onChange($option, false); }
            }
            this.updateButtonText(); this.updateSelectAll(); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
        }, selectAll: function (justVisible, triggerOnSelectAll) {
            var justVisible = typeof justVisible === 'undefined' ? true : justVisible; var allLis = $("li:not(.divider):not(.disabled):not(.multiselect-group)", this.$ul); var visibleLis = $("li:not(.divider):not(.disabled):not(.multiselect-group):not(.multiselect-filter-hidden):not(.multiselect-collapisble-hidden)", this.$ul).filter(':visible'); if (justVisible) { $('input:enabled', visibleLis).prop('checked', true); visibleLis.addClass(this.options.selectedClass); $('input:enabled', visibleLis).each($.proxy(function (index, element) { var value = $(element).val(); var option = this.getOptionByValue(value); $(option).prop('selected', true); }, this)); }
            else { $('input:enabled', allLis).prop('checked', true); allLis.addClass(this.options.selectedClass); $('input:enabled', allLis).each($.proxy(function (index, element) { var value = $(element).val(); var option = this.getOptionByValue(value); $(option).prop('selected', true); }, this)); }
            $('li input[value="' + this.options.selectAllValue + '"]', this.$ul).prop('checked', true); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
            if (triggerOnSelectAll) { this.options.onSelectAll(); }
        }, deselectAll: function (justVisible, triggerOnDeselectAll) {
            var justVisible = typeof justVisible === 'undefined' ? true : justVisible; var allLis = $("li:not(.divider):not(.disabled):not(.multiselect-group)", this.$ul); var visibleLis = $("li:not(.divider):not(.disabled):not(.multiselect-group):not(.multiselect-filter-hidden):not(.multiselect-collapisble-hidden)", this.$ul).filter(':visible'); if (justVisible) { $('input[type="checkbox"]:enabled', visibleLis).prop('checked', false); visibleLis.removeClass(this.options.selectedClass); $('input[type="checkbox"]:enabled', visibleLis).each($.proxy(function (index, element) { var value = $(element).val(); var option = this.getOptionByValue(value); $(option).prop('selected', false); }, this)); }
            else { $('input[type="checkbox"]:enabled', allLis).prop('checked', false); allLis.removeClass(this.options.selectedClass); $('input[type="checkbox"]:enabled', allLis).each($.proxy(function (index, element) { var value = $(element).val(); var option = this.getOptionByValue(value); $(option).prop('selected', false); }, this)); }
            $('li input[value="' + this.options.selectAllValue + '"]', this.$ul).prop('checked', false); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
            if (triggerOnDeselectAll) { this.options.onDeselectAll(); }
        }, rebuild: function () {
            this.$ul.html(''); this.options.multiple = this.$select.attr('multiple') === "multiple"; this.buildSelectAll(); this.buildDropdownOptions(); this.buildFilter(); this.updateButtonText(); this.updateSelectAll(true); if (this.options.enableClickableOptGroups && this.options.multiple) { this.updateOptGroups(); }
            if (this.options.disableIfEmpty && $('option', this.$select).length <= 0) { this.disable(); }
            else { this.enable(); }
            if (this.options.dropRight) { this.$ul.addClass('pull-right'); }
        }, dataprovider: function (dataprovider) {
            var groupCounter = 0; var $select = this.$select.empty(); $.each(dataprovider, function (index, option) {
                var $tag; if ($.isArray(option.children)) {
                    groupCounter++; $tag = $('<optgroup/>').attr({ label: option.label || 'Group ' + groupCounter, disabled: !!option.disabled, value: option.value }); forEach(option.children, function (subOption) {
                        var attributes = { value: subOption.value, label: subOption.label || subOption.value, title: subOption.title, selected: !!subOption.selected, disabled: !!subOption.disabled }; for (var key in subOption.attributes) { attributes['data-' + key] = subOption.attributes[key]; }
                        $tag.append($('<option/>').attr(attributes));
                    });
                }
                else {
                    var attributes = { 'value': option.value, 'label': option.label || option.value, 'title': option.title, 'class': option['class'], 'selected': !!option['selected'], 'disabled': !!option['disabled'] }; for (var key in option.attributes) { attributes['data-' + key] = option.attributes[key]; }
                    $tag = $('<option/>').attr(attributes); $tag.text(option.label || option.value);
                }
                $select.append($tag);
            }); this.rebuild();
        }, enable: function () { this.$select.prop('disabled', false); this.$button.prop('disabled', false).removeClass('disabled'); }, disable: function () { this.$select.prop('disabled', true); this.$button.prop('disabled', true).addClass('disabled'); }, setOptions: function (options) { this.options = this.mergeOptions(options); }, mergeOptions: function (options) { return $.extend(true, {}, this.defaults, this.options, options); }, hasSelectAll: function () { return $('li.multiselect-all', this.$ul).length > 0; }, updateOptGroups: function () {
            var $groups = $('li.multiselect-group', this.$ul)
            var selectedClass = this.options.selectedClass; $groups.each(function () {
                var $options = $(this).nextUntil('li.multiselect-group').not('.multiselect-filter-hidden').not('.disabled'); var checked = true; $options.each(function () { var $input = $('input', this); if (!$input.prop('checked')) { checked = false; } }); if (selectedClass) {
                    if (checked) { $(this).addClass(selectedClass); }
                    else { $(this).removeClass(selectedClass); }
                }
                $('input', this).prop('checked', checked);
            });
        }, updateSelectAll: function (notTriggerOnSelectAll) {
            if (this.hasSelectAll()) {
                var allBoxes = $("li:not(.multiselect-item):not(.multiselect-filter-hidden):not(.multiselect-group):not(.disabled) input:enabled", this.$ul); var allBoxesLength = allBoxes.length; var checkedBoxesLength = allBoxes.filter(":checked").length; var selectAllLi = $("li.multiselect-all", this.$ul); var selectAllInput = selectAllLi.find("input"); if (checkedBoxesLength > 0 && checkedBoxesLength === allBoxesLength) { selectAllInput.prop("checked", true); selectAllLi.addClass(this.options.selectedClass); }
                else { selectAllInput.prop("checked", false); selectAllLi.removeClass(this.options.selectedClass); }
            }
        }, updateButtonText: function () {
            var options = this.getSelected(); if (this.options.enableHTML) { $('.multiselect .multiselect-selected-text', this.$container).html(this.options.buttonText(options, this.$select)); }
            else { $('.multiselect .multiselect-selected-text', this.$container).text(this.options.buttonText(options, this.$select)); }
            $('.multiselect', this.$container).attr('title', this.options.buttonTitle(options, this.$select));
        }, getSelected: function () { return $('option', this.$select).filter(":selected"); }, getOptionByValue: function (value) { var options = $('option', this.$select); var valueToCompare = value.toString(); for (var i = 0; i < options.length; i = i + 1) { var option = options[i]; if (option.value === valueToCompare) { return $(option); } } }, getInputByValue: function (value) { var checkboxes = $('li input:not(.multiselect-search)', this.$ul); var valueToCompare = value.toString(); for (var i = 0; i < checkboxes.length; i = i + 1) { var checkbox = checkboxes[i]; if (checkbox.value === valueToCompare) { return $(checkbox); } } }, updateOriginalOptions: function () { this.originalOptions = this.$select.clone()[0].options; }, asyncFunction: function (callback, timeout, self) { var args = Array.prototype.slice.call(arguments, 3); return setTimeout(function () { callback.apply(self || window, args); }, timeout); }, setAllSelectedText: function (allSelectedText) { this.options.allSelectedText = allSelectedText; this.updateButtonText(); }
    }; $.fn.multiselect = function (option, parameter, extraOptions) {
        return this.each(function () {
            var data = $(this).data('multiselect'); var options = typeof option === 'object' && option; if (!data) { data = new Multiselect(this, options); $(this).data('multiselect', data); }
            if (typeof option === 'string') { data[option](parameter, extraOptions); if (option === 'destroy') { $(this).data('multiselect', false); } }
        });
    }; $.fn.multiselect.Constructor = Multiselect; $(function () { $("select[data-role=multiselect]").multiselect(); });
});