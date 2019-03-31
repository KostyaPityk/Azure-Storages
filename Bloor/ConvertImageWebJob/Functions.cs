using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Drawing;
using StackBlur.Extensions;

namespace ConvertImageWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("transformqueue")] string imageId,
            [Blob("images/{queueTrigger}", FileAccess.Read)] Stream inputImageStream,
            [Blob("images/{queueTrigger}", FileAccess.Write)] Stream outputImageStream)
        {
            using (var bitmap = new Bitmap(inputImageStream))
            {
                int radius = 70;

                bitmap.StackBlur(radius);

                bitmap.Save(outputImageStream, bitmap.RawFormat);
            }
        }
    }
}
