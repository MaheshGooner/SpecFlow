﻿using System.IO;
using TechTalk.SpecFlow.CommonModels;

namespace TechTalk.SpecFlow.FileAccess
{
    public class BinaryFileAccessor : IBinaryFileAccessor
    {
        public Result<Stream> OpenAppendOrCreateFile(string filePath)
        {
            try
            {
                string parentDirectoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(parentDirectoryPath))
                {
                    Directory.CreateDirectory(parentDirectoryPath);
                }

                var streamToReturn = File.Open(filePath, FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                return Result<Stream>.Success(streamToReturn);
            }
            catch (IOException)
            {
                return Result<Stream>.Failure();
            }
        }
    }
}
