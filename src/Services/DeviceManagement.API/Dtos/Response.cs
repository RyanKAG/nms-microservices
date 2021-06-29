using DeviceManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using DeviceManagement.API.Base;

namespace DeviceManagement.API.Dtos
{
    public class Response<TModel> : BaseResponse
        where TModel : class
    {
        public TModel Model { get; private set; }

        private Response(bool success, string message, TModel model) : base(success, message)
        {
            Model = model;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="model">Saved category.</param>
        /// <returns>Response.</returns>
        public Response(TModel model) : this(true, string.Empty, model)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public Response(string message) : this(false, message, null)
        { }
    }
}