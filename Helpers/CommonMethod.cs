using iTextSharp.text.pdf;
using iTextSharp.text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DOTP_BE.Helpers
{
    //al_29_03_2023
    public class CommonMethod
    {
        public string FilePathNameString(string input)
        {
            string fileName = string.Copy(input);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        //whit FileStream Can't change file type
        //public async void SaveImage(IFormFile file, string path)
        //{
        //    using(FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync (fileStream);
        //    }
        //}

        public bool SaveImage(IFormFile file, string path)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var byteFile = ms.ToArray();
                    System.IO.File.WriteAllBytes(path, byteFile);
                    return true;
                    //return path + "<";
                }
            }
            catch (Exception ex) { return false; }
        }

        public string GenerateT_IdandC_Id(string TorC, int value, int changeLength)
        {
            string str = string.Empty;
            int lenght = value.ToString().Length;
            while (changeLength > lenght)
            {
                str += "0";
                lenght++;
            }
            return (TorC+"_"+ DateTime.Now.Year +"_" + str + value);
        }

        public string IntToStringByL(int value, int changeLength)
        {
            string str = string.Empty;
            int lenght = value.ToString().Length;
            while(changeLength > lenght)
            {
                str += "0";
                lenght++;
            }
            return str + value;
        }

        public static async Task<bool> AddOperatorLicenseAttachPDFAsync(List<IFormFile> formFiles, string savePath)
        {
            bool oky = true;
            try
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(savePath, FileMode.Create));

                document.Open();
                foreach (var item in formFiles)
                {   
                    if (item.ContentType == "image/png" || item.ContentType == "image/jpeg" || item.ContentType == "image/gif" || item.ContentType == "image/bmp")
                    {
                        byte[] bytes;
                        using (var ms = new MemoryStream())
                        {
                            await item.CopyToAsync(ms);
                            bytes = ms.ToArray();
                        }

                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(bytes); 
                        //image.ScaleAbsolute(575f, 820.25f);
                        image.Alignment = Element.ALIGN_CENTER;
                        image.ScaleToFit(document.PageSize.Width - 10, document.PageSize.Height - 10);
                        document.Add(image);

                        //var paragraph = new Paragraph("Accept your self and just move on.");
                        //document.Add(paragraph);

                        document.NewPage();
                    }
                }
                document.Close();
            }
            catch (Exception ex)
            {
                oky = false;
            }

            return oky;
        }
    }
}
