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
        /// Cliente no registrado
        /// </summary>
        CustomerNotFound = 1,

        /// <summary>
        /// Cliente adicionado
        /// </summary>
        CustomerAdded = 2,

        /// <summary>
        /// Cliente editado
        /// </summary>
        CustomerEdited = 3,

        /// <summary>
        /// Cliente eliminado
        /// </summary>
        CustomerDeleted = 4
    }
}
