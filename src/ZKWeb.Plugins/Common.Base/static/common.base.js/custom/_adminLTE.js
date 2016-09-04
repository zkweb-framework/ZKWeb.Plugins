/*! AdminLTE app.js
 * ================
 * Main JS application file for AdminLTE v2. This file
 * should be included in all pages. It controls some layout
 * options and implements exclusive AdminLTE plugins.
 *
 * @Author  Almsaeed Studio
 * @Support <http://www.almsaeedstudio.com>
 * @Email   <abdullah@almsaeedstudio.com>
 * @version 2.3.6
 * @license MIT <http://opensource.org/licenses/MIT>
 */

//Make sure jQuery has been loaded before app.js
if (typeof jQuery === "undefined") {
	throw new Error("AdminLTE requires jQuery");
}

/* AdminLTE
 *
 * @type Object
 * @description $.AdminLTE is the main object for the template's app.
 *              It's used for implementing functions and options related
 *              to the template. Keeping everything wrapped in an object
 *              prevents conflict with other plugins and is a better
 *              way to organize our code.
 */
$.AdminLTE = {};

/* --------------------
 * - AdminLTE Options -
 * --------------------
 * Modify these options to suit your implementation
 */
$.AdminLTE.options = {
	navbarMenuHeight: "200px", //The height of the inner menu
	//General animation speed for JS animated elements such as box collapse/expand and
	//sidebar treeview slide up/down. This options accepts an integer as milliseconds,
	//'fast', 'normal', or 'slow'
	animationSpeed: 500,
	//Sidebar push menu toggle button selector
	sidebarToggleSelector: "[data-toggle='offcanvas']",
	//Activate sidebar push menu
	sidebarPushMenu: true,
	//Enable sidebar expand on hover effect for sidebar mini
	//This option is forced to true if both the fixed layout and sidebar mini
	//are used together
	sidebarExpandOnHover: false,
	//Bootstrap.js tooltip
	enableBSToppltip: true,
	BSTooltipSelector: "[data-toggle='tooltip']",
	//Enable Fast Click. Fastclick.js creates a more
	//native touch experience with touch devices. If you
	//choose to enable the plugin, make sure you load the script
	//before AdminLTE's app.js
	enableFastclick: false,
	//Define the set of colors to use globally around the website
	colors: {
		lightBlue: "#3c8dbc",
		red: "#f56954",
		green: "#00a65a",
		aqua: "#00c0ef",
		yellow: "#f39c12",
		blue: "#0073b7",
		navy: "#001F3F",
		teal: "#39CCCC",
		olive: "#3D9970",
		lime: "#01FF70",
		orange: "#FF851B",
		fuchsia: "#F012BE",
		purple: "#8E24AA",
		maroon: "#D81B60",
		black: "#222222",
		gray: "#d2d6de"
	},
	//The standard screen sizes that bootstrap uses.
	//If you change these in the variables.less file, change
	//them here too.
	screenSizes: {
		xs: 480,
		sm: 768,
		md: 992,
		lg: 1200
	}
};

/* ------------------
 * - Implementation -
 * ------------------
 * The next block of code implements AdminLTE's
 * functions and plugins as specified by the
 * options above.
 */
$(function () {
	"use strict";

	//Fix for IE page transitions
	$("body").removeClass("hold-transition");

	//Extend options if external options exist
	if (typeof AdminLTEOptions !== "undefined") {
		$.extend(true,
			$.AdminLTE.options,
			AdminLTEOptions);
	}

	//Easy access to options
	var o = $.AdminLTE.options;

	//Set up the object
	$.AdminLTE.init();

	//Fix sidebar height
	//Delay 1s to make this work faster
	var fixSidebarTimeout = null;
	var fixSidebarHeight = function () {
		clearTimeout(fixSidebarTimeout);
		fixSidebarTimeout = setTimeout(function () {
			$(".main-sidebar").height($(".page-body").outerHeight() + $(".page-footer").outerHeight());
		}, 1);
	}
	fixSidebarHeight();
	$(window).resize(fixSidebarHeight);

	//Enable sidebar tree view controls
	$.AdminLTE.tree('.sidebar');

	//Activate sidebar push menu
	if (o.sidebarPushMenu) {
		$.AdminLTE.pushMenu.activate(o.sidebarToggleSelector);
	}

	//Activate Bootstrap tooltip
	if (o.enableBSToppltip) {
		$('body').tooltip({
			selector: o.BSTooltipSelector
		});
	}

	//Activate fast click
	if (o.enableFastclick && typeof FastClick != 'undefined') {
		FastClick.attach(document.body);
	}

	/*
     * INITIALIZE BUTTON TOGGLE
     * ------------------------
     */
	$('.btn-group[data-toggle="btn-toggle"]').each(function () {
		var group = $(this);
		$(this).find(".btn").on('click', function (e) {
			group.find(".btn.active").removeClass("active");
			$(this).addClass("active");
			e.preventDefault();
		});
	});
});

/* ----------------------------------
 * - Initialize the AdminLTE Object -
 * ----------------------------------
 * All AdminLTE functions are implemented below.
 */
$.AdminLTE.init = function () {
	'use strict';
	/* PushMenu()
     * ==========
     * Adds the push menu functionality to the sidebar.
     *
     * @type Function
     * @usage: $.AdminLTE.pushMenu("[data-toggle='offcanvas']")
     */
	$.AdminLTE.pushMenu = {
		activate: function (toggleBtn) {
			//Get the screen sizes
			var screenSizes = $.AdminLTE.options.screenSizes;

			//Enable sidebar toggle
			$(document).on('click', toggleBtn, function (e) {
				e.preventDefault();

				//Enable sidebar push menu
				if ($(window).width() > (screenSizes.sm - 1)) {
					if ($("body").hasClass('sidebar-collapse')) {
						$("body").removeClass('sidebar-collapse').trigger('expanded.pushMenu');
					} else {
						$("body").addClass('sidebar-collapse').trigger('collapsed.pushMenu');
					}
				}
					//Handle sidebar push menu for small screens
				else {
					if ($("body").hasClass('sidebar-open')) {
						$("body").removeClass('sidebar-open').removeClass('sidebar-collapse').trigger('collapsed.pushMenu');
					} else {
						$("body").addClass('sidebar-open').trigger('expanded.pushMenu');
					}
				}
			});

			$(".content-wrapper").click(function () {
				//Enable hide menu when clicking on the content-wrapper on small screens
				if ($(window).width() <= (screenSizes.sm - 1) && $("body").hasClass("sidebar-open")) {
					$("body").removeClass('sidebar-open');
				}
			});

			//Enable expand on hover for sidebar mini
			if ($.AdminLTE.options.sidebarExpandOnHover
				|| ($('body').hasClass('fixed')
				&& $('body').hasClass('sidebar-mini'))) {
				this.expandOnHover();
			}
		},
		expandOnHover: function () {
			var _this = this;
			var screenWidth = $.AdminLTE.options.screenSizes.sm - 1;
			//Expand sidebar on hover
			$('.main-sidebar').hover(function () {
				if ($('body').hasClass('sidebar-mini')
					&& $("body").hasClass('sidebar-collapse')
					&& $(window).width() > screenWidth) {
					_this.expand();
				}
			}, function () {
				if ($('body').hasClass('sidebar-mini')
					&& $('body').hasClass('sidebar-expanded-on-hover')
					&& $(window).width() > screenWidth) {
					_this.collapse();
				}
			});
		},
		expand: function () {
			$("body").removeClass('sidebar-collapse').addClass('sidebar-expanded-on-hover');
		},
		collapse: function () {
			if ($('body').hasClass('sidebar-expanded-on-hover')) {
				$('body').removeClass('sidebar-expanded-on-hover').addClass('sidebar-collapse');
			}
		}
	};

	/* Tree()
     * ======
     * Converts the sidebar into a multilevel
     * tree view menu.
     *
     * @type Function
     * @Usage: $.AdminLTE.tree('.sidebar')
     */
	$.AdminLTE.tree = function (menu) {
		var _this = this;
		var animationSpeed = $.AdminLTE.options.animationSpeed;
		$(document).off('click', menu + ' li a').on('click', menu + ' li a', function (e) {
			//Get the clicked link and the next element
			var $this = $(this);
			var checkElement = $this.next();

			//Check if the next element is a menu and is visible
			if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible')) && (!$('body').hasClass('sidebar-collapse'))) {
				//Close the menu
				checkElement.slideUp(animationSpeed, function () {
					checkElement.removeClass('menu-open');
				});
				checkElement.parent("li").removeClass("active");
			}
				//If the menu is not visible
			else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
				//Get the parent menu
				var parent = $this.parents('ul').first();
				//Close all open menus within the parent
				var ul = parent.find('ul:visible').slideUp(animationSpeed);
				//Remove the menu-open class from the parent
				ul.removeClass('menu-open');
				//Get the parent li
				var parent_li = $this.parent("li");

				//Open the target menu and add the menu-open class
				checkElement.slideDown(animationSpeed, function () {
					//Add the class active to the parent li
					checkElement.addClass('menu-open');
					parent.find('li.active').removeClass('active');
					parent_li.addClass('active');
				});
			}
			//if this isn't a link, prevent the page from being redirected
			if (checkElement.is('.treeview-menu')) {
				e.preventDefault();
			}
		});
	};
};
