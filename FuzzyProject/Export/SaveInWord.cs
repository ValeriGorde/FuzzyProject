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
using FuzzyProject.Models;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Underline = DocumentFormat.OpenXml.Wordprocessing.Underline;

namespace FuzzyProject.Export
{
    public class SaveInWord
    {
        public static void InsertAPictureWithText(string filePath, string[] fileNames, string coordinates, string material, string colorant, string results)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();


                AddText(wordDocument, coordinates, material, colorant);

                string[] captions = new string[] { "Исходное изображение:", "Изображение после усреднения:" };

                for (int i = 0; i < fileNames.Length; i++)
                {
                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                    using (FileStream stream = new FileStream(fileNames[i], FileMode.Open))
                    {
                        imagePart.FeedData(stream);
                    }

                    CreateImg(wordDocument, mainPart.GetIdOfPart(imagePart), captions[i]);
                }


                AddResults(wordDocument, results);
            }
        }

        private static void CreateImg(WordprocessingDocument wordDoc, string relationshipId, string caption)
        {
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

            Paragraph captionParagraph = new Paragraph(
                new Run(
                new RunProperties(
                    new Italic(),
                    new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                    new FontSize() { Val = "28" }),
                new Text(caption)));

            wordDoc.MainDocumentPart.Document.Body.AppendChild(captionParagraph);
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        private static void AddResults(WordprocessingDocument wordDoc, string results)
        {
            // создаём параграф для строки результата
            Paragraph paragraphResult = new Paragraph();
            Run runResultText = new Run(new Text($"Результаты: {results}"));

            RunProperties resProperties = new RunProperties();
            RunFonts runFontsResult = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" };
            FontSize fontSizeResult = new FontSize() { Val = "28" };
            resProperties.Append(runFontsResult);
            resProperties.Append(fontSizeResult);
            runResultText.PrependChild(resProperties);
            paragraphResult.Append(runResultText);


            if (wordDoc.MainDocumentPart.Document == null)
            {
                wordDoc.MainDocumentPart.Document = new Document();
            }

            if (wordDoc.MainDocumentPart.Document.Body == null)
            {
                wordDoc.MainDocumentPart.Document.Body = new Body();
            }
            // добавляем результат
            wordDoc.MainDocumentPart.Document.Body.AppendChild(paragraphResult);

            // закрываем документ
            wordDoc.Save();
            wordDoc.Dispose();
        }

        private static void AddText(WordprocessingDocument wordDoc, string coordinates, string material, string colorant)
        {

            string dateTime = $"Отчёт за {DateTime.Now}";

            //создание параграфа для заголовка
            Paragraph titleParagraph = new Paragraph(
                new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center }),
                new Run(
                new RunProperties(
                    new Bold(),
                    new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                    new FontSize() { Val = "30" }),
                new Text(dateTime)));

            //создание параграфа для доп текста
            Paragraph addingText = new Paragraph(
                new Run(
                new RunProperties(
                    new Underline() { Val = UnderlineValues.Single },
                    new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                    new FontSize() { Val = "28" }),
                new Text("Результаты анализа цветовых характеристик экструдата:")));

            if (wordDoc.MainDocumentPart.Document == null)
            {
                wordDoc.MainDocumentPart.Document = new Document();
            }

            if (wordDoc.MainDocumentPart.Document.Body == null)
            {
                wordDoc.MainDocumentPart.Document.Body = new Body();
            }

            // добавляем заголовок
            wordDoc.MainDocumentPart.Document.Body.AppendChild(titleParagraph);
            wordDoc.MainDocumentPart.Document.Body.AppendChild(addingText);

            // основной текст:
            StringBuilder mainText = new StringBuilder();

            mainText.Append($"Тип материала: {material}.");
            mainText.Append($"Цветовые координаты: {coordinates}.");
            mainText.Append($"Краситель: {colorant}.");

            string[] sentences = mainText.ToString().Split(new[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);

            // создаем параграф для каждого предложения
            foreach (string sentence in sentences)
            {
                Paragraph paragraphMain = new Paragraph();
                Run runMainText = new Run(new Text(sentence.Trim() + "."));

                // Задаем шрифт и размер текста
                RunProperties runProperties = new RunProperties();
                RunFonts runMainFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" };
                FontSize fontSizeMain = new FontSize() { Val = "28" };
                runProperties.Append(runMainFonts);
                runProperties.Append(fontSizeMain);
                runMainText.PrependChild(runProperties);

                // добавляем основной текст
                wordDoc.MainDocumentPart.Document.Body.AppendChild(paragraphMain);

                paragraphMain.Append(runMainText);
            }
        }


        public void Export(string fileName, Bitmap imgSecond, string coordinates, string material, string colorant, string results, Bitmap imgFirst)
        {
            string[] tempFiles = new string[2];
            for (int i = 0; i < 2; i++)
            {
                string tempFilePath = System.IO.Path.GetTempFileName() + ".jpg";
                tempFiles[i] = tempFilePath;
                if (i == 0)
                {
                    imgFirst.Save(tempFilePath, ImageFormat.Jpeg);
                }
                else if (i == 1)
                {
                    imgSecond.Save(tempFilePath, ImageFormat.Jpeg);
                }
            }

            //добавление изображения в документ
            InsertAPictureWithText(fileName, tempFiles, coordinates, material, colorant, results);

            for (int i = 0; i < tempFiles.Length; i++)
            {
                File.Delete(tempFiles[i]);
            };

            MessageBoxResult result = MessageBox.Show($"Файл успешно сохранён по пути:\n {fileName}.\n Хотите открыть его сейчас?", "Открытие файла", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = true
                });
            }
        }

        #region Для спектрофотометра
        public void ExportSpect(string fileName, Bitmap img, string coordinates, string material, string colorant, string results)
        {
            string tempFilePath = System.IO.Path.GetTempFileName() + ".jpg";
            img.Save(tempFilePath, ImageFormat.Jpeg);


            //добавление изображения в документ
            InsertAPictureWithTextSpect(fileName, tempFilePath, coordinates, material, colorant, results);

            File.Delete(tempFilePath);

            MessageBoxResult result = MessageBox.Show($"Файл успешно сохранён по пути:\n {fileName}.\n Хотите открыть его сейчас?", "Открытие файла", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = true
                });
            }
        }

        public static void InsertAPictureWithTextSpect(string filePath, string picPath, string coordinates, string material, string colorant, string results)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();


                AddText(wordDocument, coordinates, material, colorant);

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                using (FileStream stream = new FileStream(picPath, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                CreateImg(wordDocument, mainPart.GetIdOfPart(imagePart), "Изображение оттенка экструдата по цветовым координатам:");


                AddResults(wordDocument, results);
            }
        }
        #endregion




    }
}

