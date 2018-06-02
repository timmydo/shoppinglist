using Microsoft.Bot;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using backend.Interfaces.Auth;
using backend.Interfaces.Api;
using backend.Models.Requests;
using backend.Models.Documents;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using backend.Models.Responses;

namespace backend.Services.Bot
{
    public class ShoppingListBot : IBot
    {
        private const string ChatListName = "chat list";
        private readonly IUserApi userApi;

        public ShoppingListBot(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public async Task OnTurn(ITurnContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    if (context.Activity.MembersAdded[0].Id != context.Activity.Recipient.Id)
                    {
                        await ShowHelp(context);
                    }
                    break;

                case ActivityTypes.Message:
                    await HandleRequest(context, context.Activity.Text);
                    break;
            }
        }

        private async Task HandleRequest(ITurnContext context, string untrimmedRequest)
        {
            var request = untrimmedRequest.Trim().ToLowerInvariant();
            var user = GetUserId(context.Activity);
            var res = await userApi.GetAccount(user);
            if (res == null)
            {
                var createResult = await userApi.CreateAccount(user);
            }

            if (res == null || res.Lists.Count != 1)
            {
                if (request.StartsWith("use"))
                {
                    await SetupChatList(user, context, request, res?.Lists);
                    return;
                }
                else
                {
                    await ShowHelpNeedToCreateList(context);
                    return;
                }
            }

            var chatListId = res.Lists[0].Id;


            if (request.StartsWith('+') || request.StartsWith('-') || request.StartsWith('!'))
            {
                var item = request.Substring(1).Trim();
                var state = ListItemStates.Unknown;

                switch (request[0])
                {
                    case '-':
                        state = ListItemStates.Complete;
                        break;
                    case '!':
                        state = ListItemStates.Deleted;
                        break;
                    case '+':
                    default:
                        state = ListItemStates.Active;
                        break;
                }

                var result = await DoListRequest(user, chatListId, item, state);
                var resultStrings = result.Marks.Select(mr => $"Request {mr.Id}: Success: {mr.Success} (code {mr.ReasonCode})");
                await context.SendActivity(string.Join('\n', resultStrings));
            }
            else if (request.StartsWith("ls"))
            {
                await ShowListContents(user, chatListId, context);
            }
            else if (request.StartsWith("use"))
            {
                await SetupChatList(user, context, request, res?.Lists);
                return;
            }
            else
            {
                await ShowHelpListOperations(context);
            }

        }

        private async Task ShowListContents(IUser user, string chatListId, ITurnContext context)
        {
            var listRequest = new ListRequest()
            {
                ListsToGet = new List<string>() { chatListId },
            };

            var result = await userApi.ListRequest(user, listRequest);
            var requestedList = result.Lists.FirstOrDefault(ld => ld.Id == chatListId);
            if (requestedList == null)
            {
                await context.SendActivity("Cannot find list");
            }
            else
            {
                var items = requestedList.Items.Where(li => li.State == ListItemStates.Active).Select(li => li.Name);
                await context.SendActivity(string.Join(", ", items));
            }
        }

        private async Task<ListResponse> DoListRequest(IUser user, string chatListId, string item, ListItemStates state)
        {
            var listRequest = new ListRequest()
            {
                ListsToGet = new List<string>(),
                Marks = new List<MarkRequest>(),
            };

            listRequest.Marks.Add(new MarkRequest()
            {
                Item = new ListItemObject()
                {
                    Name = item,
                    State = state,
                },
                ListId = chatListId,
                RequestId = $"{item} to {state.ToString()}",
            });

            return await userApi.ListRequest(user, listRequest);
        }

        private async Task SetupChatList(IUser user, ITurnContext context, string request, IList<ListDescriptorObject> listsToRemove)
        {
            var useList = request.Substring("use".Length).Trim();
            var userRequest = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                        {
                            new ListDescriptorObject()
                            {
                                Id = useList,
                                Name = ChatListName,
                            }
                        },
                ListsToRemove = listsToRemove
            };

            await userApi.UserRequest(user, userRequest);
            await context.SendActivity($"using {useList}");
        }

        private async Task ShowHelp(ITurnContext context)
        {
            var user = GetUserId(context.Activity);
            var res = await userApi.GetAccount(user);
            if (res == null)
            {
                var createResult = await userApi.CreateAccount(user);
            }

            if (res == null || res.Lists.Count == 0)
            {
                await ShowHelpNeedToCreateList(context);
            }
            else
            {
                await ShowHelpListOperations(context);
            }
        }

        private async Task ShowHelpListOperations(ITurnContext context)
        {
            await context.SendActivity("Operations\n`+<item>`: add item to list\n`-<item>`: check complete item from list\n`!<item>`: delete item from list\n`ls`: list items\n`use <secret_list_id>` to use another list");
        }

        private async Task ShowHelpNeedToCreateList(ITurnContext context)
        {
            await context.SendActivity("Type: `use <secret_list_id>` to associated a list with your account");
        }

        private IUser GetUserId(Activity activity)
        {
            var id = $"bot_{Base64UrlEncoder.Encode(activity.ChannelId + "/" + activity.From.Id)}";
            return new User()
            {
                Id = id,
            };
        }

        private class User : IUser
        {
            public string Id { get; set; }
        }
    }
}
