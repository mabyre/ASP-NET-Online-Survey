// 
// Collection de fichiers pour les trier par date de dernier acces
// DateTime GetLastWriteTime( string path );
//
//
using System;
using System.Collections;
using System.Collections.Generic;

///<summary>
/// Description d'un Fichier
///</summary>
public class Fichier : IComparer<Fichier>
{
    public Fichier() 
    {
        _nom = "";
        _dateDerniereEcriture = Tools.DateInit;
    }

    public Fichier
    (
        string Nom,
        DateTime dateDerniereEcriture
    )
    {
        _nom = Nom;
        _dateDerniereEcriture = dateDerniereEcriture;
    }

    private string _nom;
    public string Nom
    {
        get { return _nom; }
        set { _nom = value; }			
    }

    private DateTime _dateDerniereEcriture;
    public DateTime DateDerniereEcriture
    {
        get { return _dateDerniereEcriture; }
        set { _dateDerniereEcriture = value; }
    }

    // Methode lie a IComparer<Fichier>
    public int Compare( Fichier x, Fichier y )
    {
        return x.DateDerniereEcriture.CompareTo( y.DateDerniereEcriture );
    }
}
