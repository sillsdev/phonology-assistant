@rem --------------------------------------------------------------------------------------------
@rem    When these commands are run, then the assumed root directory for the
@rem    builds is the directory that is current in the OS when this batch
@rem    file is run (unless this batch file is called using a short cut
@rem    whose working directory is set). If that directory is not the correct
@rem    root directory for the build processes, then you may add another
@rem    argument to these build commands as in the following examples:
@rem
@rem    dobuilder /p:PaReleaseBuild.xml /l:"Build Logs\PaBuild.log" /rootbuilddir:"c:\Phonology Assistant"
@rem    dobuilder /p:PaInstallerBuild.xml /l:"Build Logs\PaInstallerBuild.log" /rootbuilddir:"c:\Phonology Assistant"
@rem
@rem ---------------------------------------------------------------------------------------------
dobuilder /p:PaReleaseBuild.xml /l:"Build Logs\PaBuild.log"
dobuilder /p:PaInstallerBuild.xml /l:"Build Logs\PaInstallerBuild.log"
dobuilder /p:PaWebArchiveBuild.xml /l:"Build Logs\PaWebArchiveBuild.log"