using System;
using System.IO;
using MAS_Project.Models.Base;
using MAS_Project.Models.Persistence;

namespace MAS_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MAS Project Test Başlangıcı ===");

            // 🔹 Sabit tarih tanımları
            var eventStart = new DateTime(2025, 6, 20, 20, 0, 0);
            var eventEnd = new DateTime(2025, 6, 21, 5, 0, 0);
            var enrollmentDate = new DateTime(2025, 6, 15, 10, 30, 0);
            var promotionRequestDate = new DateTime(2025, 6, 17, 9, 0, 0);

            // 📁 Kayıt yolu
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "data");
            Directory.CreateDirectory(basePath);

            // 1. Lokasyon ve Mekan
            var location = new Location("Poland", "Warsaw", "Main St", "00-001");
            var venue = new Venue("Hala Expo") { Location = location };

            // 2. Organizatör ve Kullanıcılar
            var organizer = new Organizer("Kemal", "kemal@mail.com", new DateTime(1990, 5, 20), "pass123", "Tech Events", true, 0);
            var regularUser = new RegularUser("Ali", "ali@mail.com", new DateTime(1995, 6, 10), "secret", "Warsaw");

            // 3. Ödeme detayı
            var organizerPayment = new PaymentDetail("Kemal Account", "PL60102010260000042270201111");
            organizer.SetPaymentDetail(organizerPayment);

            // 4. Kategoriler
            var musicCategory = new EventCategory("Music", "All music events");
            var raveCategory = new EventCategory("Rave", "Underground techno");

            // 5. Etkinlik
            var event1 = new Event(
                "Rave Night",
                "Join the underground techno rave!",
                eventStart,
                eventEnd,
                100,
                organizer,
                new[] { musicCategory, raveCategory },
                new[] { "techno", "nightlife" }
            )
            {
                Venue = venue
            };

            // 6. Enroll
            var enrollment = new Enrollment(regularUser, event1, enrollmentDate);

            // 7. Comment
            var comment = new DiscussionComment(regularUser, event1, "This looks amazing!");

            // 8. PromotedRequest
            var promo = new PromotedRequest(event1, organizer, promotionRequestDate, 15.0);
            event1.SetPromotedRequest(promo);
            promo.Confirm();

            // 9. Message gönderme
            var message = organizer.SendMessage(regularUser, "Welcome to our event!");
            regularUser.SendMessage(organizer, "Thanks!");

            // ✅ Kaydet
            EventCategoryStorage.SaveExtent(Path.Combine(basePath, "eventcategories.bin"));
            LocationStorage.SaveExtent(Path.Combine(basePath, "locations.bin"));
            VenueStorage.SaveExtent(Path.Combine(basePath, "venues.bin"));
            UserStorage.SaveExtent(Path.Combine(basePath, "users.bin"));
            RegularUserStorage.SaveExtent(Path.Combine(basePath, "regularusers.bin"));
            OrganizerStorage.SaveExtent(Path.Combine(basePath, "organizers.bin"));
            EventStorage.SaveExtent(Path.Combine(basePath, "events.bin"));
            EnrollmentStorage.SaveExtent(Path.Combine(basePath, "enrollments.bin"));
            DiscussionCommentStorage.SaveExtent(Path.Combine(basePath, "comments.bin"));
            PromotedRequestStorage.SaveExtent(Path.Combine(basePath, "promotions.bin"));
            MessageStorage.SaveExtent(Path.Combine(basePath, "messages.bin"));
            PaymentDetailStorage.SaveExtent(Path.Combine(basePath, "paymentdetails.bin"));
            TransactionStorage.SaveExtent(Path.Combine(basePath, "transactions.bin"));

            Console.WriteLine("\n Veriler başarıyla kaydedildi!");

            // 🧪 Örnek çıktı
            Console.WriteLine($"\nCreated Event: {event1.EventTitle} @ {event1.Venue.VenueName}");
            Console.WriteLine($"Enrolled Users: {event1.Enrollments.Count}");
            Console.WriteLine($"Comments: {event1.Comments.Count}");
            Console.WriteLine($"Promoted Status: {event1.PromotedRequest?.Status}");
            Console.WriteLine($"User Inbox: {regularUser.MessagesReceived.Count} messages");

            Console.WriteLine("=== MAS Project Test Tamamlandı ===");
            
            Console.WriteLine("\n=== VERİ YÜKLEME TESTİ ===");

            var loadedEvents = EventStorage.LoadExtent(Path.Combine(basePath, "events.bin"));
            var loadedUsers = UserStorage.LoadExtent(Path.Combine(basePath, "users.bin"));
            var loadedMessages = MessageStorage.LoadExtent(Path.Combine(basePath, "messages.bin"));

            Console.WriteLine($"Yüklenen Event sayısı: {loadedEvents.Count}");
            Console.WriteLine($"Yüklenen User sayısı: {loadedUsers.Count}");
            Console.WriteLine($"Yüklenen Message sayısı: {loadedMessages.Count}");

        }
    }
}
