phonology-assistant
===================
Phonology Assistant is a discovery tool. Provided with a corpus of phonetic data, it automatically charts the sounds and through its searching capabilities, helps a user discover and test the rules of sound in a language.

[web site](https://software.sil.org/phonologyassistant/)

License
-------
Phonology Assistant is licensed under MIT. See [LICENSE.md](https://github.com/sillsdev/phonology-assistant/blob/master/LICENSE)


[Binaries](http://build.palaso.org/project.html?projectId=project17&tab=projectOverview&guest=1)

[Source Code](https://github.com/sillsdev/phonology-assistant)

[Issues](https://jira.sil.org/browse/PA)

Development
-----------
After cloning the repo, install the dependencies with:

`bash ./buildupdate.sh`

Load Pa-Windows.sln and set the Configuration to Debug-Windows and the Platform to x86. Then build.

The Unit tests can be run from the PaTests project and work with ReSharper.
