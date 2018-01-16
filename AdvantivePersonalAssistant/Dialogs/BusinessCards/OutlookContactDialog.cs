using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AdvantivePersonalAssistant.Dialogs.BusinessCards
{
    [Serializable]
    public class OutlookContactDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

           

            await context.PostAsync($"here is your outlook contact.");

            context.Done<object>(null);
        }
    }
}