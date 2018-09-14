using System;
using System.Collections.Generic;

namespace ItinerariesApi.Models
{
    public partial class Invitation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int SentBy { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Administrator SentByNavigation { get; set; }
    }
}
