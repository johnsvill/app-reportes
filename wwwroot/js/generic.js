function mostrarModal(titulo = "Deseas guardar los cambios?",
    texto = "Deseas registrar los datos?") {
    return Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'SI'
    })
}

function ExportarExcel() {
    document.getElementById("tipoReporte").value = "Excel";
    var frmReporte = document.getElementById("frmReporte");
    frmReporte.submit();
}

function ExportarWord() {
    document.getElementById("tipoReporte").value = "Word";
    var frmReporte = document.getElementById("frmReporte");
    frmReporte.submit();
}

function ExportarPdf(nombreController) {
    //document.getElementById("tipoReporte").value = "PDF";
    //var frmReporte = document.getElementById("frmReporte");
    //frmReporte.submit();
    var frm = new FormData();
    var checks = document.getElementsByName("nombreProp");
    var nchecks = checks.length;
    for (var i = 0; i < nchecks; i++) {
        if (checks[i].checked == true) {
            frm.append("nombreProp[]", checks[i].value);
        }
    }

    $.ajax({
        type: "POST",
        url: nombreController + "/ExportarDatosPDF",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {
            var a = document.createElement("a");
            a.href = data;
            a.download = "Reporte.pdf";
            a.click();
        }
    })
}

function Imprimir() {
    var tcheck = document.getElementById("tcheck").outerHTML;
    var table = "<h1>Reporte a imprimir</h1>"
    table += document.getElementById("table").outerHTML;
    table = table.replace(tcheck, " ");
    var pagina = window.document.body;
    var ventana = window.open();
    ventana.document.write(table);
    ventana.print();
    ventana.close();
    window.document.body = pagina;
}