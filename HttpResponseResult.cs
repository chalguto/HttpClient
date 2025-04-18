// <copyright file="HttpResponseResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Core.HttpClient
{
    using System.Net.Http;

    /// <summary>
    /// Controla el mensaje de respuesta HTTP.
    /// </summary>
    public class HttpResponseResult
    {
        public bool IsSuccess { get; set; }

        public string Content { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Convierte un objeto HttpResponseMessage en un objeto HttpResponseResult.
        /// </summary>
        /// <param name="response">Objeto HttpResponseMessage que representa la respuesta HTTP a evaluar.</param>
        /// <returns>Un objeto HttpResponseResult que contiene información sobre el éxito de la respuesta, el contenido de la respuesta (si es exitosa) o un mensaje de error (si no es exitosa).</returns>
        public static HttpResponseResult FromHttpResponseMessage(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return new HttpResponseResult
                {
                    IsSuccess = true,
                    Content = response.Content.ReadAsStringAsync().Result,
                };
            }
            else
            {
                return new HttpResponseResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}",
                };
            }
        }
    }
}
