using AngularSPAWebAPI.Controllers;
using AngularSPAWebAPI.Models;
using CBAS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;
using MathNet.Numerics;
using System.Data.SqlClient;
using Remotion.Linq.Clauses;

namespace AngularSPAWebAPI.Services
{

    public class CogbytesService
    {
        // Function Definition to get paper info from DOI
        // private static readonly HttpClient client = new HttpClient();

        public List<CogbytesFileType> GetFileTypes()
        {
            List<CogbytesFileType> FileTypeList = new List<CogbytesFileType>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From FileType"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FileTypeList.Add(new CogbytesFileType
                    {
                        ID = Int32.Parse(dr["FileTypeID"].ToString()),
                        FileType = Convert.ToString(dr["FileType"].ToString()),

                    });
                }
            }

            return FileTypeList;
        }

        // Function Definition to extract list of all Cognitive Tasks
        public List<PubScreenTask> GetTasks()
        {
            List<PubScreenTask> TaskList = new List<PubScreenTask>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Task"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TaskList.Add(new PubScreenTask
                    {
                        ID = Int32.Parse(dr["TaskID"].ToString()),
                        Task = Convert.ToString(dr["Name"].ToString()),


                    });
                }
            }

            return TaskList;
        }

        // Function Definition to extract list of all Species
        public List<PubScreenSpecie> GetSpecies()
        {
            List<PubScreenSpecie> SpecieList = new List<PubScreenSpecie>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Species"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SpecieList.Add(new PubScreenSpecie
                    {
                        ID = Int32.Parse(dr["SpeciesID"].ToString()),
                        Species = Convert.ToString(dr["Species"].ToString()),


                    });
                }
            }

            return SpecieList;
        }

        //// Function Definition to extract list of all Sex
        public List<PubScreenSex> GetSex()
        {
            List<PubScreenSex> SexList = new List<PubScreenSex>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Sex"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SexList.Add(new PubScreenSex
                    {
                        ID = Int32.Parse(dr["SexID"].ToString()),
                        Sex = Convert.ToString(dr["Sex"].ToString()),


                    });
                }
            }

            return SexList;
        }

        //// Function Definition to extract list of all Strains
        public List<PubScreenStrain> GetStrains()
        {
            List<PubScreenStrain> StrainList = new List<PubScreenStrain>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Strain"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StrainList.Add(new PubScreenStrain
                    {
                        ID = Int32.Parse(dr["StrainID"].ToString()),
                        Strain = Convert.ToString(dr["Strain"].ToString()),


                    });
                }
            }

            return StrainList;
        }

        //// Function Definition to extract list of all Genotypes
        public List<Geno> GetGenos()
        {
            List<Geno> GenoList = new List<Geno>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Genotype"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    GenoList.Add(new Geno
                    {
                        ID = Int32.Parse(dr["GenoID"].ToString()),
                        Genotype = Convert.ToString(dr["Genotype"].ToString()),


                    });
                }
            }

            return GenoList;
        }

        //// Function Definition to extract list of all ages
        public List<Age> GetAges()
        {
            List<Age> AgeList = new List<Age>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Age"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AgeList.Add(new Age
                    {
                        ID = Int32.Parse(dr["AgeID"].ToString()),
                        AgeInMonth = Convert.ToString(dr["AgeInMonth"].ToString()),


                    });
                }
            }

            return AgeList;
        }


        //// Function defintion to add a new author to database
        public int AddAuthors(PubScreenAuthor author, string userEmail)
        {
            string sql = $@"Insert into Author (FirstName, LastName, Affiliation, Username) Values
                            ('{author.FirstName}', '{author.LastName}', '{author.Affiliation}', '{userEmail}'); SELECT @@IDENTITY AS 'Identity';";

            return Int32.Parse(Dal.ExecScalarCog(sql).ToString());
        }


        //// Function Definition to extract list of Authors 
        public List<PubScreenAuthor> GetAuthors()
        {
            List<PubScreenAuthor> AuthorList = new List<PubScreenAuthor>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From Author Order By AuthorID"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AuthorList.Add(new PubScreenAuthor
                    {
                        ID = Int32.Parse(dr["AuthorID"].ToString()),
                        FirstName = Convert.ToString(dr["FirstName"].ToString()),
                        LastName = Convert.ToString(dr["LastName"].ToString()),
                        Affiliation = Convert.ToString(dr["Affiliation"].ToString()),

                    });
                }
            }

            return AuthorList;
        }

        public int AddNewPI(Request request, string userEmail)
        {


            string sql = $@"Insert into PI (Username, FullName, Email, Affiliation) Values
                            ('{userEmail}', '{HelperService.EscapeSql(request.PIFullName)}',
                             '{HelperService.EscapeSql(request.PIEmail)}', '{HelperService.EscapeSql(request.PIInstitution)}'); SELECT @@IDENTITY AS 'Identity';";

            return Int32.Parse(Dal.ExecScalarCog(sql).ToString());

        }

        //// Function Definition to extract list of PIs 
        public List<Request> GetPIs()
        {
            List<Request> PIList = new List<Request>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From PI Order By PIID"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PIList.Add(new Request
                    {
                        ID = Int32.Parse(dr["PIID"].ToString()),
                        PIFullName = Convert.ToString(dr["FullName"].ToString()),
                        PIEmail = Convert.ToString(dr["Email"].ToString()),
                        PIInstitution = Convert.ToString(dr["Affiliation"].ToString()),

                    });
                }
            }

            return PIList;
        }
        //// Delete publication
        //public void DeletePublicationById(int pubId)
        //{
        //    string sql = $@" 
        //                     Delete From Publication_Author Where PublicationID = {pubId};
        //                     Delete From Publication_CellType Where PublicationID = {pubId};
        //                     Delete From Publication_Disease Where PublicationID = {pubId};
        //                     Delete From Publication_Method Where PublicationID = {pubId};
        //                     Delete From Publication_NeuroTransmitter Where PublicationID = {pubId};
        //                     Delete From Publication_PaperType Where PublicationID = {pubId};
        //                     Delete From Publication_Region Where PublicationID = {pubId};
        //                     Delete From Publication_Sex Where PublicationID = {pubId};
        //                     Delete From Publication_Specie Where PublicationID = {pubId};
        //                     Delete From Publication_Strain Where PublicationID = {pubId};
        //                     Delete From Publication_SubRegion Where PublicationID = {pubId};
        //                     Delete From Publication_Task Where PublicationID = {pubId};
        //                     Delete From Publication Where id = { pubId};";

        //    Dal.ExecuteNonQueryPub(sql);
        //}

        ////************************************************************************************Adding Repository*************************************************************************************
        // Function Definition to add a new repository to database Cogbytes
        public int? AddRepository(Cogbytes repository, string Username)
        {

            string sqlRepository = $@"Insert into UserRepository (Title, Date, DOI, Keywords, PrivacyStatus, Description, AdditionalNotes, Link, Username, DateRepositoryCreated) Values
                                    ('{HelperService.EscapeSql((HelperService.NullToString(repository.Title)).Trim())}',
                                     '{repository.Date}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(repository.DOI)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(repository.Keywords)).Trim())}',
                                     '{repository.PrivacyStatus}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(repository.Description)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(repository.AdditionalNotes)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(repository.Link)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(Username)))}',
                                     '{repository.DateRepositoryCreated}'
                                      ); SELECT @@IDENTITY AS 'Identity'; ";

            int RepositoryID = Int32.Parse(Dal.ExecScalarCog(sqlRepository).ToString());

            // Adding Author **********************************************************************************************************************

            string sqlAuthor = "";
            for (int i = 0; i < repository.AuthourID.Length; i++)
            {
                sqlAuthor += $@"Insert into RepAuthor (AuthorID, RepID) Values ({repository.AuthourID[i]}, {RepositoryID});";
            }
            if (sqlAuthor != "") { Dal.ExecuteNonQueryCog(sqlAuthor); };

            // Adding PI

            string sqlPI = "";
            for (int i = 0; i < repository.PIID.Length; i++)
            {
                sqlPI += $@"Insert into RepPI (PIID, RepID) Values ({repository.PIID[i]}, {RepositoryID});";
            }
            if (sqlPI != "") { Dal.ExecuteNonQueryCog(sqlPI); };

            return RepositoryID;

        }

        //// Function Definition to extract a user's repositories 
        public List<Cogbytes> GetRepositories(string userEmail)
        {
            List<Cogbytes> RepList = new List<Cogbytes>();
            using (DataTable dt = Dal.GetDataTableCog($@"Select * From UserRepository Where Username='{userEmail}' Order By DateRepositoryCreated"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int repID = Int32.Parse(dr["RepID"].ToString());
                    RepList.Add(new Cogbytes
                    {
                        ID = repID,
                        Title = Convert.ToString(dr["Title"].ToString()),
                        Date = Convert.ToString(dr["Date"].ToString()),
                        Keywords = Convert.ToString(dr["Keywords"].ToString()),
                        DOI = Convert.ToString(dr["DOI"].ToString()),
                        Link = Convert.ToString(dr["Link"].ToString()),
                        PrivacyStatus = Boolean.Parse(dr["PrivacyStatus"].ToString()),
                        Description = Convert.ToString(dr["Description"].ToString()),
                        AdditionalNotes = Convert.ToString(dr["AdditionalNotes"].ToString()),
                        AuthourID = FillCogbytesItemArray($"Select AuthorID From RepAuthor Where RepID={repID}", "AuthorID"),
                        PIID = FillCogbytesItemArray($"Select PIID From RepPI Where RepID={repID}", "PIID")
                    });
                }
            }

            return RepList;
        }

        // Function Definition to edit a respository in database Cogbytes
        public bool EditRepository(int repositoryID, Cogbytes repository, string Username)
        {

            string sqlRepository = $@"Update UserRepository set Title = @title, Date = @date, DOI = @doi, Keywords = @keywords, PrivacyStatus = @privacyStatus,
                                                                Description = @description, AdditionalNotes = @additionalNotes, Link = @link
                                                                where RepID = {repositoryID}";

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@title", HelperService.NullToString(HelperService.EscapeSql(repository.Title)).Trim()));
            parameters.Add(new SqlParameter("@date", HelperService.NullToString(HelperService.EscapeSql(repository.Date)).Trim()));
            parameters.Add(new SqlParameter("@doi", HelperService.NullToString(HelperService.EscapeSql(repository.DOI)).Trim()));
            parameters.Add(new SqlParameter("@keywords", HelperService.NullToString(HelperService.EscapeSql(repository.Keywords)).Trim()));
            parameters.Add(new SqlParameter("@privacyStatus", HelperService.NullToString(repository.PrivacyStatus).Trim()));
            parameters.Add(new SqlParameter("@description", HelperService.NullToString(HelperService.EscapeSql(repository.Description)).Trim()));
            parameters.Add(new SqlParameter("@additionalNotes", HelperService.NullToString(HelperService.EscapeSql(repository.AdditionalNotes)).Trim()));
            parameters.Add(new SqlParameter("@link", HelperService.NullToString(HelperService.EscapeSql(repository.Link)).Trim()));

            Int32.Parse(Dal.ExecuteNonQueryCog(CommandType.Text, sqlRepository, parameters.ToArray()).ToString());

            string sqlAuthor = "";

            string sqlDelete = $"DELETE From RepAuthor where RepID = {repositoryID}";
            Dal.ExecuteNonQueryCog(sqlDelete);

            for (int i = 0; i < repository.AuthourID.Length; i++)
            {
                sqlAuthor += $@"Insert into RepAuthor (AuthorID, RepID) Values ({repository.AuthourID[i]}, {repositoryID});";
            }

            Dal.ExecuteNonQueryCog(sqlAuthor);

            string sqlPI = "";
            sqlDelete = $"DELETE From RepPI where RepID = {repositoryID}";
            Dal.ExecuteNonQueryCog(sqlDelete);

            for (int i = 0; i < repository.PIID.Length; i++)
            {
                sqlPI += $@"Insert into RepPI (PIID, RepID) Values ({repository.PIID[i]}, {repositoryID});";
            }

            Dal.ExecuteNonQueryCog(sqlPI);

            return true;

        }

        // Function Definition to add a new repository to database Cogbytes
        public int? AddUpload(CogbytesUpload upload)
        {

            string sqlUpload = $@"Insert into Upload (RepID, FileTypeID, Name, DateUpload, Description, AdditionalNotes, IsIntervention, InterventionDescription, ImageIds, ImageDescription, Housing, LightCycle, TaskBattery) Values
                                    ('{upload.RepID}',
                                     '{upload.FileTypeID}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.Name)).Trim())}',
                                     '{upload.DateUpload}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.Description)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.AdditionalNotes)).Trim())}',
                                     '{upload.IsIntervention}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.InterventionDescription)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.ImageIds)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.ImageDescription)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.Housing)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.LightCycle)).Trim())}',
                                     '{HelperService.EscapeSql((HelperService.NullToString(upload.TaskBattery)).Trim())}'
                                      ); SELECT @@IDENTITY AS 'Identity'; ";

            int UploadID = Int32.Parse(Dal.ExecScalarCog(sqlUpload).ToString());

            // Adding Author **********************************************************************************************************************

            //string sqlAuthor = "";
            //for (int i = 0; i < repository.AuthourID.Length; i++)
            //{
            //    sqlAuthor += $@"Insert into RepAuthor (AuthorID, RepID) Values ({repository.AuthourID[i]}, {RepositoryID});";
            //}
            //if (sqlAuthor != "") { Dal.ExecuteNonQueryCog(sqlAuthor); };

            //// Adding PI

            //string sqlPI = "";
            //for (int i = 0; i < repository.PIID.Length; i++)
            //{
            //    sqlPI += $@"Insert into RepPI (PIID, RepID) Values ({repository.PIID[i]}, {RepositoryID});";
            //}
            //if (sqlPI != "") { Dal.ExecuteNonQueryCog(sqlPI); };

            return UploadID;

        }

        ////*******************************************************************************************************************************************************************

        //// Edit publication
        //public bool EditPublication(int publicationId, PubScreen publication, string Username)
        //{


        //    string sqlPublication = $@"Update Publication set Title = @title, Abstract = @abstract, Keywords = @keywords,
        //                                                       DOI = @doi, Year = @year where id = {publicationId}";

        //    var parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@title", HelperService.NullToString(HelperService.EscapeSql(publication.Title)).Trim()));
        //    parameters.Add(new SqlParameter("@abstract", HelperService.NullToString(HelperService.EscapeSql(publication.Abstract)).Trim()));
        //    parameters.Add(new SqlParameter("@keywords", HelperService.NullToString(HelperService.EscapeSql(publication.Keywords)).Trim()));
        //    parameters.Add(new SqlParameter("@doi", HelperService.NullToString(HelperService.EscapeSql(publication.DOI)).Trim()));
        //    parameters.Add(new SqlParameter("@year", HelperService.NullToString(HelperService.EscapeSql(publication.Year)).Trim()));

        //    Int32.Parse(Dal.ExecuteNonQueryPub(CommandType.Text, sqlPublication, parameters.ToArray()).ToString());

        //    string sqlDelete = "";

        //    // Editing Author **********************************************************************************************************************
        //    // Edit Publication_Author Table 
        //    if (publication.AuthourID != null && publication.AuthourID.Length != 0)
        //    {
        //        string sqlAuthor = "";

        //        sqlDelete = $"DELETE From Publication_Author where PublicationID = {publicationId}";
        //        Dal.ExecuteNonQueryPub(sqlDelete);

        //        for (int i = 0; i < publication.AuthourID.Length; i++)
        //        {
        //            sqlAuthor += $@"Insert into Publication_Author (AuthorID, PublicationID) Values ({publication.AuthourID[i]}, {publicationId});";
        //        }

        //        if (sqlAuthor != "") { Dal.ExecuteNonQueryPub(sqlAuthor); };

        //    }

        //    //Edit Publication_PaperType Table**********************************************************************************************

        //    // When DOI or Pubmedkey is not available
        //    var paperTypeId = 0;
        //    if (!string.IsNullOrEmpty(publication.PaperType))
        //    {
        //        var sql = $"select id from PaperType where ltrim(rtrim(papertype)) = '{HelperService.NullToString(HelperService.EscapeSql(publication.PaperType)).Trim()}'";
        //        var retPaperType = Dal.ExecScalarPub(sql);
        //        if (retPaperType != null)
        //        {
        //            paperTypeId = Int32.Parse(Dal.ExecScalarPub(sql).ToString());

        //        }
        //    }

        //    if (paperTypeId != 0)
        //    {
        //        sqlDelete = $"DELETE From Publication_PaperType where PublicationID = {publicationId}";
        //        Dal.ExecuteNonQueryPub(sqlDelete);

        //        string sqlPaperType = "";
        //        sqlPaperType = $@"Insert into Publication_PaperType (PaperTypeID, PublicationID) Values ({paperTypeId}, {publicationId});";
        //        Dal.ExecuteNonQueryPub(sqlPaperType);
        //    }

        //    //Editing to Publication_Task
        //    if (publication.TaskID != null && publication.TaskID.Length != 0)
        //    {
        //        sqlDelete = $"DELETE From Publication_Task where PublicationID = {publicationId}";
        //        Dal.ExecuteNonQueryPub(sqlDelete);

        //        string sqlTask = "";
        //        for (int i = 0; i < publication.TaskID.Length; i++)
        //        {
        //            sqlTask += $@"Insert into Publication_Task (TaskID, PublicationID) Values ({publication.TaskID[i]}, {publicationId});";

        //        }
        //        if (sqlTask != "") { Dal.ExecuteNonQueryPub(sqlTask); };

        //    }

        //    //Editing to Publication_Specie
        //    sqlDelete = $"DELETE From Publication_Specie where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.SpecieID != null && publication.SpecieID.Length != 0)
        //    {
        //        string sqlSpecie = "";
        //        for (int i = 0; i < publication.SpecieID.Length; i++)
        //        {
        //            sqlSpecie += $@"Insert into Publication_Specie (SpecieID, PublicationID) Values ({publication.SpecieID[i]}, {publicationId});";

        //        }
        //        if (sqlSpecie != "") { Dal.ExecuteNonQueryPub(sqlSpecie); };
        //    }

        //    //Editing to Publication_Sex
        //    sqlDelete = $"DELETE From Publication_Sex where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.sexID != null && publication.sexID.Length != 0)
        //    {
        //        string sqlSex = "";
        //        for (int i = 0; i < publication.sexID.Length; i++)
        //        {
        //            sqlSex += $@"Insert into Publication_Sex (sexID, PublicationID) Values ({publication.sexID[i]}, {publicationId});";


        //        }
        //        if (sqlSex != "") { Dal.ExecuteNonQueryPub(sqlSex); };

        //    }

        //    //Editing to Publication_Strain
        //    sqlDelete = $"DELETE From Publication_Strain where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.StrainID != null && publication.StrainID.Length != 0)
        //    {
        //        string sqlStrain = "";
        //        for (int i = 0; i < publication.StrainID.Length; i++)
        //        {
        //            sqlStrain += $@"Insert into Publication_Strain (StrainID, PublicationID) Values ({publication.StrainID[i]}, {publicationId});";

        //        }
        //        if (sqlStrain != "") { Dal.ExecuteNonQueryPub(sqlStrain); };

        //    }

        //    //Editing to Publication_Disease
        //    sqlDelete = $"DELETE From Publication_Disease where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.DiseaseID != null && publication.DiseaseID.Length != 0)
        //    {
        //        string sqlDiease = "";
        //        for (int i = 0; i < publication.DiseaseID.Length; i++)
        //        {
        //            sqlDiease += $@"Insert into Publication_Disease (DiseaseID, PublicationID) Values ({publication.DiseaseID[i]}, {publicationId});";

        //        }
        //        if (sqlDiease != "") { Dal.ExecuteNonQueryPub(sqlDiease); };

        //    }

        //    //Editing to Publication_Region
        //    sqlDelete = $"DELETE From Publication_Region where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.RegionID != null && publication.RegionID.Length != 0)
        //    {
        //        string sqlRegion = "";
        //        for (int i = 0; i < publication.RegionID.Length; i++)
        //        {
        //            sqlRegion += $@"Insert into Publication_Region (RegionID, PublicationID) Values ({publication.RegionID[i]}, {publicationId});";

        //        }
        //        if (sqlRegion != "") { Dal.ExecuteNonQueryPub(sqlRegion); };

        //    }

        //    //Editing to Publication_SubRegion
        //    sqlDelete = $"DELETE From Publication_SubRegion where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.SubRegionID != null && publication.SubRegionID.Length != 0)
        //    {
        //        string sqlSubRegion = "";
        //        for (int i = 0; i < publication.SubRegionID.Length; i++)
        //        {
        //            sqlSubRegion += $@"Insert into Publication_SubRegion (SubRegionID, PublicationID) Values ({publication.SubRegionID[i]}, {publicationId});";

        //        }
        //        if (sqlSubRegion != "") { Dal.ExecuteNonQueryPub(sqlSubRegion); };

        //    }

        //    //Editing to Publication_CellType
        //    sqlDelete = $"DELETE From Publication_CellType where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.CellTypeID != null && publication.CellTypeID.Length != 0)
        //    {
        //        string sqlCelltype = "";
        //        for (int i = 0; i < publication.CellTypeID.Length; i++)
        //        {
        //            sqlCelltype += $@"Insert into Publication_CellType (CellTypeID, PublicationID) Values ({publication.CellTypeID[i]}, {publicationId});";

        //        }
        //        if (sqlCelltype != "") { Dal.ExecuteNonQueryPub(sqlCelltype); };

        //    }

        //    //Editing to Publication_Method
        //    sqlDelete = $"DELETE From Publication_Method where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.MethodID != null && publication.MethodID.Length != 0)
        //    {
        //        string sqlMethod = "";
        //        for (int i = 0; i < publication.MethodID.Length; i++)
        //        {
        //            sqlMethod += $@"Insert into Publication_Method (MethodID, PublicationID) Values ({publication.MethodID[i]}, {publicationId});";

        //        }
        //        if (sqlMethod != "") { Dal.ExecuteNonQueryPub(sqlMethod); };

        //    }

        //    //Editing to Publication_NeuroTransmitter
        //    sqlDelete = $"DELETE From Publication_NeuroTransmitter where PublicationID = {publicationId}";
        //    Dal.ExecuteNonQueryPub(sqlDelete);

        //    if (publication.TransmitterID != null && publication.TransmitterID.Length != 0)
        //    {
        //        string sqlTransmitter = "";
        //        for (int i = 0; i < publication.TransmitterID.Length; i++)
        //        {
        //            sqlTransmitter += $@"Insert into Publication_NeuroTransmitter (TransmitterID, PublicationID) Values ({publication.TransmitterID[i]}, {publicationId});";

        //        }
        //        if (sqlTransmitter != "") { Dal.ExecuteNonQueryPub(sqlTransmitter); };

        //    }

        //    return true;

        //}

        //// Function definition to search publications in database
        //public List<PubScreenSearch> SearchPublications(PubScreen pubScreen)
        //{
        //    List<PubScreenSearch> lstPubScreen = new List<PubScreenSearch>();

        //    string sql = "Select * From SearchPub Where ";

        //    // Title
        //    if (!string.IsNullOrEmpty(pubScreen.Title))
        //    {
        //        sql += $@"SearchPub.Title like '%{(HelperService.EscapeSql(pubScreen.Title)).Trim()}%' AND ";
        //    }

        //    //Keywords
        //    if (!string.IsNullOrEmpty(pubScreen.Keywords))
        //    {
        //        sql += $@"SearchPub.Keywords like '%{HelperService.EscapeSql(pubScreen.Keywords)}%' AND ";
        //    }

        //    // DOI
        //    if (!string.IsNullOrEmpty(pubScreen.DOI))
        //    {
        //        sql += $@"SearchPub.DOI = '{HelperService.EscapeSql(pubScreen.DOI)}' AND ";
        //    }



        //    // search query for Author
        //    if (pubScreen.AuthourID != null && pubScreen.AuthourID.Length != 0)
        //    {
        //        if (pubScreen.AuthourID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Author like '%'  + (Select CONCAT(Author.FirstName, '-', Author.LastName) From Author Where Author.ID = {pubScreen.AuthourID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.AuthourID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Author like '%'  + (Select CONCAT(Author.FirstName, '-', Author.LastName) From Author Where Author.ID = {pubScreen.AuthourID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }

        //    }

        //    // search query for Year
        //    if (pubScreen.YearFrom != null && pubScreen.YearTo!=null)
        //    {
        //        sql += $@"(SearchPub.Year >= {pubScreen.YearFrom} AND SearchPub.Year <= {pubScreen.YearTo}) AND ";
        //    }

        //    if (pubScreen.YearFrom != null && pubScreen.YearTo == null)
        //    {
        //        sql += $@"(SearchPub.Year >= {pubScreen.YearFrom}) AND ";
        //    }

        //    if(pubScreen.YearTo != null && pubScreen.YearFrom == null)
        //    {
        //        sql += $@"(SearchPub.Year <= {pubScreen.YearTo}) AND ";
        //    }

        //    // search query for PaperType
        //    if (pubScreen.PaperTypeID != null)
        //    {
        //        sql += $@"SearchPub.PaperType like '%'  + (Select PaperType From PaperType Where PaperType.ID = {pubScreen.PaperTypeID}) +  '%' AND ";
        //    }

        //    // search query for Task
        //    if (pubScreen.TaskID != null && pubScreen.TaskID.Length != 0)
        //    {

        //        if (pubScreen.TaskID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Task like '%'  + (Select Task From Task Where Task.ID = {pubScreen.TaskID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.TaskID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Task like '%'  + (Select Task From Task Where Task.ID = {pubScreen.TaskID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }

        //    }

        //    // search query for Species
        //    if (pubScreen.SpecieID != null && pubScreen.SpecieID.Length != 0)
        //    {
        //        if (pubScreen.SpecieID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Species like '%'  + (Select Species From Species Where Species.ID = {pubScreen.SpecieID[0]}) +  '%' AND ";

        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.SpecieID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Species like '%'  + (Select Species From Species Where Species.ID = {pubScreen.SpecieID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for Sex
        //    if (pubScreen.sexID != null && pubScreen.sexID.Length != 0)
        //    {
        //        if (pubScreen.sexID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Sex like '%'  + (Select Sex From Sex Where Sex.ID = {pubScreen.sexID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.sexID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Sex like '%'  + (Select Sex From Sex Where Sex.ID = {pubScreen.sexID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for Strain
        //    if (pubScreen.StrainID != null && pubScreen.StrainID.Length != 0)
        //    {
        //        if (pubScreen.StrainID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Strain like '%'  + (Select Strain From Strain Where Strain.ID = {pubScreen.StrainID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.StrainID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Strain like '%'  + (Select Strain From Strain Where Strain.ID = {pubScreen.StrainID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for Disease
        //    if (pubScreen.DiseaseID != null && pubScreen.DiseaseID.Length != 0)
        //    {
        //        if (pubScreen.DiseaseID.Length == 1)
        //        {
        //            sql += $@"SearchPub.DiseaseModel like '%'  + (Select DiseaseModel From DiseaseModel Where DiseaseModel.ID = {pubScreen.DiseaseID[0]}) +  '%' AND ";

        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.DiseaseID.Length; i++)
        //            {
        //                sql += $@"SearchPub.DiseaseModel like '%'  + (Select DiseaseModel From DiseaseModel Where DiseaseModel.ID = {pubScreen.DiseaseID[i]}) +  '%' OR ";

        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for BrainRegion
        //    if (pubScreen.RegionID != null && pubScreen.RegionID.Length != 0)
        //    {
        //        if (pubScreen.RegionID.Length == 1)
        //        {
        //            sql += $@"SearchPub.BrainRegion like '%'  + (Select BrainRegion From BrainRegion Where BrainRegion.ID = {pubScreen.RegionID[0]}) +  '%' AND ";

        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.RegionID.Length; i++)
        //            {
        //                sql += $@"SearchPub.BrainRegion like '%'  + (Select BrainRegion From BrainRegion Where BrainRegion.ID = {pubScreen.RegionID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for SubRegion
        //    if (pubScreen.SubRegionID != null && pubScreen.SubRegionID.Length != 0)
        //    {
        //        if (pubScreen.SubRegionID.Length == 1)
        //        {
        //            sql += $@"SearchPub.SubRegion like '%'  + (Select SubRegion From SubRegion Where SubRegion.ID = {pubScreen.SubRegionID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.SubRegionID.Length; i++)
        //            {
        //                sql += $@"SearchPub.SubRegion like '%'  + (Select SubRegion From SubRegion Where SubRegion.ID = {pubScreen.SubRegionID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }

        //    }

        //    // search query for CellType
        //    if (pubScreen.CellTypeID != null && pubScreen.CellTypeID.Length != 0)
        //    {
        //        if (pubScreen.CellTypeID.Length == 1)
        //        {
        //            sql += $@"SearchPub.CellType like '%'  + (Select CellType From CellType Where CellType.ID = {pubScreen.CellTypeID[0]}) +  '%' AND ";

        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.CellTypeID.Length; i++)
        //            {
        //                sql += $@"SearchPub.CellType like '%'  + (Select CellType From CellType Where CellType.ID = {pubScreen.CellTypeID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }

        //    // search query for Method
        //    if (pubScreen.MethodID != null && pubScreen.MethodID.Length != 0)
        //    {
        //        if (pubScreen.MethodID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Method like '%'  + (Select Method From Method Where Method.ID = {pubScreen.MethodID[0]}) +  '%' AND ";
        //        }

        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.MethodID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Method like '%'  + (Select Method From Method Where Method.ID = {pubScreen.MethodID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }
        //    }
        //    // search query for Neuro Transmitter
        //    if (pubScreen.TransmitterID != null && pubScreen.TransmitterID.Length != 0)
        //    {
        //        if (pubScreen.TransmitterID.Length == 1)
        //        {
        //            sql += $@"SearchPub.Neurotransmitter like '%'  + (Select Neurotransmitter From Neurotransmitter Where Neurotransmitter.ID = {pubScreen.TransmitterID[0]}) +  '%' AND ";
        //        }
        //        else
        //        {
        //            sql += "(";
        //            for (int i = 0; i < pubScreen.TransmitterID.Length; i++)
        //            {
        //                sql += $@"SearchPub.Neurotransmitter like '%'  + (Select Neurotransmitter From Neurotransmitter Where Neurotransmitter.ID = {pubScreen.TransmitterID[i]}) +  '%' OR ";
        //            }
        //            sql = sql.Substring(0, sql.Length - 3);
        //            sql += ") AND ";
        //        }

        //    }

        //    sql = sql.Substring(0, sql.Length - 4); // to remvoe the last NAD from the query
        //    string sqlMB = "";
        //    List<Experiment> lstExperiment = new List<Experiment>();
        //    using (DataTable dt = Dal.GetDataTablePub(sql))
        //    {

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            sqlMB = $@"Select Experiment.*, Task.Name as TaskName From Experiment
        //                       Inner join Task on Task.ID = Experiment.TaskID
        //                       Where DOI = '{Convert.ToString(dr["DOI"].ToString())}'";

        //            // empty lstExperiment list
        //            lstExperiment.Clear();
        //            using (DataTable dtExp = Dal.GetDataTable(sqlMB))
        //            {
        //                foreach (DataRow drExp in dtExp.Rows)
        //                {

        //                    lstExperiment.Add(new Experiment
        //                    {
        //                        ExpID = Int32.Parse(drExp["ExpID"].ToString()),
        //                        ExpName = Convert.ToString(drExp["ExpName"].ToString()),
        //                        StartExpDate = Convert.ToDateTime(drExp["StartExpDate"].ToString()),
        //                        TaskName = Convert.ToString(drExp["TaskName"].ToString()),
        //                        DOI = Convert.ToString(drExp["DOI"].ToString()),
        //                        Status = Convert.ToBoolean(drExp["Status"]),
        //                        TaskBattery = Convert.ToString(drExp["TaskBattery"].ToString()),

        //                    });
        //                }

        //            }

        //            lstPubScreen.Add(new PubScreenSearch
        //            {
        //                ID = Int32.Parse(dr["ID"].ToString()),
        //                Title = Convert.ToString(dr["Title"].ToString()),
        //                Keywords = Convert.ToString(dr["Keywords"].ToString()),
        //                DOI = Convert.ToString(dr["DOI"].ToString()),
        //                Year = Convert.ToString(dr["Year"].ToString()),
        //                Author = Convert.ToString(dr["Author"].ToString()),
        //                PaperType = Convert.ToString(dr["PaperType"].ToString()),
        //                Task = Convert.ToString(dr["Task"].ToString()),
        //                Species = Convert.ToString(dr["Species"].ToString()),
        //                Sex = Convert.ToString(dr["Sex"].ToString()),
        //                Strain = Convert.ToString(dr["Strain"].ToString()),
        //                DiseaseModel = Convert.ToString(dr["DiseaseModel"].ToString()),
        //                BrainRegion = Convert.ToString(dr["BrainRegion"].ToString()),
        //                SubRegion = Convert.ToString(dr["SubRegion"].ToString()),
        //                CellType = Convert.ToString(dr["CellType"].ToString()),
        //                Method = Convert.ToString(dr["Method"].ToString()),
        //                NeuroTransmitter = Convert.ToString(dr["NeuroTransmitter"].ToString()),
        //                Reference = Convert.ToString(dr["Reference"].ToString()),
        //                Experiment = new List<Experiment>(lstExperiment),


        //            });
        //            //lstExperiment.Clear();
        //        }

        //    }

        //    // search MouseBytes database to see if the dataset exists********************************************


        //    return lstPubScreen;


        //}

        //// Function definition to get all year's values in database
        //public List<PubScreenSearch> GetAllYears()
        //{
        //    List<PubScreenSearch> YearList = new List<PubScreenSearch>();
        //    using (DataTable dt = Dal.GetDataTablePub($@"Select distinct Year from Publication"))
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            YearList.Add(new PubScreenSearch
        //            {

        //                Year = Convert.ToString(dr["Year"].ToString()),
        //            });

        //        }
        //    }

        //    return YearList;
        //}

        //public PubScreen GetPaperInfoByID(int id)
        //{
        //    var pubScreen = new PubScreen();

        //    string sql = $"Select AuthorID From Publication_Author Where PublicationID ={id}";
        //    pubScreen.AuthourID = FillPubScreenItemArray(sql, "AuthorID");

        //    sql = $"Select CelltypeID From Publication_CellType Where PublicationID ={id}";
        //    pubScreen.CellTypeID = FillPubScreenItemArray(sql, "CelltypeID");

        //    sql = $"Select DiseaseID From Publication_Disease Where PublicationID ={id}";
        //    pubScreen.DiseaseID = FillPubScreenItemArray(sql, "DiseaseID");

        //    sql = $"Select MethodID From Publication_Method Where PublicationID ={id}";
        //    pubScreen.MethodID = FillPubScreenItemArray(sql, "MethodID");

        //    sql = $"Select TransmitterID From Publication_NeuroTransmitter Where PublicationID ={id}";
        //    pubScreen.TransmitterID = FillPubScreenItemArray(sql, "TransmitterID");

        //    sql = $"Select RegionID From Publication_Region Where PublicationID ={id}";
        //    pubScreen.RegionID = FillPubScreenItemArray(sql, "RegionID");

        //    sql = $"Select SexID From Publication_Sex Where PublicationID ={id}";
        //    pubScreen.sexID = FillPubScreenItemArray(sql, "SexID");

        //    sql = $"Select SpecieID From Publication_Specie Where PublicationID ={id}";
        //    pubScreen.SpecieID = FillPubScreenItemArray(sql, "SpecieID");

        //    sql = $"Select StrainID From Publication_Strain Where PublicationID ={id}";
        //    pubScreen.StrainID = FillPubScreenItemArray(sql, "StrainID");

        //    sql = $"Select SubRegionID From Publication_SubRegion Where PublicationID ={id}";
        //    pubScreen.SubRegionID = FillPubScreenItemArray(sql, "SubRegionID");

        //    sql = $"Select TaskID From Publication_Task Where PublicationID ={id}";
        //    pubScreen.TaskID = FillPubScreenItemArray(sql, "TaskID");

        //    //sql = $"Select PaperTypeID From Publication_PaperType Where PublicationID ={id}";
        //    sql = $"Select top 1 pt.PaperType From Publication_PaperType ppt inner join PaperType pt on ppt.PaperTypeID = pt.ID where ppt.PublicationID = {id}";
        //    var paperTypeVal = Dal.ExecScalarPub(sql);
        //    pubScreen.PaperType = paperTypeVal == null ? "" : paperTypeVal.ToString();

        //    sql = $"Select * From Publication Where ID ={id}";
        //    using (DataTable dt = Dal.GetDataTablePub(sql))
        //    {
        //        pubScreen.DOI = dt.Rows[0]["DOI"].ToString();
        //        pubScreen.Keywords = dt.Rows[0]["Keywords"].ToString();
        //        pubScreen.Title = dt.Rows[0]["Title"].ToString();
        //        pubScreen.Abstract = dt.Rows[0]["Abstract"].ToString();
        //        pubScreen.Year = dt.Rows[0]["Year"].ToString();
        //        pubScreen.Reference = dt.Rows[0]["Reference"].ToString();
        //        pubScreen.Source = dt.Rows[0]["Source"].ToString();
        //    }

        //    return pubScreen;
        //}

        private int?[] FillCogbytesItemArray(string sql, string fieldName)
        {

            var retVal = new int?[0];
            using (DataTable dt = Dal.GetDataTableCog(sql))
            {
                retVal = new int?[dt.Rows.Count];
                var i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    retVal[i] = Int32.Parse(dr[fieldName].ToString());
                    i++;
                }
            }

            return retVal;
        }

        //public void ProcessOther(string inputOther, string tableOther, string fieldOther, string tblPublication,
        //                        string tblPublicationField, int PublicationID, string Username)
        //{
        //    if (!String.IsNullOrEmpty(inputOther))
        //    {
        //        List<string> ItemList = inputOther.Split(';').Select(p => p.Trim()).ToList();
        //        foreach (string item in ItemList)
        //        {
        //            string sqlOther = $@"Select ID From {tableOther} Where ltrim(rtrim({fieldOther})) = '{HelperService.NullToString(HelperService.EscapeSql(item)).Trim()}';";
        //            var IDOther = Dal.ExecScalarPub(sqlOther);

        //            if (IDOther == null)
        //            {
        //                string sqlOther2 = $@"Insert Into {tableOther} ({fieldOther}, Username) Values
        //                                    ('{HelperService.NullToString(HelperService.EscapeSql(item)).Trim()}', '{Username}'); SELECT @@IDENTITY AS 'Identity';";
        //                int IDOther2 = Int32.Parse((Dal.ExecScalarPub(sqlOther2).ToString()));

        //                string sqlOther3 = $@"Insert into {tblPublication} ({tblPublicationField}, PublicationID) Values ({IDOther2}, {PublicationID}); ";
        //                Dal.ExecuteNonQueryPub(sqlOther3);
        //            }

        //        }

        //    }
        //}


    }
}
