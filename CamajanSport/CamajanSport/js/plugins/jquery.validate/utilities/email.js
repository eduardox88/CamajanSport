//custom validation rule
$.validator.addMethod("email",
    function (value, element) {
        return /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
    },
    "El correo electrónico es inválido"
);