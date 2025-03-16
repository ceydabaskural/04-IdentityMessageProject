namespace IdentityMessageProject.EntityLayer.Concrete
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string ReciverMail { get; set; }
        public string SenderMail { get; set; }
        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }

        //reply özelliği için
        public int? ParentMessageId { get; set; }

    }
}
