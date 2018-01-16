using System;
using Microsoft.Bot.Builder.FormFlow;

namespace AdvantivePersonalAssistant.Queries
{
    [Serializable]
    public class PhoneNumberQuery
    {
        [Prompt("Please enter your {&}")]
        public string PhoneNumber { get; set; }
    }
}