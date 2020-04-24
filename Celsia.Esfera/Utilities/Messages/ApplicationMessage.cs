namespace Utilities.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.Extensions.PlatformAbstractions;
    using Utilities.Cache;

    /// <summary>
    /// Define la información para los mensajes.
    /// </summary>
    public class ApplicationMessage
    {
        /// <summary>
        /// Inicializa la clase mensaje.
        /// </summary>
        public ApplicationMessage()
        {
        }

        /// <summary>
        /// Inicializa la clase mensaje con base en el código de mensaje enviado.
        /// </summary>
        /// <param name="cache">Injección de dependencias para usar la cache.</param>
        /// <param name="codigoMensaje">Código del mensaje que se desea obtener.</param>
        public ApplicationMessage(ICacheUtility cache, MessageCode codigoMensaje)
        {
            try
            {
                List<ApplicationMessage> mensajes = cache.GetCacheValue(MessagesResources.ClaveChacheMensajes, GetMessages);

                ApplicationMessage mensajeEncontrado = mensajes.First(mensaje => mensaje.Code == codigoMensaje);

                this.Code = mensajeEncontrado.Code;
                this.Text = mensajeEncontrado.Text;
                this.Type = mensajeEncontrado.Type;
            }
            catch (Exception excepcion)
            {
                this.Code = 0;
                this.Text = string.Format(CultureInfo.InvariantCulture, MessagesResources.ErrorObteniendoMensajes, excepcion.Message);
                this.Type = MessageType.Error;
            }
        }

        /// <summary>
        /// Obtiene los mesajes desde el archivo Json para el sistema.
        /// </summary>
        /// <returns>Lista de mensajes de la aplicación</returns>
        private static List<ApplicationMessage> GetMessages()
        {
            string rutaMensajesJson = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", PlatformServices.Default.Application.ApplicationBasePath, MessagesResources.RutaArchivoMensajes);
            string datosJson = File.ReadAllText(rutaMensajesJson);

            return JsonSerializer.Deserialize<List<ApplicationMessage>>(datosJson);
        }

        /// <summary>
        /// Obtiene o establece el código del mensaje.
        /// </summary>
        public MessageCode Code { get; set; }

        /// <summary>
        /// Obtiene o establece el texto que informa el mensaje.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de mensaje que identifica su rol en la aplicación.
        /// </summary>
        public MessageType Type { get; set; }
    }
}
