ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=${PWD}
endif
ifndef bindst
bindst=$(binsrc)/Output/Release
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=3.6.5.0
endif
ifndef Platform
Platform=Any CPU
endif
ifndef MONO_PREFIX
MONO_PREFIX=/opt/mono4-sil
endif
ifndef GDK_SHARP
GDK_SHARP=$(MONO_PREFIX)/lib/mono/gtk-sharp-3.0
endif
ifndef LD_LIBRARY_PATH
LD_LIBRARY_PATH=$(MONO_PREFIX)/lib
endif
ifndef PKG_CONFIG_PATH
PKG_CONFIG_PATH=$(MONO_PREFIX)/lib/pkgconfig
endif
ifndef MONO_GAC_PREFIX
MONO_GAC_PREFIX=$(MONO_PREFIX)
endif
ifndef MONO_MWF_SCALING
MONO_MWF_SCALING=disable
endif
PATH := $(MONO_PREFIX)/bin:$(PATH)

build:
	xbuild /t:ReBuild /p:BUILD_NUMBER=$(BUILD_NUMBER)\;Configuration=Release-Windows\;Platform='$(Platform)'\;OS=Linux\;SolutionDir=$(binsrc)/ Pa-Ubuntu.sln

debug:
	xbuild /t:ReBuild /p:BUILD_NUMBER=$(BUILD_NUMBER)\;Configuration=Debug-Windows\;Platform='$(Platform)'\;OS=Linux\;SolutionDir=$(binsrc)/ Pa-Ubuntu.sln

tests:
	nunit-console -exclude=SkipOnTeamCity\;LongTest -labels -nodots Output/Debug/Pa-Tests.dll

install:
	mkdir -p $(DESTDIR)$(prefix)/lib/phonology-assistant
	cp -r $(bindst)/. $(DESTDIR)$(prefix)/lib/phonology-assistant
	cp -r $(binsrc)/DistFiles/* $(DESTDIR)$(prefix)/lib/phonology-assistant/.
	mkdir -p $(DESTDIR)$(prefix)/bin
	cp src/Pa.sh $(DESTDIR)$(prefix)/bin/Pa
	cp src/DisablePASplashScreen.sh $(DESTDIR)$(prefix)/bin/DisablePASplashScreen
	mkdir -p $(DESTDIR)$(prefix)/share/python-support
	chmod 777 $(DESTDIR)$(prefix)/share/python-support
	mkdir -p $(DESTDIR)$(prefix)/share/doc/phonology-assistant
	chmod 777 $(DESTDIR)$(prefix)/share/doc/phonology-assistant
	mkdir -p $(DESTDIR)$(prefix)/share/phonology-assistant
	chmod 777 $(DESTDIR)$(prefix)/share/phonology-assistant
	mkdir -p $(DESTDIR)$(prefix)/share/applications
	chmod 777 $(DESTDIR)$(prefix)/share/applications
	cp debian/*.desktop $(DESTDIR)$(prefix)/share/applications
	mkdir -p $(DESTDIR)$(prefix)/share/pixmaps
	chmod 777 $(DESTDIR)$(prefix)/share/pixmaps
	cp debian/*.png $(DESTDIR)$(prefix)/share/pixmaps
	cp debian/*.xpm $(DESTDIR)$(prefix)/share/pixmaps
	mkdir -p $(DESTDIR)$(prefix)/share/man
	chmod 777 $(DESTDIR)$(prefix)/share/man

binary:
	exit 0

clean:
	rm -rf output/*

uninstall:
	-sudo apt-get -y remove phonology-assistant
	sudo rm -rf $(DESTDIR)$(prefix)/lib/phonology-assistant
	sudo rm $(DESTDIR)$(prefix)/bin/Pa
	sudo rm $(DESTDIR)$(prefix)/bin/DisablePASplashScreen
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/phonology-assistant
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/phonology-assistant*
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/phonology-assistant*
	sudo rm -rf $(DESTDIR)$(prefix)/share/phonology-assistant
	-xdg-desktop-menu uninstall /etc/phonology-assistant/sil-Pa.desktop
	sudo rm -rf $(DESTDIR)/etc/phonology-assistant

clean-build:
	rm -rf debian/phonology-assistant bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f *.dsc pathway_*.tar.gz pathway_*.build pathway_*.diff.gz
	rm -f *.changes phonology-assistant*.deb


