using System;
using System.Collections.Generic;
using System.Data.SqlClient;  //namespace koji je .NET Data Provider za SQL Server i koji nam omogucava da pristupamo podacima iz baza podataka
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPFTeretana
{
    public class Konekcija
    {
      public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-06GNPCT\SQLEXPRESS",
                InitialCatalog = "Teretana",
                IntegratedSecurity = true
            };
            string con = ccnSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}
