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