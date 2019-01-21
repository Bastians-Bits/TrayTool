using System;
using System.Drawing;
using System.IO;

namespace TrayTool
{
    class ImageHandler
    {
        private static ImageHandler instance;

        public string PathDefaultImage { get { return Path.GetTempPath(); } }
        private bool _isDefaultFolderCreated = false;
        private bool _isDefaultItemCreated = false;
        private bool _isSeperatorCreated = false;

        public static ImageHandler getIt()
        {
            if (instance == null)
                instance = new ImageHandler();
            return instance;
        }


        public string PathForApplication(string applicationPath)
        {
            return applicationPath;
        }

        public void CreateDefaultImages()
        {
            if (!_isDefaultFolderCreated)
            {
                if (!File.Exists(PathDefaultImage + "TrayToolFolder.png"))
                    File.WriteAllBytes(PathDefaultImage + "TrayToolFolder.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Folder, typeof(byte[])));
                else
                {
                    try
                    {
                        File.Delete(PathDefaultImage + "TrayToolFolder.png");
                        File.WriteAllBytes(PathDefaultImage + "TrayToolFolder.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Folder, typeof(byte[])));
                        _isDefaultFolderCreated = !_isDefaultFolderCreated;
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception("Could not delete " + PathDefaultImage + "TrayToolFolder.png", e);
                        
                    }
                }
            }

            if (!_isDefaultItemCreated)
            {
                if (!File.Exists(PathDefaultImage + "TrayToolItem.png"))
                    File.WriteAllBytes(PathDefaultImage + "TrayToolItem.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Shortcut, typeof(byte[])));
                else
                {
                    try
                    {
                        File.Delete(PathDefaultImage + "TrayToolItem.png");
                        File.WriteAllBytes(PathDefaultImage + "TrayToolItem.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Shortcut, typeof(byte[])));
                        _isDefaultItemCreated = !_isDefaultItemCreated;
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception("Could not delete " + PathDefaultImage + "TrayToolItem.png", e);
                        //throw ex;
                    }
                }
            }

            if (!_isSeperatorCreated)
            {
                if (!File.Exists(PathDefaultImage + "TrayToolSeperator.png"))
                    File.WriteAllBytes(PathDefaultImage + "TrayToolSeperator.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Seperator, typeof(byte[])));
                else
                {
                    try
                    {
                        File.Delete(PathDefaultImage + "TrayToolSeperator.png");
                        File.WriteAllBytes(PathDefaultImage + "TrayToolSeperator.png", (byte[])new ImageConverter().ConvertTo(Properties.Resources.Seperator, typeof(byte[])));
                        _isSeperatorCreated = !_isSeperatorCreated;
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception("Could not delete " + PathDefaultImage + "TrayToolSeperator.png", e);
                        //throw ex;
                    }
                }
            }
        }
    }
}
