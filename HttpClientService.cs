// <copyright file="HttpClientService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Core.HttpClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

	 public class RequestConfiguration
	 {
		 public Uri RequestUri { get; set; }
		 public Dictionary<string, string> Headers { get; set; }
		 public string Username { get; set; }
		 public string Password { get; set; }

		 public RequestConfiguration()
		 {
			 Headers = new Dictionary<string, string>();
		 }
	 }
	 
    /// <summary>
    /// Controla las solicitudes HTTP.
    /// </summary>
    public class HttpClientService
    {
        private readonly RequestConfiguration requestConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientService"/> class.
        /// </summary>
        /// <param name="requestConfiguration">Objeto RequestConfiguration que contiene la configuración de la solicitud.</param>
        public HttpClientService(RequestConfiguration requestConfiguration)
        {
            this.requestConfiguration = requestConfiguration;
        }

        /// <summary>
        /// Envía una solicitud HTTP y devuelve el resultado de la solicitud.
        /// </summary>
        /// <param name="method">Método HTTP a utilizar (por ejemplo, GET, POST, PUT, DELETE).</param>
        /// <param name="payload">Cadena que representa la carga útil de la solicitud.</param>
        /// <returns>Una tarea que representa una operación asincrónica. El resultado de la tarea es un objeto HttpResponseResult que contiene información sobre el éxito de la respuesta, el contenido de la respuesta (si es exitosa) o un mensaje de error (si no es exitosa).</returns>
        public async Task<HttpResponseResult> SendHttpRequestAsync(HttpMethod method, string payload)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            using (HttpClient client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage
                {
                    RequestUri = this.requestConfiguration.RequestUri,
                    Method = method,
                };

                foreach (var header in this.requestConfiguration.Headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }

                var byteArray = Encoding.ASCII.GetBytes($"{this.requestConfiguration.Username}:{this.requestConfiguration.Password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                if (!string.IsNullOrEmpty(payload))
                {
                    requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage httpResponse = await client.SendAsync(requestMessage);
                var responseResult = HttpResponseResult.FromHttpResponseMessage(httpResponse);

                if (!responseResult.IsSuccess)
                {
                    throw new HttpRequestException(responseResult.ErrorMessage);
                }

                return responseResult;
            }
        }
    }
}