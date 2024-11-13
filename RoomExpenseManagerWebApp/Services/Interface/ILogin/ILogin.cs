

using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace RoomExpenseManagerWebApp.Services.Interface.ILogin
{
    public interface ILogin
    {
        Task<JsonObject> GetUserByCred(Models.Login login);
    }
}
