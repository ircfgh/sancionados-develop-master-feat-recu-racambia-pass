$(document).ready(() => {

    var resizeCardBody = function (extraSize) {

        var bodyWidth = document.body.clientWidth;
        var sizeHeadCard = 37;

        bodyWidth = bodyWidth + extraSize;

        $('.card-body-size').width(bodyWidth - sizeHeadCard - (265 - (bodyWidth <= 974 ? (bodyWidth <= 750 ? 110 : 80) : 0)));

    };


    $(window).on('resize', function () {
        resizeCardBody((navigator.userAgent.indexOf("Firefox") != -1) ? -30 : 0);
    });

    resizeCardBody((navigator.userAgent.indexOf("Firefox") != -1) ? -30 : 0);


    /**************************************
     * Acordeon
     * script para inicializar el acordeon
     * ************************************/
    const horizontalAccordions = $(".accordion.width");

    horizontalAccordions.each((index, element) => {
        const accordion = $(element);
        const collapse = accordion.find(".collapse");
        //const cards = accordion.find(".card");
        //const bodies = collapse.find("> *");

        //bodies.width(bodies.eq(0).width());

        collapse.not(".show").each((index, element) => {
            $(element).parent().find("[data-toggle='collapse']").addClass("collapsed");
        });
    });


    $(".accordion-toggle").attr("data-toggle", "");


    $(".view-detail").on("click", function () {
        $(".accordion-toggle").attr("data-toggle", "collapse");
        $(".accordion-toggle").removeClass("disabled-card");
    });

});
