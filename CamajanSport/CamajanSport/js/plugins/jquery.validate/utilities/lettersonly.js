$.validator.addMethod("lettersonly", function (value, element) {
    var texto = value.replace(' ','');
    return this.optional(element) || /^[a-zA-Z\s]+$/.test(texto);
}, "Solo se permiten letras" );
