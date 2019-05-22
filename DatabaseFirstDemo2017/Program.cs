using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFirstDemo2017
{
    class Program
    {
        class Operations
        {
            //Updated student by Id
            public void UpadteStudent(int id)
            {
                using (UniContextEntities context = new UniContextEntities())
                {
                    // var student = (from d in context.Students where d.StudentID == id select d).Single();
                    var student = context.Students.Where(s => s.StudentID == id).Single();
                    student.LastName = "XYZ";
                    //context.Students.(student);
                    context.SaveChanges();
                    
                    //  Display Updated value of the student
                    //    var studentRead = (from d in context.Students where d.StudentID == id select d);
                     //   Student studentRead = context.Students.Where(s => s.StudentID == 1).Single();
                    var studentRead = from d in context.Students where d.StudentID == 1 select d;
                    Console.WriteLine("All student List as beloew:-");
                    foreach (var studentDetail in studentRead)
                    {
                        Console.WriteLine(studentDetail.LastName + " " + studentDetail.FirstName + " " + "Enrollment Date =" + studentDetail.EnrollmentDate);
                    }
                }

                // Adding new Student
                
            }

            public void AddStudent()
            {
                using (UniContextEntities context = new UniContextEntities())
                {
                    var student = new Student()
                    {
                        LastName = "Smith",
                        FirstName = "Alex",
                        EnrollmentDate = DateTime.Parse("12/02/2009")

                      

                    };
                    context.Students.Add(student);
                    //foreach(var enroll in student.Enrollments)
                    //{
                    //    context.Enrollments.Add(enroll );
                    //}
                    
                    Console.WriteLine(context.Entry(student).State);
                    context.SaveChanges();
                    Console.WriteLine("Student added");

                }

            }
            // Using View 

            public void UseViewExample()
            {
                using (var context = new UniContextEntities())
                {
             //    var sudent=from d in context.
                    var studentList = context.TotalStudents.ToList();
                    foreach (var stu in studentList)
                    {
                        Console.WriteLine("First name=" + stu.FirstName+ " " +"lastname="+stu.LastName );
                    }
                }
            }

            // Using Stored procedure

            public void StoredProcedureExample()
            {
                using (var context = new UniContextEntities())
                {
                    Console.WriteLine("Enter student Id=");
                    
                    var studentID = int.Parse (Console.ReadLine()) ;
                    Console.WriteLine("Student details using Stored Procedure:--");
                    Console.WriteLine("===============================================");
                    var studentDetails = context.GetSudentGrade(studentID);
                    foreach (var std in studentDetails )
                    {
                        
                       
                        Console.WriteLine("Enrollment ID ="+ std.EnrollmentID +"  "+ "Course ID=" +std.CourseID +" " + "Grade =" +std.Grade + "  "+"student Firstname ="+std.FirstName +" "+"student Lastname=" +std.LastName );
                    }

                    //Using Table Valued Functions

                    Console.WriteLine("Enter Course Id=");

                    var courseID = int.Parse(Console.ReadLine());
                    Console.WriteLine("Student details using TVF:--");
                    Console.WriteLine("===============================================");
                    var studentDetails2 = context.GetStudentGradesForCourse(courseID);
                    foreach (var std in studentDetails2)
                    {


                        Console.WriteLine("Enrollment ID =" + std.EnrollmentID + "  " + "Course ID=" + std.CourseID + " " + "Grade =" + std.Grade );
                    }
                }
            }

            // Transaction example

            public void TransactionExample()
            {
                using (var context = new UniContextEntities())
                {
                    //context.Database.Log=Console.WriteLine ;
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            Course cr = new Course()
                            {
                                Title="Geography",
                                Credits=58

                            };
                            context.Courses.Add(cr);


                            // Reading course and displaying values of Course Id ==2 
                            var queryCourseRead = context.Courses.Where(c => c.CourseID == 2);
                            foreach (var Course in queryCourseRead)
                            {
                                Console.WriteLine( "Course details for Course ID=2 is async follow :-");
                                Console.WriteLine("====================================================");
                                Console.WriteLine("Course title =" + Course.Title + "  credit =" + Course.Credits);

                            }

                            //updating Course Title for Couse ID==3

                            context.Database.ExecuteSqlCommand(@"update Course set title='Calculus' where CourseID=3");
                             queryCourseRead = context.Courses.Where(c => c.CourseID == 3);
                            foreach (var Course in queryCourseRead)
                            {
                                Console.WriteLine("Course details for Course ID=2 is async follow :-");
                                Console.WriteLine("====================================================");
                                Console.WriteLine("Course title =" + Course.Title + "  credit =" + Course.Credits);

                            }

                            context.SaveChanges();
                            dbContextTransaction.Commit();

                        }
                        catch(Exception )
                        {
                            dbContextTransaction.Rollback();
                        }


                    }
                }
            }

            public void DefferentLoadingMethods()
            {
                //eger Load all students and their enrollment
                using (var context = new UniContextEntities())
                {
                    var students = context.Students
              .Include("Enrollments").ToList();
                    context.ChangeTracker.Entries().Count();
                    
                    foreach (var student in students)
                    {
                        string name = student.FirstName  + " " + student.LastName;
                        Console.WriteLine("ID: {0}, Name: {1}", student.StudentID, name);

                        foreach (var enrollment in student.Enrollments)
                        {
                            Console.WriteLine("Enrollment ID: {0}, Course ID: {1}",
                               enrollment.EnrollmentID, enrollment.CourseID);
                        }
                    }
                 //      // change tracking 
                 //Console.WriteLine( "Total Change tracker count=" + context.ChangeTracker.Entries().Count());
                 //   var entries = context.ChangeTracker.Entries();
                 //   foreach (var entry in entries)
                 //   {
                 //       Console.WriteLine(" Entity name =" +entry.Entity.GetType().Name);
                 //       Console.WriteLine(" Entity State =" + entry.State );
                 //   }

                }
            }


        }


        static void Main(string[] args)
        {
            Operations op = new Operations();
            op.UpadteStudent(1);
            op.AddStudent();
            op.StoredProcedureExample();
            op.DefferentLoadingMethods();
         //   op.UseViewExample();
            //op.TransactionExample();

            //using (var context = new UniContextEntities())
            //{
            //    var query = (from d in context.Students orderby d.LastName  select d);


            //    Console.WriteLine("All student List as beloew:-");
            //    foreach (var student in query)
            //    {
            //        Console.WriteLine(student.LastName + " " + student.FirstName + " " + "Enrollment Date =" + student.EnrollmentDate);
            //    }

            //}
        }
    }
}
