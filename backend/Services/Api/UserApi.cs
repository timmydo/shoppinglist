using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Models.Documents;
using backend.Models.Requests;
using backend.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Api
{
    public class UserApi : IUserApi
    {
        private readonly IDatabase database;

        public UserApi(IDatabase database)
        {
            this.database = database;
        }

        public async Task<GetMyAccountResponse> GetAccount(IUser userId)
        {
            var user = await database.Read<UserObject>(userId.Id);
            if (user == null)
            {
                return null;
            }

            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<GetMyAccountResponse> CreateAccount(IUser userId)
        {
            var user = new UserObject()
            {
                Id = userId.Id,
                Lists = new List<ListDescriptorObject>(),
            };

            await database.Create(user);
            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<ListResponse> ListRequest(IUser userId, ListRequest request)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await database.Read<UserObject>(userId.Id);
            bool writeUser = false;
            var response = new ListResponse()
            {
                Lists = new List<ListResponse.ListData>(),
                Marks = new List<ListResponse.MarkResponse>(),
            };

            if (user == null)
            {
                return response;
            }

            if (request.ListsToAdd != null)
            {
                foreach (var toAdd in request.ListsToAdd)
                {
                    user.Lists = user.Lists.Where(li => li.Id != toAdd.Id).ToList();
                    user.Lists.Add(toAdd);
                    writeUser = true;
                }
            }

            var listCache = new Dictionary<string, ListObject>();
            var needsSaving = new HashSet<ListObject>();
            if (request.Marks != null)
            {
                foreach (var mark in request.Marks)
                {
                    var listObject = await ReadList(mark.ListId, listCache);
                    if (listObject != null)
                    {
                        //to do fixme
                        //if (mark.Item
                        needsSaving.Add(listObject);
                    }
                    else
                    {
                        response.Marks.Add(new ListResponse.MarkResponse()
                        {
                            Id = mark.RequestId,
                            ReasonCode = MarkResponseReasonCode.ListNotFound,
                            Success = false,
                        });
                    }
                }
            }

            if (request.ListsToGet != null)
            {
                foreach (var listToGet in request.ListsToGet)
                {
                    var listObject = await database.Read<ListObject>(listToGet);
                    if (listObject != null)
                    {
                        listCache.Add(listToGet, listObject);
                    }
                }
            }


            if (writeUser)
            {
                await database.Write(user);
            }

            if (needsSaving.Any())
            {
                foreach (var toSave in needsSaving)
                {
                    await database.Write(toSave);
                }
            }

            return response;
        }

        private async Task<ListObject> ReadList(string listId, IDictionary<string, ListObject> cache)
        {
            if (cache.TryGetValue(listId, out var list))
            {
                return list;
            }

            var listObject = await database.Read<ListObject>(listId);
            if (listObject != null)
            {
                cache.Add(listId, listObject);
            }

            return listObject;
        }
    }
}
