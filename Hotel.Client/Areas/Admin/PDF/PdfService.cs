using DinkToPdf;
using DinkToPdf.Contracts;
using Hotel.Client.Areas.Admin.Interface;

namespace Hotel.Client.Areas.Admin.PDF
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public async Task<byte[]> GeneratePdfAsync(string htmlContent)
        {
            // Tạo đối tượng HtmlToPdfDocument với các cài đặt toàn cục
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color, // Chế độ màu
                    Orientation = Orientation.Portrait, // Hướng giấy
                    PaperSize = PaperKind.A4 // Kích thước giấy
                }
            };

            // Thêm đối tượng ObjectSettings vào danh sách Objects của document
            doc.Objects.Add(new ObjectSettings
            {
                PagesCount = true, // Đếm số trang
                HtmlContent = htmlContent, // Nội dung HTML
                WebSettings = { DefaultEncoding = "utf-8" } // Cấu hình mã hóa
            });

            // Chuyển đổi nội dung HTML thành PDF
            byte[] pdf = _converter.Convert(doc);
            return await Task.FromResult(pdf);
        }
    }
}
