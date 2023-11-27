using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CadExtension
{
    public class DbLoadUtil
    {

        public string LoadLines()
        {
            string result = "";
            SqlConnection conn = DbUtil.GetConnection();
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                using(Transaction trans= doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] tv= new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "MTEXT"), 0);
                    SelectionFilter filter = new SelectionFilter(tv);

                    PromptSelectionResult ssPromt = ed.SelectAll(filter);
                    // Seçili bir nesne var mı kontrol edelim.
                    if(ssPromt.Status==PromptStatus.OK)
                    {
                        double startPtx=0.0,startPty=0.0,endPtx=0.0,endPty=0.0;
                        double len = 0.0;
                        Line line = new Line();
                        SelectionSet ss = ssPromt.Value;
                        String sql = @"INSERT INTO dbo.Lines(StartPtx,StartPty,EndPtx,EndPty,Length,Created)
VALUES(@StartPtx,@StartPty,@EndPtx,@EndPty,@Length,@Created)";
                        conn.Open();
                         foreach(SelectedObject sObj in ss)
                        {
                            line = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as Line;
                            startPtx = line.StartPoint.X;
                            startPty = line.StartPoint.Y;
                            endPtx = line.EndPoint.X;
                            endPty = line.EndPoint.Y;   
                            len = line.Length;
                            
                            SqlCommand cmd=new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@StartPtx", startPtx);
                            cmd.Parameters.AddWithValue("@StartPty", startPty);
                            cmd.Parameters.AddWithValue("@EndPtx", endPtx);
                            cmd.Parameters.AddWithValue("@EndPty", endPty);
                            cmd.Parameters.AddWithValue("@Length",len );
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("No object selected.");
                    }
                    result = "Done. Completed Succesfully.";
                }
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result; }
    }
}
