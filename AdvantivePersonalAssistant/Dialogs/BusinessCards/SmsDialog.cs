using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdvantivePersonalAssistant.Queries;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace AdvantivePersonalAssistant.Dialogs.BusinessCards
{
    [Serializable]
    public class SmsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var numberFormDialog = FormDialog.FromForm(this.BuildPhoneNumberForm, FormOptions.PromptInStart);
            context.Call(numberFormDialog, this.ResumeAfterPhoneNumberFormDialog);
        }

        private IForm<PhoneNumberQuery> BuildPhoneNumberForm()
        {
            OnCompletionAsyncDelegate<PhoneNumberQuery> sendSms = async (context, state) =>
            {
                await context.PostAsync($"Ok. Done sending contact information to {state.PhoneNumber}");
            };

            return new FormBuilder<PhoneNumberQuery>()
                .Field(nameof(PhoneNumberQuery.PhoneNumber))
                .Message("Start sending to {PhoneNumber}...")
                .AddRemainingFields()
                .OnCompletion(sendSms)
                .Build();
        }

        private async Task ResumeAfterPhoneNumberFormDialog(IDialogContext context, IAwaitable<PhoneNumberQuery> result)
        {
            try
            {
                var searchQuery = await result;

                var sendOk = await this.SendSmsTask(searchQuery);

            
                await context.PostAsync("SMS Send");
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation. Quitting from the HotelsDialog";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        private async Task<bool> SendSmsTask(PhoneNumberQuery searchQuery)
        {

            return true;
        }
    }

}