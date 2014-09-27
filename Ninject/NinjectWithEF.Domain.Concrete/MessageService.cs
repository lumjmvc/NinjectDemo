using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Abstract;

namespace NinjectWithEF.Domain.Concrete
{
    public class MessageService : IMessageService
    {
        public string Message {
            get { return "Message Service Called"; }
        }

        public string ImageUrl { get; set; }
    }
}
