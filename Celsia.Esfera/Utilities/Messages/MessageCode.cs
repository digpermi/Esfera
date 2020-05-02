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
        /// Error del sistema
        /// </summary>
        GeneralError = 1,

        /// <summary>
        /// Cliente no registrado
        /// </summary>
        CustomerNotFound = 2,

        /// <summary>
        /// Persona adicionada
        /// </summary>
        PersonAdded = 3,

        /// <summary>
        /// Persona editada
        /// </summary>
        PersonEdited = 4,

        /// <summary>
        /// Persona eliminada
        /// </summary>
        PersonDeleted = 5,

        /// <summary>
        /// Persona eliminada
        /// </summary>
        PersonExist = 6,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: {1}
        /// </summary>
        InvalidPersonRow = 7,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: El código {0}, no existe como cliente.
        /// </summary>
        PersonCustomerNotValid = 8,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: El código {0}, no existe como tipo de identificación.
        /// </summary>
        IdentificationTypeNotValid = 9,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: El código {0}, no existe como relación.
        /// </summary>
        RelationshipNotValid = 10,

        /// <summary>
        /// La fila {0} tiene los siguientes errores: El código {0}, no existe como interés.
        /// </summary>
        InterestNotValid = 11,

        /// <summary>
        /// La fila {0} fue adicionada con éxito.
        /// </summary>
        PersonImported = 12
    }
}
