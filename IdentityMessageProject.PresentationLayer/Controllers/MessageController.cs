using Ganss.Xss;
using IdentityMessageProject.BusinessLayer.Abstract;
using IdentityMessageProject.BusinessLayer.Concrete;
using IdentityMessageProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityMessageProject.PresentationLayer.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(IMessageService messageService, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _messageService = messageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            ViewBag.i = _messageService.TGetAll().Count();
            var user = await _userManager.GetUserAsync(User); // Kullanıcıyı al
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var messages = _messageService.TGetInbox(user.Email); // Email ile mesajları al
            return View(messages);


        }
        public async Task<IActionResult> Sentbox()
        {

            var user = await _userManager.GetUserAsync(User); // Kullanıcıyı al
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var value = _messageService.TGetSentbox(user.Email);
            ViewBag.s = value.Count();
            return View(value);
        }

        public IActionResult Details(int id)
        {
            var value = _messageService.TGetById(id);
            return View(value);
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMessage(Message message)
        {
            //Claims'e bağlı olarak kullanıcının emailini aldık.
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            message.IsDeleted = false;
            message.Date = DateTime.Now;
            //message.Sender = User.Identity.Name;
            message.SenderMail = userEmail;
            message.ParentMessageId = message.MessageId;
            _messageService.TInsert(message);
            return RedirectToAction("Inbox");
        }

        [HttpGet]
        public IActionResult Reply(int messageId)
        {
            var originalMessage = _messageService.TGetById(messageId);

            if (originalMessage == null)
            {
                return NotFound();
            }

            var replyMessage = new Message
            {
                //Sender = User.Identity.Name,
                SenderMail = User.FindFirst(ClaimTypes.Email)?.Value,
                Date = originalMessage.Date,
                Subject = originalMessage.Subject, // Orijinal mesajın başlığı
                Details = "",
                IsDeleted = false,
                ParentMessageId = originalMessage.MessageId, // Orijinal mesajı referans alıyoruz
                ReciverMail = originalMessage.SenderMail // Orijinal mesajı gönderen kişiye yanıt veriyoruz
            };

            return View(replyMessage); // Yanıt formunu göster
        }

        [HttpPost]
        public IActionResult Reply(Message message)
        {
            if (message == null || string.IsNullOrEmpty(message.Details))
            {
                ModelState.AddModelError("Error", "Please provide a message body.");
                return View(message);
            }
            if (!message.ParentMessageId.HasValue)
            {
                return BadRequest("ParentMessageId is required.");
            }

            var originalMessage = _messageService.TGetById(message.ParentMessageId.Value);


            if (originalMessage == null)
            {
                return NotFound();
            }

            message.IsDeleted = false;
            message.Date = DateTime.Now;
            //message.Sender = User.Identity.Name;
            message.SenderMail = User.FindFirst(ClaimTypes.Email)?.Value;
            message.ReciverMail = originalMessage.SenderMail; // Yanıt alıcısı, gelen mesajın göndereni olacak
            message.ParentMessageId = originalMessage.MessageId; // Yanıtın hangi mesaja ait olduğunu belirt

            _messageService.TInsert(message); // Yeni mesajı veritabanına kaydet

            return RedirectToAction("Inbox","Message", new {id=message.MessageId}); // Mesajlar kutusuna yönlendir
        }

        [HttpPost]
        public IActionResult SaveDraft(Message message)
        {
            message.SenderMail = User.FindFirst(ClaimTypes.Email).Value;
            message.IsDeleted = false;
            message.Date = DateTime.Now;
            message.IsDraft = true; 
            message.ParentMessageId = message.MessageId;
            _messageService.TInsert(message);

            return RedirectToAction("Drafts"); // Taslaklar sayfasına yönlendir
        }

        public IActionResult Drafts(Message message)
        {
            ViewBag.d = _messageService.TGetDrafts().Count();
            var value = _messageService.TGetDrafts();
            return View(value);
        }

        [HttpPost]
        public IActionResult DeleteSelectedMessages(List<int> selectedMessages)
        {
            if (selectedMessages == null || !selectedMessages.Any())
            {
                // Eğer hiçbir mesaj seçilmediyse kullanıcıya hata mesajı gösterebiliriz
                TempData["Error"] = "No messages selected to delete.";
                return RedirectToAction("Inbox");
            }

            // Seçilen mesajları silme işlemi
            foreach (var messageId in selectedMessages)
            {
                var deletedMessage = _messageService.TGetById(messageId);
                if (deletedMessage != null)
                {
                    _messageService.TDelete(deletedMessage.MessageId); // MessageId ile silme işlemi
                    deletedMessage.IsDeleted = false;    
                }
            }

            TempData["Success"] = "Selected messages deleted successfully.";
            
            return RedirectToAction("Inbox");
        }

        public IActionResult Bin()
        {
            ViewBag.b=_messageService.TGetDeleted().Count();
            var value = _messageService.TGetDeleted();
            return View(value);
        }
    }
}
