$(document).ready(() => {


    // Sidebar Menu
    setTimeout(function () {
        $(".vertical-nav-menu").metisMenu();
    }, 1000);


    // Search wrapper trigger
    $('.search-icon').click(function () {
        $(this).parent().parent().addClass('active');
    });

    $('.search-wrapper .close').click(function () {
        $(this).parent().removeClass('active');
    });


    // Stop Bootstrap 4 Dropdown for closing on click inside
    $('.dropdown-menu').on('click', function (event) {
        var events = $._data(document, 'events') || {};
        events = events.click || [];
        for (var i = 0; i < events.length; i++) {
            if (events[i].selector) {

                if ($(event.target).is(events[i].selector)) {
                    events[i].handler.call(event.target, event);
                }

                $(event.target).parents(events[i].selector).each(function () {
                    events[i].handler.call(this, event);
                });
            }
        }
        event.stopPropagation(); //Always stop propagation
    });

    $(function () {
        $('[data-toggle="popover"]').popover();
    });


    // BS4 Tooltips
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    $('.mobile-toggle-nav').click(function () {
        $(this).toggleClass('is-active');
        $('.app-container').toggleClass('sidebar-mobile-open');
    });

    $('.mobile-toggle-header-nav').click(function () {
        $(this).toggleClass('active');
        $('.app-header__content').toggleClass('header-mobile-open');
    });

    //Sidebar right options
    $('.open-options').click(function () {
        $('.ui-theme-settings').toggleClass('settings-open');
    });


    // Scrollbar
    setTimeout(function () {

        if ($(".scrollbar-container")[0]) {

            $('.scrollbar-container').each(function () {
                const ps = new PerfectScrollbar($(this)[0], {
                    wheelSpeed: 2,
                    wheelPropagation: false,
                    minScrollbarLength: 20
                });
            });

            const ps = new PerfectScrollbar('.scrollbar-sidebar', {
                wheelSpeed: 2,
                wheelPropagation: false,
                minScrollbarLength: 20
            });

        }

    }, 1000);


    // Responsive
    var resizeClass = function () {
        var win = document.body.clientWidth;

        if (win < 1367) {
            $('.app-container').addClass('closed-sidebar-mobile closed-sidebar');
        } else {
            $('.app-container').removeClass('closed-sidebar-mobile closed-sidebar');
        }
    };

    $(window).on('resize', function () {
        resizeClass();
    });

    resizeClass();


});
