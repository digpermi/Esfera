namespace Utilities.Messages
{
    /// <summary>
    /// Define los tipos de mensaje.
    /// </summary>
    public enum MessageType : int
    {
        /// <summary>
        /// No tiene tipo de mensaje.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indica que el mensaje presenta un texto de operación exitosa.
        /// </summary>
        Ok = 1,

        /// <summary>
        /// Indica que el mensaje presenta un texto con algún mensaje de validación.
        /// </summary>
        Information = 2,

        /// <summary>
        /// Indica que el mensaje presenta un texto con una pregunta.
        /// </summary>
        Question = 3,

        /// <summary>
        /// Indica que el mensaje presenta un texto de operación erronea.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Indica que el mensaje presenta un texto de con una advertencia.
        /// </summary>
        Alert = 5
    }
}
