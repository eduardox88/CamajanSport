$.validator.addMethod( "decimal", function( value, element ) {
    return this.optional( element ) || /^\d+\.?\d{0,2}$/.test( value );
}, "Digite un monto correcto." );



