﻿using IdentityMessageProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMessageProject.DataAccessLayer.Abstract
{
    public interface IMessageDal : IGenericDal<Message>
    {
        List<Message> GetInbox(string userEmail);
        List<Message> GetSentbox(string senderMail);
        List<Message> GetDrafts();
        List<Message> GetDeleted();
    }
}
