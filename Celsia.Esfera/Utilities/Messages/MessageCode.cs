namespace Utilities.Messages
{
    /// <summary>
    /// Define los códigos de los mensajes del sistema.
    /// </summary>
    public enum MessageCode : int
    {
        /// <summary>
        /// No hay un mensaje definido.
        /// </summary>
        None = 0,

        /// <summary>
        /// Prueba mensajes.
        /// </summary>
        Test = 1,

        CustomerqueryOk = 2,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: {1}
        /// </summary>
        InvalidPersonRow = 3,

        /// <summary>
        /// El código {0}, no existe como cliente.
        /// </summary>
        PersonCustomerNotValid = 4,
    }
}
