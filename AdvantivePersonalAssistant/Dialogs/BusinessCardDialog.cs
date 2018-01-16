using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AdvantivePersonalAssistant.Dialogs.BusinessCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AdvantivePersonalAssistant.Dialogs
{
    [Serializable]
    public class BusinessCardDialog : IDialog<object>
    {
        private const string Vcard = "Electronic Business Card";

        private const string OutlookContact = "Outlook Contact";

        private const string smsMessage = "Sms message";


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //var message = await result;
            this.ShowOptions(context);

        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { Vcard, OutlookContact, smsMessage }, "What's your preffered format of business card?", "Not a valid option", 3);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case Vcard:
                        context.Call(new VcardDialog(), this.ResumeAfterOptionDialog);
                        break;
                    case OutlookContact:
                        context.Call(new OutlookContactDialog(), this.ResumeAfterOptionDialog);
                        break;
                    case smsMessage:
                        await context.Forward(new SmsDialog(), this.ResumeAfterOptionDialog, context, CancellationToken.None);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                await context.PostAsync("after option");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}