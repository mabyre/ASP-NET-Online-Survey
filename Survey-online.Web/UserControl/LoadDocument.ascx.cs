//
// Il y a une proprietee de HtmlInput qui est Value mais ca marche comme ca alors on verra apres
// On peut rien faire avec ce putain d'objet PostedFile.FileName Value sont en lecture seulement
// donc on premier PostBack qui se pointe tout est efface et on ne peut rien faire !
// C'est pas la peine d'essayer
// La propriete Accept ne sert à rien non plus les type MIMEs sont tellement bordeliques ...
//

using System;
using System.Web;
using System.IO;

public partial class Controls_LoadDocument : System.Web.UI.UserControl
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            ValidationMessage.Visible = false;
        }
    }

    protected void ButtonAdd_OnClick( object sender, EventArgs e )
    {
        if ( DocumentNom.PostedFile.FileName == "" ) 
        {
            ValidationMessage.Visible = true;
            ValidationMessage.Text = "Choisissez un fichier à importer.";
            return;
        }

        HttpPostedFile file = DocumentNom.PostedFile;
        int size = file.ContentLength;
        if ( size <= 0 )
        {
            ValidationMessage.Visible = true;
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Text = "Un problème est survenu avec le téléchargement du fichier sur le serveur, le fichier n'est pas disponible.";
            return;
        }

        String virtualPath = "~/Document/";
        string filePath = virtualPath;
        virtualPath += Path.GetFileName( DocumentNom.PostedFile.FileName );
        String physicalDir = Server.MapPath( filePath );
        String physicalPath = Server.MapPath( virtualPath );

        // Creer le Repertoire s'il n'exite pas
        if ( !Directory.Exists( physicalDir ) )
        {
            Directory.CreateDirectory( physicalDir );
        }

        // Sauver le fichier
        try
        {
            int tailleMax = 10 * 1024 * 1024; // 10 Mo
            if ( DocumentNom.PostedFile.ContentLength >= tailleMax )
            {
                ValidationMessage.Visible = true;
                ValidationMessage.Text += "Error taille du fichier : " + Tools.FileSizeFormat( DocumentNom.PostedFile.ContentLength, "N" );
                ValidationMessage.Text += " supérieure à la taille maximum : " + Tools.FileSizeFormat( tailleMax, "N" );
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                return;
            }
            DocumentNom.PostedFile.SaveAs( physicalPath );
        }
        catch ( Exception ex )
        {
            ValidationMessage.Visible = true;
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Text = ex.Message;
            return;
        }

        // On ne peut pas lire directement le fichier sinon on obtient une erreur de securite
        // sur le fichier quelque soit ses attributs sur le client. Je pense qu'il faut
        // d'abord transferer le fichier que le serveur
        try
        {
            FileStream fs = new FileStream( physicalPath, FileMode.Open, FileAccess.Read );
        }
        catch
        {
            ValidationMessage.Visible = true;
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Text = "Accès en lecture au fichier refusé ! <br>Vous devez donner le contrôle total à tout le monde <br> pour le fichier que vous souhaitez importé.";
            return;
        }

        ValidationMessage.Visible = true;
        ValidationMessage.Text = "Téléchargement du fichier terminé.";

        string url = string.Format( "?cmd=parse&file={0}", physicalPath );
        Response.Redirect( Request.FilePath + url, true );
    }
}

