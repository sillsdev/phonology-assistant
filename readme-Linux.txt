README - PHONOLOGY ASSISTANT FOR LINUX


GETTING STARTED

The following steps were tested May-2013 on Ubuntu 13.04 Desktop using 13.04's default Mono (version 
2.10.8.1).

  * install tools:

  sudo apt-get install mercurial mono-devel mono-gmcs monodevelop

  * clone repository and build Pa.exe:

  cd ~/Documents/src/ # or wherever you keep your code
  hg clone http://hg.palaso.org/phonology-assistant
  cd phonology-assistant/
  hg update Linux
  mdtool build --configuration:Debug-Linux # or build in MonoDevelop GUI with configuration 'Debug-Linux'

  * simulate an installation on a build system:

  cd ~/Documents/src/phonology-assistant/src/Pa/bin/Debug-Linux/
  ln -s ../../../../DistFiles/Configuration Configuration
  ln -s ../../../../DistFiles DistFiles

  * make a working copy of the distribution files (this mimics what the Windows installer does and what 
the Linux installer will need to do):

  cd ~/Documents/src/phonology-assistant/
  mkdir src/Pa/bin/Debug-Linux/Training/
  cd DistFiles/Training\ Projects/
  cp ../TrainingProjectsSetup.xml ../../src/Pa/bin/Debug-Linux/Training/TrainingProjectsSetup.xml
  zip -r                          ../../src/Pa/bin/Debug-Linux/Training/PaTrainingProjects.zip Sekpele*

  * run:

  ~/Documents/src/phonology-assistant/src/Pa/bin/Debug-Linux/Pa.exe

  * in Phonology Assistant, select File > Sekpele 2 (if 'Sekpele 2' is not in the File menu, then some 
of the above steps were probably not done correctly).  Have fun!


ICU.NET.DLL

This is optional in most cases.

  * install icu.net.dll:

  sudo apt-add-repository "deb http://ppa.palaso.org/ubuntu oneiric main"
  wget -q http://ppa.palaso.org/ubuntu/palaso.asc -O- |sudo apt-key add -
  sudo aptitude update
  sudo aptitude install libicu-cil

  * build icu.net.dll from source:

  cd ~/Documents/src/
  hg clone http://hg.palaso.org/icu-dotnet
  cd icu-dotnet/
  xbuild "/target:Clean;Compile" /p:Configuration=DebugMono /p:RootDir=`pwd` bld/build.mono.proj
  sudo xbuild "/target:Install" /p:Configuration=DebugMono /p:RootDir=`pwd` bld/build.mono.proj
  sudo chmod 775  /usr/lib/cli/icu-cil-0.1.2/
  sudo chmod 775  /usr/lib/cli/icu-cil-0.1.2/icu.net.dll
  sudo chmod 664  /usr/lib/cli/icu-cil-0.1.2/icu.net.dll.config


PALASO.DLL

This is optional in most cases.

  * clone repository and build Palaso.dll:

  cd ~/Documents/src/
  hg clone http://hg.palaso.org/palaso
  cd palaso/
  hg update DefaultMono
  export CONFIG=ReleaseMono # or DebugMono for testing
  xbuild "/target:Clean;Compile" /p:Configuration=$CONFIG /p:RootDir=`pwd` build/build.mono.proj
  cp output/$CONFIG/Palaso.dll ../phonology-assistant/DistFiles/Linux/


L10NSHARP

This is optional in most cases.

L10NSharp was formerly known as Localization Manager and stored at 
https://bitbucket.org/domferrari/localizationmanager (this is now deprecated; prior to that it was 
https://svn.sil.org/langsw/LocalizationManager).  The steps below do not currently work as L10NSharp 
does not yet build under Mono.  Also, Phonology Assistant expects LocalizationManager.dll rather than 
L10NSharp.dll.

  * clone repository and build LocalizationManager.dll:

  cd ~/Documents/src/
  hg clone https://bitbucket.org/hatton/l10nsharp/
  cd l10nsharp/
  mdtool build --configuration:Release # or build in MonoDevelop GUI
  cp Output/Release/LocalizationManager.dll ../phonology-assistant/DistFiles/Linux/


NOTES

  * the latest Calgary Mono is likely to eliminate a number of bugs in Phonology Assistant
  * when using the Calgary Mono, add symlinks to make other Mono applications (Xbuild, MonoDevelop) work:

  sudo ln -s /usr/bin/mono /usr/local/sbin/mono
  sudo ln -s /usr/bin/xbuild /usr/local/sbin/xbuild

  * to delete all PA projects and settings:

  rm -R ~/Documents/Phonology\ Assistant/ ~/.mono/registry/CurrentUser/software/sil/phonology\ assistant/ ~/.mono/mwf_config/

