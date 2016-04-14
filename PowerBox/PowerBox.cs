using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DropNet;
using DropNet.Models;
using DropNet.Authenticators;
using DropNet.Extensions;
using DropNet.Helpers;
using DropNet.Exceptions;
using System.IO;


namespace PowerBox
{

    [Cmdlet(VerbsCommon.Get, "DropboxConnection")]
    public class GetDropboxConnection : PSCmdlet
    {
        private string Key;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("dropkey", "secret key")]

        public String key
        {
            get { return Key; }
            set { Key = value; }
        }

        private string Secret;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("DropSec", "Sec")]

        public String secret
        {
            get { return Secret; }
            set { Secret = value; }
        }

        private string UseToke;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserToke", "Toke")]

        public String useToke
        {
            get { return UseToke; }
            set { UseToke = value; }
        }

        private string UseSec;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserSec", "UsSec")]

        public String useSecret
        {
            get { return UseSec; }
            set { UseSec = value; }
        }

        //private DropNetClient _client;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            //_client = new DropNetClient(Key, Secret);
            //_client.UseSandbox = true;
            DropBox dropboxClient = new DropBox(Key, Secret, UseToke, UseSec, false);
            WriteObject(dropboxClient);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }

    [Cmdlet(VerbsCommon.Get, "DropboxUpload")]
    public class GetDropboxUpload : PSCmdlet
    {
        private string Key;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("dropkey", "secret key")]

        public String key
        {
            get { return Key; }
            set { Key = value; }
        }

        private string Secret;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("DropSec", "Sec")]

        public String secret
        {
            get { return Secret; }
            set { Secret = value; }
        }

        private string UseToke;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserToke", "Toke")]

        public String useToke
        {
            get { return UseToke; }
            set { UseToke = value; }
        }

        private string UseSec;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserSec", "UsSec")]

        public String useSecret
        {
            get { return UseSec; }
            set { UseSec = value; }
        }

        private string fileLocation;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Location of the file that will be uploaded to dropbox"
            )
        ]
        [Alias("fileLoc", "filePlace")]

        public String filePath
        {
            get { return fileLocation; }
            set { fileLocation = value; }
        }

        private string fileToUpload;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Name of the file that will be uploaded to dropbox"
            )
        ]
        [Alias("file2Upload", "file2Up")]

        public String fileToUp
        {
            get { return fileToUpload; }
            set { fileToUpload = value; }
        }

        private DropNetClient _client;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            _client = new DropNetClient(Key, Secret, UseToke, UseSec);
            _client.UseSandbox = true;
            string file = (fileLocation + fileToUpload);
             Console.Write(file);
             int chunkSize = 1 * 1024 * 1024;

             var buffer = new byte[chunkSize];
             int bytesRead;
             int chunkCount = 0;
             int size = 0;
             ChunkedUpload chunkupload = null;

             using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
             {
                 while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                 {
                     size = size + buffer.Length;
                     chunkCount++;
                     if (chunkCount == 1)
                     {
                         if(bytesRead < chunkSize)
                         {
                             var smallerbuffer = new byte[bytesRead];
                             for (int i = 0; i < bytesRead; i++)
                             {
                                 smallerbuffer[i] = buffer[i];
                             }
                             chunkupload = _client.StartChunkedUpload(smallerbuffer);
                         }
                         else
                         {
                             chunkupload = _client.StartChunkedUpload(buffer);
                         }
                     }
                     else if(bytesRead >= chunkSize)
                     {
                         chunkupload = _client.AppendChunkedUpload(chunkupload, buffer);
                     }
                     else
                     {
                         var smallerbuffer = new byte[bytesRead];
                         for (int i = 0; i < bytesRead; i++)
                         {
                             smallerbuffer[i] = buffer[i];
                         }
                         chunkupload = _client.AppendChunkedUpload(chunkupload, smallerbuffer);
                     }

                 }
             }
             var metadata = _client.CommitChunkedUpload(chunkupload, ("/"+fileToUpload), true);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    };

    [Cmdlet(VerbsCommon.Get, "DropboxCreds")]
    public class GetDropboxCreds : PSCmdlet
    {
        private string Key;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("dropkey", "secret key")]

        public String key
        {
            get { return Key; }
            set { Key = value; }
        }

        private string Secret;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("DropSec", "Sec")]

        public String secret
        {
            get { return Secret; }
            set { Secret = value; }
        }

        private DropNetClient _client;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            _client = new DropNetClient(Key, Secret);
            _client.UseSandbox = true;
            var token = _client.GetToken();
            var url = _client.BuildAuthorizeUrl();
            Process.Start(url);
            WriteObject(_client);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }


    [Cmdlet(VerbsCommon.Get, "DropboxDownload")]
    public class GetDropboxDownload : PSCmdlet
    {
        private string Key;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("dropkey", "secret key")]

        public String key
        {
            get { return Key; }
            set { Key = value; }
        }

        private string Secret;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("DropSec", "Sec")]

        public String secret
        {
            get { return Secret; }
            set { Secret = value; }
        }

        private string UseToke;

        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserToke", "Toke")]

        public String useToke
        {
            get { return UseToke; }
            set { UseToke = value; }
        }

        private string UseSec;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Setup Token for Dropbox"
            )
        ]
        [Alias("UserSec", "UsSec")]

        public String useSecret
        {
            get { return UseSec; }
            set { UseSec = value; }
        }

        private string Name;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Location of the file that will be uploaded to dropbox"
            )
        ]
        [Alias("NameFile", "filename")]

        public String name
        {
            get { return Name; }
            set { Name = value; }
        }

        private string fileLocation;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Location of the file that will be uploaded to dropbox"
            )
        ]
        [Alias("fileLoc", "filePlace")]

        public String filePath
        {
            get { return fileLocation; }
            set { fileLocation = value; }
        }

        private string DownloadLocation;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Name of the file that will be uploaded to dropbox"
            )
        ]
        [Alias("downloadPath", "download")]

        public String downloadLocation
        {
            get { return DownloadLocation; }
            set { DownloadLocation = value; }
        }
      
        private string DownloadName;
        [
            Parameter
            (
                Mandatory = true,
                ValueFromPipelineByPropertyName = true,
                ValueFromPipeline = true,
                Position = 0,
                HelpMessage = "Name of the file that will be downloaded from dropbox"
            )
        ]
        [Alias("download name", "down name")]

        public String downloadName
        {
            get { return DownloadName; }
            set { DownloadName = value; }
        }

        private DropNetClient _client;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            _client = new DropNetClient(Key, Secret, UseToke, UseSec);
            _client.UseSandbox = true;
            Console.Write(fileLocation + Name);
           var fileBytes = _client.GetFile((fileLocation+Name));
            using (FileStream fs = new FileStream(DownloadLocation + DownloadName, FileMode.Create))
            {
                for (int i = 0; i < fileBytes.Length; i++)
                {
                    fs.WriteByte(fileBytes[i]);
                }
                fs.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < fileBytes.Length; i++)
                {
                    if (fileBytes[i] != fs.ReadByte())
                    {
                        //MessageBox.Show("Error writing data for " + file);
                        break;
                    }
                }
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    };

    public class DropBox
    {

        public DropBox(string key, string secret, string userToken, string userSecret, bool debug)
        {
            _client = new DropNetClient(key, secret, userToken, userSecret);
            _client.UseSandbox = true;
            Debug = debug;
        }

        public System.Collections.Generic.List<MetaData> DropboxContentsList(string dropboxPath)
        {
            string dbPath = dropboxPath.Replace(@"\", @"/");
            var test = _client.GetMetaData(dbPath);
            return test.Contents;
        }

        public void Upload(string filePath, string fileName, string dropboxPath, string dropboxFileName)
        {
            string file = (filePath + fileName);
            Console.Write(file);
            int chunkSize = 1 * 1024 * 1024;

            var buffer = new byte[chunkSize];
            int bytesRead;
            int chunkCount = 0;
            int size = 0;
            ChunkedUpload chunkupload = null;
            string dbPath = dropboxPath.Replace(@"\", @"/");

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    size = size + buffer.Length;
                    chunkCount++;
                    if (chunkCount == 1)
                    {
                        if (bytesRead < chunkSize)
                        {
                            var smallerbuffer = new byte[bytesRead];
                            for (int i = 0; i < bytesRead; i++)
                            {
                                smallerbuffer[i] = buffer[i];
                            }
                            chunkupload = _client.StartChunkedUpload(smallerbuffer);
                        }
                        else
                        {
                            chunkupload = _client.StartChunkedUpload(buffer);
                        }
                    }
                    else if (bytesRead >= chunkSize)
                    {
                        chunkupload = _client.AppendChunkedUpload(chunkupload, buffer);
                    }
                    else
                    {
                        var smallerbuffer = new byte[bytesRead];
                        for (int i = 0; i < bytesRead; i++)
                        {
                            smallerbuffer[i] = buffer[i];
                        }
                        chunkupload = _client.AppendChunkedUpload(chunkupload, smallerbuffer);
                    }

                }
            }
            var metadata = _client.CommitChunkedUpload(chunkupload, (dbPath + dropboxFileName), true);
        }

        public void Download(string path, string name, string dropboxPath, string dropboxName)
        {
            
            string dbPath = dropboxPath.Replace(@"\", @"/");
            //Console.Write(dbPath + dropboxName);
            if (fileExists(dbPath, dropboxName))
            {
                var fileBytes = _client.GetFile(dbPath + dropboxName);
                using (FileStream fs = new FileStream(path + name, FileMode.Create))
                {
                    for (int i = 0; i < fileBytes.Length; i++)
                    {
                        fs.WriteByte(fileBytes[i]);
                    }
                    fs.Seek(0, SeekOrigin.Begin);
                    for (int i = 0; i < fileBytes.Length; i++)
                    {
                        if (fileBytes[i] != fs.ReadByte())
                        {
                            //MessageBox.Show("Error writing data for " + file);
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.Write(dbPath + dropboxName + " does not exist");
            }
        }

        public void Move(string fromPath, string name, string toPath)
        {
            string fPath = fromPath.Replace(@"\", @"/");
            string tPath = toPath.Replace(@"\", @"/");
            if (fileExists(fromPath, name))
            {
                _client.Move((fPath + name), (tPath + name));
            }
            else
            {
                Console.Write(fromPath + name + " does not exist");
            }
        }

        public void Delete(string path, string Name)
        {
            string deletePath = path.Replace(@"\", @"/");
            if(fileExists(path,Name))
            {
                _client.Delete((deletePath + Name));
                Console.Write(deletePath + Name + " Has been deleted");
            }
            else{
                Console.Write(deletePath + Name + " Does not exist");
            }
        }

        public bool fileExists(string path, string Name)
        {
            string testPath = path.Replace(@"\", @"/");
            var temp = _client.GetMetaData(testPath).Contents;
            bool exists = false;
            for (int i = 0; i < temp.Count && !exists; i++ )
            {
                if (temp[i].Name == Name)
                {
                    exists = true;
                }
            }
            return exists;
        }

        private bool Debug;
        private DropNetClient _client;
    }

}
