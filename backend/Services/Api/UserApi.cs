using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Models.Documents;
using backend.Models.Exceptions;
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

        public async Task<UserResponse> GetAccount(IUser userId)
        {
            var user = await database.Read<UserObject>(userId.Id);
            if (user == null)
            {
                return null;
            }

            var response = new UserResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<UserResponse> CreateAccount(IUser userId)
        {
            var user = new UserObject()
            {
                Id = userId.Id,
                Lists = new List<ListDescriptorObject>(),
            };

            await database.Create(user);
            var response = new UserResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<UserResponse> UserRequest(IUser userId, UserRequest request)
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
            var response = new UserResponse();

            if (user == null)
            {
                return response;
            }

            var toAdd = new HashSet<string>(request.ListsToAdd?.Select(li => li.Id) ?? new string[0]);
            var toRemove = new HashSet<string>(request.ListsToRemove?.Select(li => li.Id) ?? new string[0]);
            user.Lists = user.Lists.Where(li => !toAdd.Contains(li.Id) && !toRemove.Contains(li.Id)).ToList();

            if (request.ListsToAdd != null)
            {
                foreach (var item in request.ListsToAdd)
                {
                    if (!string.IsNullOrWhiteSpace(item.Id))
                    {
                        user.Lists.Add(item);
                    }
                }
            }


            if (toAdd.Any() || toRemove.Any())
            {
                await database.Write(user);
            }

            response.Lists = user.Lists;
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
            var response = new ListResponse()
            {
                Lists = new List<ListResponse.ListData>(),
                Marks = new List<ListResponse.MarkResponse>(),
            };

            if (user == null)
            {
                return response;
            }

            var listCache = new Dictionary<string, ListObject>();
            var needsSaving = new Dictionary<ListObject, List<string>>();
            if (request.Marks != null)
            {
                foreach (var mark in request.Marks)
                {
                    var listObject = await ReadList(mark.ListId, listCache);
                    if (listObject != null)
                    {
                        if (listObject.Items == null)
                        {
                            listObject.Items = new List<ListItemObject>();
                        }

                        if (string.IsNullOrWhiteSpace(mark.Item?.Name))
                        {
                            response.Marks.Add(new ListResponse.MarkResponse()
                            {
                                Id = mark.RequestId,
                                ReasonCode = MarkResponseReasonCode.MarkItemMissing,
                                Success = false,
                            });
                        }
                        else if (mark.Item.State != ListItemStates.Active
                            && mark.Item.State != ListItemStates.Complete
                            && mark.Item.State != ListItemStates.Deleted)
                        {
                            response.Marks.Add(new ListResponse.MarkResponse()
                            {
                                Id = mark.RequestId,
                                ReasonCode = MarkResponseReasonCode.InvalidState,
                                Success = false,
                            });
                        }
                        else
                        {

                            listObject.Items = listObject.Items.Where(li => !string.Equals(li.Name, mark.Item.Name, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(li.Name)).ToList();
                            if (mark.Item.State != ListItemStates.Deleted)
                            {
                                listObject.Items.Add(mark.Item);
                            }

                            if (needsSaving.ContainsKey(listObject))
                            {
                                needsSaving[listObject].Add(mark.RequestId);
                            }
                            else
                            {
                                needsSaving[listObject] = new List<string>() { mark.RequestId };
                            }
                        }
                    }
                    else
                    {
                        var newList = new ListObject()
                        {
                            Id = mark.ListId,
                            Items = new List<ListItemObject>()
                            {
                                mark.Item
                            },
                        };

                        await database.Create(newList);
                        listCache.Add(newList.Id, newList);

                        response.Marks.Add(new ListResponse.MarkResponse()
                        {
                            Id = mark.RequestId,
                            ReasonCode = MarkResponseReasonCode.None,
                            Success = true,
                        });
                    }
                }
            }

            if (request.ListsToGet != null)
            {
                foreach (var listToGet in request.ListsToGet)
                {
                    var listObject = await ReadList(listToGet, listCache);
                    response.Lists.Add(new ListResponse.ListData()
                    {
                        Id = listToGet,
                        Items = listObject?.Items ?? new List<ListItemObject>(),
                    });
                }
            }

            if (needsSaving.Any())
            {
                foreach (var toSave in needsSaving)
                {
                    try
                    {
                        await database.Write(toSave.Key);
                        foreach (var req in toSave.Value)
                        {
                            response.Marks.Add(new ListResponse.MarkResponse()
                            {
                                Id = req,
                                ReasonCode = MarkResponseReasonCode.None,
                                Success = true,
                            });
                        }
                    }
                    catch (EtagMismatchException)
                    {
                        foreach (var req in toSave.Value)
                        {
                            response.Marks.Add(new ListResponse.MarkResponse()
                            {
                                Id = req,
                                ReasonCode = MarkResponseReasonCode.WriteFailed,
                                Success = false,
                            });
                        }
                    }
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
