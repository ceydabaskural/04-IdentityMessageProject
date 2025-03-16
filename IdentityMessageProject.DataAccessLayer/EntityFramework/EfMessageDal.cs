using IdentityMessageProject.DataAccessLayer.Abstract;
using IdentityMessageProject.DataAccessLayer.Context;
using IdentityMessageProject.DataAccessLayer.Repositories;
using IdentityMessageProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMessageProject.DataAccessLayer.EntityFramework
{
    public class EfMessageDal : GenericRepository<Message>, IMessageDal
    {
        public EfMessageDal(IdentityMessageContext context) : base(context)
        {
        }

        public List<Message> GetDeleted()
        {
            var context = new IdentityMessageContext();
            var values = context.Messages.Where(x => x.IsDeleted == true).ToList();
            return values;
        }

        public List<Message> GetDrafts()
        {
            var context = new IdentityMessageContext();
            var values = context.Messages.Where(x => x.IsDraft == true).ToList();
            return values;
        }

        public List<Message> GetInbox(string userEmail)
        {
            var context = new IdentityMessageContext();
            var values = context.Messages.Where(x => x.ReciverMail == userEmail).OrderByDescending(m => m.Date) // Yeni mesajlar üstte olacak
            .ToList();

            return values;
        }

        public List<Message> GetSentbox(string senderMail)
        {
            var context = new IdentityMessageContext();
            var values = context.Messages.Where(x => x.SenderMail == senderMail).ToList();
            return values;
        }
    }
}
