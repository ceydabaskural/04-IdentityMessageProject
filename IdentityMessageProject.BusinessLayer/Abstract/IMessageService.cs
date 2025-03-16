using IdentityMessageProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMessageProject.BusinessLayer.Abstract
{
    public interface IMessageService : IGenericService<Message> 
    {
        List<Message> TGetInbox(string userEmail);
        List<Message> TGetSentbox(string senderMail);
        List<Message> TGetDrafts();
        List<Message> TGetDeleted();
    }
}
