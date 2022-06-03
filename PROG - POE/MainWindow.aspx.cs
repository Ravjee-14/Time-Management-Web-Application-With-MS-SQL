using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.IO;

namespace PROG___POE
{
    public partial class MainWindow : System.Web.UI.Page
    {

        //classes are declared into main window
        Module myModule = new Module();
        Working myWorking = new Working();
        Semester mySemester = new Semester();
        WorkingDay myWorkingDay = new WorkingDay();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                //class values are populated by textboxes
                myModule.ID = txtStudent_ID.Text;
                myModule.Code = txtModuleCode.Text;
                myModule.Name = txtModuleName.Text;
                myModule.Credits = double.Parse(txtCredits.Text);
                myModule.HoursWeekly = double.Parse(txtClassHours.Text);

                con.Open();

                //sql command used to input moule information
                SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[Modules]
                                                            ([Student_ID]
                                                           ,[Module_Code]
                                                           ,[Module_Name]
                                                        ,[Module_Credits]
                                                     ,[Module_ClassHours])
                                   VALUES('" + txtStudent_ID.Text + "', '" + myModule.Code + "','" + myModule.Name + "'," +
                                                myModule.Credits + "," + myModule.HoursWeekly + ");", con);

                SqlDataReader dr = cmd.ExecuteReader();
                con.Close();
                Response.Write("<script>alert('Module Information Recorded successfully');</script>");
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Missing or Incorrect Values');</script>");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //class values are populated using textbox values
                mySemester.DateSemester = dpSemester.Text;
                mySemester.NumWeeks = double.Parse(txtNumWeeks.Text);

                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                con.Open();

                //SQL command used to input semester info like start date and number of weeks
                SqlCommand cmd3 = new SqlCommand(@"INSERT INTO [dbo].[Semester]
                                                                  ([Student_ID]
                                                           ,[Semester_NumWeeks]
                                                          ,[Semester_StartDate])
                                        VALUES('" + txtStudent_ID.Text + "', " + mySemester.NumWeeks + ", '" +
                                                    mySemester.DateSemester + "');", con);
                SqlDataReader dr3 = cmd3.ExecuteReader();
                con.Close();
                Response.Write("<script>alert('Semester Information Correctly Recorded');</script>");
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Missing or Incorrect data');</script>");
            }
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                //class values are populated using textbox values
                myWorking.WorkingModuleCode = txtWorkingModuleCode.Text;
                myWorking.HoursWorked = double.Parse(txtHoursSpent.Text);
                myWorking.DateWorking = dpWorking.Text;

                con.Open();

                //sql command used to insert working hours
                SqlCommand cmd2 = new SqlCommand(@"INSERT INTO [dbo].[Working]
                                                          ([Working_Hours]
                                                            ,[Module_Code]
                                                           ,[Working_Date]
                                                             ,[Student_ID])
                                VALUES(" + myWorking.HoursWorked + ", '" + txtWorkingModuleCode.Text + "', '" +
                                               myWorking.DateWorking + "', '" + txtStudent_ID.Text + "');", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                con.Close();

                Response.Write("<script>alert('Hours Worked Recorded successfully');</script>");
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Missing or Incorrect Values');</script>");
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                myWorkingDay.WorkingCode = txtWorkingDay_Code.Text;
                myWorkingDay.DateWorking = dpWorkingDay.Text;

                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                con.Open();

                //sql command used to insert working hours
                SqlCommand cmd4 = new SqlCommand(@"INSERT INTO [dbo].[Working_Day]
                                                          ([Student_ID]
                                                          ,[Module_Code]
                                                          ,[WorkingDay_Date])
                                                    VALUES('" + txtStudent_ID.Text + "', '" + myWorkingDay.WorkingCode + "', '" + myWorkingDay.DateWorking + "');", con);
                SqlDataReader dr4 = cmd4.ExecuteReader();
                con.Close();
                Response.Write("<script>alert('Informaton Successfuly recorded');</script>");

            }
            catch(Exception)
            {
                Response.Write("<script>alert('Missing or incorrect information');</script>");
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                con.Open();

                //SQL command used to display values as well as do calculations
                SqlCommand cmd = new SqlCommand(@"SELECT Login.Student_ID, Modules.Module_Name, Modules.Module_Code, Modules.Module_Credits, Semester.Semester_StartDate, 
                                             Semester.Semester_NumWeeks, Working.Working_Date, Working.Working_Hours, 
			                                (Modules.Module_Credits * 10 / Semester.Semester_NumWeeks - Modules.Module_ClassHours) AS Self_Study_Hours_Per_Week
	                                                FROM Login
	                                                INNER JOIN Modules ON Modules.Student_ID = Login.Student_ID
	                                                INNER JOIN Semester ON Semester.Student_ID = Login.Student_ID
	                                                INNER JOIN Working ON Working.Module_Code = Modules.Module_Code
		                                                WHERE Login.Student_ID = '" + txtStudent_ID.Text + "';", con);

                SqlDataReader read = cmd.ExecuteReader();
                GridView1.DataSource = read;
                GridView1.DataBind();

                con.Close();
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Please Enter Student ID" + "\n\nRemember Student ID Is Same As Username');</script>");
            }
        }

        protected void btnViewReserved_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                con.Open();

                //SQL command used to display values as well as do calculations
                SqlCommand cmd = new SqlCommand(@"SELECT Working_Day.Student_ID, Working_Day.Module_Code, Modules.Module_Name, [WorkingDay_Date]
                                                        FROM [dbo].[Working_Day]
                                                        INNER JOIN Modules ON Modules.Module_Code = Working_Day.Module_Code
                                                        WHERE Working_Day.Student_ID = '" + txtStudent_ID.Text + "';", con);

                SqlDataReader reader = cmd.ExecuteReader();
                GridView2.DataSource = reader;
                GridView2.DataBind();

                con.Close();
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Please Enter Student ID" + "\n\nRemember Student ID Is Same As Username');</script>");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try statement to tell user if a value has not been inputted
            try
            {
                // try statement to tell user if the file cannot be written
                try
                {

                    SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                    con.Open();

                    //SQL command used to display values as well as do calculations
                    SqlCommand cmd4 = new SqlCommand(@"SELECT Login.Student_ID, Modules.Module_Name, Modules.Module_Code, Modules.Module_Credits, Semester.Semester_StartDate, 
                                             Semester.Semester_NumWeeks, Working.Working_Date, Working.Working_Hours, 
			                                (Modules.Module_Credits * 10 / Semester.Semester_NumWeeks - Modules.Module_ClassHours) AS Self_Study_Hours_Per_Week
	                                                FROM Login
	                                                INNER JOIN Modules ON Modules.Student_ID = Login.Student_ID
	                                                INNER JOIN Semester ON Semester.Student_ID = Login.Student_ID
	                                                INNER JOIN Working ON Working.Module_Code = Modules.Module_Code
		                                                WHERE Login.Student_ID = '" + txtStudent_ID.Text + "';", con);

                    SqlDataReader dt4 = cmd4.ExecuteReader();

                    //destination to save file
                    //please enter save destination before starting the program
                    StreamWriter tw = new StreamWriter(@"C:\Users\");

                    //what will be written in the file
                    tw.Write(dt4);
                    tw.Close();

                    con.Close();

                    Response.Write("<script>alert('File has successfully been written');</script>");
                }
                //catch expception to display what went wrong
                catch (Exception)
                {
                    Response.Write("<script>alert('Unable to Save to file" +
                                    "\nReason - Access Denied from System Firewall');</script>");
                }
            }
            //catch statment to tell user what is wrong
            catch (Exception)
            {
                Response.Write("<script>alert('Please enter a value in all fields" +
                                "\nNo Special characters are allowed');</script>");
            }
        }

        private void MultiThread()
        {
            Thread.Sleep(2000);
        }
    }
}