using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Drawing;
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Drawing;
using System.Reflection.Metadata;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using System.Drawing.Imaging;

namespace FuzzyProject.Export
{
    public class SaveInWord
    {
        public static void InsertAPicture(string filePath, string imgPath)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();


                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                using (FileStream stream = new FileStream(imgPath, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                AddImageToBody(wordDocument, mainPart.GetIdOfPart(imagePart));
            }
        }
        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 3960000L, Cy = 3168000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 3960000L, Cy = 3168000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

           

            if (wordDoc.MainDocumentPart.Document == null)
            {
                wordDoc.MainDocumentPart.Document = new Document();
            }

            if (wordDoc.MainDocumentPart.Document.Body == null)
            {
                wordDoc.MainDocumentPart.Document.Body = new Body();
            }


            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));

        }


        public void Export(string fileName, Bitmap img)
        {
            //, string coordinates, string typeMaterial, string colorant, string results, byte[] imageData

            string dateTime = $"Отчёт за {DateTime.Now}";
            Body body = new Body();

            //создание параграфа для заголовка
            Paragraph titleParagraph = new Paragraph();
            Run run = new Run();

            Text text = new Text(dateTime);

            //стили для заголовка
            RunProperties titleProperties = new RunProperties();
            RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" };
            FontSize fontSize = new FontSize() { Val = "30" };
            Bold bold = new Bold();
            Justification justificationTitle = new Justification() { Val = JustificationValues.Center };
            titleProperties.Append(runFonts);
            titleProperties.Append(bold);
            titleProperties.Append(justificationTitle);
            titleProperties.Append(fontSize);

            run.Append(titleProperties);
            run.Append(text);
            titleParagraph.Append(run);

            body.AppendChild(titleParagraph);

            //создание временного файла для хранения изображения
            string tempFilePath = System.IO.Path.GetTempFileName() + ".jpg";
            img.Save(tempFilePath, ImageFormat.Jpeg);

            //добавление изображения в документ
            InsertAPicture(fileName, tempFilePath);

            File.Delete(tempFilePath);

            //using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            //{



            //string dateTime = $"Отчёт за {DateTime.Now}";



            //Body body = new Body();


            ////создание параграфа для заголовка
            //Paragraph titleParagraph = new Paragraph();
            //Run run = new Run();

            //Text text = new Text(dateTime);

            

            

            ////основной текст:
            //StringBuilder mainText = new StringBuilder();

            //mainText.Append($"Тип материала: {typeMaterial}. ");
            //mainText.Append($"Цветовые координаты: {coordinates}.");
            //mainText.Append($"Краситель: {colorant}.");
            //mainText.Append($"Изображение: .");
            //mainText.Append($"Результаты анализа: {results}.");

            //string[] sentences = mainText.ToString().Split(new[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);

            //// Создаем параграф для каждого предложения
            //foreach (string sentence in sentences)
            //{
            //    Paragraph paragraphMain = new Paragraph();
            //    Run runMainText = new Run(new Text(sentence.Trim() + "."));

            //    // Задаем шрифт и размер текста
            //    RunProperties runProperties = new RunProperties();
            //    RunFonts runMainFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" };
            //    FontSize fontSizeMain = new FontSize() { Val = "28" };
            //    runProperties.Append(runMainFonts);
            //    runProperties.Append(fontSizeMain);
            //    runMainText.PrependChild(runProperties);

            //    paragraphMain.Append(runMainText);
            //    body.Append(paragraphMain);
            //}

            //body.Append(paragraphImg);

            //// Добавление объекта Body в объект Document
            //documentObj.Append(body);


            //// Добавление объекта Document в объект MainDocumentPart
            //mainPart.Document = documentObj;

            //// Сохранение изменений в документе Word
            //mainPart.Document.Save();

            ////сохранение отчёта
            //wordDocument.Save();
            //wordDocument.Dispose();

            // Уведомляем пользователя о сохранении файла
            MessageBox.Show("Файл успешно сохранен");

        }
    }
}

