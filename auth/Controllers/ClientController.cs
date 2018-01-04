﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using STS.Attributes;
using STS.Inputs;
using STS.Interface;
using STS.Models;

namespace STS.Controllers
{
    [Route("client")]
    public class ClientController : Controller
    {
        private readonly IRepository _db;

        public ClientController(IRepository db)
        {
            _db = db;
        }

        [Authorize]
        [RequiresPermissions("sts:get-all:client")]
        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            return Task.Run<IActionResult>(() =>
            {
                var clients = _db.All<Client>();

                var clientsOutput = new List<ClientModel>();

                foreach (var client in clients)
                {
                    
                    client.Properties.TryGetValue("permissions", out var permissions);
                    var clientModel = new ClientModel
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName,
                        AllowedGrantTypes = client.AllowedGrantTypes.ToArray(),
                        AllowedScopes = client.AllowedScopes.ToArray(),
                        AllowOfflineAccess = client.AllowOfflineAccess,
                        Permissions = JsonConvert.DeserializeObject<List<string>>(permissions ?? "[]").ToArray()
                    };
                    clientsOutput.Add(clientModel);
                }

                return Ok(new
                {
                    success = true,
                    clients = clientsOutput
                });
            });
        }

        [Authorize]
        [RequiresPermissions("sts:get-all:client")]
        [HttpGet("count")]
        public Task<IActionResult> Count()
        {
            return Task.Run<IActionResult>(() =>
            {
                var count = _db.All<Client>()
                    .ToList()
                    .Count;
                return Ok(new
                {
                    success = true,
                    count
                });
            });
        }

        [Authorize]
        [RequiresPermissions("sts:create:client")]
        [HttpPost]
        public Task<IActionResult> Create(ClientModel input)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var clientIdExists = _db.Where<Client>(c => c.ClientId == input.ClientId).Any();

                if (clientIdExists)
                {
                    return new StatusCodeResult((int) HttpStatusCode.Conflict);
                }

                var client = new Client()
                {
                    ClientId = input.ClientId,
                    ClientName = input.ClientName,
                    ClientSecrets = {new Secret(input.ClientSecret.Sha256())},
                    AllowedGrantTypes = input.AllowedGrantTypes,
                    AllowedScopes = input.AllowedScopes,
                    AllowOfflineAccess = input.AllowOfflineAccess,
                    Properties =
                    {
                        {"permissions", JsonConvert.SerializeObject(input.Permissions)}
                    }
                };

                _db.Add(client);

                return Ok(new {success = true});
            });
        }

        [Authorize]
        [RequiresPermissions("sts:update:client")]
        [HttpPut]
        public Task<IActionResult> Update(ClientModel input)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var client = _db.Single<Client>(c => c.ClientId == input.ClientId);

                if (client == null)
                {
                    return new StatusCodeResult((int) HttpStatusCode.NotFound);
                }

                client.ClientId = input.ClientId;
                client.ClientName = input.ClientName;
                client.ClientSecrets = new List<Secret>()
                {
                    new Secret(input.ClientSecret.Sha256())
                };

                client.AllowedGrantTypes = input.AllowedGrantTypes;
                client.AllowedScopes = input.AllowedScopes;
                client.AllowOfflineAccess = input.AllowOfflineAccess;
                client.Properties = new Dictionary<string, string>
                {
                    {
                        "permissions", JsonConvert.SerializeObject(input.Permissions)
                    }
                };

                _db.Replace(c => c.ClientId == input.ClientId, client);

                return Ok(new {success = true});
            });
        }

        [Authorize]
        [RequiresPermissions("sts:delete:client")]
        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(string id)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _db.Delete<Client>(c => c.ClientId == id);

                return Ok(new {success = true});
            });
        }
    }
}