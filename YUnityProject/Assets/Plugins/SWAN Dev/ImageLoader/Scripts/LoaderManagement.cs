using System;
using System.IO;
using UnityEngine;

// ImageBox namespace.
namespace IMBX
{
    /// <summary>
    /// The loader Cache Management and Loading Settings.
    /// </summary>
    [Serializable]
    public class LoaderManagement
    {
        [Tooltip("If true, show debug log in the editor console.")]
        public bool IsDebug;

        [Tooltip("The behavior for handling Load and Cache files:" +
            "\n'NoCache': do not save and load from the local cache folder." +
            "\n'UseCached': download at the first time, use the locally cached file if exist." +
            "\n'Replace': always download and replace the locally cached file.")]
        public ImageLoader.CacheMode CacheMode = ImageLoader.CacheMode.UseCached;

        [Tooltip("If 'true', automatically call the ManageCachedFiles method to manage cached files when loading images/files; otherwise, you must manually call it in your scripts." +
            "\n\n* (Manually) Coding: IMBX.ImageLoader.ManageCachedFiles(...);")]
        public bool AutoManageCachedFiles = true;

        [Tooltip("The enum that determines which application path to load and save(cache) file to.")]
        public FilePathName.AppPath CacheDirectoryEnum = FilePathName.AppPath.PersistentDataPath;

        [Tooltip("The sub-folder under cache directory for loading and storing(caching) files to.")]
        public string FolderName = "";

        [Tooltip("If 'true', for URL start with 'http', the loader will cache the image as per URL address. " +
            "This will bypass the filename generating function to use an MD5 hash(generated based on the URL) as the filename." +
            "\n\nAnd, when you download with the same URL next time and the CacheMode is 'UseCached', the loader will load the locally cached file by comparing the URL(MD5 hash).")]
        public bool CacheAsPerUrl = false;

        [Tooltip("The filename prefix for storing images. The final filename is combined by this prefix, separator, and index together.")]
        public string FileNamePrefix = "Pic";
        [Tooltip("e.g.: .jpg or .png.")]
        public string FileExtension = ".png";
        [Tooltip("Number of Digits for the Index follow the Filename Prefix.")]
        public uint FileIndexFormatDigitsCount = 4;
        [Tooltip("File Name Starting Index. (Set this value to set an offset for the filename index. The default starting index is 0.)")]
        public uint FileNameStartingIndex = 0;
        [Tooltip("Separator text between the File Name and Index. (Please use filename friendly characters only)")]
        public string FileNameAndIndexSeparator = "_";

        [Tooltip("The maximum number of files can be stored(Cached) in the cache folder. ( 0 means no limit )")]
        public uint MaxCacheFilePerFolder = 0;

        [Tooltip("Time duration in seconds for keeping files not being deleted. (eg. 86400 = 3600s * 24h = 1 day.)" +
            "\n\nZero means no minimum keeping time: when file num limit exceeded, older files will be removed without checking their last-modified-time")]
        public uint MinTimeForKeepingFiles = 0;

        [Tooltip("Time duration in seconds, files must be deleted if the last modify time from now more than this duration. (Zero means infinite)" +
            "\nFor example, set this value = 3600(1 hour), then all files in the folder those modified 1 hour ago will be deleted. ")]
        public uint MaxTimeForKeepingFiles = 0;

        [Tooltip("Number of times to retry when a loading failed. Retry per second until the retry value is Zero.")]
        public uint LoadingRetry = 0;

        [Tooltip("The max time duration for waiting for the download process, stop and kill the loader if time exceeds.")]
        public float LoadingTimeOut = 0;

        [Tooltip("If 'true', allows downloading the same URL using multiple loaders, else it will not start a new download if that URL is being downloaded." +
            "\n* This flag always 'true' for ProTexturePlayer *")]
        public bool AllowDuplicateDownload = true;

        [Tooltip("Default : load the image as texture normally." +
            "\nLoadTextureAndKeepData : load the image as texture and keep the image bytes in the Result/CacheItem objects." +
            "\nLoadFileDataOnly : load the image bytes into the Result/CacheItem objects, but do not create textures. Which you can decode later (using your own image decoder)." +
            "\n* ProTexturePlayer supports Default mode only.")]
        public LoadingMode LoadFileMode;

        [Tooltip("If the file is already cached, update its file time to the current DateTime. This prevents it from being deleted by the cache management functions.")]
        public bool SetFileTimeOnAccess;

        public LoaderManagement() { }

        /// <summary>
        /// Creates a new LoaderManagement object using the settings of an existing one.
        /// </summary>
        /// <param name="LMGT"> The origin LoaderManagement object. Its settings will be copied to the new object. </param>
        public LoaderManagement(LoaderManagement LMGT)
        {
            this.IsDebug = LMGT.IsDebug;
            this.CacheDirectoryEnum = LMGT.CacheDirectoryEnum;
            this.CacheMode = LMGT.CacheMode;
            this.AutoManageCachedFiles = LMGT.AutoManageCachedFiles;
            this.FileExtension = LMGT.FileExtension;
            this.FileIndexFormatDigitsCount = LMGT.FileIndexFormatDigitsCount;
            this.FileNameAndIndexSeparator = LMGT.FileNameAndIndexSeparator;
            this.CacheAsPerUrl = LMGT.CacheAsPerUrl;
            this.FileNamePrefix = LMGT.FileNamePrefix;
            this.FileNameStartingIndex = LMGT.FileNameStartingIndex;
            this.FolderName = LMGT.FolderName;
            this.LoadingRetry = LMGT.LoadingRetry;
            this.LoadingTimeOut = LMGT.LoadingTimeOut;
            this.AllowDuplicateDownload = LMGT.AllowDuplicateDownload;
            this.MaxCacheFilePerFolder = LMGT.MaxCacheFilePerFolder;
            this.MaxTimeForKeepingFiles = LMGT.MaxTimeForKeepingFiles;
            this.MinTimeForKeepingFiles = LMGT.MinTimeForKeepingFiles;
            this.LoadFileMode = LMGT.LoadFileMode;
            this.SetFileTimeOnAccess = LMGT.SetFileTimeOnAccess;
        }

        public enum LoadingMode
        {
            /// <summary>
            /// Load the image as texture normally.
            /// </summary>
            Default = 0,

            /// <summary>
            /// Load the image as texture and Keep the image bytes in the Result/CacheItem objects. *This variable has no effect in ProTexturePlayer*
            /// </summary>
            LoadTextureAndKeepData,

            /// <summary>
            /// Load the image bytes into the Result/CacheItem objects, but do not create textures. Which you can decode later (using your own image decoder).
            /// *This variable has no effect in ProTexturePlayer*
            /// </summary>
            LoadFileDataOnly,
        }

        /// <summary>
        /// The root directory for loading and storing(caching) image files.
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return FilePathName.Instance.GetAppPath(CacheDirectoryEnum);
            }
        }

        /// <summary>
        /// The cache folder path that combined by the (root) CacheDirectory and FolderName.
        /// </summary>
        public string CacheFolderPath
        {
            get
            {
                return string.IsNullOrEmpty(FolderName) ? CacheDirectory : Path.Combine(CacheDirectory, FolderName);
            }
        }

        /// <summary>
        /// Check if a specific image exists in the cache directory/folder of this LoaderManagement object.
        /// </summary>
        /// <param name="filename"> The filename of the requesting image, e.g. MyImage001 (optional to provide the file extension) </param>
        public bool HasStorageCache(string filename)
        {
            if (CacheDirectoryEnum == FilePathName.AppPath.StreamingAssetsPath)
            {
                Debug.LogWarning("StreamingAssetsPath is not intended for caching files. Please select PersistentDataPath or TemporaryCachePath instead.");
                return false;
            }
            string filePath = Path.Combine(CacheFolderPath, filename);
            return File.Exists(filePath) || File.Exists(filePath + FileExtension);
        }

        /// <summary>
        /// Delete a cached file by its cache filename. In which the file was cached with the current LMGT path setting.
        /// </summary>
        public void DeleteCachedFileByFilename(string filename)
        {
            string filePath = Path.Combine(CacheFolderPath, filename);
            ImageLoader.DeleteFileByPath(filePath);
        }

        /// <summary>
        /// Delete a cached file by its URL. In which the file was cached with the current LMGT path setting and CacheAsPerUrl is enabled.
        /// </summary>
        public void DeleteCachedFileByUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string filename = MD5Util.ToMD5Hash(url); // MD5 hash as filename, for Cache-As-Per-URL
                string filePath = Path.Combine(CacheFolderPath, filename);
                ImageLoader.DeleteFileByPath(filePath);
            }
        }

        public void SetFileNameFormat(uint fileIndexFormatDigitsCount, uint fileNameStartingIndex = 0, string fileNameAndIndexSeparator = "_")
        {
            FileIndexFormatDigitsCount = fileIndexFormatDigitsCount;
            FileNameStartingIndex = fileNameStartingIndex;
            FileNameAndIndexSeparator = fileNameAndIndexSeparator;
        }

        /// <summary>
        /// Generate a FileName(without extension) base on FileNamePrefix, FileIndexFormatDigitsCount, FileNameStartingIndex, FileNameAndIndexSeparator and file index.
        /// ( e.g. "Pic" + "-" + string format "0000" with fileIndex 12 = "Pic-0012" )
        /// </summary>
        public string GenerateFileName(int fileIndex)
        {
            if (FileIndexFormatDigitsCount <= 0) return FileNamePrefix;
            FileIndexFormatDigitsCount = (uint)Mathf.Clamp(FileIndexFormatDigitsCount, 0, 18);
            string fileIndexFormat = "{0," + FileIndexFormatDigitsCount + ":D" + FileIndexFormatDigitsCount + "}";
            string fileName = FileNamePrefix + FileNameAndIndexSeparator + String.Format(fileIndexFormat, FileNameStartingIndex + fileIndex); // e.g. "Pic" + "-" + string format "0000" with fileIndex 12 = "Pic-0012"
            return fileName;
        }
        public string GenerateFileName(uint fileIndex)
        {
            return GenerateFileName((int)fileIndex);
        }
    }
}
