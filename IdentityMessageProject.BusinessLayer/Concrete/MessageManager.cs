using IdentityMessageProject.BusinessLayer.Abstract;
using IdentityMessageProject.DataAccessLayer.Abstract;
using IdentityMessageProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMessageProject.BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void TDelete(int id)
        {
            _messageDal.Delete(id);
        }

        public List<Message> TGetAll()
        {
            return _messageDal.GetAll();
        }

        public Message TGetById(int id)
        {
            return _messageDal.GetById(id);
        }

        public List<Message> TGetDeleted()
        {
            return _messageDal.GetDeleted();
        }

        public List<Message> TGetDrafts()
        {
            return _messageDal.GetDrafts();
        }

        public List<Message> TGetInbox(string userEmail)
        {


            return _messageDal.GetInbox(userEmail);
        }

        public List<Message> TGetSentbox(string senderMail)
        {
            return _messageDal.GetSentbox(senderMail);
        }

        public void TInsert(Message entity)
        {
            _messageDal.Insert(entity);
        }

        public void TUpdate(Message entity)
        {
            _messageDal.Update(entity);
        }
    }
}
