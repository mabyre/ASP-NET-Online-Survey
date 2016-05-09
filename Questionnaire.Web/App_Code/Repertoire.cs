using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Parcourir un Répertoire de façon récursive
/// </summary>
public class RepertoireInfo
{
    private static long _LongueurFiles  = 0;

    public RepertoireInfo()
	{
        _LongueurFiles = 0;
	}

    public static long GetTaille( string path )
    {
        _LongueurFiles = 0;
        if( Directory.Exists( path ) ) 
        {
            // This path is a directory
            ProcessDirectory(path);
        }
        return _LongueurFiles;
    }

    // Process all files in the directory passed in, recurse on any directories 
    // that are found, and process the files they contain.
    public static void ProcessDirectory( string targetDirectory )
    {
        // Process the list of files found in the directory.

        DirectoryInfo di = new DirectoryInfo( targetDirectory );
        FileInfo[] fi = di.GetFiles();

        //string[] fileEntries = Directory.GetFiles( targetDirectory, "*.*", SearchOption.TopDirectoryOnly );
        foreach ( FileInfo f in fi )
            ProcessFile( f.Length );

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories( targetDirectory );
        foreach ( string subdirectory in subdirectoryEntries )
            ProcessDirectory( subdirectory );
    }

    // Insert logic for processing found files here.
    public static void ProcessFile( long length )
    {
        //FileInfo fi = new FileInfo( path );
        _LongueurFiles += length;
    }

}
