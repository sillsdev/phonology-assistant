rem -----------------------------------------------------------------------
rem When this command is run, then the assumed root directory for the
rem build is the directory that is current in the OS when this batch
rem file is run (unless this batch file is called using a short cut
rem whose working directory is set). If that directory is not the correct
rem root directory for the build process, then you may add another
rem argument to this build command as in the following example:
rem
rem dobuilder /p:PaReleaseBuild.xml /l:"Build Logs\PaBuild.log" /rootbuilddir:"c:\Phonology Assistant"
rem
rem -----------------------------------------------------------------------
dobuilder /p:PaReleaseBuild.xml /l:"Build Logs\PaBuild.log"