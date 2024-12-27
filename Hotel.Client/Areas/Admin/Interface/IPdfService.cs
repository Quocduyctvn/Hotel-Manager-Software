namespace Hotel.Client.Areas.Admin.Interface
{
    public interface IPdfService
    {
        /// <summary>
        /// Generates a PDF from the given HTML content.
        /// </summary>
        /// <param name="htmlContent">The HTML content to convert to PDF.</param>
        /// <returns>A byte array representing the generated PDF.</returns>
        Task<byte[]> GeneratePdfAsync(string htmlContent);
    }
}
