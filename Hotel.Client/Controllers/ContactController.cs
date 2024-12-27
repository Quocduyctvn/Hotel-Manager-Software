using Hotel.Client.DTOs;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Hotel.Client.Controllers
{
    public class ContactController : ControllerBase
    {
        // Biến configuration để lấy thông tin cấu hình mail
        private readonly IConfiguration _configuration;
        public ContactController(ApplicationDbContext DbContext, IConfiguration configuration) : base(DbContext)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult Send(ContactDTO contact)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Thêm thông tin liên hệ vào cơ sở dữ liệu
					AppContact appContact = new AppContact
					{
						Name = contact.Name,
						Phone = contact.Phone,
						Email = contact.Email,
						Content = contact.Content,
						CreatedDate = DateTime.Now
					};

					_HotelDbContext.AppContacts.Add(appContact);
					_HotelDbContext.SaveChanges();

					// Nội dung email
					// Nội dung email
					string content = $@"
									<html>
									<body style='font-family: Arial, sans-serif; color: #333333;'>
										<table width='100%' cellpadding='0' cellspacing='0'>
											<tr>
												<td style='padding: 20px; background-color: #f8f8f8; text-align: center;'>
													<h2 style='color: #4CAF50;'>Cảm ơn bạn đã liên hệ với HMSoft.com!</h2>
													<p style='font-size: 16px;'>Chúng tôi rất vui khi nhận được thông tin từ bạn và sẽ phản hồi sớm nhất có thể.</p>
												</td>
											</tr>
										</table>

										<table width='100%' cellpadding='10' cellspacing='0' style='padding: 20px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
											<tr>
												<td colspan='2' style='font-size: 18px; color: #333333; font-weight: bold;'>Thông tin bạn đã gửi:</td>
											</tr>
											<tr>
												<td style='width: 150px; font-weight: bold;'>Họ tên:</td>
												<td>{contact.Name}</td>
											</tr>
											<tr>
												<td style='width: 150px; font-weight: bold;'>Số điện thoại:</td>
												<td>{contact.Phone}</td>
											</tr>
											<tr>
												<td style='width: 150px; font-weight: bold;'>Email:</td>
												<td>{contact.Email}</td>
											</tr>
											<tr>
												<td style='width: 150px; font-weight: bold;'>Nội dung:</td>
												<td>{contact.Content}</td>
											</tr>
										</table>

										<table width='100%' cellpadding='20' cellspacing='0'>
											<tr>
												<td style='padding-top: 20px; text-align: center;'>
													<p style='font-size: 14px; color: #777777;'>Chúng tôi sẽ xem xét và trả lời bạn trong thời gian sớm nhất. Nếu bạn có bất kỳ câu hỏi nào khác, vui lòng liên hệ lại với chúng tôi.</p>
													<p style='font-size: 14px; color: #777777;'>Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi!</p>
												</td>
											</tr>
										</table>

										<table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f8f8f8; padding: 20px 0;'>
											<tr>
												<td style='text-align: center;'>
													<p style='font-size: 14px; color: #888888;'>Trân trọng, <br/><strong>Đội ngũ hỗ trợ HMSoft.com</strong></p>
												</td>
											</tr>
										</table>
									</body>
									</html>
									";


					// Cấu hình gửi email
					var Username = "quocduyctvn@gmail.com";
					var Password = "ylya rfag tclg nhae";
					var Host = "smtp.gmail.com";
					var Port = 587;
					var FromEmail = "quocduyctvn@gmail.com";

					MailMessage message = new MailMessage
					{
						From = new MailAddress(FromEmail),
						Subject = "Thông báo liên hệ - HMSoft.com",
						IsBodyHtml = true,
						Body = content
					};

					// Thêm email người nhận
					message.To.Add(contact.Email);

					// Tạo đối tượng SmtpClient và gửi email
					using (SmtpClient mailClient = new SmtpClient
					{
						Host = Host,
						Port = Port,
						EnableSsl = true,
						UseDefaultCredentials = false,
						Credentials = new System.Net.NetworkCredential(Username, Password)
					})
					{
						mailClient.Send(message);
					}

					// Gửi email thành công
					SetSuccessMesg("Gửi thông tin thành công");
				}
				catch (Exception ex)
				{
					// Gửi email thất bại
					Console.WriteLine($"Lỗi khi xử lý: {ex.Message}");
					SetErrorMesg("Gửi thông tin thành công nhưng gửi email thất bại.");
				}

				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("", "Có lỗi xảy ra, vui lòng kiểm tra lại thông tin");
				return View("Index");
			}
		}

	}
}
