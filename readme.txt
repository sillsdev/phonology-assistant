Build Instructions for Phonology Assistant

1.  Start Visual Studio 2005. I'm using "Microsoft Visual C# 2005 Express Edition."

2.  Open the Source\PaExe\PaExe.csproj project into a blank solution. Build and save the resulting solution in the location of your choice. (Personally, I use the pa\Source directory.) You'll get an error telling you that the "referenced component PaDll.dll couldn't be found." This is expected.

3.  Right-click the solution and add the project in Source\PaDll. Build the solution again. Observe the additional missing referenced components and add the relevant existing projects.

4. Verify PaExe as startup project (will show up as bold in the Solution Explorer) by right clicking on PaExe.

5. Build the solution (projects will build as necessary as part of this step)

6. Download and install Nunit from http://www.nunit.org/download.html. <I'll add more details here around running the unit tests when I get that far.>

7. Congratulate yourself to a job well done.
