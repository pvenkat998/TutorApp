using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TutorApp2.Models
{
    public interface ITwilioMessenger
    {
        Task<bool> InitializeAsync();

        void SendMessage(string text);

        Action<Message> MessageAdded { get; set; }
    }
}