/*!
    * Start Bootstrap - SB Admin v6.0.0 (https://startbootstrap.com/templates/sb-admin)
    * Copyright 2013-2020 Start Bootstrap
    * Licensed under MIT (https://github.com/BlackrockDigital/startbootstrap-sb-admin/blob/master/LICENSE)
*/
(function($) {
    "use strict";

    // Add active state to sidbar nav links
    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
        $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function() {
            if (this.href === path) {
                $(this).addClass("active");
            }
        });

    // Toggle the side navigation
    $("#sidebarToggle").on("click", function(e) {
        e.preventDefault();
        $("body").toggleClass("sb-sidenav-toggled");
    });
})(jQuery);

// Call the dataTables jQuery plugin
$(document).ready(function () {
    if ($('#dataTable').length >= 1) {
        $('#dataTable').DataTable({
            columnDefs: [
                { orderable: false, targets: -1 }
            ],
            "bLengthChange": false,
            "searching": false,
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Spanish.json"
            }
        });
    }
    if ($('#dataTableSearch').length >= 1) {
        $('#dataTableSearch').DataTable({
            columnDefs: [
                { orderable: false, targets: -1 }
            ],
            "bLengthChange": false,
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Spanish.json"
            }
        });
    }
  
});


